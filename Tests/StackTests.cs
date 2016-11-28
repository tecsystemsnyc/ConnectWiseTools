using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using System.Collections.ObjectModel;

namespace Tests
{
    [TestClass]
    public class StackTests
    {

        public TECBid Bid = new TECBid();


        #region Undo
        [TestMethod]
        public void Undo_Bid_Name()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            string expected = Bid.Name;
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Name = edit;
            testStack.Undo();

            //assert
            string actual = Bid.Name;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_Number()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            string expected = Bid.BidNumber;
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.BidNumber = edit;
            testStack.Undo();

            //assert
            string actual = Bid.BidNumber;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_DueDate()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            DateTime expected = Bid.DueDate;
            DateTime edit = new DateTime(2000, 1, 1);

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.DueDate = edit;
            testStack.Undo();

            //assert
            DateTime actual = Bid.DueDate;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_Salesperson()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            string expected = Bid.Salesperson;
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Salesperson = edit;
            testStack.Undo();

            //assert
            string actual = Bid.Salesperson;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_Estimator()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            string expected = Bid.Estimator;
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Estimator = edit;
            testStack.Undo();

            //assert
            string actual = Bid.Estimator;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_Labor()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            TECLabor expected = Bid.Labor;
            TECLabor edit = new TECLabor();
            edit.CommCoef = 1.1;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Labor = edit;
            testStack.Undo();

            //assert
            TECLabor actual = Bid.Labor;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_ScopeTree()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECScopeBranch> expected = Bid.ScopeTree;
            TECScopeBranch edit = new TECScopeBranch();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.ScopeTree.Add(edit);
            testStack.Undo();

            //assert
            ObservableCollection<TECScopeBranch> actual = Bid.ScopeTree;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_Systems()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECSystem> expected = Bid.Systems;
            TECSystem edit = new TECSystem();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems.Add(edit);
            testStack.Undo();

            //assert
            ObservableCollection<TECSystem> actual = Bid.Systems;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_DeviceCatalog()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECDevice> expected = Bid.DeviceCatalog;
            TECDevice edit = new TECDevice();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.DeviceCatalog.Add(edit);
            testStack.Undo();

            //assert
            ObservableCollection<TECDevice> actual = Bid.DeviceCatalog;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_ManufacturerCatalog()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECManufacturer> expected = Bid.ManufacturerCatalog;
            TECManufacturer edit = new TECManufacturer();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.ManufacturerCatalog.Add(edit);
            testStack.Undo();

            //assert
            ObservableCollection<TECManufacturer> actual = Bid.ManufacturerCatalog;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_Notes()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECNote> expected = Bid.Notes;
            TECNote edit = new TECNote();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Notes.Add(edit);
            testStack.Undo();

            //assert
            ObservableCollection<TECNote> actual = Bid.Notes;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_Exclusions()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECExclusion> expected = Bid.Exclusions;
            TECExclusion edit = new TECExclusion();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Exclusions.Add(edit);
            testStack.Undo();

            //assert
            ObservableCollection<TECExclusion> actual = Bid.Exclusions;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_Tags()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECTag> expected = Bid.Tags;
            TECTag edit = new TECTag();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Tags.Add(edit);
            testStack.Undo();

            //assert
            ObservableCollection<TECTag> actual = Bid.Tags;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Bid_Drawings()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECDrawing> expected = Bid.Drawings;
            TECDrawing edit = new TECDrawing("This");

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Drawings.Add(edit);
            testStack.Undo();

            //assert
            ObservableCollection<TECDrawing> actual = Bid.Drawings;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_System_Name()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            string expected = Bid.Systems[0].Name;
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Name = edit;
            testStack.Undo();

            //assert
            string actual = Bid.Systems[0].Name;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_System_Description()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            string expected = Bid.Systems[0].Description;
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Description = edit;
            testStack.Undo();

            //assert
            string actual = Bid.Systems[0].Description;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_System_Quantity()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            int expected = Bid.Systems[0].Quantity;
            int edit = 3;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Quantity = edit;
            testStack.Undo();

            //assert
            int actual = Bid.Systems[0].Quantity;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_System_Equipment()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECEquipment> expected = Bid.Systems[0].Equipment;
            TECEquipment edit = new TECEquipment();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment.Add(edit);
            testStack.Undo();

            //assert
            ObservableCollection<TECEquipment> actual = Bid.Systems[0].Equipment;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Equipment_Name()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            string expected = Bid.Systems[0].Equipment[0].Name;
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].Name = edit;
            testStack.Undo();

