using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class SaveAsTests
    {
        static TECBid expectedBid;
        static TECSystem expectedSystem;
        static TECEquipment expectedEquipment;
        static TECSubScope expectedSubScope;
        static TECDevice expectedDevice;
        static TECPoint expectedPoint;

        static string path;

        static TECBid actualBid;
        static TECSystem actualSystem;
        static TECEquipment actualEquipment;
        static TECSubScope actualSubScope;
        static TECDevice actualDevice;
        static TECPoint actualPoint;

        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext TestContext)
        {
            //Arrange
            expectedBid = TestHelper.CreateTestBid();
            expectedSystem = expectedBid.Systems[0];
            expectedEquipment = expectedSystem.Equipment[0];
            expectedSubScope = expectedEquipment.SubScope[0];
            expectedDevice = expectedSubScope.Devices[0];
            expectedPoint = expectedSubScope.Points[0];

            path = Path.GetTempFileName();

            //Act
            EstimatingLibraryDatabase.SaveBidToNewDB(path, expectedBid);

            actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            actualSystem = actualBid.Systems[0];
            actualEquipment = actualSystem.Equipment[0];
            actualSubScope = actualEquipment.SubScope[0];
            actualDevice = actualSubScope.Devices[0];
            actualPoint = actualSubScope.Points[0];
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        [TestMethod]
        public void SaveAs_Bid_Info()
        {
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
            //Assert
            Assert.AreEqual(expectedSystem.Name, actualSystem.Name);
            Assert.AreEqual(expectedSystem.Description, actualSystem.Description);
            Assert.AreEqual(expectedSystem.Quantity, actualSystem.Quantity);
            Assert.AreEqual(expectedSystem.BudgetPrice, actualSystem.BudgetPrice);
        }

        [TestMethod]
        public void SaveAs_Bid_Equipment()
        {
            //Assert
            Assert.AreEqual(expectedEquipment.Name, actualEquipment.Name);
            Assert.AreEqual(expectedEquipment.Description, actualEquipment.Description);
            Assert.AreEqual(expectedEquipment.Quantity, actualEquipment.Quantity);
            Assert.AreEqual(expectedEquipment.BudgetPrice, actualEquipment.BudgetPrice);
        }

        [TestMethod]
        public void SaveAs_Bid_SubScope()
        {
            //Assert
            Assert.AreEqual(expectedSubScope.Name, actualSubScope.Name);
            Assert.AreEqual(expectedSubScope.Description, actualSubScope.Description);
            Assert.AreEqual(expectedSubScope.Quantity, actualSubScope.Quantity);
        }

        [TestMethod]
        public void SaveAs_Bid_Device()
        {
            //Assert
            Assert.AreEqual(expectedDevice.Name, actualDevice.Name);
            Assert.AreEqual(expectedDevice.Description, actualDevice.Description);
            Assert.AreEqual(expectedDevice.Quantity, actualDevice.Quantity);
            Assert.AreEqual(expectedDevice.Cost, actualDevice.Cost);
            Assert.AreEqual(expectedDevice.Wire, actualDevice.Wire);
        }

        [TestMethod]
        public void SaveAs_Bid_Point()
        {
            //Assert
            Assert.AreEqual(expectedPoint.Name, actualPoint.Name);
            Assert.AreEqual(expectedPoint.Description, actualPoint.Description);
            Assert.AreEqual(expectedPoint.Quantity, actualPoint.Quantity);
            Assert.AreEqual(expectedPoint.Type, actualPoint.Type);
        }
    }
}
