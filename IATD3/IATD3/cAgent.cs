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

            // Relative location
            relativeLocationX = 0;
            relativeLocationY = 0;

            inferences = new List<cInference>();
            knownCells = new List<Tuple<int, int>>();
            scopeCells = new List<Tuple<int, int>>();

            loadRulesFile();
            FactTableManager.CreateFactFile();
        }

        private void loadRulesFile()
        {
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
            bool isWindy = sensorWind.Sense();
            bool isSmelly = sensorOdour.Sense();
            bool isBright = sensorLight.Sense();

            // Add facts to the facts tables
            // I propose facts to be in this form :

            // <Fact locationX="x" locationY="y" presence="true/false">FactName</Fact>

            // Location would be a relative location (agent only knows what moves he did, not where he is exactly)
            // But we assume each are cell sizes are equal

            Dictionary<string, string> attributes = new Dictionary<string, string>();
            attributes.Add("presence", isSmelly.ToString());
            FactTableManager.AddOrReplaceFactAtLocation("Smell", relativeLocationX, relativeLocationY, attributes);
            
            attributes = new Dictionary<string, string>();
            attributes.Add("presence", isBright.ToString());
            FactTableManager.AddOrReplaceFactAtLocation("Portal", relativeLocationX, relativeLocationY, attributes);

            attributes = new Dictionary<string, string>();
            attributes.Add("presence", isWindy.ToString());
            FactTableManager.AddOrReplaceFactAtLocation("Wind", relativeLocationX, relativeLocationY, attributes);

            UpdateCellsKnowledge(relativeLocationX, relativeLocationY);
        }

        private void UpdateCellsKnowledge(int relativeLocationX, int relativeLocationY)
        {
            knownCells.Add(new Tuple<int, int>(relativeLocationX, relativeLocationY));

            scopeCells.Add(new Tuple<int, int>(relativeLocationX - 1, relativeLocationY));
            scopeCells.Add(new Tuple<int, int>(relativeLocationX, relativeLocationY - 1));
            scopeCells.Add(new Tuple<int, int>(relativeLocationX + 1, relativeLocationY));
            scopeCells.Add(new Tuple<int, int>(relativeLocationX, relativeLocationY + 1));
        }

        public void ThrowRock()
        {
            effectorThrowRock.LaunchPosX = 1;
            effectorThrowRock.DoAction();
        }

        public void Die(bool hadMonster, bool hadAbyss)
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            attributes.Add("presence", hadMonster.ToString());
            FactTableManager.AddOrReplaceFactAtLocation("Monster", relativeLocationX, relativeLocationY, attributes);

            attributes = new Dictionary<string, string>();
            attributes.Add("presence", hadAbyss.ToString());
            FactTableManager.AddOrReplaceFactAtLocation("Abyss", relativeLocationX, relativeLocationY, attributes);

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

        private void AdaptFacts()
        {
            foreach (var cell in scopeCells)
            {
                foreach (var inference in inferences)
                {
                    bool conditionsAreRespected = true;
                    foreach (var fact in inference.Facts)
                    {
                        bool sameFact = true;
                        foreach (var attribut in fact.Attributs)
                        {
                            String factAttribute = FactTableManager.GetAttributeOfFactAtLocation(fact.Element, cell.Item1, cell.Item2, attribut.Key);
                            sameFact = sameFact && (factAttribute == attribut.Value);
                        }
                        conditionsAreRespected = conditionsAreRespected && sameFact;
                    }
                    if (conditionsAreRespected)
                    {
                        foreach (var implication in inference.Implies)
                        {
                            FactTableManager.AddOrReplaceFactAtLocation(
                                implication.Element,
                                relativeLocationX + Int32.Parse(implication.Attributs["locationX"]),
                                relativeLocationY + Int32.Parse(implication.Attributs["locationY"]),
                                implication.Attributs
                            );
                        }
                    }
                }
            }
        }
    }
}
