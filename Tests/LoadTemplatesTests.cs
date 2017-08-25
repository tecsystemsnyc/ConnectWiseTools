using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
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
    public class LoadTemplatesTests
    {
        static TECTemplates actualTemplates;

        static Guid TEST_TAG_GUID = new Guid("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
        static Guid TEST_TEC_COST_GUID = new Guid("1c2a7631-9e3b-4006-ada7-12d6cee52f08");
        static Guid TEST_ELECTRICAL_COST_GUID = new Guid("63ed1eb7-c05b-440b-9e15-397f64ff05c7");
        static Guid TEST_LOCATION_GUID = new Guid("4175d04b-82b1-486b-b742-b2cc875405cb");
        static Guid TEST_RATED_COST_GUID = new Guid("b7c01526-c195-442f-a1f1-28d07db61144");

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
            string path = Path.GetTempFileName();
            TestDBHelper.CreateTestTemplates(path);
            actualTemplates = TestHelper.LoadTestTemplates(path);
        }

        [TestMethod]
        public void Load_Templates_LaborConsts()
        {
            throw new NotImplementedException();
            ////Assert
            //double expectedPMCoef = 2;
            //double expectedPMRate = 30;
            //Assert.AreEqual(expectedPMCoef, actualTemplates.Parameters.PMCoef, "PM Coefficient didn't load properly.");
            //Assert.AreEqual(expectedPMRate, actualTemplates.Parameters.PMRate, "PM Rate didn't load properly.");

            //double expectedENGCoef = 2;
            //double expectedENGRate = 40;
            //Assert.AreEqual(expectedENGCoef, actualTemplates.Parameters.ENGCoef, "ENG Coefficient didn't load properly.");
            //Assert.AreEqual(expectedENGRate, actualTemplates.Parameters.ENGRate, "ENG Rate didn't load properly.");

            //double expectedCommCoef = 2;
            //double expectedCommRate = 50;
            //Assert.AreEqual(expectedCommCoef, actualTemplates.Parameters.CommCoef, "Comm Coefficient didn't load properly.");
            //Assert.AreEqual(expectedCommRate, actualTemplates.Parameters.CommRate, "Comm Rate didn't load properly.");

            //double expectedSoftCoef = 2;
            //double expectedSoftRate = 60;
            //Assert.AreEqual(expectedSoftCoef, actualTemplates.Parameters.SoftCoef, "Software Coefficient didn't load properly.");
            //Assert.AreEqual(expectedSoftRate, actualTemplates.Parameters.SoftRate, "Software Rate didn't load properly.");

            //double expectedGraphCoef = 2;
            //double expectedGraphRate = 70;
            //Assert.AreEqual(expectedGraphCoef, actualTemplates.Parameters.GraphCoef, "Graphics Coefficient didn't load properly.");
            //Assert.AreEqual(expectedGraphRate, actualTemplates.Parameters.GraphRate, "Graphics Rate didn't load properly.");
        }

        [TestMethod]
        public void Load_Templates_SubcontractorConsts()
        {
            throw new NotImplementedException();
            ////Assert
            //double expectedElectricalRate = 50;
            //double expectedElectricalSuperRate = 60;
            //double expectedElectricalNonUnionRate = 30;
            //double expectedElectricalSuperNonUnionRate = 40;
            //double expectedElectricalSuperRatio = 0.25;
            //bool expectedOT = false;
            //bool expectedUnion = true;
            //Assert.AreEqual(expectedElectricalRate, actualTemplates.Parameters.ElectricalRate, "Electrical rate didn't load properly.");
            //Assert.AreEqual(expectedElectricalSuperRate, actualTemplates.Parameters.ElectricalSuperRate, "Electrical Supervision rate didn't load properly.");
            //Assert.AreEqual(expectedElectricalNonUnionRate, actualTemplates.Parameters.ElectricalNonUnionRate, "Electrical Non-Union rate didn't load properly.");
            //Assert.AreEqual(expectedElectricalSuperNonUnionRate, actualTemplates.Parameters.ElectricalSuperNonUnionRate, "Electrical Supervision Non-Union rate didn't load properly.");
            //Assert.AreEqual(expectedElectricalSuperRatio, actualTemplates.Parameters.ElectricalSuperRatio, "Electrical Supervision time ratio didn't load properly.");
            //Assert.AreEqual(expectedOT, actualTemplates.Parameters.ElectricalIsOnOvertime, "Electrical overtime bool didn't load properly.");
            //Assert.AreEqual(expectedUnion, actualTemplates.Parameters.ElectricalIsUnion, "Electrical union bool didn't load properly.");
        }

        [TestMethod]
        public void Load_Templates_System()
        {
            //Arrange
            Guid expectedGuid = new Guid("ebdbcc85-10f4-46b3-99e7-d896679f874a");
            string expectedName = "Typical System";
            string expectedDescription = "Typical System Description";
            int expectedQuantity = 1;
            double expectedBP = 100;
            bool expectedProposeEquipment = true;

            Guid childEquipment = new Guid("8a9bcc02-6ae2-4ac9-bbe1-e33d9a590b0e");
            Guid childController = new Guid("1bb86714-2512-4fdd-a80f-46969753d8a0");
            Guid childPanel = new Guid("e7695d68-d79f-44a2-92f5-b303436186af");
            Guid childScopeBranch = new Guid("814710f1-f2dd-4ae6-9bc4-9279288e4994");

            TECSystem actualSystem = null;
            foreach (TECSystem system in actualTemplates.SystemTemplates)
            {
                if (system.Guid == expectedGuid)
                {
                    actualSystem = system;
                    break;
                }
            }

            bool foundEquip = false;
            foreach (TECEquipment equip in actualSystem.Equipment)
            {
                if (equip.Guid == childEquipment)
                {
                    foundEquip = true;
                    break;
                }
            }
            bool foundControl = false;
            foreach (TECController control in actualSystem.Controllers)
            {
                if (control.Guid == childController)
                {
                    foundControl = true;
                    break;
                }
            }
            bool foundPanel = false;
            foreach (TECPanel panel in actualSystem.Panels)
            {
                if (panel.Guid == childPanel)
                {
                    foundPanel = true;
                    break;
                }
            }
            bool foundScopeBranch = false;
            foreach(TECScopeBranch branch in actualSystem.ScopeBranches)
            {
                if (branch.Guid == childScopeBranch)
                {
                    foundScopeBranch = true;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualSystem.Name);
            Assert.AreEqual(expectedDescription, actualSystem.Description);
            Assert.AreEqual(expectedProposeEquipment, actualSystem.ProposeEquipment);

            foreach (TECSystem instance in actualSystem.Instances)
            {
                Assert.AreEqual(actualSystem.Equipment.Count, instance.Equipment.Count);
                Assert.AreEqual(actualSystem.Panels.Count, instance.Panels.Count);
                Assert.AreEqual(actualSystem.Controllers.Count, instance.Controllers.Count);
            }

            Assert.IsTrue(foundEquip, "Equipment not loaded properly into system.");
            Assert.IsTrue(foundControl, "Controller not loaded properly into system.");
            Assert.IsTrue(foundPanel, "Panel not loaded properly into system.");
            Assert.IsTrue(foundScopeBranch, "Scope branch not loaded properly into system.");

            testForTag(actualSystem);
            testForCosts(actualSystem);
        }

        [TestMethod]
        public void Load_Templates_Equipment()
        {
            Guid expectedGuid = new Guid("1645886c-fce7-4380-a5c3-295f91961d16");
            string expectedName = "Template Equip";
            string expectedDescription = "Template Equip Description";
            int expectedQuantity = 1;
            double expectedBP = 25;

            Guid childSubScope = new Guid("214dc8d1-22be-4fbf-8b6b-d66c21105f61");

            TECEquipment actualEquipment = null;
            foreach(TECEquipment equip in actualTemplates.EquipmentTemplates)
            {
                if (equip.Guid == expectedGuid)
                {
                    actualEquipment = equip;
                    break;
                }
            }

            bool foundSubScope = false;
            foreach (TECSubScope ss in actualEquipment.SubScope)
            {
                if (ss.Guid == childSubScope)
                {
                    foundSubScope = true;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualEquipment.Name);
            Assert.AreEqual(expectedDescription, actualEquipment.Description);

            Assert.IsTrue(foundSubScope, "Subscope not loaded properly into equipment.");

            testForTag(actualEquipment);
            testForCosts(actualEquipment);
        }

        [TestMethod]
        public void Load_Templates_SubScope()
        {
            //Arrange
            Guid expectedGuid = new Guid("3ebdfd64-5249-4332-a832-ff3cc0cdb309");
            string expectedName = "Template SS";
            string expectedDescription = "Template SS Description";
            int expectedQuantity = 1;

            Guid childPoint = new Guid("6776a30b-0325-42ad-8aa3-3c065b4bb908");
            Guid childDevice = new Guid("95135fdf-7565-4d22-b9e4-1f177febae15");

            TECSubScope actualSubScope = null;
            foreach(TECSubScope ss in actualTemplates.SubScopeTemplates)
            {
                if (ss.Guid == expectedGuid)
                {
                    actualSubScope = ss;
                    break;
                }
            }

            bool foundPoint = false;
            foreach (TECPoint point in actualSubScope.Points)
            {
                if (point.Guid == childPoint)
                {
                    foundPoint = true;
                    break;
                }
            }
            bool foundDevice = false;
            foreach (TECDevice device in actualSubScope.Devices)
            {
                if (device.Guid == childDevice)
                {
                    foundDevice = true;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualSubScope.Name, "Name not loaded");
            Assert.AreEqual(expectedDescription, actualSubScope.Description, "Description not loaded");

            Assert.IsTrue(foundPoint, "Point not loaded into subscope properly.");
            Assert.IsTrue(foundDevice, "Device not loaded into subscope properly.");

            testForTag(actualSubScope);
            testForCosts(actualSubScope);
        }

        [TestMethod]
        public void Load_Templates_Device()
        {
            Guid expectedGuid = new Guid("95135fdf-7565-4d22-b9e4-1f177febae15");
            string expectedName = "Test Device";
            string expectedDescription = "Test Device Description";
            double expectedCost = 123.45;

            Guid manufacturerGuid = new Guid("90cd6eae-f7a3-4296-a9eb-b810a417766d");
            Guid connectionTypeGuid = new Guid("f38867c8-3846-461f-a6fa-c941aeb723c7");

            TECDevice actualDevice = null;
            foreach (TECDevice dev in actualTemplates.Catalogs.Devices)
            {
                if (dev.Guid == expectedGuid)
                {
                    actualDevice = dev;
                    break;
                }
            }

            bool foundConnectionType = false;
            foreach (TECElectricalMaterial connectType in actualDevice.ConnectionTypes)
            {
                if (connectType.Guid == connectionTypeGuid)
                {
                    foundConnectionType = true;
                    break;
                }
            }

            Assert.AreEqual(expectedName, actualDevice.Name, "Device name didn't load properly.");
            Assert.AreEqual(expectedDescription, actualDevice.Description, "Device description didn't load properly.");
            Assert.AreEqual(expectedCost, actualDevice.Cost, "Device cost didn't load properly.");
            Assert.AreEqual(manufacturerGuid, actualDevice.Manufacturer.Guid, "Manufacturer didn't load properly into device.");

            Assert.IsTrue(foundConnectionType, "Connection type didn't load properly into device.");

            testForTag(actualDevice);
            testForCosts(actualDevice);
        }

        [TestMethod]
        public void Load_Templates_Manufacturer()
        {
            //Arrange
            Guid expectedGuid = new Guid("90cd6eae-f7a3-4296-a9eb-b810a417766d");
            string expectedName = "Test Manufacturer";
            double expectedMultiplier = 0.5;


            TECManufacturer actualManufacturer = null;
            foreach (TECManufacturer man in actualTemplates.Catalogs.Manufacturers)
            {
                if (man.Guid == expectedGuid)
                {
                    actualManufacturer = man;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualManufacturer.Label);
            Assert.AreEqual(expectedMultiplier, actualManufacturer.Multiplier);
        }

        //[TestMethod]
        //public void Load_Templates_Controller()
        //{
        //    //Arrange
        //    Guid expectedGuid = new Guid("98e6bc3e-31dc-4394-8b54-9ca53c193f46");
        //    string expectedName = "Bid Controller";
        //    string expectedDescription = "Bid Controller Description";
        //    double expectedCost = 1812;
        //    NetworkType expectedType = NetworkType.Server;
        //    bool expectedGlobalStatus = true;

        //    TECController actualController = null;
        //    foreach (TECController controller in actualTemplates.ControllerTemplates)
        //    {
        //        if (controller.Guid == expectedGuid)
        //        {
        //            actualController = controller;
        //            break;
        //        }
        //    }
            
        //    Guid expectedIOGuid = new Guid("1f6049cc-4dd6-4b50-a9d5-045b629ae6fb");

        //    bool hasIO = false;
        //    foreach (TECIO io in actualController.IO)
        //    {
        //        if (io.Guid == expectedIOGuid)
        //        {
        //            hasIO = true;
        //            break;
        //        }
        //    }

        //    //Assert
        //    Assert.AreEqual(expectedName, actualController.Name);
        //    Assert.AreEqual(expectedDescription, actualController.Description);
        //    Assert.AreEqual(expectedCost, actualController.Cost);
        //    Assert.AreEqual(expectedType, actualController.NetworkType);
        //    Assert.AreEqual(expectedGlobalStatus, actualController.IsGlobal);
        //    Assert.IsTrue(hasIO);
        //    testForTag(actualController);
        //    testForCosts(actualController);
        //}

        [TestMethod]
        public void Load_Templates_ConnectionType()
        {
            //Arrange
            Guid expectedGuid = new Guid("f38867c8-3846-461f-a6fa-c941aeb723c7");
            string expectedName = "Test Connection Type";
            double expectedCost = 12.48;
            double expectedLabor = 84.21;

            TECElectricalMaterial actualConnectionType = null;
            foreach (TECElectricalMaterial connectionType in actualTemplates.Catalogs.ConnectionTypes)
            {
                if (connectionType.Guid == expectedGuid)
                {
                    actualConnectionType = connectionType;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualConnectionType.Name);
            Assert.AreEqual(expectedCost, actualConnectionType.Cost);
            Assert.AreEqual(expectedLabor, actualConnectionType.Labor);

            testForCosts(actualConnectionType);
            testForRatedCosts(actualConnectionType);
        }

        [TestMethod]
        public void Load_Templates_ConduitType()
        {
            //Arrange
            Guid expectedGuid = new Guid("8d442906-efa2-49a0-ad21-f6b27852c9ef");
            string expectedName = "Test Conduit Type";
            double expectedCost = 45.67;
            double expectedLabor = 76.54;

            TECElectricalMaterial actualConduitType = null;
            foreach (TECElectricalMaterial conduitType in actualTemplates.Catalogs.ConduitTypes)
            {
                if (conduitType.Guid == expectedGuid)
                {
                    actualConduitType = conduitType;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualConduitType.Name);
            Assert.AreEqual(expectedCost, actualConduitType.Cost);
            Assert.AreEqual(expectedLabor, actualConduitType.Labor);

            testForCosts(actualConduitType);
            testForRatedCosts(actualConduitType);
        }

        [TestMethod]
        public void Load_Templates_AssociatedCosts()
        {
            Guid expectedTECGuid = new Guid("1c2a7631-9e3b-4006-ada7-12d6cee52f08");
            string expectedTECName = "Test TEC Associated Cost";
            double expectedTECCost = 31;
            double expectedTECLabor = 13;
            CostType expectedTECType = CostType.TEC;

            Guid expectedElectricalGuid = new Guid("63ed1eb7-c05b-440b-9e15-397f64ff05c7");
            string expectedElectricalName = "Test Electrical Associated Cost";
            double expectedElectricalCost = 42;
            double expectedElectricalLabor = 24;
            CostType expectedElectricalType = CostType.Electrical;

            TECCost actualTECCost = null;
            TECCost actualElectricalCost = null;
            foreach (TECCost cost in actualTemplates.Catalogs.AssociatedCosts)
            {
                if (cost.Guid == expectedTECGuid)
                {
                    actualTECCost = cost;
                }
                else if (cost.Guid == expectedElectricalGuid)
                {
                    actualElectricalCost = cost;
                }
                if (actualTECCost != null && actualElectricalCost != null)
                {
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedTECName, actualTECCost.Name, "TEC cost name didn't load properly.");
            Assert.AreEqual(expectedTECCost, actualTECCost.Cost, "TEC cost cost didn't load properly.");
            Assert.AreEqual(expectedTECLabor, actualTECCost.Labor, "TEC cost labor didn't load properly.");
            Assert.AreEqual(expectedTECType, actualTECCost.Type, "TEC cost type didn't load properly.");

            Assert.AreEqual(expectedElectricalName, actualElectricalCost.Name, "Electrical cost name didn't load properly.");
            Assert.AreEqual(expectedElectricalCost, actualElectricalCost.Cost, "Electrical cost cost didn't load properly.");
            Assert.AreEqual(expectedElectricalLabor, actualElectricalCost.Labor, "Electrical cost labor didn't load properly.");
            Assert.AreEqual(expectedElectricalType, actualElectricalCost.Type, "Electrical cost type didn't load properly.");
        }

        [TestMethod]
        public void Load_Templates_Tag()
        {
            Guid expectedGuid = new Guid("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            string expectedString = "Test Tag";

            TECLabeled actualTag = null;
            foreach (TECLabeled tag in actualTemplates.Catalogs.Tags)
            {
                if (tag.Guid == expectedGuid)
                {
                    actualTag = tag;
                    break;
                }
            }

            Assert.AreEqual(expectedString, actualTag.Label, "Tag text didn't load properly.");
        }

        [TestMethod]
        public void Load_Templates_MiscCost()
        {
            //Arrange
            Guid expectedGuid = new Guid("5df99701-1d7b-4fbe-843d-40793f4145a8");
            string expectedName = "Bid Misc";
            double expectedCost = 1298;
            double expectedLabor = 8921;
            double expectedQuantity = 2;
            CostType expectedType = CostType.Electrical;
            TECMisc actualMisc = null;
            foreach (TECMisc misc in actualTemplates.MiscCostTemplates)
            {
                if (misc.Guid == expectedGuid)
                {
                    actualMisc = misc;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualMisc.Name);
            Assert.AreEqual(expectedQuantity, actualMisc.Quantity);
            Assert.AreEqual(expectedCost, actualMisc.Cost);
            Assert.AreEqual(expectedLabor, actualMisc.Labor);
            Assert.AreEqual(expectedType, actualMisc.Type);
        }

        [TestMethod]
        public void Load_Templates_IOModule()
        {
            //Arrange
            Guid expectedGuid = new Guid("b346378d-dc72-4dda-b275-bbe03022dd12");
            string expectedName = "Test IO Module";
            string expectedDescription = "Test IO Module Description";
            double expectedCost = 2233;
            double expectedIOPerModule = 10;

            TECIOModule actualModule = null;
            foreach (TECIOModule module in actualTemplates.Catalogs.IOModules)
            {
                if (module.Guid == expectedGuid)
                {
                    actualModule = module;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualModule.Name);
            Assert.AreEqual(expectedDescription, actualModule.Description);
            Assert.AreEqual(expectedCost, actualModule.Cost);
            Assert.AreEqual(expectedIOPerModule, actualModule.IOPerModule);
        }

        [TestMethod]
        public void Load_Templates_PanelType()
        {
            //Arrange
            Guid expectedGuid = new Guid("04e3204c-b35f-4e1a-8a01-db07f7eb055e");
            string expectedName = "Test Panel Type";
            double expectedCost = 1324;
            double expectedLabor = 4231;

            TECPanelType actualType = null;
            foreach (TECPanelType type in actualTemplates.Catalogs.PanelTypes)
            {
                if (type.Guid == expectedGuid)
                {
                    actualType = type;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualType.Name);
            Assert.AreEqual(expectedCost, actualType.Cost);
            Assert.AreEqual(expectedLabor, actualType.Labor);
        }

        [TestMethod]
        public void Load_Templates_Panel()
        {
            //Arrange
            Guid expectedGuid = new Guid("a8cdd31c-e690-4eaa-81ea-602c72904391");
            string expectedName = "Bid Panel";
            string expectedDescription = "Bid Panel Description";
            int expectedQuantity = 1;

            Guid expectedTypeGuid = new Guid("04e3204c-b35f-4e1a-8a01-db07f7eb055e");

            TECPanel actualPanel = null;
            foreach (TECPanel panel in actualTemplates.PanelTemplates)
            {
                if (panel.Guid == expectedGuid)
                {
                    actualPanel = panel;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualPanel.Name);
            Assert.AreEqual(expectedDescription, actualPanel.Description);
            Assert.AreEqual(expectedTypeGuid, actualPanel.Type.Guid);
            testForCosts(actualPanel);
        }

        [TestMethod]
        public void Load_Templates_ScopeBranch()
        {
            Guid expectedGuid = new Guid("814710f1-f2dd-4ae6-9bc4-9279288e4994");
            string expectedName = "System Scope Branch";

            Guid childGuid = new Guid("542802f6-a7b1-4020-9be4-e58225c433a8");

            TECScopeBranch actualBranch = null;
            foreach(TECSystem system in actualTemplates.SystemTemplates)
            {
                foreach(TECScopeBranch branch in system.ScopeBranches)
                {
                    if (branch.Guid == expectedGuid)
                    {
                        actualBranch = branch;
                        break;
                    }
                }
                if (actualBranch != null) break;
            }

            bool foundChildBranch = false;
            foreach(TECScopeBranch branch in actualBranch.Branches)
            {
                if (branch.Guid == childGuid)
                {
                    foundChildBranch = true;
                    break;
                }
            }

            Assert.AreEqual(expectedName, actualBranch.Label, "Scope branch name didn't load properly.");

            Assert.IsTrue(foundChildBranch, "Child branch didn't load properly into scope branch.");
        }

        [TestMethod]
        public void Load_Templates_SubScopeConnection()
        {
            Guid expectedGuid = new Guid("5723e279-ac5c-4ee0-ae01-494a0c524b5c");
            double expectedWireLength = 40;
            double expectedConduitLength = 20;

            Guid expectedParentControllerGuid = new Guid("1bb86714-2512-4fdd-a80f-46969753d8a0");
            Guid expectedConduitTypeGuid = new Guid("8d442906-efa2-49a0-ad21-f6b27852c9ef");
            Guid expectedSubScopeGuid = new Guid("fbe0a143-e7cd-4580-a1c4-26eff0cd55a6");

            TECSubScopeConnection actualSSConnect = null;
            foreach (TECSystem typical in actualTemplates.SystemTemplates)
            {
                foreach (TECController controller in typical.Controllers)
                {
                    foreach (TECConnection connection in controller.ChildrenConnections)
                    {
                        if (connection.Guid == expectedGuid)
                        {
                            actualSSConnect = (connection as TECSubScopeConnection);
                            break;
                        }
                    }
                    if (actualSSConnect != null) break;
                }
                if (actualSSConnect != null) break;
            }

            //Assert
            Assert.AreEqual(expectedWireLength, actualSSConnect.Length, "Length didn't load properly in subscope connection.");
            Assert.AreEqual(expectedConduitLength, actualSSConnect.ConduitLength, "ConduitLength didn't load properly in subscope connection.");

            Assert.AreEqual(expectedParentControllerGuid, actualSSConnect.ParentController.Guid, "Parent controller didn't load properly in subscope connection.");
            Assert.AreEqual(expectedConduitTypeGuid, actualSSConnect.ConduitType.Guid, "Conduit type didn't load properly in subscope connection.");
            Assert.AreEqual(expectedSubScopeGuid, actualSSConnect.SubScope.Guid, "Subscope didn't load properly in subscope connection.");
        }

        private void testForTag(TECScope scope)
        {
            bool foundTag = false;

            foreach (TECLabeled tag in scope.Tags)
            {
                if (tag.Guid == TEST_TAG_GUID)
                {
                    foundTag = true;
                    break;
                }
            }

            Assert.IsTrue(foundTag, "Tag not loaded properly into scope.");
        }
        private void testForCosts(TECScope scope)
        {
            bool foundTECCost = false;
            bool foundElectricalCost = false;

            foreach (TECCost cost in scope.AssociatedCosts)
            {
                if (cost.Guid == TEST_TEC_COST_GUID)
                {
                    foundTECCost = true;
                    break;
                }
            }
            foreach (TECCost cost in scope.AssociatedCosts)
            {
                if (cost.Guid == TEST_ELECTRICAL_COST_GUID)
                {
                    foundElectricalCost = true;
                    break;
                }
            }

            Assert.IsTrue(foundTECCost, "TEC Cost not loaded properly into scope.");
            Assert.IsTrue(foundElectricalCost, "Electrical Cost not loaded properly into scope.");
        }

        private void testForRatedCosts(TECElectricalMaterial component)
        {
            bool foundCost = false;

            foreach (TECCost cost in component.RatedCosts)
            {
                if (cost.Guid == TEST_RATED_COST_GUID)
                {
                    foundCost = true;
                    break;
                }
            }

            Assert.IsTrue(foundCost, "Rated Cost not loaded properly into scope.");
        }
    }
}
