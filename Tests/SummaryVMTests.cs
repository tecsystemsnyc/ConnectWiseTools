﻿using System;
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

            Total total = calculateCost(cost, CostType.TEC);

            Assert.AreEqual(vm.TotalCost, total.cost, "Total cost didn't update properly.");
            Assert.AreEqual(vm.TotalLabor, total.labor, "Total labor didn't update properly.");
            Assert.AreEqual(vm.TotalMiscCost, total.cost, "Total misc cost didn't update properly.");
            Assert.AreEqual(vm.TotalMiscLabor, total.labor, "Total misc labor didn't update properly.");
        }

        [TestMethod]
        public void AddTECMiscToBid()
        {
            TECMisc misc = null;
            while (misc == null)
            {
                TECMisc randomMisc = TestHelper.CreateTestMisc();
                if (randomMisc.Type == CostType.TEC)
                {
                    misc = randomMisc;
                }
            }

            TECMaterialSummaryVM vm = new TECMaterialSummaryVM(new TECBid());

            PrivateObject testVM = new PrivateObject(vm);
            testVM.Invoke("addMiscCost", misc);

            Total total = calculateTotal(misc, CostType.TEC);
        }
        #endregion

        #region Remove

        #endregion
        #endregion

        #region ElectricalSummaryTests

        #endregion

        #region Calculation Methods

        private Total calculateTotal(TECCost cost, CostType type)
        {
            if (cost.Type == type)
            {
                Total total;
                total.cost = cost.Cost * cost.Quantity;
                total.labor = cost.Labor * cost.Quantity;
                return total;
            }
            else
            {
                Total total;
                total.cost = 0;
                total.labor = 0;
                return total;
            }
        }

        
        #endregion

        private struct Total
        {
            public double cost;
            public double labor;

            public static Total operator +(Total left, Total right)
            {
                Total total;
                total.cost = left.cost + right.cost;
                total.labor = left.labor + right.labor;
                return total;
            }

            public static Total operator -(Total left, Total right)
            {
                Total total;
                total.cost = left.cost - right.cost;
                total.labor = left.labor - right.labor;
                return total;
            }
        }
    }
}
