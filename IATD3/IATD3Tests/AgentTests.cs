using IATD3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace IATD3Tests
{
    /// <summary>
    /// Description résumée pour AgentTests
    /// </summary>
    [TestClass]
    public class AgentTests
    {
        #region GetSafetyProbability
        [TestMethod]
        public void T_GetSafetyProbability_Safe()
        {
            cAgent agent = new cAgent(new cEnvironment());
            PrivateObject po = new PrivateObject(agent);

            XmlDocument xmldoc = new XmlDocument();
            FileStream fs = new FileStream(@"../../facts.xml", FileMode.Open, FileAccess.ReadWrite);
            xmldoc.LoadXml("<FactTable><Facts><Fact locationX='4' locationY='0' isSafe='True'>Scope</Fact></Facts></FactTable>");
            fs.SetLength(0);
            xmldoc.Save(fs);
            fs.Close();

            object safety = po.Invoke("GetSafetyProbability", new object[] { 4, 0 });

            Assert.AreEqual((float)safety, 100f);
        }

        [TestMethod]
        public void T_GetSafetyProbability_NoAttribute()
        {
            cAgent agent = new cAgent(new cEnvironment());
            PrivateObject po = new PrivateObject(agent);

            XmlDocument xmldoc = new XmlDocument();
            FileStream fs = new FileStream(@"../../facts.xml", FileMode.Open, FileAccess.ReadWrite);
            xmldoc.LoadXml("<FactTable><Facts><Fact locationX='2' locationY='3' >Scope</Fact></Facts></FactTable>");
            fs.SetLength(0);
            xmldoc.Save(fs);
            fs.Close();

            object safety = po.Invoke("GetSafetyProbability", new object[] { 2, 3 });

            Assert.AreEqual((float)safety, 100f);
        }

        [TestMethod]
        public void T_GetSafetyProbability_NoMonster()
        {

            cAgent agent = new cAgent(new cEnvironment());
            PrivateObject po = new PrivateObject(agent);

            XmlDocument xmldoc = new XmlDocument();
            FileStream fs = new FileStream(@"../../facts.xml", FileMode.Open, FileAccess.ReadWrite);
            xmldoc.LoadXml("<FactTable><Facts><Fact locationX='1' locationY='6' hasAbyss='True' hasAbyssProbability='25'>Scope</Fact></Facts></FactTable>");
            fs.SetLength(0);
            xmldoc.Save(fs);
            fs.Close();

            object safety = po.Invoke("GetSafetyProbability", new object[] { 1, 6 });

            Assert.AreEqual((float)safety, 75f);
        }

        [TestMethod]
        public void T_GetSafetyProbability_NoAbyss()
        {

            cAgent agent = new cAgent(new cEnvironment());
            PrivateObject po = new PrivateObject(agent);

            XmlDocument xmldoc = new XmlDocument();
            FileStream fs = new FileStream(@"../../facts.xml", FileMode.Open, FileAccess.ReadWrite);
            xmldoc.LoadXml("<FactTable><Facts><Fact locationX='0' locationY='0' hasMonster='True' hasMonsterProbability='50'>Scope</Fact></Facts></FactTable>");
            fs.SetLength(0);
            xmldoc.Save(fs);
            fs.Close();

            object safety = po.Invoke("GetSafetyProbability", new object[] { 0, 0 });

            Assert.AreEqual((float)safety, 50f);
        }

        [TestMethod]
        public void T_GetSafetyProbability_Both()
        {

            cAgent agent = new cAgent(new cEnvironment());
            PrivateObject po = new PrivateObject(agent);

            XmlDocument xmldoc = new XmlDocument();
            FileStream fs = new FileStream(@"../../facts.xml", FileMode.Open, FileAccess.ReadWrite);
            xmldoc.LoadXml("<FactTable><Facts><Fact locationX='2' locationY='5' hasMonster='True' hasMonsterProbability='50' " +
                "hasAbyss='True' hasAbyssProbability='25'>Scope</Fact></Facts></FactTable>");
            fs.SetLength(0);
            xmldoc.Save(fs);
            fs.Close();

            object safety = po.Invoke("GetSafetyProbability", new object[] { 2, 5 });

            Assert.AreEqual((float)safety, 37.5f);
        }

        #endregion
    }
}
