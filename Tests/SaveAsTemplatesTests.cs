using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
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
        static TECManufacturer expectedManufacturer;
        static TECTag expectedTag;

        static string path;

        static TECTemplates actualTemplates;
        static TECSystem actualSystem;
        static TECEquipment actualEquipment;
        static TECSubScope actualSubScope;
        static TECDevice actualDevice;
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
        public static void ClassInitialize(TestContext TestContext)
        {
            //Arrange
            expectedTemplates = TestHelper.CreateTestTemplates();
            expectedSystem = expectedTemplates.SystemTemplates[0];
            expectedEquipment = expectedTemplates.EquipmentTemplates[0];
            expectedSubScope = expectedTemplates.SubScopeTemplates[0];
            expectedDevice = expectedTemplates.DeviceCatalog[0];
            expectedManufacturer = expectedTemplates.ManufacturerCatalog[0];
            expectedTag = expectedTemplates.Tags[0];

            path = Path.GetTempFileName();

            //Act
            EstimatingLibraryDatabase.SaveTemplatesToNewDB(path, expectedTemplates);

            actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

            foreach (TECSystem sys in actualTemplates.SystemTemplates)
            {
                if (sys.Guid == expectedSystem.Guid)
                {
                    actualSystem = sys;
                    break;
                }
            }

            foreach (TECEquipment equip in actualTemplates.EquipmentTemplates)
            {
                if (equip.Guid == expectedEquipment.Guid)
                {
                    actualEquipment = equip;
                    break;
                }
            }

            foreach (TECSubScope ss in actualTemplates.SubScopeTemplates)
            {
                if (ss.Guid == expectedSubScope.Guid)
                {
                    actualSubScope = ss;
                    break;
                }
            }

            foreach (TECDevice dev in actualTemplates.DeviceCatalog)
            {
                if (dev.Guid == expectedDevice.Guid)
                {
                    actualDevice = dev;
                    break;
                }
            }

            foreach (TECManufacturer man in actualTemplates.ManufacturerCatalog)
            {
                if (man.Guid == expectedManufacturer.Guid)
                {
                    actualManufacturer = man;
                    break;
                }
            }

            foreach (TECTag tag in actualTemplates.Tags)
            {
                if (tag.Guid == expectedTag.Guid)
                {
                    actualTag = tag;
                    break;
                }
            }
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //File.Delete(path);
            Console.WriteLine("SaveAs test templates saved to: " + path);
        }

        [TestMethod]
        public void SaveAs_Templates_System()
        {
            //Arrange
            TECEquipment expectedSysEquipment = expectedSystem.Equipment[0];
            TECSubScope expectedSysSubScope = expectedSysEquipment.SubScope[0];
            TECDevice expectedChildDevice = expectedSysSubScope.Devices[0];
            TECPoint expectedSysPoint = expectedSysSubScope.Points[0];
            TECManufacturer expectedChildMan = expectedChildDevice.Manufacturer;

            TECEquipment actualSysEquipment = actualSystem.Equipment[0];
            TECSubScope actualSysSubScope = actualSysEquipment.SubScope[0];
            TECDevice actualChildDevice = actualSysSubScope.Devices[0];
            TECPoint actualSysPoint = actualSysSubScope.Points[0];
            TECManufacturer actualChildMan = actualChildDevice.Manufacturer;

            //Assert
            Assert.AreEqual(expectedSystem.Name, actualSystem.Name);
            Assert.AreEqual(expectedSystem.Description, actualSystem.Description);
            Assert.AreEqual(expectedSystem.BudgetPrice, actualSystem.BudgetPrice);
            Assert.AreEqual(expectedSystem.Tags[0].Text, actualSystem.Tags[0].Text);

            Assert.AreEqual(expectedSysEquipment.Name, actualSysEquipment.Name);
            Assert.AreEqual(expectedSysEquipment.Description, actualSysEquipment.Description);
            Assert.AreEqual(expectedSysEquipment.Quantity, actualSysEquipment.Quantity);
            Assert.AreEqual(expectedSysEquipment.BudgetPrice, actualSysEquipment.BudgetPrice);
            Assert.AreEqual(expectedSysEquipment.Tags[0].Text, actualSysEquipment.Tags[0].Text);

            Assert.AreEqual(expectedSysSubScope.Name, actualSysSubScope.Name);
            Assert.AreEqual(expectedSysSubScope.Description, actualSysSubScope.Description);
            Assert.AreEqual(expectedSysSubScope.Quantity, actualSysSubScope.Quantity);
            Assert.AreEqual(expectedSysSubScope.Tags[0].Text, actualSysSubScope.Tags[0].Text);

            Assert.AreEqual(expectedChildDevice.Name, actualChildDevice.Name);
            Assert.AreEqual(expectedChildDevice.Description, actualChildDevice.Description);
            Assert.AreEqual(expectedChildDevice.Cost, actualChildDevice.Cost);
            Assert.AreEqual(expectedChildDevice.Wire, actualChildDevice.Wire);
            Assert.AreEqual(expectedChildDevice.Tags[0].Text, actualChildDevice.Tags[0].Text);

            Assert.AreEqual(expectedSysPoint.Name, actualSysPoint.Name);
            Assert.AreEqual(expectedSysPoint.Description, actualSysPoint.Description);
            Assert.AreEqual(expectedSysPoint.Quantity, actualSysPoint.Quantity);
            Assert.AreEqual(expectedSysPoint.Type, actualSysPoint.Type);

            Assert.AreEqual(expectedChildMan.Name, actualChildMan.Name);
            Assert.AreEqual(expectedChildMan.Multiplier, actualChildMan.Multiplier);
        }

        [TestMethod]
        public void SaveAs_Templates_Equipment()
        {
            //Arrange
            TECSubScope actualEquipSubScope = actualEquipment.SubScope[0];
            TECDevice actualChildDevice = actualEquipSubScope.Devices[0];
            TECPoint actualEquipPoint = actualEquipSubScope.Points[0];
            TECManufacturer actualChildMan = actualChildDevice.Manufacturer;

            TECSubScope expectedEquipSubScope = expectedEquipment.SubScope[0];
            TECDevice expectedChildDevice = expectedEquipSubScope.Devices[0];
            TECPoint expectedEquipPoint = expectedEquipSubScope.Points[0];
            TECManufacturer expectedChildMan = expectedChildDevice.Manufacturer;

            //Assert
            Assert.AreEqual(expectedEquipment.Name, actualEquipment.Name);
            Assert.AreEqual(expectedEquipment.Description, actualEquipment.Description);
            Assert.AreEqual(expectedEquipment.BudgetPrice, actualEquipment.BudgetPrice);
            Assert.AreEqual(expectedEquipment.Tags[0].Text, actualEquipment.Tags[0].Text);

            Assert.AreEqual(expectedEquipSubScope.Name, actualEquipSubScope.Name);
            Assert.AreEqual(expectedEquipSubScope.Description, actualEquipSubScope.Description);
            Assert.AreEqual(expectedEquipSubScope.Quantity, actualEquipSubScope.Quantity);
            Assert.AreEqual(expectedEquipSubScope.Tags[0].Text, actualEquipSubScope.Tags[0].Text);

            Assert.AreEqual(expectedChildDevice.Name, actualChildDevice.Name);
            Assert.AreEqual(expectedChildDevice.Description, actualChildDevice.Description);
            Assert.AreEqual(expectedChildDevice.Cost, actualChildDevice.Cost);
            Assert.AreEqual(expectedChildDevice.Wire, actualChildDevice.Wire);
            Assert.AreEqual(expectedChildDevice.Tags[0].Text, actualChildDevice.Tags[0].Text);

            Assert.AreEqual(expectedEquipPoint.Name, actualEquipPoint.Name);
            Assert.AreEqual(expectedEquipPoint.Description, actualEquipPoint.Description);
            Assert.AreEqual(expectedEquipPoint.Quantity, actualEquipPoint.Quantity);
            Assert.AreEqual(expectedEquipPoint.Type, actualEquipPoint.Type);

            Assert.AreEqual(expectedChildMan.Name, actualChildMan.Name);
            Assert.AreEqual(expectedChildMan.Multiplier, actualChildMan.Multiplier);
        }

        [TestMethod]
        public void SaveAs_Templates_SubScope()
        {
            //Arrange
            TECDevice actualChildDevice = actualSubScope.Devices[0];
            TECPoint actualSSPoint = actualSubScope.Points[0];
            TECManufacturer actualChildMan = actualChildDevice.Manufacturer;

            TECDevice expectedChildDevice = expectedSubScope.Devices[0];
            TECPoint expectedSSPoint = expectedSubScope.Points[0];
            TECManufacturer expectedChildMan = expectedChildDevice.Manufacturer;

            //Assert
            Assert.AreEqual(expectedSubScope.Name, actualSubScope.Name);
            Assert.AreEqual(expectedSubScope.Description, actualSubScope.Description);
            Assert.AreEqual(expectedSubScope.Tags[0].Text, actualSubScope.Tags[0].Text);

            Assert.AreEqual(expectedChildDevice.Name, actualChildDevice.Name);
            Assert.AreEqual(expectedChildDevice.Description, actualChildDevice.Description);
            Assert.AreEqual(expectedChildDevice.Cost, actualChildDevice.Cost);
            Assert.AreEqual(expectedChildDevice.Wire, actualChildDevice.Wire);
            Assert.AreEqual(expectedChildDevice.Tags[0].Text, actualChildDevice.Tags[0].Text);

            Assert.AreEqual(expectedSSPoint.Name, actualSSPoint.Name);
            Assert.AreEqual(expectedSSPoint.Description, actualSSPoint.Description);
            Assert.AreEqual(expectedSSPoint.Quantity, actualSSPoint.Quantity);
            Assert.AreEqual(expectedSSPoint.Type, actualSSPoint.Type);

            Assert.AreEqual(expectedChildMan.Name, actualChildMan.Name);
            Assert.AreEqual(expectedChildMan.Multiplier, actualChildMan.Multiplier);
        }

        [TestMethod]
        public void SaveAs_Templates_Device()
        {
            //Arrange
            TECManufacturer actualChildMan = actualDevice.Manufacturer;

            TECManufacturer expectedChildMan = expectedDevice.Manufacturer;

            //Assert
            Assert.AreEqual(expectedDevice.Name, actualDevice.Name);
            Assert.AreEqual(expectedDevice.Description, actualDevice.Description);
            Assert.AreEqual(expectedDevice.Cost, actualDevice.Cost);
            Assert.AreEqual(expectedDevice.Wire, actualDevice.Wire);
            Assert.AreEqual(expectedDevice.Tags[0].Text, actualDevice.Tags[0].Text);

            Assert.AreEqual(expectedChildMan.Name, actualChildMan.Name);
            Assert.AreEqual(expectedChildMan.Multiplier, actualChildMan.Multiplier);
        }

        [TestMethod]
        public void SaveAs_Templates_Manufacturer()
        {
            //Assert
            Assert.AreEqual(expectedManufacturer.Name, actualManufacturer.Name);
            Assert.AreEqual(expectedManufacturer.Multiplier, actualManufacturer.Multiplier);
        }

        [TestMethod]
        public void SaveAs_Templates_Tag()
        {
            //Assert
            Assert.AreEqual(expectedTag.Text, actualTag.Text);
        }
    }
}
