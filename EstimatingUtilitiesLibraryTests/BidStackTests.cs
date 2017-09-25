using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using System.Collections.ObjectModel;
using System.Diagnostics;
using EstimatingLibrary.Utilities;
using EstimatingLibrary.Interfaces;

namespace Tests
{
    [TestClass]
    public class BidStackTests
    {

        #region Undo

        #region Bid Properties
        [TestMethod]
        public void Undo_Bid_Name()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string expected = Bid.Name;
            string edit = "Edit";

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid);
            DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            Bid.Name = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            string actual = Bid.Name;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_Number()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string expected = Bid.BidNumber;
            string edit = "Edit";

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            Bid.BidNumber = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            string actual = Bid.BidNumber;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_DueDate()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string expected = Bid.DueDate.ToString();
            DateTime edit = new DateTime(2000, 1, 1);

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            Bid.DueDate = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            string actual = Bid.DueDate.ToString();
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_Salesperson()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string expected = Bid.Salesperson;
            string edit = "Edit";

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            Bid.Salesperson = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            string actual = Bid.Salesperson;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_Estimator()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string expected = Bid.Estimator;
            string edit = "Edit";

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            Bid.Estimator = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            string actual = Bid.Estimator;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_ScopeTree()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECScopeBranch> expected = new ObservableCollection<TECScopeBranch>();
            foreach (TECScopeBranch item in Bid.ScopeTree)
            {
                expected.Add(item);
            }
            TECScopeBranch edit = new TECScopeBranch(false);

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            Bid.ScopeTree.Add(edit);
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            ObservableCollection<TECScopeBranch> actual = Bid.ScopeTree;
            Assert.AreEqual(expected.Count, actual.Count, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_Systems()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECSystem> expected = new ObservableCollection<TECSystem>();
            foreach (TECSystem item in Bid.Systems)
            {
                expected.Add(item);
            }
            TECTypical edit = new TECTypical();

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            Bid.Systems.Add(edit);
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            ObservableCollection<TECTypical> actual = Bid.Systems;
            Assert.AreEqual(expected.Count, actual.Count, "Not Undone");

        }
        
        [TestMethod]
        public void Undo_Bid_Notes()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECLabeled> expected = new ObservableCollection<TECLabeled>();
            foreach (TECLabeled item in Bid.Notes)
            {
                expected.Add(item);
            }
            TECLabeled edit = new TECLabeled();

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            Bid.Notes.Add(edit);
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            ObservableCollection<TECLabeled> actual = Bid.Notes;
            Assert.AreEqual(expected.Count, actual.Count, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_Exclusions()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECLabeled> expected = new ObservableCollection<TECLabeled>();
            foreach (TECLabeled item in Bid.Exclusions)
            {
                expected.Add(item);
            }
            TECLabeled edit = new TECLabeled();

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            Bid.Exclusions.Add(edit);
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            ObservableCollection<TECLabeled> actual = Bid.Exclusions;
            Assert.AreEqual(expected.Count, actual.Count, "Not Undone");

        }
        
        
        [TestMethod]
        public void Undo_Bid_Locations()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECLabeled> expected = new ObservableCollection<TECLabeled>();
            foreach (TECLabeled item in Bid.Locations)
            {
                expected.Add(item);
            }
            TECLabeled edit = new TECLabeled();
            edit.Label = "Edit";

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            Bid.Locations.Add(edit);
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            ObservableCollection<TECLabeled> actual = Bid.Locations;
            Assert.AreEqual(expected.Count, actual.Count, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_MiscCost()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECMisc> expected = new ObservableCollection<TECMisc>();
            foreach (TECMisc item in Bid.MiscCosts)
            {
                expected.Add(item);
            }
            TECMisc edit = new TECMisc(CostType.TEC, false);

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            Bid.MiscCosts.Add(edit);
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            ObservableCollection<TECMisc> actual = Bid.MiscCosts;
            Assert.AreEqual(expected.Count, actual.Count, "Not Undone");

        }
        
