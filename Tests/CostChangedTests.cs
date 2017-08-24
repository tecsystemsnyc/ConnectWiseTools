﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EstimatingLibrary.Utilities;
using EstimatingLibrary;

namespace Tests
{
    /// <summary>
    /// Summary description for CostChangedTests
    /// </summary>
    [TestClass]
    public class CostChangedTests
    {

        List<TECCost> costs;
        TECBid bid;
        TECManufacturer manufacturer;

        public CostChangedTests()
        {
            manufacturer = new TECManufacturer();
            manufacturer.Multiplier = 1;
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

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            bid = new TECBid();
            ChangeWatcher watcher = new ChangeWatcher(bid);
            costs = new List<TECCost>();
            watcher.CostChanged += (e) =>
            {
                costs.AddRange(e);
            };
            
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

        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Bid_AddController()
        {
            TECControllerType controllerType = new TECControllerType(manufacturer);
            controllerType.Cost = 100;
            TECController controller = new TECController(controllerType);

            bid.Controllers.Add(controller);

            Assert.AreEqual(1, costs.Count);
            Assert.AreEqual(100, cost(costs, CostType.TEC));
            Assert.AreEqual(0, cost(costs, CostType.Electrical));
        }

        [TestMethod]
        public void Bid_AddPanel()
        {
            TECPanelType panelType = new TECPanelType(manufacturer);
            panelType.Cost = 100;
            TECPanel panel = new TECPanel(panelType);

            bid.Panels.Add(panel);

            Assert.AreEqual(1, costs.Count);
            Assert.AreEqual(100, cost(costs, CostType.TEC));
            Assert.AreEqual(0, cost(costs, CostType.Electrical));
        }

        private double cost(List<TECCost> costs, CostType type)
        {
            double outCost = 0;
            foreach(TECCost item in costs)
            {
                if(item.Type == type)
                {
                    outCost += item.Cost;
                }
            }
            return outCost;
        }
    }
}