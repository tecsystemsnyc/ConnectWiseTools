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

            string path = Path.GetTempFileName();

            //Act
            EstimatingLibraryDatabase.SaveBidToNewDB(path, expectedBid);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            //Assert
            Assert.AreEqual(expectedBid.Name, actualBid.Name);
            Assert.AreEqual(expectedBid.BidNumber, actualBid.BidNumber);
            Assert.AreEqual(expectedBid.DueDate, actualBid.DueDate);
            Assert.AreEqual(expectedBid.Salesperson, actualBid.Salesperson);
            Assert.AreEqual(expectedBid.Estimator, actualBid.Estimator);

            File.Delete(path);
        }

        [TestMethod]
        public void SaveAs_Bid_System()
        {
            //Arrange
            TECBid expectedbid = CreateTestBid();

            TECSystem expectedSystem = expectedbid.Systems[0];

            string path = Path.GetTempFileName();

            //Act
            EstimatingLibraryDatabase.SaveBidToNewDB(path, expectedbid);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECSystem actualSystem = actualBid.Systems[0];

            //Assert
            Assert.AreEqual(expectedSystem.Name, actualSystem.Name);
            Assert.AreEqual(expectedSystem.Description, actualSystem.Description);
            Assert.AreEqual(expectedSystem.Quantity, actualSystem.Quantity);
            Assert.AreEqual(expectedSystem.BudgetPrice, actualSystem.BudgetPrice);

            File.Delete(path);
        }
    }
}
