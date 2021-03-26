using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IATD3;
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace IATD3Tests
{
    
    /// <summary>
    /// Description résumée pour CellTests
    /// </summary>
    [TestClass]
    public class FactTableManagerTests
    {
        #region IsAttributeInTable
        [TestMethod]
        public void T_IsAttributeInTable_True()
        {
            XmlDocument xmlDoc = new XmlDocument();
            FileStream fs = new FileStream(@"../../facts.xml", FileMode.Create, FileAccess.ReadWrite);
            xmlDoc.LoadXml("<FactTable>\n" +
                "\t<Fact locationX='2' locationY='1' presence='false' value='high'>Monster</Fact>\n" +
                "\t<Fact locationX='1' locationY='0' presence='true'>Odour</Fact>\n" +
                "</FactTable>");
            fs.SetLength(0);
            xmlDoc.Save(fs);
            fs.Close();

            Dictionary<string, string> attributes = new Dictionary<string, string>();
            attributes.Add("presence", "false");
            attributes.Add("value", "high");
            bool isAttributeIn = FactTableManager.IsAttributeInTable("Monster", 2, 1, attributes);

            Assert.IsTrue(isAttributeIn);
        }

        [TestMethod]
        public void T_IsAttributeInTable_NoAttributeName()
        {
            XmlDocument xmlDoc = new XmlDocument();
            FileStream fs = new FileStream(@"../../facts.xml", FileMode.Create, FileAccess.ReadWrite);
            xmlDoc.LoadXml("<FactTable>\n" +
                "\t<Fact locationX='2' locationY='1' presence='false' value='high'>Monster</Fact>\n" +
                "\t<Fact locationX='1' locationY='0' presence='true'>Odour</Fact>\n" +
                "</FactTable>");
            fs.SetLength(0);
            xmlDoc.Save(fs);
            fs.Close();

            Dictionary<string, string> attributes = new Dictionary<string, string>();
            attributes.Add("presence", "false");
            attributes.Add("value", "high");
            bool isAttributeIn = FactTableManager.IsAttributeInTable("Fact", 2, 1, attributes);

            Assert.IsFalse(isAttributeIn);
        }

        [TestMethod]
        public void T_IsAttributeInTable_WrongXLocation()
        {
            XmlDocument xmlDoc = new XmlDocument();
            FileStream fs = new FileStream(@"../../facts.xml", FileMode.Create, FileAccess.ReadWrite);
            xmlDoc.LoadXml("<FactTable>\n" +
                "\t<Fact locationX='2' locationY='1' presence='false' value='high'>Monster</Fact>\n" +
                "\t<Fact locationX='1' locationY='0' presence='true'>Odour</Fact>\n" +
                "</FactTable>");
            fs.SetLength(0);
            xmlDoc.Save(fs);
            fs.Close();

            Dictionary<string, string> attributes = new Dictionary<string, string>();
            attributes.Add("presence", "true");
            bool isAttributeIn = FactTableManager.IsAttributeInTable("Odour", 2, 0, attributes);

            Assert.IsFalse(isAttributeIn);
        }

        [TestMethod]
        public void T_IsAttributeInTable_WrongYLocation()
        {
            XmlDocument xmlDoc = new XmlDocument();
            FileStream fs = new FileStream(@"../../facts.xml", FileMode.Create, FileAccess.ReadWrite);
            xmlDoc.LoadXml("<FactTable>\n" +
                "\t<Fact locationX='2' locationY='1' presence='false' value='high'>Monster</Fact>\n" +
                "\t<Fact locationX='1' locationY='0' presence='true' velocity='slow'>Wind</Fact>\n" +
                "</FactTable>");
            fs.SetLength(0);
            xmlDoc.Save(fs);
            fs.Close();

            Dictionary<string, string> attributes = new Dictionary<string, string>();
            attributes.Add("presence", "true");
            attributes.Add("velocity", "slow");
            bool isAttributeIn = FactTableManager.IsAttributeInTable("Wind", 1, 2, attributes);

            Assert.IsFalse(isAttributeIn);
        }

        [TestMethod]
        public void T_IsAttributeInTable_WrongAttributes() {
            XmlDocument xmlDoc = new XmlDocument();
            FileStream fs = new FileStream(@"../../facts.xml", FileMode.Create, FileAccess.ReadWrite);
            xmlDoc.LoadXml("<FactTable>\n" +
                "\t<Fact locationX='2' locationY='1' presence='false' value='high'>Monster</Fact>\n" +
                "\t<Fact locationX='1' locationY='0' presence='true' velocity='slow'>Wind</Fact>\n" +
                "</FactTable>");
            fs.SetLength(0);
            xmlDoc.Save(fs);
            fs.Close();

            Dictionary<string, string> attributes = new Dictionary<string, string>();
            attributes.Add("presence", "true");
            attributes.Add("velocity", "fast");
            bool isAttributeIn = FactTableManager.IsAttributeInTable("Wind", 1, 0, attributes);

            Assert.IsFalse(isAttributeIn);
        }
        #endregion

        #region GetAttributeOfFactAtLocation

        [TestMethod]
        public void T_GetAttributeOfFactAtLocation_NoFactName()
        {
            XmlDocument xmlDoc = new XmlDocument();
            FileStream fs = new FileStream(@"../../facts.xml", FileMode.Create, FileAccess.ReadWrite);
            xmlDoc.LoadXml("<FactTable>\n" +
                "\t<Fact locationX='2' locationY='1' presence='false' value='high'>Monster</Fact>\n" +
                "\t<Fact locationX='1' locationY='0' presence='true'>Odour</Fact>\n" +
                "</FactTable>");
            fs.SetLength(0);
            xmlDoc.Save(fs);
            fs.Close();

            string attributeValue = FactTableManager.GetAttributeOfFactAtLocation("Wind", 2, 1, "value");

            Assert.IsNull(attributeValue);
        }

        [TestMethod]
        public void T_GetAttributeOfFactAtLocation_NoXLocation()
        {
            XmlDocument xmlDoc = new XmlDocument();
            FileStream fs = new FileStream(@"../../facts.xml", FileMode.Create, FileAccess.ReadWrite);
            xmlDoc.LoadXml("<FactTable>\n" +
                "\t<Fact locationX='2' locationY='1' presence='false' value='high'>Monster</Fact>\n" +
                "\t<Fact locationX='1' locationY='0' presence='true'>Odour</Fact>\n" +
                "</FactTable>");
            fs.SetLength(0);
            xmlDoc.Save(fs);
            fs.Close();

            string attributeValue = FactTableManager.GetAttributeOfFactAtLocation("Odour", 0, 0, "presence");

            Assert.IsNull(attributeValue);
        }

        [TestMethod]
        public void T_GetAttributeOfFactAtLocation_NoYLocation()
        {
            XmlDocument xmlDoc = new XmlDocument();
            FileStream fs = new FileStream(@"../../facts.xml", FileMode.Create, FileAccess.ReadWrite);
            xmlDoc.LoadXml("<FactTable>\n" +
                "\t<Fact locationX='3' locationY='6' presence='false' zone='forest'>Portal</Fact>\n" +
                "</FactTable>");
            fs.SetLength(0);
            xmlDoc.Save(fs);
            fs.Close();

            string attributeValue = FactTableManager.GetAttributeOfFactAtLocation("Portal", 3, 2, "zone");

            Assert.IsNull(attributeValue);
        }

        [TestMethod]
        public void T_GetAttributeOfFactAtLocation_NoAttribute()
        {
            XmlDocument xmlDoc = new XmlDocument();
            FileStream fs = new FileStream(@"../../facts.xml", FileMode.Create, FileAccess.ReadWrite);
            xmlDoc.LoadXml("<FactTable>\n" +
                "\t<Fact locationX='1' locationY='4' presence='false'>Monster</Fact>\n" +
                "\t<Fact locationX='2' locationY='2' presence='true'>Odour</Fact>\n" +
                "\t<Fact locationX='0' locationY='0' presence='true' velocity='low'>Wind</Fact>\n" +
                "</FactTable>");
            fs.SetLength(0);
            xmlDoc.Save(fs);
            fs.Close();

            string attributeValue = FactTableManager.GetAttributeOfFactAtLocation("Monster", 1, 4, "zone");

            Assert.IsNull(attributeValue);
        }

        [TestMethod]
        public void T_GetAttributeOfFactAtLocation_Complete()
        {
            XmlDocument xmlDoc = new XmlDocument();
            FileStream fs = new FileStream(@"../../facts.xml", FileMode.Create, FileAccess.ReadWrite);
            xmlDoc.LoadXml("<FactTable>\n" +
                "\t<Fact locationX='1' locationY='4' presence='false'>Monster</Fact>\n" +
                "\t<Fact locationX='2' locationY='2' presence='true'>Odour</Fact>\n" +
                "\t<Fact locationX='0' locationY='0' presence='true' velocity='low'>Wind</Fact>\n" +
                "</FactTable>");
            fs.SetLength(0);
            xmlDoc.Save(fs);
            fs.Close();

            string attributeValue = FactTableManager.GetAttributeOfFactAtLocation("Odour", 2, 2, "presence");

            string expectedResult = "true";
            Assert.AreEqual(attributeValue, expectedResult);
        }
        #endregion

        #region AddOrReplaceFactAtLocation
        [TestMethod]
        public void T_AddOrReplaceFactAtLocation_NewFactNoAttributes()
        {

            XmlDocument xmldoc = new XmlDocument();
            FileStream fs = new FileStream(@"../../facts.xml", FileMode.Open, FileAccess.ReadWrite);
            xmldoc.LoadXml("<FactTable><Facts></Facts></FactTable>");
            fs.SetLength(0);
            xmldoc.Save(fs);
            fs.Close();

            FactTableManager.AddOrReplaceFactAtLocation("Monster", 2, 1, new Dictionary<string, string>());

            string text = File.ReadAllText(@"../../facts.xml");
            text = text.Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace("\"", "'").Replace(" ", "");

            string expectedXml = "<FactTable><Facts><FactlocationX='2'locationY='1'>Monster</Fact></Facts></FactTable>";

            Assert.AreEqual(text, expectedXml);

        }
        [TestMethod]
        public void T_AddOrReplaceFactAtLocation_NewFactAttributes()
        {

            XmlDocument xmldoc = new XmlDocument();
            FileStream fs = new FileStream(@"../../facts.xml", FileMode.Open, FileAccess.ReadWrite);
            xmldoc.LoadXml("<FactTable><Facts></Facts></FactTable>");
            fs.SetLength(0);
            xmldoc.Save(fs);
            fs.Close();

            Dictionary<string, string> attributes = new Dictionary<string, string>();
            attributes.Add("presence", "true");
            attributes.Add("value", "high");

            FactTableManager.AddOrReplaceFactAtLocation("Portal", 1, 4, attributes);

            string text = File.ReadAllText(@"../../facts.xml");
            text = text.Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace("\"", "'").Replace(" ", "");

            string expectedXml = "<FactTable><Facts><Factpresence='true'value='high'locationX='1'locationY='4'>Portal</Fact></Facts></FactTable>";

            Assert.AreEqual(text, expectedXml);
        }

        [TestMethod]
        public void T_AddOrReplaceFactAtLocation_ReplaceFactNoAttributes()
        {

            XmlDocument xmldoc = new XmlDocument();
            FileStream fs = new FileStream(@"../../facts.xml", FileMode.Open, FileAccess.ReadWrite);
            xmldoc.LoadXml("<FactTable><Facts><Fact locationX='2' locationY='3'>Portal</Fact></Facts></FactTable>");
            fs.SetLength(0);
            xmldoc.Save(fs);
            fs.Close();

            Dictionary<string, string> attributes = new Dictionary<string, string>();

            FactTableManager.AddOrReplaceFactAtLocation("Portal", 2, 3, attributes);

            string text = File.ReadAllText(@"../../facts.xml");
            text = text.Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace("\"", "'").Replace(" ", "");

            string expectedXml = "<FactTable><Facts><FactlocationX='2'locationY='3'>Portal</Fact></Facts></FactTable>";

            Assert.AreEqual(text, expectedXml);
        }
        #endregion
    }
}
