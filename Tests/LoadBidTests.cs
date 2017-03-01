using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        static ObservableCollection<TECDevice> actualDevices;
        static TECPoint actualPoint;
        static TECManufacturer actualManufacturer;
        static TECTag actualTag;
        static TECDrawing actualDrawing;
        static TECPage actualPage;
        static TECVisualScope actualVisScope;
        static TECController actualController;
        static TECConnection actualConnection;
        static TECAssociatedCost actualAssociatedCost;
        static TECConduitType actualConduitType;
        static TECConnectionType actualConnectionType;

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
            actualBid = TestHelper.LoadTestBid(TestHelper.StaticTestBidPath);
            actualSystem = actualBid.Systems[0];
            actualEquipment = actualSystem.Equipment[0];
            actualSubScope = actualEquipment.SubScope[0];
            actualDevice = actualSubScope.Devices[0];
            actualDevices = actualSubScope.Devices;
            actualPoint = actualSubScope.Points[0];
            actualManufacturer = actualBid.ManufacturerCatalog[0];
            actualTag = actualBid.Tags[0];
            actualAssociatedCost = actualBid.AssociatedCostsCatalog[0];
            actualConnectionType = actualBid.ConnectionTypes[0];
            actualConduitType = actualBid.ConduitTypes[0];

            actualDrawing = actualBid.Drawings[0];
            actualPage = actualDrawing.Pages[0];
            actualVisScope = actualPage.PageScope[0];

            actualController = actualBid.Controllers[0];
            actualConnection = actualBid.Connections[0];
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
        public void Load_Bid_LaborConsts()
        {
            //Assert
            double expectedPMCoef = 2;
            double expectedPMRate = 2.1;
            Assert.AreEqual(expectedPMCoef, actualBid.Labor.PMCoef);
            Assert.AreEqual(expectedPMRate, actualBid.Labor.PMRate);

            double expectedENGCoef = 3;
            double expectedENGRate = 3.1;
            Assert.AreEqual(expectedENGCoef, actualBid.Labor.ENGCoef);
            Assert.AreEqual(expectedENGRate, actualBid.Labor.ENGRate);

            double expectedCommCoef = 4;
            double expectedCommRate = 4.1;
            Assert.AreEqual(expectedCommCoef, actualBid.Labor.CommCoef);
            Assert.AreEqual(expectedCommRate, actualBid.Labor.CommRate);

            double expectedSoftCoef = 5;
            double expectedSoftRate = 5.1;
            Assert.AreEqual(expectedSoftCoef, actualBid.Labor.SoftCoef);
            Assert.AreEqual(expectedSoftRate, actualBid.Labor.SoftRate);

            double expectedGraphCoef = 6;
            double expectedGraphRate = 6.1;
            Assert.AreEqual(expectedGraphCoef, actualBid.Labor.GraphCoef);
            Assert.AreEqual(expectedGraphRate, actualBid.Labor.GraphRate);
        }

        [TestMethod]
        public void Load_Bid_SubcontractorConsts()
        {
            //Assert
            double expectedElectricalRate = 7;
            double expectedElectricalSuperRate = 7.1;
            Assert.AreEqual(expectedElectricalRate, actualBid.Labor.ElectricalRate);
            Assert.AreEqual(expectedElectricalSuperRate, actualBid.Labor.ElectricalSuperRate);
        }

        [TestMethod]
        public void Load_Bid_UserAdjustments()
        {
            //Assert
            double expectedPMExtra = 10;
            double expectedENGExtra = 10.1;
            double expectedCommExtra = 10.2;
            double expectedSoftExtra = 10.3;
            double expectedGraphExtra = 10.4;

            Assert.AreEqual(expectedPMExtra, actualBid.Labor.PMExtraHours);
            Assert.AreEqual(expectedENGExtra, actualBid.Labor.ENGExtraHours);
            Assert.AreEqual(expectedCommExtra, actualBid.Labor.CommExtraHours);
            Assert.AreEqual(expectedSoftExtra, actualBid.Labor.SoftExtraHours);
            Assert.AreEqual(expectedGraphExtra, actualBid.Labor.GraphExtraHours);
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
            Assert.AreEqual("Test ConduitType", actualSubScope.ConduitType.Name);
            
            string expectedDescription = "Test SubScope Description";
            Assert.AreEqual(expectedDescription, actualSubScope.Description);
            
            int expectedQuantity = 789;
            Assert.AreEqual(expectedQuantity, actualSubScope.Quantity);
            Assert.AreEqual(12, actualSubScope.Length);
            Assert.AreEqual(actualConnection, actualSubScope.Connection);
            Assert.AreEqual("Test Cost", actualSubScope.AssociatedCosts[0].Name);
        }

        [TestMethod]
        public void Load_Bid_Device()
        {
            //Assert
            string expectedName = "Test Device";
            Assert.AreEqual(expectedName, actualDevice.Name);
            
            string expectedDescription = "Test Device Description";
            Assert.AreEqual(expectedDescription, actualDevice.Description);
            
            int expectedQuantity = 3;
            int actualQuantity = 0;
            foreach(TECDevice device in actualDevices)
            {
                if(device.Guid == actualDevice.Guid)
                {
                    actualQuantity++;
                }
            }
            Assert.AreEqual(expectedQuantity, actualQuantity);
            
            double expectedCost = 654;
            Assert.AreEqual(expectedCost, actualDevice.Cost);
            
            Assert.AreEqual("ThreeC18", actualDevice.ConnectionType.Name);

            Assert.AreEqual(actualManufacturer.Guid, actualDevice.Manufacturer.Guid);
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
        public void Load_Bid_ScopeTree()
        {
            TECScopeBranch actualScopeParent = actualBid.ScopeTree[0];
            TECScopeBranch actualScopeChild = actualScopeParent.Branches[0];
            TECScopeBranch actualScopeGrandChild = actualScopeChild.Branches[0];

            //Assert
            Assert.AreEqual("Scope 1", actualScopeParent.Name);
            Assert.AreEqual("1st Description", actualScopeParent.Description);

            Assert.AreEqual("Scope 2", actualScopeChild.Name);
            Assert.AreEqual("2nd Description", actualScopeChild.Description);

            Assert.AreEqual("Scope 3", actualScopeGrandChild.Name);
            Assert.AreEqual("3rd Description", actualScopeGrandChild.Description);

            Assert.AreEqual(1, actualBid.ScopeTree.Count);
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

        [TestMethod]
        public void Load_Bid_Tag()
        {
            //Assert
            string expectedText = "Test Tag";
            Assert.AreEqual(expectedText, actualTag.Text);

            Guid expectedGuid = actualTag.Guid;

            Assert.AreEqual(expectedGuid, actualSystem.Tags[0].Guid);
            Assert.AreEqual(expectedText, actualSystem.Tags[0].Text);

            Assert.AreEqual(expectedGuid, actualEquipment.Tags[0].Guid);
            Assert.AreEqual(expectedText, actualEquipment.Tags[0].Text);

            Assert.AreEqual(expectedGuid, actualSubScope.Tags[0].Guid);
            Assert.AreEqual(expectedText, actualSubScope.Tags[0].Text);

            Assert.AreEqual(expectedGuid, actualDevice.Tags[0].Guid);
            Assert.AreEqual(expectedText, actualDevice.Tags[0].Text);

            Assert.AreEqual(expectedGuid, actualPoint.Tags[0].Guid);
            Assert.AreEqual(expectedText, actualPoint.Tags[0].Text);

            Assert.AreEqual(expectedGuid, actualController.Tags[0].Guid);
            Assert.AreEqual(expectedText, actualController.Tags[0].Text);
        }

        [TestMethod]
        public void Load_Bid_Drawing()
        {
            //Assert
            string expectedName = "Test Drawing";
            string expectedDescription = "Test Drawing Description";

            Assert.AreEqual(expectedName, actualDrawing.Name);
            Assert.AreEqual(expectedDescription, actualDrawing.Description);
        }

        [TestMethod]
        public void Load_Bid_Page()
        {
            //Assert
            int expectedPageNum = 1;

            Assert.AreEqual(expectedPageNum, actualPage.PageNum);
            Assert.AreEqual(actualVisScope, actualPage.PageScope[0]);
        }

        [TestMethod]
        public void Load_Bid_VisualScope()
        {
            //Assert
            double expectedXPos = 119;
            double expectedYPos = 69.08;

            Assert.AreEqual(expectedXPos, actualVisScope.X);
            Assert.AreEqual(expectedYPos, actualVisScope.Y);
            Assert.AreEqual(actualSystem, actualVisScope.Scope);
        }

        [TestMethod]
        public void Load_Bid_Controller()
        {
            //Arrange
            string expectedName = "Test Controller";
            string expectedDescription = "Test Controller Description";
            double expectedCost = 64.94;

            bool hasAI = false;
            bool hasAO = false;

            foreach (TECIO io in actualController.IO)
            {
                if (io.Type == IOType.AI)
                {
                    hasAI = true;
                }
                else if (io.Type == IOType.AO)
                {
                    hasAO = true;
                }
            }

            bool hasConnection = false;
            foreach (TECConnection conn in actualController.Connections)
            {
                if (conn == actualConnection)
                {
                    hasConnection = true;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualController.Name);
            Assert.AreEqual(expectedDescription, actualController.Description);
            Assert.AreEqual(expectedCost, actualController.Cost);
            Assert.IsTrue(hasAI);
            Assert.IsTrue(hasAO);

            Assert.IsTrue(hasConnection);
        }

        [TestMethod]
        public void Load_Bid_Connection()
        {
            //Arrange
            double expectedLength = 493.45;

            bool hasThreeC18 = false;
            foreach (TECConnectionType type in actualConnection.ConnectionTypes)
            {
                if (type.Name == "ThreeC18")
                {
                    hasThreeC18 = true;
                }
            }

            bool hasSubScope = false;
            foreach (TECScope scope in actualConnection.Scope)
            {
                if (scope == actualSubScope)
                {
                    hasSubScope = true;
                }
            }

            //Assert
            Assert.AreEqual(expectedLength, actualConnection.Length);
            Assert.IsTrue(hasThreeC18);
            Assert.IsTrue(hasSubScope);
        }

        [TestMethod]
        public void Load_Bid_ProposalScope()
        {
            //Arrange
            TECProposalScope actualPropScope = actualBid.ProposalScope[0];

            TECScope expectedScope = actualSystem;

            bool expectedIsProposed = true;

            string expectedPropScopeBranchName = "Proposal Scope";

            //Assert
            Assert.AreEqual(expectedScope, actualPropScope.Scope);
            Assert.AreEqual(expectedIsProposed, actualPropScope.IsProposed);
            Assert.AreEqual(expectedPropScopeBranchName, actualPropScope.Notes[0].Name);
        }

        [TestMethod]
        public void Load_Bid_AssociatedCosts()
        {
            //Assert
            string expectedName = "Test Cost";
            Assert.AreEqual(expectedName, actualAssociatedCost.Name);
        }

        [TestMethod]
        public void Load_Bid_ConnectionType()
        {
            //Assert
            string expectedName = "ThreeC18";
            Assert.AreEqual(expectedName, actualConnectionType.Name);
        }

        [TestMethod]
        public void Load_Bid_ConduitType()
        {
            //Assert
            string expectedName = "Test ConduitType";
            Assert.AreEqual(expectedName, actualConduitType.Name);
        }
    }
}
