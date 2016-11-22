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
        private string testBidPath = Environment.CurrentDirectory + @"\Test Files\UnitTestBid.bdb";

        [TestMethod]
        public void Load_Bid_Info()
        {

        }

        [TestMethod]
        public void Load_Bid_System()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECSystem system = new TECSystem();

            //Act
            bid = EstimatingLibraryDatabase.LoadDBToBid(testBidPath, new TECTemplates());
            if (bid.Systems.Count > 0)
            {
                system = bid.Systems[0];
            }
            else
            {
                Assert.Fail("No systems loaded from path: " + testBidPath);
            }

            //Assert
            string actual = system.Name;
            string expected = "Test System";
            string failMessage = "Name test failed. Test bid path: " + testBidPath;
            Assert.AreEqual(expected, actual, failMessage);

            actual = system.Description;
            expected = "Test System Description";
            failMessage = "Description test failed. Test bid path: " + testBidPath;
            Assert.AreEqual(expected, actual, failMessage);

            actual = system.Quantity.ToString();
            expected = "123";
            failMessage = "Quantity test failed. Test bid path: " + testBidPath;
            Assert.AreEqual(expected, actual, failMessage);

            actual = system.BudgetPrice.ToString();
            expected = "123";
            failMessage = "Budget Price test failed. Test bid path: " + testBidPath;
            Assert.AreEqual(expected, actual, failMessage);
        }


    }
}
