using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EstimatingLibrary;
using TECUserControlLibrary.ViewModels;

namespace Tests
{
    /// <summary>
    /// Summary description for SummaryVMTests
    /// </summary>
    [TestClass]
    public class SummaryVMTests
    {
        private static double delta = 1.0 / 1000.0;
        
        #region Add
        [TestMethod]
        public void AddTECCost()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
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

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            //Act
            typical.AssociatedCosts.Add(cost);

            //Assert
            Assert.AreEqual(tecVM.TotalCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(elecVM.TotalLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(tecVM, elecVM, bid);
        }

        [TestMethod]
        public void AddElectricalCost()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid(); ;
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

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            //Act
            typical.AssociatedCosts.Add(cost);

            //Assert
            Assert.AreEqual(tecVM.TotalCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(elecVM.TotalLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(tecVM, elecVM, bid);
        }

        [TestMethod]
        public void AddTECMiscToBid()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            TECMisc misc = TestHelper.CreateTestMisc(CostType.TEC);

            Total totalTEC = calculateTotal(misc, CostType.TEC);
            Total totalElec = calculateTotal(misc, CostType.Electrical);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            //Act
            bid.MiscCosts.Add(misc);

            //Assert
            Assert.AreEqual(tecVM.TotalCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(elecVM.TotalLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(tecVM, elecVM, bid);
        }

        [TestMethod]
        public void AddElectricalMiscToBid()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            TECMisc misc = TestHelper.CreateTestMisc(CostType.Electrical);

            Total totalTEC = calculateTotal(misc, CostType.TEC);
            Total totalElec = calculateTotal(misc, CostType.Electrical);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            //Act
            bid.MiscCosts.Add(misc);

            //Assert
            Assert.AreEqual(tecVM.TotalCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(elecVM.TotalLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(tecVM, elecVM, bid);
        }

        [TestMethod]
        public void AddTECMiscToSystem()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            TECMisc misc = TestHelper.CreateTestMisc(CostType.TEC);

            Total totalTEC = calculateTotal(misc, CostType.TEC);
            Total totalElec = calculateTotal(misc, CostType.Electrical);

            TECSystem typical = new TECSystem();
            bid.Systems.Add(typical);
            typical.AddInstance(bid);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);
            
            //Act
            typical.MiscCosts.Add(misc);

            //Arrange
            Assert.AreEqual(tecVM.TotalCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(elecVM.TotalLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(tecVM, elecVM, bid);
        }

        [TestMethod]
        public void AddElectricalMiscToSystem()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            TECMisc misc = TestHelper.CreateTestMisc(CostType.Electrical);

            Total totalTEC = calculateTotal(misc, CostType.TEC);
            Total totalElec = calculateTotal(misc, CostType.Electrical);

            TECSystem typical = new TECSystem();
            bid.Systems.Add(typical);
            typical.AddInstance(bid);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            //Act
            typical.MiscCosts.Add(misc);
            
            //Assert
            Assert.AreEqual(tecVM.TotalCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(elecVM.TotalLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(tecVM, elecVM, bid);
        }

        [TestMethod]
        public void AddPanel()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            TECPanel panel = TestHelper.CreateTestPanel(bid.Catalogs);
            TestHelper.AssignSecondaryProperties(panel, bid.Catalogs);

            Total totalTEC = calculateTotal(panel, CostType.TEC);
            Total totalElec = calculateTotal(panel, CostType.Electrical);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            //Act
            bid.Panels.Add(panel);

            //Assert
            Assert.AreEqual(tecVM.TotalCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(elecVM.TotalLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(tecVM, elecVM, bid);
        }

        [TestMethod]
        public void AddController()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            TECController controller = TestHelper.CreateTestController(bid.Catalogs);
            TestHelper.AssignSecondaryProperties(controller, bid.Catalogs);

            Total totalTEC = calculateTotal(controller, CostType.TEC);
            Total totalElec = calculateTotal(controller, CostType.Electrical);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            //Act
            bid.Controllers.Add(controller);
            
            //Assert
            Assert.AreEqual(tecVM.TotalCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(elecVM.TotalLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(tecVM, elecVM, bid);
        }

        [TestMethod]
        public void AddPoint()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            TECPoint point = TestHelper.CreateTestPoint(bid.Catalogs);
            TestHelper.AssignSecondaryProperties(point, bid.Catalogs);

            Total totalTEC = calculateTotal(point, CostType.TEC);
            Total totalElec = calculateTotal(point, CostType.Electrical);

            TECSystem typical = new TECSystem();
            bid.Systems.Add(typical);

            TECEquipment typEquip = new TECEquipment();
            typical.Equipment.Add(typEquip);

            TECSubScope typSS = new TECSubScope();
            typEquip.SubScope.Add(typSS);

            typical.AddInstance(bid);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            //Act
            typSS.Points.Add(point);

            //Assert
            Assert.AreEqual(tecVM.TotalCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(elecVM.TotalLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(tecVM, elecVM, bid);
        }

        [TestMethod]
        public void AddDevice()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
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

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            //Act
            typSS.Devices.Add(device);

            //Assert
            Assert.AreEqual(tecVM.TotalCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(elecVM.TotalLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(tecVM, elecVM, bid);
        }

        [TestMethod]
        public void AddSubScope()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            TECSubScope subscope = TestHelper.CreateTestSubScope(bid.Catalogs);
            TestHelper.AssignSecondaryProperties(subscope, bid.Catalogs);

            Total totalTEC = calculateTotal(subscope, CostType.TEC);
            Total totalElec = calculateTotal(subscope, CostType.Electrical);

            TECSystem typical = new TECSystem();
            bid.Systems.Add(typical);

            TECEquipment typEquip = new TECEquipment();
            typical.Equipment.Add(typEquip);

            typical.AddInstance(bid);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            //Act
            typEquip.SubScope.Add(subscope);
            
            //Assert
            Assert.AreEqual(tecVM.TotalCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(elecVM.TotalLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(tecVM, elecVM, bid);
        }

        [TestMethod]
        public void AddEquipment()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            TECEquipment equipment = TestHelper.CreateTestEquipment(bid.Catalogs);
            TestHelper.AssignSecondaryProperties(equipment, bid.Catalogs);

            Total totalTEC = calculateTotal(equipment, CostType.TEC);
            Total totalElec = calculateTotal(equipment, CostType.Electrical);

            TECSystem typical = new TECSystem();
            bid.Systems.Add(typical);

            typical.AddInstance(bid);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            //Act
            typical.Equipment.Add(equipment);

            //Assert
            Assert.AreEqual(tecVM.TotalCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(elecVM.TotalLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(tecVM, elecVM, bid);
        }

        [TestMethod]
        public void AddInstanceSystem()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            TECSystem typical = TestHelper.CreateTestSystem(bid.Catalogs);
            TestHelper.AssignSecondaryProperties(typical, bid.Catalogs);
            bid.Systems.Add(typical);
            
            Total totalTEC = calculateTotal(typical, CostType.TEC);
            Total totalElec = calculateTotal(typical, CostType.Electrical);
            
            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            //Act
            typical.AddInstance(bid);

            //Assert
            Assert.AreEqual(tecVM.TotalCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(elecVM.TotalLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(tecVM, elecVM, bid);
        }

        [TestMethod]
        public void AddConnection()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();

            TECController controller = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            bid.Controllers.Add(controller);

            TECSystem typical = new TECSystem();
            bid.Systems.Add(typical);

            TECEquipment typEquip = new TECEquipment();
            typical.Equipment.Add(typEquip);

            TECSubScope ss = new TECSubScope();
            typEquip.SubScope.Add(ss);

            typical.AddInstance(bid);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            //Act
            TECConnection connection = controller.AddSubScope(ss);
            connection.Length = 50;
            connection.ConduitLength = 50;
            connection.ConduitType = bid.Catalogs.ConduitTypes.RandomObject();

            Total totalTEC = calculateTotal(connection, CostType.TEC);
            Total totalElec = calculateTotal(connection, CostType.Electrical);

            Assert.AreEqual(tecVM.TotalCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(elecVM.TotalLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(tecVM, elecVM, bid);
        }
        #endregion

        #region Remove
        [TestMethod]
        public void RemoveTECCost()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();
            TECCost cost = null;
            while (cost == null)
            {
                TECCost randomCost = bid.Catalogs.AssociatedCosts.RandomObject();
                if (randomCost.Type == CostType.TEC)
                {
                    cost = randomCost;
                }
            }
            var system = bid.Systems.RandomObject();
            system.AddInstance(bid);
            system.AssociatedCosts.Add(cost);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            double initialTecCost = tecVM.TotalCost;
            double initialTecLabor = tecVM.TotalLabor;

            double initialElecCost = elecVM.TotalCost;
            double initialElecLabor = elecVM.TotalLabor;

            Total totalTEC = calculateTotal(cost, CostType.TEC);
            Total totalElec = calculateTotal(cost, CostType.Electrical);

            //Act
            system.AssociatedCosts.Remove(cost);

            //Assert
            Assert.AreEqual(tecVM.TotalCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(elecVM.TotalLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(tecVM, elecVM, bid);
        }

        [TestMethod]
        public void RemoveElectricalCost()
        {
            //Arrange
            TECBid bid = TestHelper.CreateTestBid();
            TECCost cost = null;
            while (cost == null)
            {
                TECCost randomCost = bid.Catalogs.AssociatedCosts.RandomObject();
                if (randomCost.Type == CostType.Electrical)
                {
                    cost = randomCost;
                }
            }
            var system = bid.Systems.RandomObject();
            system.AddInstance(bid);
            system.AssociatedCosts.Add(cost);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            double initialTecCost = tecVM.TotalCost;
            double initialTecLabor = tecVM.TotalLabor;

            double initialElecCost = elecVM.TotalCost;
            double initialElecLabor = elecVM.TotalLabor;

            Total totalTEC = calculateTotal(cost, CostType.TEC);
            Total totalElec = calculateTotal(cost, CostType.Electrical);

            //Act
            system.AssociatedCosts.Remove(cost);

            //Assert
            Assert.AreEqual(tecVM.TotalCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(elecVM.TotalLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(tecVM, elecVM, bid);
        }

        [TestMethod]
        public void RemoveTECMisc()
        {
            //Arrange
            TECBid bid = TestHelper.CreateTestBid();
            TECMisc misc = TestHelper.CreateTestMisc(CostType.TEC);
            bid.MiscCosts.Add(misc);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            double initialTecCost = tecVM.TotalCost;
            double initialTecLabor = tecVM.TotalLabor;

            double initialElecCost = elecVM.TotalCost;
            double initialElecLabor = elecVM.TotalLabor;

            Total totalTEC = calculateTotal(misc, CostType.TEC);
            Total totalElec = calculateTotal(misc, CostType.Electrical);

            //Act
            bid.MiscCosts.Remove(misc);

            //Assert
            Assert.AreEqual(tecVM.TotalCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(elecVM.TotalLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(tecVM, elecVM, bid);
        }

        [TestMethod]
        public void RemoveElectricalMisc()
        {
            //Arrange
            TECBid bid = TestHelper.CreateTestBid();
            TECMisc misc = TestHelper.CreateTestMisc(CostType.Electrical);
            bid.MiscCosts.Add(misc);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            double initialTecCost = tecVM.TotalCost;
            double initialTecLabor = tecVM.TotalLabor;

            double initialElecCost = elecVM.TotalCost;
            double initialElecLabor = elecVM.TotalLabor;

            Total totalTEC = calculateTotal(misc, CostType.TEC);
            Total totalElec = calculateTotal(misc, CostType.Electrical);

            //Act
            bid.MiscCosts.Remove(misc);

            //Assert
            Assert.AreEqual(tecVM.TotalCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(elecVM.TotalLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(tecVM, elecVM, bid);
        }

        [TestMethod]
        public void RemovePanel()
        {
            //Arrange
            TECBid bid = TestHelper.CreateTestBid();
            TECPanel panel = TestHelper.CreateTestPanel(bid.Catalogs);
            TestHelper.AssignSecondaryProperties(panel, bid.Catalogs);
            bid.Panels.Add(panel);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            double initialTecCost = tecVM.TotalCost;
            double initialTecLabor = tecVM.TotalLabor;

            double initialElecCost = elecVM.TotalCost;
            double initialElecLabor = elecVM.TotalLabor;

            Total totalTEC = calculateTotal(panel, CostType.TEC);
            Total totalElec = calculateTotal(panel, CostType.Electrical);

            //Act
            bid.Panels.Remove(panel);

            //Assert
            Assert.AreEqual(tecVM.TotalCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(elecVM.TotalLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(tecVM, elecVM, bid);
        }

        [TestMethod]
        public void RemoveController()
        {
            //Arrange
            TECBid bid = TestHelper.CreateTestBid();
            TECController controller = TestHelper.CreateTestController(bid.Catalogs);
            TestHelper.AssignSecondaryProperties(controller, bid.Catalogs);
            bid.Controllers.Add(controller);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            double initialTecCost = tecVM.TotalCost;
            double initialTecLabor = tecVM.TotalLabor;

            double initialElecCost = elecVM.TotalCost;
            double initialElecLabor = elecVM.TotalLabor;

            Total totalTEC = calculateTotal(controller, CostType.TEC);
            Total totalElec = calculateTotal(controller, CostType.Electrical);

            //Act
            bid.Controllers.Remove(controller);

            //Assert
            Assert.AreEqual(tecVM.TotalCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(elecVM.TotalLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(tecVM, elecVM, bid);
        }

        [TestMethod]
        public void RemovePoint()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();

            TECSystem typical = new TECSystem();
            bid.Systems.Add(typical);

            TECEquipment typEquip = new TECEquipment();
            typical.Equipment.Add(typEquip);

            TECSubScope typSS = new TECSubScope();
            typEquip.SubScope.Add(typSS);

            TECPoint point = TestHelper.CreateTestPoint(bid.Catalogs);
            TestHelper.AssignSecondaryProperties(point, bid.Catalogs);
            typSS.Points.Add(point);

            typical.AddInstance(bid);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            double initialTecCost = tecVM.TotalCost;
            double initialTecLabor = tecVM.TotalLabor;

            double initialElecCost = elecVM.TotalCost;
            double initialElecLabor = elecVM.TotalLabor;

            Total totalTEC = calculateTotal(point, CostType.TEC);
            Total totalElec = calculateTotal(point, CostType.Electrical);

            //Act
            typSS.Points.Remove(point);

            //Assert
            Assert.AreEqual(tecVM.TotalCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(elecVM.TotalLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(tecVM, elecVM, bid);
        }

        [TestMethod]
        public void RemoveDevice()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();

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

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            double initialTecCost = tecVM.TotalCost;
            double initialTecLabor = tecVM.TotalLabor;

            double initialElecCost = elecVM.TotalCost;
            double initialElecLabor = elecVM.TotalLabor;

            Total totalTEC = calculateTotal(device, CostType.TEC);
            Total totalElec = calculateTotal(device, CostType.Electrical);

            //Act
            typSS.Devices.Remove(device);

            //Assert
            Assert.AreEqual(tecVM.TotalCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(elecVM.TotalLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(tecVM, elecVM, bid);
        }

        [TestMethod]
        public void RemoveSubScope()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();

            TECSystem typical = new TECSystem();
            bid.Systems.Add(typical);

            TECEquipment typEquip = new TECEquipment();
            typical.Equipment.Add(typEquip);

            TECSubScope subScope = TestHelper.CreateTestSubScope(bid.Catalogs);
            TestHelper.AssignSecondaryProperties(subScope, bid.Catalogs);
            typEquip.SubScope.Add(subScope);

            typical.AddInstance(bid);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            double initialTecCost = tecVM.TotalCost;
            double initialTecLabor = tecVM.TotalLabor;

            double initialElecCost = elecVM.TotalCost;
            double initialElecLabor = elecVM.TotalLabor;

            Total totalTEC = calculateTotal(subScope, CostType.TEC);
            Total totalElec = calculateTotal(subScope, CostType.Electrical);

            //Act
            typEquip.SubScope.Remove(subScope);

            //Assert
            Assert.AreEqual(tecVM.TotalCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(elecVM.TotalLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(tecVM, elecVM, bid);
        }

        [TestMethod]
        public void RemoveEquipment()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();

            TECSystem typical = new TECSystem();
            bid.Systems.Add(typical);

            TECEquipment equip = TestHelper.CreateTestEquipment(bid.Catalogs);
            TestHelper.AssignSecondaryProperties(equip, bid.Catalogs);
            typical.Equipment.Add(equip);

            typical.AddInstance(bid);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            double initialTecCost = tecVM.TotalCost;
            double initialTecLabor = tecVM.TotalLabor;

            double initialElecCost = elecVM.TotalCost;
            double initialElecLabor = elecVM.TotalLabor;

            Total totalTEC = calculateTotal(equip, CostType.TEC);
            Total totalElec = calculateTotal(equip, CostType.Electrical);

            //Act
            typical.Equipment.Remove(equip);

            //Assert
            Assert.AreEqual(tecVM.TotalCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(elecVM.TotalLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(tecVM, elecVM, bid);
        }

        [TestMethod]
        public void RemoveInstanceSystem()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();

            TECSystem typical = TestHelper.CreateTestSystem(bid.Catalogs);
            bid.Systems.Add(typical);

            TECSystem instance = typical.AddInstance(bid);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            double initialTecCost = tecVM.TotalCost;
            double initialTecLabor = tecVM.TotalLabor;

            double initialElecCost = elecVM.TotalCost;
            double initialElecLabor = elecVM.TotalLabor;

            Total totalTEC = calculateTotal(instance, CostType.TEC);
            Total totalElec = calculateTotal(instance, CostType.Electrical);

            //Act
            typical.SystemInstances.Remove(instance);

            //Assert
            Assert.AreEqual(tecVM.TotalCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(elecVM.TotalLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(tecVM, elecVM, bid);
        }

        [TestMethod]
        public void RemoveConnection()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();

            TECController controller = new TECController(bid.Catalogs.Manufacturers.RandomObject());
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

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            double initialTecCost = tecVM.TotalCost;
            double initialTecLabor = tecVM.TotalLabor;

            double initialElecCost = elecVM.TotalCost;
            double initialElecLabor = elecVM.TotalLabor;

            Total totalTEC = calculateTotal(connection, CostType.TEC);
            Total totalElec = calculateTotal(connection, CostType.Electrical);

            //Act
            controller.RemoveSubScope(ss);

            //Assert
            Assert.AreEqual(tecVM.TotalCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(elecVM.TotalLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");

            checkRefresh(tecVM, elecVM, bid);
        }
        #endregion

        #region Calculation Methods

        private Total calculateTotal(TECCost cost, CostType type)
        {
            int qty = 1;
            if(cost is TECMisc)
            {
                qty = cost.Quantity;
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

        private Total calculateTotal(TECDevice device, CostType type)
        {
            Total total = new Total();
            if (type == CostType.TEC)
            {
                total.cost += device.ExtendedCost;
                total.labor += device.Labor;
            }
            total += calculateTotal(device as TECScope, type);
            return total;
        }

        private Total calculateTotal(TECSubScope subScope, CostType type)
        {
            Total total = new Total();
            foreach(TECDevice device in subScope.Devices)
            {
                total += calculateTotal(device, type);
            }
            foreach(TECPoint point in subScope.Points)
            {
                total += calculateTotal(point, type);
            }
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
            total += calculateTotal(controller as TECCost, type);
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

        private Total calculateTotal(TECSystem system, CostType type)
        {
            Total total = new Total();
            foreach (TECEquipment equipment in system.Equipment)
            {
                total += calculateTotal(equipment, type);
            }
            foreach(TECMisc misc in system.MiscCosts)
            {
                total += (calculateTotal(misc, type) * system.SystemInstances.Count);
            }
            foreach(TECController controller in system.Controllers)
            {
                total += calculateTotal(controller, type);
            }
            foreach(TECPanel panel in system.Panels)
            {
                total += calculateTotal(panel, type);
            }
            total += calculateTotal(system as TECScope, type);
            return total;
        }

        private Total calculateTotal(TECConnection connection, CostType type)
        {
            Total total = new Total();
            if(connection is TECSubScopeConnection)
            {
                foreach(TECConnectionType conType in (connection as TECSubScopeConnection).ConnectionTypes)
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
                    total += calculateTotal(cost, type) * connection.Length;
                }
            }
            return total;
        }

        #endregion

        private void checkRefresh(TECMaterialSummaryVM tecVM, ElectricalMaterialSummaryVM elecVM, TECBid bid)
        {
            double tecCost = tecVM.TotalCost;
            double tecLabor = tecVM.TotalLabor;
            double elecCost = elecVM.TotalCost;
            double elecLabor = elecVM.TotalLabor;

            tecVM.Refresh(bid);
            elecVM.Refresh(bid);

            Assert.AreEqual(tecCost, tecVM.TotalCost, "Total tec cost didn't refresh properly.");
            Assert.AreEqual(tecLabor, tecVM.TotalLabor, "Total tec labor didn't refresh properly.");
            Assert.AreEqual(elecCost, elecVM.TotalCost, "Total elec cost didn't refresh properly.");
            Assert.AreEqual(elecLabor, elecVM.TotalLabor, "Total elec labor didn't refresh properly.");
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
