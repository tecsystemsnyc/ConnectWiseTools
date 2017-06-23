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

        private static TECLabor labor;

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
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            labor = new TECLabor();
            labor.PMCoef = 1.54;
            labor.ENGCoef = 1.25;
            labor.SoftCoef = 0.37;
            labor.GraphCoef = 0.53;
            labor.CommCoef = 1.34;
        }
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
        public void Estimate_Refresh()
        {
            TECBid bid = TestHelper.CreateTestBid();
            double expetcedPrice = bid.Estimate.TotalPrice;

            Console.WriteLine("------------------------------------------------------------");

            bid.Estimate.Refresh();

            Assert.AreEqual(expetcedPrice, bid.Estimate.TotalPrice);
        }

        [TestMethod]
        public void Estimate_AddMiscCost()
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
        public void Estimate_RemoveMiscCost()
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

            bid.MiscCosts.Remove(tecMisc);
            bid.MiscCosts.Remove(eMisc);

            Assert.AreEqual(0, bid.Estimate.TECMaterialCost, "Material cost not removed");
            Assert.AreEqual(0, bid.Estimate.ElectricalMaterialCost, "Electrical Material cost not removed");
            Assert.AreEqual(0, bid.Estimate.TECLaborHours, "Labor hours not removed");
            Assert.AreEqual(0, bid.Estimate.ElectricalLaborHours, "Electrical labor hours not removed");
        }
        
        [TestMethod]
        public void Estimate_AddMiscCostFromSystem()
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
        public void Estimate_RemoveMiscCostFromSystem()
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

            system.MiscCosts.Remove(tecMisc);
            system.MiscCosts.Remove(eMisc);

            Assert.AreEqual(0, bid.Estimate.TECMaterialCost, "Material cost not added");
            Assert.AreEqual(0, bid.Estimate.ElectricalMaterialCost, "Electrical Material cost not added");
            Assert.AreEqual(0, bid.Estimate.TECLaborHours, "Labor hours not added");
            Assert.AreEqual(0, bid.Estimate.ElectricalLaborHours, "Electrical labor hours not added");
        }

        [TestMethod]
        public void Estimate_AddAssCostFromEquipemnt()
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
        public void Estimate_RemoveAssCostFromEquipemnt()
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

            equipment.AssociatedCosts.Remove(tecCost);
            equipment.AssociatedCosts.Remove(eCost);

            Assert.AreEqual(0, bid.Estimate.TECMaterialCost, "Material cost not removed");
            Assert.AreEqual(0, bid.Estimate.ElectricalMaterialCost, "Electrical Material cost not removed");
            Assert.AreEqual(0, bid.Estimate.TECLaborHours, "Labor hours not removed");
            Assert.AreEqual(0, bid.Estimate.ElectricalLaborHours, "Electrical labor hours not removed");
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
        public void Estimate_RemoveDeviceToSubScope()
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

            subScope.Devices.Remove(device);

            Assert.AreEqual(0, bid.Estimate.TECMaterialCost, "Material cost not added");
        }

        [TestMethod]
        public void Estimate_AddDeviceWithMultiplierToSubScope()
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
            manufacturer.Multiplier = 0.5;

            var device = new TECDevice(new ObservableCollection<TECConnectionType> { new TECConnectionType() }, manufacturer);
            device.Cost = 100;
            bid.Catalogs.Devices.Add(device);

            subScope.Devices.Add(device);

            Assert.AreEqual(100, bid.Estimate.TECMaterialCost, "Material cost not added");
        }

        [TestMethod]
        public void Estimate_RemoveDeviceWithMultiplierToSubScope()
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
            manufacturer.Multiplier = 0.5;

            var device = new TECDevice(new ObservableCollection<TECConnectionType> { new TECConnectionType() }, manufacturer);
            device.Cost = 100;
            bid.Catalogs.Devices.Add(device);

            subScope.Devices.Add(device);

            subScope.Devices.Remove(device);

            Assert.AreEqual(0, bid.Estimate.TECMaterialCost, "Material cost not added");
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
        public void Estimate_RemoveConnectionToSubScope()
        {
            var bid = new TECBid();

            var system = new TECSystem();
            var equipment = new TECEquipment();
            var subScope = new TECSubScope();

            var manufacturer = new TECManufacturer();
            manufacturer.Multiplier = 1;

            var controller = new TECController(manufacturer);
            controller.IsGlobal = false;

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

            controller.RemoveSubScope(subScope);

            //For Both Conduit and Wire: 2*(length * type.Cost/Labor + length * RatedCost.Cost/Labor) = 2*(10 * 1 +10 * 1) + 2 * (5 * 1 + 5 * 1) = 40 + 10 = 50
            Assert.AreEqual(0, bid.Estimate.ElectricalLaborHours, "Electrical Labor Not Updating");
            Assert.AreEqual(0, bid.Estimate.ElectricalMaterialCost, "Electrical Material Not Updating");
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

            var conduitType = new TECConduitType();
            conduitType.Cost = 1;
            conduitType.Labor = 1;

            var controller1 = new TECController(manufacturer);
            controller1.IO.Add(io);
            var controller2 = new TECController(manufacturer);
            controller2.IO.Add(io);

            bid.Controllers.Add(controller1);
            bid.Controllers.Add(controller2);
            
            var connection = controller1.AddController(controller2, connectionType);
            connection.Length = 50;
            connection.ConduitLength = 50;
            connection.ConduitType = conduitType;

            Assert.AreEqual(100, bid.Estimate.ElectricalLaborHours, "Electrical Labor Not Updating");
            Assert.AreEqual(100, bid.Estimate.ElectricalMaterialCost, "Electrical Material Not Updating");
        }
        
        [TestMethod]
        public void Estimate_RemoveNetworkConnection()
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

            controller1.RemoveController(controller2);

            Assert.AreEqual(0, bid.Estimate.ElectricalLaborHours, "Electrical Labor Not Updating");
            Assert.AreEqual(0, bid.Estimate.ElectricalMaterialCost, "Electrical Material Not Updating");
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
        public void Estimate_RemoveNetworkConnectionToSystem()
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

            controller1.RemoveController(instanceController);

            Assert.AreEqual(0, bid.Estimate.ElectricalLaborHours, "Electrical Labor Not Updating");
            Assert.AreEqual(0, bid.Estimate.ElectricalMaterialCost, "Electrical Material Not Updating");
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
        public void Estimate_RemoveSubScopeFromSystem()
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

            equipment.SubScope.Remove(subScope);

            Assert.AreEqual(0, bid.Estimate.TECMaterialCost, "TECMaterialCost Not Updating");
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
        public void Estimate_RemoveEquipmentToSystem()
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

            system.Equipment.Remove(equipment);

            Assert.AreEqual(0, bid.Estimate.TECMaterialCost, "TECMaterialCost Not Updating");
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

            Assert.AreEqual(0.875, bid.Estimate.Tax);
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

            Assert.AreEqual(0, bid.Estimate.Tax);
        }

        [TestMethod]
        public void Estimate_TECShipping()
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
            
            Assert.AreEqual(0.3, bid.Estimate.TECShipping);
        }

        [TestMethod]
        public void Estimate_TECWarranty()
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

            Assert.AreEqual(0.5, bid.Estimate.TECWarranty);
        }

        [TestMethod]
        public void Estimate_ElectricalShipping()
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

            //For Both Conduit and Wire Cost: 2*(length * type.Cost/Labor + length * RatedCost.Cost/Labor) = 2*(10 * 1 +10 * 1) + 2 * (5 * 1 + 5 * 1) = 40 + 10 = 50
            Assert.AreEqual(1.5, bid.Estimate.ElectricalShipping);
        }

        [TestMethod]
        public void Estimate_ElectricalWarranty()
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

            //For Both Conduit and Wire Cost: 2*(length * type.Cost/Labor + length * RatedCost.Cost/Labor) = 2*(10 * 1 +10 * 1) + 2 * (5 * 1 + 5 * 1) = 40 + 10 = 50
            Assert.AreEqual(2.5, bid.Estimate.ElectricalWarranty);
        }

        #region Derived Labor
        [TestMethod]
        public void Estimate_TECLaborHoursFromPoints()
        {
            var bid = new TECBid();
            bid.Labor = labor;
            var system = new TECSystem();
            var equipment = new TECEquipment();
            var subScope = new TECSubScope();
            var point = new TECPoint();
            point.Type = PointTypes.AI;

            subScope.Points.Add(point);
            equipment.SubScope.Add(subScope);
            system.Equipment.Add(equipment);
            bid.Systems.Add(system);
            
            system.AddInstance(bid);
            system.AddInstance(bid);

            Assert.AreEqual(2, bid.Estimate.TotalPointNumber, "Total points not updating");
            Assert.AreEqual(3.08, bid.Estimate.PMLaborHours, "PM labor calcualtion");
            Assert.AreEqual(2.5, bid.Estimate.ENGLaborHours, "ENG labor calcualtion");
            Assert.AreEqual(0.74, bid.Estimate.SoftLaborHours, "Software labor calcualtion");
            Assert.AreEqual(1.06, bid.Estimate.GraphLaborHours, "Graphics labor calcualtion");
            Assert.AreEqual(2.68, bid.Estimate.CommLaborHours, "Comm labor calcualtion");
        }

        [TestMethod]
        public void Estimate_TECLaborHoursFromLump()
        {
            var bid = new TECBid();
            bid.Labor = labor;
            labor.PMExtraHours = 10;
            labor.ENGExtraHours = 10;
            labor.SoftExtraHours = 10;
            labor.GraphExtraHours = 10;
            labor.CommExtraHours = 10;

            Assert.AreEqual(10, bid.Estimate.PMLaborHours, "PM labor calcualtion");
            Assert.AreEqual(10, bid.Estimate.ENGLaborHours, "PM labor calcualtion");
            Assert.AreEqual(10, bid.Estimate.SoftLaborHours, "Software labor calcualtion");
            Assert.AreEqual(10, bid.Estimate.GraphLaborHours, "Graphics labor calcualtion");
            Assert.AreEqual(10, bid.Estimate.CommLaborHours, "Comm labor calcualtion");
        }

        [TestMethod]
        public void Estimate_AddTypicalSystem()
        {
            var bid = new TECBid();
            bid.Catalogs = TestHelper.CreateTestCatalogs();
            var system = TestHelper.CreateTestSystem(bid.Catalogs);
            bid.Systems.Add(system);
            
            Assert.AreEqual(0, bid.Estimate.TECMaterialCost, "Material cost not added");
            Assert.AreEqual(0, bid.Estimate.ElectricalMaterialCost, "Electrical Material cost not added");
            Assert.AreEqual(0, bid.Estimate.TECLaborHours, "Labor hours not added");
            Assert.AreEqual(0, bid.Estimate.ElectricalLaborHours, "Electrical labor hours not added");
        }

        #endregion
    }
}
