using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EstimatingLibrary;
using System.Collections.ObjectModel;

namespace Tests
{
    /// <summary>
    /// Summary description for EstimatorTests
    /// </summary>
    [TestClass]
    public class EstimatorTests
    {

        public EstimatorTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
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

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
        }

        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Estimate_MiscCost()
        {
            var bid = new TECBid();
            
            var tecMisc = new TECMisc();
            tecMisc.Cost = 1234;
            tecMisc.Labor = 4321;
            tecMisc.Type = CostType.TEC;

            var eMisc = new TECMisc();
            eMisc.Cost = 5678;
            eMisc.Labor = 8765;
            eMisc.Type = CostType.Electrical;

            bid.MiscCosts.Add(tecMisc);
            bid.MiscCosts.Add(eMisc);

            Assert.AreEqual(1234, bid.Estimate.TECMaterialCost, "Material cost not added");
            Assert.AreEqual(5678, bid.Estimate.ElectricalMaterialCost, "Electrical Material cost not added");
            Assert.AreEqual(4321, bid.Estimate.TECLaborHours, "Labor hours not added");
            Assert.AreEqual(8765, bid.Estimate.ElectricalLaborHours, "Electrical labor hours not added");
        }

        [TestMethod]
        public void Estimate_MiscCostFromSystem()
        {
            var bid = new TECBid();
            var system = new TECSystem();
            bid.Systems.Add(system);
            system.AddInstance(bid);
            system.AddInstance(bid);

            var tecMisc = new TECMisc();
            tecMisc.Cost = 1234;
            tecMisc.Labor = 4321;
            tecMisc.Type = CostType.TEC;

            var eMisc = new TECMisc();
            eMisc.Cost = 5678;
            eMisc.Labor = 8765;
            eMisc.Type = CostType.Electrical;

            system.MiscCosts.Add(tecMisc);
            system.MiscCosts.Add(eMisc);

            Assert.AreEqual(2468, bid.Estimate.TECMaterialCost, "Material cost not added");
            Assert.AreEqual(11356, bid.Estimate.ElectricalMaterialCost, "Electrical Material cost not added");
            Assert.AreEqual(8642, bid.Estimate.TECLaborHours, "Labor hours not added");
            Assert.AreEqual(17530, bid.Estimate.ElectricalLaborHours, "Electrical labor hours not added");
        }

        [TestMethod]
        public void Estimate_AddDeviceToSubScope()
        {
            var bid = new TECBid();
            var system = new TECSystem();
            var equipment = new TECEquipment();
            var subScope = new TECSubScope();

            equipment.SubScope.Add(subScope);
            system.Equipment.Add(equipment);
            bid.Systems.Add(system);
            system.AddInstance(bid);
            system.AddInstance(bid);

            var manufacturer = new TECManufacturer();
            manufacturer.Multiplier = 1;

            var device = new TECDevice(new ObservableCollection<TECConnectionType> { new TECConnectionType() }, manufacturer);
            device.Cost = 100;
            bid.Catalogs.Devices.Add(device);

            subScope.Devices.Add(device);
            
            Assert.AreEqual(200, bid.Estimate.TECMaterialCost, "Material cost not added");
        }

        [TestMethod]
        public void Estimate_AddConnectionToSubScope()
        {
            var bid = new TECBid();
            var system = new TECSystem();
            var equipment = new TECEquipment();
            var subScope = new TECSubScope();

            var manufacturer = new TECManufacturer();
            manufacturer.Multiplier = 1;

            var controller = new TECController(manufacturer);
            
            equipment.SubScope.Add(subScope);
            system.Equipment.Add(equipment);
            bid.Systems.Add(system);
            
            var connectionType = new TECConnectionType();
            connectionType.Cost = 1;
            connectionType.Labor = 1;
            var conduitType = new TECConduitType();
            conduitType.Cost = 1;
            conduitType.Labor = 1;

            var device = new TECDevice(new ObservableCollection<TECConnectionType> { connectionType }, manufacturer);
            bid.Catalogs.Devices.Add(device);

            subScope.Devices.Add(device);

            var connection = controller.AddSubScope(subScope);
            connection.Length = 10;
            connection.ConduitLength = 5;
            connection.ConduitType = conduitType;
            
            system.AddInstance(bid);
            system.AddInstance(bid);

            Assert.AreEqual(30, bid.Estimate.ElectricalLaborHours, "Electrical Labor Not Updating");
            Assert.AreEqual(30, bid.Estimate.ElectricalMaterialCost, "Electrical Material Not Updating");
        }
    }
}
