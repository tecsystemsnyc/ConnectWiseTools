using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EstimatingLibrary;

namespace Tests
{
    /// <summary>
    /// Summary description for ModelTests
    /// </summary>
    [TestClass]
    public class ModelTests
    {
        public ModelTests()
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
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        #region Controller 
        [TestMethod]
        public void Controller_AddSubScope()
        {
            TECController controller = new TECController(new TECControllerType(new TECManufacturer()));
            TECSubScope subScope = new TECSubScope();

            controller.AddSubScope(subScope);

            Assert.AreEqual(1, controller.ChildrenConnections.Count, "Connection not added to controller");
            Assert.AreNotEqual(null, subScope.Connection, "Connection not added to subscope");
        }

        [TestMethod]
        public void Controller_RemoveSubScope()
        {
            TECController controller = new TECController(new TECControllerType(new TECManufacturer()));
            TECSubScope subScope = new TECSubScope();

            controller.AddSubScope(subScope);
            controller.RemoveSubScope(subScope);

            Assert.AreEqual(0, controller.ChildrenConnections.Count, "Connection not removed from controller");
            Assert.AreEqual(null, subScope.Connection, "Connection not removed from subscope");
        }

        [TestMethod]
        public void Bid_TypicalSystem()
        {
            TECBid bid = new TECBid();
            bid.Catalogs = TestHelper.CreateTestCatalogs();

            TECTypical typical = new TECTypical(TestHelper.CreateTestSystem(bid.Catalogs));
            bid.Systems.Add(typical);
            
            //Assert.AreEqual(quantity, bid.Systems.Count);
            //Assert.AreEqual(quantity, bid.Controllers.Count);
            //Assert.AreEqual(quantity, bid.Panels.Count);

            foreach(TECPanel scopePanel in typical.Panels)
            {
                foreach(TECPanel bidPanel in bid.Panels)
                {
                    Assert.AreEqual(scopePanel.Controllers.Count, bidPanel.Controllers.Count);
                }
            }
            foreach (TECController scopeController in typical.Controllers)
            {
                foreach (TECController bidController in bid.Controllers)
                {
                    Assert.AreEqual(scopeController.ChildrenConnections.Count, bidController.ChildrenConnections.Count);
                }
            }
        }

        #endregion

        #region System
        
        [TestMethod]
        public void System_AddInstances()
        {
            TECBid bid = new TECBid();
            int qty = 3;
            bid.Catalogs = TestHelper.CreateTestCatalogs();
            TECTypical system = new TECTypical(TestHelper.CreateTestSystem(bid.Catalogs));
            bid.Systems.Add(system);

            for (int x = 0; x < qty; x++)
            {
                system.AddInstance(bid);
            }

            Assert.AreEqual(system.Instances.Count, qty);
            foreach (TECSystem instance in system.Instances)
            {
                Assert.AreEqual(system.Equipment.Count, instance.Equipment.Count);
                Assert.AreEqual(system.Controllers.Count, instance.Controllers.Count);
                Assert.AreEqual(system.Panels.Count, instance.Panels.Count);
            }

        }

        [TestMethod]
        public void System_EditInstances()
        {
            TECBid bid = new TECBid();
            int qty = 3;
            bid.Catalogs = TestHelper.CreateTestCatalogs();
            TECTypical system = new TECTypical(TestHelper.CreateTestSystem(bid.Catalogs));
            bid.Systems.Add(system);
            for (int x = 0; x < qty; x++)
            {
                system.AddInstance(bid);
            }

            system.Equipment.Add(TestHelper.CreateTestEquipment(bid.Catalogs));
            system.Controllers.Add(TestHelper.CreateTestController(bid.Catalogs));
            system.Panels.Add(TestHelper.CreateTestPanel(bid.Catalogs));

            foreach (TECSystem instance in system.Instances)
            {
                Assert.AreEqual(system.Equipment.Count, instance.Equipment.Count);
                Assert.AreEqual(system.Controllers.Count, instance.Controllers.Count);
                Assert.AreEqual(system.Panels.Count, instance.Panels.Count);
            }

        }

        [TestMethod]
        public void System_RemoveSystemInstanceWithBidConnection()
        {
            var bid = new TECBid();
            var bidController = new TECController(new TECControllerType(new TECManufacturer()));
            bid.Controllers.Add(bidController);

            var system = new TECTypical();
            var equipment = new TECEquipment();
            var subScope = new TECSubScope();
            system.Equipment.Add(equipment);
            equipment.SubScope.Add(subScope);
            bidController.AddSubScope(subScope);
            var instance = system.AddInstance(bid);
            
            Assert.AreEqual(2, bidController.ChildrenConnections.Count, "Connection not added");

            system.Instances.Remove(instance);

            Assert.AreEqual(1, bidController.ChildrenConnections.Count, "Connection not removed");
            
        }

        #endregion
    }
}
