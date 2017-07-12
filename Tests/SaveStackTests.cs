using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using System.Collections.ObjectModel;

namespace Tests
{
    [TestClass]
    public class SaveStackTests
    {
        #region Add
        #region Bid
        #region Controller
        [TestMethod]
        public void Bid_AddController()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECManufacturer manufacturer = new TECManufacturer();
            TECController controller = new TECController(manufacturer);

            //Act
            ChangeStack stack = new ChangeStack(bid);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.Add, controller, manufacturer));
            expectedItems.Add(new StackItem(Change.Add, bid, controller));

            int expectedCount = 2;

            bid.Controllers.Add(controller);

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }
        #endregion
        #region Panel
        [TestMethod]
        public void Bid_AddPanel()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECPanelType type = new TECPanelType();
            TECPanel panel = new TECPanel(type);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.Add, panel, type));
            expectedItems.Add(new StackItem(Change.Add, bid, panel));

            int expectedCount = 2;
            
            bid.Panels.Add(panel);

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }
        #endregion
        #region Misc
        [TestMethod]
        public void Bid_AddMisc()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECMisc misc = new TECMisc();

            //Act
            ChangeStack stack = new ChangeStack(bid);
            StackItem expectedItem = new StackItem(Change.Add, bid, misc);
            int expectedCount = 1;

            bid.MiscCosts.Add(misc);

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
        }
        #endregion
        #region Scope Branch
        [TestMethod]
        public void Bid_AddScopeBranch()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECScopeBranch scopeBranch = new TECScopeBranch();

            //Act
            ChangeStack stack = new ChangeStack(bid);
            StackItem expectedItem = new StackItem(Change.Add, bid, scopeBranch);
            int expectedCount = 1;

            bid.ScopeTree.Add(scopeBranch);

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
        }
        #endregion
        #endregion

        #region System
        [TestMethod]
        public void Bid_AddSystem()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();

            //Act
            ChangeStack stack = new ChangeStack(bid);

            StackItem expectedItem = new StackItem(Change.Add, bid, system);

            bid.Systems.Add(system);

            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
        }

        [TestMethod]
        public void Bid_AddSystemInstance()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);

            //Act
            ChangeStack stack = new ChangeStack(bid);

            TECSystem instance = system.AddInstance(bid);
            StackItem expectedItem = new StackItem(Change.Add, system, instance);
            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
        }
        #region Equipment
        [TestMethod]
        public void Bid_AddEquipmentToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            TECEquipment equip = new TECEquipment();
            bid.Systems.Add(system);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            StackItem expectedItem = new StackItem(Change.Add, system, equip);
            int expectedCount = 1;

            system.Equipment.Add(equip);

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
        }

        [TestMethod]
        public void Bid_AddEquipmentToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem typical = new TECSystem();
            TECEquipment equip = new TECEquipment();
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            ChangeStack stack = new ChangeStack(bid);

            typical.Equipment.Add(equip);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.AddRelationship, equip, instance.Equipment[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.Equipment[0]));
            expectedItems.Add(new StackItem(Change.Add, typical, equip));
            
            int expectedCount = 3;
            
            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToTypicalWithEquipment()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem typical = new TECSystem();
            TECEquipment equip = new TECEquipment();
            bid.Systems.Add(typical);
            typical.Equipment.Add(equip);

            //Act
            ChangeStack stack = new ChangeStack(bid);

            TECSystem instance = typical.AddInstance(bid);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.AddRelationship, equip, instance.Equipment[0]));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.Equipment[0]));
            expectedItems.Add(new StackItem(Change.Add, typical, instance));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }
        #endregion
        #region SubScope
        [TestMethod]
        public void Bid_AddSubScopeToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);

            //Act
            ChangeStack stack = new ChangeStack(bid);

            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            StackItem expectedItem = new StackItem(Change.Add, equipment, subScope);
            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
        }

        [TestMethod]
        public void Bid_AddSubScopeToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSystem instance = system.AddInstance(bid);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.AddRelationship, subScope, instance.Equipment[0].SubScope[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance.Equipment[0], instance.Equipment[0].SubScope[0]));
            expectedItems.Add(new StackItem(Change.Add, system.Equipment[0], subScope));

            int expectedCount = 3;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToSystemWithSubScope()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            TECSystem instance = system.AddInstance(bid);
            
            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.AddRelationship, subScope, instance.Equipment[0].SubScope[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.AddRelationship, equipment, instance.Equipment[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.Equipment[0]));
            expectedItems.Add(new StackItem(Change.Add, instance.Equipment[0], instance.Equipment[0].SubScope[0]));
            expectedItems.Add(new StackItem(Change.Add, system, instance));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }
        #endregion
        #region Point
        [TestMethod]
        public void Bid_AddPointToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            TECPoint point = new TECPoint();
            subScope.Points.Add(point);
            
            StackItem expectedItem = new StackItem(Change.Add, subScope, point);
            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
        }

        [TestMethod]
        public void Bid_AddPointToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECSystem instance = system.AddInstance(bid);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            TECPoint point = new TECPoint();
            subScope.Points.Add(point);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.AddRelationship, point, instance.Equipment[0].SubScope[0].Points[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance.Equipment[0].SubScope[0], instance.Equipment[0].SubScope[0].Points[0]));
            expectedItems.Add(new StackItem(Change.Add, system.Equipment[0].SubScope[0], point));

            int expectedCount = 3;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToSystemWithPoint()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECPoint point = new TECPoint();
            subScope.Points.Add(point);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            TECSystem instance = system.AddInstance(bid);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.AddRelationship, point, instance.Equipment[0].SubScope[0].Points[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.AddRelationship, subScope, instance.Equipment[0].SubScope[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.AddRelationship, equipment, instance.Equipment[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.Equipment[0]));
            expectedItems.Add(new StackItem(Change.Add, instance.Equipment[0], instance.Equipment[0].SubScope[0]));
            expectedItems.Add(new StackItem(Change.Add, instance.Equipment[0].SubScope[0], instance.Equipment[0].SubScope[0].Points[0]));
            expectedItems.Add(new StackItem(Change.Add, system, instance));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }
        #endregion
        #region Device
        [TestMethod]
        public void Bid_AddDeviceToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            TECDevice device = new TECDevice(new ObservableCollection<TECConnectionType>(), new TECManufacturer());
            subScope.Devices.Add(device);

            StackItem expectedItem = new StackItem(Change.Add, subScope, device);
            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
        }

        [TestMethod]
        public void Bid_AddDeviceToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECSystem instance = system.AddInstance(bid);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            TECDevice device = new TECDevice(new ObservableCollection<TECConnectionType>(), new TECManufacturer());
            subScope.Devices.Add(device);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.Add, instance.Equipment[0].SubScope[0], instance.Equipment[0].SubScope[0].Devices[0]));
            expectedItems.Add(new StackItem(Change.Add, system.Equipment[0].SubScope[0], device));

            int expectedCount = 2;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToSystemWithDevice()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECDevice device = new TECDevice(new ObservableCollection<TECConnectionType>(), new TECManufacturer());
            bid.Catalogs.Devices.Add(device);
            subScope.Devices.Add(device);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            TECSystem instance = system.AddInstance(bid);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.AddRelationship, subScope, instance.Equipment[0].SubScope[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.AddRelationship, equipment, instance.Equipment[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.Equipment[0]));
            expectedItems.Add(new StackItem(Change.Add, instance.Equipment[0], instance.Equipment[0].SubScope[0]));
            expectedItems.Add(new StackItem(Change.Add, instance.Equipment[0].SubScope[0], instance.Equipment[0].SubScope[0].Devices[0]));
            expectedItems.Add(new StackItem(Change.Add, system, instance));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }
        #endregion
        #region Controller
        [TestMethod]
        public void Bid_AddControllerToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            TECManufacturer manufacturer = new TECManufacturer();
            TECController controller = new TECController(manufacturer);
            bid.Systems.Add(system);

            //Act
            ChangeStack stack = new ChangeStack(bid);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.Add, controller, manufacturer));
            expectedItems.Add(new StackItem(Change.Add, system, controller));

            int expectedCount = 2;

            system.Controllers.Add(controller);

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddControllerToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem typical = new TECSystem();
            TECManufacturer manufacturer = new TECManufacturer();
            TECController controller = new TECController(manufacturer);
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            ChangeStack stack = new ChangeStack(bid);

            typical.Controllers.Add(controller);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.AddRelationship, controller, instance.Controllers[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance.Controllers[0], manufacturer));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.Controllers[0]));
            expectedItems.Add(new StackItem(Change.Add, controller, manufacturer));
            expectedItems.Add(new StackItem(Change.Add, typical, controller));

            int expectedCount = 5;

            
            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToTypicalWithController()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem typical = new TECSystem();
            TECController controller = new TECController(new TECManufacturer());
            bid.Systems.Add(typical);
            typical.Controllers.Add(controller);

            //Act
            ChangeStack stack = new ChangeStack(bid);

            TECSystem instance = typical.AddInstance(bid);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.AddRelationship, controller, instance.Controllers[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.Controllers[0]));
            expectedItems.Add(new StackItem(Change.Add, typical, instance));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }
        #endregion
        #region Panel
        [TestMethod]
        public void Bid_AddPanelToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            TECPanelType type = new TECPanelType();
            TECPanel panel = new TECPanel(type);
            bid.Systems.Add(system);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.Add, panel, type));
            expectedItems.Add(new StackItem(Change.Add, system, panel));
            int expectedCount = 2;

            system.Panels.Add(panel);

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddPanelToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem typical = new TECSystem();
            TECPanelType type = new TECPanelType();
            TECPanel panel = new TECPanel(type);
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            ChangeStack stack = new ChangeStack(bid);

            typical.Panels.Add(panel);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.AddRelationship, panel, instance.Panels[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance.Panels[0], type));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.Panels[0]));
            expectedItems.Add(new StackItem(Change.Add, panel, type));
            expectedItems.Add(new StackItem(Change.Add, typical, panel));

            int expectedCount = expectedItems.Count;


            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToTypicalWithPanel()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem typical = new TECSystem();
            TECPanelType type = new TECPanelType();
            TECPanel panel = new TECPanel(type);
            bid.Systems.Add(typical);
            typical.Panels.Add(panel);

            //Act
            ChangeStack stack = new ChangeStack(bid);

            TECSystem instance = typical.AddInstance(bid);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.AddRelationship, panel, instance.Panels[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.Panels[0]));
            expectedItems.Add(new StackItem(Change.Add, instance.Panels[0], type));
            expectedItems.Add(new StackItem(Change.Add, typical, instance));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }
        #endregion
        #region Misc
        [TestMethod]
        public void Bid_AddMiscToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            TECMisc misc = new TECMisc();
            bid.Systems.Add(system);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            StackItem expectedItem = new StackItem(Change.Add, system, misc);
            int expectedCount = 1;

            system.MiscCosts.Add(misc);

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
        }

        [TestMethod]
        public void Bid_AddMiscToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem typical = new TECSystem();
            TECMisc misc = new TECMisc();
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            ChangeStack stack = new ChangeStack(bid);

            typical.MiscCosts.Add(misc);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.Add, typical, misc));

            int expectedCount = expectedItems.Count;


            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToTypicalWithMisc()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem typical = new TECSystem();
            TECMisc misc = new TECMisc();
            bid.Systems.Add(typical);
            typical.MiscCosts.Add(misc);

            //Act
            ChangeStack stack = new ChangeStack(bid);

            TECSystem instance = typical.AddInstance(bid);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.Add, typical, instance));

            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }
        #endregion
        #region Scope Branch
        [TestMethod]
        public void Bid_AddScopeBranchToTypical()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            TECScopeBranch scopeBranch = new TECScopeBranch();
            bid.Systems.Add(system);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            StackItem expectedItem = new StackItem(Change.Add, system, scopeBranch);
            int expectedCount = 1;

            system.ScopeBranches.Add(scopeBranch);

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
        }
        #endregion
        #endregion
        #endregion

        #region Remove
        #region Bid
        #region Controller
        [TestMethod]
        public void Bid_RemoveController()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECManufacturer manufacturer = new TECManufacturer();
            TECController controller = new TECController(manufacturer);
            bid.Controllers.Add(controller);

            //Act
            ChangeStack stack = new ChangeStack(bid);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.Remove, controller, manufacturer));
            expectedItems.Add(new StackItem(Change.Remove, bid, controller));

            int expectedCount = 2;

            bid.Controllers.Remove(controller);

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }
        #endregion
        #region Panel
        [TestMethod]
        public void Bid_RemovePanel()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECPanelType type = new TECPanelType();
            TECPanel panel = new TECPanel(type);
            bid.Panels.Add(panel);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.Remove, panel, type));
            expectedItems.Add(new StackItem(Change.Remove, bid, panel));
            int expectedCount = 2;

            bid.Panels.Remove(panel);

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }
        #endregion
        #region Misc
        [TestMethod]
        public void Bid_RemoveMisc()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECMisc misc = new TECMisc();
            bid.MiscCosts.Add(misc);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            StackItem expectedItem = new StackItem(Change.Remove, bid, misc);
            int expectedCount = 1;

            bid.MiscCosts.Remove(misc);

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
        }
        #endregion
        #region Scope Branch
        [TestMethod]
        public void Bid_RemoveScopeBranch()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECScopeBranch scopeBranch = new TECScopeBranch();
            bid.ScopeTree.Add(scopeBranch);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            StackItem expectedItem = new StackItem(Change.Remove, bid, scopeBranch);
            int expectedCount = 1;

            bid.ScopeTree.Remove(scopeBranch);

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
        }
        #endregion
        #endregion

        #region System
        [TestMethod]
        public void Bid_RemoveSystem()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            StackItem expectedItem = new StackItem(Change.Remove, bid, system);

            bid.Systems.Remove(system);

            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
        }

        [TestMethod]
        public void Bid_RemoveSystemInstance()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECSystem instance = system.AddInstance(bid);

            //Act
            ChangeStack stack = new ChangeStack(bid);

            system.SystemInstances.Remove(instance);
            StackItem expectedItem = new StackItem(Change.Remove, system, instance);
            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
        }
        #region Equipment
        [TestMethod]
        public void Bid_RemoveEquipmentToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            TECEquipment equip = new TECEquipment();
            bid.Systems.Add(system);
            system.Equipment.Add(equip);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            StackItem expectedItem = new StackItem(Change.Remove, system, equip);
            int expectedCount = 1;

            system.Equipment.Remove(equip);

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
        }

        [TestMethod]
        public void Bid_RemoveEquipmentToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem typical = new TECSystem();
            TECEquipment equip = new TECEquipment();
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            typical.Equipment.Add(equip);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            var removed = instance.Equipment[0];
            typical.Equipment.Remove(equip);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.RemoveRelationship, equip, removed, typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Remove, instance, removed));
            expectedItems.Add(new StackItem(Change.Remove, typical, equip));

            int expectedCount = 3;
            

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceToTypicalWithEquipment()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem typical = new TECSystem();
            TECEquipment equip = new TECEquipment();
            bid.Systems.Add(typical);
            typical.Equipment.Add(equip);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            ChangeStack stack = new ChangeStack(bid);

            typical.SystemInstances.Remove(instance);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.RemoveRelationship, equip, instance.Equipment[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Remove, instance, instance.Equipment[0]));
            expectedItems.Add(new StackItem(Change.Remove, typical, instance));

            int expectedCount = 3;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }
        #endregion
        #region SubScope
        [TestMethod]
        public void Bid_RemoveSubScopeToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);

            //Act
            ChangeStack stack = new ChangeStack(bid);

            equipment.SubScope.Remove(subScope);
            StackItem expectedItem = new StackItem(Change.Remove, equipment, subScope);
            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
        }

        [TestMethod]
        public void Bid_RemoveSubScopeToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSystem instance = system.AddInstance(bid);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            var removed = instance.Equipment[0].SubScope[0];
            equipment.SubScope.Remove(subScope);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.RemoveRelationship, subScope, removed, typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Remove, instance.Equipment[0], removed));
            expectedItems.Add(new StackItem(Change.Remove, system.Equipment[0], subScope));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceToSystemWithSubScope()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECSystem instance = system.AddInstance(bid);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            system.SystemInstances.Remove(instance);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.RemoveRelationship, subScope, instance.Equipment[0].SubScope[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Remove, instance.Equipment[0], instance.Equipment[0].SubScope[0]));
            expectedItems.Add(new StackItem(Change.RemoveRelationship, equipment, instance.Equipment[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Remove, instance, instance.Equipment[0]));
            expectedItems.Add(new StackItem(Change.Remove, system, instance));

            int expectedCount = 5;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }
        #endregion
        #region Point
        [TestMethod]
        public void Bid_RemovePointToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECPoint point = new TECPoint();
            subScope.Points.Add(point);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            subScope.Points.Remove(point);

            StackItem expectedItem = new StackItem(Change.Remove, subScope, point);
            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
        }

        [TestMethod]
        public void Bid_RemovePointToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECSystem instance = system.AddInstance(bid);
            TECPoint point = new TECPoint();
            subScope.Points.Add(point);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            var removed = instance.Equipment[0].SubScope[0].Points[0];
            subScope.Points.Remove(point);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.RemoveRelationship, point, removed, typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Remove, instance.Equipment[0].SubScope[0], removed));
            expectedItems.Add(new StackItem(Change.Remove, system.Equipment[0].SubScope[0], point));

            int expectedCount = 3;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceToSystemWithPoint()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECPoint point = new TECPoint();
            subScope.Points.Add(point);
            TECSystem instance = system.AddInstance(bid);
            
            //Act
            ChangeStack stack = new ChangeStack(bid);
            system.SystemInstances.Remove(instance);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.RemoveRelationship, point, instance.Equipment[0].SubScope[0].Points[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Remove, instance.Equipment[0].SubScope[0], instance.Equipment[0].SubScope[0].Points[0]));
            expectedItems.Add(new StackItem(Change.RemoveRelationship, subScope, instance.Equipment[0].SubScope[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Remove, instance.Equipment[0], instance.Equipment[0].SubScope[0]));
            expectedItems.Add(new StackItem(Change.RemoveRelationship, equipment, instance.Equipment[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Remove, instance, instance.Equipment[0]));
            expectedItems.Add(new StackItem(Change.Remove, system, instance));

            int expectedCount = 7;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }
        #endregion
        #region Device
        [TestMethod]
        public void Bid_RemoveDeviceToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECDevice device = new TECDevice(new ObservableCollection<TECConnectionType>(), new TECManufacturer());
            subScope.Devices.Add(device);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            subScope.Devices.Remove(device);

            StackItem expectedItem = new StackItem(Change.RemoveRelationship, subScope, device);
            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
        }

        [TestMethod]
        public void Bid_RemoveDeviceToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECSystem instance = system.AddInstance(bid);
            TECDevice device = new TECDevice(new ObservableCollection<TECConnectionType>(), new TECManufacturer());
            bid.Catalogs.Devices.Add(device);
            subScope.Devices.Add(device);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            subScope.Devices.Remove(device);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.RemoveRelationship, instance.Equipment[0].SubScope[0], device));
            expectedItems.Add(new StackItem(Change.RemoveRelationship, system.Equipment[0].SubScope[0], device));

            int expectedCount = 2;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceToSystemWithDevice()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECDevice device = new TECDevice(new ObservableCollection<TECConnectionType>(), new TECManufacturer());
            bid.Catalogs.Devices.Add(device);
            subScope.Devices.Add(device);
            TECSystem instance = system.AddInstance(bid);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            system.SystemInstances.Remove(instance);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.RemoveRelationship, subScope, instance.Equipment[0].SubScope[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.RemoveRelationship, equipment, instance.Equipment[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Remove, instance, instance.Equipment[0]));
            expectedItems.Add(new StackItem(Change.Remove, instance.Equipment[0], instance.Equipment[0].SubScope[0]));
            expectedItems.Add(new StackItem(Change.Remove, instance.Equipment[0].SubScope[0], instance.Equipment[0].SubScope[0].Devices[0]));
            expectedItems.Add(new StackItem(Change.Remove, system, instance));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }
        #endregion
        #region Controller
        [TestMethod]
        public void Bid_RemoveControllerToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            TECManufacturer manufacturer = new TECManufacturer();
            TECController controller = new TECController(manufacturer);
            bid.Systems.Add(system);
            system.Controllers.Add(controller);

            //Act
            ChangeStack stack = new ChangeStack(bid);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.Remove, controller, manufacturer));
            expectedItems.Add(new StackItem(Change.Remove, system, controller));

            int expectedCount = 2;

            system.Controllers.Remove(controller);

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveControllerToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem typical = new TECSystem();
            TECManufacturer manufacturer = new TECManufacturer();
            TECController controller = new TECController(manufacturer);
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            typical.Controllers.Add(controller);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            var removed = instance.Controllers[0];
            typical.Controllers.Remove(controller);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.RemoveRelationship, controller, removed, typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Remove, removed, manufacturer));
            expectedItems.Add(new StackItem(Change.Remove, instance, removed));
            expectedItems.Add(new StackItem(Change.Remove, controller, manufacturer));
            expectedItems.Add(new StackItem(Change.Remove, typical, controller));

            int expectedCount = expectedItems.Count;
            
            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceToTypicalWithController()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem typical = new TECSystem();
            TECController controller = new TECController(new TECManufacturer());
            bid.Systems.Add(typical);
            typical.Controllers.Add(controller);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            ChangeStack stack = new ChangeStack(bid);

            typical.SystemInstances.Remove(instance);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.RemoveRelationship, controller, instance.Controllers[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Remove, instance, instance.Controllers[0]));
            expectedItems.Add(new StackItem(Change.Remove, typical, instance));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }
        #endregion
        #region Panel
        [TestMethod]
        public void Bid_RemovePanelToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            TECPanelType type = new TECPanelType();
            TECPanel panel = new TECPanel(type);
            bid.Systems.Add(system);
            system.Panels.Add(panel);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.Remove, panel, type));
            expectedItems.Add(new StackItem(Change.Remove, system, panel));
            int expectedCount = 2;

            system.Panels.Remove(panel);

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemovePanelToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem typical = new TECSystem();
            TECPanelType type = new TECPanelType();
            TECPanel panel = new TECPanel(type);
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            typical.Panels.Add(panel);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            var removed = instance.Panels[0];
            typical.Panels.Remove(panel);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.RemoveRelationship, panel, removed, typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Remove, removed, type));
            expectedItems.Add(new StackItem(Change.Remove, instance, removed));
            expectedItems.Add(new StackItem(Change.Remove, panel, type));
            expectedItems.Add(new StackItem(Change.Remove, typical, panel));

            int expectedCount = expectedItems.Count;


            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceToTypicalWithPanel()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem typical = new TECSystem();
            TECPanelType type = new TECPanelType();
            TECPanel panel = new TECPanel(type);
            bid.Systems.Add(typical);
            typical.Panels.Add(panel);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            typical.SystemInstances.Remove(instance);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.Remove, instance, instance.Panels[0]));
            expectedItems.Add(new StackItem(Change.Remove, instance.Panels[0], type));
            expectedItems.Add(new StackItem(Change.RemoveRelationship, panel, instance.Panels[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Remove, typical, instance));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }
        #endregion
        #region Misc
        [TestMethod]
        public void Bid_RemoveMiscToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            TECMisc misc = new TECMisc();
            bid.Systems.Add(system);
            system.MiscCosts.Add(misc);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            StackItem expectedItem = new StackItem(Change.Remove, system, misc);
            int expectedCount = 1;

            system.MiscCosts.Remove(misc);

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
        }

        [TestMethod]
        public void Bid_RemoveMiscToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem typical = new TECSystem();
            TECMisc misc = new TECMisc();
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            typical.MiscCosts.Add(misc);

            //Act
            ChangeStack stack = new ChangeStack(bid);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.Remove, typical, misc));
            int expectedCount = expectedItems.Count;
            typical.MiscCosts.Remove(misc);

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceToTypicalWithMisc()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem typical = new TECSystem();
            TECMisc misc = new TECMisc();
            bid.Systems.Add(typical);
            typical.MiscCosts.Add(misc);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            typical.SystemInstances.Remove(instance);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.Remove, typical, instance));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItems(expectedItems, stack);
        }
        #endregion
        #region Scope Branch
        [TestMethod]
        public void Bid_RemoveScopeBranchToTypical()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();
            TECScopeBranch scopeBranch = new TECScopeBranch();
            bid.Systems.Add(system);
            system.ScopeBranches.Add(scopeBranch);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            StackItem expectedItem = new StackItem(Change.Remove, system, scopeBranch);
            int expectedCount = 1;

            system.ScopeBranches.Remove(scopeBranch);

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
        }
        #endregion
        #endregion
        #endregion

        public void checkStackItem(StackItem expectedItem, StackItem actualItem)
        {
            Assert.AreEqual(expectedItem.Change, actualItem.Change);
            Assert.AreEqual(expectedItem.ReferenceObject, actualItem.ReferenceObject);
            Assert.AreEqual(expectedItem.TargetObject, actualItem.TargetObject);
            Assert.AreEqual(expectedItem.ReferenceType, actualItem.ReferenceType);
            Assert.AreEqual(expectedItem.TargetType, actualItem.TargetType);
        }

        public void checkStackItems(List<StackItem> expectedItems, ChangeStack stack)
        {
            int numToCheck = expectedItems.Count;
            foreach(StackItem item in expectedItems)
            {
                checkStackItem(item, stack.SaveStack[stack.SaveStack.Count - numToCheck]);
                numToCheck--;
            }
        }
    }
}
