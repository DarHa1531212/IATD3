using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IATD3
{
    public static class FactTableManager
    {
        private const string _path = @"../../facts.xml";

        public static void CreateFactFile()
        {
            XmlDocument xmldoc = new XmlDocument();
            FileStream fs = new FileStream(_path, FileMode.Create, FileAccess.ReadWrite);
            xmldoc.LoadXml("<FactTable></FactTable>");

            XmlNode facts = xmldoc.CreateElement("Facts");
            xmldoc.DocumentElement.AppendChild(facts);

            xmldoc.Save(fs);
            fs.Close();
        }

        private static void AddFact(string factName, Dictionary<string, string> attributes)
        {
            XmlDocument xmldoc = new XmlDocument();
            FileStream fs = new FileStream(_path, FileMode.Open, FileAccess.ReadWrite);
            xmldoc.Load(fs);

            XmlNode facts = xmldoc.DocumentElement.SelectSingleNode("Facts");

            XmlElement newFact = xmldoc.CreateElement("Fact");
            newFact.InnerText = factName;
            foreach (string attribute in attributes.Keys)
            {
                newFact.SetAttribute(attribute, attributes[attribute]);
            }
            facts.AppendChild(newFact);
            fs.SetLength(0);
            xmldoc.Save(fs);
            fs.Close();
        }

        public static void AddOrReplaceFactAtLocation(string factName,
            int locationX,
            int locationY,
            Dictionary<string, string> newAttributes,
            string newFactName = null)
        {
            XmlDocument xmldoc = new XmlDocument();
            FileStream fs = new FileStream(_path, FileMode.Open, FileAccess.ReadWrite);
            xmldoc.Load(fs);

            XmlNode element = xmldoc.SelectSingleNode("//Fact[@locationX='" + locationX.ToString() +
                "' and @locationY='" + locationY.ToString() + "' and text()='" + factName + "']");

            newAttributes.Add("locationX", locationX.ToString());
            newAttributes.Add("locationY", locationY.ToString());

            if (element == null)
            {
                fs.Close();
                AddFact(newFactName != null ? newFactName : factName, newAttributes);
            }
            else
            {
                XmlElement xmlElement = element as XmlElement;
                if (newFactName != null)
                {
                    xmlElement.RemoveAllAttributes();
                    xmlElement.InnerText = newFactName;
                }
                foreach (string attribute in newAttributes.Keys)
                {
                    xmlElement.SetAttribute(attribute, newAttributes[attribute]);
                }
                fs.SetLength(0);
                xmldoc.Save(fs);
                fs.Close();
            }
        }

        public static void AddOrReplaceFactStatusAtLocation(
            string oldFactName,
            string newFactName,
            int locationX,
            int locationY,
            Dictionary<string, string> newAttributes)
        {
            XmlDocument xmldoc = new XmlDocument();
            FileStream fs = new FileStream(_path, FileMode.Open, FileAccess.ReadWrite);
            xmldoc.Load(fs);

            XmlNode element = xmldoc.SelectSingleNode("//Fact[@locationX='" + locationX.ToString() +
                "' and @locationY='" + locationY.ToString() + "' and text()='" + oldFactName + "']");

            newAttributes.Add("locationX", locationX.ToString());
            newAttributes.Add("locationY", locationY.ToString());

            if (element == null)
            {
                fs.Close();
                AddFact(newFactName, newAttributes);
            }
            else
            {
                XmlElement xmlElement = element as XmlElement;
                xmlElement.RemoveAllAttributes();
                foreach (string attribute in newAttributes.Keys)
                {
                    xmlElement.SetAttribute(attribute, newAttributes[attribute]);
                }
                xmlElement.InnerText = newFactName;
                fs.SetLength(0);
                xmldoc.Save(fs);
                fs.Close();
            }
        }

        public static string GetAttributeOfFactAtLocation(string factName, int locationX, int locationY, string attribute)
        {
            XmlDocument xmldoc = new XmlDocument();
            FileStream fs = new FileStream(_path, FileMode.Open, FileAccess.ReadWrite);
            xmldoc.Load(fs);

            XmlNode element = xmldoc.SelectSingleNode("//Fact[@" + attribute + " and @locationX='" + locationX.ToString() +
                "' and @locationY='" + locationY.ToString() + "' and text()='" + factName + "']");

            if (element == null)
            {
                fs.Close();
                return null;
            }
            XmlElement xmlElement = element as XmlElement;
            fs.Close();
            return xmlElement.GetAttribute(attribute);
        }

        public static string GetAttributeAtLocation(int locationX, int locationY, string attribute)
        {
            XmlDocument xmldoc = new XmlDocument();
            FileStream fs = new FileStream(_path, FileMode.Open, FileAccess.ReadWrite);
            xmldoc.Load(fs);

            XmlNode element = xmldoc.SelectSingleNode("//Fact[@" + attribute + " and @locationX='" + locationX.ToString() +
                "' and @locationY='" + locationY.ToString() + "']");

            if (element == null)
            {
                fs.Close();
                return null;
            }
            XmlElement xmlElement = element as XmlElement;
            fs.Close();
            return xmlElement.GetAttribute(attribute);
        }

        public static bool IsFactInTable(string factName, int locationX, int locationY, Dictionary<string, string> attributes)
        {
            XmlDocument xmldoc = new XmlDocument();
            FileStream fs = new FileStream(_path, FileMode.Open, FileAccess.ReadWrite);
            xmldoc.Load(fs);

            string xpath = "//Fact[@locationX='" + locationX.ToString() +
                "' and @locationY='" + locationY.ToString() + "'";

            foreach (string attribute in attributes.Keys)
            {
                xpath += " and @" + attribute + "='" + attributes[attribute] + "'";
            }

            if (factName != String.Empty)
            {
                xpath += " and text()='" + factName + "'";
            }
            xpath += "]";

            //error here. XML functions are badly implemented
            XmlNode element = xmldoc.SelectSingleNode(xpath);
            fs.Close();

            return (element != null);
        }

        public static bool IsFactInTable(cFact fact)
        {
            XmlDocument xmldoc = new XmlDocument();
            FileStream fs = new FileStream(_path, FileMode.Open, FileAccess.ReadWrite);
            xmldoc.Load(fs);

            string xpath = "//Fact[";

            bool isFirstAttribute = true;
            foreach (string attribute in fact.Attributs.Keys)
            {
                if (!isFirstAttribute)
                {
                    xpath += "and";
                }
                xpath += " @" + attribute + "='" + fact.Attributs[attribute] + "'";
                if (isFirstAttribute)
                {
                    isFirstAttribute = false;
                }
            }

            xpath += "]";

            XmlNode element = xmldoc.SelectSingleNode(xpath);
            fs.Close();

            return (element != null);
        }

        public static int AddOrChangeAttributeIfNecessary(
            int locationX, int locationY,
            string attribute, string value,
            string probabilityAttribute, string probabilityValue)
        {
            String valueInTable = GetAttributeAtLocation(locationX, locationY, attribute);
            int probabilityValueInTable = Convert.ToInt32(GetAttributeAtLocation(locationX, locationY, probabilityAttribute));

            String newValue = value;
            int newProbability = Convert.ToInt32(probabilityValue);

            if (value != valueInTable && valueInTable != String.Empty)
            {
                // Conversion dans le même référentiel (false / true)
                newValue = valueInTable;
                newProbability = 100 - newProbability;
            }
            newProbability += probabilityValueInTable;
            newProbability = (newProbability > 100 ? 100 : (newProbability < 0 ? 0 : newProbability)); // On garde un pourcentage

            bool probabilityHasToBeUpdated = (
                newProbability > (probabilityValueInTable == 0 ?
                                    -1 :
                                    probabilityValueInTable)
            );

            if (probabilityHasToBeUpdated)
            {
                int returnCode = AddOrChangeAttribute(locationX, locationY, attribute, newValue);
                int returnCodeProb = AddOrChangeAttribute(locationX, locationY, probabilityAttribute, newProbability.ToString());
                return 1;
            }
            return 0;

            /*int p = Int32.Parse(probabilityValue) + probabilityValueInTable;
            p = (p > 100 ? 100 : (p < 0 ? 0 : p)); // On garde un pourcentage
            bool hasNewProbability =
                p > (probabilityValueInTable == 0 ?
                        -1 :
                        probabilityValueInTable);

            bool hasNewValue = GetAttributeAtLocation(locationX, locationY, attribute) != value;

            if (hasNewValue || hasNewProbability)
            {
                int returnCode = AddOrChangeAttribute(locationX, locationY, attribute, value);
                int returnCodeProb = AddOrChangeAttribute(locationX, locationY, probabilityAttribute, probabilityValue);
                return 1;
            }
            return 0;*/
        }

        public static int AddOrChangeAttribute(int locationX, int locationY, string attribute, string value)
        {
            XmlDocument xmldoc = new XmlDocument();
            FileStream fs = new FileStream(_path, FileMode.Open, FileAccess.ReadWrite);
            xmldoc.Load(fs);

            string xpath = "//Fact[@locationX='" + locationX.ToString() +
                "' and @locationY='" + locationY.ToString() + "']";

            XmlNode element = xmldoc.SelectSingleNode(xpath);

            if (element == null)
            {
                fs.Close();
                return -1;
            }
            int returnCode = 0;

            XmlElement xmlElement = element as XmlElement;
            if (xmlElement.HasAttribute(attribute))
            {
                returnCode = 1;
            }
            xmlElement.SetAttribute(attribute, value);

            fs.SetLength(0);
            xmldoc.Save(fs);
            fs.Close();
            return returnCode;
        }

        private static void AddOrCreateFactsId(int id)
        {
            XmlDocument xmldoc = new XmlDocument();
            FileStream fs = new FileStream(_path, FileMode.Open, FileAccess.ReadWrite);
            xmldoc.Load(fs);

            XmlNode element = xmldoc.SelectSingleNode("//Fact[@id='" + id + "']");
            if (element == null)
            {
                fs.Close();
                return;
            }
            XmlNode facts = xmldoc.CreateElement("Facts");
            XmlElement factsElement = facts as XmlElement;
            factsElement.SetAttribute("id", id.ToString());
            xmldoc.DocumentElement.AppendChild(facts);

            fs.SetLength(0);
            xmldoc.Save(fs);
            fs.Close();
        }

        public static int ChangeInnerTextAtLocation(string newInnerText, int locationX, int locationY)
        {

            XmlDocument xmldoc = new XmlDocument();
            FileStream fs = new FileStream(_path, FileMode.Open, FileAccess.ReadWrite);
            xmldoc.Load(fs);

            XmlNode element = xmldoc.SelectSingleNode("//Fact[@locationX='" + locationX.ToString() +
                "' and @locationY='" + locationY.ToString() + "']");
            if (element == null)
            {
                fs.Close();
                return -1;
            }
            XmlElement fact = element as XmlElement;
            String oldInnerText = fact.InnerText;
            fact.InnerText = newInnerText;
            fs.SetLength(0);
            xmldoc.Save(fs);
            fs.Close();

            if (newInnerText != oldInnerText)
            {
                return 1;
            }
            return 0;
        }

        public static void UpdateProbability(int locationX, int locationY, String attributeName, String newProbability)
        {
            String oldProbability = GetAttributeAtLocation(locationX, locationY, attributeName);

            // Cas l'attribut n'existe pas
            if (oldProbability == null || oldProbability == "")
            {
                AddOrChangeAttribute(locationX, locationY, attributeName, newProbability);
                return;
            }

            // Cas la probabilité était de 0
            if (oldProbability == "0")
            {
                return;
            }

            // Cas où la probabilité n'était pas de 0

            // Cas où la nouvelle probabilité est de 0
            if (newProbability == "0")
            {
                AddOrChangeAttribute(locationX, locationY, attributeName, newProbability);
            }
            else
            {
                int oldProba = Int32.Parse(oldProbability);
                int newProba = Int32.Parse(newProbability);

                if(newProba > oldProba)
                {
                    AddOrChangeAttribute(locationX, locationY, attributeName, newProbability);
                }
            }
        }
    }
}
