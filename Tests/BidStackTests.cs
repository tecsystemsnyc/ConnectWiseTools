using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using System.Collections.ObjectModel;
using System.Diagnostics;

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
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Name = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
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
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.BidNumber = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
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
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.DueDate = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
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
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Salesperson = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
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
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Estimator = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
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
            TECScopeBranch edit = new TECScopeBranch();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.ScopeTree.Add(edit);
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
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
            TECSystem edit = new TECSystem();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Systems.Add(edit);
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            ObservableCollection<TECSystem> actual = Bid.Systems;
            Assert.AreEqual(expected.Count, actual.Count, "Not Undone");

        }

        //[TestMethod]
        //public void Undo_Bid_VisualScope()
        //{
        //    //Arrange
        //    var Bid = TestHelper.CreateTestBid();
        //    ObservableCollection<TECVisualScope> expected = new ObservableCollection<TECVisualScope>();
        //    foreach (TECVisualScope item in Bid.Drawings[0].Pages[0].PageScope)
        //    {
        //        expected.Add(item);
        //    }
        //    TECVisualScope edit = new TECVisualScope(new TECSystem(), 1.0, 1.0);

        //    //Act
        //    ChangeStack testStack = new ChangeStack(Bid);
        //    int beforeCount = testStack.UndoStack.Count;
        //    Bid.Drawings[0].Pages[0].PageScope.Add(edit);
        //    Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
        //    testStack.Undo();

        //    //assert
        //    ObservableCollection<TECVisualScope> actual = Bid.Drawings[0].Pages[0].PageScope;
        //    Assert.AreEqual(expected.Count, actual.Count, "Not Undone");

        //}

        [TestMethod]
        public void Undo_Bid_Catalogs_Devices()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECDevice> expected = new ObservableCollection<TECDevice>();
            foreach (TECDevice item in Bid.Catalogs.Devices)
            {
                expected.Add(item);
            }
            ObservableCollection<TECConnectionType> types = new ObservableCollection<TECConnectionType>();
            types.Add(Bid.Catalogs.ConnectionTypes[0]);
            TECDevice edit = new TECDevice(types, Bid.Catalogs.Manufacturers[0]);

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Catalogs.Devices.Add(edit);
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            ObservableCollection<TECDevice> actual = Bid.Catalogs.Devices;
            Assert.AreEqual(expected.Count, actual.Count, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_Catalogs_Manufacturers()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECManufacturer> expected = new ObservableCollection<TECManufacturer>();
            foreach (TECManufacturer item in Bid.Catalogs.Manufacturers)
            {
                expected.Add(item);
            }
            TECManufacturer edit = new TECManufacturer();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Catalogs.Manufacturers.Add(edit);
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            ObservableCollection<TECManufacturer> actual = Bid.Catalogs.Manufacturers;
            Assert.AreEqual(expected.Count, actual.Count, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_Notes()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECNote> expected = new ObservableCollection<TECNote>();
            foreach (TECNote item in Bid.Notes)
            {
                expected.Add(item);
            }
            TECNote edit = new TECNote();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Notes.Add(edit);
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            ObservableCollection<TECNote> actual = Bid.Notes;
            Assert.AreEqual(expected.Count, actual.Count, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_Exclusions()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECExclusion> expected = new ObservableCollection<TECExclusion>();
            foreach (TECExclusion item in Bid.Exclusions)
            {
                expected.Add(item);
            }
            TECExclusion edit = new TECExclusion();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Exclusions.Add(edit);
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            ObservableCollection<TECExclusion> actual = Bid.Exclusions;
            Assert.AreEqual(expected.Count, actual.Count, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_Tags()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECTag> expected = new ObservableCollection<TECTag>();
            foreach (TECTag item in Bid.Catalogs.Tags)
            {
                expected.Add(item);
            }
            TECTag edit = new TECTag();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Catalogs.Tags.Add(edit);
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            ObservableCollection<TECTag> actual = Bid.Catalogs.Tags;
            Assert.AreEqual(expected.Count, actual.Count, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_Drawings()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECDrawing> expected = new ObservableCollection<TECDrawing>();
            foreach (TECDrawing item in Bid.Drawings)
            {
                expected.Add(item);
            }
            TECDrawing edit = new TECDrawing("This");

            Trace.Write("Number of drawings: " + Bid.Drawings.Count + "\n");
            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Drawings.Add(edit);
            Trace.Write("Undo count: " + testStack.UndoStack.Count);
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            ObservableCollection<TECDrawing> actual = Bid.Drawings;
            Assert.AreEqual(expected.Count, actual.Count, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_Locations()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECLocation> expected = new ObservableCollection<TECLocation>();
            foreach (TECLocation item in Bid.Locations)
            {
                expected.Add(item);
            }
            TECLocation edit = new TECLocation();
            edit.Name = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Locations.Add(edit);
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            ObservableCollection<TECLocation> actual = Bid.Locations;
            Assert.AreEqual(expected.Count, actual.Count, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_MiscCost()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECMiscCost> expected = new ObservableCollection<TECMiscCost>();
            foreach (TECMiscCost item in Bid.MiscCosts)
            {
                expected.Add(item);
            }
            TECMiscCost edit = new TECMiscCost();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.MiscCosts.Add(edit);
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            ObservableCollection<TECMiscCost> actual = Bid.MiscCosts;
            Assert.AreEqual(expected.Count, actual.Count, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_MiscWiring()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECMiscWiring> expected = new ObservableCollection<TECMiscWiring>();
            foreach (TECMiscWiring item in Bid.MiscWiring)
            {
                expected.Add(item);
            }
            TECMiscWiring edit = new TECMiscWiring();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.MiscWiring.Add(edit);
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            ObservableCollection<TECMiscWiring> actual = Bid.MiscWiring;
            Assert.AreEqual(expected.Count, actual.Count, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_PanelType()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECPanelType> expected = new ObservableCollection<TECPanelType>();
            foreach (TECPanelType item in Bid.Catalogs.PanelTypes)
            {
                expected.Add(item);
            }
            TECPanelType edit = new TECPanelType();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Catalogs.PanelTypes.Add(edit);
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            ObservableCollection<TECPanelType> actual = Bid.Catalogs.PanelTypes;
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
            TECPanel edit = new TECPanel(Bid.Catalogs.PanelTypes[0]);

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Panels.Add(edit);
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
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
            double expected = Bid.Labor.SoftCoef;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Labor.SoftCoef = 1.1;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            double actual = Bid.Labor.SoftCoef;
            Assert.AreEqual(expected, actual, "Not Undone");
        }

        [TestMethod]
        public void Undo_Labor_PM()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            double expected = Bid.Labor.PMCoef;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Labor.PMCoef = 1.1;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            double actual = Bid.Labor.PMCoef;
            Assert.AreEqual(expected, actual, "Not Undone");
        }

        [TestMethod]
        public void Undo_Labor_ENG()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            double expected = Bid.Labor.ENGCoef;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Labor.ENGCoef = 1.1;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            double actual = Bid.Labor.ENGCoef;
            Assert.AreEqual(expected, actual, "Not Undone");
        }

        [TestMethod]
        public void Undo_Labor_Comm()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            double expected = Bid.Labor.CommCoef;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Labor.CommCoef = 1.1;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            double actual = Bid.Labor.CommCoef;
            Assert.AreEqual(expected, actual, "Not Undone");
        }

        [TestMethod]
        public void Undo_Labor_Graph()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            double expected = Bid.Labor.GraphCoef;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Labor.GraphCoef = 1.1;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            double actual = Bid.Labor.GraphCoef;
            Assert.AreEqual(expected, actual, "Not Undone");
        }

        [TestMethod]
        public void Undo_Labor_Electrical()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            double expected = Bid.Labor.ElectricalRate;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Labor.ElectricalRate = 1.1;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            double actual = Bid.Labor.ElectricalRate;
            Assert.AreEqual(expected, actual, "Not Undone");
        }

        [TestMethod]
        public void Undo_Labor_ElectricalModifier()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            bool expected = Bid.Labor.ElectricalIsOnOvertime;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Labor.ElectricalIsOnOvertime = !Bid.Labor.ElectricalIsOnOvertime;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            bool actual = Bid.Labor.ElectricalIsOnOvertime;
            Assert.AreEqual(expected, actual, "Not Undone");
        }

        #endregion

        #region Location Properties

        [TestMethod]
        public void Undo_Location_Name()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string expected = Bid.Locations[0].Name;
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Locations[0].Name = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            string actual = Bid.Locations[0].Name;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        #endregion

        #region System Properties

        [TestMethod]
        public void Undo_System_Name()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string expected = Bid.Systems[0].Name;
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Systems[0].Name = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            string actual = Bid.Systems[0].Name;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_System_Description()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string expected = Bid.Systems[0].Description;
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Systems[0].Description = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            string actual = Bid.Systems[0].Description;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_System_Quantity()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            int expected = Bid.Systems[0].Quantity;
            int edit = 3;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Systems[0].Quantity = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            int actual = Bid.Systems[0].Quantity;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_System_Equipment()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECEquipment> expected = new ObservableCollection<TECEquipment>();
            foreach (TECEquipment item in Bid.Systems[0].Equipment)
            {
                expected.Add(item);
            }
            TECEquipment edit = new TECEquipment();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Systems[0].Equipment.Add(edit);
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            ObservableCollection<TECEquipment> actual = Bid.Systems[0].Equipment;
            Assert.AreEqual(expected.Count, actual.Count, "Not Undone");

        }

        [TestMethod]
        public void Undo_System_Location()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            Guid expected = new Guid(Bid.Systems[0].Location.Guid.ToString());
            TECLocation edit = new TECLocation();
            edit.Name = "Floor 42";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Systems[0].Location = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            Guid actual = new Guid(Bid.Systems[0].Location.Guid.ToString());
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        #endregion

        #region Equipment Properties

        [TestMethod]
        public void Undo_Equipment_Name()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string expected = Bid.Systems[0].Equipment[0].Name;
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Systems[0].Equipment[0].Name = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            string actual = Bid.Systems[0].Equipment[0].Name;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Equipment_Description()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string expected = Bid.Systems[0].Equipment[0].Description;
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Systems[0].Equipment[0].Description = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            string actual = Bid.Systems[0].Equipment[0].Description;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Equipment_Quantity()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            int expected = Bid.Systems[0].Equipment[0].Quantity;
            int edit = 3;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Systems[0].Equipment[0].Quantity = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            int actual = Bid.Systems[0].Equipment[0].Quantity;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Equipment_SubScope()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECSubScope> expected = new ObservableCollection<TECSubScope>();
            foreach (TECSubScope item in Bid.Systems[0].Equipment[0].SubScope)
            {
                expected.Add(item);
            }
            TECSubScope edit = new TECSubScope();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Systems[0].Equipment[0].SubScope.Add(edit);
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            ObservableCollection<TECSubScope> actual = Bid.Systems[0].Equipment[0].SubScope;
            Assert.AreEqual(expected.Count, actual.Count, "Not Undone");

        }

        #endregion

        #region SubScope Properties

        [TestMethod]
        public void Undo_SubScope_Name()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string expected = Bid.Systems[0].Equipment[0].SubScope[0].Name;
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Systems[0].Equipment[0].SubScope[0].Name = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            string actual = Bid.Systems[0].Equipment[0].SubScope[0].Name;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_SubScope_Description()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string expected = Bid.Systems[0].Equipment[0].SubScope[0].Description;
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Systems[0].Equipment[0].SubScope[0].Description = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            string actual = Bid.Systems[0].Equipment[0].SubScope[0].Description;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_SubScope_Quantity()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            int expected = Bid.Systems[0].Equipment[0].SubScope[0].Quantity;
            int edit = 3;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Systems[0].Equipment[0].SubScope[0].Quantity = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            int actual = Bid.Systems[0].Equipment[0].SubScope[0].Quantity;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_SubScope_Points()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECPoint> expected = new ObservableCollection<TECPoint>();
            foreach (TECPoint item in Bid.Systems[0].Equipment[0].SubScope[0].Points)
            {
                expected.Add(item);
            }
            TECPoint edit = new TECPoint();
            edit.Type = PointTypes.AI;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Systems[0].Equipment[0].SubScope[0].Points.Add(edit);
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            ObservableCollection<TECPoint> actual = Bid.Systems[0].Equipment[0].SubScope[0].Points;
            Assert.AreEqual(expected.Count, actual.Count, "Not Undone");

        }

        [TestMethod]
        public void Undo_SubScope_Device()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECDevice> expected = new ObservableCollection<TECDevice>();
            foreach (TECDevice item in Bid.Systems[0].Equipment[0].SubScope[0].Devices)
            {
                expected.Add(item);
            }
            ObservableCollection<TECConnectionType> types = new ObservableCollection<TECConnectionType>();
            types.Add(Bid.Catalogs.ConnectionTypes[0]);
            TECDevice edit = new TECDevice(types, Bid.Catalogs.Manufacturers[0]);

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Systems[0].Equipment[0].SubScope[0].Devices.Add(edit);
            Assert.AreEqual((beforeCount + 2), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            ObservableCollection<TECDevice> actual = Bid.Systems[0].Equipment[0].SubScope[0].Devices;
            Assert.AreEqual(expected.Count, actual.Count, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_Connection_ConduitType()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECConduitType expected = Bid.Controllers[0].ChildrenConnections[0].ConduitType;
            TECConduitType edit = Bid.Catalogs.ConduitTypes[1];

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Controllers[0].ChildrenConnections[0].ConduitType = edit;
            testStack.Undo();

            //assert
            TECConduitType actual = Bid.Controllers[0].ChildrenConnections[0].ConduitType;
            Assert.AreEqual(expected.Guid, actual.Guid, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_SubScope_AssociatedCost()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            int expected = Bid.Systems[0].Equipment[0].SubScope[0].AssociatedCosts.Count;
            TECCost edit = new TECCost();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope[0].AssociatedCosts.Add(edit);
            testStack.Undo();

            //assert
            int actual = Bid.Systems[0].Equipment[0].SubScope[0].AssociatedCosts.Count;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        #endregion

        #region Point Properties

        [TestMethod]
        public void Undo_Point_Name()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string expected = Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Name;
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Name = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            string actual = Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Name;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Point_Description()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string expected = Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Description;
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Description = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            string actual = Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Description;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Point_Quantity()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            int expected = Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Quantity;
            int edit = 3;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Quantity = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            int actual = Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Quantity;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Point_Type()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string expected = Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Type.ToString();
            PointTypes edit = PointTypes.AO;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Type = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            string actual = Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Type.ToString(); ;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        #endregion

        #region Manufacturer Properties

        [TestMethod]
        public void Undo_Manufacturer_Name()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string expected = Bid.Systems[0].Equipment[0].SubScope[0].Devices[0].Manufacturer.Name;
            string edit = "changedName";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Systems[0].Equipment[0].SubScope[0].Devices[0].Manufacturer.Name = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            string actual = Bid.Systems[0].Equipment[0].SubScope[0].Devices[0].Manufacturer.Name;
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
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.MiscCosts[0].Name = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            string actual = Bid.MiscCosts[0].Name;
            Assert.AreEqual(expected, actual, "Not Undone");

        }
        [TestMethod]
        public void Undo_MiscWiring_Name()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string expected = Bid.MiscWiring[0].Name;
            string edit = "changedName";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.MiscWiring[0].Name = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            string actual = Bid.MiscWiring[0].Name;
            Assert.AreEqual(expected, actual, "Not Undone");

        }
        #endregion

        #region Controller Properties
        [TestMethod]
        public void Undo_Controller_IOModule()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECIOModule expected = Bid.Controllers[0].IO[0].IOModule;
            TECIOModule edit = new TECIOModule();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Controllers[0].IO[0].IOModule = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();

            //assert
            TECIOModule actual = Bid.Controllers[0].IO[0].IOModule;
            Assert.AreEqual(expected.Guid, actual.Guid, "Not Undone");
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
            ChangeStack testStack = new ChangeStack(Bid);
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
            ChangeStack testStack = new ChangeStack(Bid);
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
            ChangeStack testStack = new ChangeStack(Bid);
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
            ChangeStack testStack = new ChangeStack(Bid);
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
            ChangeStack testStack = new ChangeStack(Bid);
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
            TECScopeBranch edit = new TECScopeBranch();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
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
            TECSystem edit = new TECSystem();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems.Add(edit);
            var expected = new ObservableCollection<TECSystem>();
            foreach (TECSystem item in Bid.Systems)
            {
                expected.Add(item);
            }
            testStack.Undo();
            testStack.Redo();

            //assert
            ObservableCollection<TECSystem> actual = Bid.Systems;
            Assert.AreEqual(expected.Count, actual.Count, "Not Redone");

        }

        [TestMethod]
        public void Redo_Bid_Catalogs_Devices()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECConnectionType> types = new ObservableCollection<TECConnectionType>();
            types.Add(Bid.Catalogs.ConnectionTypes[0]);
            TECDevice edit = new TECDevice(types, Bid.Catalogs.Manufacturers[0]);

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Catalogs.Devices.Add(edit);
            var expected = new ObservableCollection<TECDevice>();
            foreach (TECDevice item in Bid.Catalogs.Devices)
            {
                expected.Add(item);
            }
            testStack.Undo();
            testStack.Redo();

            //assert
            ObservableCollection<TECDevice> actual = Bid.Catalogs.Devices;
            Assert.AreEqual(expected.Count, actual.Count, "Not Redone");

        }

        [TestMethod]
        public void Redo_Bid_Catalogs_Manufacturers()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECManufacturer edit = new TECManufacturer();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Catalogs.Manufacturers.Add(edit);
            var expected = new ObservableCollection<TECManufacturer>();
            foreach (TECManufacturer item in Bid.Catalogs.Manufacturers)
            {
                expected.Add(item);
            }
            testStack.Undo();
            testStack.Redo();

            //assert
            ObservableCollection<TECManufacturer> actual = Bid.Catalogs.Manufacturers;
            Assert.AreEqual(expected.Count, actual.Count, "Not Redone");

        }

        [TestMethod]
        public void Redo_Bid_Notes()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECNote edit = new TECNote();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Notes.Add(edit);
            var expected = new ObservableCollection<TECNote>();
            foreach (TECNote item in Bid.Notes)
            {
                expected.Add(item);
            }
            testStack.Undo();
            testStack.Redo();

            //assert
            ObservableCollection<TECNote> actual = Bid.Notes;
            Assert.AreEqual(expected.Count, actual.Count, "Not Redone");

        }

        [TestMethod]
        public void Redo_Bid_Exclusions()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECExclusion edit = new TECExclusion();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Exclusions.Add(edit);
            var expected = new ObservableCollection<TECExclusion>();
            foreach (TECExclusion item in Bid.Exclusions)
            {
                expected.Add(item);
            }
            testStack.Undo();
            testStack.Redo();

            //assert
            ObservableCollection<TECExclusion> actual = Bid.Exclusions;
            Assert.AreEqual(expected.Count, actual.Count, "Not Redone");

        }

        [TestMethod]
        public void Redo_Bid_MiscCost()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECMiscCost edit = new TECMiscCost();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.MiscCosts.Add(edit);
            var expected = new ObservableCollection<TECMiscCost>();
            foreach (TECMiscCost item in Bid.MiscCosts)
            {
                expected.Add(item);
            }
            testStack.Undo();
            testStack.Redo();

            //assert
            ObservableCollection<TECMiscCost> actual = Bid.MiscCosts;
            Assert.AreEqual(expected.Count, actual.Count, "Not Redone");

        }

        [TestMethod]
        public void Redo_Bid_MiscWiring()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECMiscWiring edit = new TECMiscWiring();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.MiscWiring.Add(edit);
            var expected = new ObservableCollection<TECMiscWiring>();
            foreach (TECMiscWiring item in Bid.MiscWiring)
            {
                expected.Add(item);
            }
            testStack.Undo();
            testStack.Redo();

            //assert
            ObservableCollection<TECMiscWiring> actual = Bid.MiscWiring;
            Assert.AreEqual(expected.Count, actual.Count, "Not Redone");

        }

        [TestMethod]
        public void Redo_Bid_PanelType()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECPanelType edit = new TECPanelType();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Catalogs.PanelTypes.Add(edit);
            var expected = new ObservableCollection<TECPanelType>();
            foreach (TECPanelType item in Bid.Catalogs.PanelTypes)
            {
                expected.Add(item);
            }
            testStack.Undo();
            testStack.Redo();

            //assert
            ObservableCollection<TECPanelType> actual = Bid.Catalogs.PanelTypes;
            Assert.AreEqual(expected.Count, actual.Count, "Not Redone");

        }

        [TestMethod]
        public void Redo_Bid_Tags()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECTag edit = new TECTag();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Catalogs.Tags.Add(edit);
            var expected = new ObservableCollection<TECTag>();
            foreach (TECTag item in Bid.Catalogs.Tags)
            {
                expected.Add(item);
            }
            testStack.Undo();
            testStack.Redo();

            //assert
            ObservableCollection<TECTag> actual = Bid.Catalogs.Tags;
            Assert.AreEqual(expected.Count, actual.Count, "Not Redone");

        }

        [TestMethod]
        public void Redo_Bid_Drawings()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECDrawing edit = new TECDrawing("This");

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Drawings.Add(edit);
            var expected = new ObservableCollection<TECDrawing>();
            foreach (TECDrawing item in Bid.Drawings)
            {
                expected.Add(item);
            }
            testStack.Undo();
            testStack.Redo();

            //assert
            ObservableCollection<TECDrawing> actual = Bid.Drawings;
            Assert.AreEqual(expected.Count, actual.Count, "Not Redone");

        }

        [TestMethod]
        public void Redo_Bid_Locations()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECLocation edit = new TECLocation();
            edit.Name = "This";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Locations.Add(edit);
            var expected = new ObservableCollection<TECLocation>();
            foreach (TECLocation item in Bid.Locations)
            {
                expected.Add(item);
            }
            testStack.Undo();
            testStack.Redo();

            //assert
            ObservableCollection<TECLocation> actual = Bid.Locations;
            Assert.AreEqual(expected.Count, actual.Count, "Not Redone");

        }

        [TestMethod]
        public void Redo_Location_Name()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Locations[0].Name = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            string actual = Bid.Locations[0].Name;
            Assert.AreEqual(edit, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Labor_PM()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Labor.PMCoef = 1.1;
            double expected = 1.1;

            //Act
            testStack.Undo();
            testStack.Redo();

            //assert
            double actual = Bid.Labor.PMCoef;
            Assert.AreEqual(expected, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Labor_ENG()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Labor.ENGCoef = 1.1;
            double expected = 1.1;

            //Act
            testStack.Undo();
            testStack.Redo();

            //assert
            double actual = Bid.Labor.ENGCoef;
            Assert.AreEqual(expected, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Labor_Comm()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Labor.CommCoef = 1.1;
            double expected = 1.1;

            //Act
            testStack.Undo();
            testStack.Redo();

            //assert
            double actual = Bid.Labor.CommCoef;
            Assert.AreEqual(expected, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Labor_Soft()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Labor.SoftCoef = 1.1;
            double expected = 1.1;

            //Act
            testStack.Undo();
            testStack.Redo();

            //assert
            double actual = Bid.Labor.SoftCoef;
            Assert.AreEqual(expected, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Labor_Graph()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Labor.GraphCoef = 1.1;
            double expected = 1.1;

            //Act
            testStack.Undo();
            testStack.Redo();

            //assert
            double actual = Bid.Labor.GraphCoef;
            Assert.AreEqual(expected, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Labor_Electrical()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Labor.ElectricalRate = 1.1;
            double expected = 1.1;

            //Act
            testStack.Undo();
            testStack.Redo();

            //assert
            double actual = Bid.Labor.ElectricalRate;
            Assert.AreEqual(expected, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Labor_ElectricalModifier()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Labor.ElectricalIsOnOvertime = !Bid.Labor.ElectricalIsOnOvertime;
            bool expected = Bid.Labor.ElectricalIsOnOvertime;

            //Act
            testStack.Undo();
            testStack.Redo();

            //assert
            bool actual = Bid.Labor.ElectricalIsOnOvertime;
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
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Name = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            string actual = Bid.Systems[0].Name;
            Assert.AreEqual(edit, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_System_Description()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Description = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            string actual = Bid.Systems[0].Description;
            Assert.AreEqual(edit, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_System_Quantity()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            int edit = 3;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Quantity = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            int actual = Bid.Systems[0].Quantity;
            Assert.AreEqual(edit, actual, "Not Undone");

        }

        [TestMethod]
        public void Redo_System_Equipment()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECEquipment edit = new TECEquipment();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment.Add(edit);
            var expected = new ObservableCollection<TECEquipment>();
            foreach (TECEquipment item in Bid.Systems[0].Equipment)
            {
                expected.Add(item);
            }
            testStack.Undo();
            testStack.Redo();

            //assert
            ObservableCollection<TECEquipment> actual = Bid.Systems[0].Equipment;
            Assert.AreEqual(expected.Count, actual.Count, "Not Undone");

        }

        [TestMethod]
        public void Redo_System_Location()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECLocation edit = new TECLocation();
            edit.Name = "Floor 42";

            var system = new TECSystem();
            Bid.Systems.Add(system);

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            system.Location = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            TECLocation actual = system.Location;
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
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].Name = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            string actual = Bid.Systems[0].Equipment[0].Name;
            Assert.AreEqual(edit, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Equipment_Description()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].Description = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            string actual = Bid.Systems[0].Equipment[0].Description;
            Assert.AreEqual(edit, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Equipment_Quantity()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            int edit = 3;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].Quantity = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            int actual = Bid.Systems[0].Equipment[0].Quantity;
            Assert.AreEqual(edit, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Equipment_SubScope()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECSubScope edit = new TECSubScope();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope.Add(edit);
            var expected = new ObservableCollection<TECSubScope>();
            foreach (TECSubScope item in Bid.Systems[0].Equipment[0].SubScope)
            {
                expected.Add(item);
            }
            testStack.Undo();
            testStack.Redo();

            //assert
            ObservableCollection<TECSubScope> actual = Bid.Systems[0].Equipment[0].SubScope;
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
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope[0].Name = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            string actual = Bid.Systems[0].Equipment[0].SubScope[0].Name;
            Assert.AreEqual(edit, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_SubScope_Description()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope[0].Description = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            string actual = Bid.Systems[0].Equipment[0].SubScope[0].Description;
            Assert.AreEqual(edit, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_SubScope_Quantity()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            int edit = 3;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope[0].Quantity = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            int actual = Bid.Systems[0].Equipment[0].SubScope[0].Quantity;
            Assert.AreEqual(edit, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_SubScope_Points()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECPoint edit = new TECPoint();
            edit.Type = PointTypes.AI;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope[0].Points.Add(edit);
            var expected = new ObservableCollection<TECPoint>();
            foreach (TECPoint item in Bid.Systems[0].Equipment[0].SubScope[0].Points)
            {
                expected.Add(item);
            }
            testStack.Undo();
            testStack.Redo();

            //assert
            ObservableCollection<TECPoint> actual = Bid.Systems[0].Equipment[0].SubScope[0].Points;
            Assert.AreEqual(expected.Count, actual.Count, "Not Redone");

        }

        [TestMethod]
        public void Redo_SubScope_Device()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECConnectionType> types = new ObservableCollection<TECConnectionType>();
            types.Add(Bid.Catalogs.ConnectionTypes[0]);
            TECDevice edit = new TECDevice(types, Bid.Catalogs.Manufacturers[0]);

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope[0].Devices.Add(edit);
            var expected = new ObservableCollection<TECDevice>();
            foreach (TECDevice item in Bid.Systems[0].Equipment[0].SubScope[0].Devices)
            {
                expected.Add(item);
            }
            testStack.Undo();
            testStack.Redo();

            //assert
            ObservableCollection<TECDevice> actual = Bid.Systems[0].Equipment[0].SubScope[0].Devices;
            Assert.AreEqual(expected.Count, actual.Count, "Not Redone");

        }

        #endregion

        #region Device
        [TestMethod]
        public void Redo_Device_Name()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope[0].Devices[0].Name = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            string actual = Bid.Systems[0].Equipment[0].SubScope[0].Devices[0].Name;
            Assert.AreEqual(edit, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Device_Description()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope[0].Devices[0].Description = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            string actual = Bid.Systems[0].Equipment[0].SubScope[0].Devices[0].Description;
            Assert.AreEqual(edit, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Device_Cost()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            double edit = 123;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope[0].Devices[0].Cost = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            double actual = Bid.Systems[0].Equipment[0].SubScope[0].Devices[0].Cost;
            Assert.AreEqual(edit, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Device_Manufacturer()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECManufacturer edit = new TECManufacturer();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope[0].Devices[0].Manufacturer = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            TECManufacturer actual = Bid.Systems[0].Equipment[0].SubScope[0].Devices[0].Manufacturer;
            Assert.AreEqual(edit, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Device_Quantity()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            int edit = 123;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope[0].Devices[0].Quantity = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            int actual = Bid.Systems[0].Equipment[0].SubScope[0].Devices[0].Quantity;
            Assert.AreEqual(edit, actual, "Not Redone");

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
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Name = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            string actual = Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Name;
            Assert.AreEqual(edit, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Point_Description()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Description = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            string actual = Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Description;
            Assert.AreEqual(edit, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Point_Quantity()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            int edit = 3;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            int beforeCount = testStack.UndoStack.Count;
            Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Quantity = edit;
            Assert.AreEqual((beforeCount + 1), testStack.UndoStack.Count, "Not added to undo stack");
            testStack.Undo();
            testStack.Redo();

            //assert
            int actual = Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Quantity;
            Assert.AreEqual(edit, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Point_Type()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            PointTypes edit = PointTypes.AO;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Type = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            PointTypes actual = Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Type;
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
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Panels[0].Name = edit;
            testStack.Undo();
            testStack.Redo();

            //assert
            string actual = Bid.Panels[0].Name;
            Assert.AreEqual(edit, actual, "Not Redone");

        }

        [TestMethod]
        public void Redo_Panel_PanelType()
        {
            //Arrange
            var Bid = TestHelper.CreateTestBid();
            TECPanelType edit = new TECPanelType();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Panels[0].Type = edit;
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
