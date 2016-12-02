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
        static TECLabor expectedLabor;
        static TECSystem expectedSystem;
        static TECSystem expectedSystem1;
        static TECEquipment expectedEquipment;
        static TECSubScope expectedSubScope;
        static TECDevice expectedDevice;
        static TECPoint expectedPoint;

        static string path;

        static TECBid actualBid;
        static TECLabor actualLabor;
        static TECSystem actualSystem;
        static TECSystem actualSystem1;
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
            expectedLabor = expectedBid.Labor;
            expectedSystem = expectedBid.Systems[0];
            expectedSystem1 = expectedBid.Systems[1];
            expectedEquipment = expectedSystem.Equipment[0];
            expectedSubScope = expectedEquipment.SubScope[0];
            expectedDevice = expectedSubScope.Devices[0];
            expectedPoint = expectedSubScope.Points[0];

            path = Path.GetTempFileName();

            //Act
            EstimatingLibraryDatabase.SaveBidToNewDB(path, expectedBid);

            actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            actualLabor = actualBid.Labor;

            foreach (TECSystem sys in actualBid.Systems)
            {
                if (sys.Guid == expectedSystem.Guid)
                {
                    actualSystem = sys;
                }
                else if (sys.Guid == expectedSystem1.Guid)
                {
                    actualSystem1 = sys;
                }
                if (actualSystem != null && actualSystem1 != null)
                {
                    break;
                }
            }

            foreach (TECEquipment equip in actualSystem.Equipment)
            {
                if (equip.Guid == expectedEquipment.Guid)
                {
                    actualEquipment = equip;
                    break;
                }
            }

            foreach (TECSubScope ss in actualEquipment.SubScope)
            {
                if (ss.Guid == expectedSubScope.Guid)
                {
                    actualSubScope = ss;
                    break;
                }
            }

            foreach (TECDevice dev in actualSubScope.Devices)
            {
                if (dev.Guid == expectedDevice.Guid)
                {
                    actualDevice = dev;
                    break;
                }
            }

            foreach (TECPoint point in actualSubScope.Points)
            {
                if (point.Guid == expectedPoint.Guid)
                {
                    actualPoint = point;
                    break;
                }
            }
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
            //Console.WriteLine("SaveAs test bid saved to: " + path);
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
        public void SaveAs_Bid_Labor()
        {
            //Assert
            Assert.AreEqual(expectedLabor.PMCoef, actualLabor.PMCoef);
            Assert.AreEqual(expectedLabor.ENGCoef, actualLabor.ENGCoef);
            Assert.AreEqual(expectedLabor.CommCoef, actualLabor.CommCoef);
            Assert.AreEqual(expectedLabor.SoftCoef, actualLabor.SoftCoef);
            Assert.AreEqual(expectedLabor.GraphCoef, actualLabor.GraphCoef);
            Assert.AreEqual(expectedLabor.ElectricalRate, actualLabor.ElectricalRate);
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

        [TestMethod]
        public void SaveAs_Bid_Location()
        {
            //Assert
            Assert.AreEqual(expectedSystem.Location.Guid, actualSystem.Location.Guid);
            Assert.AreEqual(expectedSystem1.Location.Guid, actualSystem1.Location.Guid);
            Assert.AreEqual(expectedEquipment.Location.Guid, actualEquipment.Location.Guid);
            Assert.AreEqual(expectedSubScope.Location.Guid, actualSubScope.Location.Guid);

            //In CreateTestBid, the first system has the same location as its equipment.
            Assert.AreEqual(expectedSystem.Location, expectedEquipment.Location);
        }
    }
}