            //assert
            string actual = Bid.Systems[0].Equipment[0].Name;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Equipment_Description()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            string expected = Bid.Systems[0].Equipment[0].Description;
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].Description = edit;
            testStack.Undo();

            //assert
            string actual = Bid.Systems[0].Equipment[0].Description;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Equipment_Quantity()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            int expected = Bid.Systems[0].Equipment[0].Quantity;
            int edit = 3;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].Quantity = edit;
            testStack.Undo();

            //assert
            int actual = Bid.Systems[0].Equipment[0].Quantity;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Equipment_SubScope()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECSubScope> expected = Bid.Systems[0].Equipment[0].SubScope;
            TECSubScope edit = new TECSubScope();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope.Add(edit);
            testStack.Undo();

            //assert
            ObservableCollection<TECSubScope> actual = Bid.Systems[0].Equipment[0].SubScope;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_SubScope_Name()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            string expected = Bid.Systems[0].Equipment[0].SubScope[0].Name;
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope[0].Name = edit;
            testStack.Undo();

            //assert
            string actual = Bid.Systems[0].Equipment[0].SubScope[0].Name;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_SubScope_Description()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            string expected = Bid.Systems[0].Equipment[0].SubScope[0].Description;
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope[0].Description = edit;
            testStack.Undo();

            //assert
            string actual = Bid.Systems[0].Equipment[0].SubScope[0].Description;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_SubScope_Quantity()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            int expected = Bid.Systems[0].Equipment[0].SubScope[0].Quantity;
            int edit = 3;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope[0].Quantity = edit;
            testStack.Undo();

            //assert
            int actual = Bid.Systems[0].Equipment[0].SubScope[0].Quantity;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_SubScope_Points()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECPoint> expected = Bid.Systems[0].Equipment[0].SubScope[0].Points;
            TECPoint edit = new TECPoint();
            edit.Type = PointTypes.AI;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope[0].Points.Add(edit);
            testStack.Undo();

            //assert
            ObservableCollection<TECPoint> actual = Bid.Systems[0].Equipment[0].SubScope[0].Points;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_SubScope_Device()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            ObservableCollection<TECDevice> expected = Bid.Systems[0].Equipment[0].SubScope[0].Devices;
            TECDevice edit = new TECDevice();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope[0].Devices.Add(edit);
            testStack.Undo();

            //assert
            ObservableCollection<TECDevice> actual = Bid.Systems[0].Equipment[0].SubScope[0].Devices;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Device_Name()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            string expected = Bid.Systems[0].Equipment[0].SubScope[0].Devices[0].Name;
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope[0].Devices[0].Name = edit;
            testStack.Undo();

            //assert
            string actual = Bid.Systems[0].Equipment[0].SubScope[0].Devices[0].Name;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Device_Description()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            string expected = Bid.Systems[0].Equipment[0].SubScope[0].Devices[0].Description;
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope[0].Devices[0].Description = edit;
            testStack.Undo();

            //assert
            string actual = Bid.Systems[0].Equipment[0].SubScope[0].Devices[0].Description;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Device_Cost()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            double expected = Bid.Systems[0].Equipment[0].SubScope[0].Devices[0].Cost;
            double edit = 123;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope[0].Devices[0].Cost = edit;
            testStack.Undo();

            //assert
            double actual = Bid.Systems[0].Equipment[0].SubScope[0].Devices[0].Cost;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Device_Manufacturer()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            TECManufacturer expected = Bid.Systems[0].Equipment[0].SubScope[0].Devices[0].Manufacturer;
            TECManufacturer edit = new TECManufacturer();

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope[0].Devices[0].Manufacturer = edit;
            testStack.Undo();

            //assert
            TECManufacturer actual = Bid.Systems[0].Equipment[0].SubScope[0].Devices[0].Manufacturer;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Point_Name()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            string expected = Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Name;
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Name = edit;
            testStack.Undo();

            //assert
            string actual = Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Name;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Point_Description()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            string expected = Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Description;
            string edit = "Edit";

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Description = edit;
            testStack.Undo();

            //assert
            string actual = Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Description;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Point_Quantity()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            int expected = Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Quantity;
            int edit = 3;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Quantity = edit;
            testStack.Undo();

            //assert
            int actual = Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Quantity;
            Assert.AreEqual(expected, actual, "Not Undone");

        }

        [TestMethod]
        public void Undo_Point_Type()
        {
            //Arrange
            Bid = TestHelper.CreateTestBid();
            PointTypes expected = Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Type;
            PointTypes edit = PointTypes.AO;

            //Act
            ChangeStack testStack = new ChangeStack(Bid);
            Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Type = edit;
            testStack.Undo();

            //assert
            PointTypes actual = Bid.Systems[0].Equipment[0].SubScope[0].Points[0].Type;
            Assert.AreEqual(expected, actual, "Not Undone");

        }
        #endregion
    }
}
