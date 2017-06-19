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
        static private TECCatalogs catalogs;

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            catalogs = TestHelper.CreateTestCatalogs();
        }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        #region TECSummaryTests
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

            TECMaterialSummaryVM vm = new TECMaterialSummaryVM(new TECBid());

            PrivateObject testVM = new PrivateObject(vm);
            testVM.Invoke("addAssCost", cost);

            Total total = calculateTotal(cost, CostType.TEC);

            Assert.AreEqual(vm.TotalCost, total.cost, "Total cost didn't update properly.");
            Assert.AreEqual(vm.TotalLabor, total.labor, "Total labor didn't update properly.");
            Assert.AreEqual(vm.TotalMiscCost, total.cost, "Total misc cost didn't update properly.");
            Assert.AreEqual(vm.TotalMiscLabor, total.labor, "Total misc labor didn't update properly.");
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

            TECMaterialSummaryVM vm = new TECMaterialSummaryVM(new TECBid());

            PrivateObject testVM = new PrivateObject(vm);
            testVM.Invoke("addAssCost", cost);

            Total total = calculateTotal(cost, CostType.TEC);

            Assert.AreEqual(vm.TotalCost, total.cost, "Total cost didn't update properly.");
            Assert.AreEqual(vm.TotalLabor, total.labor, "Total labor didn't update properly.");
            Assert.AreEqual(vm.TotalMiscCost, total.cost, "Total misc cost didn't update properly.");
            Assert.AreEqual(vm.TotalMiscLabor, total.labor, "Total misc labor didn't update properly.");
        }

        [TestMethod]
        public void AddTECMiscToBid()
        {
            TECMisc misc = TestHelper.CreateTestMisc(CostType.TEC);

            TECMaterialSummaryVM vm = new TECMaterialSummaryVM(new TECBid());

            PrivateObject testVM = new PrivateObject(vm);
            testVM.Invoke("addMiscCost", misc, null);

            Total total = calculateTotal(misc, CostType.TEC);

            Assert.AreEqual(vm.TotalCost, total.cost, "Total cost didn't update properly.");
            Assert.AreEqual(vm.TotalLabor, total.labor, "Total labor didn't update properly.");
        }

        [TestMethod]
        public void AddElectricalMiscToBid()
        {
            TECMisc misc = TestHelper.CreateTestMisc(CostType.Electrical);

            TECMaterialSummaryVM vm = new TECMaterialSummaryVM(new TECBid());

            PrivateObject testVM = new PrivateObject(vm);
            testVM.Invoke("addMiscCost", misc, null);

            Total total = calculateTotal(misc, CostType.TEC);

            Assert.AreEqual(vm.TotalCost, total.cost, "Total cost didn't update properly.");
            Assert.AreEqual(vm.TotalLabor, total.labor, "Total labor didn't update properly.");
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

            TECMaterialSummaryVM vm = new TECMaterialSummaryVM(new TECBid());

            PrivateObject testVM = new PrivateObject(vm);
            testVM.Invoke("addMiscCost", misc, system);

            Total total = calculateTotal(misc, CostType.TEC);
            total *= system.SystemInstances.Count;

            Assert.AreEqual(vm.TotalMiscCost, total.cost, "Total misc cost didn't update properly.");
            Assert.AreEqual(vm.TotalMiscLabor, total.labor, "Total misc labor didn't update properly.");
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

            TECMaterialSummaryVM vm = new TECMaterialSummaryVM(new TECBid());

            PrivateObject testVM = new PrivateObject(vm);
            testVM.Invoke("addMiscCost", misc, system);

            Total total = calculateTotal(misc, CostType.TEC);
            total *= system.SystemInstances.Count;

            Assert.AreEqual(vm.TotalMiscCost, total.cost, "Total misc cost didn't update properly.");
            Assert.AreEqual(vm.TotalMiscLabor, total.labor, "Total misc labor didn't update properly.");
        }

        [TestMethod]
        public void AddPanel()
        {
            TECPanel panel = TestHelper.CreateTestPanel(catalogs);

            TECMaterialSummaryVM vm = new TECMaterialSummaryVM(new TECBid());

            PrivateObject testVM = new PrivateObject(vm);
            testVM.Invoke("addPanel", panel);

            Total total = calculateTotal(panel, CostType.TEC);

            Assert.AreEqual(vm.TotalCost, total.cost, "Total cost didn't update properly.");
            Assert.AreEqual(vm.TotalLabor, total.labor, "Total labor didn't update properly.");
        }

        [TestMethod]
        public void AddController()
        {
            TECController controller = TestHelper.CreateTestController(catalogs);

            TECMaterialSummaryVM vm = new TECMaterialSummaryVM(new TECBid());

            PrivateObject testVM = new PrivateObject(vm);
            testVM.Invoke("addController", controller);

            Total total = calculateTotal(controller, CostType.TEC);

            Assert.AreEqual(vm.TotalCost, total.cost, "Total cost didn't update properly.");
            Assert.AreEqual(vm.TotalLabor, total.labor, "Total labor didn't update properly.");
        }

        [TestMethod]
        public void AddPoint()
        {
            TECPoint point = TestHelper.CreateTestPoint(catalogs);

            TECMaterialSummaryVM vm = new TECMaterialSummaryVM(new TECBid());

            PrivateObject testVM = new PrivateObject(vm);
            testVM.Invoke("addPoint", point);

            Total total = calculateTotal(point, CostType.TEC);

            Assert.AreEqual(vm.TotalCost, total.cost, "Total cost didn't update properly.");
            Assert.AreEqual(vm.TotalLabor, total.labor, "Total labor didn't update properly.");
        }

        [TestMethod]
        public void AddDevice()
        {
            TECDevice device = TestHelper.CreateTestDevice(catalogs);

            TECMaterialSummaryVM vm = new TECMaterialSummaryVM(new TECBid());

            PrivateObject testVM = new PrivateObject(vm);
            testVM.Invoke("addDevice", device);

            Total total = calculateTotal(device, CostType.TEC);

            Assert.AreEqual(vm.TotalCost, total.cost, "Total cost didn't update properly.");
            Assert.AreEqual(vm.TotalLabor, total.labor, "Total labor didn't update properly.");
        }

        [TestMethod]
        public void AddSubScope()
        {
            TECSubScope subscope = TestHelper.CreateTestSubScope(catalogs);

            TECMaterialSummaryVM vm = new TECMaterialSummaryVM(new TECBid());

            PrivateObject testVM = new PrivateObject(vm);
            testVM.Invoke("addSubScope", subscope);

            Total total = calculateTotal(subscope, CostType.TEC);

            Assert.AreEqual(vm.TotalCost, total.cost, "Total cost didn't update properly.");
            Assert.AreEqual(vm.TotalLabor, total.labor, "Total labor didn't update properly.");
        }

        [TestMethod]
        public void AddEquipment()
        {
            TECEquipment equipment = TestHelper.CreateTestEquipment(catalogs);

            TECMaterialSummaryVM vm = new TECMaterialSummaryVM(new TECBid());

            PrivateObject testVM = new PrivateObject(vm);
            testVM.Invoke("addEquipment", equipment);

            Total total = calculateTotal(equipment, CostType.TEC);

            Assert.AreEqual(vm.TotalCost, total.cost, "Total cost didn't update properly.");
            Assert.AreEqual(vm.TotalLabor, total.labor, "Total labor didn't update properly.");
        }

        public void AddSystem()
        {

        }
        #endregion

        #region Remove

        #endregion
        #endregion

        #region ElectricalSummaryTests
        #region Add
        [TestMethod]
        public void ElectricalSummary_AddTECCost()
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

            ElectricalMaterialSummaryVM vm = new ElectricalMaterialSummaryVM(new TECBid());

            PrivateObject testVM = new PrivateObject(vm);
            testVM.Invoke("addAssCost", cost);

            Total total = calculateTotal(cost, CostType.Electrical);

            Assert.AreEqual(vm.TotalCost, total.cost, "Total cost didn't update properly.");
            Assert.AreEqual(vm.TotalLabor, total.labor, "Total labor didn't update properly.");
            Assert.AreEqual(vm.TotalMiscCost, total.cost, "Total misc cost didn't update properly.");
            Assert.AreEqual(vm.TotalMiscLabor, total.labor, "Total misc labor didn't update properly.");
        }

        [TestMethod]
        public void ElectricalSummary_AddElectricalCost()
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

            ElectricalMaterialSummaryVM vm = new ElectricalMaterialSummaryVM(new TECBid());

            PrivateObject testVM = new PrivateObject(vm);
            testVM.Invoke("addAssCost", cost);

            Total total = calculateTotal(cost, CostType.Electrical);

            Assert.AreEqual(vm.TotalCost, total.cost, "Total cost didn't update properly.");
            Assert.AreEqual(vm.TotalLabor, total.labor, "Total labor didn't update properly.");
            Assert.AreEqual(vm.TotalMiscCost, total.cost, "Total misc cost didn't update properly.");
            Assert.AreEqual(vm.TotalMiscLabor, total.labor, "Total misc labor didn't update properly.");
        }

        [TestMethod]
        public void ElectricalSummary_AddElectricalMiscToBid()
        {
            TECMisc misc = null;
            while (misc == null)
            {
                TECMisc randomMisc = TestHelper.CreateTestMisc();
                if (randomMisc.Type == CostType.Electrical)
                {
                    misc = randomMisc;
                }
            }

            ElectricalMaterialSummaryVM vm = new ElectricalMaterialSummaryVM(new TECBid());

            PrivateObject testVM = new PrivateObject(vm);
            testVM.Invoke("addMiscCost", misc, null);

            Total total = calculateTotal(misc, CostType.Electrical);

            Assert.AreEqual(vm.TotalCost, total.cost, "Total cost didn't update properly.");
            Assert.AreEqual(vm.TotalLabor, total.labor, "Total labor didn't update properly.");
            Assert.AreEqual(vm.TotalMiscCost, total.cost, "Total misc cost didn't update properly.");
            Assert.AreEqual(vm.TotalMiscLabor, total.labor, "Total misc labor didn't update properly.");
        }

        [TestMethod]
        public void ElectricalSummary_AddTypicalSystem()
        {
            TECSystem system = TestHelper.CreateTestSystem(TestHelper.CreateTestCatalogs());

            ElectricalMaterialSummaryVM vm = new ElectricalMaterialSummaryVM(new TECBid());

            PrivateObject testVM = new PrivateObject(vm);
            testVM.Invoke("addTypicalSystem", system);

            Total total = calculateTotal(system, CostType.Electrical);

            Assert.AreEqual(vm.TotalCost, total.cost, "Total cost didn't update properly.");
            Assert.AreEqual(vm.TotalLabor, total.labor, "Total labor didn't update properly.");
            Assert.AreEqual(vm.TotalMiscCost, total.cost, "Total misc cost didn't update properly.");
            Assert.AreEqual(vm.TotalMiscLabor, total.labor, "Total misc labor didn't update properly.");
        }

        //[TestMethod]
        //public void ElectricalSummary_AddTypicalSystem()
        //{
        //    TECMisc misc = null;
        //    while (misc == null)
        //    {
        //        TECMisc randomMisc = TestHelper.CreateTestMisc();
        //        if (randomMisc.Type == CostType.Electrical)
        //        {
        //            misc = randomMisc;
        //        }
        //    }

        //    ElectricalMaterialSummaryVM vm = new ElectricalMaterialSummaryVM(new TECBid());

        //    PrivateObject testVM = new PrivateObject(vm);
        //    testVM.Invoke("addMiscCost", misc, null);

        //    Total total = calculateTotal(misc, CostType.Electrical);

        //    Assert.AreEqual(vm.TotalCost, total.cost, "Total cost didn't update properly.");
        //    Assert.AreEqual(vm.TotalLabor, total.labor, "Total labor didn't update properly.");
        //    Assert.AreEqual(vm.TotalMiscCost, total.cost, "Total misc cost didn't update properly.");
        //    Assert.AreEqual(vm.TotalMiscLabor, total.labor, "Total misc labor didn't update properly.");
        //}

        //[TestMethod]
        //public void ElectricalSummary_AddTypicalSystem()
        //{
        //    TECMisc misc = null;
        //    while (misc == null)
        //    {
        //        TECMisc randomMisc = TestHelper.CreateTestMisc();
        //        if (randomMisc.Type == CostType.Electrical)
        //        {
        //            misc = randomMisc;
        //        }
        //    }

        //    ElectricalMaterialSummaryVM vm = new ElectricalMaterialSummaryVM(new TECBid());

        //    PrivateObject testVM = new PrivateObject(vm);
        //    testVM.Invoke("addMiscCost", misc, null);

        //    Total total = calculateTotal(misc, CostType.Electrical);

        //    Assert.AreEqual(vm.TotalCost, total.cost, "Total cost didn't update properly.");
        //    Assert.AreEqual(vm.TotalLabor, total.labor, "Total labor didn't update properly.");
        //    Assert.AreEqual(vm.TotalMiscCost, total.cost, "Total misc cost didn't update properly.");
        //    Assert.AreEqual(vm.TotalMiscLabor, total.labor, "Total misc labor didn't update properly.");
        //}

        //[TestMethod]
        //public void ElectricalSummary_AddTypicalSystem()
        //{
        //    TECMisc misc = null;
        //    while (misc == null)
        //    {
        //        TECMisc randomMisc = TestHelper.CreateTestMisc();
        //        if (randomMisc.Type == CostType.Electrical)
        //        {
        //            misc = randomMisc;
        //        }
        //    }

        //    ElectricalMaterialSummaryVM vm = new ElectricalMaterialSummaryVM(new TECBid());

        //    PrivateObject testVM = new PrivateObject(vm);
        //    testVM.Invoke("addMiscCost", misc, null);

        //    Total total = calculateTotal(misc, CostType.Electrical);

        //    Assert.AreEqual(vm.TotalCost, total.cost, "Total cost didn't update properly.");
        //    Assert.AreEqual(vm.TotalLabor, total.labor, "Total labor didn't update properly.");
        //    Assert.AreEqual(vm.TotalMiscCost, total.cost, "Total misc cost didn't update properly.");
        //    Assert.AreEqual(vm.TotalMiscLabor, total.labor, "Total misc labor didn't update properly.");
        //}

        //[TestMethod]
        //public void ElectricalSummary_AddTypicalSystem()
        //{
        //    TECMisc misc = null;
        //    while (misc == null)
        //    {
        //        TECMisc randomMisc = TestHelper.CreateTestMisc();
        //        if (randomMisc.Type == CostType.Electrical)
        //        {
        //            misc = randomMisc;
        //        }
        //    }

        //    ElectricalMaterialSummaryVM vm = new ElectricalMaterialSummaryVM(new TECBid());

        //    PrivateObject testVM = new PrivateObject(vm);
        //    testVM.Invoke("addMiscCost", misc, null);

        //    Total total = calculateTotal(misc, CostType.Electrical);

        //    Assert.AreEqual(vm.TotalCost, total.cost, "Total cost didn't update properly.");
        //    Assert.AreEqual(vm.TotalLabor, total.labor, "Total labor didn't update properly.");
        //    Assert.AreEqual(vm.TotalMiscCost, total.cost, "Total misc cost didn't update properly.");
        //    Assert.AreEqual(vm.TotalMiscLabor, total.labor, "Total misc labor didn't update properly.");
        //}

        //[TestMethod]
        //public void ElectricalSummary_AddTypicalSystem()
        //{
        //    TECMisc misc = null;
        //    while (misc == null)
        //    {
        //        TECMisc randomMisc = TestHelper.CreateTestMisc();
        //        if (randomMisc.Type == CostType.Electrical)
        //        {
        //            misc = randomMisc;
        //        }
        //    }

        //    ElectricalMaterialSummaryVM vm = new ElectricalMaterialSummaryVM(new TECBid());

        //    PrivateObject testVM = new PrivateObject(vm);
        //    testVM.Invoke("addMiscCost", misc, null);

        //    Total total = calculateTotal(misc, CostType.Electrical);

        //    Assert.AreEqual(vm.TotalCost, total.cost, "Total cost didn't update properly.");
        //    Assert.AreEqual(vm.TotalLabor, total.labor, "Total labor didn't update properly.");
        //    Assert.AreEqual(vm.TotalMiscCost, total.cost, "Total misc cost didn't update properly.");
        //    Assert.AreEqual(vm.TotalMiscLabor, total.labor, "Total misc labor didn't update properly.");
        //}

        //[TestMethod]
        //public void ElectricalSummary_AddTypicalSystem()
        //{
        //    TECMisc misc = null;
        //    while (misc == null)
        //    {
        //        TECMisc randomMisc = TestHelper.CreateTestMisc();
        //        if (randomMisc.Type == CostType.Electrical)
        //        {
        //            misc = randomMisc;
        //        }
        //    }

        //    ElectricalMaterialSummaryVM vm = new ElectricalMaterialSummaryVM(new TECBid());

        //    PrivateObject testVM = new PrivateObject(vm);
        //    testVM.Invoke("addMiscCost", misc, null);

        //    Total total = calculateTotal(misc, CostType.Electrical);

        //    Assert.AreEqual(vm.TotalCost, total.cost, "Total cost didn't update properly.");
        //    Assert.AreEqual(vm.TotalLabor, total.labor, "Total labor didn't update properly.");
        //    Assert.AreEqual(vm.TotalMiscCost, total.cost, "Total misc cost didn't update properly.");
        //    Assert.AreEqual(vm.TotalMiscLabor, total.labor, "Total misc labor didn't update properly.");
        //}

        //[TestMethod]
        //public void ElectricalSummary_AddTypicalSystem()
        //{
        //    TECMisc misc = null;
        //    while (misc == null)
        //    {
        //        TECMisc randomMisc = TestHelper.CreateTestMisc();
        //        if (randomMisc.Type == CostType.Electrical)
        //        {
        //            misc = randomMisc;
        //        }
        //    }

        //    ElectricalMaterialSummaryVM vm = new ElectricalMaterialSummaryVM(new TECBid());

        //    PrivateObject testVM = new PrivateObject(vm);
        //    testVM.Invoke("addMiscCost", misc, null);

        //    Total total = calculateTotal(misc, CostType.Electrical);

        //    Assert.AreEqual(vm.TotalCost, total.cost, "Total cost didn't update properly.");
        //    Assert.AreEqual(vm.TotalLabor, total.labor, "Total labor didn't update properly.");
        //    Assert.AreEqual(vm.TotalMiscCost, total.cost, "Total misc cost didn't update properly.");
        //    Assert.AreEqual(vm.TotalMiscLabor, total.labor, "Total misc labor didn't update properly.");
        //}

        //[TestMethod]
        //public void ElectricalSummary_AddTypicalSystem()
        //{
        //    TECMisc misc = null;
        //    while (misc == null)
        //    {
        //        TECMisc randomMisc = TestHelper.CreateTestMisc();
        //        if (randomMisc.Type == CostType.Electrical)
        //        {
        //            misc = randomMisc;
        //        }
        //    }

        //    ElectricalMaterialSummaryVM vm = new ElectricalMaterialSummaryVM(new TECBid());

        //    PrivateObject testVM = new PrivateObject(vm);
        //    testVM.Invoke("addMiscCost", misc, null);

        //    Total total = calculateTotal(misc, CostType.Electrical);

        //    Assert.AreEqual(vm.TotalCost, total.cost, "Total cost didn't update properly.");
        //    Assert.AreEqual(vm.TotalLabor, total.labor, "Total labor didn't update properly.");
        //    Assert.AreEqual(vm.TotalMiscCost, total.cost, "Total misc cost didn't update properly.");
        //    Assert.AreEqual(vm.TotalMiscLabor, total.labor, "Total misc labor didn't update properly.");
        //}
        #endregion

        #region Remove

        #endregion
        #endregion

        #region Calculation Methods

        private Total calculateTotal(TECCost cost, CostType type)
        {
            if (cost.Type == type)
            {
                Total total = new Total();
                total.cost = cost.Cost * cost.Quantity;
                total.labor = cost.Labor * cost.Quantity;
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
            total += calculateTotal(device as TECCost, type);
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
                calculateTotal(controller, type);
            }
            foreach(TECPanel panel in system.Panels)
            {
                calculateTotal(panel, type);
            }
            total += calculateTotal(system as TECScope, type);
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
