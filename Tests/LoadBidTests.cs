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
    public class LoadBidTests
    {
        static TECBid actualBid;
        static TECSystem actualSystem;
        static TECEquipment actualEquipment;
        static TECSubScope actualSubScope;
        static TECDevice actualDevice;
        static TECPoint actualPoint;

        static TECManufacturer actualManufacturer;

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
            actualBid = LoadTestBid(StaticTestBidPath);
            actualSystem = actualBid.Systems[0];
            actualEquipment = actualSystem.Equipment[0];
            actualSubScope = actualEquipment.SubScope[0];
            actualDevice = actualSubScope.Devices[0];
            actualPoint = actualSubScope.Points[0];

            actualManufacturer = actualBid.ManufacturerCatalog[0];
        }

        [TestMethod]
        public void Load_Bid_Info()
        {
            //Assert
            string expectedName = "Unit Testimate";
            Assert.AreEqual(expectedName, actualBid.Name);
            
            string expectedNumber = "1701-117";
            Assert.AreEqual(expectedNumber, actualBid.BidNumber);
            
            DateTime expectedDueDate = new DateTime(1969, 7, 20);
            Assert.AreEqual(expectedDueDate, actualBid.DueDate);
            
            string expectedSales = "Mrs. Test";
            Assert.AreEqual(expectedSales, actualBid.Salesperson);
            
            string expectedEstimator = "Mr. Test";
            Assert.AreEqual(expectedEstimator, actualBid.Estimator);
        }

        [TestMethod]
        public void Load_Bid_Labor()
        {
            //Assert
            double expectedPM = 2;
            Assert.AreEqual(expectedPM, actualBid.Labor.PMCoef);

            double expectedENG = 3;
            Assert.AreEqual(expectedENG, actualBid.Labor.ENGCoef);

            double expectedComm = 4;
            Assert.AreEqual(expectedComm, actualBid.Labor.CommCoef);

            double expectedSoft = 5;
            Assert.AreEqual(expectedSoft, actualBid.Labor.SoftCoef);

            double expectedGraph = 6;
            Assert.AreEqual(expectedGraph, actualBid.Labor.GraphCoef);

            double expectedElec = 7;
            Assert.AreEqual(expectedElec, actualBid.Labor.ElectricalRate);
        }

        [TestMethod]
        public void Load_Bid_System()
        {
            //Assert
            string expectedName = "Test System";
            Assert.AreEqual(expectedName, actualSystem.Name);
            
            string expectedDescription = "Test System Description";
            Assert.AreEqual(expectedDescription, actualSystem.Description);
            
            int expectedQuantity = 123;
            Assert.AreEqual(expectedQuantity, actualSystem.Quantity);
            
            double expectedBP = 123;
            Assert.AreEqual(expectedBP, actualSystem.BudgetPrice);
        }

        [TestMethod]
        public void Load_Bid_Equipment()
        {
            //Assert
            string expectedName = "Test Equipment";
            Assert.AreEqual(expectedName, actualEquipment.Name);
            
            string expectedDescription = "Test Equipment Description";
            Assert.AreEqual(expectedDescription, actualEquipment.Description);
            
            int expectedQuantity = 456;
            Assert.AreEqual(expectedQuantity, actualEquipment.Quantity);
            
            double expectedBP = 456;
            Assert.AreEqual(expectedBP, actualEquipment.BudgetPrice);
        }

        [TestMethod]
        public void Load_Bid_SubScope()
        {
            //Assert
            string expectedName = "Test SubScope";
            Assert.AreEqual(expectedName, actualSubScope.Name);
            
            string expectedDescription = "Test SubScope Description";
            Assert.AreEqual(expectedDescription, actualSubScope.Description);
            
            int expectedQuantity = 789;
            Assert.AreEqual(expectedQuantity, actualSubScope.Quantity);
        }

        [TestMethod]
        public void Load_Bid_Device()
        {
            //Assert
            string expectedName = "Test Device";
            Assert.AreEqual(expectedName, actualDevice.Name);
            
            string expectedDescription = "Test Device Description";
            Assert.AreEqual(expectedDescription, actualDevice.Description);
            
            int expectedQuantity = 987;
            Assert.AreEqual(expectedQuantity, actualDevice.Quantity);
            
            double expectedCost = 654;
            Assert.AreEqual(expectedCost, actualDevice.Cost);
            
            string expectedWire = "Test Wire";
            Assert.AreEqual(expectedWire, actualDevice.Wire);
        }

        [TestMethod]
        public void Load_Bid_Manufacturer()
        {
            //Assert
            string expectedName = "Test Manufacturer";
            double expectedMultiplier = 0.17;

            Assert.AreEqual(expectedName, actualManufacturer.Name);
            Assert.AreEqual(expectedMultiplier, actualManufacturer.Multiplier);

            Assert.AreEqual(expectedName, actualDevice.Manufacturer.Name);
            Assert.AreEqual(expectedMultiplier, actualDevice.Manufacturer.Multiplier);
        }

        [TestMethod]
        public void Load_Bid_Point()
        {
            //Assert
            string expectedName = "Test Point";
            Assert.AreEqual(expectedName, actualPoint.Name);
            
            string expectedDescription = "Test Point Description";
            Assert.AreEqual(expectedDescription, actualPoint.Description);
            
            int expectedQuantity = 321;
            Assert.AreEqual(expectedQuantity, actualPoint.Quantity);
            
            PointTypes expectedType = PointTypes.Serial;
            Assert.AreEqual(expectedType, actualPoint.Type);
        }

        [TestMethod]
        public void Load_Bid_Location()
        {
            //Assert
            string expectedLocationName = "Test Location";
            Assert.AreEqual(expectedLocationName, actualBid.Locations[0].Name);

            string expectedLocation2Name = "Test Location 2";
            Assert.AreEqual(expectedLocation2Name, actualBid.Locations[1].Name);

            //System and Equipment have the same location, but subscope does not
            Assert.AreEqual(actualBid.Locations[0], actualSystem.Location);
            Assert.AreEqual(actualBid.Locations[0], actualEquipment.Location);
            Assert.AreEqual(actualBid.Locations[1], actualSubScope.Location);
        }

        [TestMethod]
        public void Load_Bid_Note()
        {
            //Assert
            string expectedText = "Test Note";
            Assert.AreEqual(expectedText, actualBid.Notes[0].Text);
        }

        [TestMethod]
        public void Load_Bid_Exclusion()
        {
            //Assert
            string expectedText = "Test Exclusion";
            Assert.AreEqual(expectedText, actualBid.Exclusions[0].Text);
        }
    }
}
