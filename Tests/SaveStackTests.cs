using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EstimatingLibrary;
using EstimatingUtilitiesLibrary;

namespace Tests
{
    [TestClass]
    public class SaveStackTests
    {
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

       
        public void checkStackItem(StackItem expectedItem, StackItem actualItem)
        {
            Assert.AreEqual(expectedItem.Change, actualItem.Change);
            Assert.AreEqual(expectedItem.ReferenceObject, actualItem.ReferenceObject);
            Assert.AreEqual(expectedItem.TargetObject, actualItem.TargetObject);
            Assert.AreEqual(expectedItem.ReferenceType, actualItem.ReferenceType);
            Assert.AreEqual(expectedItem.TargetType, actualItem.TargetType);
        }
    }
}
