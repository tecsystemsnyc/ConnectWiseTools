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
            bid.Systems.Add(new TECSystem());

            //Act
            ChangeStack stack = new ChangeStack(bid);

            system.AddInstance(bid);

            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
        }
    }
}
