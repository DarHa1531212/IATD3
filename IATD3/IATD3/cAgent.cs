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

        private void UpdateFacts()
        {
            FactTableManager.AddOrReplaceFactAtLocation("Known", relativeLocationX, relativeLocationY, new Dictionary<String, String>());
            Tuple<int, int> currentCell = new Tuple<int, int>(relativeLocationX, relativeLocationY);
            knownCells.Add(currentCell);
            scopeCells.Remove(currentCell);

            foreach (var neighbourPos in sensorNeighbours.Get(relativeLocationX, relativeLocationY))
            {
                FactTableManager.AddOrReplaceFactAtLocation("Scope", neighbourPos.Item1, neighbourPos.Item2, new Dictionary<String, String>());
                scopeCells.Add(new Tuple<int, int>(neighbourPos.Item1, neighbourPos.Item2));
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
                inferences.Add(inference);
            }
            fs.Close();
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

        public void ThrowRock()
        {
            effectorThrowRock.LaunchPosX = 1;
            effectorThrowRock.DoAction();
        }

        public void Die(bool hadMonster, bool hadAbyss)
        {
            FactTableManager.AddOrChangeAttribute(relativeLocationX, relativeLocationY, "hasMonster", hadMonster.ToString());
            FactTableManager.AddOrChangeAttribute(relativeLocationX, relativeLocationY, "hasAbyss", hadMonster.ToString());

            relativeLocationX = 0;
            relativeLocationY = 0;
            // Reset la position en (0,0)
        }

        private void ChainageAvant()
        {
            // loadRulesFile();
            //obtenir les faits initiaux

            //tant que  (pas ternimé et il reste au moins une règle non marquée)
            /*
                faire
                    sélectionner les règles applicables
                        celles non marquées
                        si une des règles est en contraciction
                            marquer la règle
                        celles dont les conditions existent dans la base de faits
                    choisir la règle à appliquer
                    appliquer la règle: ajouter les conclusions à la base de faits
                    marquer la règle      
            */

            //soit frontière
            //de chaque case frontière, calculer le risque de monstre ou crevasse
            //aller à l'emplacement le moins dangeureux

            //int[,] percievedThreat = new int[x,x];
            //
        }

        private void UpdateFactsFromRules()
        {
            List<Tuple<int, int>> cellsToCheck = new List<Tuple<int, int>>();
            cellsToCheck.Concat(knownCells).Concat(scopeCells);
            foreach (var cell in cellsToCheck)
            {
                foreach (var inference in inferences)
                {
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
                            foreach(var attribute in implication.Attributs)
                            {
                                // /!\ Il y a toujours la mise à jour de tous les attributs (écrasement) de la case alors qu'il ne faut pas forcément
                                // → Il faut retenir l'information la plus importante (la plus sure et utile) plutôt que la dernière
                                FactTableManager.AddOrChangeAttribute(
                                    relativeLocationX + Int32.Parse(implication.Attributs["locationX"]),
                                    relativeLocationY + Int32.Parse(implication.Attributs["locationY"]),
                                    attribute.Key,
                                    attribute.Value
                                );
                            }
                        }
                    }
                }
            }
        }
    }
}
