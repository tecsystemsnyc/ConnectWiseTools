using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EstimatingLibrary;
using TECUserControlLibrary.ViewModels;
using System.Collections.ObjectModel;
using EstimatingLibrary.Utilities;

namespace Tests
{
    /// <summary>
    /// Summary description for SummaryVMTests
    /// </summary>
    [TestClass]
    public class SummaryVMTests
    {
        private static double delta = 1.0 / 1000.0;
        
        #region Material Summary VM
        #region Add
        [TestMethod]
        public void AddTECCostToSystem()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            ChangeWatcher cw = new ChangeWatcher(bid);
            bid.Catalogs = TestHelper.CreateTestCatalogs();
            TECCost cost = null;
            while(cost == null)
            {
                TECCost randomCost = bid.Catalogs.AssociatedCosts.RandomObject();
                if (randomCost.Type == CostType.TEC)
                {
                    cost = randomCost;
                }
            }

            Total totalTEC = calculateTotal(cost, CostType.TEC);
            Total totalElec = calculateTotal(cost, CostType.Electrical);

            TECSystem typical = new TECSystem();
            bid.Systems.Add(typical);
            typical.AddInstance(bid);

            MaterialSummaryVM matVM = new MaterialSummaryVM(bid, cw);

            //Act
            typical.AssociatedCosts.Add(cost);

