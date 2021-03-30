using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IATD3
{
    public class cAgent
    {
        // Effectors
        cEffectorMove effectorMove;
        cEffectorUsePortal effectorUsePortal;
        cEffectorThrowRock effectorThrowRock;

        // Sensors
        cSensorLight sensorLight;
        cSensorWind sensorWind;
        cSensorOdour sensorOdour;
        cSensorNeighbours sensorNeighbours;

        // Relative location
        int relativeLocationX;
        int relativeLocationY;

        List<cInference> inferences;
        List<Tuple<int, int>> knownCells;
        List<Tuple<int, int>> scopeCells;

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

            // Relative location
            relativeLocationX = 0;
            relativeLocationY = 0;

            inferences = new List<cInference>();
            knownCells = new List<Tuple<int, int>>();
            scopeCells = new List<Tuple<int, int>>();

            loadRulesFile();
            FactTableManager.CreateFactFile();
        }

        public int UsePortal()
        {
            knownCells.Clear();
            scopeCells.Clear();
            FactTableManager.CreateFactFile();
            return effectorUsePortal.DoAction();
        }

        public int ThrowStoneTo(int locationX, int locationY)
        {
            effectorThrowRock.LaunchPosX = locationX;
            effectorThrowRock.LaunchPosY = locationY;
            return effectorThrowRock.DoAction();
        }

        public int Act()
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            attributes.Add("hasPortal", "True");

            //comparring the created attributes to the ones in the fact table. AKA, are we on a portal? 
            if (FactTableManager.IsFactInTable(string.Empty, relativeLocationX, relativeLocationY, attributes))
            {
                return UsePortal();
            }

            //Finding and executing safest move
            Tuple<int, int> position = FindSafestPositionToGoTo();
            if (position.Item1 == 0 && position.Item2 == 0)
            {
                Tuple<int, int> rockThrowPosition = FindWhereToThrowRock();
                if (rockThrowPosition != null)
                {
                    ThrowStoneTo(rockThrowPosition.Item1, rockThrowPosition.Item2);
                    return 10;
                }
                else
                {
                    //if no location is truely safe and if there is no place to throw a rock (suppose the monsters are in crevaces, it's not worth to throw a rock) 
                    //we'll move to a random location and hope for the best. This is a worst case scenario
                    position = MoveToRandomLocationInScope();
                    Console.WriteLine(position);
                }

            }
            return MoveToLocation(position.Item1, position.Item2);
        }

        public void UseSensors()
        {
            UpdateFacts();

            bool isWindy = sensorWind.Sense();
            bool isSmelly = sensorOdour.Sense();
            bool isBright = sensorLight.Sense();

            // Add facts to the facts tables
            // I propose facts to be in this form :

            // <Fact locationX="x" locationY="y" presence="true/false">FactName</Fact>

            // Location would be a relative location (agent only knows what moves he did, not where he is exactly)
            // But we assume each are cell sizes are equal

            FactTableManager.AddOrChangeAttribute(relativeLocationX, relativeLocationY, "hasOdour", isSmelly.ToString());
            FactTableManager.AddOrChangeAttribute(relativeLocationX, relativeLocationY, "hasWind", isWindy.ToString());
            FactTableManager.AddOrChangeAttribute(relativeLocationX, relativeLocationY, "hasPortal", isBright.ToString());

            UpdateFactsFromRules();
        }

        public void Die(bool hadMonster, bool hadAbyss)
        {
            FactTableManager.AddOrChangeAttribute(relativeLocationX, relativeLocationY, "hasMonster", hadMonster.ToString());
            FactTableManager.AddOrChangeAttribute(relativeLocationX, relativeLocationY, "hasAbyss", hadMonster.ToString());

            /*relativeLocationX = 0;
            relativeLocationY = 0;*/
            UpdatePosition(0, 0);
            // Reset la position en (0,0)
        }

        public void UpdatePosition(int posX, int posY)
        {
            relativeLocationX = posX;
            relativeLocationY = posY;
        }

        private Tuple<int, int> MoveToRandomLocationInScope()
        {
            Random rng = new Random();
            return scopeCells.ElementAt(rng.Next(0, scopeCells.Count()));
        }

        private Tuple<int, int> FindWhereToThrowRock()
        {
            foreach (var position in scopeCells)
            {
                Dictionary<string, string> attributes = new Dictionary<string, string>();
                attributes.Add("hasMonster", "True");
                attributes.Add("hasAbyss", "False");
                attributes.Add("probabilityAbyss", "100");
                if (FactTableManager.IsFactInTable("Scope", position.Item1, position.Item2, attributes))
                {
                    return position;
                }
            }

            return null;
        }

        private void UpdateFacts()
        {
            FactTableManager.AddOrReplaceFactAtLocation("Scope", relativeLocationX, relativeLocationY, new Dictionary<String, String>(), "Known");
            Tuple<int, int> currentCell = new Tuple<int, int>(relativeLocationX, relativeLocationY);
            knownCells.Add(currentCell);
            scopeCells.Remove(currentCell);

            foreach (var neighbourPos in sensorNeighbours.Get(relativeLocationX, relativeLocationY))
            {
                if (!knownCells.Contains(neighbourPos))
                {
                    FactTableManager.AddOrReplaceFactAtLocation("Scope", neighbourPos.Item1, neighbourPos.Item2, new Dictionary<String, String>());
                    scopeCells.Add(new Tuple<int, int>(neighbourPos.Item1, neighbourPos.Item2));
                }
            }
        }

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
                for (var j = 0; j < facts.Count; j++)
                {
                    // Récupération du fait
                    cFact fact = new cFact(facts.Item(j).Value);

                    // Récupération des attributs associés au fait
                    XmlElement xmlElement = facts.Item(j) as XmlElement;
                    XmlAttributeCollection attributes = xmlElement.Attributes;
                    foreach (XmlAttribute att in attributes)
                    {
                        fact.Attributs.Add(att.Name, att.Value);
                    }

                    inference.Facts.Add(fact);
                }
                for (var j = 0; j < implies.Count; j++)
                {
                    // Récupération de l'implication
                    cFact implie = new cFact(implies.Item(j).Value);

                    // Récupération des attributs associés à l'implication
                    XmlElement xmlElement = implies.Item(j) as XmlElement;
                    XmlAttributeCollection attributes = xmlElement.Attributes;
                    foreach (XmlAttribute att in attributes)
                    {
                        implie.Attributs.Add(att.Name, att.Value);
                    }

                    inference.Implies.Add(implie);
                }
                inference.IsActionInference = false;
                if (actions.Count > 0)
                {
                    foreach (var action in actions)
                    {
                        cAction ourAction = new cAction();
                        XmlElement xmlElement = action as XmlElement;
                        // Récupérer son nom
                        ourAction.Name = xmlElement.GetAttribute("name");

                        // Récupérer ses paramètres
                        string attributs = xmlElement.GetAttribute("parameters");
                        ourAction.SetParameters(attributs);

                        // L'ajouter à l'inférence  // 
                        inference.Actions.Add(ourAction);

                    }
                    // L'inférence est une inférence d'action
                    inference.IsActionInference = true;
                }
                inferences.Add(inference);
            }
            fs.Close();
        }

        private Tuple<int, int> FindSafestPositionToGoTo()
        {
            foreach (var position in scopeCells)
            {
                Dictionary<string, string> attributes = new Dictionary<string, string>();
                attributes.Add("isSafe", "True");
                if (FactTableManager.IsFactInTable("Scope", position.Item1, position.Item2, attributes))
                {
                    return position;
                }
            }
            // Si aucune case safe, sélectionner la plus safe (ou lancer une pierre)
            return new Tuple<int, int>(0, 0);

            // Si aucune case safe au dessus d'un seuil, retourner null
        }

        private void createXmlFile()
        {
            XmlTextWriter writer = new XmlTextWriter(@"../../facts.xml", null);
            writer.WriteStartDocument();
            writer.WriteEndElement();

            writer.WriteEndDocument();
            writer.Close();
        }

        /// <summary>
        ///    Uses each sensor to add facts to fact table.
        /// </summary>


        private void UpdateFactsFromRules()
        {
            List<Tuple<int, int>> cellsToCheck = new List<Tuple<int, int>>();
            cellsToCheck.AddRange(knownCells);
            cellsToCheck.AddRange(scopeCells);
            foreach (var cell in cellsToCheck)
            {
                foreach (var inference in inferences)
                {
                    if (inference.IsActionInference)
                    {
                        continue;
                    }
                    bool conditionsAreRespected = inference.Facts.All(
                        fact =>
                        fact.Attributs.All(
                            attribute =>
                            (attribute.Key == "locationX") || (attribute.Key == "locationY") ||
                            (FactTableManager.GetAttributeAtLocation(cell.Item1, cell.Item2, attribute.Key) == attribute.Value)
                        )
                    );
                    if (conditionsAreRespected)
                    {
                        foreach (var implication in inference.Implies)
                        {
                            foreach (var attribute in implication.Attributs)
                            {
                                if ((attribute.Key == "locationX") || (attribute.Key == "locationY"))
                                {
                                    continue;
                                }

                                int xPos = cell.Item1 + Int32.Parse(implication.Attributs["locationX"]);
                                int yPos = cell.Item2 + Int32.Parse(implication.Attributs["locationY"]);
                                // /!\ Il y a toujours la mise à jour de tous les attributs (écrasement) de la case alors qu'il ne faut pas forcément
                                // → Il faut retenir l'information la plus importante (la plus sure et utile) plutôt que la dernière
                                FactTableManager.AddOrChangeAttribute(xPos, yPos, attribute.Key, attribute.Value);
                            }
                        }
                    }
                }
            }
        }

        //Déplace l'agent à l'emplacement spécifié en paramètre d'entrée
        private int MoveToLocation(int locationX, int locationY)
        {
            return effectorMove.DoAction(locationX, locationY);
        }

        private void ChainageArriere()
        {
            /*
             * 1. Charger les faits initiaux
               2. Empiler le but dans une pile
               pile_buts
               3. Tant que
               Pas Terminé faire
                   3.1 Sélectionner les règles applicables et non marquées
                   -
                   celles dont la conclusion = au sommet de la pile_buts
                   -
                   si pas de règles applicables. Dépiler la pile_buts , et la pile_regles
                   3.2 Choisir la règle à appliquer
                   -
                   ajouter les autres règles dans la pile sélectionnée ( pile_regles
                   3.3 Appliquer la règle
                   -
                   Ajouter les conditions au sommet de la pile_buts
                   -
                   Empiler la règle dans pile_regles
                   -
                   Marquer la règle
                   3.4 Si
                   pile_buts est vide alors le processus est terminé
             */
        }

        private void GoToPortal()
        {
            UpdateFactsFromRules();

            //pile de buts
            Stack<cFact> goalsStack = new Stack<cFact>();
            cFact finalGoal = new cFact("");
            finalGoal.Attributs.Add("cleared", "true");
            goalsStack.Push(finalGoal);

            // Appeler chainage arrière qui renvoie une action.
            // Si non nulle, l'effectuer.
        }

        public void GoToSafeCell()
        {
            UpdateFactsFromRules();

            //pile de buts
            Stack<cFact> goalsStack = new Stack<cFact>();
            cFact safeCell = new cFact("");
            safeCell.Attributs.Add("isSafe", "true");
            safeCell.Attributs.Add("locationX", "0");
            safeCell.Attributs.Add("locationY", "0");
            goalsStack.Push(safeCell);

            cFact agentOnSafeCell = new cFact("AgentPosition");
            agentOnSafeCell.Attributs.Add("onLocationX", "0");
            agentOnSafeCell.Attributs.Add("onLocationY", "0");
            goalsStack.Push(agentOnSafeCell);

            // Appeler chainage arrière qui renvoie une action.
            // Si non nulle, l'effectuer.
        }

        private void ExecuteAction(cAction actionParams)
        {
            string[] parameters = actionParams.Parameters;
            switch (actionParams.Name)
            {
                case "UsePortal":
                    UsePortal();
                    break;
                case "MoveTo":
                    MoveToLocation(Convert.ToInt32(parameters[0]), Convert.ToInt32(parameters[1]));
                    break;
                case "ThrowStone":
                    ThrowStoneTo(Convert.ToInt32(parameters[0]), Convert.ToInt32(parameters[1]));
                    break;

            }

        }

        /*  public int MoveTo()
          {
              Tuple<int, int> newPosition = Move();

              if (newPosition == null)
              {
                  return int.MinValue;
              }
              effectorMove.MovementPosX = newPosition.Item1;
              effectorMove.MovementPosY = newPosition.Item2;
              return effectorMove.DoAction();
          }*/
    }
}
