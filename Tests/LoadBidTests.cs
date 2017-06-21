using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class LoadBidTests
    {
        static TECBid actualBid;

        static Guid TEST_TAG_GUID = new Guid("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
        static Guid TEST_TEC_COST_GUID = new Guid("1c2a7631-9e3b-4006-ada7-12d6cee52f08");
        static Guid TEST_ELECTRICAL_COST_GUID = new Guid("63ed1eb7-c05b-440b-9e15-397f64ff05c7");
        static Guid TEST_LOCATION_GUID = new Guid("4175d04b-82b1-486b-b742-b2cc875405cb");

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
            var path = Path.GetTempFileName();
            TestDBHelper.CreateTestBid(path);
            actualBid = TestHelper.LoadTestBid(path);
        }

        [TestMethod]
        public void Load_Bid_Info()
        {
            //Assert
            string expectedName = "Testimate";
            Assert.AreEqual(expectedName, actualBid.Name, "Bid name didn't load properly.");

            string expectedNumber = "7357";
            Assert.AreEqual(expectedNumber, actualBid.BidNumber, "Bid number didn't load properly.");

            DateTime expectedDueDate = new DateTime(1969, 7, 20);
            Assert.AreEqual(expectedDueDate, actualBid.DueDate, "Bid due date didn't load properly.");

            string expectedSales = "Mrs. Salesperson";
            Assert.AreEqual(expectedSales, actualBid.Salesperson, "Salesperson didn't load properly.");

            string expectedEstimator = "Mr. Estimator";
            Assert.AreEqual(expectedEstimator, actualBid.Estimator, "Estimator didn't load properly.");
        }

        [TestMethod]
        public void Load_Bid_Parameters()
        {
            double expectedEscalation = 10;
            double expectedOverhead = 20;
            double expectedProfit = 20;
            double expectedSubcontractorMarkup = 20;
            double expectedSubcontractorEscalation = 10;
            bool expectedIsTaxExempt = false;
            bool expectedRequiresBond = false;
            bool expectedRequiresWrapUp = false;

            Assert.AreEqual(expectedEscalation, actualBid.Parameters.Escalation, "Escalation didn't load properly.");
            Assert.AreEqual(expectedOverhead, actualBid.Parameters.Overhead, "Overhead didn't load properly.");
            Assert.AreEqual(expectedProfit, actualBid.Parameters.Profit, "Profit didn't load properly.");
            Assert.AreEqual(expectedSubcontractorMarkup, actualBid.Parameters.SubcontractorMarkup, "Subcontractor markup didn't load properly.");
            Assert.AreEqual(expectedSubcontractorEscalation, actualBid.Parameters.SubcontractorEscalation, "Subcontractor escalation didn't load properly.");
            Assert.AreEqual(expectedIsTaxExempt, actualBid.Parameters.IsTaxExempt, "Is tax exempt didn't load properly.");
            Assert.AreEqual(expectedRequiresBond, actualBid.Parameters.RequiresBond, "Requires bond didn't load properly.");
            Assert.AreEqual(expectedRequiresWrapUp, actualBid.Parameters.RequiresWrapUp, "Requires wrap up didn't load properly.");
        }

        [TestMethod]
        public void Load_Bid_LaborConsts()
        {
            //Assert
            double expectedPMCoef = 2;
            double expectedPMRate = 30;
            Assert.AreEqual(expectedPMCoef, actualBid.Labor.PMCoef, "PM Coefficient didn't load properly.");
            Assert.AreEqual(expectedPMRate, actualBid.Labor.PMRate, "PM Rate didn't load properly.");

            double expectedENGCoef = 2;
            double expectedENGRate = 40;
            Assert.AreEqual(expectedENGCoef, actualBid.Labor.ENGCoef, "ENG Coefficient didn't load properly.");
            Assert.AreEqual(expectedENGRate, actualBid.Labor.ENGRate, "ENG Rate didn't load properly.");

            double expectedCommCoef = 2;
            double expectedCommRate = 50;
            Assert.AreEqual(expectedCommCoef, actualBid.Labor.CommCoef, "Comm Coefficient didn't load properly.");
            Assert.AreEqual(expectedCommRate, actualBid.Labor.CommRate, "Comm Rate didn't load properly.");

            double expectedSoftCoef = 2;
            double expectedSoftRate = 60;
            Assert.AreEqual(expectedSoftCoef, actualBid.Labor.SoftCoef, "Software Coefficient didn't load properly.");
            Assert.AreEqual(expectedSoftRate, actualBid.Labor.SoftRate, "Software Rate didn't load properly.");

            double expectedGraphCoef = 2;
            double expectedGraphRate = 70;
            Assert.AreEqual(expectedGraphCoef, actualBid.Labor.GraphCoef, "Graphics Coefficient didn't load properly.");
            Assert.AreEqual(expectedGraphRate, actualBid.Labor.GraphRate, "Graphics Rate didn't load properly.");
        }

        [TestMethod]
        public void Load_Bid_SubcontractorConsts()
        {
            //Assert
            double expectedElectricalRate = 50;
            double expectedElectricalSuperRate = 60;
            double expectedElectricalNonUnionRate = 30;
            double expectedElectricalSuperNonUnionRate = 40;
            double expectedElectricalSuperRatio = 0.25;
            bool expectedOT = false;
            bool expectedUnion = true;
            Assert.AreEqual(expectedElectricalRate, actualBid.Labor.ElectricalRate, "Electrical rate didn't load properly.");
            Assert.AreEqual(expectedElectricalSuperRate, actualBid.Labor.ElectricalSuperRate, "Electrical Supervision rate didn't load properly.");
            Assert.AreEqual(expectedElectricalNonUnionRate, actualBid.Labor.ElectricalNonUnionRate, "Electrical Non-Union rate didn't load properly.");
            Assert.AreEqual(expectedElectricalSuperNonUnionRate, actualBid.Labor.ElectricalSuperNonUnionRate, "Electrical Supervision Non-Union rate didn't load properly.");
            Assert.AreEqual(expectedElectricalSuperRatio, actualBid.Labor.ElectricalSuperRatio, "Electrical Supervision time ratio didn't load properly.");
            Assert.AreEqual(expectedOT, actualBid.Labor.ElectricalIsOnOvertime, "Electrical overtime bool didn't load properly.");
            Assert.AreEqual(expectedUnion, actualBid.Labor.ElectricalIsUnion, "Electrical union bool didn't load properly.");
        }

        [TestMethod]
        public void Load_Bid_UserAdjustments()
        {
            //Assert
            double expectedPMExtra = 120;
            double expectedENGExtra = 110;
            double expectedCommExtra = 100;
            double expectedSoftExtra = 90;
            double expectedGraphExtra = 80;

            Assert.AreEqual(expectedPMExtra, actualBid.Labor.PMExtraHours, "PM Extra Hours didn't load properly.");
            Assert.AreEqual(expectedENGExtra, actualBid.Labor.ENGExtraHours, "ENG Extra Hours didn't load properly.");
            Assert.AreEqual(expectedCommExtra, actualBid.Labor.CommExtraHours, "Comm Extra Hours didn't load properly.");
            Assert.AreEqual(expectedSoftExtra, actualBid.Labor.SoftExtraHours, "Soft Extra Hours didn't load properly.");
            Assert.AreEqual(expectedGraphExtra, actualBid.Labor.GraphExtraHours, "Graph Extra Hours didn't load properly.");
        }

        [TestMethod]
        public void Load_Bid_Note()
        {
            //Assert
            Guid expectedGuid = new Guid("50f3a707-fc1b-4eb3-9413-1dbde57b1d90");
            string expectedText = "Test Note";

            TECNote actualNote = null;
            foreach(TECNote note in actualBid.Notes)
            {
                if (note.Guid == expectedGuid)
                {
                    actualNote = note;
                    break;
                }
            }

            Assert.AreEqual(expectedText, actualNote.Text, "Note text didn't load properly.");
        }

        [TestMethod]
        public void Load_Bid_Exclusion()
        {
            //Assert
            Guid expectedGuid = new Guid("15692e12-e728-4f1b-b65c-de365e016e7a");
            string expectedText = "Test Exclusion";

            TECExclusion actualExclusion = null;
            foreach (TECExclusion note in actualBid.Exclusions)
            {
                if (note.Guid == expectedGuid)
                {
                    actualExclusion = note;
                    break;
                }
            }

            Assert.AreEqual(expectedText, actualExclusion.Text, "Exclusion text didn't load properly.");
        }

        [TestMethod]
        public void Load_Bid_BidScopeTree()
        {
            Guid expectedParentGuid = new Guid("25e815fa-4ac7-4b69-9640-5ae220f0cd40");
            string expectedParentName = "Bid Scope Branch";
            string expectedParentDescription = "Bid Scope Branch Description";
            Guid expectedChildGuid = new Guid("81adfc62-20ec-466f-a2a0-430e1223f64f");
            string expectedChildName = "Bid Child Branch";
            string expectedChildDescription = "Bid Child Branch Description";

            TECScopeBranch actualParent = null;
            TECScopeBranch actualChild = null;
            foreach(TECScopeBranch branch in actualBid.ScopeTree)
            {
                if (branch.Guid == expectedParentGuid)
                {
                    actualParent = branch;
                    foreach(TECScopeBranch child in branch.Branches)
                    {
                        if (child.Guid == expectedChildGuid)
                        {
                            actualChild = child;
                            break;
                        }
                    }
                    break;
                }
            }

            Assert.AreEqual(expectedParentName, actualParent.Name, "Parent scope branch name didn't load properly.");
            Assert.AreEqual(expectedParentDescription, actualParent.Description, "Parent scope branch description didn't load properly.");
            Assert.AreEqual(expectedChildName, actualChild.Name, "Child scope branch name didn't load properly.");
            Assert.AreEqual(expectedChildDescription, actualChild.Description, "Child scope branch description didn't load properly.");
        }
        
        [TestMethod]
        public void Load_Bid_TypicalSystem()
        {
            //Arrange
            Guid expectedGuid = new Guid("ebdbcc85-10f4-46b3-99e7-d896679f874a");
            string expectedName = "Typical System";
            string expectedDescription = "Typical System Description";
            int expectedQuantity = 1;
            double expectedBP = 100;
            bool expectedProposeEquipment = true;
            int expectedChildren = 1;

            Guid childEquipment = new Guid("8a9bcc02-6ae2-4ac9-bbe1-e33d9a590b0e");
            Guid childController = new Guid("1bb86714-2512-4fdd-a80f-46969753d8a0");
            Guid childPanel = new Guid("e7695d68-d79f-44a2-92f5-b303436186af");

            TECSystem actualSystem = null;
            foreach(TECSystem system in actualBid.Systems)
            {
                if(system.Guid == expectedGuid)
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
            foreach(TECPanel panel in actualSystem.Panels)
            {
                if (panel.Guid == childPanel)
                {
                    foundPanel = true;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualSystem.Name);
            Assert.AreEqual(expectedDescription, actualSystem.Description);
            Assert.AreEqual(expectedQuantity, actualSystem.Quantity);
            Assert.AreEqual(expectedBP, actualSystem.BudgetPriceModifier);
            Assert.AreEqual(expectedChildren, actualSystem.SystemInstances.Count);
            Assert.AreEqual(expectedProposeEquipment, actualSystem.ProposeEquipment);

            foreach(TECSystem instance in actualSystem.SystemInstances)
            {
                Assert.AreEqual(actualSystem.Equipment.Count, instance.Equipment.Count);
                Assert.AreEqual(actualSystem.Panels.Count, instance.Panels.Count);
                Assert.AreEqual(actualSystem.Controllers.Count, instance.Controllers.Count);
            }

            Assert.IsTrue(foundEquip, "Typical equipment not loaded properly into typical system.");
            Assert.IsTrue(foundControl, "Typical controller not loaded properly into typical system.");
            Assert.IsTrue(foundPanel, "Typical panel not loaded properly into typical system.");

            testForTag(actualSystem);
            testForCosts(actualSystem);
        }

        [TestMethod]
        public void Load_Bid_InstanceSystem()
        {
            Guid expectedGuid = new Guid("ba2e71d4-a2b9-471a-9229-9fbad7432bf7");
            string expectedName = "Instance System";
            string expectedDescription = "Instance System Description";
            int expectedQuantity = 1;
            double expectedBP = 100;

            TECSystem actualSystem = null;
            foreach (TECSystem typical in actualBid.Systems)
            {
                foreach(TECSystem instance in typical.SystemInstances)
                {
                    if (instance.Guid == expectedGuid)
                    {
                        actualSystem = instance;
                        break;
                    }
                }
                if (actualSystem != null) { break; }
            }

            //Assert
            Assert.AreEqual(expectedName, actualSystem.Name);
            Assert.AreEqual(expectedDescription, actualSystem.Description);
            Assert.AreEqual(expectedQuantity, actualSystem.Quantity);
            Assert.AreEqual(expectedBP, actualSystem.BudgetPriceModifier);

            testForScopeChildren(actualSystem);
        }

        [TestMethod]
        public void Load_Bid_TypicalSubScope()
        {
            //Arrange
            Guid expectedGuid = new Guid("fbe0a143-e7cd-4580-a1c4-26eff0cd55a6");
            Guid expectedConnectionGuid = new Guid("5723e279-ac5c-4ee0-ae01-494a0c524b5c");
            string expectedName = "Typical SS";
            string expectedDescription = "Typical SS Description";
            int expectedQuantity = 1;

            TECSubScope actualSubScope = null;
            foreach (TECSystem system in actualBid.Systems)
            {
                foreach (TECEquipment equipment in system.Equipment)
                {
                    foreach (TECSubScope subScope in equipment.SubScope)
                    {
                        if (subScope.Guid == expectedGuid)
                        {
                            actualSubScope = subScope;
                            break;
                        }
                    }
                    if (actualSubScope != null)
                    {
                        break;
                    }
                }
                if (actualSubScope != null)
                {
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualSubScope.Name, "Name not loaded");
            Assert.AreEqual(expectedDescription, actualSubScope.Description, "Description not loaded");
            Assert.AreEqual(expectedQuantity, actualSubScope.Quantity, "Quantity not loaded");
            Assert.AreEqual(expectedConnectionGuid, actualSubScope.Connection.Guid, "Connection not loaded");
            testForTag(actualSubScope);
            testForCosts(actualSubScope);
        }

        [TestMethod]
        public void Load_Bid_InstanceSubScope()
        {
            //Arrange
            Guid expectedGuid = new Guid("94726d87-b468-46a8-9421-3ff9725d5239");
            Guid expectedConnectionGuid = new Guid("560ffd84-444d-4611-a346-266074f62f6f");
            string expectedName = "Instance SS";
            string expectedDescription = "Instance SS Description";
            int expectedQuantity = 1;

            TECSubScope actualSubScope = null;
            foreach (TECSystem typical in actualBid.Systems)
            {
                foreach (TECSystem system in typical.SystemInstances)
                {
                    foreach (TECEquipment equipment in system.Equipment)
                    {
                        foreach (TECSubScope subScope in equipment.SubScope)
                        {
                            if (subScope.Guid == expectedGuid)
                            {
                                actualSubScope = subScope;
                                break;
                            }
                        }
                        if (actualSubScope != null)
                        {
                            break;
                        }
                    }
                    if (actualSubScope != null)
                    {
                        break;
                    }
                }
                if (actualSubScope != null)
                {
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualSubScope.Name, "Name not loaded");
            Assert.AreEqual(expectedDescription, actualSubScope.Description, "Description not loaded");
            Assert.AreEqual(expectedQuantity, actualSubScope.Quantity, "Quantity not loaded");
            Assert.AreEqual(expectedConnectionGuid, actualSubScope.Connection.Guid, "Connection not loaded");
            testForScopeChildren(actualSubScope);
        }

        [TestMethod]
        public void Load_Bid_Controller()
        {
            //Arrange
            Guid expectedGuid = new Guid("98e6bc3e-31dc-4394-8b54-9ca53c193f46");
            string expectedName = "Bid Controller";
            string expectedDescription = "Bid Controller Description";
            double expectedCost = 1812;
            NetworkType expectedType = NetworkType.Server;

            TECController actualController = null;
            foreach (TECController controller in actualBid.Controllers)
            {
                if (controller.Guid == expectedGuid)
                {
                    actualController = controller;
                    break;
                }
            }

            Guid expectedConnectionGuid = new Guid("4f93907a-9aab-4ed5-8e55-43aab2af5ef8");
            Guid expectedIOGuid = new Guid("1f6049cc-4dd6-4b50-a9d5-045b629ae6fb");

            bool hasIO = false;
            foreach (TECIO io in actualController.IO)
            {
                if (io.Guid == expectedIOGuid)
                {
                    hasIO = true;
                    break;
                }
            }

            bool hasConnection = false;
            foreach (TECConnection conn in actualController.ChildrenConnections)
            {
                if (conn.Guid == expectedConnectionGuid)
                {
                    hasConnection = true;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualController.Name);
            Assert.AreEqual(expectedDescription, actualController.Description);
            Assert.AreEqual(expectedCost, actualController.Cost);
            Assert.AreEqual(expectedType, actualController.NetworkType);
            Assert.IsTrue(hasIO);
            Assert.IsTrue(hasConnection);
            testForScopeChildren(actualController);
        }

        [TestMethod]
        public void Load_Bid_MiscCost()
        {
            //Arrange
            Guid expectedGuid = new Guid("5df99701-1d7b-4fbe-843d-40793f4145a8");
            string expectedName = "Bid Misc";
            double expectedCost = 1298;
            double expectedLabor = 8921;
            double expectedQuantity = 2;
            CostType expectedType = CostType.Electrical;
            TECMisc actualMisc = null;
            foreach (TECMisc misc in actualBid.MiscCosts)
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
        //----------------------------------------To break out, link tests-----------------------------------------------------

        [TestMethod]
        public void Load_Bid_EditSystemInstances()
        {
            Guid expectedGuid = new Guid("ebdbcc85-10f4-46b3-99e7-d896679f874a");

            TECSystem actualSystem = null;
            foreach (TECSystem system in actualBid.Systems)
            {
                if (system.Guid == expectedGuid)
                {
                    actualSystem = system;
                    break;
                }
            }

            actualSystem.Equipment.Add(TestHelper.CreateTestEquipment(actualBid.Catalogs));
            actualSystem.Controllers.Add(TestHelper.CreateTestController(actualBid.Catalogs));
            actualSystem.Panels.Add(TestHelper.CreateTestPanel(actualBid.Catalogs));

            foreach (TECSystem instance in actualSystem.SystemInstances)
            {
                Assert.AreEqual(actualSystem.Equipment.Count, instance.Equipment.Count);
                Assert.AreEqual(actualSystem.Controllers.Count, instance.Controllers.Count);
                Assert.AreEqual(actualSystem.Panels.Count, instance.Panels.Count);
            }

        }

        [TestMethod]
        public void Load_Bid_Equipment()
        {
            //Arrange
            Guid expectedSystemGuid = new Guid("ebdbcc85-10f4-46b3-99e7-d896679f874a");
            TECSystem actualSystem = null;
            foreach (TECSystem system in actualBid.Systems)
            {
                if (system.Guid == expectedSystemGuid)
                {
                    actualSystem = system;
                    break;
                }
            }

            Guid expectedGuid = new Guid("8a9bcc02-6ae2-4ac9-bbe1-e33d9a590b0e");
            TECEquipment actualEquipment = null;
            foreach (TECEquipment equipment in actualSystem.Equipment)
            {
                if (equipment.Guid == expectedGuid)
                {
                    actualEquipment = equipment;
                    break;
                }
            }

            string expectedName = "Typical Equip";
            string expectedDescription = "Typical Equip Description";
            int expectedQuantity = 1;
            double expectedBP = 50;

            string expectedInstanceName = "Instance Equip";
            string expectedInstanceDescription = "Instance Equip Description";
            int expectedInstanceQuantity = 1;
            double expectedInstanceBP = 50;
            Guid expectedInstanceGuid = new Guid("cdd9d7f7-ff3e-44ff-990f-c1b721e0ff8d");

            TECEquipment actualInstance = null;
            foreach (TECScope item in actualSystem.CharactersticInstances.GetInstances(actualEquipment))
            {
                if (item.Guid == expectedInstanceGuid)
                {
                    actualInstance = item as TECEquipment;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualEquipment.Name);
            Assert.AreEqual(expectedDescription, actualEquipment.Description);
            Assert.AreEqual(expectedQuantity, actualEquipment.Quantity);
            Assert.AreEqual(expectedBP, actualEquipment.BudgetUnitPrice);

            Assert.AreEqual(expectedInstanceName, actualInstance.Name);
            Assert.AreEqual(expectedInstanceDescription, actualInstance.Description);
            Assert.AreEqual(expectedInstanceQuantity, actualInstance.Quantity);
            Assert.AreEqual(expectedInstanceBP, actualInstance.BudgetUnitPrice);
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
                            if (!actualBid.Catalogs.Devices.Contains(device))
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
                foreach (TECCost cost in system.AssociatedCosts)
                {
                    if (!actualBid.Catalogs.AssociatedCosts.Contains(cost))
                    { Assert.Fail("Associated costs in system not linked"); }
                }
                foreach (TECEquipment equipment in system.Equipment)
                {
                    foreach (TECCost cost in equipment.AssociatedCosts)
                    {
                        if (!actualBid.Catalogs.AssociatedCosts.Contains(cost))
                        { Assert.Fail("Associated costs in equipment not linked"); }
                    }
                    foreach (TECSubScope subScope in equipment.SubScope)
                    {
                        foreach (TECCost cost in subScope.AssociatedCosts)
                        {
                            if (!actualBid.Catalogs.AssociatedCosts.Contains(cost))
                            { Assert.Fail("Associated costs in subscope not linked"); }
                        }
                        foreach (TECDevice device in subScope.Devices)
                        {
                            foreach (TECCost cost in device.AssociatedCosts)
                            {
                                if (!actualBid.Catalogs.AssociatedCosts.Contains(cost))
                                { Assert.Fail("Associated costs in subscope not linked"); }
                            }
                        }
                    }
                }
            }

            foreach (TECDevice device in actualBid.Catalogs.Devices)
            {
                foreach (TECCost cost in device.AssociatedCosts)
                {
                    if (!actualBid.Catalogs.AssociatedCosts.Contains(cost))
                    { Assert.Fail("Associated costs in device catalog not linked"); }
                }
            }
            foreach (TECConduitType conduitType in actualBid.Catalogs.ConduitTypes)
            {
                foreach (TECCost cost in conduitType.AssociatedCosts)
                {
                    if (!actualBid.Catalogs.AssociatedCosts.Contains(cost))
                    { Assert.Fail("Associated costs in conduit type catalog not linked"); }
                }
            }
            foreach (TECConnectionType connectionType in actualBid.Catalogs.ConnectionTypes)
            {
                foreach (TECCost cost in connectionType.AssociatedCosts)
                {
                    if (!actualBid.Catalogs.AssociatedCosts.Contains(cost))
                    { Assert.Fail("Associated costs in connection type catalog not linked"); }
                }
            }

            Assert.IsTrue(true, "All Associated costs Linked");
        }

        [TestMethod]
        public void Load_Bid_Linked_Manufacturers()
        {
            foreach (TECDevice device in actualBid.Catalogs.Devices)
            {
                if (device.Manufacturer == null)
                {
                    Assert.Fail("Device doesn't have manufacturer.");
                }
                if (!actualBid.Catalogs.Manufacturers.Contains(device.Manufacturer))
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
                if (!actualBid.Catalogs.Manufacturers.Contains(controller.Manufacturer))
                {
                    Assert.Fail("Manufacturers not linked in controllers");
                }
            }
            Assert.IsTrue(true, "All Manufacturers linked");
        }

        [TestMethod]
        public void Load_Bid_Linked_ConduitTypes()
        {
            foreach (TECController controller in actualBid.Controllers)
            {
                foreach (TECConnection connection in controller.ChildrenConnections)
                {
                    if (!actualBid.Catalogs.ConduitTypes.Contains(connection.ConduitType) && connection.ConduitType != null)
                    { Assert.Fail("Conduit types in connection not linked"); }
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
                    if (!actualBid.Catalogs.Tags.Contains(tag))
                    { Assert.Fail("Tags in system templates not linked"); }
                }
                foreach (TECEquipment equipment in system.Equipment)
                {
                    foreach (TECTag tag in equipment.Tags)
                    {
                        if (!actualBid.Catalogs.Tags.Contains(tag))
                        { Assert.Fail("Tags in system templates not linked"); }
                    }
                    foreach (TECSubScope subScope in equipment.SubScope)
                    {
                        foreach (TECTag tag in subScope.Tags)
                        {
                            if (!actualBid.Catalogs.Tags.Contains(tag))
                            { Assert.Fail("Tags in system templates not linked"); }
                        }
                        foreach (TECDevice device in subScope.Devices)
                        {
                            foreach (TECTag tag in device.Tags)
                            {
                                if (!actualBid.Catalogs.Tags.Contains(tag))
                                { Assert.Fail("Tags in system templates not linked"); }
                            }
                        }
                    }
                }
            }

            foreach (TECDevice device in actualBid.Catalogs.Devices)
            {
                foreach (TECTag tag in device.Tags)
                {
                    if (!actualBid.Catalogs.Tags.Contains(tag))
                    { Assert.Fail("Tags in device catalog not linked"); }
                }
            }
            foreach (TECConduitType conduitType in actualBid.Catalogs.ConduitTypes)
            {
                foreach (TECTag tag in conduitType.Tags)
                {
                    if (!actualBid.Catalogs.Tags.Contains(tag))
                    { Assert.Fail("Tags in conduit type catalog not linked"); }
                }
            }
            foreach (TECConnectionType connectionType in actualBid.Catalogs.ConnectionTypes)
            {
                foreach (TECTag tag in connectionType.Tags)
                {
                    if (!actualBid.Catalogs.Tags.Contains(tag))
                    { Assert.Fail("Tags in connection type catalog not linked"); }
                }
            }

            Assert.IsTrue(true, "All Tags Linked");
        }

        [TestMethod]
        public void Load_Bid_Linked_ConnectionTypes()
        {
            foreach (TECDevice device in actualBid.Catalogs.Devices)
            {
                if (device.ConnectionTypes.Count == 0)
                {
                    Assert.Fail("Device doesn't have connectionType");
                }
                foreach (TECConnectionType type in device.ConnectionTypes)
                {
                    if (!actualBid.Catalogs.ConnectionTypes.Contains(type))
                    {
                        Assert.Fail("ConnectionTypes not linked in device catalog");
                    }
                }
            }
        }

        [TestMethod]
        public void Load_Bid_Estimate()
        {
            Assert.AreEqual(2308.8142, actualBid.Estimate.TotalCost);
        }
        //----------------------------------------Tests above have new values, below do not-------------------------------------------
        
        

        [TestMethod]
        public void Load_Bid_Device()
        {
            //Arrange
            ObservableCollection<TECDevice> actualDevices = actualBid.Systems[0].Equipment[0].SubScope[0].Devices;
            TECDevice actualDevice = actualDevices[0];
            TECManufacturer actualManufacturer = actualBid.Catalogs.Manufacturers[0];

            //Assert
            string expectedName = "Test Device";
            Assert.AreEqual(expectedName, actualDevice.Name);

            string expectedDescription = "Test Device Description";
            Assert.AreEqual(expectedDescription, actualDevice.Description);

            int expectedQuantity = 3;
            int actualQuantity = 0;
            foreach (TECDevice device in actualDevices)
            {
                if (device.Guid == actualDevice.Guid)
                {
                    actualQuantity++;
                }
            }
            Assert.AreEqual(expectedQuantity, actualQuantity);

            double expectedCost = 654;
            Assert.AreEqual(expectedCost, actualDevice.Cost);

            Assert.AreEqual("ThreeC18", actualDevice.ConnectionTypes[0].Name);

            Assert.AreEqual(actualManufacturer.Guid, actualDevice.Manufacturer.Guid);
        }

        [TestMethod]
        public void Load_Bid_Manufacturer()
        {
            //Arrange
            TECManufacturer actualManufacturer = actualBid.Catalogs.Manufacturers[0];
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
        public void Load_Bid_Tag()
        {
            //Arrange
            TECTag actualTag = actualBid.Catalogs.Tags[0];
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
        public void Load_Bid_SubScopeConnection()
        {
            //Arrange
            TECSubScopeConnection actualConnection = actualBid.Controllers[0].ChildrenConnections[0] as TECSubScopeConnection;

            TECSubScope actualSubScope = null;
            foreach (TECSystem system in actualBid.Systems)
            {
                foreach (TECEquipment equipment in system.Equipment)
                {
                    foreach (TECSubScope subScope in equipment.SubScope)
                    {
                        if (subScope.Guid == actualConnection.SubScope.Guid)
                        {
                            actualSubScope = subScope;
                            break;
                        }
                    }
                    if (actualSubScope != null)
                    {
                        break;
                    }
                }
                if (actualSubScope != null)
                {
                    break;
                }
            }
            TECController actualController = null;
            foreach (TECController controller in actualBid.Controllers)
            {
                if (controller.Name == "Test Controller")
                {
                    actualController = controller;
                    break;
                }
            }


            double expectedLength = 521;

            bool hasSubScope = false;
            if (actualConnection.SubScope == actualSubScope)
            {
                hasSubScope = true;
            }


            //Assert
            Assert.AreEqual(expectedLength, actualConnection.Length);
            Assert.AreEqual(actualController, actualConnection.ParentController);
            Assert.IsTrue(hasSubScope, "Connection scope failed to load.");
        }
        
        [TestMethod]
        public void Load_Bid_AssociatedCosts()
        {
            //Arrange
            TECCost actualAssociatedCost = actualBid.Catalogs.AssociatedCosts[0];

            //Assert
            string expectedName = "Test Cost";
            Assert.AreEqual(expectedName, actualAssociatedCost.Name);
        }

        [TestMethod]
        public void Load_Bid_ConnectionType()
        {
            //Arrange
            TECConnectionType actualConnectionType = actualBid.Catalogs.ConnectionTypes[0];

            //Assert
            string expectedName = "ThreeC18";
            Assert.AreEqual(expectedName, actualConnectionType.Name);
        }

        [TestMethod]
        public void Load_Bid_ConduitType()
        {
            //Arrange
            TECConduitType actualConduitType = actualBid.Catalogs.ConduitTypes[0];

            //Assert
            string expectedName = "Test ConduitType";
            Assert.AreEqual(expectedName, actualConduitType.Name);
        }
        
        [TestMethod]
        public void Load_Bid_PanelType()
        {
            //Arrange
            TECPanelType actualCost = actualBid.Catalogs.PanelTypes[0];

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
        

        //[TestMethod]
        //public void Load_Bid_Drawing()
        //{
        //    //Arrange
        //    TECDrawing actualDrawing = actualBid.Drawings[0];

        //    //Assert
        //    string expectedName = "Test Drawing";
        //    string expectedDescription = "Test Drawing Description";

        //    Assert.AreEqual(expectedName, actualDrawing.Name);
        //    Assert.AreEqual(expectedDescription, actualDrawing.Description);
        //}

        //[TestMethod]
        //public void Load_Bid_Page()
        //{
        //    //Arrange
        //    TECPage actualPage = actualBid.Drawings[0].Pages[0];

        //    //Assert
        //    int expectedPageNum = 1;

        //    Assert.AreEqual(expectedPageNum, actualPage.PageNum);
        //}

        //[TestMethod]
        //public void Load_Bid_VisualScope()
        //{
        //    //Arrange
        //    TECVisualScope actualVisScope = actualBid.Drawings[0].Pages[0].PageScope[0];
        //    TECSystem actualSystem = actualBid.Systems[0];

        //    //Assert
        //    double expectedXPos = 119;
        //    double expectedYPos = 69.08;

        //    Assert.AreEqual(expectedXPos, actualVisScope.X);
        //    Assert.AreEqual(expectedYPos, actualVisScope.Y);
        //    Assert.AreEqual(actualSystem, actualVisScope.Scope);
        //}

        private void testForScopeChildren(TECScope scope)
        {
            testForTag(scope);
            testForCosts(scope);
            testForLocation(scope);
        }

        private void testForTag(TECScope scope)
        {
            bool foundTag = false;

            foreach (TECTag tag in scope.Tags)
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
        private void testForLocation(TECScope scope)
        {
            bool foundLocation = (scope.Location.Guid == TEST_LOCATION_GUID);
            Assert.IsTrue(foundLocation, "Location not loaded properly into scope.");
        }
    }
}
