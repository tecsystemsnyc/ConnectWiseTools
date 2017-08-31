using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using System.Collections.ObjectModel;
using EstimatingLibrary.Utilities;
using EstimatingUtilitiesLibrary.Database;

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
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECControllerType type = new TECControllerType(new TECManufacturer());
            TECController controller = new TECController(type);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[ControllerControllerTypeTable.ControllerID.Name] = controller.Guid.ToString();
            data[ControllerControllerTypeTable.TypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ControllerControllerTypeTable.TableName, data));

            data = new Dictionary<string, string>();
            data[ControllerTable.ID.Name] = controller.Guid.ToString();
            data[ControllerTable.Name.Name] = controller.Name;
            data[ControllerTable.Description.Name] = controller.Name;
            data[ControllerTable.Type.Name] = controller.Type.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ControllerTable.TableName, data));

            int expectedCount = expectedItems.Count;

            bid.Controllers.Add(controller);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Panel
        [TestMethod]
        public void Bid_AddPanel()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECPanelType type = new TECPanelType(new TECManufacturer());
            TECPanel panel = new TECPanel(type);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[PanelPanelTypeTable.PanelID.Name] = panel.Guid.ToString();
            data[PanelPanelTypeTable.PanelTypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PanelPanelTypeTable.TableName, data));

            data = new Dictionary<string, string>();
            data[PanelTable.ID.Name] = panel.Guid.ToString();
            data[PanelTable.Name.Name] = panel.Name;
            data[PanelTable.Description.Name] = panel.Name;
            expectedItems.Add(new UpdateItem(Change.Add, PanelTable.TableName, data));

            int expectedCount = expectedItems.Count;
            
            bid.Panels.Add(panel);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Misc
        [TestMethod]
        public void Bid_AddMisc()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECMisc misc = new TECMisc(CostType.TEC);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            List<UpdateItem> expectedItems = new List<UpdateItem>();
            
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[MiscTable.ID.Name] = misc.Guid.ToString();
            data[MiscTable.Name.Name] = misc.Name.ToString();
            data[MiscTable.Cost.Name] = misc.Cost.ToString();
            data[MiscTable.Labor.Name] = misc.Labor.ToString();
            data[MiscTable.Type.Name] = misc.Type.ToString();
            data[MiscTable.Quantity.Name] = misc.Quantity.ToString();

            expectedItems.Add(new UpdateItem(Change.Add, MiscTable.TableName,data));

            data = new Dictionary<string, string>();
            data[BidMiscTable.BidID.Name] = bid.Guid.ToString();
            data[BidMiscTable.MiscID.Name] = misc.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, BidMiscTable.TableName, data));

            int expectedCount = expectedItems.Count;


            bid.MiscCosts.Add(misc);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);

        }
        #endregion
        #region Scope Branch
        [TestMethod]
        public void Bid_AddScopeBranch()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECScopeBranch scopeBranch = new TECScopeBranch();

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[ScopeBranchTable.ID.Name] = scopeBranch.Guid.ToString();
            data[ScopeBranchTable.Label.Name] = scopeBranch.Label.ToString();

            UpdateItem expectedItem = new UpdateItem(Change.Add, ScopeBranchTable.TableName, data);
            int expectedCount = 1;

            bid.ScopeTree.Add(scopeBranch);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
        }
        #endregion
        #endregion

        #region System
        [TestMethod]
        public void Bid_AddSystem()
        {
            //Arrange
            TECBid bid = new TECBid();
            ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = system.Guid.ToString();
            data[SystemTable.Name.Name] = system.Name.ToString();
            data[SystemTable.Description.Name] = system.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = system.ProposeEquipment.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemTable.TableName, data));

            data = new Dictionary<string, string>();
            data[BidSystemTable.BidID.Name] = bid.Guid.ToString();
            data[BidSystemTable.SystemID.Name] = system.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, BidSystemTable.TableName, data));

            int expectedCount = expectedItems.Count;
            
            bid.Systems.Add(system);
            
            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddSystemInstance()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            TECSystem instance = system.AddInstance(bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            data[SystemTable.Name.Name] = instance.Name.ToString();
            data[SystemTable.Description.Name] = instance.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = instance.ProposeEquipment.ToString();

            expectedItems.Add( new UpdateItem(Change.Add, SystemTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = system.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();

            expectedItems.Add(new UpdateItem(Change.Add, SystemHierarchyTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #region Equipment
        [TestMethod]
        public void Bid_AddEquipmentToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            TECEquipment equip = new TECEquipment();
            bid.Systems.Add(system);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = equip.Guid.ToString();
            data[EquipmentTable.Name.Name] = equip.Name.ToString();
            data[EquipmentTable.Description.Name] = equip.Description.ToString();

            expectedItems.Add(new UpdateItem(Change.Add, EquipmentTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = system.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = equip.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemEquipmentTable.TableName, data));

            int expectedCount = expectedItems.Count;

            system.Equipment.Add(equip);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
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
            ChangeWatcher watcher = new ChangeWatcher(bid);
            DeltaStacker stack = new DeltaStacker(watcher);

            typical.Equipment.Add(equip);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = equip.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentTable.Name.Name] = instance.Equipment[0].Name.ToString();
            data[EquipmentTable.Description.Name] = instance.Equipment[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, EquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemEquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = equip.Guid.ToString();
            data[EquipmentTable.Name.Name] = equip.Name.ToString();
            data[EquipmentTable.Description.Name] = equip.Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, EquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = typical.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = equip.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemEquipmentTable.TableName, data));
            
            int expectedCount = expectedItems.Count;
            
            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToTypicalWithEquipment()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECEquipment equip = new TECEquipment();
            bid.Systems.Add(typical);
            typical.Equipment.Add(equip);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            TECSystem instance = typical.AddInstance(bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = equip.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));

            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentTable.Name.Name] = instance.Equipment[0].Name.ToString();
            data[EquipmentTable.Description.Name] = instance.Equipment[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, EquipmentTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemEquipmentTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            data[SystemTable.Name.Name] = instance.Name.ToString();
            data[SystemTable.Description.Name] = instance.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = instance.ProposeEquipment.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = typical.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemHierarchyTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region SubScope
        [TestMethod]
        public void Bid_AddSubScopeToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            List<UpdateItem> expectedItems = new List<UpdateItem>();
            
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);

            Dictionary<string, string>  data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = subScope.Guid.ToString();
            data[SubScopeTable.Name.Name] = subScope.Name.ToString();
            data[SubScopeTable.Description.Name] = subScope.Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeTable.TableName, data));

            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = equipment.Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = subScope.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, EquipmentSubScopeTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddSubScopeToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSystem instance = system.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = subScope.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeTable.Name.Name] = instance.Equipment[0].SubScope[0].Name.ToString();
            data[SubScopeTable.Description.Name] = instance.Equipment[0].SubScope[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, EquipmentSubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = subScope.Guid.ToString();
            data[SubScopeTable.Name.Name] = subScope.Name.ToString();
            data[SubScopeTable.Description.Name] = subScope.Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = system.Equipment[0].Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = subScope.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, EquipmentSubScopeTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToSystemWithSubScope()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            TECSystem instance = system.AddInstance(bid);
            
            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = subScope.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = equipment.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeTable.Name.Name] = instance.Equipment[0].SubScope[0].Name.ToString();
            data[SubScopeTable.Description.Name] = instance.Equipment[0].SubScope[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, EquipmentSubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentTable.Name.Name] = instance.Equipment[0].Name.ToString();
            data[EquipmentTable.Description.Name] = instance.Equipment[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, EquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemEquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            data[SystemTable.Name.Name] = instance.Name.ToString();
            data[SystemTable.Description.Name] = instance.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = instance.ProposeEquipment.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = system.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemHierarchyTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Point
        [TestMethod]
        public void Bid_AddPointToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            TECPoint point = new TECPoint();
            subScope.Points.Add(point);

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[PointTable.ID.Name] = point.Guid.ToString();
            data[PointTable.Name.Name] = point.Label.ToString();
            data[PointTable.Type.Name] = point.Type.ToString();
            data[PointTable.Quantity.Name] = point.Type.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PointTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SubScopePointTable.SubScopeID.Name] = subScope.Guid.ToString();
            data[SubScopePointTable.PointID.Name] = point.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopePointTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
            
        }

        [TestMethod]
        public void Bid_AddPointToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECSystem instance = system.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            TECPoint point = new TECPoint();
            subScope.Points.Add(point);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = point.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].SubScope[0].Points[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PointTable.ID.Name] = instance.Equipment[0].SubScope[0].Points[0].Guid.ToString();
            data[PointTable.Name.Name] = instance.Equipment[0].SubScope[0].Points[0].Label.ToString();
            data[PointTable.Type.Name] = instance.Equipment[0].SubScope[0].Points[0].Type.ToString();
            data[PointTable.Quantity.Name] = instance.Equipment[0].SubScope[0].Points[0].Type.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PointTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopePointTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopePointTable.PointID.Name] = instance.Equipment[0].SubScope[0].Points[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopePointTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PointTable.ID.Name] = point.Guid.ToString();
            data[PointTable.Name.Name] = point.Label.ToString();
            data[PointTable.Type.Name] = point.Type.ToString();
            data[PointTable.Quantity.Name] = point.Type.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PointTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopePointTable.SubScopeID.Name] = system.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopePointTable.PointID.Name] = point.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopePointTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
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
            ChangeWatcher watcher = new ChangeWatcher(bid);
            DeltaStacker stack = new DeltaStacker(watcher);
            TECSystem instance = system.AddInstance(bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = point.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].SubScope[0].Points[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = subScope.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = equipment.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PointTable.ID.Name] = instance.Equipment[0].SubScope[0].Points[0].Guid.ToString();
            data[PointTable.Name.Name] = instance.Equipment[0].SubScope[0].Points[0].Label.ToString();
            data[PointTable.Type.Name] = instance.Equipment[0].SubScope[0].Points[0].Type.ToString();
            data[PointTable.Quantity.Name] = instance.Equipment[0].SubScope[0].Points[0].Type.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PointTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopePointTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopePointTable.PointID.Name] = instance.Equipment[0].SubScope[0].Points[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopePointTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeTable.Name.Name] = instance.Equipment[0].SubScope[0].Name.ToString();
            data[SubScopeTable.Description.Name] = instance.Equipment[0].SubScope[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, EquipmentSubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentTable.Name.Name] = instance.Equipment[0].Name.ToString();
            data[EquipmentTable.Description.Name] = instance.Equipment[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, EquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemEquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            data[SystemTable.Name.Name] = instance.Name.ToString();
            data[SystemTable.Description.Name] = instance.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = instance.ProposeEquipment.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = system.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemHierarchyTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Device
        [TestMethod]
        public void Bid_AddDeviceToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            TECDevice device = new TECDevice(new ObservableCollection<TECElectricalMaterial>(), new TECManufacturer());
            subScope.Devices.Add(device);

            Dictionary < string, string> data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = subScope.Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = device.Guid.ToString();
            UpdateItem expectedItem = new UpdateItem(Change.Add, SubScopeDeviceTable.TableName, data);
            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
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
            ChangeWatcher watcher = new ChangeWatcher(bid);
            DeltaStacker stack = new DeltaStacker(watcher);
            TECDevice device = new TECDevice(new ObservableCollection<TECElectricalMaterial>(), new TECManufacturer());
            subScope.Devices.Add(device);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string>  data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = device.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeDeviceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = system.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = device.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeDeviceTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToSystemWithDevice()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECDevice device = new TECDevice(new ObservableCollection<TECElectricalMaterial>(), new TECManufacturer());
            bid.Catalogs.Devices.Add(device);
            subScope.Devices.Add(device);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            TECSystem instance = system.AddInstance(bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = subScope.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = equipment.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = device.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeDeviceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeTable.Name.Name] = instance.Equipment[0].SubScope[0].Name.ToString();
            data[SubScopeTable.Description.Name] = instance.Equipment[0].SubScope[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, EquipmentSubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentTable.Name.Name] = instance.Equipment[0].Name.ToString();
            data[EquipmentTable.Description.Name] = instance.Equipment[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, EquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemEquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            data[SystemTable.Name.Name] = instance.Name.ToString();
            data[SystemTable.Description.Name] = instance.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = instance.ProposeEquipment.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = system.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemHierarchyTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Controller
        [TestMethod]
        public void Bid_AddControllerToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            TECManufacturer manufacturer = new TECManufacturer();
            TECControllerType type = new TECControllerType(manufacturer);
            TECController controller = new TECController(type);
            bid.Systems.Add(system);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[ControllerControllerTypeTable.ControllerID.Name] = controller.Guid.ToString();
            data[ControllerControllerTypeTable.TypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ControllerControllerTypeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[ControllerTable.ID.Name] = controller.Guid.ToString();
            data[ControllerTable.Name.Name] = controller.Name.ToString();
            data[ControllerTable.Description.Name] = controller.Description.ToString();
            data[ControllerTable.Type.Name] = controller.Type.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ControllerTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemControllerTable.SystemID.Name] = system.Guid.ToString();
            data[SystemControllerTable.ControllerID.Name] = controller.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemControllerTable.TableName, data));

            int expectedCount = expectedItems.Count;

            system.Controllers.Add(controller);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddControllerToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); 
            TECSystem typical = new TECSystem();
            TECManufacturer manufacturer = new TECManufacturer();
            TECControllerType type = new TECControllerType(manufacturer);
            TECController controller = new TECController(type);
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            ChangeWatcher watcher = new ChangeWatcher(bid);
            DeltaStacker stack = new DeltaStacker(watcher);

            typical.Controllers.Add(controller);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = controller.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Controllers[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[ControllerControllerTypeTable.ControllerID.Name] = instance.Controllers[0].Guid.ToString();
            data[ControllerControllerTypeTable.TypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ControllerControllerTypeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[ControllerTable.ID.Name] = instance.Controllers[0].Guid.ToString();
            data[ControllerTable.Name.Name] = instance.Controllers[0].Name.ToString();
            data[ControllerTable.Description.Name] = instance.Controllers[0].Description.ToString();
            data[ControllerTable.Type.Name] = instance.Controllers[0].Type.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ControllerTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemControllerTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemControllerTable.ControllerID.Name] = instance.Controllers[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemControllerTable.TableName, data));
            data = new Dictionary<string, string>();
            data[ControllerControllerTypeTable.ControllerID.Name] = controller.Guid.ToString();
            data[ControllerControllerTypeTable.TypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ControllerControllerTypeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[ControllerTable.ID.Name] = controller.Guid.ToString();
            data[ControllerTable.Name.Name] = controller.Name.ToString();
            data[ControllerTable.Description.Name] = controller.Description.ToString();
            data[ControllerTable.Type.Name] = controller.Type.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ControllerTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemControllerTable.SystemID.Name] = typical.Guid.ToString();
            data[SystemControllerTable.ControllerID.Name] = controller.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemControllerTable.TableName, data));
            data = new Dictionary<string, string>();

            int expectedCount = expectedItems.Count;

            
            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToTypicalWithController()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECManufacturer manufacturer = new TECManufacturer();
            TECControllerType type = new TECControllerType(manufacturer);

            TECController controller = new TECController(type);
            bid.Systems.Add(typical);
            typical.Controllers.Add(controller);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            TECSystem instance = typical.AddInstance(bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = controller.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Controllers[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[ControllerControllerTypeTable.ControllerID.Name] = instance.Controllers[0].Guid.ToString();
            data[ControllerControllerTypeTable.TypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ControllerControllerTypeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[ControllerTable.ID.Name] = instance.Controllers[0].Guid.ToString();
            data[ControllerTable.Name.Name] = instance.Controllers[0].Name.ToString();
            data[ControllerTable.Description.Name] = instance.Controllers[0].Description.ToString();
            data[ControllerTable.Type.Name] = instance.Controllers[0].Type.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ControllerTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemControllerTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemControllerTable.ControllerID.Name] = instance.Controllers[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemControllerTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            data[SystemTable.Name.Name] = instance.Name.ToString();
            data[SystemTable.Description.Name] = instance.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = instance.ProposeEquipment.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = typical.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemHierarchyTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Panel
        [TestMethod]
        public void Bid_AddPanelToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            TECPanelType type = new TECPanelType(new TECManufacturer());
            TECPanel panel = new TECPanel(type);
            bid.Systems.Add(system);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[PanelPanelTypeTable.PanelID.Name] = panel.Guid.ToString();
            data[PanelPanelTypeTable.PanelTypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PanelPanelTypeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PanelTable.ID.Name] = panel.Guid.ToString();
            data[PanelTable.Name.Name] = panel.Name.ToString();
            data[PanelTable.Description.Name] = panel.Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PanelTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemPanelTable.SystemID.Name] = system.Guid.ToString();
            data[SystemPanelTable.PanelID.Name] = panel.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemPanelTable.TableName, data));

            int expectedCount = expectedItems.Count;

            system.Panels.Add(panel);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddPanelToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECPanelType type = new TECPanelType(new TECManufacturer());
            TECPanel panel = new TECPanel(type);
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            typical.Panels.Add(panel);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = panel.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Panels[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PanelPanelTypeTable.PanelID.Name] = instance.Panels[0].Guid.ToString();
            data[PanelPanelTypeTable.PanelTypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PanelPanelTypeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PanelTable.ID.Name] = instance.Panels[0].Guid.ToString();
            data[PanelTable.Name.Name] = instance.Panels[0].Name.ToString();
            data[PanelTable.Description.Name] = instance.Panels[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PanelTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemPanelTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemPanelTable.PanelID.Name] = instance.Panels[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemPanelTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PanelPanelTypeTable.PanelID.Name] = panel.Guid.ToString();
            data[PanelPanelTypeTable.PanelTypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PanelPanelTypeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PanelTable.ID.Name] = panel.Guid.ToString();
            data[PanelTable.Name.Name] = panel.Name.ToString();
            data[PanelTable.Description.Name] = panel.Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PanelTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemPanelTable.SystemID.Name] = typical.Guid.ToString();
            data[SystemPanelTable.PanelID.Name] = panel.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemPanelTable.TableName, data));
            int expectedCount = expectedItems.Count;


            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToTypicalWithPanel()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECPanelType type = new TECPanelType(new TECManufacturer());
            TECPanel panel = new TECPanel(type);
            bid.Systems.Add(typical);
            typical.Panels.Add(panel);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            TECSystem instance = typical.AddInstance(bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = panel.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Panels[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PanelPanelTypeTable.PanelID.Name] = instance.Panels[0].Guid.ToString();
            data[PanelPanelTypeTable.PanelTypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PanelPanelTypeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PanelTable.ID.Name] = instance.Panels[0].Guid.ToString();
            data[PanelTable.Name.Name] = instance.Panels[0].Name.ToString();
            data[PanelTable.Description.Name] = instance.Panels[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PanelTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemPanelTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemPanelTable.PanelID.Name] = instance.Panels[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemPanelTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            data[SystemTable.Name.Name] = instance.Name.ToString();
            data[SystemTable.Description.Name] = instance.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = instance.ProposeEquipment.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = typical.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemHierarchyTable.TableName, data));

            int expectedCount = expectedItems.Count;


            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Misc
        [TestMethod]
        public void Bid_AddMiscToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            TECMisc misc = new TECMisc(CostType.TEC);
            bid.Systems.Add(system);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string>  data = new Dictionary<string, string>();
            data[MiscTable.ID.Name] = misc.Guid.ToString();
            data[MiscTable.Name.Name] = misc.Name.ToString();
            data[MiscTable.Cost.Name] = misc.Cost.ToString();
            data[MiscTable.Labor.Name] = misc.Labor.ToString();
            data[MiscTable.Quantity.Name] = misc.Quantity.ToString();
            data[MiscTable.Type.Name] = misc.Type.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, MiscTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemMiscTable.SystemID.Name] = system.Guid.ToString();
            data[SystemMiscTable.MiscID.Name] = misc.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemMiscTable.TableName, data));

            int expectedCount = expectedItems.Count;

            system.MiscCosts.Add(misc);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddMiscToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECMisc misc = new TECMisc(CostType.TEC);
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            typical.MiscCosts.Add(misc);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[MiscTable.ID.Name] = misc.Guid.ToString();
            data[MiscTable.Name.Name] = misc.Name.ToString();
            data[MiscTable.Cost.Name] = misc.Cost.ToString();
            data[MiscTable.Labor.Name] = misc.Labor.ToString();
            data[MiscTable.Quantity.Name] = misc.Quantity.ToString();
            data[MiscTable.Type.Name] = misc.Type.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, MiscTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemMiscTable.SystemID.Name] = typical.Guid.ToString();
            data[SystemMiscTable.MiscID.Name] = misc.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemMiscTable.TableName, data));


            int expectedCount = expectedItems.Count;


            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToTypicalWithMisc()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECMisc misc = new TECMisc(CostType.TEC);
            bid.Systems.Add(typical);
            typical.MiscCosts.Add(misc);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            TECSystem instance = typical.AddInstance(bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string>  data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            data[SystemTable.Name.Name] = instance.Name.ToString();
            data[SystemTable.Description.Name] = instance.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = instance.ProposeEquipment.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = typical.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemHierarchyTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Scope Branch
        [TestMethod]
        public void Bid_AddScopeBranchToTypical()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            TECScopeBranch scopeBranch = new TECScopeBranch();
            bid.Systems.Add(system);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[ScopeBranchTable.ID.Name] = scopeBranch.Guid.ToString();
            data[ScopeBranchTable.Label.Name] = scopeBranch.Label.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ScopeBranchTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemScopeBranchTable.SystemID.Name] = system.Guid.ToString();
            data[SystemScopeBranchTable.BranchID.Name] = scopeBranch.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemScopeBranchTable.TableName, data));
            int expectedCount = expectedItems.Count;

            system.ScopeBranches.Add(scopeBranch);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
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
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECManufacturer manufacturer = new TECManufacturer();
            TECControllerType type = new TECControllerType(manufacturer);
            TECController controller = new TECController(type);
            bid.Controllers.Add(controller);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[ControllerControllerTypeTable.ControllerID.Name] = controller.Guid.ToString();
            data[ControllerControllerTypeTable.TypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ControllerControllerTypeTable.TableName, data));

            data = new Dictionary<string, string>();
            data[ControllerTable.ID.Name] = controller.Guid.ToString();
            data[ControllerTable.Name.Name] = controller.Name;
            data[ControllerTable.Description.Name] = controller.Name;
            data[ControllerTable.Type.Name] = controller.Type.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ControllerTable.TableName, data));

            int expectedCount = expectedItems.Count;

            bid.Controllers.Remove(controller);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Panel
        [TestMethod]
        public void Bid_RemovePanel()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECPanelType type = new TECPanelType(new TECManufacturer());
            TECPanel panel = new TECPanel(type);
            bid.Panels.Add(panel);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[PanelPanelTypeTable.PanelID.Name] = panel.Guid.ToString();
            data[PanelPanelTypeTable.PanelTypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PanelPanelTypeTable.TableName, data));

            data = new Dictionary<string, string>();
            data[PanelTable.ID.Name] = panel.Guid.ToString();
            data[PanelTable.Name.Name] = panel.Name;
            data[PanelTable.Description.Name] = panel.Name;
            expectedItems.Add(new UpdateItem(Change.Remove, PanelTable.TableName, data));

            int expectedCount = expectedItems.Count;

            bid.Panels.Remove(panel);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Misc
        [TestMethod]
        public void Bid_RemoveMisc()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECMisc misc = new TECMisc(CostType.TEC);
            bid.MiscCosts.Add(misc);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[MiscTable.ID.Name] = misc.Guid.ToString();
            data[MiscTable.Name.Name] = misc.Name.ToString();
            data[MiscTable.Cost.Name] = misc.Cost.ToString();
            data[MiscTable.Labor.Name] = misc.Labor.ToString();
            data[MiscTable.Type.Name] = misc.Type.ToString();
            data[MiscTable.Quantity.Name] = misc.Quantity.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, MiscTable.TableName, data));

            data = new Dictionary<string, string>();
            data[BidMiscTable.BidID.Name] = bid.Guid.ToString();
            data[BidMiscTable.MiscID.Name] = misc.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, BidMiscTable.TableName, data));

            int expectedCount = expectedItems.Count;

            bid.MiscCosts.Remove(misc);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Scope Branch
        [TestMethod]
        public void Bid_RemoveScopeBranch()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECScopeBranch scopeBranch = new TECScopeBranch();
            bid.ScopeTree.Add(scopeBranch);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[ScopeBranchTable.ID.Name] = scopeBranch.Guid.ToString();
            data[ScopeBranchTable.Label.Name] = scopeBranch.Label.ToString();

            UpdateItem expectedItem = new UpdateItem(Change.Remove, ScopeBranchTable.TableName, data);
            int expectedCount = 1;

            bid.ScopeTree.Remove(scopeBranch);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
        }
        #endregion
        #endregion

        #region System
        [TestMethod]
        public void Bid_RemoveSystem()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            Dictionary<string, string> data = new Dictionary<string, string>();
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            data[SystemTable.ID.Name] = system.Guid.ToString();
            data[SystemTable.Name.Name] = system.Name.ToString();
            data[SystemTable.Description.Name] = system.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = system.ProposeEquipment.ToString();

            expectedItems.Add(new UpdateItem(Change.Remove, SystemTable.TableName, data));


            data = new Dictionary<string, string>();
            data[BidSystemTable.BidID.Name] = bid.Guid.ToString();
            data[BidSystemTable.SystemID.Name] = system.Guid.ToString();

            expectedItems.Add(new UpdateItem(Change.Remove, BidSystemTable.TableName, data));

            int expectedCount = expectedItems.Count;

            bid.Systems.Remove(system);


            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveSystemInstance()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECSystem instance = system.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            system.Instances.Remove(instance);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            data[SystemTable.Name.Name] = instance.Name.ToString();
            data[SystemTable.Description.Name] = instance.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = instance.ProposeEquipment.ToString();

            expectedItems.Add(new UpdateItem(Change.Remove, SystemTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = system.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();

            expectedItems.Add(new UpdateItem(Change.Remove, SystemHierarchyTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #region Equipment
        [TestMethod]
        public void Bid_RemoveEquipmentToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            TECEquipment equip = new TECEquipment();
            bid.Systems.Add(system);
            system.Equipment.Add(equip);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = equip.Guid.ToString();
            data[EquipmentTable.Name.Name] = equip.Name.ToString();
            data[EquipmentTable.Description.Name] = equip.Description.ToString();

            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = system.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = equip.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemEquipmentTable.TableName, data));

            int expectedCount = 2;

            system.Equipment.Remove(equip);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveEquipmentFromTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECEquipment equip = new TECEquipment();
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            typical.Equipment.Add(equip);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            var removed = instance.Equipment[0];
            typical.Equipment.Remove(equip);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string>
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = equip.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = removed.Guid.ToString();
            data[EquipmentTable.Name.Name] = removed.Name.ToString();
            data[EquipmentTable.Description.Name] = removed.Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemEquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = equip.Guid.ToString();
            data[EquipmentTable.Name.Name] = equip.Name.ToString();
            data[EquipmentTable.Description.Name] = equip.Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = typical.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = equip.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemEquipmentTable.TableName, data));
            
            int expectedCount = expectedItems.Count;
            

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceFromTypicalWithEquipment()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECEquipment equip = new TECEquipment();
            bid.Systems.Add(typical);
            typical.Equipment.Add(equip);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            typical.Instances.Remove(instance);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentTable.Name.Name] = instance.Equipment[0].Name.ToString();
            data[EquipmentTable.Description.Name] = instance.Equipment[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemEquipmentTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            data[SystemTable.Name.Name] = instance.Name.ToString();
            data[SystemTable.Description.Name] = instance.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = instance.ProposeEquipment.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = typical.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemHierarchyTable.TableName, data));

            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = equip.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region SubScope
        [TestMethod]
        public void Bid_RemoveSubScopeToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            equipment.SubScope.Remove(subScope);
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = subScope.Guid.ToString();
            data[SubScopeTable.Name.Name] = subScope.Name.ToString();
            data[SubScopeTable.Description.Name] = subScope.Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeTable.TableName, data));

            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = equipment.Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = subScope.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentSubScopeTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveSubScopeToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSystem instance = system.AddInstance(bid);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            var removed = instance.Equipment[0].SubScope[0];
            equipment.SubScope.Remove(subScope);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = subScope.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = removed.Guid.ToString();
            data[SubScopeTable.Name.Name] = removed.Name.ToString();
            data[SubScopeTable.Description.Name] = removed.Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentSubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = subScope.Guid.ToString();
            data[SubScopeTable.Name.Name] = subScope.Name.ToString();
            data[SubScopeTable.Description.Name] = subScope.Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = system.Equipment[0].Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = subScope.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentSubScopeTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceFromSystemWithSubScope()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECSystem instance = system.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            system.Instances.Remove(instance);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = subScope.Guid.ToString();
            data[SubScopeTable.Name.Name] = subScope.Name.ToString();
            data[SubScopeTable.Description.Name] = subScope.Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentSubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentTable.Name.Name] = instance.Equipment[0].Name.ToString();
            data[EquipmentTable.Description.Name] = instance.Equipment[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemEquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            data[SystemTable.Name.Name] = instance.Name.ToString();
            data[SystemTable.Description.Name] = instance.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = instance.ProposeEquipment.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = system.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemHierarchyTable.TableName, data));
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = subScope.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = equipment.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Point
        [TestMethod]
        public void Bid_RemovePointToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECPoint point = new TECPoint();
            subScope.Points.Add(point);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            subScope.Points.Remove(point);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[PointTable.ID.Name] = point.Guid.ToString();
            data[PointTable.Name.Name] = point.Label.ToString();
            data[PointTable.Type.Name] = point.Type.ToString();
            data[PointTable.Quantity.Name] = point.Type.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PointTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SubScopePointTable.SubScopeID.Name] = subScope.Guid.ToString();
            data[SubScopePointTable.PointID.Name] = point.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopePointTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemovePointToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
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
            DeltaStacker stack = new DeltaStacker(watcher);
            var removed = instance.Equipment[0].SubScope[0].Points[0];
            subScope.Points.Remove(point);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = point.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PointTable.ID.Name] = removed.Guid.ToString();
            data[PointTable.Name.Name] = removed.Label.ToString();
            data[PointTable.Type.Name] = removed.Type.ToString();
            data[PointTable.Quantity.Name] = removed.Type.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PointTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopePointTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopePointTable.PointID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopePointTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PointTable.ID.Name] = point.Guid.ToString();
            data[PointTable.Name.Name] = point.Label.ToString();
            data[PointTable.Type.Name] = point.Type.ToString();
            data[PointTable.Quantity.Name] = point.Type.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PointTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopePointTable.SubScopeID.Name] = system.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopePointTable.PointID.Name] = point.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopePointTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceFromSystemWithPoint()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
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
            DeltaStacker stack = new DeltaStacker(watcher);
            system.Instances.Remove(instance);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[PointTable.ID.Name] = instance.Equipment[0].SubScope[0].Points[0].Guid.ToString();
            data[PointTable.Name.Name] = instance.Equipment[0].SubScope[0].Points[0].Label.ToString();
            data[PointTable.Type.Name] = instance.Equipment[0].SubScope[0].Points[0].Type.ToString();
            data[PointTable.Quantity.Name] = instance.Equipment[0].SubScope[0].Points[0].Type.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PointTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopePointTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopePointTable.PointID.Name] = instance.Equipment[0].SubScope[0].Points[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopePointTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeTable.Name.Name] = instance.Equipment[0].SubScope[0].Name.ToString();
            data[SubScopeTable.Description.Name] = instance.Equipment[0].SubScope[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentSubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentTable.Name.Name] = instance.Equipment[0].Name.ToString();
            data[EquipmentTable.Description.Name] = instance.Equipment[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemEquipmentTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            data[SystemTable.Name.Name] = instance.Name.ToString();
            data[SystemTable.Description.Name] = instance.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = instance.ProposeEquipment.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = system.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemHierarchyTable.TableName, data));

            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = point.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].SubScope[0].Points[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = subScope.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = equipment.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Device
        [TestMethod]
        public void Bid_RemoveDeviceToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECDevice device = new TECDevice(new ObservableCollection<TECElectricalMaterial>(), new TECManufacturer());
            subScope.Devices.Add(device);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            subScope.Devices.Remove(device);

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = subScope.Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = device.Guid.ToString();
            UpdateItem expectedItem = new UpdateItem(Change.Remove, SubScopeDeviceTable.TableName, data);
            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
        }

        [TestMethod]
        public void Bid_RemoveDeviceToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECSystem instance = system.AddInstance(bid);
            TECDevice device = new TECDevice(new ObservableCollection<TECElectricalMaterial>(), new TECManufacturer());
            bid.Catalogs.Devices.Add(device);
            subScope.Devices.Add(device);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            subScope.Devices.Remove(device);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = device.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeDeviceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = system.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = device.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeDeviceTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceFromSystemWithDevice()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECDevice device = new TECDevice(new ObservableCollection<TECElectricalMaterial>(), new TECManufacturer());
            bid.Catalogs.Devices.Add(device);
            subScope.Devices.Add(device);
            TECSystem instance = system.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            system.Instances.Remove(instance);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string>
                data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = device.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeDeviceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeTable.Name.Name] = instance.Equipment[0].SubScope[0].Name.ToString();
            data[SubScopeTable.Description.Name] = instance.Equipment[0].SubScope[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentSubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentTable.Name.Name] = instance.Equipment[0].Name.ToString();
            data[EquipmentTable.Description.Name] = instance.Equipment[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemEquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            data[SystemTable.Name.Name] = instance.Name.ToString();
            data[SystemTable.Description.Name] = instance.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = instance.ProposeEquipment.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = system.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemHierarchyTable.TableName, data));
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = subScope.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = equipment.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Controller
        [TestMethod]
        public void Bid_RemoveControllerToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            TECManufacturer manufacturer = new TECManufacturer();
            TECControllerType type = new TECControllerType(manufacturer);
            TECController controller = new TECController(type);
            bid.Systems.Add(system);
            system.Controllers.Add(controller);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[ControllerControllerTypeTable.ControllerID.Name] = controller.Guid.ToString();
            data[ControllerControllerTypeTable.TypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ControllerControllerTypeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[ControllerTable.ID.Name] = controller.Guid.ToString();
            data[ControllerTable.Name.Name] = controller.Name.ToString();
            data[ControllerTable.Description.Name] = controller.Description.ToString();
            data[ControllerTable.Type.Name] = controller.Type.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ControllerTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemControllerTable.SystemID.Name] = system.Guid.ToString();
            data[SystemControllerTable.ControllerID.Name] = controller.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemControllerTable.TableName, data));

            int expectedCount = expectedItems.Count;

            system.Controllers.Remove(controller);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveControllerToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECManufacturer manufacturer = new TECManufacturer();
            TECControllerType type = new TECControllerType(manufacturer);
            TECController controller = new TECController(type);
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            typical.Controllers.Add(controller);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            var removed = instance.Controllers[0];
            typical.Controllers.Remove(controller);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = controller.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[ControllerControllerTypeTable.ControllerID.Name] = removed.Guid.ToString();
            data[ControllerControllerTypeTable.TypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ControllerControllerTypeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[ControllerTable.ID.Name] = removed.Guid.ToString();
            data[ControllerTable.Name.Name] = removed.Name.ToString();
            data[ControllerTable.Description.Name] = removed.Description.ToString();
            data[ControllerTable.Type.Name] = removed.Type.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ControllerTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemControllerTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemControllerTable.ControllerID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemControllerTable.TableName, data));
            data = new Dictionary<string, string>();
            data[ControllerControllerTypeTable.ControllerID.Name] = controller.Guid.ToString();
            data[ControllerControllerTypeTable.TypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ControllerControllerTypeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[ControllerTable.ID.Name] = controller.Guid.ToString();
            data[ControllerTable.Name.Name] = controller.Name.ToString();
            data[ControllerTable.Description.Name] = controller.Description.ToString();
            data[ControllerTable.Type.Name] = controller.Type.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ControllerTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemControllerTable.SystemID.Name] = typical.Guid.ToString();
            data[SystemControllerTable.ControllerID.Name] = controller.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemControllerTable.TableName, data));
            data = new Dictionary<string, string>();

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceFromTypicalWithController()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECManufacturer manufacturer = new TECManufacturer();
            TECControllerType type = new TECControllerType(manufacturer);
            TECController controller = new TECController(type);
            bid.Systems.Add(typical);
            typical.Controllers.Add(controller);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            typical.Instances.Remove(instance);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[ControllerControllerTypeTable.ControllerID.Name] = instance.Controllers[0].Guid.ToString();
            data[ControllerControllerTypeTable.TypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ControllerControllerTypeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[ControllerTable.ID.Name] = instance.Controllers[0].Guid.ToString();
            data[ControllerTable.Name.Name] = instance.Controllers[0].Name.ToString();
            data[ControllerTable.Description.Name] = instance.Controllers[0].Description.ToString();
            data[ControllerTable.Type.Name] = instance.Controllers[0].Type.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ControllerTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemControllerTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemControllerTable.ControllerID.Name] = instance.Controllers[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemControllerTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            data[SystemTable.Name.Name] = instance.Name.ToString();
            data[SystemTable.Description.Name] = instance.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = instance.ProposeEquipment.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = typical.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemHierarchyTable.TableName, data));
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = controller.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Controllers[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Panel
        [TestMethod]
        public void Bid_RemovePanelToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            TECPanelType type = new TECPanelType(new TECManufacturer());
            TECPanel panel = new TECPanel(type);
            bid.Systems.Add(system);
            system.Panels.Add(panel);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[PanelPanelTypeTable.PanelID.Name] = panel.Guid.ToString();
            data[PanelPanelTypeTable.PanelTypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PanelPanelTypeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PanelTable.ID.Name] = panel.Guid.ToString();
            data[PanelTable.Name.Name] = panel.Name.ToString();
            data[PanelTable.Description.Name] = panel.Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PanelTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemPanelTable.SystemID.Name] = system.Guid.ToString();
            data[SystemPanelTable.PanelID.Name] = panel.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemPanelTable.TableName, data));

            int expectedCount = expectedItems.Count;

            system.Panels.Remove(panel);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemovePanelFromTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECPanelType type = new TECPanelType(new TECManufacturer());
            TECPanel panel = new TECPanel(type);
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            typical.Panels.Add(panel);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            var removed = instance.Panels[0];
            typical.Panels.Remove(panel);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = panel.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PanelPanelTypeTable.PanelID.Name] = removed.Guid.ToString();
            data[PanelPanelTypeTable.PanelTypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PanelPanelTypeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PanelTable.ID.Name] = removed.Guid.ToString();
            data[PanelTable.Name.Name] = removed.Name.ToString();
            data[PanelTable.Description.Name] = removed.Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PanelTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemPanelTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemPanelTable.PanelID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemPanelTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PanelPanelTypeTable.PanelID.Name] = panel.Guid.ToString();
            data[PanelPanelTypeTable.PanelTypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PanelPanelTypeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PanelTable.ID.Name] = panel.Guid.ToString();
            data[PanelTable.Name.Name] = panel.Name.ToString();
            data[PanelTable.Description.Name] = panel.Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PanelTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemPanelTable.SystemID.Name] = typical.Guid.ToString();
            data[SystemPanelTable.PanelID.Name] = panel.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemPanelTable.TableName, data));
            int expectedCount = expectedItems.Count;


            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceFromTypicalWithPanel()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECPanelType type = new TECPanelType(new TECManufacturer());
            TECPanel panel = new TECPanel(type);
            bid.Systems.Add(typical);
            typical.Panels.Add(panel);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            typical.Instances.Remove(instance);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[PanelPanelTypeTable.PanelID.Name] = instance.Panels[0].Guid.ToString();
            data[PanelPanelTypeTable.PanelTypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PanelPanelTypeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PanelTable.ID.Name] = instance.Panels[0].Guid.ToString();
            data[PanelTable.Name.Name] = instance.Panels[0].Name.ToString();
            data[PanelTable.Description.Name] = instance.Panels[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PanelTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemPanelTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemPanelTable.PanelID.Name] = instance.Panels[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemPanelTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            data[SystemTable.Name.Name] = instance.Name.ToString();
            data[SystemTable.Description.Name] = instance.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = instance.ProposeEquipment.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = typical.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemHierarchyTable.TableName, data));
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = panel.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Panels[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Misc
        [TestMethod]
        public void Bid_RemoveMiscToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            TECMisc misc = new TECMisc(CostType.TEC);
            bid.Systems.Add(system);
            system.MiscCosts.Add(misc);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[MiscTable.ID.Name] = misc.Guid.ToString();
            data[MiscTable.Name.Name] = misc.Name.ToString();
            data[MiscTable.Cost.Name] = misc.Cost.ToString();
            data[MiscTable.Labor.Name] = misc.Labor.ToString();
            data[MiscTable.Quantity.Name] = misc.Quantity.ToString();
            data[MiscTable.Type.Name] = misc.Type.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, MiscTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemMiscTable.SystemID.Name] = system.Guid.ToString();
            data[SystemMiscTable.MiscID.Name] = misc.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemMiscTable.TableName, data));

            int expectedCount = expectedItems.Count;

            system.MiscCosts.Remove(misc);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveMiscToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECMisc misc = new TECMisc(CostType.TEC);
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            typical.MiscCosts.Add(misc);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[MiscTable.ID.Name] = misc.Guid.ToString();
            data[MiscTable.Name.Name] = misc.Name.ToString();
            data[MiscTable.Cost.Name] = misc.Cost.ToString();
            data[MiscTable.Labor.Name] = misc.Labor.ToString();
            data[MiscTable.Quantity.Name] = misc.Quantity.ToString();
            data[MiscTable.Type.Name] = misc.Type.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, MiscTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemMiscTable.SystemID.Name] = typical.Guid.ToString();
            data[SystemMiscTable.MiscID.Name] = misc.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemMiscTable.TableName, data));


            int expectedCount = expectedItems.Count;
            
            typical.MiscCosts.Remove(misc);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceFromTypicalWithMisc()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECMisc misc = new TECMisc(CostType.TEC);
            bid.Systems.Add(typical);
            typical.MiscCosts.Add(misc);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            typical.Instances.Remove(instance);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            data[SystemTable.Name.Name] = instance.Name.ToString();
            data[SystemTable.Description.Name] = instance.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = instance.ProposeEquipment.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = typical.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemHierarchyTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Scope Branch
        [TestMethod]
        public void Bid_RemoveScopeBranchToTypical()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            TECScopeBranch scopeBranch = new TECScopeBranch();
            bid.Systems.Add(system);
            system.ScopeBranches.Add(scopeBranch);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[ScopeBranchTable.ID.Name] = scopeBranch.Guid.ToString();
            data[ScopeBranchTable.Label.Name] = scopeBranch.Label.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ScopeBranchTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemScopeBranchTable.SystemID.Name] = system.Guid.ToString();
            data[SystemScopeBranchTable.BranchID.Name] = scopeBranch.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemScopeBranchTable.TableName, data));
            int expectedCount = expectedItems.Count;

            system.ScopeBranches.Remove(scopeBranch);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #endregion
        #endregion

        public void checkUpdateItem(UpdateItem expectedItem, UpdateItem actualItem)
        {
            Assert.AreEqual(expectedItem.Table, actualItem.Table, "Tables do not match on UpdateItems");
            Assert.AreEqual(expectedItem.FieldData.Count, actualItem.FieldData.Count, "FieldData does not match on UpdateItems");
            Assert.AreEqual(expectedItem.Change, actualItem.Change, "Change does not match on UpdateItems");
        }

        public void checkUpdateItems(List<UpdateItem> expectedItems, DeltaStacker stack)
        {
            int numToCheck = expectedItems.Count;
            foreach(UpdateItem item in expectedItems)
            {
                checkUpdateItem(item, stack.CleansedStack()[stack.CleansedStack().Count - numToCheck]);
                numToCheck--;
            }
        }
    }
}
