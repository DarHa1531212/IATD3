using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace IATD3
{
    public class cAgent
    {
        #region Attributes

        // Effectors
        readonly private cEffectorMove effectorMove;
        readonly private cEffectorUsePortal effectorUsePortal;
        readonly private cEffectorThrowRock effectorThrowRock;

        // Sensors
        readonly private cSensorLight sensorLight;
        readonly private cSensorWind sensorWind;
        readonly private cSensorOdour sensorOdour;
        readonly private cSensorNeighbours sensorNeighbours;
        readonly private cSensorAbyss sensorAbyss;
        readonly private cSensorMonster sensorMonster;

        // Relative position
        // The agent only knowns where he began and what moves he made
        private int relativePositionX;
        private int relativePositionY;

        // Gathered agent data
        readonly private List<cInference> inferences;
        readonly private List<Tuple<int, int>> knownCells;
        readonly private List<Tuple<int, int>> scopeCells;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="cAgent"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public cAgent(cEnvironment environment)
        {
            // Instantiate effectors
            effectorMove = new cEffectorMove(environment);
            effectorThrowRock = new cEffectorThrowRock(environment);
            effectorUsePortal = new cEffectorUsePortal(environment);

            // Instantiate sensors
            sensorLight = new cSensorLight(environment);
            sensorOdour = new cSensorOdour(environment);
            sensorWind = new cSensorWind(environment);
            sensorNeighbours = new cSensorNeighbours(environment);
            sensorAbyss = new cSensorAbyss(environment);
            sensorMonster = new cSensorMonster(environment);

            // Relative position
            relativePositionX = 0;
            relativePositionY = 0;

            // Initialize data
            inferences = new List<cInference>();
            knownCells = new List<Tuple<int, int>>();
            scopeCells = new List<Tuple<int, int>>();

            // Initialize rules
            loadRulesFile();
            FactTableManager.CreateFactFile();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Updates data at the position of death.
        /// </summary>
        /// <param name="positionX">The position x.</param>
        /// <param name="positionY">The position y.</param>
        /// <param name="isDeadlyCell">The value for isDeadlyCell data.</param>
        public void UpdateCellDeath(int positionX, int positionY, String isDeadlyCell)
        {
            FactTableManager.AddOrChangeAttribute(positionX, positionY, isDeadlyCell, "True");
        }

        /// <summary>
        /// Make the agent action.
        /// </summary>
        /// <returns></returns>
        public int Act()
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>
            {
                { "hasPortal", "True" }
            };

            // Compare the created attributes with those of the fact table i.e. are we on a portal? 
            if (FactTableManager.IsFactInTable(string.Empty, relativePositionX, relativePositionY, attributes))
            {
                return UsePortal();
            }

            // Finding and executing the safest move
            Tuple<int, int> position = FindSafestPositionToGoTo();
            if (position == null)
            {
                Tuple<int, int> stoneThrowPosition = FindWhereToThrowStone();
                if (stoneThrowPosition != null)
                {
                    int throwStoneCost = ThrowStoneTo(stoneThrowPosition.Item1, stoneThrowPosition.Item2);
                    return (throwStoneCost);
                }
                else
                {
                    // If no position is truely safe and if there is no place to throw a rock,
                    // we'll move to a random position and hope for the best. This is a worst case scenario.
                    position = GetRandomPositionInScope();
                }

            }
            return MoveToPosition(position.Item1, position.Item2);
        }

        /// <summary>
        /// Uses the sensors.
        /// </summary>
        public void UseSensors()
        {
            UpdateFacts();

            bool isWindy = sensorWind.Sense();
            bool isSmelly = sensorOdour.Sense();
            bool isBright = sensorLight.Sense();

            FactTableManager.AddOrChangeAttribute(relativePositionX, relativePositionY, "hasOdour", isSmelly.ToString());
            FactTableManager.AddOrChangeAttribute(relativePositionX, relativePositionY, "hasWind", isWindy.ToString());
            FactTableManager.AddOrChangeAttribute(relativePositionX, relativePositionY, "hasPortal", isBright.ToString());

            if (sensorMonster.Sense())
            {
                FactTableManager.AddOrChangeAttribute(relativePositionX, relativePositionY, "hasMonsterProbability", "100");
            }
            else
            {
                FactTableManager.AddOrChangeAttribute(relativePositionX, relativePositionY, "hasMonsterProbability", "0");
            }
            if (sensorAbyss.Sense())
            {
                FactTableManager.AddOrChangeAttribute(relativePositionX, relativePositionY, "hasAbyssProbability", "100");
            }
            else
            {
                FactTableManager.AddOrChangeAttribute(relativePositionX, relativePositionY, "hasAbyssProbability", "0");
            }
            FactTableManager.AddOrChangeAttribute(relativePositionX, relativePositionY, "hasAbyssProbability", "0");

            UpdateFactsFromRules();
        }

        /// <summary>
        /// Specifies to the agent that he died (painfully, for sure).
        /// </summary>
        /// <param name="hadMonster">If the cause of death is a monster.</param>
        /// <param name="hadAbyss">If the cause of death is an abyss.</param>
        public void Die(bool hadMonster, bool hadAbyss)
        {
            if (hadMonster == true)
            {
                FactTableManager.UpdateProbability(relativePositionX, relativePositionY, "hasMonsterProbability", "100");
            }
            else
            {
                FactTableManager.UpdateProbability(relativePositionX, relativePositionY, "hasMonsterProbability", "0");
            }

            if (hadAbyss == true)
            {
                FactTableManager.UpdateProbability(relativePositionX, relativePositionY, "hasAbyssProbability", "100");
            }
            else
            {
                FactTableManager.UpdateProbability(relativePositionX, relativePositionY, "hasAbyssProbability", "0");

            }
            UpdatePosition(0, 0);
        }

        /// <summary>
        /// Updates the relative position.
        /// </summary>
        /// <param name="posX">The position x.</param>
        /// <param name="posY">The position y.</param>
        public void UpdatePosition(int posX, int posY)
        {
            relativePositionX = posX;
            relativePositionY = posY;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Uses the portal to the next board.
        /// </summary>
        /// <returns>Action cost.</returns>
        private int UsePortal()
        {
            knownCells.Clear();
            scopeCells.Clear();
            FactTableManager.CreateFactFile();

            relativePositionX = 0;
            relativePositionY = 0;
            return effectorUsePortal.DoAction();
        }

        /// <summary>
        /// Throws the stone to a specific position.
        /// </summary>
        /// <param name="positionX">The position x.</param>
        /// <param name="positionY">The position y.</param>
        /// <returns></returns>
        private int ThrowStoneTo(int positionX, int positionY)
        {
            effectorThrowRock.LaunchPosX = positionX;
            effectorThrowRock.LaunchPosY = positionY;

            FactTableManager.UpdateProbability(positionX, positionY, "hasMonsterProbability", "0");

            return effectorThrowRock.DoAction();
        }

        /// <summary>
        /// Gets the random position among scope positions.
        /// </summary>
        /// <returns>The position.</returns>
        private Tuple<int, int> GetRandomPositionInScope()
        {
            Random rng = new Random();
            Tuple<int, int> newPos;
            do
            {
                newPos = scopeCells.ElementAt(rng.Next(0, scopeCells.Count()));
            } while (newPos.Item1 == relativePositionX && newPos.Item2 == relativePositionY);
            return newPos;
        }

        /// <summary>
        /// Finds where to throw a stone.
        /// </summary>
        /// <returns>The position to throw the stone at.</returns>
        private Tuple<int, int> FindWhereToThrowStone()
        {
            Tuple<int, int> bestCell = null;
            int minAbyssProba = int.MaxValue;
            int monsterProba = int.MinValue;

            int monsterProbability;
            int abyssProbability;
            foreach (Tuple<int, int> position in scopeCells)
            {
                try
                {
                    monsterProbability = Convert.ToInt32(FactTableManager.GetAttributeOfFactAtLocation(
                        "Scope", position.Item1, position.Item2, "hasMonsterProbability"));
                }
                catch (FormatException)
                {
                    monsterProbability = 0;
                }

                try
                {
                    abyssProbability = Convert.ToInt32(FactTableManager.GetAttributeOfFactAtLocation(
                        "Scope", position.Item1, position.Item2, "hasAbyssProbability"));
                }
                catch (FormatException)
                {
                    abyssProbability = 0;
                }
                if (monsterProbability == 0)
                {
                    continue;
                }
                if (abyssProbability < minAbyssProba)
                {
                    minAbyssProba = abyssProbability;
                    monsterProba = monsterProbability;
                    bestCell = position;
                }
                if (abyssProbability == minAbyssProba)
                {
                    if (monsterProbability > monsterProba)
                    {
                        bestCell = position;
                        monsterProba = monsterProbability;
                    }
                }
            }
            return bestCell;
        }

        /// <summary>
        /// Updates the facts in the table.
        /// </summary>
        private void UpdateFacts()
        {
            int result = FactTableManager.ChangeInnerTextAtLocation("Known", relativePositionX, relativePositionY);
            if (result == 0)
            {
                return;
            }
            if (result == -1)
            {
                FactTableManager.AddOrReplaceFactAtLocation("Scope", 
                    relativePositionX, relativePositionY, 
                    new Dictionary<String, String>(), "Known"
                );
            }
            Tuple<int, int> currentCell = new Tuple<int, int>(relativePositionX, relativePositionY);
            knownCells.Add(currentCell);
            scopeCells.Remove(currentCell);

            foreach (Tuple<int, int> neighbourPos in sensorNeighbours.Get(relativePositionX, relativePositionY))
            {
                if (!knownCells.Contains(neighbourPos) && !scopeCells.Contains(neighbourPos))
                {
                    FactTableManager.AddOrReplaceFactAtLocation("Scope", 
                        neighbourPos.Item1, neighbourPos.Item2, 
                        new Dictionary<String, String>()
                    );
                    scopeCells.Add(new Tuple<int, int>(neighbourPos.Item1, neighbourPos.Item2));
                }
            }
        }

        /// <summary>
        /// Loads the rules from the file.
        /// </summary>
        private void loadRulesFile()
        {
            inferences.Clear();
            XmlDocument xmldoc = new XmlDocument();
            XmlNodeList xmlnode;
            int i = 0;
            FileStream fs = new FileStream(@"../../rules.xml", FileMode.Open, FileAccess.Read);
            xmldoc.Load(fs);
            xmlnode = xmldoc.GetElementsByTagName("Rule");
            for (i = 0; i <= xmlnode.Count - 1; i++)
            {
                XmlNodeList facts = ((XmlElement)xmlnode[i]).GetElementsByTagName("Fact");
                XmlNodeList implies = ((XmlElement)xmlnode[i]).GetElementsByTagName("Implies");
                XmlNodeList actions = ((XmlElement)xmlnode[i]).GetElementsByTagName("Action");

                cInference inference = new cInference();
                for (int j = 0; j < facts.Count; j++)
                {
                    // Récupération du fait
                    cFact fact = new cFact(facts.Item(j).Value);

                    // Récupération des attributes associés au fait
                    XmlElement xmlElement = facts.Item(j) as XmlElement;
                    XmlAttributeCollection attributes = xmlElement.Attributes;
                    foreach (XmlAttribute att in attributes)
                    {
                        fact.Attributes.Add(att.Name, att.Value);
                    }

                    inference.Facts.Add(fact);
                }
                for (int j = 0; j < implies.Count; j++)
                {
                    // Récupération de l'implication
                    cFact implie = new cFact(implies.Item(j).Value);

                    // Récupération des attributes associés à l'implication
                    XmlElement xmlElement = implies.Item(j) as XmlElement;
                    XmlAttributeCollection attributes = xmlElement.Attributes;
                    foreach (XmlAttribute att in attributes)
                    {
                        implie.Attributes.Add(att.Name, att.Value);
                    }

                    inference.Implies.Add(implie);
                }
                inference.IsActionInference = false;
                if (actions.Count > 0)
                {
                    foreach (object action in actions)
                    {
                        cAction ourAction = new cAction();
                        XmlElement xmlElement = action as XmlElement;
                        // Récupérer son nom
                        ourAction.Name = xmlElement.GetAttribute("name");

                        // Récupérer ses paramètres
                        string attributs = xmlElement.GetAttribute("parameters");
                        ourAction.SetParameters(attributs);

                        // L'ajouter à l'inférence
                        inference.Actions.Add(ourAction);

                    }
                    // L'inférence est une inférence d'action
                    inference.IsActionInference = true;
                }
                inferences.Add(inference);
            }
            fs.Close();
        }

        /// <summary>
        /// Finds the safest position to go to.
        /// </summary>
        /// <returns>The safest position to go to.</returns>
        private Tuple<int, int> FindSafestPositionToGoTo()
        {
            foreach (Tuple<int, int> position in scopeCells)
            {
                Dictionary<string, string> attributes = new Dictionary<string, string>
                {
                    { "isSafe", "True" }
                };
                if (FactTableManager.IsFactInTable("Scope", position.Item1, position.Item2, attributes))
                {
                    return position;
                }
            }

            Tuple<int, int> posWithBestProbability = null;
            int bestProbability = int.MaxValue;
            foreach (Tuple<int, int> position in scopeCells)
            {
                String monsterProbability = FactTableManager.GetAttributeOfFactAtLocation(
                    "Scope", position.Item1, position.Item2, "hasMonsterProbability");
                String abyssProbability = FactTableManager.GetAttributeOfFactAtLocation(
                    "Scope", position.Item1, position.Item2, "hasAbyssProbability");

                if (monsterProbability != null &&
                    monsterProbability != "" &&
                    Int32.Parse(monsterProbability) > 0)
                {
                    // Besoin de lancer une pierre en premier avant de bouger
                    return null;
                }

                if (abyssProbability == "0" || abyssProbability == null || abyssProbability == "")
                {
                    abyssProbability = "0";
                    return position;
                }
                else
                {
                    int abyssProb = Convert.ToInt32(abyssProbability);
                    if (bestProbability > abyssProb)
                    {
                        bestProbability = abyssProb;
                        posWithBestProbability = position;
                    }
                }
            }
            return posWithBestProbability;
        }

        /// <summary>
        /// Updates the facts according to the rules.
        /// </summary>
        private void UpdateFactsFromRules()
        {
            List<Tuple<int, int>> cellsToCheck = new List<Tuple<int, int>>();
            cellsToCheck.AddRange(knownCells);
            cellsToCheck.AddRange(scopeCells);

            foreach (Tuple<int, int> cell in cellsToCheck)
            {
                foreach (cInference inference in inferences)
                {
                    if (inference.IsActionInference)
                    {
                        continue;
                    }

                    bool conditionsAreRespected = inference.Facts.All(
                        fact =>
                        {
                            int posX = cell.Item1 + Convert.ToInt32(fact.Attributes["locationX"]);
                            int posY = cell.Item2 + Convert.ToInt32(fact.Attributes["locationY"]);
                            return fact.Attributes.All(
                                attribute =>
                                    (attribute.Key == "locationX") || (attribute.Key == "locationY") ||
                                    (FactTableManager.GetAttributeAtLocation(posX, posY, attribute.Key) == attribute.Value)
                            );
                        }

                    );
                    if (conditionsAreRespected)
                    {
                        foreach (cFact implication in inference.Implies)
                        {
                            int xPos = cell.Item1 + Convert.ToInt32(implication.Attributes["locationX"]);
                            int yPos = cell.Item2 + Convert.ToInt32(implication.Attributes["locationY"]);

                            foreach (KeyValuePair<string, string> attribute in implication.Attributes)
                            {
                                if ((attribute.Key == "locationX") || (attribute.Key == "locationY"))
                                {
                                    continue;
                                }

                                if (attribute.Key.Contains("Probability"))
                                {
                                    FactTableManager.UpdateProbability(xPos, yPos, attribute.Key, attribute.Value);
                                }
                                else
                                {
                                    FactTableManager.AddOrChangeAttribute(xPos, yPos, attribute.Key, attribute.Value);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Moves agent to specified position.
        /// </summary>
        /// <param name="positionX">The position x.</param>
        /// <param name="positionY">The position y.</param>
        /// <returns>The move cost.</returns>
        private int MoveToPosition(int positionX, int positionY)
        {
            return effectorMove.DoAction(positionX, positionY);
        }

        #endregion
    }
}
