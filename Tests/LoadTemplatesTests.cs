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
    public class LoadTemplatesTests
    {
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
            actualTemplates = TestHelper.LoadTestTemplates(TestHelper.StaticTestTemplatesPath);

            actualSystem = actualTemplates.SystemTemplates[0];

            actualEquipment = actualTemplates.EquipmentTemplates[0];

            actualSubScope = actualTemplates.SubScopeTemplates[0];

            actualDevice = null;
            foreach (TECDevice dev in actualTemplates.DeviceCatalog)
            {
                if (dev.Name == "Test Device") actualDevice = dev;
            }

            actualManufacturer = null;
            foreach (TECManufacturer man in actualTemplates.ManufacturerCatalog)
            {
                if (man.Name == "Test Manufacturer") actualManufacturer = man;
            }

            actualTag = null;
            foreach (TECTag tag in actualTemplates.Tags)
            {
                if (tag.Text == "Test Tag") actualTag = tag;
            }
        }

        [TestMethod]
        public void Load_Templates_System()
        {
            //Arrange
            TECEquipment sysEquipment = actualSystem.Equipment[0];
            TECSubScope sysSubScope = sysEquipment.SubScope[0];
            TECDevice childDevice = sysSubScope.Devices[0];
            TECPoint sysPoint = sysSubScope.Points[0];
            TECManufacturer childMan = childDevice.Manufacturer;

            //Assert
            Assert.AreEqual("Test System", actualSystem.Name);
            Assert.AreEqual("System Description", actualSystem.Description);
            Assert.AreEqual(12.3, actualSystem.BudgetPrice);
            Assert.AreEqual("System Tag", actualSystem.Tags[0].Text);

            Assert.AreEqual("System Equipment", sysEquipment.Name);
            Assert.AreEqual("Child Equipment", sysEquipment.Description);
            Assert.AreEqual(654, sysEquipment.Quantity);
            Assert.AreEqual(65.4, sysEquipment.BudgetPrice);
            Assert.AreEqual("Equipment Tag", sysEquipment.Tags[0].Text);

            Assert.AreEqual("System SubScope", sysSubScope.Name);
            Assert.AreEqual("Child SubScope", sysSubScope.Description);
            Assert.AreEqual(486, sysSubScope.Quantity);
            Assert.AreEqual("SubScope Tag", sysSubScope.Tags[0].Text);

            Assert.AreEqual("Child Device", childDevice.Name);
            Assert.AreEqual("Child Device", childDevice.Description);
            Assert.AreEqual(89.3, childDevice.Cost);
            Assert.AreEqual("Test Wire (Child)", childDevice.ConnectionType);
            Assert.AreEqual("Device Tag", childDevice.Tags[0].Text);

            Assert.AreEqual("System Point", sysPoint.Name);
            Assert.AreEqual("Child Point", sysPoint.Description);
            Assert.AreEqual(34, sysPoint.Quantity);
            Assert.AreEqual(PointTypes.Serial, sysPoint.Type);

            Assert.AreEqual("Child Manufacturer (Child Device)", childMan.Name);
            Assert.AreEqual(0.3, childMan.Multiplier);
        }

        [TestMethod]
        public void Load_Templates_Equipment()
        {
            //Arrange
            TECSubScope equipSubScope = actualEquipment.SubScope[0];
            TECDevice childDevice = equipSubScope.Devices[0];
            TECPoint equipPoint = equipSubScope.Points[0];
            TECManufacturer childMan = childDevice.Manufacturer;

            //Assert
            Assert.AreEqual("Test Equipment", actualEquipment.Name);
            Assert.AreEqual("Equipment Description", actualEquipment.Description);
            Assert.AreEqual(64.1, actualEquipment.BudgetPrice);
            Assert.AreEqual("Equipment Tag", actualEquipment.Tags[0].Text);

            Assert.AreEqual("Equipment SubScope", equipSubScope.Name);
            Assert.AreEqual("Child SubScope", equipSubScope.Description);
            Assert.AreEqual(346, equipSubScope.Quantity);
            Assert.AreEqual("SubScope Tag", equipSubScope.Tags[0].Text);

            Assert.AreEqual("Child Device", childDevice.Name);
            Assert.AreEqual("Child Device", childDevice.Description);
            Assert.AreEqual(89.3, childDevice.Cost);
            Assert.AreEqual("Test Wire (Child)", childDevice.ConnectionType);
            Assert.AreEqual("Device Tag", childDevice.Tags[0].Text);

            Assert.AreEqual("Equipment Point", equipPoint.Name);
            Assert.AreEqual("Child Point", equipPoint.Description);
            Assert.AreEqual(81, equipPoint.Quantity);
            Assert.AreEqual(PointTypes.AI, equipPoint.Type);

            Assert.AreEqual("Child Manufacturer (Child Device)", childMan.Name);
            Assert.AreEqual(0.3, childMan.Multiplier);
        }

        [TestMethod]
        public void Load_Templates_SubScope()
        {
            //Arrange
            TECDevice childDevice = actualSubScope.Devices[0];
            TECPoint ssPoint = actualSubScope.Points[0];
            TECManufacturer childMan = childDevice.Manufacturer;

            //Assert
            Assert.AreEqual("Test SubScope", actualSubScope.Name);
            Assert.AreEqual("SubScope Description", actualSubScope.Description);
            Assert.AreEqual("SubScope Tag", actualSubScope.Tags[0].Text);

            Assert.AreEqual("Child Device", childDevice.Name);
            Assert.AreEqual("Child Device", childDevice.Description);
            Assert.AreEqual(89.3, childDevice.Cost);
            Assert.AreEqual("Test Wire (Child)", childDevice.ConnectionType);
            Assert.AreEqual("Device Tag", childDevice.Tags[0].Text);

            Assert.AreEqual("SubScope Point", ssPoint.Name);
            Assert.AreEqual("Child Point", ssPoint.Description);
            Assert.AreEqual(349, ssPoint.Quantity);
            Assert.AreEqual(PointTypes.BO, ssPoint.Type);

            Assert.AreEqual("Child Manufacturer (Child Device)", childMan.Name);
            Assert.AreEqual(0.3, childMan.Multiplier);
        }

        [TestMethod]
        public void Load_Templates_Device()
        {
            //Arrange
            TECManufacturer childMan = actualDevice.Manufacturer;

            //Assert
            Assert.AreEqual("Test Device", actualDevice.Name);
            Assert.AreEqual("Device Description", actualDevice.Description);
            Assert.AreEqual(72.9, actualDevice.Cost);
            Assert.AreEqual("Test Wire", actualDevice.ConnectionType);
            Assert.AreEqual("Device Tag", actualDevice.Tags[0].Text);

            Assert.AreEqual("Child Manufacturer (Test Device)", childMan.Name);
            Assert.AreEqual(0.123, childMan.Multiplier);
        }

        [TestMethod]
        public void Load_Templates_Manufacturer()
        {
            //Assert
            Assert.AreEqual("Test Manufacturer", actualManufacturer.Name);
            Assert.AreEqual(0.65, actualManufacturer.Multiplier);
        }

        [TestMethod]
        public void Load_Templates_Tag()
        {
            //Assert
            Assert.AreEqual("Test Tag", actualTag.Text);
        }
    }
}
