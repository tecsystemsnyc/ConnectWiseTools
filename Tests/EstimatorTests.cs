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

            bid.Estimate.Refresh();

            Assert.AreEqual(expetcedPrice, bid.Estimate.TotalPrice);
        }

        [TestMethod]
        public void Estimate_AddControllerToBid()
        {
            var bid = new TECBid();

            var manufacturer = new TECManufacturer();

            var controller = new TECController(manufacturer);
            controller.Cost = 100;

            bid.Controllers.Add(controller);

            Assert.AreEqual(100, bid.Estimate.TECMaterialCost, "Material cost not added.");
            Assert.AreEqual(0, bid.Estimate.ElectricalMaterialCost);
            Assert.AreEqual(0, bid.Estimate.ElectricalLaborHours);

            checkRefresh(bid);
        }

        [TestMethod]
        public void Estimate_RemoveControllerFromBid()
        {
            var bid = new TECBid();

            var manufacturer = new TECManufacturer();

            var controller = new TECController(manufacturer);
            controller.Cost = 100;

            bid.Controllers.Add(controller);
            bid.Controllers.Remove(controller);

            assertNoCostOrLabor(bid);

            checkRefresh(bid);
        }

        [TestMethod]
        public void Estimate_AddControllerToTypicalWithInstances()
        {
            var bid = new TECBid();

            var manufacturer = new TECManufacturer();

            var controller = new TECController(manufacturer);
            controller.Cost = 100;

            var typical = new TECSystem();
            bid.Systems.Add(typical);

            typical.AddInstance(bid);

            typical.Controllers.Add(controller);

            Assert.AreEqual(100, bid.Estimate.TECMaterialCost, "Material cost not added.");
            Assert.AreEqual(0, bid.Estimate.ElectricalMaterialCost);

            checkRefresh(bid);
        }

        [TestMethod]
        public void Estimate_RemoveControllerToTypicalWithInstances()
        {
            var bid = new TECBid();

            var manufacturer = new TECManufacturer();

            var controller = new TECController(manufacturer);
            controller.Cost = 100;

            var typical = new TECSystem();
            bid.Systems.Add(typical);

            typical.AddInstance(bid);

            typical.Controllers.Add(controller);
            typical.Controllers.Remove(controller);

            assertNoCostOrLabor(bid);

            checkRefresh(bid);
        }

        [TestMethod]
        public void Estimate_AddSystemInstancesWithController()
        {
            var bid = new TECBid();

            var manufacturer = new TECManufacturer();

            var controller = new TECController(manufacturer);
            controller.Cost = 100;

            var typical = new TECSystem();
            bid.Systems.Add(typical);

            typical.Controllers.Add(controller);

            typical.AddInstance(bid);

            Assert.AreEqual(100, bid.Estimate.TECMaterialCost, "Material cost not added.");
            Assert.AreEqual(0, bid.Estimate.ElectricalMaterialCost);

            checkRefresh(bid);
        }

        [TestMethod]
        public void Estimate_RemoveSystemInstancesWithController()
        {
            var bid = new TECBid();

            var manufacturer = new TECManufacturer();

            var controller = new TECController(manufacturer);
            controller.Cost = 100;

            var typical = new TECSystem();
            bid.Systems.Add(typical);

            typical.Controllers.Add(controller);

            typical.AddInstance(bid);

            typical.Controllers.Remove(controller);

            assertNoCostOrLabor(bid);

            checkRefresh(bid);
        }

        [TestMethod]
        public void Estimate_AddPanelToBid()
        {
            var bid = new TECBid();

            var panelType = new TECPanelType();
            panelType.Cost = 50;
            panelType.Labor = 7;

            var panel = new TECPanel(panelType);

            bid.Panels.Add(panel);

            Assert.AreEqual(50, bid.Estimate.TECMaterialCost, "Material cost not added.");
            Assert.AreEqual(7, bid.Estimate.TECLaborHours, "Labor hours not added.");
            Assert.AreEqual(0, bid.Estimate.ElectricalMaterialCost);
            Assert.AreEqual(0, bid.Estimate.ElectricalLaborHours);

            checkRefresh(bid);
        }

        [TestMethod]
        public void Estimate_RemovePanelToBid()
        {
            var bid = new TECBid();

            var panelType = new TECPanelType();
            panelType.Cost = 50;
            panelType.Labor = 7;

            var panel = new TECPanel(panelType);

            bid.Panels.Add(panel);
            bid.Panels.Remove(panel);

            assertNoCostOrLabor(bid);

            checkRefresh(bid);
        }

        [TestMethod]
        public void Estimate_AddPanelToTypicalWithInstances()
        {
            var bid = new TECBid();

            var panelType = new TECPanelType();
            panelType.Cost = 50;
            panelType.Labor = 7;

            var panel = new TECPanel(panelType);

            var typical = new TECSystem();
            bid.Systems.Add(typical);

            typical.AddInstance(bid);

            typical.Panels.Add(panel);

            Assert.AreEqual(50, bid.Estimate.TECMaterialCost, "Material cost not added.");
            Assert.AreEqual(7, bid.Estimate.TECLaborHours, "Labor hours not added.");
            Assert.AreEqual(0, bid.Estimate.ElectricalMaterialCost);
            Assert.AreEqual(0, bid.Estimate.ElectricalLaborHours);

            checkRefresh(bid);
        }

        [TestMethod]
        public void Estimate_RemovePanelToTypicalWithInstances()
        {
            var bid = new TECBid();

            var panelType = new TECPanelType();
            panelType.Cost = 50;
            panelType.Labor = 7;

            var panel = new TECPanel(panelType);

            var typical = new TECSystem();
            bid.Systems.Add(typical);

            typical.AddInstance(bid);

            typical.Panels.Add(panel);

            typical.Panels.Remove(panel);

            assertNoCostOrLabor(bid);

            checkRefresh(bid);
        }

        [TestMethod]
        public void Estimate_AddSystemInstanceWithPanel()
        {
            var bid = new TECBid();

            var panelType = new TECPanelType();
            panelType.Cost = 50;
            panelType.Labor = 7;

            var panel = new TECPanel(panelType);

            var typical = new TECSystem();
            bid.Systems.Add(typical);

            typical.Panels.Add(panel);

            typical.AddInstance(bid);

            Assert.AreEqual(50, bid.Estimate.TECMaterialCost, "Material cost not added.");
            Assert.AreEqual(7, bid.Estimate.TECLaborHours, "Labor hours not added.");
            Assert.AreEqual(0, bid.Estimate.ElectricalMaterialCost);
            Assert.AreEqual(0, bid.Estimate.ElectricalLaborHours);

            checkRefresh(bid);
        }

        [TestMethod]
        public void Estimate_RemoveSystemInstanceWithPanel()
        {
            var bid = new TECBid();

            var panelType = new TECPanelType();
            panelType.Cost = 50;
            panelType.Labor = 7;

            var panel = new TECPanel(panelType);

            var typical = new TECSystem();
            bid.Systems.Add(typical);

            typical.Panels.Add(panel);

            typical.AddInstance(bid);

            typical.Panels.Remove(panel);

            assertNoCostOrLabor(bid);

            checkRefresh(bid);
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

            checkRefresh(bid);
        }

        [TestMethod]
        public void Estimate_AddInstanceSystemWithEquipment()
        {
            var bid = new TECBid();
            var manufacturer = new TECManufacturer();
            var connectionType = new TECConnectionType();
            var device = new TECDevice(new ObservableCollection<TECConnectionType> { connectionType }, manufacturer);
            device.Cost = 10;
            bid.Catalogs.Devices.Add(device);
            connectionType.Cost = 1;
            connectionType.Labor = 1;
            
            var system = new TECSystem();
            var equipment = new TECEquipment();
            var subScope = new TECSubScope();
            subScope.Devices.Add(device);
            equipment.SubScope.Add(subScope);
            system.Equipment.Add(equipment);
            
            bid.Systems.Add(system);
            system.AddInstance(bid);

            Assert.AreEqual(10, bid.Estimate.TECMaterialCost, "TECMaterialCost Not Updating");

            checkRefresh(bid);
        }

        [TestMethod]
        public void Estimate_AddInstanceSystemWithMisc()
        {
            var bid = new TECBid();
            
            var system = new TECSystem();
            var tecMisc = new TECMisc();
            tecMisc.Cost = 11;
            tecMisc.Labor = 12;
            tecMisc.Type = CostType.TEC;
            var electricalMisc = new TECMisc();
            electricalMisc.Cost = 13;
            electricalMisc.Labor = 14;
            electricalMisc.Type = CostType.Electrical;

            system.MiscCosts.Add(tecMisc);
            system.MiscCosts.Add(electricalMisc);

            bid.Systems.Add(system);
            system.AddInstance(bid);

            Assert.AreEqual(11, bid.Estimate.TECMaterialCost, "Material cost not added");
            Assert.AreEqual(13, bid.Estimate.ElectricalMaterialCost, "Electrical Material cost not added");
            Assert.AreEqual(12, bid.Estimate.TECLaborHours, "Labor hours not added");
            Assert.AreEqual(14, bid.Estimate.ElectricalLaborHours, "Electrical labor hours not added");

            checkRefresh(bid);
        }

        [TestMethod]
        public void Estimate_AddSystemWithSubScopeConnection()
        {
            var bid = new TECBid();
            var manufacturer = new TECManufacturer();
            manufacturer.Multiplier = 1;
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
            bid.Catalogs.ConnectionTypes.Add(connectionType);
            bid.Catalogs.ConduitTypes.Add(conduitType);
            bid.Catalogs.AssociatedCosts.Add(ratedCost);
            bid.Catalogs.Manufacturers.Add(manufacturer);


            var system = new TECSystem();
            var equipment = new TECEquipment();
            var subScope = new TECSubScope();
            
            var controller = new TECController(manufacturer);

            equipment.SubScope.Add(subScope);
            system.Equipment.Add(equipment);
            system.Controllers.Add(controller);
            
            subScope.Devices.Add(device);

            var connection = controller.AddSubScope(subScope);
            connection.Length = 10;
            connection.ConduitLength = 5;
            connection.ConduitType = conduitType;

            bid.Systems.Add(system);

            system.AddInstance(bid);
            system.AddInstance(bid);

            //For Both Conduit and Wire: 2*(length * type.Cost/Labor + length * RatedCost.Cost/Labor) = 2*(10 * 1 +10 * 1) + 2 * (5 * 1 + 5 * 1) = 40 + 10 = 50
            Assert.AreEqual(50, bid.Estimate.ElectricalLaborHours, "Electrical Labor Not Updating");
            Assert.AreEqual(50, bid.Estimate.ElectricalMaterialCost, "Electrical Material Not Updating");

            checkRefresh(bid);
        }

        [TestMethod]
        public void Estimate_AddSubScopeConnectionInSystem()
        {
            var bid = new TECBid();

            var manufacturer = new TECManufacturer();
            manufacturer.Multiplier = 1;
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
            bid.Catalogs.ConnectionTypes.Add(connectionType);
            bid.Catalogs.ConduitTypes.Add(conduitType);
            bid.Catalogs.AssociatedCosts.Add(ratedCost);
            bid.Catalogs.Manufacturers.Add(manufacturer);

            var system = new TECSystem();
            var equipment = new TECEquipment();
            var subScope = new TECSubScope();
            
            var controller = new TECController(manufacturer);

            system.Controllers.Add(controller);
            system.Equipment.Add(equipment);
            equipment.SubScope.Add(subScope);
            subScope.Devices.Add(device);
            bid.Systems.Add(system);

            system.AddInstance(bid);
            system.AddInstance(bid);
            
            var connection = controller.AddSubScope(subScope);
            connection.Length = 10;
            connection.ConduitLength = 5;
            connection.ConduitType = conduitType;

            foreach(TECSystem instance in system.SystemInstances)
            {
                foreach(TECController instanceController in instance.Controllers)
                {
                    foreach(TECConnection instanceConnection in instanceController.ChildrenConnections)
                    {
                        instanceConnection.Length = 10;
                        instanceConnection.ConduitLength = 5;
                        instanceConnection.ConduitType = conduitType;
                    }
                }
            }


            //For Both Conduit and Wire: 2*(length * type.Cost/Labor + length * RatedCost.Cost/Labor) = 2*(10 * 1 +10 * 1) + 2 * (5 * 1 + 5 * 1) = 40 + 10 = 50
            Assert.AreEqual(50, bid.Estimate.ElectricalLaborHours, "Electrical Labor Not Updating");
            Assert.AreEqual(50, bid.Estimate.ElectricalMaterialCost, "Electrical Material Not Updating");

            checkRefresh(bid);
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

            checkRefresh(bid);
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

            checkRefresh(bid);
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

            checkRefresh(bid);
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

            checkRefresh(bid);
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

            checkRefresh(bid);
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

            checkRefresh(bid);
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

            checkRefresh(bid);
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

            checkRefresh(bid);
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

            checkRefresh(bid);
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

            checkRefresh(bid);
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

            checkRefresh(bid);
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

            checkRefresh(bid);
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

            checkRefresh(bid);
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

            checkRefresh(bid);
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

            checkRefresh(bid);
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

            checkRefresh(bid);
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

            checkRefresh(bid);
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

            checkRefresh(bid);
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

            checkRefresh(bid);
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

            checkRefresh(bid);
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

            checkRefresh(bid);
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

            checkRefresh(bid);
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

            checkRefresh(bid);
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

            checkRefresh(bid);
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

            checkRefresh(bid);
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

            checkRefresh(bid);
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

            checkRefresh(bid);
        }
        
        #endregion

        private void assertNoCostOrLabor(TECBid bid)
        {
            Assert.AreEqual(0, bid.Estimate.TECMaterialCost);
            Assert.AreEqual(0, bid.Estimate.TECLaborHours);
            Assert.AreEqual(0, bid.Estimate.ElectricalMaterialCost);
            Assert.AreEqual(0, bid.Estimate.ElectricalLaborHours);
        }

        private void checkRefresh(TECBid bid)
        {
            double tecCost = bid.Estimate.TECMaterialCost;
            double tecLabor = bid.Estimate.TECLaborHours;
            double elecCost = bid.Estimate.ElectricalMaterialCost;
            double elecLabor = bid.Estimate.ElectricalLaborHours;
            double total = bid.Estimate.TotalPrice;

            bid.Estimate.Refresh();

            Assert.AreEqual(tecCost, bid.Estimate.TECMaterialCost, "TEC material cost refresh failed.");
            Assert.AreEqual(tecLabor, bid.Estimate.TECLaborHours, "TEC labor hours refresh failed.");
            Assert.AreEqual(elecCost, bid.Estimate.ElectricalMaterialCost, "Electrtical material cost refresh failed.");
            Assert.AreEqual(elecLabor, bid.Estimate.ElectricalLaborHours, "Elecrtical labor horus refresh failed.");
            Assert.AreEqual(total, bid.Estimate.TotalPrice);
        }
    }
}
