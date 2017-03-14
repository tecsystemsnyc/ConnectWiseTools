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
            double expectedElectricalNonUnionRate = 8;
            double expectedElectricalSuperNonUnionRate = 8.1;
            bool expectedOT = true;
            bool expectedUnion = true;
            Assert.AreEqual(expectedElectricalRate, actualBid.Labor.ElectricalRate);
            Assert.AreEqual(expectedElectricalSuperRate, actualBid.Labor.ElectricalSuperRate);
            Assert.AreEqual(expectedElectricalNonUnionRate, actualBid.Labor.ElectricalNonUnionRate);
            Assert.AreEqual(expectedElectricalSuperNonUnionRate, actualBid.Labor.ElectricalSuperNonUnionRate);
            Assert.AreEqual(expectedOT, actualBid.Labor.ElectricalIsOnOvertime);
            Assert.AreEqual(expectedUnion, actualBid.Labor.ElectricalIsUnion);
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
            //Arrange
            TECSystem actualSystem = actualBid.Systems[0];

            //Assert
            string expectedName = "Test System";
            Assert.AreEqual(expectedName, actualSystem.Name);
            
            string expectedDescription = "Test System Description";
            Assert.AreEqual(expectedDescription, actualSystem.Description);
            
            int expectedQuantity = 123;
            Assert.AreEqual(expectedQuantity, actualSystem.Quantity);
            
            double expectedBP = 123;
            Assert.AreEqual(expectedBP, actualSystem.BudgetPriceModifier);
        }

        [TestMethod]
        public void Load_Bid_Equipment()
        {
            //Arrange
            TECEquipment actualEquipment = actualBid.Systems[0].Equipment[0];

            //Assert
            string expectedName = "Test Equipment";
            Assert.AreEqual(expectedName, actualEquipment.Name);
            
            string expectedDescription = "Test Equipment Description";
            Assert.AreEqual(expectedDescription, actualEquipment.Description);
            
            int expectedQuantity = 456;
            Assert.AreEqual(expectedQuantity, actualEquipment.Quantity);
            
            double expectedBP = 456;
            Assert.AreEqual(expectedBP, actualEquipment.BudgetUnitPrice);
        }

        [TestMethod]
        public void Load_Bid_SubScope()
        {
            //Arrange
            TECSubScope actualSubScope = actualBid.Systems[0].Equipment[0].SubScope[0];
            TECConnection actualConnection = actualBid.Connections[0];

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
            //Arrange
            ObservableCollection<TECDevice> actualDevices = actualBid.Systems[0].Equipment[0].SubScope[0].Devices;
            TECDevice actualDevice = actualDevices[0];
            TECManufacturer actualManufacturer = actualBid.ManufacturerCatalog[0];

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
            //Arrange
            TECManufacturer actualManufacturer = actualBid.ManufacturerCatalog[0];
            TECDevice actualDevice = actualBid.Systems[0].Equipment[0].SubScope[0].Devices[0];

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
            //Arrange
            TECPoint actualPoint = actualBid.Systems[0].Equipment[0].SubScope[0].Points[0];

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
            //Arrange
            TECSystem actualSystem = actualBid.Systems[0];
            TECEquipment actualEquipment = actualSystem.Equipment[0];
            TECSubScope actualSubScope = actualEquipment.SubScope[0];

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
            //Arrange
            TECTag actualTag = actualBid.Tags[0];
            TECSystem actualSystem = actualBid.Systems[0];
            TECEquipment actualEquipment = actualSystem.Equipment[0];
            TECSubScope actualSubScope = actualEquipment.SubScope[0];
            TECDevice actualDevice = actualSubScope.Devices[0];
            TECPoint actualPoint = actualSubScope.Points[0];
            TECController actualController = actualBid.Controllers[0];

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
            //Arrange
            TECDrawing actualDrawing = actualBid.Drawings[0];

            //Assert
            string expectedName = "Test Drawing";
            string expectedDescription = "Test Drawing Description";

            Assert.AreEqual(expectedName, actualDrawing.Name);
            Assert.AreEqual(expectedDescription, actualDrawing.Description);
        }

        [TestMethod]
        public void Load_Bid_Page()
        {
            //Arrange
            TECPage actualPage = actualBid.Drawings[0].Pages[0];

            //Assert
            int expectedPageNum = 1;

            Assert.AreEqual(expectedPageNum, actualPage.PageNum);
        }

        [TestMethod]
        public void Load_Bid_VisualScope()
        {
            //Arrange
            TECVisualScope actualVisScope = actualBid.Drawings[0].Pages[0].PageScope[0];
            TECSystem actualSystem = actualBid.Systems[0];

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
            TECController actualController = actualBid.Controllers[0];
            TECConnection actualConnection = actualBid.Connections[0];

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
            TECConnection actualConnection = actualBid.Connections[0];
            TECSubScope actualSubScope = actualBid.Systems[0].Equipment[0].SubScope[0];

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
            Assert.IsTrue(hasThreeC18, "Connection type failed to load.");
            Assert.IsTrue(hasSubScope, "Connection scope failed to load.");
        }

        [TestMethod]
        public void Load_Bid_ProposalScope()
        {
            TECSystem actualSystem = actualBid.Systems[0];

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
            //Arrange
            TECAssociatedCost actualAssociatedCost = actualBid.AssociatedCostsCatalog[0];

            //Assert
            string expectedName = "Test Cost";
            Assert.AreEqual(expectedName, actualAssociatedCost.Name);
        }

        [TestMethod]
        public void Load_Bid_ConnectionType()
        {
            //Arrange
            TECConnectionType actualConnectionType = actualBid.ConnectionTypes[0];

            //Assert
            string expectedName = "ThreeC18";
            Assert.AreEqual(expectedName, actualConnectionType.Name);
        }

        [TestMethod]
        public void Load_Bid_ConduitType()
        {
            //Arrange
            TECConduitType actualConduitType = actualBid.ConduitTypes[0];

            //Assert
            string expectedName = "Test ConduitType";
            Assert.AreEqual(expectedName, actualConduitType.Name);
        }
        
        [TestMethod]
        public void Load_Bid_MiscCost()
        {
            //Arrange
            TECMiscCost actualCost = actualBid.MiscCosts[0];

            //Assert
            Assert.AreEqual("Test Misc Cost", actualCost.Name);
            Assert.AreEqual(654.9648, actualCost.Cost);
            Assert.AreEqual(19, actualCost.Quantity);
        }


        [TestMethod]
        public void Load_Bid_MiscWiring()
        {
            //Arrange
            TECMiscWiring actualCost = actualBid.MiscWiring[0];

            //Assert
            Assert.AreEqual("Test Misc Wiring", actualCost.Name);
            Assert.AreEqual(654.9648, actualCost.Cost);
            Assert.AreEqual(19, actualCost.Quantity);
        }


        [TestMethod]
        public void Load_Bid_PanelType()
        {
            //Arrange
            TECPanelType actualCost = actualBid.PanelTypeCatalog[0];

            //Assert
            Assert.AreEqual("Test Panel Type", actualCost.Name);
            Assert.AreEqual(654.9648, actualCost.Cost);
        }

        [TestMethod]
        public void Load_Bid_Panel()
        {
            //Arrange
            TECPanel actualPanel = actualBid.Panels[0];
            TECPanelType actualPanelType = actualBid.Panels[0].Type;

            //Assert
            Assert.AreEqual("Test Panel", actualPanel.Name);
            Assert.AreEqual("Test Panel Type", actualPanelType.Name);
        }

        [TestMethod]
        public void Load_Bid_Linked_Devices()
        {
            foreach (TECSystem system in actualBid.Systems)
            {
                foreach (TECEquipment equipment in system.Equipment)
                {
                    foreach (TECSubScope subScope in equipment.SubScope)
                    {
                        foreach (TECDevice device in subScope.Devices)
                        {
                            if (!actualBid.DeviceCatalog.Contains(device))
                            {
                                Assert.Fail("Devices in systems not linked");
                            }
                        }
                    }
                }
            }
            
            Assert.IsTrue(true, "All Devices Linked");
        }

        [TestMethod]
        public void Load_Bid_Linked_AssociatedCosts()
        {
            foreach (TECSystem system in actualBid.Systems)
            {
                foreach (TECAssociatedCost cost in system.AssociatedCosts)
                {
                    if (!actualBid.AssociatedCostsCatalog.Contains(cost))
                    { Assert.Fail("Associated costs in system not linked"); }
                }
                foreach (TECEquipment equipment in system.Equipment)
                {
                    foreach (TECAssociatedCost cost in equipment.AssociatedCosts)
                    {
                        if (!actualBid.AssociatedCostsCatalog.Contains(cost))
                        { Assert.Fail("Associated costs in equipment not linked"); }
                    }
                    foreach (TECSubScope subScope in equipment.SubScope)
                    {
                        foreach (TECAssociatedCost cost in subScope.AssociatedCosts)
                        {
                            if (!actualBid.AssociatedCostsCatalog.Contains(cost))
                            { Assert.Fail("Associated costs in subscope not linked"); }
                        }
                        foreach (TECDevice device in subScope.Devices)
                        {
                            foreach (TECAssociatedCost cost in device.AssociatedCosts)
                            {
                                if (!actualBid.AssociatedCostsCatalog.Contains(cost))
                                { Assert.Fail("Associated costs in subscope not linked"); }
                            }
                        }
                    }
                }
            }
           
            foreach (TECDevice device in actualBid.DeviceCatalog)
            {
                foreach (TECAssociatedCost cost in device.AssociatedCosts)
                {
                    if (!actualBid.AssociatedCostsCatalog.Contains(cost))
                    { Assert.Fail("Associated costs in device catalog not linked"); }
                }
            }
            foreach (TECConduitType conduitType in actualBid.ConduitTypes)
            {
                foreach (TECAssociatedCost cost in conduitType.AssociatedCosts)
                {
                    if (!actualBid.AssociatedCostsCatalog.Contains(cost))
                    { Assert.Fail("Associated costs in conduit type catalog not linked"); }
                }
            }
            foreach (TECConnectionType connectionType in actualBid.ConnectionTypes)
            {
                foreach (TECAssociatedCost cost in connectionType.AssociatedCosts)
                {
                    if (!actualBid.AssociatedCostsCatalog.Contains(cost))
                    { Assert.Fail("Associated costs in connection type catalog not linked"); }
                }
            }

            Assert.IsTrue(true, "All Associated costs Linked");
        }

        [TestMethod]
        public void Load_Bid_Linked_Manufacturers()
        {
            foreach (TECDevice device in actualBid.DeviceCatalog)
            {
                if (device.Manufacturer == null)
                {
                    Assert.Fail("Device doesn't have manufacturer.");
                }
                if (!actualBid.ManufacturerCatalog.Contains(device.Manufacturer))
                {
                    Assert.Fail("Manufacturers not linked in device catalog");
                }
            }
            foreach (TECController controller in actualBid.Controllers)
            {
                if (controller.Manufacturer == null)
                {
                    Assert.Fail("Controller doesn't have manufacturer.");
                }
                if (!actualBid.ManufacturerCatalog.Contains(controller.Manufacturer))
                {
                    Assert.Fail("Manufacturers not linked in controllers");
                }
            }
            Assert.IsTrue(true, "All Manufacturers linked");
        }

        [TestMethod]
        public void Load_Bid_Linked_ConduitTypes()
        {
            foreach (TECSystem system in actualBid.Systems)
            {
                foreach (TECEquipment equipment in system.Equipment)
                {
                    foreach (TECSubScope subScope in equipment.SubScope)
                    {
                        if (!actualBid.ConduitTypes.Contains(subScope.ConduitType))
                        { Assert.Fail("Conduit types in subscope not linked"); }
                    }
                }
            }
            
            Assert.IsTrue(true, "All conduit types Linked");
        }

        [TestMethod]
        public void Load_Bid_Linked_Tags()
        {
            foreach (TECSystem system in actualBid.Systems)
            {
                foreach (TECTag tag in system.Tags)
                {
                    if (!actualBid.Tags.Contains(tag))
                    { Assert.Fail("Tags in system templates not linked"); }
                }
                foreach (TECEquipment equipment in system.Equipment)
                {
                    foreach (TECTag tag in equipment.Tags)
                    {
                        if (!actualBid.Tags.Contains(tag))
                        { Assert.Fail("Tags in system templates not linked"); }
                    }
                    foreach (TECSubScope subScope in equipment.SubScope)
                    {
                        foreach (TECTag tag in subScope.Tags)
                        {
                            if (!actualBid.Tags.Contains(tag))
                            { Assert.Fail("Tags in system templates not linked"); }
                        }
                        foreach (TECDevice device in subScope.Devices)
                        {
                            foreach (TECTag tag in device.Tags)
                            {
                                if (!actualBid.Tags.Contains(tag))
                                { Assert.Fail("Tags in system templates not linked"); }
                            }
                        }
                    }
                }
            }
            
            foreach (TECDevice device in actualBid.DeviceCatalog)
            {
                foreach (TECTag tag in device.Tags)
                {
                    if (!actualBid.Tags.Contains(tag))
                    { Assert.Fail("Tags in device catalog not linked"); }
                }
            }
            foreach (TECConduitType conduitType in actualBid.ConduitTypes)
            {
                foreach (TECTag tag in conduitType.Tags)
                {
                    if (!actualBid.Tags.Contains(tag))
                    { Assert.Fail("Tags in conduit type catalog not linked"); }
                }
            }
            foreach (TECConnectionType connectionType in actualBid.ConnectionTypes)
            {
                foreach (TECTag tag in connectionType.Tags)
                {
                    if (!actualBid.Tags.Contains(tag))
                    { Assert.Fail("Tags in connection type catalog not linked"); }
                }
            }

            Assert.IsTrue(true, "All Tags Linked");
        }

        [TestMethod]
        public void Load_Bid_Linked_ConnectionTypes()
        {
            foreach (TECDevice device in actualBid.DeviceCatalog)
            {
                if (device.ConnectionType == null)
                {
                    Assert.Fail("Device doesn't have connectionType");
                }

                if (!actualBid.ConnectionTypes.Contains(device.ConnectionType))
                {
                    Assert.Fail("ConnectionTypes not linked in device catalog");
                }
            }
        }
        
    }
}
