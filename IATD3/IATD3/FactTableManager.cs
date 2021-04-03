using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace IATD3
{
    public static class FactTableManager
    {
        #region Constants

        private const string _path = @"../../facts.xml";

        #endregion

        #region Public methods

        /// <summary>
        /// Creates the fact file.
        /// </summary>
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

        /// <summary>
        /// Adds or replaces the fact at the specified location.
        /// </summary>
        /// <param name="factName">Name of the fact.</param>
        /// <param name="locationX">The location x.</param>
        /// <param name="locationY">The location y.</param>
        /// <param name="newAttributes">The new attributes.</param>
        /// <param name="newFactName">New name of the fact.</param>
        public static void AddOrReplaceFactAtLocation(
            string factName,
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

        /// <summary>
        /// Gets the attribute of a fact at the given location.
        /// </summary>
        /// <param name="factName">Name of the fact.</param>
        /// <param name="locationX">The location x.</param>
        /// <param name="locationY">The location y.</param>
        /// <param name="attribute">The attribute.</param>
        /// <returns>The attribute</returns>
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

        /// <summary>
        /// Gets the attribute at location.
        /// </summary>
        /// <param name="locationX">The location x.</param>
        /// <param name="locationY">The location y.</param>
        /// <param name="attribute">The attribute.</param>
        /// <returns>The attributes.</returns>
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

        /// <summary>
        /// Determines whether a fact with the given name and attributes is in the table already.
        /// </summary>
        /// <param name="factName">Name of the fact.</param>
        /// <param name="locationX">The location x.</param>
        /// <param name="locationY">The location y.</param>
        /// <param name="attributes">The attributes.</param>
        /// <returns>
        ///   <c>true</c> if the specified facts are in the table; otherwise, <c>false</c>.
        /// </returns>
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

            XmlNode element = xmldoc.SelectSingleNode(xpath);
            fs.Close();

            return (element != null);
        }

        /// <summary>
        /// Changes the inner text at location.
        /// </summary>
        /// <param name="newInnerText">The new inner text.</param>
        /// <param name="locationX">The location x.</param>
        /// <param name="locationY">The location y.</param>
        /// <returns>an opcode depending on the execution of the function
        ///  - -1 if the fact doesn't exist
        ///  - 0 if the fact exists already have the given text
        ///  - 1 if the fact is updated.
        /// 
        ///</returns>
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

        /// <summary>
        /// Updates the probability.
        /// </summary>
        /// <param name="locationX">The location x.</param>
        /// <param name="locationY">The location y.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="newProbability">The new probability.</param>
        public static void UpdateProbability(int locationX, int locationY, String attributeName, String newProbability)
        {
            String oldProbability = GetAttributeAtLocation(locationX, locationY, attributeName);

            // Cas où l'attribut n'existe pas
            if (oldProbability == null || oldProbability == "")
            {
                AddOrChangeAttribute(locationX, locationY, attributeName, newProbability);
                return;
            }

            // Cas où la probabilité était de 0
            if (oldProbability == "0")
            {
                return;
            }

            // Cas où la nouvelle probabilité est de 0
            if (newProbability == "0")
            {
                AddOrChangeAttribute(locationX, locationY, attributeName, newProbability);
            }
            else
            {
                int oldProba = Convert.ToInt32(oldProbability);
                int newProba = Convert.ToInt32(newProbability);

                if (newProba > oldProba)
                {
                    AddOrChangeAttribute(locationX, locationY, attributeName, newProbability);
                }
            }
        }

        /// <summary>
        /// Adds or changes the attribute.
        /// </summary>
        /// <param name="locationX">The location x.</param>
        /// <param name="locationY">The location y.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// Returns an opcode depending on the execution of the function :
        ///  - -1 if the fact doesn't exist
        ///  - 0 if the fact exists and do not already have the attribute
        ///  - 1 otherwise (if the fact exists with the attribute).
        /// </returns>
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

        #endregion

        #region Private methods

        /// <summary>
        /// Adds the fact to the fact table.
        /// </summary>
        /// <param name="factName">Name of the fact.</param>
        /// <param name="attributes">The attributes.</param>
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

        #endregion
    }
}
