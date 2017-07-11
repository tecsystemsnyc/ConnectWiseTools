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
