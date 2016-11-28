using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tests.TestHelper;

namespace Tests
{
    [TestClass]
    public class SaveTests
    {
        [TestMethod]
        public void SaveAs_Bid()
        {
            //Arrange
            TECBid expectedBid = CreateTestBid();
            TECSystem expectedSystem = expectedBid.Systems[0];
            TECEquipment expectedEquipment = expectedSystem.Equipment[0];
            TECSubScope expectedSubScope = expectedEquipment.SubScope[0];
            TECDevice expectedDevice = expectedSubScope.Devices[0];
            TECPoint expectedPoint = expectedSubScope.Points[0];

            string path = Path.GetTempFileName();

            //Act
            EstimatingLibraryDatabase.SaveBidToNewDB(path, expectedBid);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            TECSystem actualSystem = actualBid.Systems[0];
            TECEquipment actualEquipment = actualSystem.Equipment[0];
            TECSubScope actualSubScope = actualEquipment.SubScope[0];
            TECDevice actualDevice = actualSubScope.Devices[0];
            TECPoint actualPoint = actualSubScope.Points[0];

            //Assert
            Assert.AreEqual(expectedBid.Name, actualBid.Name);
            Assert.AreEqual(expectedBid.BidNumber, actualBid.BidNumber);
            Assert.AreEqual(expectedBid.DueDate, actualBid.DueDate);
            Assert.AreEqual(expectedBid.Salesperson, actualBid.Salesperson);
            Assert.AreEqual(expectedBid.Estimator, actualBid.Estimator);

            Assert.AreEqual(expectedSystem.Name, actualSystem.Name);
            Assert.AreEqual(expectedSystem.Description, actualSystem.Description);
            Assert.AreEqual(expectedSystem.Quantity, actualSystem.Quantity);
            Assert.AreEqual(expectedSystem.BudgetPrice, actualSystem.BudgetPrice);

            Assert.AreEqual(expectedEquipment.Name, actualEquipment.Name);
            Assert.AreEqual(expectedEquipment.Description, actualEquipment.Description);
            Assert.AreEqual(expectedEquipment.Quantity, actualEquipment.Quantity);
            Assert.AreEqual(expectedEquipment.BudgetPrice, actualEquipment.BudgetPrice);

            Assert.AreEqual(expectedSubScope.Name, actualSubScope.Name);
            Assert.AreEqual(expectedSubScope.Description, actualSubScope.Description);
            Assert.AreEqual(expectedSubScope.Quantity, actualSubScope.Quantity);

            Assert.AreEqual(expectedDevice.Name, actualDevice.Name);
            Assert.AreEqual(expectedDevice.Description, actualDevice.Description);
            Assert.AreEqual(expectedDevice.Quantity, actualDevice.Quantity);
            Assert.AreEqual(expectedDevice.Cost, actualDevice.Cost);
            Assert.AreEqual(expectedDevice.Wire, actualDevice.Wire);

            Assert.AreEqual(expectedPoint.Name, actualPoint.Name);
            Assert.AreEqual(expectedPoint.Description, actualPoint.Description);
            Assert.AreEqual(expectedPoint.Quantity, actualPoint.Quantity);
            Assert.AreEqual(expectedPoint.Type, actualPoint.Type);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        #region Save BidInfo
        [TestMethod]
        public void Save_BidInfo_Name()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            string expectedName = "Save Name";
            bid.Name = expectedName;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            string actualName = actualBid.Name;

            //Assert
            Assert.AreEqual(expectedName, actualName);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        [TestMethod]
        public void Save_BidInfo_BidNo()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            string expectedBidNo = "Save BidNo";
            bid.BidNumber = expectedBidNo;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            string actualBidNo = actualBid.BidNumber;

            //Assert
            Assert.AreEqual(expectedBidNo, actualBidNo);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        [TestMethod]
        public void Save_BidInfo_DueDate()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            DateTime expectedDueDate = DateTime.Now;
            bid.DueDate = expectedDueDate;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            DateTime actualDueDate = actualBid.DueDate;

            //Assert
            Assert.AreEqual(expectedDueDate, actualDueDate);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        [TestMethod]
        public void Save_BidInfo_Salesperson()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            string expectedSalesperson = "Save Salesperson";
            bid.Salesperson = expectedSalesperson;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            string actualSalesperson = actualBid.Salesperson;

            //Assert
            Assert.AreEqual(expectedSalesperson, actualSalesperson);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        [TestMethod]
        public void Save_BidInfo_Estimator()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            string expectedEstimator = "Save Estimator";
            bid.Estimator = expectedEstimator;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            string actualEstimator = actualBid.Estimator;

            //Assert
            Assert.AreEqual(expectedEstimator, actualEstimator);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }
        #endregion Save BidInfo

