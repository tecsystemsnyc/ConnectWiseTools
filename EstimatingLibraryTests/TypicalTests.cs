﻿using EstimatingLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class TypicalTests
    {
        private TestContext testContextInstance;
        
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
        
        [TestMethod]
        public void AddInstances()
        {
            TECBid bid = new TECBid();
            int qty = 3;
            bid.Catalogs = TestHelper.CreateTestCatalogs();
            TECTypical system = TestHelper.CreateTestTypical(bid.Catalogs);
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
        public void EditInstances()
        {
            TECBid bid = new TECBid();
            int qty = 3;
            bid.Catalogs = TestHelper.CreateTestCatalogs();
            TECTypical system = TestHelper.CreateTestTypical(bid.Catalogs);
            bid.Systems.Add(system);
            for (int x = 0; x < qty; x++)
            {
                system.AddInstance(bid);
            }

            system.Equipment.Add(TestHelper.CreateTestEquipment(true, bid.Catalogs));
            system.AddController(TestHelper.CreateTestController(true, bid.Catalogs));
            system.Panels.Add(TestHelper.CreateTestPanel(true, bid.Catalogs));

            foreach (TECSystem instance in system.Instances)
            {
                Assert.AreEqual(system.Equipment.Count, instance.Equipment.Count);
                Assert.AreEqual(system.Controllers.Count, instance.Controllers.Count);
                Assert.AreEqual(system.Panels.Count, instance.Panels.Count);
            }
        }

        [TestMethod]
        public void AddRemoveSystemInstanceWithBidConnection()
        {
            var bid = new TECBid();
            var bidController = new TECController(new TECControllerType(new TECManufacturer()), false);
            bid.AddController(bidController);

            var system = new TECTypical();
            var equipment = new TECEquipment(true);
            var subScope = new TECSubScope(true);
            system.Equipment.Add(equipment);
            equipment.SubScope.Add(subScope);
            bidController.AddSubScope(subScope);
            var instance = system.AddInstance(bid);
            bidController.AddSubScope(instance.GetAllSubScope()[0]);
            
            Assert.AreEqual(2, bidController.ChildrenConnections.Count, "Connection not added");

            system.Instances.Remove(instance);

            Assert.AreEqual(1, bidController.ChildrenConnections.Count, "Connection not removed");
        }

        [TestMethod]
        public void RemoveTypicalSubScopeFromBidController()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECController controller = new TECController(new TECControllerType(new TECManufacturer()), false);
            bid.AddController(controller);

            TECTypical typical = new TECTypical();
            TECEquipment equip = new TECEquipment(true);
            TECSubScope ss = new TECSubScope(true);
            typical.Equipment.Add(equip);
            equip.SubScope.Add(ss);
            bid.Systems.Add(typical);

            TECSystem instance = typical.AddInstance(bid);
            TECSubScope instanceSS = instance.Equipment[0].SubScope[0];

            controller.AddSubScope(ss);

            //Act
            controller.RemoveSubScope(ss);

            //Assert
            Assert.IsTrue(instanceSS.Connection == null, "Instance subscope connection wasn't removed.");
            Assert.AreEqual(0, controller.ChildrenConnections.Count, "Bid controller still contains connections.");
        }

        [TestMethod]
    }
}
