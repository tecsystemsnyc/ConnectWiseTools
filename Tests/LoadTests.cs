using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class LoadTests
    {
        [TestMethod]
        public void Load_Bid_Info()
        {
            //Act
            TECBid bid = TestHelper.LoadTestBid();

            //Assert
            string actualName = bid.Name;
            string expectedName = "Unit Testimate";
            string failMessage = "Name test failed.";
            Assert.AreEqual(expectedName, actualName, failMessage);

            string actualNumber = bid.BidNumber;
            string expectedNumber = "1701-117";
            failMessage = "Bid Number test failed.";
            Assert.AreEqual(expectedNumber, actualNumber, failMessage);

            string actualSales = bid.Salesperson;
            string expectedSales = "Mrs. Test";
            failMessage = "Salesperson test failed.";
            Assert.AreEqual(expectedSales, actualSales, failMessage);

            string actualEstimator = bid.Estimator;
            string expectedEstimator = "Mr. Test";
            failMessage = "Estimator test failed.";
            Assert.AreEqual(expectedEstimator, actualEstimator, failMessage);
        }

        [TestMethod]
        public void Load_Bid_System()
        {
            //Arrange
            TECSystem system = new TECSystem();

            //Act
            TECBid bid = TestHelper.LoadTestBid();
            if (bid.Systems.Count > 0)
            {
                system = bid.Systems[0];
            }
            else
            {
                Assert.Fail("No systems loaded.");
            }

            //Assert
            string actualName = system.Name;
            string expectedName = "Test System";
            string failMessage = "Name test failed.";
            Assert.AreEqual(expectedName, actualName, failMessage);

            string actualDescription = system.Description;
            string expectedDescription = "Test System Description";
            failMessage = "Description test failed.";
            Assert.AreEqual(expectedDescription, actualDescription, failMessage);

            int actualQuantity = system.Quantity;
            int expectedQuantity = 123;
            failMessage = "Quantity test failed.";
            Assert.AreEqual(expectedQuantity, actualQuantity, failMessage);

            double actualBP = system.BudgetPrice;
            double expectedBP = 123;
            failMessage = "Budget Price test failed.";
            Assert.AreEqual(expectedBP, actualBP, failMessage);
        }

        [TestMethod]
        public void Load_Bid_Equipment()
        {
            //Arrange
            TECEquipment equipment = new TECEquipment();
        }
    }
}