            //Assert
            Assert.AreEqual(matVM.TotalTECCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalTECLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(matVM.TotalElecCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(matVM.TotalElecLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(matVM, bid, cw);
        }

        [TestMethod]
        public void AddElectricalCostToSystem()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            ChangeWatcher cw = new ChangeWatcher(bid);
            TECCost cost = null;
            while (cost == null)
            {
                TECCost randomCost = bid.Catalogs.AssociatedCosts.RandomObject();
                if (randomCost.Type == CostType.Electrical)
                {
                    cost = randomCost;
                }
            }

            Total totalTEC = calculateTotal(cost, CostType.TEC);
            Total totalElec = calculateTotal(cost, CostType.Electrical);

            TECSystem typical = new TECSystem();
            bid.Systems.Add(typical);
            typical.AddInstance(bid);

            MaterialSummaryVM matVM = new MaterialSummaryVM(bid, cw);

            //Act
            typical.AssociatedCosts.Add(cost);

            //Assert
            Assert.AreEqual(matVM.TotalTECCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalTECLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(matVM.TotalElecCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(matVM.TotalElecLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(matVM, bid, cw);
        }

        [TestMethod]
        public void AddTECMiscToBid()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            ChangeWatcher cw = new ChangeWatcher(bid);
            TECMisc misc = TestHelper.CreateTestMisc(CostType.TEC);

            Total totalTEC = calculateTotal(misc, CostType.TEC);
            Total totalElec = calculateTotal(misc, CostType.Electrical);

            MaterialSummaryVM matVM = new MaterialSummaryVM(bid, cw);

            //Act
            bid.MiscCosts.Add(misc);

            //Assert
            Assert.AreEqual(matVM.TotalTECCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalTECLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(matVM.TotalElecCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(matVM.TotalElecLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(matVM, bid, cw);
        }

        [TestMethod]
        public void AddElectricalMiscToBid()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            ChangeWatcher cw = new ChangeWatcher(bid);
            TECMisc misc = TestHelper.CreateTestMisc(CostType.Electrical);

            Total totalTEC = calculateTotal(misc, CostType.TEC);
            Total totalElec = calculateTotal(misc, CostType.Electrical);

            MaterialSummaryVM matVM = new MaterialSummaryVM(bid, cw);

            //Act
            bid.MiscCosts.Add(misc);

            //Assert
            Assert.AreEqual(matVM.TotalTECCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalTECLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(matVM.TotalElecCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(matVM.TotalElecLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(matVM, bid, cw);
        }

        [TestMethod]
        public void AddTECMiscToSystem()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            ChangeWatcher cw = new ChangeWatcher(bid);
            TECMisc misc = TestHelper.CreateTestMisc(CostType.TEC);

            Total totalTEC = calculateTotal(misc, CostType.TEC);
            Total totalElec = calculateTotal(misc, CostType.Electrical);

            TECSystem typical = new TECSystem();
            bid.Systems.Add(typical);
            typical.AddInstance(bid);

            MaterialSummaryVM matVM = new MaterialSummaryVM(bid, cw);

            //Act
            typical.MiscCosts.Add(misc);

            //Arrange
            Assert.AreEqual(matVM.TotalTECCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalTECLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(matVM.TotalElecCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(matVM.TotalElecLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(matVM, bid, cw);
        }

        [TestMethod]
        public void AddElectricalMiscToSystem()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            ChangeWatcher cw = new ChangeWatcher(bid);
            TECMisc misc = TestHelper.CreateTestMisc(CostType.Electrical);

            Total totalTEC = calculateTotal(misc, CostType.TEC);
            Total totalElec = calculateTotal(misc, CostType.Electrical);

            TECSystem typical = new TECSystem();
            bid.Systems.Add(typical);
            typical.AddInstance(bid);

            MaterialSummaryVM matVM = new MaterialSummaryVM(bid, cw);

            //Act
            typical.MiscCosts.Add(misc);
            
            //Assert
            Assert.AreEqual(matVM.TotalTECCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalTECLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(matVM.TotalElecCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(matVM.TotalTECLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(matVM, bid, cw);
        }

        [TestMethod]
        public void AddPanel()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            ChangeWatcher cw = new ChangeWatcher(bid);
            TECPanel panel = TestHelper.CreateTestPanel(bid.Catalogs);
            TestHelper.AssignSecondaryProperties(panel, bid.Catalogs);

            Total totalTEC = calculateTotal(panel, CostType.TEC);
            Total totalElec = calculateTotal(panel, CostType.Electrical);

            MaterialSummaryVM matVM = new MaterialSummaryVM(bid, cw);

            //Act
            bid.Panels.Add(panel);

            //Assert
            Assert.AreEqual(matVM.TotalTECCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalTECLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(matVM.TotalElecCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(matVM.TotalElecLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(matVM, bid, cw);
        }

        [TestMethod]
        public void AddController()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            ChangeWatcher cw = new ChangeWatcher(bid);
            TECController controller = TestHelper.CreateTestController(bid.Catalogs);
            TestHelper.AssignSecondaryProperties(controller, bid.Catalogs);

            Total totalTEC = calculateTotal(controller, CostType.TEC);
            Total totalElec = calculateTotal(controller, CostType.Electrical);

            MaterialSummaryVM matVM = new MaterialSummaryVM(bid, cw);

            //Act
            bid.Controllers.Add(controller);
            
            //Assert
            Assert.AreEqual(matVM.TotalTECCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalTECLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(matVM.TotalElecCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(matVM.TotalElecLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(matVM, bid, cw);
        }

        [TestMethod]
        public void AddDevice()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            ChangeWatcher cw = new ChangeWatcher(bid);
            TECDevice device = TestHelper.CreateTestDevice(bid.Catalogs);
            TestHelper.AssignSecondaryProperties(device, bid.Catalogs);

            Total totalTEC = calculateTotal(device, CostType.TEC);
            Total totalElec = calculateTotal(device, CostType.Electrical);

            TECSystem typical = new TECSystem();
            bid.Systems.Add(typical);

            TECEquipment typEquip = new TECEquipment();
            typical.Equipment.Add(typEquip);

            TECSubScope typSS = new TECSubScope();
            typEquip.SubScope.Add(typSS);

            typical.AddInstance(bid);

            MaterialSummaryVM matVM = new MaterialSummaryVM(bid, cw);

            //Act
            typSS.Devices.Add(device);

            //Assert
            Assert.AreEqual(matVM.TotalTECCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalTECLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(matVM.TotalElecCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(matVM.TotalElecLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(matVM, bid, cw);
        }

        [TestMethod]
        public void AddSubScope()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            ChangeWatcher cw = new ChangeWatcher(bid);
            TECSubScope subscope = TestHelper.CreateTestSubScope(bid.Catalogs);
            TestHelper.AssignSecondaryProperties(subscope, bid.Catalogs);

            Total totalTEC = calculateTotal(subscope, CostType.TEC);
            Total totalElec = calculateTotal(subscope, CostType.Electrical);

            TECSystem typical = new TECSystem();
            bid.Systems.Add(typical);

            TECEquipment typEquip = new TECEquipment();
            typical.Equipment.Add(typEquip);

            typical.AddInstance(bid);

            MaterialSummaryVM matVM = new MaterialSummaryVM(bid, cw);

            //Act
            typEquip.SubScope.Add(subscope);
            
            //Assert
            Assert.AreEqual(matVM.TotalTECCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalTECLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(matVM.TotalElecCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(matVM.TotalElecLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(matVM, bid, cw);
        }

        [TestMethod]
        public void AddEquipment()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            ChangeWatcher cw = new ChangeWatcher(bid);
            TECEquipment equipment = TestHelper.CreateTestEquipment(bid.Catalogs);
            TestHelper.AssignSecondaryProperties(equipment, bid.Catalogs);

            Total totalTEC = calculateTotal(equipment, CostType.TEC);
            Total totalElec = calculateTotal(equipment, CostType.Electrical);

            TECSystem typical = new TECSystem();
            bid.Systems.Add(typical);

            typical.AddInstance(bid);

            MaterialSummaryVM matVM = new MaterialSummaryVM(bid, cw);
            
            //Act
            typical.Equipment.Add(equipment);

            //Assert
            Assert.AreEqual(matVM.TotalTECCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalTECLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(matVM.TotalElecCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(matVM.TotalElecLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(matVM, bid, cw);
        }

        [TestMethod]
        public void AddInstanceSystem()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            ChangeWatcher cw = new ChangeWatcher(bid);
            TECSystem typical = TestHelper.CreateTestSystem(bid.Catalogs);
            TestHelper.AssignSecondaryProperties(typical, bid.Catalogs);
            bid.Systems.Add(typical);

            MaterialSummaryVM matVM = new MaterialSummaryVM(bid, cw);

            //Act
            TECSystem instance = typical.AddInstance(bid);

            Total totalTEC = calculateTotalInstanceSystem(instance, typical, CostType.TEC);
            Total totalElec = calculateTotalInstanceSystem(instance, typical, CostType.Electrical);

            //Assert
            Assert.AreEqual(matVM.TotalTECCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalTECLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(matVM.TotalElecCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(matVM.TotalElecLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(matVM, bid, cw);
        }

        [TestMethod]
        public void AddConnection()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            ChangeWatcher cw = new ChangeWatcher(bid);

            TECControllerType controllerType = new TECControllerType(bid.Catalogs.Manufacturers.RandomObject());
            bid.Catalogs.ControllerTypes.Add(controllerType);
            TECController controller = new TECController(controllerType);
            bid.Controllers.Add(controller);

            TECSystem typical = new TECSystem();
            bid.Systems.Add(typical);

            TECEquipment typEquip = new TECEquipment();
            typical.Equipment.Add(typEquip);

            TECSubScope typSS = new TECSubScope();
            typEquip.SubScope.Add(typSS);

            ObservableCollection<TECElectricalMaterial> connectionTypes = new ObservableCollection<TECElectricalMaterial>();
            connectionTypes.Add(bid.Catalogs.ConnectionTypes.RandomObject());
            TECDevice dev = new TECDevice(connectionTypes, bid.Catalogs.Manufacturers.RandomObject());
            bid.Catalogs.Devices.Add(dev);
            typSS.Devices.Add(dev);

            MaterialSummaryVM matVM = new MaterialSummaryVM(bid, cw);

            //Act
            TECConnection connection = controller.AddSubScope(typSS);
            connection.Length = 50;
            connection.ConduitLength = 50;
            connection.ConduitType = bid.Catalogs.ConduitTypes.RandomObject();

            typical.AddInstance(bid);

            Total totalTEC = calculateTotal(connection, CostType.TEC);
            Total totalElec = calculateTotal(connection, CostType.Electrical);

            Assert.AreEqual(matVM.TotalTECCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalTECLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(matVM.TotalElecCost, totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalElecLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(matVM, bid, cw);
        }
        #endregion

        #region Remove
        [TestMethod]
        public void RemoveTECCost()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            ChangeWatcher cw = new ChangeWatcher(bid);
            TECCost cost = null;
            while (cost == null)
            {
                TECCost randomCost = bid.Catalogs.AssociatedCosts.RandomObject();
                if (randomCost.Type == CostType.TEC)
                {
                    cost = randomCost;
                }
            }
            var system = new TECSystem();
            bid.Systems.Add(system);
            system.AddInstance(bid);
            system.AssociatedCosts.Add(cost);

            MaterialSummaryVM matVM = new MaterialSummaryVM(bid, cw);

            double initialTecCost = matVM.TotalTECCost;
            double initialTecLabor = matVM.TotalTECLabor;

            double initialElecCost = matVM.TotalElecCost;
            double initialElecLabor = matVM.TotalElecLabor;

            Total totalTEC = calculateTotal(cost, CostType.TEC);
            Total totalElec = calculateTotal(cost, CostType.Electrical);

            //Act
            system.AssociatedCosts.Remove(cost);

            //Assert
            Assert.AreEqual(matVM.TotalTECCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalTECLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(matVM.TotalElecCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalElecLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(matVM, bid, cw);
        }

        [TestMethod]
        public void RemoveElectricalCost()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            ChangeWatcher cw = new ChangeWatcher(bid);
            TECCost cost = null;
            while (cost == null)
            {
                TECCost randomCost = bid.Catalogs.AssociatedCosts.RandomObject();
                if (randomCost.Type == CostType.Electrical)
                {
                    cost = randomCost;
                }
            }
            var system = new TECSystem();
            bid.Systems.Add(system);
            system.AddInstance(bid);
            system.AssociatedCosts.Add(cost);

            MaterialSummaryVM matVM = new MaterialSummaryVM(bid, cw);

            double initialTecCost = matVM.TotalTECCost;
            double initialTecLabor = matVM.TotalTECLabor;

            double initialElecCost = matVM.TotalElecCost;
            double initialElecLabor = matVM.TotalElecLabor;

            Total totalTEC = calculateTotal(cost, CostType.TEC);
            Total totalElec = calculateTotal(cost, CostType.Electrical);

            //Act
            system.AssociatedCosts.Remove(cost);

            //Assert
            Assert.AreEqual(matVM.TotalTECCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalTECLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(matVM.TotalElecCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalElecLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(matVM, bid, cw);
        }

        [TestMethod]
        public void RemoveTECMisc()
        {
            //Arrange
            TECBid bid = TestHelper.CreateTestBid();
            ChangeWatcher cw = new ChangeWatcher(bid);
            TECMisc misc = TestHelper.CreateTestMisc(CostType.TEC);
            bid.MiscCosts.Add(misc);

            MaterialSummaryVM matVM = new MaterialSummaryVM(bid, cw);

            double initialTecCost = matVM.TotalTECCost;
            double initialTecLabor = matVM.TotalTECLabor;

            double initialElecCost = matVM.TotalElecCost;
            double initialElecLabor = matVM.TotalElecLabor;

            Total totalTEC = calculateTotal(misc, CostType.TEC);
            Total totalElec = calculateTotal(misc, CostType.Electrical);

            //Act
            bid.MiscCosts.Remove(misc);

            //Assert
            Assert.AreEqual(matVM.TotalTECCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalTECLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(matVM.TotalElecCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalElecLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(matVM, bid, cw);
        }

        [TestMethod]
        public void RemoveElectricalMisc()
        {
            //Arrange
            TECBid bid = TestHelper.CreateTestBid();
            ChangeWatcher cw = new ChangeWatcher(bid);
            TECMisc misc = TestHelper.CreateTestMisc(CostType.Electrical);
            bid.MiscCosts.Add(misc);

            MaterialSummaryVM matVM = new MaterialSummaryVM(bid, cw);

            double initialTecCost = matVM.TotalTECCost;
            double initialTecLabor = matVM.TotalTECLabor;

            double initialElecCost = matVM.TotalElecCost;
            double initialElecLabor = matVM.TotalElecLabor;

            Total totalTEC = calculateTotal(misc, CostType.TEC);
            Total totalElec = calculateTotal(misc, CostType.Electrical);

            //Act
            bid.MiscCosts.Remove(misc);

            //Assert
            Assert.AreEqual(matVM.TotalTECCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalTECLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(matVM.TotalElecCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalElecLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(matVM, bid, cw);
        }

        [TestMethod]
        public void RemovePanel()
        {
            //Arrange
            TECBid bid = TestHelper.CreateTestBid();
            ChangeWatcher cw = new ChangeWatcher(bid);
            TECPanel panel = TestHelper.CreateTestPanel(bid.Catalogs);
            TestHelper.AssignSecondaryProperties(panel, bid.Catalogs);
            bid.Panels.Add(panel);

            MaterialSummaryVM matVM = new MaterialSummaryVM(bid, cw);

            double initialTecCost = matVM.TotalTECCost;
            double initialTecLabor = matVM.TotalTECLabor;

            double initialElecCost = matVM.TotalElecCost;
            double initialElecLabor = matVM.TotalElecLabor;

            Total totalTEC = calculateTotal(panel, CostType.TEC);
            Total totalElec = calculateTotal(panel, CostType.Electrical);

            //Act
            bid.Panels.Remove(panel);

            //Assert
            Assert.AreEqual(matVM.TotalTECCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalTECLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(matVM.TotalElecCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalElecLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(matVM, bid, cw);
        }

        [TestMethod]
        public void RemoveController()
        {
            //Arrange
            TECBid bid = TestHelper.CreateTestBid();
            ChangeWatcher cw = new ChangeWatcher(bid);
            TECController controller = TestHelper.CreateTestController(bid.Catalogs);
            TestHelper.AssignSecondaryProperties(controller, bid.Catalogs);
            bid.Controllers.Add(controller);

            MaterialSummaryVM matVM = new MaterialSummaryVM(bid, cw);

            double initialTecCost = matVM.TotalTECCost;
            double initialTecLabor = matVM.TotalTECLabor;

            double initialElecCost = matVM.TotalElecCost;
            double initialElecLabor = matVM.TotalElecLabor;

            Total totalTEC = calculateTotal(controller, CostType.TEC);
            Total totalElec = calculateTotal(controller, CostType.Electrical);

            //Act
            bid.Controllers.Remove(controller);

            //Assert
            Assert.AreEqual(matVM.TotalTECCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalTECLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(matVM.TotalElecCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalElecLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(matVM, bid, cw);
        }

        [TestMethod]
        public void RemoveDevice()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            ChangeWatcher cw = new ChangeWatcher(bid);

            TECSystem typical = new TECSystem();
            bid.Systems.Add(typical);

            TECEquipment typEquip = new TECEquipment();
            typical.Equipment.Add(typEquip);

            TECSubScope typSS = new TECSubScope();
            typEquip.SubScope.Add(typSS);

            TECDevice device = bid.Catalogs.Devices.RandomObject();
            TestHelper.AssignSecondaryProperties(device, bid.Catalogs);
            typSS.Devices.Add(device);

            typical.AddInstance(bid);

            MaterialSummaryVM matVM = new MaterialSummaryVM(bid, cw);

            double initialTecCost = matVM.TotalTECCost;
            double initialTecLabor = matVM.TotalTECLabor;

            double initialElecCost = matVM.TotalElecCost;
            double initialElecLabor = matVM.TotalElecLabor;

            Total totalTEC = calculateTotal(device, CostType.TEC);
            Total totalElec = calculateTotal(device, CostType.Electrical);

            //Act
            typSS.Devices.Remove(device);

            //Assert
            Assert.AreEqual(matVM.TotalTECCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalTECLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(matVM.TotalElecCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalElecLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(matVM, bid, cw);
        }

        [TestMethod]
        public void RemoveSubScope()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            ChangeWatcher cw = new ChangeWatcher(bid);

            TECSystem typical = new TECSystem();
            bid.Systems.Add(typical);

            TECEquipment typEquip = new TECEquipment();
            typical.Equipment.Add(typEquip);

            TECSubScope subScope = TestHelper.CreateTestSubScope(bid.Catalogs);
            TestHelper.AssignSecondaryProperties(subScope, bid.Catalogs);
            typEquip.SubScope.Add(subScope);

            typical.AddInstance(bid);

            MaterialSummaryVM matVM = new MaterialSummaryVM(bid, cw);

            double initialTecCost = matVM.TotalTECCost;
            double initialTecLabor = matVM.TotalTECLabor;

            double initialElecCost = matVM.TotalElecCost;
            double initialElecLabor = matVM.TotalElecLabor;

            Total totalTEC = calculateTotal(subScope, CostType.TEC);
            Total totalElec = calculateTotal(subScope, CostType.Electrical);

            //Act
            typEquip.SubScope.Remove(subScope);

            //Assert
            Assert.AreEqual(matVM.TotalTECCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalTECLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(matVM.TotalElecCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalElecLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(matVM, bid, cw);
        }

        [TestMethod]
        public void RemoveEquipment()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            ChangeWatcher cw = new ChangeWatcher(bid);

            TECSystem typical = new TECSystem();
            bid.Systems.Add(typical);

            TECEquipment equip = TestHelper.CreateTestEquipment(bid.Catalogs);
            TestHelper.AssignSecondaryProperties(equip, bid.Catalogs);
            typical.Equipment.Add(equip);

            typical.AddInstance(bid);

            MaterialSummaryVM matVM = new MaterialSummaryVM(bid, cw);

            double initialTecCost = matVM.TotalTECCost;
            double initialTecLabor = matVM.TotalTECLabor;

            double initialElecCost = matVM.TotalElecCost;
            double initialElecLabor = matVM.TotalElecLabor;

            Total totalTEC = calculateTotal(equip, CostType.TEC);
            Total totalElec = calculateTotal(equip, CostType.Electrical);

            //Act
            typical.Equipment.Remove(equip);

            //Assert
            Assert.AreEqual(matVM.TotalTECCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalTECLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(matVM.TotalElecCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalElecLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(matVM, bid, cw);
        }

        [TestMethod]
        public void RemoveInstanceSystem()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            ChangeWatcher cw = new ChangeWatcher(bid);

            TECSystem typical = TestHelper.CreateTestSystem(bid.Catalogs);
            bid.Systems.Add(typical);

            TECSystem instance = typical.AddInstance(bid);

            MaterialSummaryVM matVM = new MaterialSummaryVM(bid, cw);

            double initialTecCost = matVM.TotalTECCost;
            double initialTecLabor = matVM.TotalTECLabor;

            double initialElecCost = matVM.TotalElecCost;
            double initialElecLabor = matVM.TotalElecLabor;

            Total totalTEC = calculateTotalInstanceSystem(instance, typical, CostType.TEC);
            Total totalElec = calculateTotalInstanceSystem(instance, typical, CostType.Electrical);

            //Act
            typical.Instances.Remove(instance);

            //Assert
            Assert.AreEqual(matVM.TotalTECCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalTECLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(matVM.TotalElecCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalElecLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(matVM, bid, cw);
        }

        [TestMethod]
        public void RemoveConnection()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            ChangeWatcher cw = new ChangeWatcher(bid);

            TECController controller = new TECController(bid.Catalogs.ControllerTypes.RandomObject());
            bid.Controllers.Add(controller);

            TECSystem typical = new TECSystem();
            bid.Systems.Add(typical);

            TECEquipment typEquip = new TECEquipment();
            typical.Equipment.Add(typEquip);

            TECSubScope ss = new TECSubScope();
            typEquip.SubScope.Add(ss);

            TECConnection connection = controller.AddSubScope(ss);
            connection.Length = 50;
            connection.ConduitLength = 50;
            connection.ConduitType = bid.Catalogs.ConduitTypes.RandomObject();

            typical.AddInstance(bid);

            MaterialSummaryVM matVM = new MaterialSummaryVM(bid, cw);

            double initialTecCost = matVM.TotalTECCost;
            double initialTecLabor = matVM.TotalTECLabor;

            double initialElecCost = matVM.TotalElecCost;
            double initialElecLabor = matVM.TotalElecLabor;

            Total totalTEC = calculateTotal(connection, CostType.TEC);
            Total totalElec = calculateTotal(connection, CostType.Electrical);

            //Act
            controller.RemoveSubScope(ss);

            //Assert
            Assert.AreEqual(matVM.TotalTECCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalTECLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(matVM.TotalElecCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(matVM.TotalElecLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(matVM, bid, cw);
        }
        #endregion

        #region Special Tests
        [TestMethod]
        public void AddTypicalSubScopeConnectionToController()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            ChangeWatcher cw = new ChangeWatcher(bid);

            TECController controller = new TECController(bid.Catalogs.ControllerTypes.RandomObject());
            bid.Controllers.Add(controller);

            TECSystem typical = new TECSystem();

            TECEquipment equip = new TECEquipment();
            typical.Equipment.Add(equip);

            TECSubScope ss = new TECSubScope();
            equip.SubScope.Add(ss);

            TECDevice dev = bid.Catalogs.Devices.RandomObject();
            ss.Devices.Add(dev);

            bid.Systems.Add(typical);

            MaterialSummaryVM matVM = new MaterialSummaryVM(bid, cw);

            //Act
            TECConnection connection = controller.AddSubScope(ss);
            connection.Length = 100;
            connection.ConduitLength = 100;
            connection.ConduitType = bid.Catalogs.ConduitTypes.RandomObject();

            Assert.AreEqual(0, matVM.TotalTECCost, "Typical connection added to tec cost.");
            Assert.AreEqual(0, matVM.TotalTECLabor, "Typical connection added to tec labor.");
            Assert.AreEqual(0, matVM.TotalElecCost, "Typical connection added to elec cost.");
            Assert.AreEqual(0, matVM.TotalElecLabor, "Typical connection added to elec labor.");

            checkRefresh(matVM, bid, cw);
        }
        #endregion

        #endregion
        
        #region Hardware Summary VM
        [TestMethod]
        public void AddControllerTypeToHardwareVM()
        {
            //Arrange
            HardwareSummaryVM hardwareVM = new HardwareSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECControllerType controllerType = catalogs.ControllerTypes.RandomObject();

            //Act
            List<CostObject> deltas = hardwareVM.AddHardware(controllerType);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            Total assocTECTotal = new Total();
            Total assocElecTotal = new Total();
            foreach(TECCost cost in controllerType.AssociatedCosts)
            {
                assocTECTotal += calculateTotal(cost, CostType.TEC);
                assocElecTotal += calculateTotal(cost, CostType.Electrical);
            }

            Total controllerTypeTotalTEC = calculateTotal(controllerType, CostType.TEC);
            Total controllerTypeTotalElec = calculateTotal(controllerType, CostType.Electrical);

            //Assert
            //Check hardware properties
            Assert.AreEqual(controllerType.ExtendedCost, hardwareVM.HardwareCost, delta, "HardwareCost property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(controllerType.Labor, hardwareVM.HardwareLabor, delta, "HardwareLabor property in HardwareSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(assocTECTotal.cost, hardwareVM.AssocTECCostTotal, delta, "AssocTECCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(assocTECTotal.labor, hardwareVM.AssocTECLaborTotal, delta, "AssocTECLaborTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(assocElecTotal.cost, hardwareVM.AssocElecCostTotal, delta, "AssocElecCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(assocElecTotal.labor, hardwareVM.AssocElecLaborTotal, delta, "AssocElecLaborTotal property in HardwareSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(controllerTypeTotalTEC.cost, tecDelta.Cost, delta, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(controllerTypeTotalTEC.labor, tecDelta.Labor, delta, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(controllerTypeTotalElec.cost, elecDelta.Cost, delta, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(controllerTypeTotalElec.labor, elecDelta.Labor, delta, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void RemoveControllerTypeFromHardwareVM()
        {
            //Arrange
            HardwareSummaryVM hardwareVM = new HardwareSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECControllerType controllerType = catalogs.ControllerTypes.RandomObject();

            //Act
            hardwareVM.AddHardware(controllerType);
            List<CostObject> deltas = hardwareVM.RemoveHardware(controllerType);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            Total controllerTypeTotalTEC = calculateTotal(controllerType, CostType.TEC);
            Total controllerTypeTotalElec = calculateTotal(controllerType, CostType.Electrical);

            //Assert
            //Check hardware properties
            Assert.AreEqual(0, hardwareVM.HardwareCost, delta, "HardwareCost property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.HardwareLabor, delta, "HardwareLabor property in HardwareSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(0, hardwareVM.AssocTECCostTotal, delta, "AssocTECCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocTECLaborTotal, delta, "AssocTECLaborTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecCostTotal, delta, "AssocElecCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecLaborTotal, delta, "AssocElecLaborTotal property in HardwareSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(-controllerTypeTotalTEC.cost, tecDelta.Cost, delta, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(-controllerTypeTotalTEC.labor, tecDelta.Labor, delta, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(-controllerTypeTotalElec.cost, elecDelta.Cost, delta, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(-controllerTypeTotalElec.labor, elecDelta.Labor, delta, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void AddPanelTypeToHardwareVM()
        {
            //Arrange
            HardwareSummaryVM hardwareVM = new HardwareSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECPanelType panelType = catalogs.PanelTypes.RandomObject();

            //Act
            List<CostObject> deltas = hardwareVM.AddHardware(panelType);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            Total assocTECTotal = new Total();
            Total assocElecTotal = new Total();
            foreach (TECCost cost in panelType.AssociatedCosts)
            {
                assocTECTotal += calculateTotal(cost, CostType.TEC);
                assocElecTotal += calculateTotal(cost, CostType.Electrical);
            }

            Total panelTypeTotalTEC = calculateTotal(panelType, CostType.TEC);
            Total panelTypeTotalElec = calculateTotal(panelType, CostType.Electrical);

            //Assert
            //Check hardware properties
            Assert.AreEqual(panelType.ExtendedCost, hardwareVM.HardwareCost, delta, "HardwareCost property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(panelType.Labor, hardwareVM.HardwareLabor, delta, "HardwareLabor property in HardwareSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(assocTECTotal.cost, hardwareVM.AssocTECCostTotal, delta, "AssocTECCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(assocTECTotal.labor, hardwareVM.AssocTECLaborTotal, delta, "AssocTECLaborTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(assocElecTotal.cost, hardwareVM.AssocElecCostTotal, delta, "AssocElecCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(assocElecTotal.labor, hardwareVM.AssocElecLaborTotal, delta, "AssocElecLaborTotal property in HardwareSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(panelTypeTotalTEC.cost, tecDelta.Cost, delta, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(panelTypeTotalTEC.labor, tecDelta.Labor, delta, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(panelTypeTotalElec.cost, elecDelta.Cost, delta, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(panelTypeTotalElec.labor, elecDelta.Labor, delta, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void RemovePanelTypeFromHardwareVM()
        {
            //Arrange
            HardwareSummaryVM hardwareVM = new HardwareSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECPanelType panelType = catalogs.PanelTypes.RandomObject();

            //Act
            hardwareVM.AddHardware(panelType);
            List<CostObject> deltas = hardwareVM.RemoveHardware(panelType);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            Total panelTypeTotalTEC = calculateTotal(panelType, CostType.TEC);
            Total panelTypeTotalElec = calculateTotal(panelType, CostType.Electrical);

            //Assert
            //Check hardware properties
            Assert.AreEqual(0, hardwareVM.HardwareCost, delta, "HardwareCost property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.HardwareLabor, delta, "HardwareLabor property in HardwareSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(0, hardwareVM.AssocTECCostTotal, delta, "AssocTECCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocTECLaborTotal, delta, "AssocTECLaborTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecCostTotal, delta, "AssocElecCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecLaborTotal, delta, "AssocElecLaborTotal property in HardwareSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(-panelTypeTotalTEC.cost, tecDelta.Cost, delta, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(-panelTypeTotalTEC.labor, tecDelta.Labor, delta, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(-panelTypeTotalElec.cost, elecDelta.Cost, delta, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(-panelTypeTotalElec.labor, elecDelta.Labor, delta, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void AddDeviceToHardwareVM()
        {
            //Arrange
            HardwareSummaryVM hardwareVM = new HardwareSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECDevice device = catalogs.Devices.RandomObject();

            //Act
            List<CostObject> deltas = hardwareVM.AddHardware(device);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            Total assocTECTotal = new Total();
            Total assocElecTotal = new Total();
            foreach (TECCost cost in device.AssociatedCosts)
            {
                assocTECTotal += calculateTotal(cost, CostType.TEC);
                assocElecTotal += calculateTotal(cost, CostType.Electrical);
            }

            Total deviceTotalTEC = calculateTotal(device, CostType.TEC);
            Total deviceTotalElec = calculateTotal(device, CostType.Electrical);

            //Assert
            //Check hardware properties
            Assert.AreEqual(device.ExtendedCost, hardwareVM.HardwareCost, delta, "HardwareCost property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(device.Labor, hardwareVM.HardwareLabor, delta, "HardwareLabor property in HardwareSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(assocTECTotal.cost, hardwareVM.AssocTECCostTotal, delta, "AssocTECCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(assocTECTotal.labor, hardwareVM.AssocTECLaborTotal, delta, "AssocTECLaborTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(assocElecTotal.cost, hardwareVM.AssocElecCostTotal, delta, "AssocElecCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(assocElecTotal.labor, hardwareVM.AssocElecLaborTotal, delta, "AssocElecLaborTotal property in HardwareSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(deviceTotalTEC.cost, tecDelta.Cost, delta, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(deviceTotalTEC.labor, tecDelta.Labor, delta, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(deviceTotalElec.cost, elecDelta.Cost, delta, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(deviceTotalElec.labor, elecDelta.Labor, delta, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void RemoveDeviceFromHardwareVM()
        {
            //Arrange
            HardwareSummaryVM hardwareVM = new HardwareSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECDevice device = catalogs.Devices.RandomObject();

            //Act
            hardwareVM.AddHardware(device);
            List<CostObject> deltas = hardwareVM.RemoveHardware(device);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            Total deviceTotalTEC = calculateTotal(device, CostType.TEC);
            Total deviceTotalElec = calculateTotal(device, CostType.Electrical);

            //Assert
            //Check hardware properties
            Assert.AreEqual(0, hardwareVM.HardwareCost, delta, "HardwareCost property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.HardwareLabor, delta, "HardwareLabor property in HardwareSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(0, hardwareVM.AssocTECCostTotal, delta, "AssocTECCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocTECLaborTotal, delta, "AssocTECLaborTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecCostTotal, delta, "AssocElecCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecLaborTotal, delta, "AssocElecLaborTotal property in HardwareSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(-deviceTotalTEC.cost, tecDelta.Cost, delta, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(-deviceTotalTEC.labor, tecDelta.Labor, delta, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(-deviceTotalElec.cost, elecDelta.Cost, delta, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(-deviceTotalElec.labor, elecDelta.Labor, delta, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void AddTECCostToHardwareVM()
        {
            //Arrange
            HardwareSummaryVM hardwareVM = new HardwareSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECCost tecCost = null;
            foreach(TECCost cost in catalogs.AssociatedCosts)
            {
                if (cost.Type == CostType.TEC)
                {
                    tecCost = cost;
                    break;
                }
            }

            //Act
            List<CostObject> deltas = hardwareVM.AddCost(tecCost);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            //Assert
            //Check hardware properties
            Assert.AreEqual(0, hardwareVM.HardwareCost, delta, "HardwareCost property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.HardwareLabor, delta, "HardwareLabor property in HardwareSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(tecCost.Cost, hardwareVM.AssocTECCostTotal, delta, "AssocTECCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(tecCost.Labor, hardwareVM.AssocTECLaborTotal, delta, "AssocTECLaborTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecCostTotal, delta, "AssocElecCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecLaborTotal, delta, "AssocElecLaborTotal property in HardwareSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(tecCost.Cost, tecDelta.Cost, delta, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(tecCost.Labor, tecDelta.Labor, delta, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(0, elecDelta.Cost, delta, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(0, elecDelta.Labor, delta, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void RemoveTECCostFromHardwareVM()
        {
            //Arrange
            HardwareSummaryVM hardwareVM = new HardwareSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECCost tecCost = null;
            foreach (TECCost cost in catalogs.AssociatedCosts)
            {
                if (cost.Type == CostType.TEC)
                {
                    tecCost = cost;
                    break;
                }
            }

            //Act
            hardwareVM.AddCost(tecCost);
            List<CostObject> deltas = hardwareVM.RemoveCost(tecCost);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            //Assert
            //Check hardware properties
            Assert.AreEqual(0, hardwareVM.HardwareCost, delta, "HardwareCost property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.HardwareLabor, delta, "HardwareLabor property in HardwareSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(0, hardwareVM.AssocTECCostTotal, delta, "AssocTECCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocTECLaborTotal, delta, "AssocTECLaborTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecCostTotal, delta, "AssocElecCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecLaborTotal, delta, "AssocElecLaborTotal property in HardwareSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(-tecCost.Cost, tecDelta.Cost, delta, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(-tecCost.Labor, tecDelta.Labor, delta, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(0, elecDelta.Cost, delta, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(0, elecDelta.Labor, delta, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void AddElecCostToHardwareVM()
        {
            //Arrange
            HardwareSummaryVM hardwareVM = new HardwareSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECCost elecCost = null;
            foreach (TECCost cost in catalogs.AssociatedCosts)
            {
                if (cost.Type == CostType.Electrical)
                {
                    elecCost = cost;
                    break;
                }
            }

            //Act
            List<CostObject> deltas = hardwareVM.AddCost(elecCost);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            //Assert
            //Check hardware properties
            Assert.AreEqual(0, hardwareVM.HardwareCost, delta, "HardwareCost property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.HardwareLabor, delta, "HardwareLabor property in HardwareSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(0, hardwareVM.AssocTECCostTotal, delta, "AssocTECCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocTECLaborTotal, delta, "AssocTECLaborTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(elecCost.Cost, hardwareVM.AssocElecCostTotal, delta, "AssocElecCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(elecCost.Labor, hardwareVM.AssocElecLaborTotal, delta, "AssocElecLaborTotal property in HardwareSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(0, tecDelta.Cost, delta, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(0, tecDelta.Labor, delta, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(elecCost.Cost, elecDelta.Cost, delta, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(elecCost.Labor, elecDelta.Labor, delta, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void RemoveElecCostFromHardwareVM()
        {
            //Arrange
            HardwareSummaryVM hardwareVM = new HardwareSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECCost elecCost = null;
            foreach (TECCost cost in catalogs.AssociatedCosts)
            {
                if (cost.Type == CostType.Electrical)
                {
                    elecCost = cost;
                    break;
                }
            }

            //Act
            hardwareVM.AddCost(elecCost);
            List<CostObject> deltas = hardwareVM.RemoveCost(elecCost);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            //Assert
            //Check hardware properties
            Assert.AreEqual(0, hardwareVM.HardwareCost, delta, "HardwareCost property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.HardwareLabor, delta, "HardwareLabor property in HardwareSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(0, hardwareVM.AssocTECCostTotal, delta, "AssocTECCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocTECLaborTotal, delta, "AssocTECLaborTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecCostTotal, delta, "AssocElecCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecLaborTotal, delta, "AssocElecLaborTotal property in HardwareSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(0, tecDelta.Cost, delta, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(0, tecDelta.Labor, delta, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(-elecCost.Cost, elecDelta.Cost, delta, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(-elecCost.Labor, elecDelta.Labor, delta, "Returned Elec labor delta is wrong.");
        }
        #endregion

        #region Length Summary VM

        #endregion

        #region Misc Costs Summary VM

        #endregion

        #region Calculation Methods

        private Total calculateTotal(TECHardware hardware, CostType type)
        {
            Total total = new Total();
            if (type == hardware.Type)
            {
                total.cost = hardware.ExtendedCost;
                total.labor = hardware.Labor;
            }
            total += calculateTotal(hardware as TECScope, type);
            return total;
        }

        private Total calculateTotal(TECCost cost, CostType type)
        {
            int qty = 1;
            if(cost is TECMisc)
            {
                qty = (cost as TECMisc).Quantity;
            }
            if (cost.Type == type)
            {
                Total total = new Total();
                total.cost = cost.Cost * qty;
                total.labor = cost.Labor * qty;
                return total;
            }
            else
            {
                return new Total();
            }
        }

        private Total calculateTotal(TECScope scope, CostType type)
        {
            Total total = new Total();
            foreach(TECCost cost in scope.AssociatedCosts)
            {
                total += calculateTotal(cost, type);
            }
            return total;
        }

        private Total calculateTotal(TECSubScope subScope, CostType type)
        {
            Total total = new Total();
            foreach(TECDevice device in subScope.Devices)
            {
                total += calculateTotal(device, type);
            }
            //foreach(TECPoint point in subScope.Points)
            //{
            //    total += calculateTotal(point, type);
            //}
            total += calculateTotal(subScope as TECScope, type);
            return total;
        }

        private Total calculateTotal(TECEquipment equipment, CostType type)
        {
            Total total = new Total();
            foreach (TECSubScope subScope in equipment.SubScope)
            {
                total += calculateTotal(subScope, type);
            }
            total += calculateTotal(equipment as TECScope, type);
            return total;
        }

        private Total calculateTotal(TECController controller, CostType type)
        {
            Total total = new Total();
            total += calculateTotal(controller as TECScope, type);
            total += calculateTotal(controller.Type as TECCost, type);
            foreach(TECConnection connection in controller.ChildrenConnections)
            {
                total += calculateTotal(connection, type);
            }
            return total;
        }

        private Total calculateTotal(TECPanel panel, CostType type)
        {
            Total total = new Total();
            total += calculateTotal(panel as TECScope, type);
            total += calculateTotal(panel.Type as TECCost, type);
            return total;
        }

        private Total calculateTotalInstanceSystem(TECSystem instance, TECSystem typical, CostType type)
        {
            Total total = new Total();
            foreach (TECEquipment equipment in instance.Equipment)
            {
                Total equipSubTotal = calculateTotal(equipment, type);
                total += equipSubTotal;
            }
            foreach(TECMisc misc in typical.MiscCosts)
            {
                Total miscSubTotal = calculateTotal(misc, type);
                total += miscSubTotal;
            }
            foreach(TECController controller in instance.Controllers)
            {
                Total controllerSubTotal = calculateTotal(controller, type);
                total += controllerSubTotal;
            }
            foreach(TECPanel panel in instance.Panels)
            {
                Total panelSubTotal = calculateTotal(panel, type);
                total += panelSubTotal;
            }
            Total systemScopeSubTotal = calculateTotal(instance as TECScope, type);
            total += systemScopeSubTotal;
            return total;
        }

        private Total calculateTotal(TECConnection connection, CostType type)
        {
            Total total = new Total();
            if(connection is TECSubScopeConnection)
            {
                foreach(TECElectricalMaterial conType in (connection as TECSubScopeConnection).ConnectionTypes)
                {
                    total += calculateTotal(conType, type) * connection.Length;
                    total += calculateTotal(conType as TECScope, type);
                    foreach(TECCost cost in conType.RatedCosts)
                    {
                        total += calculateTotal(cost, type) * connection.Length;
                    }
                }
            } else if(connection is TECNetworkConnection)
            {
                total += calculateTotal((connection as TECNetworkConnection).ConnectionType, type) * connection.Length;
                total += calculateTotal((connection as TECNetworkConnection).ConnectionType as TECScope, type);
                foreach (TECCost cost in (connection as TECNetworkConnection).ConnectionType.RatedCosts)
                {
                    total += calculateTotal(cost, type) * connection.Length;
                }
            }
            if(connection.ConduitType != null)
            {
                total += calculateTotal(connection.ConduitType, type) * connection.ConduitLength;
                total += calculateTotal(connection.ConduitType as TECScope, type);
                foreach (TECCost cost in connection.ConduitType.RatedCosts)
                {
                    total += calculateTotal(cost, type) * connection.ConduitLength;
                }
            }
            return total;
        }

        #endregion

        private void checkRefresh(MaterialSummaryVM matVM, TECBid bid, ChangeWatcher cw)
        {
            double tecCost = matVM.TotalTECCost;
            double tecLabor = matVM.TotalTECLabor;
            double elecCost = matVM.TotalElecCost;
            double elecLabor = matVM.TotalElecLabor;

            matVM.Refresh(bid, cw);

            Assert.AreEqual(tecCost, matVM.TotalTECCost, delta, "Total tec cost didn't refresh properly.");
            Assert.AreEqual(tecLabor, matVM.TotalTECLabor, delta, "Total tec labor didn't refresh properly.");
            Assert.AreEqual(elecCost, matVM.TotalElecCost, delta, "Total elec cost didn't refresh properly.");
            Assert.AreEqual(elecLabor, matVM.TotalElecLabor, delta, "Total elec labor didn't refresh properly.");
        }

        private class Total
        {
            public double cost;
            public double labor;

            public Total()
            {
                cost = 0;
                labor = 0;
            }

            public static Total operator +(Total left, Total right)
            {
                Total total = new Total();
                total.cost = left.cost + right.cost;
                total.labor = left.labor + right.labor;
                return total;
            }

            public static Total operator -(Total left, Total right)
            {
                Total total = new Total();
                total.cost = left.cost - right.cost;
                total.labor = left.labor - right.labor;
                return total;
            }

            public static Total operator *(Total left, double right)
            {
                Total total = new Total();
                total.cost = left.cost * right;
                total.labor = left.labor * right;
                return total;
            }
        }
    }
}
