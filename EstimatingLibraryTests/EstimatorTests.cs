using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EstimatingLibrary;
using System.Collections.ObjectModel;
using EstimatingLibrary.Utilities;

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

        private static TECParameters parameters;

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
            parameters = new TECParameters(Guid.NewGuid());
            parameters.PMCoef = 1.54;
            parameters.ENGCoef = 1.25;
            parameters.SoftCoef = 0.37;
            parameters.GraphCoef = 0.53;
            parameters.CommCoef = 1.34;
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
            ChangeWatcher watcher = new ChangeWatcher(bid);
            var estimate = new TECEstimator(bid, watcher);
            double expetcedPrice = estimate.TotalPrice;

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_AddControllerToBid()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);

            var manufacturer = new TECManufacturer();
            var type = new TECControllerType(manufacturer);
            type.Price = 100;

            var controller = new TECController(type, false);

            bid.Controllers.Add(controller);

            Assert.AreEqual(100, estimate.TECMaterialCost, "Material cost not added.");
            Assert.AreEqual(0, estimate.ElectricalMaterialCost);
            Assert.AreEqual(0, estimate.ElectricalLaborHours);

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_RemoveControllerFromBid()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);

            var manufacturer = new TECManufacturer();
            var type = new TECControllerType(manufacturer);
            type.Price = 100;

            var controller = new TECController(type, false);

            bid.Controllers.Add(controller);
            bid.Controllers.Remove(controller);

            assertNoCostOrLabor(estimate);

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_AddControllerToTypicalWithInstances()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);

            var manufacturer = new TECManufacturer();
            var type = new TECControllerType(manufacturer);
            type.Price = 100;

            var controller = new TECController(type, true);

            var typical = new TECTypical();
            bid.Systems.Add(typical);

            typical.AddInstance(bid);

            typical.Controllers.Add(controller);

            Assert.AreEqual(100, estimate.TECMaterialCost, "Material cost not added.");
            Assert.AreEqual(0, estimate.ElectricalMaterialCost);

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_RemoveControllerToTypicalWithInstances()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);

            var manufacturer = new TECManufacturer();
            var type = new TECControllerType(manufacturer);
            type.Price = 100;

            var controller = new TECController(type, true);

            var typical = new TECTypical();
            bid.Systems.Add(typical);

            typical.AddInstance(bid);

            typical.Controllers.Add(controller);
            typical.Controllers.Remove(controller);

            assertNoCostOrLabor(estimate);

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_AddInstancesWithController()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);

            var manufacturer = new TECManufacturer();
            var type = new TECControllerType(manufacturer);
            type.Price = 100;

            var controller = new TECController(type, true);

            var typical = new TECTypical();
            bid.Systems.Add(typical);

            typical.Controllers.Add(controller);

            typical.AddInstance(bid);

            Assert.AreEqual(100, estimate.TECMaterialCost, "Material cost not added.");
            Assert.AreEqual(0, estimate.ElectricalMaterialCost);

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_RemoveInstancesWithController()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);

            var manufacturer = new TECManufacturer();
            var type = new TECControllerType(manufacturer);
            type.Price = 100;

            var controller = new TECController(type, true);

            var typical = new TECTypical();
            bid.Systems.Add(typical);

            typical.Controllers.Add(controller);

            typical.AddInstance(bid);

            typical.Controllers.Remove(controller);

            assertNoCostOrLabor(estimate);

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_AddPanelToBid()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);

            var manufacturer = new TECManufacturer();
            var panelType = new TECPanelType(manufacturer);
            panelType.Price = 50;
            panelType.Labor = 7;

            var panel = new TECPanel(panelType, true);

            bid.Panels.Add(panel);

            Assert.AreEqual(50, estimate.TECMaterialCost, "Material cost not added.");
            Assert.AreEqual(7, estimate.TECLaborHours, "Labor hours not added.");
            Assert.AreEqual(0, estimate.ElectricalMaterialCost);
            Assert.AreEqual(0, estimate.ElectricalLaborHours);

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_RemovePanelToBid()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);

            var manufacturer = new TECManufacturer();
            var panelType = new TECPanelType(manufacturer);
            panelType.Price = 50;
            panelType.Labor = 7;

            var panel = new TECPanel(panelType, true);

            bid.Panels.Add(panel);
            bid.Panels.Remove(panel);

            assertNoCostOrLabor(estimate);

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_AddPanelToTypicalWithInstances()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);

            var manufacturer = new TECManufacturer();
            var panelType = new TECPanelType(manufacturer);
            panelType.Price = 50;
            panelType.Labor = 7;

            var panel = new TECPanel(panelType, true);

            var typical = new TECTypical();
            bid.Systems.Add(typical);

            typical.AddInstance(bid);

            typical.Panels.Add(panel);

            Assert.AreEqual(50, estimate.TECMaterialCost, "Material cost not added.");
            Assert.AreEqual(7, estimate.TECLaborHours, "Labor hours not added.");
            Assert.AreEqual(0, estimate.ElectricalMaterialCost);
            Assert.AreEqual(0, estimate.ElectricalLaborHours);

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_RemovePanelToTypicalWithInstances()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);

            var manufacturer = new TECManufacturer();
            var panelType = new TECPanelType(manufacturer);
            panelType.Price = 50;
            panelType.Labor = 7;

            var panel = new TECPanel(panelType, true);

            var typical = new TECTypical();
            bid.Systems.Add(typical);

            typical.AddInstance(bid);

            typical.Panels.Add(panel);

            typical.Panels.Remove(panel);

            assertNoCostOrLabor(estimate);

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_AddSystemInstanceWithPanel()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);

            var manufacturer = new TECManufacturer();
            var panelType = new TECPanelType(manufacturer);
            panelType.Price = 50;
            panelType.Labor = 7;

            var panel = new TECPanel(panelType, true);

            var typical = new TECTypical();
            bid.Systems.Add(typical);

            typical.Panels.Add(panel);

            typical.AddInstance(bid);

            Assert.AreEqual(50, estimate.TECMaterialCost, "Material cost not added.");
            Assert.AreEqual(7, estimate.TECLaborHours, "Labor hours not added.");
            Assert.AreEqual(0, estimate.ElectricalMaterialCost);
            Assert.AreEqual(0, estimate.ElectricalLaborHours);

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_RemoveSystemInstanceWithPanel()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);

            var manufacturer = new TECManufacturer();
            var panelType = new TECPanelType(manufacturer);
            panelType.Price = 50;
            panelType.Labor = 7;

            var panel = new TECPanel(panelType, true);

            var typical = new TECTypical();
            bid.Systems.Add(typical);

            typical.Panels.Add(panel);

            typical.AddInstance(bid);

            typical.Panels.Remove(panel);

            assertNoCostOrLabor(estimate);

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_AddTypicalSystem()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            bid.Catalogs = TestHelper.CreateTestCatalogs();
            var system = TestHelper.CreateTestTypical(bid.Catalogs);
            bid.Systems.Add(system);

            Assert.AreEqual(0, estimate.TECMaterialCost, "Material cost not added");
            Assert.AreEqual(0, estimate.ElectricalMaterialCost, "Electrical Material cost not added");
            Assert.AreEqual(0, estimate.TECLaborHours, "Labor hours not added");
            Assert.AreEqual(0, estimate.ElectricalLaborHours, "Electrical labor hours not added");

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_RemoveTypicalSystem()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            bid.Catalogs = TestHelper.CreateTestCatalogs();
            var system = TestHelper.CreateTestTypical(bid.Catalogs);
            bid.Systems.Add(system);

            system.AddInstance(bid);
            system.AddInstance(bid);

            bid.Systems.Remove(system);

            //Assert
            assertNoCostOrLabor(estimate);

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_AddInstanceSystemWithEquipment()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            var manufacturer = new TECManufacturer();
            var connectionType = new TECElectricalMaterial();
            var device = new TECDevice(new ObservableCollection<TECElectricalMaterial> { connectionType }, manufacturer);
            device.Price = 10;
            bid.Catalogs.Devices.Add(device);
            connectionType.Cost = 1;
            connectionType.Labor = 1;
            
            var system = new TECTypical();
            var equipment = new TECEquipment(true);
            var subScope = new TECSubScope(true);
            subScope.Devices.Add(device);
            equipment.SubScope.Add(subScope);
            system.Equipment.Add(equipment);
            
            bid.Systems.Add(system);
            system.AddInstance(bid);

            Assert.AreEqual(10, estimate.TECMaterialCost, "TECMaterialCost Not Updating");

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_RemoveInstanceSystemWithEquipment()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            var manufacturer = new TECManufacturer();
            var connectionType = new TECElectricalMaterial();
            var device = new TECDevice(new ObservableCollection<TECElectricalMaterial> { connectionType }, manufacturer);
            device.Price = 10;
            bid.Catalogs.Devices.Add(device);
            connectionType.Cost = 1;
            connectionType.Labor = 1;

            var system = new TECTypical();
            var equipment = new TECEquipment(true);
            var subScope = new TECSubScope(true);
            subScope.Devices.Add(device);
            equipment.SubScope.Add(subScope);
            system.Equipment.Add(equipment);

            bid.Systems.Add(system);
            TECSystem instance = system.AddInstance(bid);

            system.Instances.Remove(instance);

            //Assert
            assertNoCostOrLabor(estimate);

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_AddInstanceSystemWithMisc()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            
            var system = new TECTypical();
            var tecMisc = new TECMisc(CostType.TEC, true);
            tecMisc.Cost = 11;
            tecMisc.Labor = 12;
            tecMisc.Type = CostType.TEC;
            var electricalMisc = new TECMisc(CostType.TEC, true);
            electricalMisc.Cost = 13;
            electricalMisc.Labor = 14;
            electricalMisc.Type = CostType.Electrical;

            system.MiscCosts.Add(tecMisc);
            system.MiscCosts.Add(electricalMisc);

            bid.Systems.Add(system);
            system.AddInstance(bid);

            Assert.AreEqual(11, estimate.TECMaterialCost, "Material cost not added");
            Assert.AreEqual(13, estimate.ElectricalMaterialCost, "Electrical Material cost not added");
            Assert.AreEqual(12, estimate.TECLaborHours, "Labor hours not added");
            Assert.AreEqual(14, estimate.ElectricalLaborHours, "Electrical labor hours not added");

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_RemoveInstanceSystemWithMisc()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);

            var system = new TECTypical();
            var tecMisc = new TECMisc(CostType.TEC, true);
            tecMisc.Cost = 11;
            tecMisc.Labor = 12;
            tecMisc.Type = CostType.TEC;
            var electricalMisc = new TECMisc(CostType.TEC, true);
            electricalMisc.Cost = 13;
            electricalMisc.Labor = 14;
            electricalMisc.Type = CostType.Electrical;

            system.MiscCosts.Add(tecMisc);
            system.MiscCosts.Add(electricalMisc);

            bid.Systems.Add(system);
            var instance = system.AddInstance(bid);

            system.Instances.Remove(instance);

            //Assert
            assertNoCostOrLabor(estimate);

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_AddInstanceSystemWithSubScopeConnection()
        {
            var bid = new TECBid();
            var watcher = new ChangeWatcher(bid);
            var estimate = new TECEstimator(bid, watcher);
            var manufacturer = new TECManufacturer();
            manufacturer.Multiplier = 1;
            var controllerType = new TECControllerType(manufacturer);
            var ratedCost = new TECCost(CostType.TEC);
            ratedCost.Cost = 1;
            ratedCost.Labor = 1;
            ratedCost.Type = CostType.Electrical;

            var connectionType = new TECElectricalMaterial();
            connectionType.Cost = 1;
            connectionType.Labor = 1;
            connectionType.RatedCosts.Add(ratedCost);
            var conduitType = new TECElectricalMaterial();
            conduitType.Cost = 1;
            conduitType.Labor = 1;
            conduitType.RatedCosts.Add(ratedCost);

            var device = new TECDevice(new ObservableCollection<TECElectricalMaterial> { connectionType }, manufacturer);
            bid.Catalogs.Devices.Add(device);
            bid.Catalogs.ConnectionTypes.Add(connectionType);
            bid.Catalogs.ConduitTypes.Add(conduitType);
            bid.Catalogs.AssociatedCosts.Add(ratedCost);
            bid.Catalogs.Manufacturers.Add(manufacturer);
            bid.Catalogs.ControllerTypes.Add(controllerType);

            var system = new TECTypical();
            var equipment = new TECEquipment(true);
            var subScope = new TECSubScope(true);
            
            var controller = new TECController(controllerType, true);

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

            //For Both Conduit and Wire: 2*(length * type.Price/Labor + length * RatedCost.Cost/Labor) = 2*(10 * 1 +10 * 1) + 2 * (5 * 1 + 5 * 1) = 40 + 20 = 60
            Assert.AreEqual(60, estimate.ElectricalLaborHours, "Electrical Labor Not Updating");
            Assert.AreEqual(60, estimate.ElectricalMaterialCost, "Electrical Material Not Updating");

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_RemoveInstanceSystemWithSubScopeConnection()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            var manufacturer = new TECManufacturer();
            manufacturer.Multiplier = 1;
            var controllerType = new TECControllerType(manufacturer);
            var ratedCost = new TECCost(CostType.TEC);
            ratedCost.Cost = 1;
            ratedCost.Labor = 1;
            ratedCost.Type = CostType.Electrical;

            var connectionType = new TECElectricalMaterial();
            connectionType.Cost = 1;
            connectionType.Labor = 1;
            connectionType.RatedCosts.Add(ratedCost);
            var conduitType = new TECElectricalMaterial();
            conduitType.Cost = 1;
            conduitType.Labor = 1;
            conduitType.RatedCosts.Add(ratedCost);

            var device = new TECDevice(new ObservableCollection<TECElectricalMaterial> { connectionType }, manufacturer);
            bid.Catalogs.Devices.Add(device);
            bid.Catalogs.ConnectionTypes.Add(connectionType);
            bid.Catalogs.ConduitTypes.Add(conduitType);
            bid.Catalogs.AssociatedCosts.Add(ratedCost);
            bid.Catalogs.Manufacturers.Add(manufacturer);
            bid.Catalogs.ControllerTypes.Add(controllerType);

            var system = new TECTypical();
            var equipment = new TECEquipment(true);
            var subScope = new TECSubScope(true);

            var controller = new TECController(controllerType, true);

            equipment.SubScope.Add(subScope);
            system.Equipment.Add(equipment);
            system.Controllers.Add(controller);

            subScope.Devices.Add(device);

            var connection = controller.AddSubScope(subScope);
            connection.Length = 10;
            connection.ConduitLength = 5;
            connection.ConduitType = conduitType;

            bid.Systems.Add(system);

            var instance = system.AddInstance(bid);

            system.Instances.Remove(instance);

            //Assert
            assertNoCostOrLabor(estimate);

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_AddSubScopeConnectionInSystem()
        {
            var bid = new TECBid();
            var watcher = new ChangeWatcher(bid);
            var estimate = new TECEstimator(bid, watcher);

            var manufacturer = new TECManufacturer();
            manufacturer.Multiplier = 1;
            var controllerType = new TECControllerType(manufacturer);
            var ratedCost = new TECCost(CostType.Electrical);
            ratedCost.Cost = 1;
            ratedCost.Labor = 1;

            var assCost = new TECCost(CostType.Electrical);
            assCost.Cost = 1;
            assCost.Labor = 1;

            var connectionType = new TECElectricalMaterial();
            connectionType.Cost = 1;
            connectionType.Labor = 1;
            connectionType.RatedCosts.Add(ratedCost);
            connectionType.AssociatedCosts.Add(assCost);
            var conduitType = new TECElectricalMaterial();
            conduitType.Cost = 1;
            conduitType.Labor = 1;
            conduitType.RatedCosts.Add(ratedCost);
            conduitType.AssociatedCosts.Add(assCost);

            var device = new TECDevice(new ObservableCollection<TECElectricalMaterial> { connectionType }, manufacturer);
            bid.Catalogs.Devices.Add(device);
            bid.Catalogs.ConnectionTypes.Add(connectionType);
            bid.Catalogs.ConduitTypes.Add(conduitType);
            bid.Catalogs.AssociatedCosts.Add(ratedCost);
            bid.Catalogs.Manufacturers.Add(manufacturer);
            bid.Catalogs.ControllerTypes.Add(controllerType);

            var system = new TECTypical();
            var equipment = new TECEquipment(true);
            var subScope = new TECSubScope(true);
            
            var controller = new TECController(controllerType, true);

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
            

            foreach (TECSystem instance in system.Instances)
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


            //For Both Conduit and Wire: 2*(length * type.Price/Labor + length * RatedCost.Cost/Labor + AssCost.Cost/Labor) = 2*(10 * 1 +10 * 1 + 2) + 2 * (5 * 1 + 5 * 1 + 2) = 40 + 10 = 54
            Assert.AreEqual(64, estimate.ElectricalLaborHours, "Electrical Labor Not Updating");
            Assert.AreEqual(64, estimate.ElectricalMaterialCost, "Electrical Material Not Updating");

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_RemoveSubScopeConnectionInSystem()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);

            var manufacturer = new TECManufacturer();
            manufacturer.Multiplier = 1;
            var controllerType = new TECControllerType(manufacturer);
            var ratedCost = new TECCost(CostType.TEC);
            ratedCost.Cost = 1;
            ratedCost.Labor = 1;
            ratedCost.Type = CostType.Electrical;

            var assCost = new TECCost(CostType.TEC);
            assCost.Cost = 1;
            assCost.Labor = 1;
            assCost.Type = CostType.Electrical;

            var connectionType = new TECElectricalMaterial();
            connectionType.Cost = 1;
            connectionType.Labor = 1;
            connectionType.RatedCosts.Add(ratedCost);
            connectionType.AssociatedCosts.Add(assCost);
            var conduitType = new TECElectricalMaterial();
            conduitType.Cost = 1;
            conduitType.Labor = 1;
            conduitType.RatedCosts.Add(ratedCost);

            var device = new TECDevice(new ObservableCollection<TECElectricalMaterial> { connectionType }, manufacturer);
            bid.Catalogs.Devices.Add(device);
            bid.Catalogs.ConnectionTypes.Add(connectionType);
            bid.Catalogs.ConduitTypes.Add(conduitType);
            bid.Catalogs.AssociatedCosts.Add(ratedCost);
            bid.Catalogs.Manufacturers.Add(manufacturer);
            conduitType.AssociatedCosts.Add(assCost);
            bid.Catalogs.ControllerTypes.Add(controllerType);

            var system = new TECTypical();
            var equipment = new TECEquipment(true);
            var subScope = new TECSubScope(true);

            var controller = new TECController(controllerType, true);

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

            foreach (TECSystem instance in system.Instances)
            {
                foreach (TECController instanceController in instance.Controllers)
                {
                    foreach (TECConnection instanceConnection in instanceController.ChildrenConnections)
                    {
                        instanceConnection.Length = 10;
                        instanceConnection.ConduitLength = 5;
                        instanceConnection.ConduitType = conduitType;
                    }
                }
            }

            controller.RemoveSubScope(subScope);

            //Assert
            assertNoCostOrLabor(estimate);

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_AddSubScopeConnectionInTypical()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);

            var manufacturer = new TECManufacturer();
            manufacturer.Multiplier = 1;
            var controllerType = new TECControllerType(manufacturer);
            var ratedCost = new TECCost(CostType.TEC);
            ratedCost.Cost = 1;
            ratedCost.Labor = 1;
            ratedCost.Type = CostType.Electrical;

            var connectionType = new TECElectricalMaterial();
            connectionType.Cost = 1;
            connectionType.Labor = 1;
            connectionType.RatedCosts.Add(ratedCost);
            var conduitType = new TECElectricalMaterial();
            conduitType.Cost = 1;
            conduitType.Labor = 1;
            conduitType.RatedCosts.Add(ratedCost);

            var device = new TECDevice(new ObservableCollection<TECElectricalMaterial> { connectionType }, manufacturer);
            bid.Catalogs.Devices.Add(device);
            bid.Catalogs.ConnectionTypes.Add(connectionType);
            bid.Catalogs.ConduitTypes.Add(conduitType);
            bid.Catalogs.AssociatedCosts.Add(ratedCost);
            bid.Catalogs.Manufacturers.Add(manufacturer);
            bid.Catalogs.ControllerTypes.Add(controllerType);

            var system = new TECTypical();
            var equipment = new TECEquipment(true);
            var subScope = new TECSubScope(true);

            var controller = new TECController(controllerType, false);
            bid.Controllers.Add(controller);

            system.Equipment.Add(equipment);
            equipment.SubScope.Add(subScope);
            subScope.Devices.Add(device);
            bid.Systems.Add(system);

            var connection = controller.AddSubScope(subScope);
            connection.Length = 10;
            connection.ConduitLength = 5;
            connection.ConduitType = conduitType;

            //For Both Conduit and Wire: 2*(length * type.Price/Labor + length * RatedCost.Cost/Labor) = 2*(10 * 1 +10 * 1) + 2 * (5 * 1 + 5 * 1) = 40 + 10 = 60
            Assert.AreEqual(0, estimate.ElectricalLaborHours, "Electrical Labor Not Updating");
            Assert.AreEqual(0, estimate.ElectricalMaterialCost, "Electrical Material Not Updating");
            assertNoCostOrLabor(estimate);

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_RemoveSubScopeConnectionInTypical()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);

            var manufacturer = new TECManufacturer();
            manufacturer.Multiplier = 1;
            var controllerType = new TECControllerType(manufacturer);
            var ratedCost = new TECCost(CostType.TEC);
            ratedCost.Cost = 1;
            ratedCost.Labor = 1;
            ratedCost.Type = CostType.Electrical;

            var connectionType = new TECElectricalMaterial();
            connectionType.Cost = 1;
            connectionType.Labor = 1;
            connectionType.RatedCosts.Add(ratedCost);
            var conduitType = new TECElectricalMaterial();
            conduitType.Cost = 1;
            conduitType.Labor = 1;
            conduitType.RatedCosts.Add(ratedCost);

            var device = new TECDevice(new ObservableCollection<TECElectricalMaterial> { connectionType }, manufacturer);
            bid.Catalogs.Devices.Add(device);
            bid.Catalogs.ConnectionTypes.Add(connectionType);
            bid.Catalogs.ConduitTypes.Add(conduitType);
            bid.Catalogs.AssociatedCosts.Add(ratedCost);
            bid.Catalogs.Manufacturers.Add(manufacturer);
            bid.Catalogs.ControllerTypes.Add(controllerType);

            var system = new TECTypical();
            var equipment = new TECEquipment(true);
            var subScope = new TECSubScope(true);

            var controller = new TECController(controllerType, false);
            bid.Controllers.Add(controller);

            system.Equipment.Add(equipment);
            equipment.SubScope.Add(subScope);
            subScope.Devices.Add(device);
            bid.Systems.Add(system);
            var connection = controller.AddSubScope(subScope);
            connection.Length = 10;
            connection.ConduitLength = 5;
            connection.ConduitType = conduitType;
            
            controller.RemoveSubScope(subScope);

            //Assert
            assertNoCostOrLabor(estimate);

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_AddMiscCost()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            
            var tecMisc = new TECMisc(CostType.TEC, false);
            tecMisc.Cost = 1234;
            tecMisc.Labor = 4321;

            var eMisc = new TECMisc(CostType.Electrical, false);
            eMisc.Cost = 5678;
            eMisc.Labor = 8765;

            bid.MiscCosts.Add(tecMisc);
            bid.MiscCosts.Add(eMisc);

            Assert.AreEqual(1234, estimate.TECMaterialCost, "Material cost incorrect");
            Assert.AreEqual(5678, estimate.ElectricalMaterialCost, "Electrical Material cost not added");
            Assert.AreEqual(4321, estimate.TECLaborHours, "Labor hours not added");
            Assert.AreEqual(8765, estimate.ElectricalLaborHours, "Electrical labor hours not added");

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_RemoveMiscCost()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);

            var tecMisc = new TECMisc(CostType.TEC, false);
            tecMisc.Cost = 1234;
            tecMisc.Labor = 4321;

            var eMisc = new TECMisc(CostType.Electrical, false);
            eMisc.Cost = 5678;
            eMisc.Labor = 8765;

            bid.MiscCosts.Add(tecMisc);
            bid.MiscCosts.Add(eMisc);

            bid.MiscCosts.Remove(tecMisc);
            bid.MiscCosts.Remove(eMisc);

            Assert.AreEqual(0, estimate.TECMaterialCost, "Material cost not removed");
            Assert.AreEqual(0, estimate.ElectricalMaterialCost, "Electrical Material cost not removed");
            Assert.AreEqual(0, estimate.TECLaborHours, "Labor hours not removed");
            Assert.AreEqual(0, estimate.ElectricalLaborHours, "Electrical labor hours not removed");

            checkRefresh(bid, estimate);
        }
        
        [TestMethod]
        public void Estimate_AddMiscCostFromSystem()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            var system = new TECTypical();
            bid.Systems.Add(system);
            system.AddInstance(bid);
            system.AddInstance(bid);

            var tecMisc = new TECMisc(CostType.TEC, true);
            tecMisc.Cost = 1234;
            tecMisc.Labor = 4321;

            var eMisc = new TECMisc(CostType.Electrical, true);
            eMisc.Cost = 5678;
            eMisc.Labor = 8765;

            system.MiscCosts.Add(tecMisc);
            system.MiscCosts.Add(eMisc);

            Assert.AreEqual(2468, estimate.TECMaterialCost, "Material cost not added");
            Assert.AreEqual(11356, estimate.ElectricalMaterialCost, "Electrical Material cost not added");
            Assert.AreEqual(8642, estimate.TECLaborHours, "Labor hours not added");
            Assert.AreEqual(17530, estimate.ElectricalLaborHours, "Electrical labor hours not added");

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_RemoveMiscCostFromSystem()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            var system = new TECTypical();
            bid.Systems.Add(system);
            system.AddInstance(bid);
            system.AddInstance(bid);

            var tecMisc = new TECMisc(CostType.TEC, true);
            tecMisc.Cost = 1234;
            tecMisc.Labor = 4321;

            var eMisc = new TECMisc(CostType.Electrical, true);
            eMisc.Cost = 5678;
            eMisc.Labor = 8765;

            system.MiscCosts.Add(tecMisc);
            system.MiscCosts.Add(eMisc);

            system.MiscCosts.Remove(tecMisc);
            system.MiscCosts.Remove(eMisc);

            Assert.AreEqual(0, estimate.TECMaterialCost, "Material cost not added");
            Assert.AreEqual(0, estimate.ElectricalMaterialCost, "Electrical Material cost not added");
            Assert.AreEqual(0, estimate.TECLaborHours, "Labor hours not added");
            Assert.AreEqual(0, estimate.ElectricalLaborHours, "Electrical labor hours not added");

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_AddAssCostToSystem()
        {
            var bid = new TECBid();
            var watcher = new ChangeWatcher(bid);
            var estimate = new TECEstimator(bid, watcher);
            var system = new TECTypical();
            bid.Systems.Add(system);
            system.AddInstance(bid);
            system.AddInstance(bid);

            var tecCost = new TECCost(CostType.TEC);
            tecCost.Cost = 1234;
            tecCost.Labor = 4321;

            var eCost = new TECCost(CostType.Electrical);
            eCost.Cost = 5678;
            eCost.Labor = 8765;

            system.AssociatedCosts.Add(tecCost);
            system.AssociatedCosts.Add(eCost);

            Assert.AreEqual(2468, estimate.TECMaterialCost, "Material cost not added");
            Assert.AreEqual(11356, estimate.ElectricalMaterialCost, "Electrical Material cost not added");
            Assert.AreEqual(8642, estimate.TECLaborHours, "Labor hours not added");
            Assert.AreEqual(17530, estimate.ElectricalLaborHours, "Electrical labor hours not added");

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_RemoveAssCostRemoveSystem()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            var system = new TECTypical();
            bid.Systems.Add(system);
            system.AddInstance(bid);
            system.AddInstance(bid);

            var tecCost = new TECCost(CostType.TEC);
            tecCost.Cost = 1234;
            tecCost.Labor = 4321;
            tecCost.Type = CostType.TEC;

            var eCost = new TECCost(CostType.TEC);
            eCost.Cost = 5678;
            eCost.Labor = 8765;
            eCost.Type = CostType.Electrical;

            system.AssociatedCosts.Add(tecCost);
            system.AssociatedCosts.Add(eCost);

            system.AssociatedCosts.Remove(tecCost);
            system.AssociatedCosts.Remove(eCost);

            Assert.AreEqual(0, estimate.TECMaterialCost, "Material cost not removed");
            Assert.AreEqual(0, estimate.ElectricalMaterialCost, "Electrical Material cost not removed");
            Assert.AreEqual(0, estimate.TECLaborHours, "Labor hours not removed");
            Assert.AreEqual(0, estimate.ElectricalLaborHours, "Electrical labor hours not removed");

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_AddAssCostFromEquipemnt()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            var system = new TECTypical();
            var equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            bid.Systems.Add(system);
            system.AddInstance(bid);
            system.AddInstance(bid);

            var tecCost = new TECCost(CostType.TEC);
            tecCost.Cost = 1234;
            tecCost.Labor = 4321;
            tecCost.Type = CostType.TEC;

            var eCost = new TECCost(CostType.TEC);
            eCost.Cost = 5678;
            eCost.Labor = 8765;
            eCost.Type = CostType.Electrical;

            equipment.AssociatedCosts.Add(tecCost);
            equipment.AssociatedCosts.Add(eCost);

            Assert.AreEqual(2468, estimate.TECMaterialCost, "Material cost not added");
            Assert.AreEqual(11356, estimate.ElectricalMaterialCost, "Electrical Material cost not added");
            Assert.AreEqual(8642, estimate.TECLaborHours, "Labor hours not added");
            Assert.AreEqual(17530, estimate.ElectricalLaborHours, "Electrical labor hours not added");

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_RemoveAssCostFromEquipemnt()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            var system = new TECTypical();
            var equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            bid.Systems.Add(system);
            system.AddInstance(bid);
            system.AddInstance(bid);

            var tecCost = new TECCost(CostType.TEC);
            tecCost.Cost = 1234;
            tecCost.Labor = 4321;
            tecCost.Type = CostType.TEC;

            var eCost = new TECCost(CostType.TEC);
            eCost.Cost = 5678;
            eCost.Labor = 8765;
            eCost.Type = CostType.Electrical;

            equipment.AssociatedCosts.Add(tecCost);
            equipment.AssociatedCosts.Add(eCost);

            equipment.AssociatedCosts.Remove(tecCost);
            equipment.AssociatedCosts.Remove(eCost);

            Assert.AreEqual(0, estimate.TECMaterialCost, "Material cost not removed");
            Assert.AreEqual(0, estimate.ElectricalMaterialCost, "Electrical Material cost not removed");
            Assert.AreEqual(0, estimate.TECLaborHours, "Labor hours not removed");
            Assert.AreEqual(0, estimate.ElectricalLaborHours, "Electrical labor hours not removed");

            checkRefresh(bid, estimate);
        }
        
        [TestMethod]
        public void Estimate_AddDeviceToSubScope()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            var system = new TECTypical();
            var equipment = new TECEquipment(true);
            var subScope = new TECSubScope(true);

            equipment.SubScope.Add(subScope);
            system.Equipment.Add(equipment);
            bid.Systems.Add(system);
            system.AddInstance(bid);
            system.AddInstance(bid);

            var manufacturer = new TECManufacturer();
            manufacturer.Multiplier = 1;

            var device = new TECDevice(new ObservableCollection<TECElectricalMaterial> { new TECElectricalMaterial() }, manufacturer);
            device.Price = 100;
            bid.Catalogs.Devices.Add(device);

            subScope.Devices.Add(device);
            
            Assert.AreEqual(200, estimate.TECMaterialCost, "Material cost not added");

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_RemoveDeviceToSubScope()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            var system = new TECTypical();
            var equipment = new TECEquipment(true);
            var subScope = new TECSubScope(true);

            equipment.SubScope.Add(subScope);
            system.Equipment.Add(equipment);
            bid.Systems.Add(system);
            system.AddInstance(bid);
            system.AddInstance(bid);

            var manufacturer = new TECManufacturer();
            manufacturer.Multiplier = 1;

            var device = new TECDevice(new ObservableCollection<TECElectricalMaterial> { new TECElectricalMaterial() }, manufacturer);
            device.Price = 100;
            bid.Catalogs.Devices.Add(device);

            subScope.Devices.Add(device);

            subScope.Devices.Remove(device);

            Assert.AreEqual(0, estimate.TECMaterialCost, "Material cost not added");

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_AddDeviceWithMultiplierToSubScope()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            var system = new TECTypical();
            var equipment = new TECEquipment(true);
            var subScope = new TECSubScope(true);

            equipment.SubScope.Add(subScope);
            system.Equipment.Add(equipment);
            bid.Systems.Add(system);
            system.AddInstance(bid);
            system.AddInstance(bid);

            var manufacturer = new TECManufacturer();
            manufacturer.Multiplier = 0.5;

            var device = new TECDevice(new ObservableCollection<TECElectricalMaterial> { new TECElectricalMaterial() }, manufacturer);
            device.Price = 100;
            bid.Catalogs.Devices.Add(device);

            subScope.Devices.Add(device);

            Assert.AreEqual(100, estimate.TECMaterialCost, "Material cost not added");

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_RemoveDeviceWithMultiplierToSubScope()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            var system = new TECTypical();
            var equipment = new TECEquipment(true);
            var subScope = new TECSubScope(true);

            equipment.SubScope.Add(subScope);
            system.Equipment.Add(equipment);
            bid.Systems.Add(system);
            system.AddInstance(bid);
            system.AddInstance(bid);

            var manufacturer = new TECManufacturer();
            manufacturer.Multiplier = 0.5;

            var device = new TECDevice(new ObservableCollection<TECElectricalMaterial> { new TECElectricalMaterial() }, manufacturer);
            device.Price = 100;
            bid.Catalogs.Devices.Add(device);

            subScope.Devices.Add(device);

            subScope.Devices.Remove(device);

            Assert.AreEqual(0, estimate.TECMaterialCost, "Material cost not added");

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_AddNetworkConnection()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            var manufacturer = new TECManufacturer();

            TECIO io = new TECIO();
            io.Type = IOType.BACnetIP;
            var controllerType = new TECControllerType(manufacturer);
            controllerType.IO.Add(io);

            var connectionType = new TECElectricalMaterial();
            connectionType.Cost = 1;
            connectionType.Labor = 1;

            var conduitType = new TECElectricalMaterial();
            conduitType.Cost = 1;
            conduitType.Labor = 1;

            var controller1 = new TECController(controllerType, false);
            var controller2 = new TECController(controllerType, false);

            bid.Controllers.Add(controller1);
            bid.Controllers.Add(controller2);
            
            var connection = controller1.AddController(controller2, connectionType);
            connection.Length = 50;
            connection.ConduitLength = 50;
            connection.ConduitType = conduitType;

            Assert.AreEqual(100, estimate.ElectricalLaborHours, "Electrical Labor Not Updating");
            Assert.AreEqual(100, estimate.ElectricalMaterialCost, "Electrical Material Not Updating");

            checkRefresh(bid, estimate);
        }
        
        [TestMethod]
        public void Estimate_RemoveNetworkConnection()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            var manufacturer = new TECManufacturer();
            var controllerType = new TECControllerType(manufacturer);

            TECIO io = new TECIO();
            io.Type = IOType.BACnetIP;
            controllerType.IO.Add(io);

            var connectionType = new TECElectricalMaterial();
            connectionType.Cost = 1;
            connectionType.Labor = 1;

            var controller1 = new TECController(controllerType, false);
            var controller2 = new TECController(controllerType, false);

            bid.Controllers.Add(controller1);
            bid.Controllers.Add(controller2);

            var connection = controller1.AddController(controller2, connectionType);
            connection.Length = 50;

            controller1.RemoveController(controller2);

            Assert.AreEqual(0, estimate.ElectricalLaborHours, "Electrical Labor Not Updating");
            Assert.AreEqual(0, estimate.ElectricalMaterialCost, "Electrical Material Not Updating");

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_AddNetworkConnectionToSystem()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            var manufacturer = new TECManufacturer();
            var system = new TECTypical();
            bid.Systems.Add(system);
            var controllerType = new TECControllerType(manufacturer);

            TECIO io = new TECIO();
            io.Type = IOType.BACnetIP;
            controllerType.IO.Add(io);

            var connectionType = new TECElectricalMaterial();
            connectionType.Cost = 1;
            connectionType.Labor = 1;

            var controller1 = new TECController(controllerType, false);
            var controller2 = new TECController(controllerType, true);

            bid.Controllers.Add(controller1);
            system.Controllers.Add(controller2);
            system.AddInstance(bid);
            var instanceController = system.Instances[0].Controllers[0];

            var connection = controller1.AddController(instanceController, connectionType);
            connection.Length = 50;

            Assert.AreEqual(50, estimate.ElectricalLaborHours, "Electrical Labor Not Updating");
            Assert.AreEqual(50, estimate.ElectricalMaterialCost, "Electrical Material Not Updating");

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_RemoveNetworkConnectionToSystem()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            var manufacturer = new TECManufacturer();
            var system = new TECTypical();
            bid.Systems.Add(system);
            var controllerType = new TECControllerType(manufacturer);

            TECIO io = new TECIO();
            io.Type = IOType.BACnetIP;
            controllerType.IO.Add(io);

            var connectionType = new TECElectricalMaterial();
            connectionType.Cost = 1;
            connectionType.Labor = 1;

            var controller1 = new TECController(controllerType, false);
            var controller2 = new TECController(controllerType, true);

            bid.Controllers.Add(controller1);
            system.Controllers.Add(controller2);
            system.AddInstance(bid);
            var instanceController = system.Instances[0].Controllers[0];

            var connection = controller1.AddController(instanceController, connectionType);
            connection.Length = 50;

            controller1.RemoveController(instanceController);

            Assert.AreEqual(0, estimate.ElectricalLaborHours, "Electrical Labor Not Updating");
            Assert.AreEqual(0, estimate.ElectricalMaterialCost, "Electrical Material Not Updating");

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_AddSubScopeToSystem()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            var manufacturer = new TECManufacturer();
            var connectionType = new TECElectricalMaterial();
            connectionType.Cost = 1;
            connectionType.Labor = 1;
            var system = new TECTypical();
            bid.Systems.Add(system);

            var equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            system.AddInstance(bid);

            var device = new TECDevice(new ObservableCollection<TECElectricalMaterial> { connectionType }, manufacturer);
            device.Price = 10;
            var subScope = new TECSubScope(true);
            subScope.Devices.Add(device);

            equipment.SubScope.Add(subScope);

            Assert.AreEqual(10, estimate.TECMaterialCost, "TECMaterialCost Not Updating");

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_RemoveSubScopeFromSystem()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            var manufacturer = new TECManufacturer();
            var connectionType = new TECElectricalMaterial();
            connectionType.Cost = 1;
            connectionType.Labor = 1;
            var system = new TECTypical();
            bid.Systems.Add(system);

            var equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            system.AddInstance(bid);

            var device = new TECDevice(new ObservableCollection<TECElectricalMaterial> { connectionType }, manufacturer);
            device.Price = 10;
            var subScope = new TECSubScope(true);
            subScope.Devices.Add(device);

            equipment.SubScope.Add(subScope);

            equipment.SubScope.Remove(subScope);

            Assert.AreEqual(0, estimate.TECMaterialCost, "TECMaterialCost Not Updating");

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_AddEquipmentToSystem()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            var manufacturer = new TECManufacturer();
            var connectionType = new TECElectricalMaterial();
            connectionType.Cost = 1;
            connectionType.Labor = 1;
            var system = new TECTypical();
            bid.Systems.Add(system);
            system.AddInstance(bid);

            var equipment = new TECEquipment(true);
            var device = new TECDevice(new ObservableCollection<TECElectricalMaterial> { connectionType }, manufacturer);
            device.Price = 10;
            var subScope = new TECSubScope(true);
            subScope.Devices.Add(device);
            equipment.SubScope.Add(subScope);

            system.Equipment.Add(equipment);

            Assert.AreEqual(10, estimate.TECMaterialCost, "TECMaterialCost Not Updating");

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_RemoveEquipmentToSystem()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            var manufacturer = new TECManufacturer();
            var connectionType = new TECElectricalMaterial();
            connectionType.Cost = 1;
            connectionType.Labor = 1;
            var system = new TECTypical();
            bid.Systems.Add(system);
            system.AddInstance(bid);

            var equipment = new TECEquipment(true);
            var device = new TECDevice(new ObservableCollection<TECElectricalMaterial> { connectionType }, manufacturer);
            device.Price = 10;
            var subScope = new TECSubScope(true);
            subScope.Devices.Add(device);
            equipment.SubScope.Add(subScope);

            system.Equipment.Add(equipment);

            system.Equipment.Remove(equipment);

            Assert.AreEqual(0, estimate.TECMaterialCost, "TECMaterialCost Not Updating");

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_Tax()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            var manufacturer = new TECManufacturer();
            var connectionType = new TECElectricalMaterial();
            connectionType.Cost = 1;
            connectionType.Labor = 1;
            var system = new TECTypical();
            bid.Systems.Add(system);
            system.AddInstance(bid);

            var equipment = new TECEquipment(true);
            var device = new TECDevice(new ObservableCollection<TECElectricalMaterial> { connectionType }, manufacturer);
            device.Price = 10;
            var subScope = new TECSubScope(true);
            subScope.Devices.Add(device);
            equipment.SubScope.Add(subScope);

            system.Equipment.Add(equipment);

            Assert.AreEqual(0.875, estimate.Tax);

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_TaxExempt()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            var manufacturer = new TECManufacturer();
            var connectionType = new TECElectricalMaterial();
            connectionType.Cost = 1;
            connectionType.Labor = 1;
            var system = new TECTypical();
            bid.Systems.Add(system);
            system.AddInstance(bid);

            var equipment = new TECEquipment(true);
            var device = new TECDevice(new ObservableCollection<TECElectricalMaterial> { connectionType }, manufacturer);
            device.Price = 10;
            var subScope = new TECSubScope(true);
            subScope.Devices.Add(device);
            equipment.SubScope.Add(subScope);

            system.Equipment.Add(equipment);

            bid.Parameters.IsTaxExempt = true;

            Assert.AreEqual(0, estimate.Tax);

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_TECShipping()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            var manufacturer = new TECManufacturer();
            var connectionType = new TECElectricalMaterial();
            connectionType.Cost = 1;
            connectionType.Labor = 1;
            var system = new TECTypical();
            bid.Systems.Add(system);
            system.AddInstance(bid);

            var equipment = new TECEquipment(true);
            var device = new TECDevice(new ObservableCollection<TECElectricalMaterial> { connectionType }, manufacturer);
            device.Price = 10;
            var subScope = new TECSubScope(true);
            subScope.Devices.Add(device);
            equipment.SubScope.Add(subScope);

            system.Equipment.Add(equipment);
            
            Assert.AreEqual(0.3, estimate.TECShipping);

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_TECWarranty()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            var manufacturer = new TECManufacturer();
            var connectionType = new TECElectricalMaterial();
            connectionType.Cost = 1;
            connectionType.Labor = 1;
            var system = new TECTypical();
            bid.Systems.Add(system);
            system.AddInstance(bid);

            var equipment = new TECEquipment(true);
            var device = new TECDevice(new ObservableCollection<TECElectricalMaterial> { connectionType }, manufacturer);
            device.Price = 10;
            var subScope = new TECSubScope(true);
            subScope.Devices.Add(device);
            equipment.SubScope.Add(subScope);

            system.Equipment.Add(equipment);

            Assert.AreEqual(0.5, estimate.TECWarranty);

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_ElectricalShipping()
        {
            var bid = new TECBid();
            var watcher = new ChangeWatcher(bid);
            var estimate = new TECEstimator(bid, watcher);

            var system = new TECTypical();
            var equipment = new TECEquipment(true);
            var subScope = new TECSubScope(true);

            var manufacturer = new TECManufacturer();
            manufacturer.Multiplier = 1;
            var controllerType = new TECControllerType(manufacturer);

            var controller = new TECController(controllerType, true);

            equipment.SubScope.Add(subScope);
            system.Equipment.Add(equipment);
            system.Controllers.Add(controller);
            bid.Systems.Add(system);

            var ratedCost = new TECCost(CostType.TEC);
            ratedCost.Cost = 1;
            ratedCost.Labor = 1;
            ratedCost.Type = CostType.Electrical;

            var connectionType = new TECElectricalMaterial();
            connectionType.Cost = 1;
            connectionType.Labor = 1;
            connectionType.RatedCosts.Add(ratedCost);
            var conduitType = new TECElectricalMaterial();
            conduitType.Cost = 1;
            conduitType.Labor = 1;
            conduitType.RatedCosts.Add(ratedCost);

            var device = new TECDevice(new ObservableCollection<TECElectricalMaterial> { connectionType }, manufacturer);
            bid.Catalogs.Devices.Add(device);

            subScope.Devices.Add(device);

            var connection = controller.AddSubScope(subScope);
            connection.Length = 10;
            connection.ConduitLength = 5;
            connection.ConduitType = conduitType;

            system.AddInstance(bid);
            system.AddInstance(bid);

            //For Both Conduit and Wire Cost: 2*(length * type.Price/Labor + length * RatedCost.Cost/Labor) = 2*(10 * 1 +10 * 1) + 2 * (5 * 1 + 5 * 1) = 40 + 10 = 60
            Assert.AreEqual(1.8, estimate.ElectricalShipping, CostTestingUtilities.DELTA);

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_ElectricalWarranty()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);

            var system = new TECTypical();
            var equipment = new TECEquipment(true);
            var subScope = new TECSubScope(true);

            var manufacturer = new TECManufacturer();
            manufacturer.Multiplier = 1;
            var controllerType = new TECControllerType(manufacturer);

            var controller = new TECController(controllerType, true);

            equipment.SubScope.Add(subScope);
            system.Equipment.Add(equipment);
            system.Controllers.Add(controller);
            bid.Systems.Add(system);

            var ratedCost = new TECCost(CostType.TEC);
            ratedCost.Cost = 1;
            ratedCost.Labor = 1;
            ratedCost.Type = CostType.Electrical;

            var connectionType = new TECElectricalMaterial();
            connectionType.Cost = 1;
            connectionType.Labor = 1;
            connectionType.RatedCosts.Add(ratedCost);
            var conduitType = new TECElectricalMaterial();
            conduitType.Cost = 1;
            conduitType.Labor = 1;
            conduitType.RatedCosts.Add(ratedCost);

            var device = new TECDevice(new ObservableCollection<TECElectricalMaterial> { connectionType }, manufacturer);
            bid.Catalogs.Devices.Add(device);

            subScope.Devices.Add(device);

            var connection = controller.AddSubScope(subScope);
            connection.Length = 10;
            connection.ConduitLength = 5;
            connection.ConduitType = conduitType;

            system.AddInstance(bid);
            system.AddInstance(bid);

            //For Both Conduit and Wire Cost: 2*(length * type.Price/Labor + length * RatedCost.Cost/Labor) = 2*(10 * 1 +10 * 1) + 2 * (5 * 1 + 5 * 1) = 40 + 20 = 60
            Assert.AreEqual(3, estimate.ElectricalWarranty);

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_RemoveInstanceSystemWithBidControllerSubscopeConnection()
        {
            TECBid bid = new TECBid();
            ChangeWatcher watcher = new ChangeWatcher(bid);
            TECEstimator estimate = new TECEstimator(bid, watcher);
            bid.Catalogs = TestHelper.CreateTestCatalogs();
            TECControllerType type = new TECControllerType(new TECManufacturer());

            TECController controller = new TECController(type, false);
            bid.Controllers.Add(controller);

            TECTypical typical = new TECTypical();
            TECEquipment equipment = new TECEquipment(true);
            typical.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);

            TECSubScopeConnection ssConnect = controller.AddSubScope(subScope);
            ssConnect.Length = 50;

            typical.Instances.Remove(instance);

            //Assert
            assertNoCostOrLabor(estimate);

            checkRefresh(bid, estimate);
        }
        
        #region Derived Labor
        [TestMethod]
        public void Estimate_TECLaborHoursFromPoints()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            bid.Parameters = parameters;
            var system = new TECTypical();
            var equipment = new TECEquipment(true);
            var subScope = new TECSubScope(true);
            var point = new TECPoint(true);
            point.Type = PointTypes.AI;
            point.Quantity = 1;

            subScope.Points.Add(point);
            equipment.SubScope.Add(subScope);
            system.Equipment.Add(equipment);
            bid.Systems.Add(system);
            
            system.AddInstance(bid);
            system.AddInstance(bid);

            Assert.AreEqual(2, estimate.TotalPointNumber, "Total points not updating");
            Assert.AreEqual(3.08, estimate.PMLaborHours, "PM labor calcualtion");
            Assert.AreEqual(2.5, estimate.ENGLaborHours, "ENG labor calcualtion");
            Assert.AreEqual(0.74, estimate.SoftLaborHours, "Software labor calcualtion");
            Assert.AreEqual(1.06, estimate.GraphLaborHours, "Graphics labor calcualtion");
            Assert.AreEqual(2.68, estimate.CommLaborHours, "Comm labor calcualtion");

            checkRefresh(bid, estimate);
        }

        [TestMethod]
        public void Estimate_TECLaborHoursFromLump()
        {
            var bid = new TECBid(); var watcher = new ChangeWatcher(bid); var estimate = new TECEstimator(bid, watcher);
            bid.Parameters = parameters;
            bid.ExtraLabor.PMExtraHours = 10;
            bid.ExtraLabor.ENGExtraHours = 10;
            bid.ExtraLabor.SoftExtraHours = 10;
            bid.ExtraLabor.GraphExtraHours = 10;
            bid.ExtraLabor.CommExtraHours = 10;

            Assert.AreEqual(10, estimate.PMLaborHours, "PM labor calcualtion");
            Assert.AreEqual(10, estimate.ENGLaborHours, "PM labor calcualtion");
            Assert.AreEqual(10, estimate.SoftLaborHours, "Software labor calcualtion");
            Assert.AreEqual(10, estimate.GraphLaborHours, "Graphics labor calcualtion");
            Assert.AreEqual(10, estimate.CommLaborHours, "Comm labor calcualtion");

            checkRefresh(bid, estimate);
        }
        
        #endregion

        private void assertNoCostOrLabor(TECEstimator estimate)
        {
            Assert.AreEqual(0, estimate.TECMaterialCost, 0.0001);
            Assert.AreEqual(0, estimate.TECLaborHours, 0.0001);
            Assert.AreEqual(0, estimate.ElectricalMaterialCost, 0.0001);
            Assert.AreEqual(0, estimate.ElectricalLaborHours, 0.0001);
        }

        private void checkRefresh(TECBid bid, TECEstimator estimate)
        {
            double tecCost = estimate.TECMaterialCost;
            double tecLabor = estimate.TECLaborHours;
            double elecCost = estimate.ElectricalMaterialCost;
            double elecLabor = estimate.ElectricalLaborHours;
            double total = estimate.TotalPrice;

            estimate = new TECEstimator(bid, new ChangeWatcher(bid));

            Assert.AreEqual(tecCost, estimate.TECMaterialCost, 0.0001, "TEC material cost refresh failed.");
            Assert.AreEqual(tecLabor, estimate.TECLaborHours, 0.0001, "TEC labor hours refresh failed.");
            Assert.AreEqual(elecCost, estimate.ElectricalMaterialCost, 0.0001, "Electrtical material cost refresh failed.");
            Assert.AreEqual(elecLabor, estimate.ElectricalLaborHours, 0.0001, "Elecrtical labor hours refresh failed.");
            Assert.AreEqual(total, estimate.TotalPrice, 0.0001, "Total price refresh failed.");
        }
    }
}
