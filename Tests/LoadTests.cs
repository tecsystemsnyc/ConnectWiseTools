using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tests.TestHelper;

namespace Tests
{
    [TestClass]
    public class LoadTests
    {
        [TestMethod]
        public void Load_Bid_Info()
        {
            //Act
            TECBid bid = LoadTestBid(StaticTestBidPath);

            //Assert
            string actualName = bid.Name;
            string expectedName = "Unit Testimate";
            Assert.AreEqual(expectedName, actualName);

            string actualNumber = bid.BidNumber;
            string expectedNumber = "1701-117";
            Assert.AreEqual(expectedNumber, actualNumber);

            DateTime actualDueDate = bid.DueDate;
            DateTime expectedDueDate = new DateTime(1969, 7, 20);
            Assert.AreEqual(expectedDueDate, actualDueDate);

            string actualSales = bid.Salesperson;
            string expectedSales = "Mrs. Test";
            Assert.AreEqual(expectedSales, actualSales);

            string actualEstimator = bid.Estimator;
            string expectedEstimator = "Mr. Test";
            Assert.AreEqual(expectedEstimator, actualEstimator);
        }

        [TestMethod]
        public void Load_Bid_System()
        {
            //Arrange
            TECSystem system = LoadTestSystem(StaticTestBidPath);

            //Assert
            string actualName = system.Name;
            string expectedName = "Test System";
            Assert.AreEqual(expectedName, actualName);

            string actualDescription = system.Description;
            string expectedDescription = "Test System Description";
            Assert.AreEqual(expectedDescription, actualDescription);

            int actualQuantity = system.Quantity;
            int expectedQuantity = 123;
            Assert.AreEqual(expectedQuantity, actualQuantity);

            double actualBP = system.BudgetPrice;
            double expectedBP = 123;
            Assert.AreEqual(expectedBP, actualBP);
        }

        [TestMethod]
        public void Load_Bid_Equipment()
        {
            //Arrange
            TECEquipment equipment = LoadTestEquipment(StaticTestBidPath);

            //Assert
            string actualName = equipment.Name;
            string expectedName = "Test Equipment";
            Assert.AreEqual(expectedName, actualName);

            string actualDescription = equipment.Description;
            string expectedDescription = "Test Equipment Description";
            Assert.AreEqual(expectedDescription, actualDescription);

            int actualQuantity = equipment.Quantity;
            int expectedQuantity = 456;
            Assert.AreEqual(expectedQuantity, actualQuantity);

            double actualBP = equipment.BudgetPrice;
            double expectedBP = 456;
            Assert.AreEqual(expectedBP, actualBP);
        }

        [TestMethod]
        public void Load_Bid_SubScope()
        {
            //Arrange
            TECSubScope subScope = LoadTestSubScope(StaticTestBidPath);

            //Assert
            string actualName = subScope.Name;
            string expectedName = "Test SubScope";
            Assert.AreEqual(expectedName, actualName);

            string actualDescription = subScope.Description;
            string expectedDescription = "Test SubScope Description";
            Assert.AreEqual(expectedDescription, actualDescription);

            int actualQuantity = subScope.Quantity;
            int expectedQuantity = 789;
            Assert.AreEqual(expectedQuantity, actualQuantity);
        }

        [TestMethod]
        public void Load_Bid_Device()
        {
            //Arrange
            TECDevice dev = LoadTestDevice(StaticTestBidPath);

            //Assert
            string actualName = dev.Name;
            string expectedName = "Test Device";
            Assert.AreEqual(expectedName, actualName);

            string actualDescription = dev.Description;
            string expectedDescription = "Test Device Description";
            Assert.AreEqual(expectedDescription, actualDescription);

            int actualQuantity = dev.Quantity;
            int expectedQuantity = 987;
            Assert.AreEqual(expectedQuantity, actualQuantity);

            double actualCost = dev.Cost;
            double expectedCost = 654;
            Assert.AreEqual(expectedCost, actualCost);

            string actualWire = dev.Wire;
            string expectedWire = "Test Wire";
            Assert.AreEqual(expectedWire, actualWire);
        }

        [TestMethod]
        public void Load_Bid_Point()
        {
            //Arrange
            TECPoint point = LoadTestPoint(StaticTestBidPath);

            //Assert
            string actualName = point.Name;
            string expectedName = "Test Point";
            Assert.AreEqual(expectedName, actualName);

            string actualDescription = point.Description;
            string expectedDescription = "Test Point Description";
            Assert.AreEqual(expectedDescription, actualDescription);

            int actualQuantity = point.Quantity;
            int expectedQuantity = 321;
            Assert.AreEqual(expectedQuantity, actualQuantity);

            PointTypes actualType = point.Type;
            PointTypes expectedType = PointTypes.Serial;
            Assert.AreEqual(expectedType, actualType);
        }
    }
}
