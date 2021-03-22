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
    class FactTableManagerTests
    {
        public FactTableManagerTests()
        {
            //
            // TODO: ajoutez ici la logique du constructeur
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Obtient ou définit le contexte de test qui fournit
        ///des informations sur la série de tests active, ainsi que ses fonctionnalités.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod]
        public void T_CreateXml()
        {
            FactTableManager.CreateFactFile();
            FileStream fs = new FileStream(@"../../facts.xml", FileMode.Open, FileAccess.Read);
        }


    }
}
