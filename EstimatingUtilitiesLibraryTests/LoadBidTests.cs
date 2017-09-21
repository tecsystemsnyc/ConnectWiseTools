using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using EstimatingUtilitiesLibrary;
using EstimatingUtilitiesLibraryTests;
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
            var path = Path.GetTempFileName();
            TestDBHelper.CreateTestBid(path);
            actualBid = EULTestHelper.LoadTestBid(path);
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
            Assert.AreEqual(expectedPMCoef, actualBid.Parameters.PMCoef, "PM Coefficient didn't load properly.");
            Assert.AreEqual(expectedPMRate, actualBid.Parameters.PMRate, "PM Rate didn't load properly.");

            double expectedENGCoef = 2;
            double expectedENGRate = 40;
            Assert.AreEqual(expectedENGCoef, actualBid.Parameters.ENGCoef, "ENG Coefficient didn't load properly.");
            Assert.AreEqual(expectedENGRate, actualBid.Parameters.ENGRate, "ENG Rate didn't load properly.");

            double expectedCommCoef = 2;
            double expectedCommRate = 50;
            Assert.AreEqual(expectedCommCoef, actualBid.Parameters.CommCoef, "Comm Coefficient didn't load properly.");
            Assert.AreEqual(expectedCommRate, actualBid.Parameters.CommRate, "Comm Rate didn't load properly.");

            double expectedSoftCoef = 2;
            double expectedSoftRate = 60;
            Assert.AreEqual(expectedSoftCoef, actualBid.Parameters.SoftCoef, "Software Coefficient didn't load properly.");
            Assert.AreEqual(expectedSoftRate, actualBid.Parameters.SoftRate, "Software Rate didn't load properly.");

            double expectedGraphCoef = 2;
            double expectedGraphRate = 70;
            Assert.AreEqual(expectedGraphCoef, actualBid.Parameters.GraphCoef, "Graphics Coefficient didn't load properly.");
            Assert.AreEqual(expectedGraphRate, actualBid.Parameters.GraphRate, "Graphics Rate didn't load properly.");
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
            Assert.AreEqual(expectedElectricalRate, actualBid.Parameters.ElectricalRate, "Electrical rate didn't load properly.");
            Assert.AreEqual(expectedElectricalSuperRate, actualBid.Parameters.ElectricalSuperRate, "Electrical Supervision rate didn't load properly.");
            Assert.AreEqual(expectedElectricalNonUnionRate, actualBid.Parameters.ElectricalNonUnionRate, "Electrical Non-Union rate didn't load properly.");
            Assert.AreEqual(expectedElectricalSuperNonUnionRate, actualBid.Parameters.ElectricalSuperNonUnionRate, "Electrical Supervision Non-Union rate didn't load properly.");
            Assert.AreEqual(expectedElectricalSuperRatio, actualBid.Parameters.ElectricalSuperRatio, "Electrical Supervision time ratio didn't load properly.");
            Assert.AreEqual(expectedOT, actualBid.Parameters.ElectricalIsOnOvertime, "Electrical overtime bool didn't load properly.");
            Assert.AreEqual(expectedUnion, actualBid.Parameters.ElectricalIsUnion, "Electrical union bool didn't load properly.");
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

            Assert.AreEqual(expectedPMExtra, actualBid.ExtraLabor.PMExtraHours, "PM Extra Hours didn't load properly.");
            Assert.AreEqual(expectedENGExtra, actualBid.ExtraLabor.ENGExtraHours, "ENG Extra Hours didn't load properly.");
            Assert.AreEqual(expectedCommExtra, actualBid.ExtraLabor.CommExtraHours, "Comm Extra Hours didn't load properly.");
            Assert.AreEqual(expectedSoftExtra, actualBid.ExtraLabor.SoftExtraHours, "Soft Extra Hours didn't load properly.");
            Assert.AreEqual(expectedGraphExtra, actualBid.ExtraLabor.GraphExtraHours, "Graph Extra Hours didn't load properly.");
        }

        [TestMethod]
        public void Load_Bid_Note()
        {
            //Assert
            Guid expectedGuid = new Guid("50f3a707-fc1b-4eb3-9413-1dbde57b1d90");
            string expectedText = "Test Note";

            TECLabeled actualNote = null;
            foreach(TECLabeled note in actualBid.Notes)
            {
                if (note.Guid == expectedGuid)
                {
                    actualNote = note;
                    break;
                }
            }

            Assert.AreEqual(expectedText, actualNote.Label, "Note text didn't load properly.");
        }

        [TestMethod]
        public void Load_Bid_Exclusion()
        {
            //Assert
            Guid expectedGuid = new Guid("15692e12-e728-4f1b-b65c-de365e016e7a");
            string expectedText = "Test Exclusion";

            TECLabeled actualExclusion = null;
            foreach (TECLabeled note in actualBid.Exclusions)
            {
                if (note.Guid == expectedGuid)
                {
                    actualExclusion = note;
                    break;
                }
            }

            Assert.AreEqual(expectedText, actualExclusion.Label, "Exclusion text didn't load properly.");
        }

        [TestMethod]
        public void Load_Bid_BidScopeTree()
        {
            Guid expectedParentGuid = new Guid("25e815fa-4ac7-4b69-9640-5ae220f0cd40");
            string expectedParentName = "Bid Scope Branch";
            Guid expectedChildGuid = new Guid("81adfc62-20ec-466f-a2a0-430e1223f64f");
            string expectedChildName = "Bid Child Branch";

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

            Assert.AreEqual(1, actualBid.ScopeTree.Count);
            Assert.AreEqual(expectedParentName, actualParent.Label, "Parent scope branch name didn't load properly.");
            Assert.AreEqual(expectedChildName, actualChild.Label, "Child scope branch name didn't load properly.");
        }

        [TestMethod]
        public void Load_Bid_SystemScopeTree()
        {
            Guid expectedParentGuid = new Guid("814710f1-f2dd-4ae6-9bc4-9279288e4994");
            string expectedParentName = "System Scope Branch";
            Guid expectedChildGuid = new Guid("542802f6-a7b1-4020-9be4-e58225c433a8");
            string expectedChildName = "System Child Branch";

            TECScopeBranch actualParent = null;
            TECScopeBranch actualChild = null;
            foreach(TECTypical typical in actualBid.Systems)
            {
                foreach(TECScopeBranch branch in typical.ScopeBranches)
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
                if (actualParent != null) break;
            }

            Assert.AreEqual(expectedParentName, actualParent.Label, "Parent scope branch name didn't load properly.");
            Assert.AreEqual(expectedChildName, actualChild.Label, "Child scope branch name didn't load properly.");
        }
        
        [TestMethod]
        public void Load_Bid_TypicalSystem()
        {
            //Arrange
            Guid expectedGuid = new Guid("ebdbcc85-10f4-46b3-99e7-d896679f874a");
            string expectedName = "Typical System";
            string expectedDescription = "Typical System Description";
            bool expectedProposeEquipment = true;
            int expectedChildren = 1;

            Guid childEquipment = new Guid("8a9bcc02-6ae2-4ac9-bbe1-e33d9a590b0e");
            Guid childController = new Guid("1bb86714-2512-4fdd-a80f-46969753d8a0");
            Guid childPanel = new Guid("e7695d68-d79f-44a2-92f5-b303436186af");

            TECTypical actualSystem = null;
            foreach(TECTypical system in actualBid.Systems)
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
            Assert.AreEqual(expectedChildren, actualSystem.Instances.Count);
            Assert.AreEqual(expectedProposeEquipment, actualSystem.ProposeEquipment);

            foreach(TECSystem instance in actualSystem.Instances)
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

            TECSystem actualSystem = null;
            foreach (TECTypical typical in actualBid.Systems)
            {
                foreach(TECSystem instance in typical.Instances)
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

            testForScopeChildren(actualSystem);
        }

        [TestMethod]
        public void Load_Bid_TypicalEquipment()
        {
            Guid expectedGuid = new Guid("8a9bcc02-6ae2-4ac9-bbe1-e33d9a590b0e");
            string expectedName = "Typical Equip";
            string expectedDescription = "Typical Equip Description";

            Guid childSubScope = new Guid("fbe0a143-e7cd-4580-a1c4-26eff0cd55a6");

            TECEquipment actualEquipment = null;
            foreach (TECTypical typical in actualBid.Systems)
            {
                foreach(TECEquipment equip in typical.Equipment)
                {
                    if (equip.Guid == expectedGuid)
                    {
                        actualEquipment = equip;
                        break;
                    }
                }
                if (actualEquipment != null) break;
            }

            bool foundSubScope = false;
            foreach(TECSubScope ss in actualEquipment.SubScope)
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

            Assert.IsTrue(foundSubScope, "Typical subscope not loaded properly into typical equipment.");

            testForTag(actualEquipment);
            testForCosts(actualEquipment);
        }

        [TestMethod]
        public void Load_Bid_InstanceEquipment()
        {
            Guid expectedInstanceGuid = new Guid("cdd9d7f7-ff3e-44ff-990f-c1b721e0ff8d");
            string expectedInstanceName = "Instance Equip";
            string expectedInstanceDescription = "Instance Equip Description";

            Guid childSubScope = new Guid("94726d87-b468-46a8-9421-3ff9725d5239");

            TECEquipment actualEquipment = null;
            foreach (TECTypical typical in actualBid.Systems)
            {
                foreach (TECSystem instance in typical.Instances)
                {
                    foreach(TECEquipment equip in instance.Equipment)
                    {
                        if (equip.Guid == expectedInstanceGuid)
                        {
                            actualEquipment = equip;
                            break;
                        }
                    }
                    if (actualEquipment != null) break;
                }
                if (actualEquipment != null) break;
            }

            bool foundSubScope = false;
            foreach(TECSubScope ss in actualEquipment.SubScope)
            {
                if (ss.Guid == childSubScope)
                {
                    foundSubScope = true;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedInstanceName, actualEquipment.Name);
            Assert.AreEqual(expectedInstanceDescription, actualEquipment.Description);

            Assert.IsTrue(foundSubScope, "Instance subscope not loaded properly into instance equipment.");

            testForScopeChildren(actualEquipment);
        }

        [TestMethod]
        public void Load_Bid_TypicalSubScope()
        {
            //Arrange
            Guid expectedGuid = new Guid("fbe0a143-e7cd-4580-a1c4-26eff0cd55a6");
            string expectedName = "Typical SS";
            string expectedDescription = "Typical SS Description";

            Guid childPoint = new Guid("03a16819-9205-4e65-a16b-96616309f171");
            Guid childDevice = new Guid("95135fdf-7565-4d22-b9e4-1f177febae15");
            Guid expectedConnectionGuid = new Guid("5723e279-ac5c-4ee0-ae01-494a0c524b5c");

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

            bool foundPoint = false;
            foreach(TECPoint point in actualSubScope.Points)
            {
                if (point.Guid == childPoint)
                {
                    foundPoint = true;
                    break;
                }
            }
            bool foundDevice = false;
            foreach(TECDevice device in actualSubScope.Devices)
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
            Assert.AreEqual(expectedConnectionGuid, actualSubScope.Connection.Guid, "Connection not loaded");

            Assert.IsTrue(foundPoint, "Typical point not loaded into typical subscope properly.");
            Assert.IsTrue(foundDevice, "Typical device not loaded into typical subscope properly.");

            testForTag(actualSubScope);
            testForCosts(actualSubScope);
        }

        [TestMethod]
        public void Load_Bid_InstanceSubScope()
        {
            //Arrange
            Guid expectedGuid = new Guid("94726d87-b468-46a8-9421-3ff9725d5239");
            string expectedName = "Instance SS";
            string expectedDescription = "Instance SS Description";

            Guid childPoint = new Guid("e60437bc-09a1-47eb-9fd5-78711d942a12");
            Guid childDevice = new Guid("95135fdf-7565-4d22-b9e4-1f177febae15");
            Guid expectedConnectionGuid = new Guid("560ffd84-444d-4611-a346-266074f62f6f");

            TECSubScope actualSubScope = null;
            foreach (TECTypical typical in actualBid.Systems)
            {
                foreach (TECSystem system in typical.Instances)
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
            Assert.AreEqual(expectedConnectionGuid, actualSubScope.Connection.Guid, "Connection not loaded");

            Assert.IsTrue(foundPoint, "Instance point not loaded into typical subscope properly.");
            Assert.IsTrue(foundDevice, "Instance device not loaded into typical subscope properly.");

            testForScopeChildren(actualSubScope);
        }

        [TestMethod]
        public void Load_Bid_Device()
        {
            Guid expectedGuid = new Guid("95135fdf-7565-4d22-b9e4-1f177febae15");
            string expectedName = "Test Device";
            string expectedDescription = "Test Device Description";
            double expectedCost = 123.45;

            Guid manufacturerGuid = new Guid("90cd6eae-f7a3-4296-a9eb-b810a417766d");
            Guid connectionTypeGuid = new Guid("f38867c8-3846-461f-a6fa-c941aeb723c7");

            TECDevice actualDevice = null;
            foreach(TECDevice dev in actualBid.Catalogs.Devices)
            {
                if (dev.Guid == expectedGuid)
                {
                    actualDevice = dev;
                    break;
                }
            }

            bool foundConnectionType = false;
            foreach(TECElectricalMaterial connectType in actualDevice.ConnectionTypes)
            {
                if (connectType.Guid == connectionTypeGuid)
                {
                    foundConnectionType = true;
                    break;
                }
            }

            Assert.AreEqual(expectedName, actualDevice.Name, "Device name didn't load properly.");
            Assert.AreEqual(expectedDescription, actualDevice.Description, "Device description didn't load properly.");
            Assert.AreEqual(expectedCost, actualDevice.Price, "Device cost didn't load properly.");
            Assert.AreEqual(manufacturerGuid, actualDevice.Manufacturer.Guid, "Manufacturer didn't load properly into device.");

            Assert.IsTrue(foundConnectionType, "Connection type didn't load properly into device.");

            testForTag(actualDevice);
            testForCosts(actualDevice);
        }

        [TestMethod]
        public void Load_Bid_TypicalPoint()
        {
            Guid expectedGuid = new Guid("03a16819-9205-4e65-a16b-96616309f171");
            string expectedName = "Typical Point";
            int expectedQuantity = 1;
            IOType expectedType = IOType.AI;

            TECPoint actualPoint = null;
            foreach (TECTypical typical in actualBid.Systems)
            {
                foreach (TECEquipment equip in typical.Equipment)
                {
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        foreach (TECPoint point in ss.Points)
                        {
                            if (point.Guid == expectedGuid)
                            {
                                actualPoint = point;
                                break;
                            }
                        }
                        if (actualPoint != null) break;
                    }
                    if (actualPoint != null) break;
                }
                if (actualPoint != null) break;
            }

            Assert.AreEqual(expectedName, actualPoint.Label, "Typical point name didn't load properly.");
            Assert.AreEqual(expectedQuantity, actualPoint.Quantity, "Typical point quantity didn't load properly.");
            Assert.AreEqual(expectedType, actualPoint.Type, "Typical point type didn't load properly.");
        }

        [TestMethod]
        public void Load_Bid_InstancePoint()
        {
            Guid expectedGuid = new Guid("e60437bc-09a1-47eb-9fd5-78711d942a12");
            string expectedName = "Instance Point";
            int expectedQuantity = 1;
            IOType expectedType = IOType.AI;

            TECPoint actualPoint = null;
            foreach (TECTypical typical in actualBid.Systems)
            {
                foreach (TECSystem instance in typical.Instances)
                {
                    foreach (TECEquipment equip in instance.Equipment)
                    {
                        foreach (TECSubScope ss in equip.SubScope)
                        {
                            foreach (TECPoint point in ss.Points)
                            {
                                if (point.Guid == expectedGuid)
                                {
                                    actualPoint = point;
                                    break;
                                }
                            }
                            if (actualPoint != null) break;
                        }
                        if (actualPoint != null) break;
                    }
                    if (actualPoint != null) break;
                }
                if (actualPoint != null) break;
            }

            Assert.AreEqual(expectedName, actualPoint.Label, "Instance point name didn't load properly.");
            Assert.AreEqual(expectedQuantity, actualPoint.Quantity, "Instance point quantity didn't load properly.");
            Assert.AreEqual(expectedType, actualPoint.Type, "Instance point type didn't load properly.");
            
        }

        [TestMethod]
        public void Load_Bid_Tag()
        {
            Guid expectedGuid = new Guid("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            string expectedString = "Test Tag";

            TECLabeled actualTag = null;
            foreach (TECLabeled tag in actualBid.Catalogs.Tags)
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
        public void Load_Bid_Manufacturer()
        {
            //Arrange
            Guid expectedGuid = new Guid("90cd6eae-f7a3-4296-a9eb-b810a417766d");
            string expectedName = "Test Manufacturer";
            double expectedMultiplier = 0.5;


            TECManufacturer actualManufacturer = null;
            foreach (TECManufacturer man in actualBid.Catalogs.Manufacturers)
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

        [TestMethod]
        public void Load_Bid_Location()
        {
            //Arrange
            Guid expectedGuid = new Guid("4175d04b-82b1-486b-b742-b2cc875405cb");
            string expectedLocationName = "Test Location";

            TECLabeled actualLocation = null;
            foreach (TECLabeled location in actualBid.Locations)
            {
                if (location.Guid == expectedGuid)
                {
                    actualLocation = location;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedLocationName, actualLocation.Label);
        }

        [TestMethod]
        public void Load_Bid_ConnectionType()
        {
            //Arrange
            Guid expectedGuid = new Guid("f38867c8-3846-461f-a6fa-c941aeb723c7");
            string expectedName = "Test Connection Type";
            double expectedCost = 12.48;
            double expectedLabor = 84.21;

            TECElectricalMaterial actualConnectionType = null;
            foreach (TECElectricalMaterial connectionType in actualBid.Catalogs.ConnectionTypes)
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
        public void Load_Bid_ConduitType()
        {
            //Arrange
            Guid expectedGuid = new Guid("8d442906-efa2-49a0-ad21-f6b27852c9ef");
            string expectedName = "Test Conduit Type";
            double expectedCost = 45.67;
            double expectedLabor = 76.54;

            TECElectricalMaterial actualConduitType = null;
            foreach (TECElectricalMaterial conduitType in actualBid.Catalogs.ConduitTypes)
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
        public void Load_Bid_AssociatedCosts()
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
            foreach(TECCost cost in actualBid.Catalogs.AssociatedCosts)
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
        public void Load_Bid_TypicalSubScopeConnection()
        {
            Guid expectedGuid = new Guid("5723e279-ac5c-4ee0-ae01-494a0c524b5c");
            double expectedWireLength = 40;
            double expectedConduitLength = 20;

            Guid expectedParentControllerGuid = new Guid("1bb86714-2512-4fdd-a80f-46969753d8a0");
            Guid expectedConduitTypeGuid = new Guid("8d442906-efa2-49a0-ad21-f6b27852c9ef");
            Guid expectedSubScopeGuid = new Guid("fbe0a143-e7cd-4580-a1c4-26eff0cd55a6");

            TECSubScopeConnection actualSSConnect = null;
            foreach(TECTypical typical in actualBid.Systems)
            {
                foreach(TECController controller in typical.Controllers)
                {
                    foreach(TECConnection connection in controller.ChildrenConnections)
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

        [TestMethod]
        public void Load_Bid_InstanceSubScopeConnection()
        {
            //Arrange
            Guid expectedGuid = new Guid("560ffd84-444d-4611-a346-266074f62f6f");
            double expectedLength = 50;
            double expectedConduitLength = 30;
            Guid expectedSubScopeGuid = new Guid("94726d87-b468-46a8-9421-3ff9725d5239");
            Guid expectedControllerGuid = new Guid("f22913a6-e348-4a77-821f-80447621c6e0");
            Guid expectedConduitTypeGuid = new Guid("8d442906-efa2-49a0-ad21-f6b27852c9ef");

            TECSubScopeConnection actualConnection = null;
            foreach (TECTypical typical in actualBid.Systems)
            {
                foreach (TECSystem system in typical.Instances)
                {
                    foreach (TECController controller in system.Controllers)
                    {
                        foreach (TECConnection connection in controller.ChildrenConnections)
                        {
                            if (connection.Guid == expectedGuid)
                            {
                                actualConnection = connection as TECSubScopeConnection;
                                break;
                            }
                        }
                        if (actualConnection != null)
                        {
                            break;
                        }
                    }
                    if (actualConnection != null)
                    {
                        break;
                    }
                }
            }


            //Assert
            Assert.AreEqual(expectedLength, actualConnection.Length, "Length didn't load properly in subscope connection.");
            Assert.AreEqual(expectedConduitLength, actualConnection.ConduitLength, "ConduitLength didn't load properly in subscope connection.");
            Assert.AreEqual(expectedSubScopeGuid, actualConnection.SubScope.Guid, "Subscope didn't load properly in subscope connection.");
            Assert.AreEqual(expectedControllerGuid, actualConnection.ParentController.Guid, "Parent controller didn't load properly in subscope connection.");
            Assert.AreEqual(expectedConduitTypeGuid, actualConnection.ConduitType.Guid, "Conduit type didn't load properly in subscope connection.");
            Assert.IsFalse(actualConnection.IsTypical, "Loaded as typical.");
        }

        [TestMethod]
        public void Load_Bid_BidInstance_NetworkConnection()
        {
            Guid expectedGuid = new Guid("4f93907a-9aab-4ed5-8e55-43aab2af5ef8");
            double expectedLength = 100;
            double expectedConduitLength = 80;

            Guid expectedParentControllerGuid = new Guid("98e6bc3e-31dc-4394-8b54-9ca53c193f46");
            Guid expectedConnectionTypeGuid = new Guid("f38867c8-3846-461f-a6fa-c941aeb723c7");
            Guid expectedConduitTypeGuid = new Guid("8d442906-efa2-49a0-ad21-f6b27852c9ef");
            Guid expectedChildControllerGuid = new Guid("f22913a6-e348-4a77-821f-80447621c6e0");

            TECNetworkConnection actualNetConnect = null;
            foreach(TECController controller in actualBid.Controllers)
            {
                foreach(TECConnection connection in controller.ChildrenConnections)
                {
                    if (connection.Guid == expectedGuid)
                    {
                        actualNetConnect = (connection as TECNetworkConnection);
                        break;
                    }
                }
                if (actualNetConnect != null) break;
            }

            bool childControllerFound = false;
            foreach(TECController controller in actualNetConnect.ChildrenControllers)
            {
                if (controller.Guid == expectedChildControllerGuid)
                {
                    childControllerFound = true;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedLength, actualNetConnect.Length, "Length didn't load properly in network connection.");
            Assert.AreEqual(expectedConduitLength, actualNetConnect.ConduitLength, "ConduitLength didn't load properly in network connection.");

            Assert.AreEqual(expectedParentControllerGuid, actualNetConnect.ParentController.Guid, "Parent controller didn't load properly in network connection.");
            Assert.AreEqual(expectedConnectionTypeGuid, actualNetConnect.ConnectionType.Guid, "ConnectionType didn't load properly in network connection.");
            Assert.AreEqual(expectedConduitTypeGuid, actualNetConnect.ConduitType.Guid, "ConduitType didn't load properly in network connection.");
            Assert.IsTrue(childControllerFound, "Child controller didn't load properly in network connection.");
        }

        [TestMethod]
        public void Load_Bid_BidBidChild_NetworkConnection()
        {
            Guid expectedGuid = new Guid("6aca8c22-5115-4534-a5b1-698b7e42d6c2");
            double expectedLength = 80;
            double expectedConduitLength = 60;

            Guid expectedParentControllerGuid = new Guid("98e6bc3e-31dc-4394-8b54-9ca53c193f46");
            Guid expectedConnectionTypeGuid = new Guid("f38867c8-3846-461f-a6fa-c941aeb723c7");
            Guid expectedChildControllerGuid = new Guid("973e6100-31f7-40b0-bfe7-9d64630c1c56");

            TECNetworkConnection actualNetConnect = null;
            foreach (TECController controller in actualBid.Controllers)
            {
                foreach (TECConnection connection in controller.ChildrenConnections)
                {
                    if (connection.Guid == expectedGuid)
                    {
                        actualNetConnect = (connection as TECNetworkConnection);
                        break;
                    }
                }
                if (actualNetConnect != null) break;
            }

            bool childControllerFound = false;
            foreach (TECController controller in actualNetConnect.ChildrenControllers)
            {
                if (controller.Guid == expectedChildControllerGuid)
                {
                    childControllerFound = true;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedLength, actualNetConnect.Length, "Length didn't load properly in network connection.");
            Assert.AreEqual(expectedConduitLength, actualNetConnect.ConduitLength, "ConduitLength didn't load properly in network connection.");

            Assert.AreEqual(expectedParentControllerGuid, actualNetConnect.ParentController.Guid, "Parent controller didn't load properly in network connection.");
            Assert.AreEqual(expectedConnectionTypeGuid, actualNetConnect.ConnectionType.Guid, "ConnectionType didn't load properly in network connection.");
            Assert.IsTrue(childControllerFound, "Child controller didn't load properly in network connection.");
        }

        [TestMethod]
        public void Load_Bid_InstanceInstanceChild_NetworkConnection()
        {
            Guid expectedGuid = new Guid("e503fdd4-f299-4618-8d54-6751c3b2bc25");
            double expectedLength = 70;
            double expectedConduitLength = 50;

            Guid expectedParentControllerGuid = new Guid("f22913a6-e348-4a77-821f-80447621c6e0");
            Guid expectedConnectionTypeGuid = new Guid("f38867c8-3846-461f-a6fa-c941aeb723c7");
            Guid expectedChildControllerGuid = new Guid("ec965fe3-b1f7-4125-a545-ec47cc1e671b");

            TECNetworkConnection actualNetConnect = null;
            foreach (TECTypical typical in actualBid.Systems)
            {
                foreach(TECSystem instance in typical.Instances)
                {
                    foreach (TECController controller in instance.Controllers)
                    {
                        foreach (TECConnection connection in controller.ChildrenConnections)
                        {
                            if (connection.Guid == expectedGuid)
                            {
                                actualNetConnect = (connection as TECNetworkConnection);
                                break;
                            }
                        }
                        if (actualNetConnect != null) break;
                    }
                    if (actualNetConnect != null) break;
                }
                if (actualNetConnect != null) break;
            }

            bool childControllerFound = false;
            foreach (TECController controller in actualNetConnect.ChildrenControllers)
            {
                if (controller.Guid == expectedChildControllerGuid)
                {
                    childControllerFound = true;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedLength, actualNetConnect.Length, "Length didn't load properly in network connection.");
            Assert.AreEqual(expectedConduitLength, actualNetConnect.ConduitLength, "ConduitLength didn't load properly in network connection.");

            Assert.AreEqual(expectedParentControllerGuid, actualNetConnect.ParentController.Guid, "Parent controller didn't load properly in network connection.");
            Assert.AreEqual(expectedConnectionTypeGuid, actualNetConnect.ConnectionType.Guid, "ConnectionType didn't load properly in network connection.");
            Assert.IsTrue(childControllerFound, "Child controller didn't load properly in network connection.");
        }

        [TestMethod]
        public void Load_Bid_DaisyChain_NetworkConnection()
        {
            Guid expectedGuid = new Guid("99aea45e-ebeb-4c1a-8407-1d1a3540ceeb");
            double expectedLength = 90;
            double expectedConduitLength = 70;

            Guid expectedParentControllerGuid = new Guid("98e6bc3e-31dc-4394-8b54-9ca53c193f46");
            Guid expectedConnectionTypeGuid = new Guid("f38867c8-3846-461f-a6fa-c941aeb723c7");

            Guid expectedDaisy1Guid = new Guid("bf17527a-18ba-4765-a01e-8ab8de5664a3");
            Guid expectedDaisy2Guid = new Guid("7b6825df-57da-458a-a859-a9459c15907b");

            TECNetworkConnection actualNetConnect = null;
            foreach (TECController controller in actualBid.Controllers)
            {
                foreach (TECConnection connection in controller.ChildrenConnections)
                {
                    if (connection.Guid == expectedGuid)
                    {
                        actualNetConnect = (connection as TECNetworkConnection);
                        break;
                    }
                }
                if (actualNetConnect != null) break;
            }

            bool daisy1Found = false;
            bool daisy2Found = false;
            foreach(TECController controller in actualNetConnect.ChildrenControllers)
            {
                if(controller.Guid == expectedDaisy1Guid)
                {
                    daisy1Found = true;
                }
                else if (controller.Guid == expectedDaisy2Guid)
                {
                    daisy2Found = true;
                }
                if (daisy1Found && daisy2Found) break;
            }

            //Assert
            Assert.AreEqual(expectedLength, actualNetConnect.Length, "Length didn't load properly in network connection.");
            Assert.AreEqual(expectedConduitLength, actualNetConnect.ConduitLength, "ConduitLength didn't load properly in network connection.");

            Assert.AreEqual(expectedParentControllerGuid, actualNetConnect.ParentController.Guid, "Parent controller didn't load properly in network connection.");
            Assert.AreEqual(expectedConnectionTypeGuid, actualNetConnect.ConnectionType.Guid, "ConnectionType didn't load properly in network connection.");

            Assert.IsTrue(daisy1Found, "First daisy chain controller didn't load properly in network connection.");
            Assert.IsTrue(daisy2Found, "Second daisy chain controller didn't load properly in network connection.");
        }

        [TestMethod]
        public void Load_Bid_BidController()
        {
            //Arrange
            Guid expectedGuid = new Guid("98e6bc3e-31dc-4394-8b54-9ca53c193f46");
            string expectedName = "Bid Controller";
            string expectedDescription = "Bid Controller Description";
            double expectedCost = 142;
            NetworkType expectedType = NetworkType.Server;
            bool expectedGlobalStatus = true;

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
            foreach (TECIO io in actualController.Type.IO)
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
            Assert.AreEqual(expectedCost, actualController.Type.Price);
            Assert.AreEqual(expectedType, actualController.NetworkType);
            Assert.AreEqual(expectedGlobalStatus, actualController.IsGlobal);
            Assert.IsTrue(hasIO);
            Assert.IsTrue(hasConnection);
            testForTag(actualController);
            testForCosts(actualController);
        }

        [TestMethod]
        public void Load_Bid_SystemTypicalController()
        {
            //Arrange
            Guid expectedGuid = new Guid("1bb86714-2512-4fdd-a80f-46969753d8a0");
            string expectedName = "Typical Controller";
            string expectedDescription = "Typical Controller Description";
            double expectedCost = 142;
            NetworkType expectedType = 0;
            bool expectedGlobalStatus = false;

            TECController actualController = null;
            foreach (TECSystem system in actualBid.Systems)
            {
                foreach (TECController controller in system.Controllers)
                {
                    if (controller.Guid == expectedGuid)
                    {
                        actualController = controller;
                        break;
                    }
                }
            }

            Guid expectedConnectionGuid = new Guid("5723e279-ac5c-4ee0-ae01-494a0c524b5c");
            Guid expectedIOGuid = new Guid("1f6049cc-4dd6-4b50-a9d5-045b629ae6fb");

            bool hasIO = false;
            foreach (TECIO io in actualController.Type.IO)
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
            Assert.AreEqual(expectedCost, actualController.Type.Price);
            Assert.AreEqual(expectedType, actualController.NetworkType);
            Assert.AreEqual(expectedGlobalStatus, actualController.IsGlobal);
            Assert.IsTrue(hasIO, "IO not loaded");
            Assert.IsTrue(hasConnection, "Connection not loaded");
            testForTag(actualController);
            testForCosts(actualController);
        }

        [TestMethod]
        public void Load_Bid_SystemInstanceController()
        {
            //Arrange
            Guid expectedGuid = new Guid("f22913a6-e348-4a77-821f-80447621c6e0");
            string expectedName = "Instance Controller";
            string expectedDescription = "Instance Controller Description";
            double expectedCost = 142;
            NetworkType expectedType = NetworkType.DDC;
            bool expectedGlobalStatus = false;

            TECController actualController = null;
            foreach (TECTypical typical in actualBid.Systems)
            {
                foreach (TECSystem system in typical.Instances)
                {
                    foreach (TECController controller in system.Controllers)
                    {
                        if (controller.Guid == expectedGuid)
                        {
                            actualController = controller;
                            break;
                        }
                    }
                }
            }

            Guid expectedConnectionGuid = new Guid("560ffd84-444d-4611-a346-266074f62f6f");
            Guid expectedIOGuid = new Guid("1f6049cc-4dd6-4b50-a9d5-045b629ae6fb");

            bool hasIO = false;
            foreach (TECIO io in actualController.Type.IO)
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
            Assert.AreEqual(expectedCost, actualController.Type.Price);
            Assert.AreEqual(expectedType, actualController.NetworkType);
            Assert.AreEqual(expectedGlobalStatus, actualController.IsGlobal);
            Assert.IsTrue(hasIO, "IO not loaded");
            Assert.IsTrue(hasConnection, "Connection not loaded");
            testForTag(actualController);
            testForCosts(actualController);
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

        [TestMethod]
        public void Load_Bid_SystemMiscCost()
        {
            //Arrange
            Guid expectedGuid = new Guid("e3ecee54-1f90-415a-b493-90a78f618476");
            string expectedName = "System Misc";
            double expectedCost = 1492;
            double expectedLabor = 2941;
            double expectedQuantity = 3;
            CostType expectedType = CostType.TEC;
            TECMisc actualMisc = null;
            foreach(TECSystem system in actualBid.Systems)
            {
                foreach (TECMisc misc in system.MiscCosts)
                {
                    if (misc.Guid == expectedGuid)
                    {
                        actualMisc = misc;
                        break;
                    }
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
        public void Load_Bid_PanelType()
        {
            //Arrange
            Guid expectedGuid = new Guid("04e3204c-b35f-4e1a-8a01-db07f7eb055e");
            string expectedName = "Test Panel Type";
            double expectedCost = 1324;
            double expectedLabor = 4231;

            Guid manufacturerGuid = new Guid("90cd6eae-f7a3-4296-a9eb-b810a417766d");

            TECPanelType actualType = null;
            foreach (TECPanelType type in actualBid.Catalogs.PanelTypes)
            {
                if (type.Guid == expectedGuid)
                {
                    actualType = type;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualType.Name);
            Assert.AreEqual(expectedCost, actualType.Price);
            Assert.AreEqual(expectedLabor, actualType.Labor);
            Assert.AreEqual(manufacturerGuid, actualType.Manufacturer.Guid);
        }
    
        [TestMethod]
        public void Load_Bid_ControllerType()
        {
            //Arrange
            Guid expectedGuid = new Guid("7201ca48-f885-4a87-afa7-61b3e6942697");
            string expectedName = "Test Controller Type";
            double expectedCost = 142;
            double expectedLabor = 12;

            Guid manufacturerGuid = new Guid("90cd6eae-f7a3-4296-a9eb-b810a417766d");
            Guid ioGuid = new Guid("1f6049cc-4dd6-4b50-a9d5-045b629ae6fb");

            TECControllerType actualType = null;
            foreach (TECControllerType type in actualBid.Catalogs.ControllerTypes)
            {
                if (type.Guid == expectedGuid)
                {
                    actualType = type;
                }
            }

            bool foundIO = false;
            foreach (TECIO io in actualType.IO)
            {
                if (io.Guid == ioGuid)
                {
                    foundIO = true;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualType.Name);
            Assert.AreEqual(expectedCost, actualType.Price);
            Assert.AreEqual(expectedLabor, actualType.Labor);
            Assert.AreEqual(manufacturerGuid, actualType.Manufacturer.Guid);
            Assert.IsTrue(foundIO);
        }

        [TestMethod]
        public void Load_Bid_BidPanel()
        {
            //Arrange
            Guid expectedGuid = new Guid("a8cdd31c-e690-4eaa-81ea-602c72904391");
            string expectedName = "Bid Panel";
            string expectedDescription = "Bid Panel Description";

            Guid expectedTypeGuid = new Guid("04e3204c-b35f-4e1a-8a01-db07f7eb055e");

            TECPanel actualPanel = null;
            foreach (TECPanel panel in actualBid.Panels)
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
        public void Load_Bid_TypicalPanel()
        {
            //Arrange
            Guid expectedGuid = new Guid("e7695d68-d79f-44a2-92f5-b303436186af");
            string expectedName = "Typical Panel";
            string expectedDescription = "Typical Panel Description";

            Guid expectedTypeGuid = new Guid("04e3204c-b35f-4e1a-8a01-db07f7eb055e");

            TECPanel actualPanel = null;
            foreach (TECSystem system in actualBid.Systems)
            {
                foreach (TECPanel panel in system.Panels)
                {
                    if (panel.Guid == expectedGuid)
                    {
                        actualPanel = panel;
                        break;
                    }
                }
                if (actualPanel != null) break;
            }

            //Assert
            Assert.AreEqual(expectedName, actualPanel.Name);
            Assert.AreEqual(expectedDescription, actualPanel.Description);
            Assert.AreEqual(expectedTypeGuid, actualPanel.Type.Guid);
            testForCosts(actualPanel);
        }

        [TestMethod]
        public void Load_Bid_InstancePanel()
        {
            //Arrange
            Guid expectedGuid = new Guid("10b07f6c-4374-49fc-ba6f-84db65b61ffa");
            string expectedName = "Instance Panel";
            string expectedDescription = "Instance Panel Description";

            Guid expectedTypeGuid = new Guid("04e3204c-b35f-4e1a-8a01-db07f7eb055e");

            TECPanel actualPanel = null;
            foreach (TECTypical typical in actualBid.Systems)
            {
                foreach (TECSystem system in typical.Instances)
                {
                    foreach (TECPanel panel in system.Panels)
                    {
                        if (panel.Guid == expectedGuid)
                        {
                            actualPanel = panel;
                            break;
                        }
                    }
                    if (actualPanel != null) break;
                }
                if (actualPanel != null) break;
            }

            //Assert
            Assert.AreEqual(expectedName, actualPanel.Name);
            Assert.AreEqual(expectedDescription, actualPanel.Description);
            Assert.AreEqual(expectedTypeGuid, actualPanel.Type.Guid);
            testForCosts(actualPanel);
        }

        [TestMethod]
        public void Load_Bid_IOModule()
        {
            //Arrange
            Guid expectedGuid = new Guid("b346378d-dc72-4dda-b275-bbe03022dd12");
            string expectedName = "Test IO Module";
            string expectedDescription = "Test IO Module Description";
            double expectedCost = 2233;
            double expectedIOPerModule = 10;

            Guid manufacturerGuid = new Guid("90cd6eae-f7a3-4296-a9eb-b810a417766d");

            TECIOModule actualModule = null;
            foreach (TECIOModule module in actualBid.Catalogs.IOModules)
            {
                if (module.Guid == expectedGuid)
                {
                    actualModule = module;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualModule.Name);
            Assert.AreEqual(expectedDescription, actualModule.Description);
            Assert.AreEqual(expectedCost, actualModule.Price);
            Assert.AreEqual(expectedIOPerModule, actualModule.IO.Count);
            Assert.AreEqual(manufacturerGuid, actualModule.Manufacturer.Guid);

        }

        [TestMethod]
        public void Load_Bid_IO()
        {
            //Arrange
            Guid expectedGuid = new Guid("1f6049cc-4dd6-4b50-a9d5-045b629ae6fb");
            IOType expectedType = IOType.BACnetIP;
            int expectedQty = 2;

            Guid expectedModuleGuid = new Guid("b346378d-dc72-4dda-b275-bbe03022dd12");

            TECIO actualIO = null;
            foreach (TECController controller in actualBid.Controllers)
            {
                foreach (TECIO io in controller.Type.IO)
                {
                    if (io.Guid == expectedGuid)
                    {
                        actualIO = io;
                    }
                }
            }

            //Assert
            Assert.AreEqual(expectedType, actualIO.Type, "Type not loaded");
            Assert.AreEqual(expectedQty, actualIO.Quantity, "Quantity not loaded");
            //Assert.AreEqual(expectedModuleGuid, actualIO.IOModule.Guid, "IOModule not loaded");
        }

        [TestMethod]
        public void Load_Bid_TypicalIO()
        {
            //Arrange
            Guid expectedGuid = new Guid("1f6049cc-4dd6-4b50-a9d5-045b629ae6fb");
            IOType expectedType = IOType.BACnetIP;
            int expectedLabor = 2;

            Guid expectedModuleGuid = new Guid("b346378d-dc72-4dda-b275-bbe03022dd12");

            TECIO actualIO = null;
            foreach (TECSystem system in actualBid.Systems)
            {
                foreach (TECController controller in system.Controllers)
                {
                    foreach (TECIO io in controller.Type.IO)
                    {
                        if (io.Guid == expectedGuid)
                        {
                            actualIO = io;
                        }
                    }
                }
            }

            //Assert
            Assert.AreEqual(expectedType, actualIO.Type, "Type not loaded");
            Assert.AreEqual(expectedLabor, actualIO.Quantity, "Quantity not loaded");
            //Assert.AreEqual(expectedModuleGuid, actualIO.IOModule.Guid, "IOModule not loaded");
        }

        [TestMethod]
        public void Load_Bid_InstanceIO()
        {
            //Arrange
            Guid expectedGuid = new Guid("1f6049cc-4dd6-4b50-a9d5-045b629ae6fb");
            IOType expectedType = IOType.BACnetIP;
            int expectedLabor = 2;

            Guid expectedModuleGuid = new Guid("b346378d-dc72-4dda-b275-bbe03022dd12");

            TECIO actualIO = null;
            foreach (TECTypical typical in actualBid.Systems)
            {
                foreach (TECSystem system in typical.Instances)
                    foreach (TECController controller in system.Controllers)
                    {
                        foreach (TECIO io in controller.Type.IO)
                        {
                            if (io.Guid == expectedGuid)
                            {
                                actualIO = io;
                            }
                        }
                    }
            }

            //Assert
            Assert.AreEqual(expectedType, actualIO.Type, "Type not loaded");
            Assert.AreEqual(expectedLabor, actualIO.Quantity, "Quantity not loaded");
            //Assert.AreEqual(expectedModuleGuid, actualIO.IOModule.Guid, "IOModule not loaded");
        }

        //----------------------------------------Tests above have new values, below do not-------------------------------------------


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
            if(scope is TECLocated)
            {
                testForLocation(scope as TECLocated);

            }
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
        private void testForLocation(TECLocated scope)
        {
            bool foundLocation = (scope.Location.Guid == TEST_LOCATION_GUID);
            Assert.IsTrue(foundLocation, "Location not loaded properly into scope.");
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