        #region Save System
        [TestMethod]
        public void Save_Bid_Add_System()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            TECSystem expectedSystem = new TECSystem("New system", "New system desc", 123.5, new ObservableCollection<TECEquipment>());
            expectedSystem.Quantity = 1235;

            bid.Systems.Add(expectedSystem);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECSystem actualSystem = null;
            foreach (TECSystem system in actualBid.Systems)
            {
                if (expectedSystem.Guid == system.Guid)
                {
                    actualSystem = system;
                }
            }

            //Assert
            Assert.IsNotNull(actualSystem);

            Assert.AreEqual(expectedSystem.Name, actualSystem.Name);
            Assert.AreEqual(expectedSystem.Description, actualSystem.Description);
            Assert.AreEqual(expectedSystem.Quantity, actualSystem.Quantity);
            Assert.AreEqual(expectedSystem.BudgetPrice, actualSystem.BudgetPrice);
        }

        [TestMethod]
        public void Save_Bid_Remove_System()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            int oldNumSystems = bid.Systems.Count;
            TECSystem systemToRemove = bid.Systems[0];

            bid.Systems.Remove(systemToRemove);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECBid finalBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            //Assert
            foreach (TECSystem system in finalBid.Systems)
            {
                if (system.Guid == systemToRemove.Guid)
                {
                    Assert.Fail();
                }
            }

            Assert.AreEqual((oldNumSystems - 1), bid.Systems.Count);
        }

        #region Edit System
        [TestMethod]
        public void Save_Bid_System_Name()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            string expectedName = "Save System Name";
            bid.Systems[0].Name = expectedName;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            string actualName = actualBid.Systems[0].Name;

            //Assert
            Assert.AreEqual(expectedName, actualName);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        [TestMethod]
        public void Save_Bid_System_Description()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            string expectedDescription = "Save System Description";
            bid.Systems[0].Description = expectedDescription;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            string actualDescription = actualBid.Systems[0].Description;

            //Assert
            Assert.AreEqual(expectedDescription, actualDescription);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        [TestMethod]
        public void Save_Bid_System_Quantity()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            int expectedQuantity = 987654321;
            bid.Systems[0].Quantity = expectedQuantity;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            int actualQuantity = actualBid.Systems[0].Quantity;

            //Assert
            Assert.AreEqual(expectedQuantity, actualQuantity);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        [TestMethod]
        public void Save_Bid_System_BudgetPrice()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            double expectedBudgetPrice = 987654321;
            bid.Systems[0].BudgetPrice = expectedBudgetPrice;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            double actualBudgetPrice = actualBid.Systems[0].BudgetPrice;

            //Assert
            Assert.AreEqual(expectedBudgetPrice, actualBudgetPrice);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }
        #endregion Edit System
        #endregion Save System

        #region Save Equipment
        [TestMethod]
        public void Save_Bid_Add_Equipment()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            TECEquipment expectedEquipment = new TECEquipment("New Equipment", "New Description", 465543.54, new ObservableCollection<TECSubScope>());
            expectedEquipment.Quantity = 46554354;

            bid.Systems[0].Equipment.Add(expectedEquipment);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECEquipment actualEquipment = null;
            foreach (TECEquipment equip in actualBid.Systems[0].Equipment)
            {
                if (expectedEquipment.Guid == equip.Guid)
                {
                    actualEquipment = equip;
                }
            }

            //Assert
            Assert.IsNotNull(actualEquipment);

            Assert.AreEqual(expectedEquipment.Name, actualEquipment.Name);
            Assert.AreEqual(expectedEquipment.Description, actualEquipment.Description);
            Assert.AreEqual(expectedEquipment.Quantity, actualEquipment.Quantity);
            Assert.AreEqual(expectedEquipment.BudgetPrice, actualEquipment.BudgetPrice);
        }

        #endregion Save Equipment
    }
}
