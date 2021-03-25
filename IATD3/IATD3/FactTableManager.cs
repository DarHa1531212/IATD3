﻿using System;
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
            foreach(string attribute in attributes.Keys)
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
            Dictionary<string, string> newAttributes)
        {
            XmlDocument xmldoc = new XmlDocument();
            FileStream fs = new FileStream(_path, FileMode.Open, FileAccess.ReadWrite);
            xmldoc.Load(fs);

            XmlNode element = xmldoc.SelectSingleNode("//Fact[@locationX='"+locationX.ToString()+
                "' and @locationY='"+locationY.ToString()+"' and text()='"+factName+"']");

            if(element == null)
            {
                fs.Close();
                newAttributes.Add("locationX", locationX.ToString());
                newAttributes.Add("locationY", locationY.ToString());
                AddFact(factName, newAttributes);
            } else
            {
                XmlElement xmlElement = element as XmlElement;
                foreach (string attribute in newAttributes.Keys)
                {
                    xmlElement.SetAttribute(attribute, newAttributes[attribute]);
                }
                fs.SetLength(0);
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

            XmlNode element = xmldoc.SelectSingleNode("//Fact[@locationX='" + locationX.ToString() +
                "' and @locationY='" + locationY.ToString() + "' and text()='" + factName + "']");

            if(element == null)
            {
                return "";
            }
            XmlElement xmlElement = element as XmlElement;
            return xmlElement.GetAttribute(attribute);
        }

        public static bool IsAttributeInTable(string factName, int locationX, int locationY, Dictionary<string, string> attributes)
        {
            XmlDocument xmldoc = new XmlDocument();
            FileStream fs = new FileStream(_path, FileMode.Open, FileAccess.ReadWrite);
            xmldoc.Load(fs);

            string xpath = "//Fact[@locationX='" + locationX.ToString() +
                "' and @locationY='" + locationY.ToString() + "'";

            foreach(string attribute in attributes.Keys)
            {
                xpath += " and @" + attribute + "='" + attributes[attribute] + "'";
            }

            xpath += " and text()='" + factName + "']";

            XmlNode element = xmldoc.SelectSingleNode(xpath);

            return (element != null);
        }
    }
}
