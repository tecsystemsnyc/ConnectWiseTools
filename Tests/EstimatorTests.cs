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
        public void Estimate_AssCostFromEquipemnt()
        {
            var bid = new TECBid();
            var system = new TECSystem();
            var equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            bid.Systems.Add(system);
            system.AddInstance(bid);
            system.AddInstance(bid);

            var tecCost = new TECCost();
            tecCost.Cost = 1234;
            tecCost.Labor = 4321;
            tecCost.Type = CostType.TEC;

            var eCost = new TECCost();
            eCost.Cost = 5678;
            eCost.Labor = 8765;
            eCost.Type = CostType.Electrical;

            equipment.AssociatedCosts.Add(tecCost);
            equipment.AssociatedCosts.Add(eCost);

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
            system.Controllers.Add(controller);
            bid.Systems.Add(system);

            var ratedCost = new TECCost();
            ratedCost.Cost = 1;
            ratedCost.Labor = 1;
            ratedCost.Type = CostType.Electrical;
            
            var connectionType = new TECConnectionType();
            connectionType.Cost = 1;
            connectionType.Labor = 1;
            connectionType.RatedCosts.Add(ratedCost);
            var conduitType = new TECConduitType();
            conduitType.Cost = 1;
            conduitType.Labor = 1;
            conduitType.RatedCosts.Add(ratedCost);

            var device = new TECDevice(new ObservableCollection<TECConnectionType> { connectionType }, manufacturer);
            bid.Catalogs.Devices.Add(device);

            subScope.Devices.Add(device);

            var connection = controller.AddSubScope(subScope);
            connection.Length = 10;
            connection.ConduitLength = 5;
            connection.ConduitType = conduitType;
            
            system.AddInstance(bid);
            system.AddInstance(bid);
            
            //For Both Conduit and Wire: 2*(length * type.Cost/Labor + length * RatedCost.Cost/Labor) = 2*(10 * 1 +10 * 1) + 2 * (5 * 1 + 5 * 1) = 40 + 10 = 50
            Assert.AreEqual(50, bid.Estimate.ElectricalLaborHours, "Electrical Labor Not Updating");
            Assert.AreEqual(50, bid.Estimate.ElectricalMaterialCost, "Electrical Material Not Updating");
        }

        [TestMethod]
        public void Estimate_AddNetworkConnection()
        {
            var bid = new TECBid();
            var manufacturer = new TECManufacturer();
            
            TECIO io = new TECIO();
            io.Type = IOType.BACnetIP;

            var connectionType = new TECConnectionType();
            connectionType.Cost = 1;
            connectionType.Labor = 1;

            var controller1 = new TECController(manufacturer);
            controller1.IO.Add(io);
            var controller2 = new TECController(manufacturer);
            controller2.IO.Add(io);

            bid.Controllers.Add(controller1);
            bid.Controllers.Add(controller2);
            
            var connection = controller1.AddController(controller2, connectionType);
            connection.Length = 50;

            Assert.AreEqual(50, bid.Estimate.ElectricalLaborHours, "Electrical Labor Not Updating");
            Assert.AreEqual(50, bid.Estimate.ElectricalMaterialCost, "Electrical Material Not Updating");
        }

        [TestMethod]
        public void Estimate_AddNetworkConnectionToSystem()
        {
            var bid = new TECBid();
            var manufacturer = new TECManufacturer();
            var system = new TECSystem();
            bid.Systems.Add(system);

            TECIO io = new TECIO();
            io.Type = IOType.BACnetIP;

            var connectionType = new TECConnectionType();
            connectionType.Cost = 1;
            connectionType.Labor = 1;

            var controller1 = new TECController(manufacturer);
            controller1.IO.Add(io);
            var controller2 = new TECController(manufacturer);
            controller2.IO.Add(io);

            bid.Controllers.Add(controller1);
            system.Controllers.Add(controller2);
            system.AddInstance(bid);
            var instanceController = system.SystemInstances.RandomObject().Controllers.RandomObject();

            var connection = controller1.AddController(instanceController, connectionType);
            connection.Length = 50;

            Assert.AreEqual(50, bid.Estimate.ElectricalLaborHours, "Electrical Labor Not Updating");
            Assert.AreEqual(50, bid.Estimate.ElectricalMaterialCost, "Electrical Material Not Updating");
        }

        [TestMethod]
        public void Estimate_AddSubScopeToSystem()
        {
            var bid = new TECBid();
            var manufacturer = new TECManufacturer();
            var connectionType = new TECConnectionType();
            connectionType.Cost = 1;
            connectionType.Labor = 1;
            var system = new TECSystem();
            bid.Systems.Add(system);

            var equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            system.AddInstance(bid);

            var device = new TECDevice(new ObservableCollection<TECConnectionType> { connectionType }, manufacturer);
            device.Cost = 10;
            var subScope = new TECSubScope();
            subScope.Devices.Add(device);

            equipment.SubScope.Add(subScope);

            Assert.AreEqual(10, bid.Estimate.TECMaterialCost, "TECMaterialCost Not Updating");
        }

        [TestMethod]
        public void Estimate_AddEquipmentToSystem()
        {
            var bid = new TECBid();
            var manufacturer = new TECManufacturer();
            var connectionType = new TECConnectionType();
            connectionType.Cost = 1;
            connectionType.Labor = 1;
            var system = new TECSystem();
            bid.Systems.Add(system);
            system.AddInstance(bid);

            var equipment = new TECEquipment();
            var device = new TECDevice(new ObservableCollection<TECConnectionType> { connectionType }, manufacturer);
            device.Cost = 10;
            var subScope = new TECSubScope();
            subScope.Devices.Add(device);
            equipment.SubScope.Add(subScope);

            system.Equipment.Add(equipment);

            Assert.AreEqual(10, bid.Estimate.TECMaterialCost, "TECMaterialCost Not Updating");
        }

        [TestMethod]
        public void Estimate_Tax()
        {
            var bid = new TECBid();
            var manufacturer = new TECManufacturer();
            var connectionType = new TECConnectionType();
            connectionType.Cost = 1;
            connectionType.Labor = 1;
            var system = new TECSystem();
            bid.Systems.Add(system);
            system.AddInstance(bid);

            var equipment = new TECEquipment();
            var device = new TECDevice(new ObservableCollection<TECConnectionType> { connectionType }, manufacturer);
            device.Cost = 10;
            var subScope = new TECSubScope();
            subScope.Devices.Add(device);
            equipment.SubScope.Add(subScope);

            system.Equipment.Add(equipment);

            Assert.AreEqual(0.875, bid.Estimate.Tax, "TECMaterialCost Not Updating");
        }

        [TestMethod]
        public void Estimate_TaxExempt()
        {
            var bid = new TECBid();
            var manufacturer = new TECManufacturer();
            var connectionType = new TECConnectionType();
            connectionType.Cost = 1;
            connectionType.Labor = 1;
            var system = new TECSystem();
            bid.Systems.Add(system);
            system.AddInstance(bid);

            var equipment = new TECEquipment();
            var device = new TECDevice(new ObservableCollection<TECConnectionType> { connectionType }, manufacturer);
            device.Cost = 10;
            var subScope = new TECSubScope();
            subScope.Devices.Add(device);
            equipment.SubScope.Add(subScope);

            system.Equipment.Add(equipment);

            bid.Parameters.IsTaxExempt = true;

            Assert.AreEqual(0, bid.Estimate.Tax, "TECMaterialCost Not Updating");
        }
    }
}
