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
        public SummaryVMTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        private static double delta = 1.0 / 1000.0;

        static private TECCatalogs catalogs;

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
        
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            catalogs = TestHelper.CreateTestCatalogs();
        }
        
        #region Add
        [TestMethod]
        public void AddTECCost()
        {
            TECCost cost = null;
            while(cost == null)
            {
                TECCost randomCost = catalogs.AssociatedCosts.RandomObject();
                if (randomCost.Type == CostType.TEC)
                {
                    cost = randomCost;
                }
            }

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(new TECBid());
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(new TECBid());

            PrivateObject privateTecVM = new PrivateObject(tecVM);
            PrivateObject privateElecVM = new PrivateObject(elecVM);

            privateTecVM.Invoke("addAssCost", cost);
            privateElecVM.Invoke("addAssCost", cost);

            Total totalTEC = calculateTotal(cost, CostType.TEC);
            Total totalElec = calculateTotal(cost, CostType.Electrical);

            Assert.AreEqual(tecVM.TotalCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(elecVM.TotalLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");
        }

        [TestMethod]
        public void AddElectricalCost()
        {
            TECCost cost = null;
            while (cost == null)
            {
                TECCost randomCost = catalogs.AssociatedCosts.RandomObject();
                if (randomCost.Type == CostType.Electrical)
                {
                    cost = randomCost;
                }
            }

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(new TECBid());
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(new TECBid());

            PrivateObject privateTecVM = new PrivateObject(tecVM);
            PrivateObject privateElecVM = new PrivateObject(elecVM);

            privateTecVM.Invoke("addAssCost", cost);
            privateElecVM.Invoke("addAssCost", cost);

            Total totalTEC = calculateTotal(cost, CostType.TEC);
            Total totalElec = calculateTotal(cost, CostType.Electrical);

            Assert.AreEqual(tecVM.TotalCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(elecVM.TotalLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");
        }

        [TestMethod]
        public void AddTECMiscToBid()
        {
            TECMisc misc = TestHelper.CreateTestMisc(CostType.TEC);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(new TECBid());
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(new TECBid());

            PrivateObject privateTecVM = new PrivateObject(tecVM);
            PrivateObject privateElecVM = new PrivateObject(elecVM);

            privateTecVM.Invoke("addMiscCost", misc, null);
            privateElecVM.Invoke("addMiscCost", misc, null);

            Total totalTEC = calculateTotal(misc, CostType.TEC);
            Total totalElec = calculateTotal(misc, CostType.Electrical);

            Assert.AreEqual(tecVM.TotalCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(elecVM.TotalLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");
        }

        [TestMethod]
        public void AddElectricalMiscToBid()
        {
            TECMisc misc = TestHelper.CreateTestMisc(CostType.Electrical);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(new TECBid());
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(new TECBid());

            PrivateObject privateTecVM = new PrivateObject(tecVM);
            PrivateObject privateElecVM = new PrivateObject(elecVM);

            privateTecVM.Invoke("addMiscCost", misc, null);
            privateElecVM.Invoke("addMiscCost", misc, null);

            Total totalTEC = calculateTotal(misc, CostType.TEC);
            Total totalElec = calculateTotal(misc, CostType.Electrical);

            Assert.AreEqual(tecVM.TotalCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(elecVM.TotalLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");
        }

        [TestMethod]
        public void AddTECMiscToSystem()
        {
            TECSystem system = TestHelper.CreateTestSystem(catalogs);
            for(int i = 0; i < TestHelper.RandomInt(0, 10); i++)
            {
                system.AddInstance(new TECBid());
            }
            TECMisc misc = TestHelper.CreateTestMisc(CostType.TEC);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(new TECBid());
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(new TECBid());

            PrivateObject privateTecVM = new PrivateObject(tecVM);
            PrivateObject privateElecVM = new PrivateObject(elecVM);

            privateTecVM.Invoke("addMiscCost", misc, system);
            privateElecVM.Invoke("addMiscCost", misc, system);

            Total totalTEC = calculateTotal(misc, CostType.TEC);
            totalTEC *= system.SystemInstances.Count;
            Total totalElec = calculateTotal(misc, CostType.Electrical);
            totalElec *= system.SystemInstances.Count;

            Assert.AreEqual(tecVM.TotalCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(elecVM.TotalLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");
        }

        [TestMethod]
        public void AddElectricalMiscToSystem()
        {
            TECSystem system = TestHelper.CreateTestSystem(catalogs);
            for (int i = 0; i < TestHelper.RandomInt(0, 10); i++)
            {
                system.AddInstance(new TECBid());
            }
            TECMisc misc = TestHelper.CreateTestMisc(CostType.Electrical);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(new TECBid());
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(new TECBid());

            PrivateObject privateTecVM = new PrivateObject(tecVM);
            PrivateObject privateElecVM = new PrivateObject(elecVM);

            privateTecVM.Invoke("addMiscCost", misc, system);
            privateElecVM.Invoke("addMiscCost", misc, system);

            Total totalTEC = calculateTotal(misc, CostType.TEC);
            totalTEC *= system.SystemInstances.Count;
            Total totalElec = calculateTotal(misc, CostType.Electrical);
            totalElec *= system.SystemInstances.Count;

            Assert.AreEqual(tecVM.TotalCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(elecVM.TotalLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");
        }

        [TestMethod]
        public void AddPanel()
        {
            TECPanel panel = TestHelper.CreateTestPanel(catalogs);
            TestHelper.AssignSecondaryProperties(panel, catalogs);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(new TECBid());
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(new TECBid());

            PrivateObject privateTecVM = new PrivateObject(tecVM);
            PrivateObject privateElecVM = new PrivateObject(elecVM);

            privateTecVM.Invoke("addPanel", panel);
            privateElecVM.Invoke("addPanel", panel);

            Total totalTEC = calculateTotal(panel, CostType.TEC);
            Total totalElec = calculateTotal(panel, CostType.Electrical);

            Assert.AreEqual(tecVM.TotalCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(elecVM.TotalLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");
        }

        [TestMethod]
        public void AddController()
        {
            TECController controller = TestHelper.CreateTestController(catalogs);
            TestHelper.AssignSecondaryProperties(controller, catalogs);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(new TECBid());
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(new TECBid());

            PrivateObject privateTecVM = new PrivateObject(tecVM);
            PrivateObject privateElecVM = new PrivateObject(elecVM);

            privateTecVM.Invoke("addController", controller);
            privateElecVM.Invoke("addController", controller);

            Total totalTEC = calculateTotal(controller, CostType.TEC);
            Total totalElec = calculateTotal(controller, CostType.Electrical);

            Assert.AreEqual(tecVM.TotalCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(elecVM.TotalLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");
        }

        [TestMethod]
        public void AddPoint()
        {
            TECPoint point = TestHelper.CreateTestPoint(catalogs);
            TestHelper.AssignSecondaryProperties(point, catalogs);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(new TECBid());
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(new TECBid());

            PrivateObject privateTecVM = new PrivateObject(tecVM);
            PrivateObject privateElecVM = new PrivateObject(elecVM);

            privateTecVM.Invoke("addPoint", point);
            privateElecVM.Invoke("addPoint", point);

            Total totalTEC = calculateTotal(point, CostType.TEC);
            Total totalElec = calculateTotal(point, CostType.Electrical);

            Assert.AreEqual(tecVM.TotalCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(elecVM.TotalLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");
        }

        [TestMethod]
        public void AddDevice()
        {
            TECDevice device = TestHelper.CreateTestDevice(catalogs);
            TestHelper.AssignSecondaryProperties(device, catalogs);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(new TECBid());
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(new TECBid());

            PrivateObject privateTecVM = new PrivateObject(tecVM);
            PrivateObject privateElecVM = new PrivateObject(elecVM);

            privateTecVM.Invoke("addDevice", device);
            privateElecVM.Invoke("addDevice", device);

            Total totalTEC = calculateTotal(device, CostType.TEC);
            Total totalElec = calculateTotal(device, CostType.Electrical);

            Assert.AreEqual(tecVM.TotalCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(elecVM.TotalLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");
        }

        [TestMethod]
        public void AddSubScope()
        {
            TECSubScope subscope = TestHelper.CreateTestSubScope(catalogs);
            TestHelper.AssignSecondaryProperties(subscope, catalogs);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(new TECBid());
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(new TECBid());

            PrivateObject privateTecVM = new PrivateObject(tecVM);
            PrivateObject privateElecVM = new PrivateObject(elecVM);

            privateTecVM.Invoke("addSubScope", subscope);
            privateElecVM.Invoke("addSubScope", subscope);

            Total totalTEC = calculateTotal(subscope, CostType.TEC);
            Total totalElec = calculateTotal(subscope, CostType.Electrical);

            Assert.AreEqual(tecVM.TotalCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(elecVM.TotalLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");
        }

        [TestMethod]
        public void AddEquipment()
        {
            TECEquipment equipment = TestHelper.CreateTestEquipment(catalogs);
            TestHelper.AssignSecondaryProperties(equipment, catalogs);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(new TECBid());
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(new TECBid());

            PrivateObject privateTecVM = new PrivateObject(tecVM);
            PrivateObject privateElecVM = new PrivateObject(elecVM);

            privateTecVM.Invoke("addEquipment", equipment);
            privateElecVM.Invoke("addEquipment", equipment);

            Total totalTEC = calculateTotal(equipment, CostType.TEC);
            Total totalElec = calculateTotal(equipment, CostType.Electrical);

            Assert.AreEqual(tecVM.TotalCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(elecVM.TotalLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");
        }

        [TestMethod]
        public void AddInstanceSystem()
        {
            TECSystem system = TestHelper.CreateTestSystem(catalogs);
            TestHelper.AssignSecondaryProperties(system, catalogs);

            system.AddInstance(new TECBid());

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(new TECBid());
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(new TECBid());

            PrivateObject privateTecVM = new PrivateObject(tecVM);
            PrivateObject privateElecVM = new PrivateObject(elecVM);

            privateTecVM.Invoke("addInstanceSystem", system);
            privateElecVM.Invoke("addInstanceSystem", system);

            Total totalTEC = calculateTotal(system, CostType.TEC);
            Total totalElec = calculateTotal(system, CostType.Electrical);

            Assert.AreEqual(tecVM.TotalCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(elecVM.TotalLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");
        }

        [TestMethod]
        public void AddConnection()
        {
            TECController controller = TestHelper.CreateTestController(catalogs);
            TECSubScope subScope = TestHelper.CreateTestSubScope(catalogs);
            TECConnection connection = controller.AddSubScope(subScope);
            connection.Length = 50;
            connection.ConduitLength = 50;
            connection.ConduitType = catalogs.ConduitTypes.RandomObject();

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(new TECBid());
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(new TECBid());

            PrivateObject privateTecVM = new PrivateObject(tecVM);
            PrivateObject privateElecVM = new PrivateObject(elecVM);

            privateTecVM.Invoke("addConnection", connection);
            privateElecVM.Invoke("addConnection", connection);

            Total totalTEC = calculateTotal(connection, CostType.TEC);
            Total totalElec = calculateTotal(connection, CostType.Electrical);

            Assert.AreEqual(tecVM.TotalCost, totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, totalElec.cost, delta, "Total elec cost didn't update proplery.");
            Assert.AreEqual(elecVM.TotalLabor, totalElec.labor, delta, "Total elec labor didn't update properly.");
        }
        #endregion

        #region Remove
        [TestMethod]
        public void RemoveTECCost()
        {
            TECBid bid = TestHelper.CreateTestBid();
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

            PrivateObject privateTecVM = new PrivateObject(tecVM);
            PrivateObject privateElecVM = new PrivateObject(elecVM);

            double initialTecCost = tecVM.TotalCost;
            double initialTecLabor = tecVM.TotalLabor;

            double initialElecCost = elecVM.TotalCost;
            double initialElecLabor = elecVM.TotalLabor;

            Total totalTEC = calculateTotal(cost, CostType.TEC);
            Total totalElec = calculateTotal(cost, CostType.Electrical);

            privateTecVM.Invoke("removeAssCost", cost);
            privateElecVM.Invoke("removeAssCost", cost);

            Assert.AreEqual(tecVM.TotalCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(elecVM.TotalLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");
        }

        [TestMethod]
        public void RemoveElectricalCost()
        {
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

            PrivateObject privateTecVM = new PrivateObject(tecVM);
            PrivateObject privateElecVM = new PrivateObject(elecVM);

            double initialTecCost = tecVM.TotalCost;
            double initialTecLabor = tecVM.TotalLabor;

            double initialElecCost = elecVM.TotalCost;
            double initialElecLabor = elecVM.TotalLabor;

            Total totalTEC = calculateTotal(cost, CostType.TEC);
            Total totalElec = calculateTotal(cost, CostType.Electrical);

            privateTecVM.Invoke("removeAssCost", cost);
            privateElecVM.Invoke("removeAssCost", cost);

            Assert.AreEqual(tecVM.TotalCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(elecVM.TotalLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");
        }

        [TestMethod]
        public void RemoveTECMisc()
        {
            TECBid bid = TestHelper.CreateTestBid();
            TECMisc misc = TestHelper.CreateTestMisc(CostType.TEC);
            bid.MiscCosts.Add(misc);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            PrivateObject privateTecVM = new PrivateObject(tecVM);
            PrivateObject privateElecVM = new PrivateObject(elecVM);

            double initialTecCost = tecVM.TotalCost;
            double initialTecLabor = tecVM.TotalLabor;

            double initialElecCost = elecVM.TotalCost;
            double initialElecLabor = elecVM.TotalLabor;

            Total totalTEC = calculateTotal(misc, CostType.TEC);
            Total totalElec = calculateTotal(misc, CostType.Electrical);

            privateTecVM.Invoke("removeMiscCost", misc);
            privateElecVM.Invoke("removeMiscCost", misc);

            Assert.AreEqual(tecVM.TotalCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(elecVM.TotalLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");
        }

        [TestMethod]
        public void RemoveElectricalMisc()
        {
            TECBid bid = TestHelper.CreateTestBid();
            TECMisc misc = TestHelper.CreateTestMisc(CostType.Electrical);
            bid.MiscCosts.Add(misc);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            PrivateObject privateTecVM = new PrivateObject(tecVM);
            PrivateObject privateElecVM = new PrivateObject(elecVM);

            double initialTecCost = tecVM.TotalCost;
            double initialTecLabor = tecVM.TotalLabor;

            double initialElecCost = elecVM.TotalCost;
            double initialElecLabor = elecVM.TotalLabor;

            Total totalTEC = calculateTotal(misc, CostType.TEC);
            Total totalElec = calculateTotal(misc, CostType.Electrical);

            privateTecVM.Invoke("removeMiscCost", misc);
            privateElecVM.Invoke("removeMiscCost", misc);

            Assert.AreEqual(tecVM.TotalCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(elecVM.TotalLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");
        }

        [TestMethod]
        public void RemovePanel()
        {
            TECBid bid = TestHelper.CreateTestBid();
            TECPanel panel = TestHelper.CreateTestPanel(catalogs);
            TestHelper.AssignSecondaryProperties(panel, catalogs);
            bid.Panels.Add(panel);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            PrivateObject privateTecVM = new PrivateObject(tecVM);
            PrivateObject privateElecVM = new PrivateObject(elecVM);

            double initialTecCost = tecVM.TotalCost;
            double initialTecLabor = tecVM.TotalLabor;

            double initialElecCost = elecVM.TotalCost;
            double initialElecLabor = elecVM.TotalLabor;

            Total totalTEC = calculateTotal(panel, CostType.TEC);
            Total totalElec = calculateTotal(panel, CostType.Electrical);

            privateTecVM.Invoke("removePanel", panel);
            privateElecVM.Invoke("removePanel", panel);

            Assert.AreEqual(tecVM.TotalCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(elecVM.TotalLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");
        }

        [TestMethod]
        public void RemoveController()
        {
            TECBid bid = TestHelper.CreateTestBid();
            TECController controller = TestHelper.CreateTestController(catalogs);
            TestHelper.AssignSecondaryProperties(controller, catalogs);
            bid.Controllers.Add(controller);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            PrivateObject privateTecVM = new PrivateObject(tecVM);
            PrivateObject privateElecVM = new PrivateObject(elecVM);

            double initialTecCost = tecVM.TotalCost;
            double initialTecLabor = tecVM.TotalLabor;

            double initialElecCost = elecVM.TotalCost;
            double initialElecLabor = elecVM.TotalLabor;

            Total totalTEC = calculateTotal(controller, CostType.TEC);
            Total totalElec = calculateTotal(controller, CostType.Electrical);

            privateTecVM.Invoke("removeController", controller);
            privateElecVM.Invoke("removeController", controller);

            Assert.AreEqual(tecVM.TotalCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(elecVM.TotalLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");
        }

        [TestMethod]
        public void RemovePoint()
        {
            TECBid bid = TestHelper.CreateTestBid();
            TECPoint point = bid.RandomPoint();
            TestHelper.AssignSecondaryProperties(point, catalogs);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            PrivateObject privateTecVM = new PrivateObject(tecVM);
            PrivateObject privateElecVM = new PrivateObject(elecVM);

            double initialTecCost = tecVM.TotalCost;
            double initialTecLabor = tecVM.TotalLabor;

            double initialElecCost = elecVM.TotalCost;
            double initialElecLabor = elecVM.TotalLabor;

            Total totalTEC = calculateTotal(point, CostType.TEC);
            Total totalElec = calculateTotal(point, CostType.Electrical);

            privateTecVM.Invoke("removePoint", point);
            privateElecVM.Invoke("removePoint", point);

            Assert.AreEqual(tecVM.TotalCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(elecVM.TotalLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");

        }

        [TestMethod]
        public void RemoveDevice()
        {
            TECBid bid = TestHelper.CreateTestBid();
            TECDevice device = bid.RandomDevice();
            TestHelper.AssignSecondaryProperties(device, catalogs);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            PrivateObject privateTecVM = new PrivateObject(tecVM);
            PrivateObject privateElecVM = new PrivateObject(elecVM);

            double initialTecCost = tecVM.TotalCost;
            double initialTecLabor = tecVM.TotalLabor;

            double initialElecCost = elecVM.TotalCost;
            double initialElecLabor = elecVM.TotalLabor;

            Total totalTEC = calculateTotal(device, CostType.TEC);
            Total totalElec = calculateTotal(device, CostType.Electrical);

            privateTecVM.Invoke("removeDevice", device);
            privateElecVM.Invoke("removeDevice", device);

            Assert.AreEqual(tecVM.TotalCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(elecVM.TotalLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");
        }

        [TestMethod]
        public void RemoveSubScope()
        {
            TECBid bid = TestHelper.CreateTestBid();
            TECSubScope ss = bid.RandomSubScope();
            TestHelper.AssignSecondaryProperties(ss, catalogs);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            PrivateObject privateTecVM = new PrivateObject(tecVM);
            PrivateObject privateElecVM = new PrivateObject(elecVM);

            double initialTecCost = tecVM.TotalCost;
            double initialTecLabor = tecVM.TotalLabor;

            double initialElecCost = elecVM.TotalCost;
            double initialElecLabor = elecVM.TotalLabor;

            Total totalTEC = calculateTotal(ss, CostType.TEC);
            Total totalElec = calculateTotal(ss, CostType.Electrical);

            privateTecVM.Invoke("removeSubScope", ss);
            privateElecVM.Invoke("removeSubScope", ss);

            Assert.AreEqual(tecVM.TotalCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(elecVM.TotalLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");
        }

        [TestMethod]
        public void RemoveEquipment()
        {
            TECBid bid = TestHelper.CreateTestBid();
            TECEquipment equip = bid.RandomEquipment();
            TestHelper.AssignSecondaryProperties(equip, catalogs);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            PrivateObject privateTecVM = new PrivateObject(tecVM);
            PrivateObject privateElecVM = new PrivateObject(elecVM);

            double initialTecCost = tecVM.TotalCost;
            double initialTecLabor = tecVM.TotalLabor;

            double initialElecCost = elecVM.TotalCost;
            double initialElecLabor = elecVM.TotalLabor;

            Total totalTEC = calculateTotal(equip, CostType.TEC);
            Total totalElec = calculateTotal(equip, CostType.Electrical);

            privateTecVM.Invoke("removeEquipment", equip);
            privateElecVM.Invoke("removeEquipment", equip);

            Assert.AreEqual(tecVM.TotalCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(elecVM.TotalLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");
        }

        [TestMethod]
        public void RemoveInstanceSystem()
        {
            TECBid bid = TestHelper.CreateTestBid();
            TECSystem typical = TestHelper.CreateTestSystem(bid.Catalogs);
            TestHelper.AssignSecondaryProperties(typical, catalogs);
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            PrivateObject privateTecVM = new PrivateObject(tecVM);
            PrivateObject privateElecVM = new PrivateObject(elecVM);

            double initialTecCost = tecVM.TotalCost;
            double initialTecLabor = tecVM.TotalLabor;

            double initialElecCost = elecVM.TotalCost;
            double initialElecLabor = elecVM.TotalLabor;

            Total totalTEC = calculateTotal(instance, CostType.TEC);
            Total totalElec = calculateTotal(instance, CostType.Electrical);

            privateTecVM.Invoke("removeInstanceSystem", instance);
            privateElecVM.Invoke("removeInstanceSystem", instance);

            Assert.AreEqual(tecVM.TotalCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(elecVM.TotalLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");
        }

        [TestMethod]
        public void RemoveConnection()
        {
            TECBid bid = TestHelper.CreateTestBid();
            TECController controller = bid.Controllers.RandomObject();
            TECSubScope subScope = bid.RandomSubScope();
            TECConnection connection = controller.AddSubScope(subScope);
            connection.Length = 50;
            connection.ConduitLength = 50;
            connection.ConduitType = catalogs.ConduitTypes.RandomObject();

            TECMaterialSummaryVM tecVM = new TECMaterialSummaryVM(bid);
            ElectricalMaterialSummaryVM elecVM = new ElectricalMaterialSummaryVM(bid);

            PrivateObject privateTecVM = new PrivateObject(tecVM);
            PrivateObject privateElecVM = new PrivateObject(elecVM);

            double initialTecCost = tecVM.TotalCost;
            double initialTecLabor = tecVM.TotalLabor;

            double initialElecCost = elecVM.TotalCost;
            double initialElecLabor = elecVM.TotalLabor;

            Total totalTEC = calculateTotal(connection, CostType.TEC);
            Total totalElec = calculateTotal(connection, CostType.Electrical);

            privateTecVM.Invoke("removeConnection", connection);
            privateElecVM.Invoke("removeConnection", connection);

            Assert.AreEqual(tecVM.TotalCost, initialTecCost - totalTEC.cost, delta, "Total tec cost didn't update properly.");
            Assert.AreEqual(tecVM.TotalLabor, initialTecLabor - totalTEC.labor, delta, "Total tec labor didn't update properly.");
            Assert.AreEqual(elecVM.TotalCost, initialElecCost - totalElec.cost, delta, "Total elec cost didn't update properly.");
            Assert.AreEqual(elecVM.TotalLabor, initialElecLabor - totalElec.labor, delta, "Total elec labor didn't update properly.");
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
                total += calculateTotal(misc, type) * system.SystemInstances.Count;
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