        [TestMethod]
        public void Undo_Bid_Panel()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECPanel> expected = new ObservableCollection<TECPanel>();
            foreach (TECPanel item in Bid.Panels)
            {
                expected.Add(item);
            }
            TECPanel edit = new TECPanel(Bid.Catalogs.PanelTypes[0], false);

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            Bid.Panels.Add(edit);
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            ObservableCollection<TECPanel> actual = Bid.Panels;
            Assert.AreEqual(expected.Count, actual.Count, "Not Undone");

        }

        #endregion

        #region Labor Properties
        [TestMethod]
        public void Undo_Labor_Soft()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            double expected = Bid.Parameters.SoftCoef;

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            Bid.Parameters.SoftCoef = 1.1;
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            double actual = Bid.Parameters.SoftCoef;
            Assert.AreEqual(expected, actual, "Not Undone");
        }

        [TestMethod]
        public void Undo_Labor_PM()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            double expected = Bid.Parameters.PMCoef;

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            Bid.Parameters.PMCoef = 1.1;
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            double actual = Bid.Parameters.PMCoef;
            Assert.AreEqual(expected, actual, "Not Undone");
        }

        [TestMethod]
        public void Undo_Labor_ENG()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            double expected = Bid.Parameters.ENGCoef;

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            Bid.Parameters.ENGCoef = 1.1;
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            double actual = Bid.Parameters.ENGCoef;
            Assert.AreEqual(expected, actual, "Not Undone");
        }

        [TestMethod]
        public void Undo_Labor_Comm()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            double expected = Bid.Parameters.CommCoef;

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            Bid.Parameters.CommCoef = 1.1;
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            double actual = Bid.Parameters.CommCoef;
            Assert.AreEqual(expected, actual, "Not Undone");
        }

        [TestMethod]
        public void Undo_Labor_Graph()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            double expected = Bid.Parameters.GraphCoef;

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            Bid.Parameters.GraphCoef = 1.1;
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            double actual = Bid.Parameters.GraphCoef;
            Assert.AreEqual(expected, actual, "Not Undone");
        }

        [TestMethod]
        public void Undo_Labor_Electrical()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            double expected = Bid.Parameters.ElectricalRate;

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            Bid.Parameters.ElectricalRate = 1.1;
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            double actual = Bid.Parameters.ElectricalRate;
            Assert.AreEqual(expected, actual, "Not Undone");
        }

        [TestMethod]
        public void Undo_Labor_ElectricalModifier()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            bool expected = Bid.Parameters.ElectricalIsOnOvertime;

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            Bid.Parameters.ElectricalIsOnOvertime = !Bid.Parameters.ElectricalIsOnOvertime;
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            bool actual = Bid.Parameters.ElectricalIsOnOvertime;
            Assert.AreEqual(expected, actual, "Not Undone");
        }

        #endregion

        #region Location Properties

        [TestMethod]
        public void Undo_Location_Name()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string expected = Bid.Locations[0].Label;
            string edit = "Edit";

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            Bid.Locations[0].Label = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            string actual = Bid.Locations[0].Label;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        #endregion

        #region System Properties

        [TestMethod]
        public void Undo_System_Name()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            var system = Bid.Systems[0];
            string expected = system.Name;
            string edit = "Edit";

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            system.Name = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            string actual = system.Name;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_System_Description()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            var system = Bid.Systems[0];
            string expected = system.Description;
            string edit = "Edit";
            

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            system.Description = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            string actual = system.Description;
            Assert.AreEqual(expected, actual, "Not Undone");

        }
        
        [TestMethod]
        public void Undo_System_Equipment()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            var system = Bid.Systems[0];
            ObservableCollection<TECEquipment> expected = new ObservableCollection<TECEquipment>();
            foreach (TECEquipment item in system.Equipment)
            {
                expected.Add(item);
            }
            TECEquipment edit = new TECEquipment(true);

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            system.Equipment.Add(edit);
            //Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            ObservableCollection<TECEquipment> actual = system.Equipment;
            Assert.AreEqual(expected.Count, actual.Count, "Not Undone");

        }

        [TestMethod]
        public void Undo_System_Location()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECTypical system = null;
            foreach(TECTypical item in Bid.Systems)
            {
                if(item.Location != null)
                {
                    system = item;
                }
            }
            Guid expected = new Guid(system.Location.Guid.ToString());
            TECLabeled edit = new TECLabeled();
            edit.Label = "Floor 42";

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            system.Location = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            Guid actual = new Guid(system.Location.Guid.ToString());
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        #endregion

        #region Equipment Properties

        [TestMethod]
        public void Undo_Equipment_Name()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            var equipment = Bid.Systems[0].Equipment[0];
            string expected = equipment.Name;
            string edit = "Edit";

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            equipment.Name = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            string actual = equipment.Name;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Equipment_Description()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            var equipment = Bid.Systems[0].Equipment[0];
            string expected = equipment.Description;
            string edit = "Edit";

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            equipment.Description = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            string actual = equipment.Description;
            Assert.AreEqual(expected, actual, "Not Undone");

        }
        
        [TestMethod]
        public void Undo_Equipment_SubScope()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            var equipment = Bid.Systems[0].Equipment[0];
            ObservableCollection<TECSubScope> expected = new ObservableCollection<TECSubScope>();
            foreach (TECSubScope item in equipment.SubScope)
            {
                expected.Add(item);
            }
            TECSubScope edit = new TECSubScope(true);

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            equipment.SubScope.Add(edit);
            //Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            ObservableCollection<TECSubScope> actual = equipment.SubScope;
            Assert.AreEqual(expected.Count, actual.Count, "Not Undone");

        }

        #endregion

        #region SubScope Properties

        [TestMethod]
        public void Undo_SubScope_Name()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            var subScope = Bid.Systems[0].Equipment[0].SubScope[0];
            string expected = subScope.Name;
            string edit = "Edit";

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            subScope.Name = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            string actual = subScope.Name;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_SubScope_Description()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            var subScope = Bid.Systems[0].Equipment[0].SubScope[0];
            string expected = subScope.Description;
            string edit = "Edit";

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            subScope.Description = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            string actual = subScope.Description;
            Assert.AreEqual(expected, actual, "Not Undone");

        }
        

        [TestMethod]
        public void Undo_SubScope_Points()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            var subScope = Bid.Systems[0].Equipment[0].SubScope[0];
            ObservableCollection<TECPoint> expected = new ObservableCollection<TECPoint>();
            foreach (TECPoint item in subScope.Points)
            {
                expected.Add(item);
            }
            TECPoint edit = new TECPoint(true);
            edit.Type = IOType.AI;

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            subScope.Points.Add(edit);
            //Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            ObservableCollection<TECPoint> actual = subScope.Points;
            Assert.AreEqual(expected.Count, actual.Count, "Not Undone");

        }

        [TestMethod]
        public void Undo_SubScope_Device()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            var subScope = Bid.Systems[0].Equipment[0].SubScope[0];
            ObservableCollection<TECDevice> expected = new ObservableCollection<TECDevice>();
            foreach (TECDevice item in subScope.Devices)
            {
                expected.Add(item);
            }
            ObservableCollection<TECElectricalMaterial> types = new ObservableCollection<TECElectricalMaterial>();
            types.Add(Bid.Catalogs.ConnectionTypes[0]);
            TECDevice edit = new TECDevice(types, Bid.Catalogs.Manufacturers[0]);

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            subScope.Devices.Add(edit);
            testStack.Undo();

            //assert
            ObservableCollection<IEndDevice> actual = subScope.Devices;
            Assert.AreEqual(expected.Count, actual.Count, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_Connection_ConduitType()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECElectricalMaterial expected = Bid.Controllers[0].ChildrenConnections[0].ConduitType;
            TECElectricalMaterial edit = Bid.Catalogs.ConduitTypes[1];

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            Bid.Controllers[0].ChildrenConnections[0].ConduitType = edit;
            testStack.Undo();

            //assert
            TECElectricalMaterial actual = Bid.Controllers[0].ChildrenConnections[0].ConduitType;
            Assert.AreEqual(expected.Guid, actual.Guid, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_SubScope_AssociatedCost()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECSubScope testSubScope = Bid.Systems[0].Equipment[0].SubScope[0];
            int expected = testSubScope.AssociatedCosts.Count;
            TECCost edit = Bid.Catalogs.AssociatedCosts[0];

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid);
            DoStacker testStack = new DoStacker(watcher);
            testSubScope.AssociatedCosts.Add(edit);
            testStack.Undo();

            //assert
            int actual = testSubScope.AssociatedCosts.Count;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        #endregion

        #region Point Properties

        [TestMethod]
        public void Undo_Point_Name()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            var point = Bid.Systems[0].Equipment[0].SubScope[0].Points[0];
            string expected = point.Label;
            string edit = "Edit";

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            point.Label = edit;

            testStack.Undo();

            //assert
            string actual = point.Label;
            Assert.AreEqual(expected, actual, "Not Undone");

        }
        
        [TestMethod]
        public void Undo_Point_Quantity()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            var point = Bid.Systems[0].Equipment[0].SubScope[0].Points[0];
            int expected = point.Quantity;
            int edit = 3;

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            point.Quantity = edit;

            testStack.Undo();

            //assert
            int actual = point.Quantity;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Point_Type()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            var point = Bid.Systems[0].Equipment[0].SubScope[0].Points[0];
            string expected = point.Type.ToString();
            IOType edit = IOType.AO;

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            point.Type = edit;

            testStack.Undo();

            //assert
            string actual = point.Type.ToString();
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        #endregion
        
        #region Cost Properties
        [TestMethod]
        public void Undo_MiscCost_Name()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string expected = Bid.MiscCosts[0].Name;
            string edit = "changedName";

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            Bid.MiscCosts[0].Name = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            string actual = Bid.MiscCosts[0].Name;
            Assert.AreEqual(expected, actual, "Not Undone");

        }
        #endregion

        #region Controller Properties
        [TestMethod]
        public void Undo_Controller_Type()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECControllerType expected = Bid.Controllers[0].Type;
            TECControllerType edit = Bid.Catalogs.ControllerTypes[0];

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            Bid.Controllers[0].Type = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoCount(), "Not added to undo stack");
            testStack.Undo();

            //assert
            TECControllerType actual = Bid.Controllers[0].Type;
            Assert.AreEqual(expected, actual, "Not Undone");
        }

        #endregion

        #endregion

        #region Redo
        [TestMethod]
        public void Redo_Bid_Name()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string edit = "Edit";

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid);
            DoStacker testStack = new DoStacker(watcher);
            Bid.Name = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            string actual = Bid.Name;
            Assert.AreEqual(edit, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Bid_Number()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string edit = "Edit";

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            Bid.BidNumber = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            string actual = Bid.BidNumber;
            Assert.AreEqual(edit, actual, "Not Undone");

        }

        [TestMethod]
        public void Redo_Bid_DueDate()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            DateTime edit = new DateTime(2000, 1, 1);

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            Bid.DueDate = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            DateTime actual = Bid.DueDate;
            Assert.AreEqual(edit, actual, "Not Undone");

        }

        [TestMethod]
        public void Redo_Bid_Salesperson()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string expected = Bid.Salesperson;
            string edit = "Edit";

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            Bid.Salesperson = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            string actual = Bid.Salesperson;
            Assert.AreEqual(edit, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Bid_Estimator()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string edit = "Edit";

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            Bid.Estimator = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            string actual = Bid.Estimator;
            Assert.AreEqual(edit, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Bid_ScopeTree()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECScopeBranch edit = new TECScopeBranch(false);

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            Bid.ScopeTree.Add(edit);
            var expected = new ObservableCollection<TECScopeBranch>();
            foreach (TECScopeBranch item in Bid.ScopeTree)
            {
                expected.Add(item);
            }
            testStack.Undo();
            testStack.Redo();

            //assert
            ObservableCollection<TECScopeBranch> actual = Bid.ScopeTree;
            Assert.AreEqual(expected.Count, actual.Count, "Not Redone");

        }

        [TestMethod]
        public void Redo_Bid_Systems()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECTypical edit = new TECTypical();

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid);
            DoStacker testStack = new DoStacker(watcher);
            Bid.Systems.Add(edit);
            var expected = new ObservableCollection<TECTypical>();
            foreach (TECTypical item in Bid.Systems)
            {
                expected.Add(item);
            }
            testStack.Undo();
            testStack.Redo();

            //assert
            ObservableCollection<TECTypical> actual = Bid.Systems;
            Assert.AreEqual(expected.Count, actual.Count, "Not Redone");

        }
        
        [TestMethod]
        public void Redo_Bid_Notes()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECLabeled edit = new TECLabeled();

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            Bid.Notes.Add(edit);
            var expected = new ObservableCollection<TECLabeled>();
            foreach (TECLabeled item in Bid.Notes)
            {
                expected.Add(item);
            }
            testStack.Undo();
            testStack.Redo();

            //assert
            ObservableCollection<TECLabeled> actual = Bid.Notes;
            Assert.AreEqual(expected.Count, actual.Count, "Not Redone");

        }

        [TestMethod]
        public void Redo_Bid_Exclusions()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECLabeled edit = new TECLabeled();

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            Bid.Exclusions.Add(edit);
            var expected = new ObservableCollection<TECLabeled>();
            foreach (TECLabeled item in Bid.Exclusions)
            {
                expected.Add(item);
            }
            testStack.Undo();
            testStack.Redo();

            //assert
            ObservableCollection<TECLabeled> actual = Bid.Exclusions;
            Assert.AreEqual(expected.Count, actual.Count, "Not Redone");

        }

        [TestMethod]
        public void Redo_Bid_MiscCost()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECMisc edit = new TECMisc(CostType.TEC, false);

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            Bid.MiscCosts.Add(edit);
            var expected = new ObservableCollection<TECMisc>();
            foreach (TECMisc item in Bid.MiscCosts)
            {
                expected.Add(item);
            }
            testStack.Undo();
            testStack.Redo();

            //assert
            ObservableCollection<TECMisc> actual = Bid.MiscCosts;
            Assert.AreEqual(expected.Count, actual.Count, "Not Redone");

        }
        
        [TestMethod]
        public void Redo_Bid_Locations()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECLabeled edit = new TECLabeled();
            edit.Label = "This";

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            Bid.Locations.Add(edit);
            var expected = new ObservableCollection<TECLabeled>();
            foreach (TECLabeled item in Bid.Locations)
            {
                expected.Add(item);
            }
            testStack.Undo();
            testStack.Redo();

            //assert
            ObservableCollection<TECLabeled> actual = Bid.Locations;
            Assert.AreEqual(expected.Count, actual.Count, "Not Redone");

        }

        [TestMethod]
        public void Redo_Location_Name()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string edit = "Edit";

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            Bid.Locations[0].Label = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            string actual = Bid.Locations[0].Label ;
            Assert.AreEqual(edit, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Labor_PM()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            Bid.Parameters.PMCoef = 1.1;
            double expected = 1.1;

            //Act
            testStack.Undo();
            testStack.Redo();

            //assert
            double actual = Bid.Parameters.PMCoef;
            Assert.AreEqual(expected, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Labor_ENG()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            Bid.Parameters.ENGCoef = 1.1;
            double expected = 1.1;

            //Act
            testStack.Undo();
            testStack.Redo();

            //assert
            double actual = Bid.Parameters.ENGCoef;
            Assert.AreEqual(expected, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Labor_Comm()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            Bid.Parameters.CommCoef = 1.1;
            double expected = 1.1;

            //Act
            testStack.Undo();
            testStack.Redo();

            //assert
            double actual = Bid.Parameters.CommCoef;
            Assert.AreEqual(expected, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Labor_Soft()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            Bid.Parameters.SoftCoef = 1.1;
            double expected = 1.1;

            //Act
            testStack.Undo();
            testStack.Redo();

            //assert
            double actual = Bid.Parameters.SoftCoef;
            Assert.AreEqual(expected, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Labor_Graph()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            Bid.Parameters.GraphCoef = 1.1;
            double expected = 1.1;

            //Act
            testStack.Undo();
            testStack.Redo();

            //assert
            double actual = Bid.Parameters.GraphCoef;
            Assert.AreEqual(expected, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Labor_Electrical()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            Bid.Parameters.ElectricalRate = 1.1;
            double expected = 1.1;

            //Act
            testStack.Undo();
            testStack.Redo();

            //assert
            double actual = Bid.Parameters.ElectricalRate;
            Assert.AreEqual(expected, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Labor_ElectricalModifier()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            Bid.Parameters.ElectricalIsOnOvertime = !Bid.Parameters.ElectricalIsOnOvertime;
            bool expected = Bid.Parameters.ElectricalIsOnOvertime;

            //Act
            testStack.Undo();
            testStack.Redo();

            //assert
            bool actual = Bid.Parameters.ElectricalIsOnOvertime;
            Assert.AreEqual(expected, actual, "Not Redone");
        }

        #region System
        [TestMethod]
        public void Redo_System_Name()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string edit = "Edit";

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            var system = Bid.Systems[0];
            system.Name = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            string actual = system.Name;
            Assert.AreEqual(edit, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_System_Description()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string edit = "Edit";

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            var system = Bid.Systems[0];
            system.Description = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            string actual = system.Description;
            Assert.AreEqual(edit, actual, "Not Redone");

        }
        
        [TestMethod]
        public void Redo_System_Equipment()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECEquipment edit = new TECEquipment(true);

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            var system = Bid.Systems[0];
            system.Equipment.Add(edit);
            var expected = new ObservableCollection<TECEquipment>();
            foreach (TECEquipment item in system.Equipment)
            {
                expected.Add(item);
            }
            testStack.Undo();
            testStack.Redo();

            //assert
            ObservableCollection<TECEquipment> actual = system.Equipment;
            Assert.AreEqual(expected.Count, actual.Count, "Not Undone");

        }

        [TestMethod]
        public void Redo_System_Location()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECLabeled edit = new TECLabeled();
            edit.Label = "Floor 42";

            var system = new TECTypical();
            Bid.Systems.Add(system);

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid);
            DoStacker testStack = new DoStacker(watcher);
            system.Location = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            TECLabeled actual = system.Location;
            Assert.AreEqual(edit, actual, "Not Redone");

        }

        #endregion

        #region Equipment
        [TestMethod]
        public void Redo_Equipment_Name()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string edit = "Edit";

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            var equipment = Bid.Systems[0].Equipment[0];
            equipment.Name = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            string actual = equipment.Name;
            Assert.AreEqual(edit, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Equipment_Description()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string edit = "Edit";

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            var equipment = Bid.Systems[0].Equipment[0];
            equipment.Description = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            string actual = equipment.Description;
            Assert.AreEqual(edit, actual, "Not Redone");

        }
        
        [TestMethod]
        public void Redo_Equipment_SubScope()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECSubScope edit = new TECSubScope(true);

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            var equipment = Bid.Systems[0].Equipment[0];
            equipment.SubScope.Add(edit);
            var expected = new ObservableCollection<TECSubScope>();
            foreach (TECSubScope item in equipment.SubScope)
            {
                expected.Add(item);
            }
            testStack.Undo();
            testStack.Redo();

            //assert
            ObservableCollection<TECSubScope> actual = equipment.SubScope;
            Assert.AreEqual(expected.Count, actual.Count, "Not Redone");

        }
        #endregion

        #region Subscope
        [TestMethod]
        public void Redo_SubScope_Name()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string edit = "Edit";

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            var subscope = Bid.Systems[0].Equipment[0].SubScope[0];
            subscope.Name = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            string actual = subscope.Name;
            Assert.AreEqual(edit, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_SubScope_Description()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string edit = "Edit";

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            var subscope = Bid.Systems[0].Equipment[0].SubScope[0];
            subscope.Description = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            string actual = subscope.Description;
            Assert.AreEqual(edit, actual, "Not Redone");

        }
        
        [TestMethod]
        public void Redo_SubScope_Points()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECPoint edit = new TECPoint(true);
            edit.Type = IOType.AI;

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            var subscope = Bid.Systems[0].Equipment[0].SubScope[0];
            subscope.Points.Add(edit);
            var expected = new ObservableCollection<TECPoint>();
            foreach (TECPoint item in subscope.Points)
            {
                expected.Add(item);
            }
            testStack.Undo();
            testStack.Redo();

            //assert
            ObservableCollection<TECPoint> actual = subscope.Points;
            Assert.AreEqual(expected.Count, actual.Count, "Not Redone");

        }

        [TestMethod]
        public void Redo_SubScope_Device()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECElectricalMaterial> types = new ObservableCollection<TECElectricalMaterial>();
            types.Add(Bid.Catalogs.ConnectionTypes[0]);
            TECDevice edit = new TECDevice(types, Bid.Catalogs.Manufacturers[0]);

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            var subscope = Bid.Systems[0].Equipment[0].SubScope[0];
            subscope.Devices.Add(edit);
            var expected = new ObservableCollection<TECDevice>();
            foreach (TECDevice item in subscope.Devices)
            {
                expected.Add(item);
            }
            testStack.Undo();
            testStack.Redo();

            //assert
            ObservableCollection<IEndDevice> actual = subscope.Devices;
            Assert.AreEqual(expected.Count, actual.Count, "Not Redone");

        }

        #endregion
        
        #region Point
        [TestMethod]
        public void Redo_Point_Name()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string edit = "Edit";

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            var point = Bid.Systems[0].Equipment[0].SubScope[0].Points[0];
            point.Label = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            string actual = point.Label;
            Assert.AreEqual(edit, actual, "Not Redone");

        }
        
        [TestMethod]
        public void Redo_Point_Quantity()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            int edit = 3;

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            int beforeCount = testStack.UndoCount();
            var point = Bid.Systems[0].Equipment[0].SubScope[0].Points[0];
            point.Quantity = edit;

            testStack.Undo();
            testStack.Redo();

            //assert
            int actual = point.Quantity;
            Assert.AreEqual(edit, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Point_Type()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            IOType edit = IOType.AO;

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            var point = Bid.Systems[0].Equipment[0].SubScope[0].Points[0];
            point.Type = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            IOType actual = point.Type;
            Assert.AreEqual(edit, actual, "Not Redone");

        }

        #endregion

        #region Panel
        [TestMethod]
        public void Redo_Panel_Name()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string edit = "Edit";

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            var panel = Bid.Panels[0];
            panel.Name = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            string actual = panel.Name;
            Assert.AreEqual(edit, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Panel_PanelType()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECPanelType edit = new TECPanelType(Bid.Catalogs.Manufacturers[0]);

            //Act
            ChangeWatcher watcher = new ChangeWatcher(Bid); DoStacker testStack = new DoStacker(watcher);
            var panel = Bid.Panels[0];
            panel.Type = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            TECPanelType actual = Bid.Panels[0].Type;
            Assert.AreEqual(edit.Guid, actual.Guid, "Not Redone");

        }
        #endregion
        #endregion
    }
}
