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
            //Assert
            double expectedPMCoef = 2;
            double expectedPMRate = 30;
            Assert.AreEqual(expectedPMCoef, actualTemplates.Labor.PMCoef, "PM Coefficient didn't load properly.");
            Assert.AreEqual(expectedPMRate, actualTemplates.Labor.PMRate, "PM Rate didn't load properly.");

            double expectedENGCoef = 2;
            double expectedENGRate = 40;
            Assert.AreEqual(expectedENGCoef, actualTemplates.Labor.ENGCoef, "ENG Coefficient didn't load properly.");
            Assert.AreEqual(expectedENGRate, actualTemplates.Labor.ENGRate, "ENG Rate didn't load properly.");

            double expectedCommCoef = 2;
            double expectedCommRate = 50;
            Assert.AreEqual(expectedCommCoef, actualTemplates.Labor.CommCoef, "Comm Coefficient didn't load properly.");
            Assert.AreEqual(expectedCommRate, actualTemplates.Labor.CommRate, "Comm Rate didn't load properly.");

            double expectedSoftCoef = 2;
            double expectedSoftRate = 60;
            Assert.AreEqual(expectedSoftCoef, actualTemplates.Labor.SoftCoef, "Software Coefficient didn't load properly.");
            Assert.AreEqual(expectedSoftRate, actualTemplates.Labor.SoftRate, "Software Rate didn't load properly.");

            double expectedGraphCoef = 2;
            double expectedGraphRate = 70;
            Assert.AreEqual(expectedGraphCoef, actualTemplates.Labor.GraphCoef, "Graphics Coefficient didn't load properly.");
            Assert.AreEqual(expectedGraphRate, actualTemplates.Labor.GraphRate, "Graphics Rate didn't load properly.");
        }

        [TestMethod]
        public void Load_Templates_SubcontractorConsts()
        {
            //Assert
            double expectedElectricalRate = 50;
            double expectedElectricalSuperRate = 60;
            double expectedElectricalNonUnionRate = 30;
            double expectedElectricalSuperNonUnionRate = 40;
            double expectedElectricalSuperRatio = 0.25;
            bool expectedOT = false;
            bool expectedUnion = true;
            Assert.AreEqual(expectedElectricalRate, actualTemplates.Labor.ElectricalRate, "Electrical rate didn't load properly.");
            Assert.AreEqual(expectedElectricalSuperRate, actualTemplates.Labor.ElectricalSuperRate, "Electrical Supervision rate didn't load properly.");
            Assert.AreEqual(expectedElectricalNonUnionRate, actualTemplates.Labor.ElectricalNonUnionRate, "Electrical Non-Union rate didn't load properly.");
            Assert.AreEqual(expectedElectricalSuperNonUnionRate, actualTemplates.Labor.ElectricalSuperNonUnionRate, "Electrical Supervision Non-Union rate didn't load properly.");
            Assert.AreEqual(expectedElectricalSuperRatio, actualTemplates.Labor.ElectricalSuperRatio, "Electrical Supervision time ratio didn't load properly.");
            Assert.AreEqual(expectedOT, actualTemplates.Labor.ElectricalIsOnOvertime, "Electrical overtime bool didn't load properly.");
            Assert.AreEqual(expectedUnion, actualTemplates.Labor.ElectricalIsUnion, "Electrical union bool didn't load properly.");
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

            //Assert
            Assert.AreEqual(expectedName, actualSystem.Name);
            Assert.AreEqual(expectedDescription, actualSystem.Description);
            Assert.AreEqual(expectedQuantity, actualSystem.Quantity);
            Assert.AreEqual(expectedBP, actualSystem.BudgetPriceModifier);
            Assert.AreEqual(expectedProposeEquipment, actualSystem.ProposeEquipment);

            foreach (TECSystem instance in actualSystem.SystemInstances)
            {
                Assert.AreEqual(actualSystem.Equipment.Count, instance.Equipment.Count);
                Assert.AreEqual(actualSystem.Panels.Count, instance.Panels.Count);
                Assert.AreEqual(actualSystem.Controllers.Count, instance.Controllers.Count);
            }

            Assert.IsTrue(foundEquip, "Equipment not loaded properly into system.");
            Assert.IsTrue(foundControl, "Controller not loaded properly into system.");
            Assert.IsTrue(foundPanel, "Panel not loaded properly into system.");

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
            Assert.AreEqual(expectedQuantity, actualEquipment.Quantity);
            Assert.AreEqual(expectedBP, actualEquipment.BudgetUnitPrice);

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
            Assert.AreEqual(expectedQuantity, actualSubScope.Quantity, "Quantity not loaded");

            Assert.IsTrue(foundPoint, "Point not loaded into subscope properly.");
            Assert.IsTrue(foundDevice, "Device not loaded into subscope properly.");

            testForTag(actualSubScope);
            testForCosts(actualSubScope);
        }

        //----------------------------------Tests above have new values, below do not----------------------------------------------
        

        //[TestMethod]
        //public void Load_Templates_SubScope()
        //{
        //    //Arrange
        //    TECDevice childDevice = actualSubScope.Devices[0];
        //    TECPoint ssPoint = actualSubScope.Points[0];
        //    TECManufacturer childMan = childDevice.Manufacturer;

        //    //Assert
        //    Assert.AreEqual("Test SubScope", actualSubScope.Name);
        //    Assert.AreEqual("SubScope Description", actualSubScope.Description);
        //    Assert.AreEqual("SubScope Tag", actualSubScope.Tags[0].Text);
        //    Assert.AreEqual("Test SubScope", actualSubScope.Name);
        //    Assert.AreEqual("Test Cost", actualSubScope.AssociatedCosts[0].Name);

        //    Assert.AreEqual("Child Device", childDevice.Name);
        //    Assert.AreEqual("Child Device", childDevice.Description);
        //    Assert.AreEqual(89.3, childDevice.Cost);
        //    Assert.AreEqual("TwoC18", childDevice.ConnectionTypes[0].Name);
        //    Assert.AreEqual("Device Tag", childDevice.Tags[0].Text);

        //    Assert.AreEqual("SubScope Point", ssPoint.Name);
        //    Assert.AreEqual("Child Point", ssPoint.Description);
        //    Assert.AreEqual(349, ssPoint.Quantity);
        //    Assert.AreEqual(PointTypes.BO, ssPoint.Type);

        //    Assert.AreEqual("Child Manufacturer (Child Device)", childMan.Name);
        //    Assert.AreEqual(0.3, childMan.Multiplier);
        //}

        //[TestMethod]
        //public void Load_Templates_Device()
        //{
        //    //Arrange
        //    TECManufacturer childMan = actualDevice.Manufacturer;

        //    //Assert
        //    Assert.AreEqual("Test Device", actualDevice.Name);
        //    Assert.AreEqual("Device Description", actualDevice.Description);
        //    Assert.AreEqual(72.9, actualDevice.Cost);
        //    Assert.AreEqual("Cat6", actualDevice.ConnectionTypes[0].Name);
        //    Assert.AreEqual("Device Tag", actualDevice.Tags[0].Text);

        //    Assert.AreEqual("Child Manufacturer (Test Device)", childMan.Name);
        //    Assert.AreEqual(0.123, childMan.Multiplier);
        //}

        //[TestMethod]
        //public void Load_Templates_Manufacturer()
        //{
        //    //Assert
        //    Assert.AreEqual("Test Manufacturer", actualManufacturer.Name);
        //    Assert.AreEqual(0.65, actualManufacturer.Multiplier);
        //}

        //[TestMethod]
        //public void Load_Templates_Tag()
        //{
        //    //Assert
        //    Assert.AreEqual("Test Tag", actualTag.Text);
        //}

        //[TestMethod]
        //public void Load_Templates_Controller()
        //{
        //    //Assert
        //    Assert.AreEqual("Test Controller", actualController.Name);
        //    Assert.AreEqual("test description", actualController.Description);
        //    Assert.AreEqual(101, actualController.Cost);
        //    Assert.AreEqual(2, actualController.IO.Count);
        //    Assert.AreEqual(IOType.AI, actualController.IO[0].Type);
        //    Assert.AreEqual("Test Manufacturer", actualController.Manufacturer.Name);
        //}

        //[TestMethod]
        //public void Load_Templates_ConnectionType()
        //{
        //    bool found = false;
        //    foreach (TECCost cost in actualConnectionType.AssociatedCosts)
        //    {
        //        if (cost.Name == "Test Cost")
        //        {
        //            found = true;
        //            break;
        //        }
        //    }

        //    Assert.AreEqual("Test ConnectionType", actualConnectionType.Name);
        //    Assert.AreEqual(10, actualConnectionType.Cost);
        //    Assert.AreEqual(12, actualConnectionType.Labor);
        //    Assert.IsTrue(found);
        //    Assert.AreEqual(2, actualConnectionType.AssociatedCosts.Count);
        //}

        //[TestMethod]
        //public void Load_Templates_ConduitType()
        //{
        //    Assert.AreEqual("Test ConduitType", actualConduitType.Name);
        //    Assert.AreEqual(12, actualConduitType.Cost);
        //    Assert.AreEqual(13, actualConduitType.Labor);
        //    Assert.AreEqual("Test Cost", actualConduitType.AssociatedCosts[0].Name);

        //}

        //[TestMethod]
        //public void Load_Templates_AssociatedCost()
        //{
        //    Assert.AreEqual("Test Cost", actualAssociatedCost.Name);
        //    Assert.AreEqual(42, actualAssociatedCost.Cost);
        //}

        //[TestMethod]
        //public void Load_Templates_MiscCost()
        //{
        //    //Arrange
        //    TECMisc actualCost = actualTemplates.MiscCostTemplates[0];

        //    //Assert
        //    Assert.AreEqual("Test Misc Cost", actualCost.Name);
        //    Assert.AreEqual(654.9648, actualCost.Cost);
        //    Assert.AreEqual(19, actualCost.Quantity);
        //}


        //[TestMethod]
        //public void Load_Templates_IOModules()
        //{
        //    //Arrange
        //    //TECIOModule actualIOModule = actualTemplates.Catalogs.IOModules[0];

        //    //Assert
        //    Assert.AreEqual("Test IO Module", actualIOModule.Name);
        //    Assert.AreEqual(42, actualIOModule.Cost);
        //    Assert.AreEqual(2, actualIOModule.IOPerModule);
        //}

        //[TestMethod]
        //public void Load_Templates_PanelType()
        //{
        //    //Arrange
        //    TECPanelType actualCost = actualTemplates.Catalogs.PanelTypes[0];

        //    //Assert
        //    Assert.AreEqual("Test Panel Type", actualCost.Name);
        //    Assert.AreEqual(654.9648, actualCost.Cost);
        //}

        //[TestMethod]
        //public void Load_Templates_Panel()
        //{
        //    //Arrange
        //    TECPanel actualPanel = actualTemplates.PanelTemplates[0];
        //    TECPanelType actualPanelType = actualPanel.Type;

        //    //Assert
        //    foreach (TECPanel panel in actualTemplates.PanelTemplates)
        //    {
        //        if (panel.Name == "Controlled Panel")
        //        {
        //            Assert.Fail();
        //        }
        //    }

        //    Assert.AreEqual("Test Panel", actualPanel.Name);
        //    Assert.AreEqual("Test Panel Type", actualPanelType.Name);
        //}

        //[TestMethod]
        //public void Load_Templates_ControlledScope_Linking()
        //{
        //    //Arrange
        //    TECSystem actualConScope = actualTemplates.SystemTemplates[0];
        //    var connectionsInSystemsLinked = true;
        //    var connectionsInControllersLinked = true;
        //    var controllersInPanelsLinked = true;

        //    foreach (TECEquipment equipment in actualConScope.Equipment)
        //    {
        //        foreach (TECSubScope subScope in equipment.SubScope)
        //        {
        //            if (!actualConScope.Controllers[0].ChildrenConnections.Contains(subScope.Connection))
        //            {
        //                connectionsInSystemsLinked = false;
        //            }
        //        }
        //    }
        //    foreach (TECPanel panel in actualConScope.Panels)
        //    {
        //        foreach (TECController controller in panel.Controllers)
        //        {
        //            if (!actualConScope.Controllers.Contains(controller))
        //            {
        //                controllersInPanelsLinked = false;
        //            }
        //        }
        //    }

        //    Assert.IsTrue(connectionsInSystemsLinked);
        //    Assert.IsTrue(connectionsInControllersLinked);
        //    Assert.IsTrue(controllersInPanelsLinked);
        //}

        //[TestMethod]
        //public void Load_Templates_Linked_Devices()
        //{
        //    foreach (TECSystem system in actualTemplates.SystemTemplates)
        //    {
        //        foreach (TECEquipment equipment in system.Equipment)
        //        {
        //            foreach (TECSubScope subScope in equipment.SubScope)
        //            {
        //                foreach (TECDevice device in subScope.Devices)
        //                {
        //                    if (!actualTemplates.Catalogs.Devices.Contains(device))
        //                    {
        //                        Assert.Fail("Devices in system templates not linked");
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    foreach (TECEquipment equipment in actualTemplates.EquipmentTemplates)
        //    {
        //        foreach (TECSubScope subScope in equipment.SubScope)
        //        {
        //            foreach (TECDevice device in subScope.Devices)
        //            {
        //                if (!actualTemplates.Catalogs.Devices.Contains(device))
        //                {
        //                    Assert.Fail("Devices in equipment templates not linked");
        //                }
        //            }
        //        }
        //    }
        //    foreach (TECSubScope subScope in actualTemplates.SubScopeTemplates)
        //    {
        //        foreach (TECDevice device in subScope.Devices)
        //        {
        //            if (!actualTemplates.Catalogs.Devices.Contains(device))
        //            {
        //                Assert.Fail("Devices in subscope templates not linked");
        //            }
        //        }
        //    }
        //    Assert.IsTrue(true, "All Devices Linked");
        //}

        //[TestMethod]
        //public void Load_Templates_Linked_AssociatedCosts()
        //{
        //    foreach (TECSystem system in actualTemplates.SystemTemplates)
        //    {
        //        foreach (TECCost cost in system.AssociatedCosts)
        //        {
        //            if (!actualTemplates.Catalogs.AssociatedCosts.Contains(cost))
        //            { Assert.Fail("Associated costs in system templates not linked"); }
        //        }
        //        foreach (TECEquipment equipment in system.Equipment)
        //        {
        //            foreach (TECCost cost in equipment.AssociatedCosts)
        //            {
        //                if (!actualTemplates.Catalogs.AssociatedCosts.Contains(cost))
        //                { Assert.Fail("Associated costs in system templates not linked"); }
        //            }
        //            foreach (TECSubScope subScope in equipment.SubScope)
        //            {
        //                foreach (TECCost cost in subScope.AssociatedCosts)
        //                {
        //                    if (!actualTemplates.Catalogs.AssociatedCosts.Contains(cost))
        //                    { Assert.Fail("Associated costs in system templates not linked"); }
        //                }
        //                foreach (TECDevice device in subScope.Devices)
        //                {
        //                    foreach (TECCost cost in device.AssociatedCosts)
        //                    {
        //                        if (!actualTemplates.Catalogs.AssociatedCosts.Contains(cost))
        //                        { Assert.Fail("Associated costs in system templates not linked"); }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    foreach (TECEquipment equipment in actualTemplates.EquipmentTemplates)
        //    {
        //        foreach (TECCost cost in equipment.AssociatedCosts)
        //        {
        //            if (!actualTemplates.Catalogs.AssociatedCosts.Contains(cost))
        //            { Assert.Fail("Associated costs in equipment templates not linked"); }
        //        }
        //        foreach (TECSubScope subScope in equipment.SubScope)
        //        {
        //            foreach (TECCost cost in subScope.AssociatedCosts)
        //            {
        //                if (!actualTemplates.Catalogs.AssociatedCosts.Contains(cost))
        //                { Assert.Fail("Associated costs in equipment templates not linked"); }
        //            }
        //            foreach (TECDevice device in subScope.Devices)
        //            {
        //                foreach (TECCost cost in device.AssociatedCosts)
        //                {
        //                    if (!actualTemplates.Catalogs.AssociatedCosts.Contains(cost))
        //                    { Assert.Fail("Associated costs in equipment templates not linked"); }
        //                }
        //            }
        //        }
        //    }
        //    foreach (TECSubScope subScope in actualTemplates.SubScopeTemplates)
        //    {
        //        foreach (TECCost cost in subScope.AssociatedCosts)
        //        {
        //            if (!actualTemplates.Catalogs.AssociatedCosts.Contains(cost))
        //            { Assert.Fail("Associated costs in subscope templates not linked"); }
        //        }
        //        foreach (TECDevice device in subScope.Devices)
        //        {
        //            foreach (TECCost cost in device.AssociatedCosts)
        //            {
        //                if (!actualTemplates.Catalogs.AssociatedCosts.Contains(cost))
        //                { Assert.Fail("Associated costs in subscope templates not linked"); }
        //            }
        //        }
        //    }
        //    foreach (TECDevice device in actualTemplates.Catalogs.Devices)
        //    {
        //        foreach (TECCost cost in device.AssociatedCosts)
        //        {
        //            if (!actualTemplates.Catalogs.AssociatedCosts.Contains(cost))
        //            { Assert.Fail("Associated costs in device catalog not linked"); }
        //        }
        //    }
        //    foreach (TECConduitType conduitType in actualTemplates.Catalogs.ConduitTypes)
        //    {
        //        foreach (TECCost cost in conduitType.AssociatedCosts)
        //        {
        //            if (!actualTemplates.Catalogs.AssociatedCosts.Contains(cost))
        //            { Assert.Fail("Associated costs in conduit type catalog not linked"); }
        //        }
        //    }
        //    foreach (TECConnectionType connectionType in actualTemplates.Catalogs.ConnectionTypes)
        //    {
        //        foreach (TECCost cost in connectionType.AssociatedCosts)
        //        {
        //            if (!actualTemplates.Catalogs.AssociatedCosts.Contains(cost))
        //            { Assert.Fail("Associated costs in connection type catalog not linked"); }
        //        }
        //    }

        //    Assert.IsTrue(true, "All Associated costs Linked");
        //}

        //[TestMethod]
        //public void Load_Templates_Linked_Manufacturers()
        //{
        //    foreach (TECDevice device in actualTemplates.Catalogs.Devices)
        //    {
        //        if (!actualTemplates.Catalogs.Manufacturers.Contains(device.Manufacturer))
        //        {
        //            Assert.Fail("Manufacturers not linked in device catalog");
        //        }
        //    }
        //    foreach (TECController controller in actualTemplates.ControllerTemplates)
        //    {
        //        if (!actualTemplates.Catalogs.Manufacturers.Contains(controller.Manufacturer))
        //        {
        //            Assert.Fail("Manufacturers not linked in controller templates");
        //        }
        //    }
        //    Assert.IsTrue(true, "All Manufacturers linked");
        //}

        //[TestMethod]
        //public void Load_Templates_Linked_Tags()
        //{
        //    foreach (TECSystem system in actualTemplates.SystemTemplates)
        //    {
        //        foreach (TECTag tag in system.Tags)
        //        {
        //            if (!actualTemplates.Catalogs.Tags.Contains(tag))
        //            { Assert.Fail("Tags in system templates not linked"); }
        //        }
        //        foreach (TECEquipment equipment in system.Equipment)
        //        {
        //            foreach (TECTag tag in equipment.Tags)
        //            {
        //                if (!actualTemplates.Catalogs.Tags.Contains(tag))
        //                { Assert.Fail("Tags in system templates not linked"); }
        //            }
        //            foreach (TECSubScope subScope in equipment.SubScope)
        //            {
        //                foreach (TECTag tag in subScope.Tags)
        //                {
        //                    if (!actualTemplates.Catalogs.Tags.Contains(tag))
        //                    { Assert.Fail("Tags in system templates not linked"); }
        //                }
        //                foreach (TECDevice device in subScope.Devices)
        //                {
        //                    foreach (TECTag tag in device.Tags)
        //                    {
        //                        if (!actualTemplates.Catalogs.Tags.Contains(tag))
        //                        { Assert.Fail("Tags in system templates not linked"); }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    foreach (TECEquipment equipment in actualTemplates.EquipmentTemplates)
        //    {
        //        foreach (TECTag tag in equipment.Tags)
        //        {
        //            if (!actualTemplates.Catalogs.Tags.Contains(tag))
        //            { Assert.Fail("Tags in equipment templates not linked"); }
        //        }
        //        foreach (TECSubScope subScope in equipment.SubScope)
        //        {
        //            foreach (TECTag tag in subScope.Tags)
        //            {
        //                if (!actualTemplates.Catalogs.Tags.Contains(tag))
        //                { Assert.Fail("Tags in equipment templates not linked"); }
        //            }
        //            foreach (TECDevice device in subScope.Devices)
        //            {
        //                foreach (TECTag tag in device.Tags)
        //                {
        //                    if (!actualTemplates.Catalogs.Tags.Contains(tag))
        //                    { Assert.Fail("Tags in equipment templates not linked"); }
        //                }
        //            }
        //        }
        //    }
        //    foreach (TECSubScope subScope in actualTemplates.SubScopeTemplates)
        //    {
        //        foreach (TECTag tag in subScope.Tags)
        //        {
        //            if (!actualTemplates.Catalogs.Tags.Contains(tag))
        //            { Assert.Fail("Tags in subscope templates not linked"); }
        //        }
        //        foreach (TECDevice device in subScope.Devices)
        //        {
        //            foreach (TECTag tag in device.Tags)
        //            {
        //                if (!actualTemplates.Catalogs.Tags.Contains(tag))
        //                { Assert.Fail("Tags in subscope templates not linked"); }
        //            }
        //        }
        //    }
        //    foreach (TECDevice device in actualTemplates.Catalogs.Devices)
        //    {
        //        foreach (TECTag tag in device.Tags)
        //        {
        //            if (!actualTemplates.Catalogs.Tags.Contains(tag))
        //            { Assert.Fail("Tags in device catalog not linked"); }
        //        }
        //    }
        //    foreach (TECConduitType conduitType in actualTemplates.Catalogs.ConduitTypes)
        //    {
        //        foreach (TECTag tag in conduitType.Tags)
        //        {
        //            if (!actualTemplates.Catalogs.Tags.Contains(tag))
        //            { Assert.Fail("Tags in conduit type catalog not linked"); }
        //        }
        //    }
        //    foreach (TECConnectionType connectionType in actualTemplates.Catalogs.ConnectionTypes)
        //    {
        //        foreach (TECTag tag in connectionType.Tags)
        //        {
        //            if (!actualTemplates.Catalogs.Tags.Contains(tag))
        //            { Assert.Fail("Tags in connection type catalog not linked"); }
        //        }
        //    }

        //    Assert.IsTrue(true, "All Tags Linked");
        //}

        //[TestMethod]
        //public void Load_Templates_Linked_ConnectionTypes()
        //{
        //    foreach (TECDevice device in actualTemplates.Catalogs.Devices)
        //    {
        //        foreach (TECConnectionType type in device.ConnectionTypes)
        //        {
        //            if (!actualTemplates.Catalogs.ConnectionTypes.Contains(type))
        //            {
        //                Assert.Fail("ConnectionTypes not linked in device catalog");
        //            }
        //        }
        //    }

        //    Assert.IsTrue(true, "All Connection types linked");
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

        private void testForRatedCosts(ElectricalMaterialComponent component)
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
