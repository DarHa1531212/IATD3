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
    class cAgent
    {
        // Effectors
        cEffectorDown effectorDown;
        cEffectorUp effectorUp;
        cEffectorLeft effectorLeft;
        cEffectorRight effectorRight;
        cEffectorUsePortal effectorUsePortal;
        cEffectorThrowRock effectorThrowRock;

        // Sensors
        cSensorLight sensorLight;
        cSensorWind sensorWind;
        cSensorOdour sensorOdour;

        // Relative location
        int relativeLocationX;
        int relativeLocationY;

        public cAgent()
        {
            // Instantiate effectors
            effectorDown = new cEffectorDown();
            effectorLeft = new cEffectorLeft();
            effectorRight = new cEffectorRight();
            effectorThrowRock = new cEffectorThrowRock();
            effectorUp = new cEffectorUp();
            effectorUsePortal = new cEffectorUsePortal();

            // Instantiate sensors
            sensorLight = new cSensorLight();
            sensorOdour = new cSensorOdour();
            sensorWind = new cSensorWind();

            // Relative location
            relativeLocationX = 0;
            relativeLocationY = 0;

            loadRulesFile();
            FactTableManager.CreateFactFile();
        }

        private void loadRulesFile()
        {
            XmlDataDocument xmldoc = new XmlDataDocument();
            XmlNodeList xmlnode;
            int i = 0;
            string str = null;
            FileStream fs = new FileStream(@"../../rules.xml", FileMode.Open, FileAccess.Read);
            xmldoc.Load(fs);
            xmlnode = xmldoc.GetElementsByTagName("Rule");
            for (i = 0; i <= xmlnode.Count - 1; i++)
            {
                xmlnode[i].ChildNodes.Item(0).InnerText.Trim();
                str = xmlnode[i].ChildNodes.Item(0).InnerText.Trim() + "  " + xmlnode[i].ChildNodes.Item(1).InnerText.Trim() + "  " + xmlnode[i].ChildNodes.Item(2).InnerText.Trim();
                Console.WriteLine("********************");
                Console.WriteLine("XML tag: ");
                Console.WriteLine(str);
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
            attributes.Add("locationX", relativeLocationX.ToString());
            attributes.Add("locationY", relativeLocationY.ToString());
            attributes.Add("presence", isSmelly.ToString());
            FactTableManager.AddFact("Smell", attributes);

            attributes["presence"] = isBright.ToString();
            FactTableManager.AddFact("Portal", attributes);

            attributes["presence"] = isWindy.ToString();
            FactTableManager.AddFact("Wind", attributes);

        }
    }
}
