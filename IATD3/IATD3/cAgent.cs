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

            loadRulesFile();

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
        }

        /// <summary>
        ///    Uses each sensor to add facts to fact table.
        /// </summary>
        private void UseSensors()
        {
            bool isWindy = sensorWind.Sense();
            bool isSmelly = sensorOdour.Sense();
            bool isBright = sensorLight.Sense();
        }
    }
}
