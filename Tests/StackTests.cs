﻿using System;
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

    }
}