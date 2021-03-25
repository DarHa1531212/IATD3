using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IATD3;
using System.Xml;
using System.IO;

namespace IATD3Tests
{
    
    /// <summary>
    /// Description résumée pour CellTests
    /// </summary>
    [TestClass]
    public class FactTableManagerTests
    {
        
        [TestMethod]
        public void T_CreateXml()
        {
            FactTableManager.CreateFactFile();
            FileStream fs = new FileStream(@"../../facts.xml", FileMode.Open, FileAccess.Read);
            Assert.IsTrue(1 == 1);
        }


    }
}
