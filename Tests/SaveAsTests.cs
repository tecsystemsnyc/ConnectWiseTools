using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tests.TestHelper;

namespace Tests
{
    [TestClass]
    public class SaveAsTests
    {
        [TestMethod]
        public void SaveAs_Bid_Info()
        {
            //Arrange
            TECBid expectedBid = CreateTestBid();

            if (File.Exists(DynamicTestBidPath))
            {
                File.Delete(DynamicTestBidPath);
            }

            //Act
            EstimatingLibraryDatabase.SaveBidToNewDB(DynamicTestBidPath, expectedBid);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(DynamicTestBidPath, new TECTemplates());

            //Assert
            Assert.AreEqual(expectedBid.Name, actualBid.Name);
            Assert.AreEqual(expectedBid.BidNumber, actualBid.BidNumber);
            Assert.AreEqual(expectedBid.DueDate, actualBid.DueDate);
            Assert.AreEqual(expectedBid.Salesperson, actualBid.Salesperson);
            Assert.AreEqual(expectedBid.Estimator, actualBid.Estimator);
        }

        [TestMethod]
        public void SaveAs_Bid_System()
        {

        }
    }
}
