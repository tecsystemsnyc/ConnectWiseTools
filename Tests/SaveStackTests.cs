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
            TECController controller = new TECController(new TECManufacturer());

            //Act
            ChangeStack stack = new ChangeStack(bid);
            StackItem expectedItem = new StackItem(Change.Add, bid, controller);
            int expectedCount = 1;

            bid.Controllers.Add(controller);

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
        }
        #endregion
        #region Panel
        [TestMethod]
        public void Bid_AddPanel()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECPanel panel = new TECPanel(new TECPanelType());

            //Act
            ChangeStack stack = new ChangeStack(bid);
            StackItem expectedItem = new StackItem(Change.Add, bid, panel);
            int expectedCount = 1;

            bid.Panels.Add(panel);

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
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

            //Act
            ChangeStack stack = new ChangeStack(bid);

            bid.Systems.Add(new TECSystem());

            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
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
            expectedItems.Add(new StackItem(Change.Add, equip, instance.Equipment[0]));
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
            expectedItems.Add(new StackItem(Change.Add, equip, instance.Equipment[0]));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.Equipment[0]));
            expectedItems.Add(new StackItem(Change.Add, typical, instance));

            int expectedCount = 3;

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
            expectedItems.Add(new StackItem(Change.Add, subScope, instance.Equipment[0].SubScope[0], typeof(TECScope), typeof(TECScope)));
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
            expectedItems.Add(new StackItem(Change.Add, subScope, instance.Equipment[0].SubScope[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance.Equipment[0], instance.Equipment[0].SubScope[0]));
            expectedItems.Add(new StackItem(Change.Add, equipment, instance.Equipment[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.Equipment[0]));
            expectedItems.Add(new StackItem(Change.Add, system, instance));

            int expectedCount = 5;

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
            expectedItems.Add(new StackItem(Change.Add, point, instance.Equipment[0].SubScope[0].Points[0], typeof(TECScope), typeof(TECScope)));
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
            expectedItems.Add(new StackItem(Change.Add, point, instance.Equipment[0].SubScope[0].Points[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance.Equipment[0].SubScope[0], instance.Equipment[0].SubScope[0].Points[0]));
            expectedItems.Add(new StackItem(Change.Add, subScope, instance.Equipment[0].SubScope[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance.Equipment[0], instance.Equipment[0].SubScope[0]));
            expectedItems.Add(new StackItem(Change.Add, equipment, instance.Equipment[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.Equipment[0]));
            expectedItems.Add(new StackItem(Change.Add, system, instance));

            int expectedCount = 7;

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
            subScope.Devices.Add(device);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            TECSystem instance = system.AddInstance(bid);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.Add, instance.Equipment[0].SubScope[0], instance.Equipment[0].SubScope[0].Devices[0]));
            expectedItems.Add(new StackItem(Change.Add, subScope, instance.Equipment[0].SubScope[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance.Equipment[0], instance.Equipment[0].SubScope[0]));
            expectedItems.Add(new StackItem(Change.Add, equipment, instance.Equipment[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.Equipment[0]));
            expectedItems.Add(new StackItem(Change.Add, system, instance));

            int expectedCount = 6;
            
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
            TECController controller = new TECController(new TECManufacturer());
            bid.Systems.Add(system);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            StackItem expectedItem = new StackItem(Change.Add, system, controller);
            int expectedCount = 1;

            system.Controllers.Add(controller);

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
        }

        [TestMethod]
        public void Bid_AddControllerToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem typical = new TECSystem();
            TECController controller = new TECController(new TECManufacturer());
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            ChangeStack stack = new ChangeStack(bid);

            typical.Controllers.Add(controller);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.Add, controller, instance.Controllers[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.Controllers[0]));
            expectedItems.Add(new StackItem(Change.Add, typical, controller));

            int expectedCount = 3;



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
            expectedItems.Add(new StackItem(Change.Add, controller, instance.Controllers[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.Controllers[0]));
            expectedItems.Add(new StackItem(Change.Add, typical, instance));

            int expectedCount = 3;

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
            TECPanel panel = new TECPanel(new TECPanelType());
            bid.Systems.Add(system);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            StackItem expectedItem = new StackItem(Change.Add, system, panel);
            int expectedCount = 1;

            system.Panels.Add(panel);

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
        }

        [TestMethod]
        public void Bid_AddPanelToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem typical = new TECSystem();
            TECPanel panel = new TECPanel(new TECPanelType());
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            ChangeStack stack = new ChangeStack(bid);

            typical.Panels.Add(panel);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.Add, panel, instance.Panels[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.Panels[0]));
            expectedItems.Add(new StackItem(Change.Add, typical, panel));

            int expectedCount = 3;
            

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
            TECPanel panel = new TECPanel(new TECPanelType());
            bid.Systems.Add(typical);
            typical.Panels.Add(panel);

            //Act
            ChangeStack stack = new ChangeStack(bid);

            TECSystem instance = typical.AddInstance(bid);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.Add, panel, instance.Panels[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.Panels[0]));
            expectedItems.Add(new StackItem(Change.Add, typical, instance));

            int expectedCount = 3;

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
            expectedItems.Add(new StackItem(Change.Add, misc, instance.MiscCosts[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.MiscCosts[0]));
            expectedItems.Add(new StackItem(Change.Add, typical, misc));

            int expectedCount = 3;


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
            expectedItems.Add(new StackItem(Change.Add, misc, instance.MiscCosts[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.MiscCosts[0]));
            expectedItems.Add(new StackItem(Change.Add, typical, instance));

            int expectedCount = 3;

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
            TECController controller = new TECController(new TECManufacturer());
            bid.Controllers.Add(controller);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            StackItem expectedItem = new StackItem(Change.Remove, bid, controller);
            int expectedCount = 1;

            bid.Controllers.Remove(controller);

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
        }
        #endregion
        #region Panel
        [TestMethod]
        public void Bid_RemovePanel()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECPanel panel = new TECPanel(new TECPanelType());
            bid.Panels.Add(panel);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            StackItem expectedItem = new StackItem(Change.Remove, bid, panel);
            int expectedCount = 1;

            bid.Panels.Remove(panel);

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
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
            bid.Systems.Add(new TECSystem());

            //Act
            ChangeStack stack = new ChangeStack(bid);

            bid.Systems.Remove(new TECSystem());

            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
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
            expectedItems.Add(new StackItem(Change.Add, equip, instance.Equipment[0]));
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
            expectedItems.Add(new StackItem(Change.Add, equip, instance.Equipment[0]));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.Equipment[0]));
            expectedItems.Add(new StackItem(Change.Add, typical, instance));

            int expectedCount = 3;

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
            expectedItems.Add(new StackItem(Change.Add, subScope, instance.Equipment[0].SubScope[0], typeof(TECScope), typeof(TECScope)));
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
            expectedItems.Add(new StackItem(Change.Add, subScope, instance.Equipment[0].SubScope[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance.Equipment[0], instance.Equipment[0].SubScope[0]));
            expectedItems.Add(new StackItem(Change.Add, equipment, instance.Equipment[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.Equipment[0]));
            expectedItems.Add(new StackItem(Change.Add, system, instance));

            int expectedCount = 5;

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
            expectedItems.Add(new StackItem(Change.Add, point, instance.Equipment[0].SubScope[0].Points[0], typeof(TECScope), typeof(TECScope)));
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
            expectedItems.Add(new StackItem(Change.Add, point, instance.Equipment[0].SubScope[0].Points[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance.Equipment[0].SubScope[0], instance.Equipment[0].SubScope[0].Points[0]));
            expectedItems.Add(new StackItem(Change.Add, subScope, instance.Equipment[0].SubScope[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance.Equipment[0], instance.Equipment[0].SubScope[0]));
            expectedItems.Add(new StackItem(Change.Add, equipment, instance.Equipment[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.Equipment[0]));
            expectedItems.Add(new StackItem(Change.Add, system, instance));

            int expectedCount = 7;

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
            subScope.Devices.Add(device);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            TECSystem instance = system.AddInstance(bid);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.Add, instance.Equipment[0].SubScope[0], instance.Equipment[0].SubScope[0].Devices[0]));
            expectedItems.Add(new StackItem(Change.Add, subScope, instance.Equipment[0].SubScope[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance.Equipment[0], instance.Equipment[0].SubScope[0]));
            expectedItems.Add(new StackItem(Change.Add, equipment, instance.Equipment[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.Equipment[0]));
            expectedItems.Add(new StackItem(Change.Add, system, instance));

            int expectedCount = 6;

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
            TECController controller = new TECController(new TECManufacturer());
            bid.Systems.Add(system);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            StackItem expectedItem = new StackItem(Change.Add, system, controller);
            int expectedCount = 1;

            system.Controllers.Add(controller);

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
        }

        [TestMethod]
        public void Bid_AddControllerToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem typical = new TECSystem();
            TECController controller = new TECController(new TECManufacturer());
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            ChangeStack stack = new ChangeStack(bid);

            typical.Controllers.Add(controller);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.Add, controller, instance.Controllers[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.Controllers[0]));
            expectedItems.Add(new StackItem(Change.Add, typical, controller));

            int expectedCount = 3;



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
            expectedItems.Add(new StackItem(Change.Add, controller, instance.Controllers[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.Controllers[0]));
            expectedItems.Add(new StackItem(Change.Add, typical, instance));

            int expectedCount = 3;

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
            TECPanel panel = new TECPanel(new TECPanelType());
            bid.Systems.Add(system);

            //Act
            ChangeStack stack = new ChangeStack(bid);
            StackItem expectedItem = new StackItem(Change.Add, system, panel);
            int expectedCount = 1;

            system.Panels.Add(panel);

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
        }

        [TestMethod]
        public void Bid_AddPanelToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem typical = new TECSystem();
            TECPanel panel = new TECPanel(new TECPanelType());
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            ChangeStack stack = new ChangeStack(bid);

            typical.Panels.Add(panel);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.Add, panel, instance.Panels[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.Panels[0]));
            expectedItems.Add(new StackItem(Change.Add, typical, panel));

            int expectedCount = 3;


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
            TECPanel panel = new TECPanel(new TECPanelType());
            bid.Systems.Add(typical);
            typical.Panels.Add(panel);

            //Act
            ChangeStack stack = new ChangeStack(bid);

            TECSystem instance = typical.AddInstance(bid);

            List<StackItem> expectedItems = new List<StackItem>();
            expectedItems.Add(new StackItem(Change.Add, panel, instance.Panels[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.Panels[0]));
            expectedItems.Add(new StackItem(Change.Add, typical, instance));

            int expectedCount = 3;

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
            expectedItems.Add(new StackItem(Change.Add, misc, instance.MiscCosts[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.MiscCosts[0]));
            expectedItems.Add(new StackItem(Change.Add, typical, misc));

            int expectedCount = 3;


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
            expectedItems.Add(new StackItem(Change.Add, misc, instance.MiscCosts[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new StackItem(Change.Add, instance, instance.MiscCosts[0]));
            expectedItems.Add(new StackItem(Change.Add, typical, instance));

            int expectedCount = 3;

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
