using EstimatingLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class SaveAsTemplatesTests
    {
        static TECTemplates expectedTemplates;
        static TECSystem expectedSystem;
        static TECEquipment expectedEquipment;
        static TECSubScope expectedSubScope;
        static TECDevice expectedDevice;
        static TECPoint expectedPoint;
        static TECManufacturer expectedManufacturer;
        static TECTag expectedTag;

        static string path;

        static TECTemplates actualTemplates;
        static TECSystem actualSystem;
        static TECEquipment actualEquipment;
        static TECSubScope actualSubScope;
        static TECDevice actualDevice;
        static TECPoint actualPoint;
        static TECManufacturer actualManufacturer;
        static TECTag actualTag;

        private TestContext testContextInstance;
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

        [ClassInitialize]
        public static void ClassInitialize()
        {
            //Arrange

        }
    }
}
