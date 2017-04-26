using EstimatingLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class LoadTemplatesTests
    {
        static TECTemplates actualTemplates;
        static TECSystem actualSystem;
        static TECEquipment actualEquipment;
        static TECSubScope actualSubScope;
        static TECDevice actualDevice;
        static TECManufacturer actualManufacturer;
        static TECTag actualTag;
        static TECConnectionType actualConnectionType;
        static TECController actualController;
        static TECConduitType actualConduitType;
        static TECAssociatedCost actualAssociatedCost;
        static TECIOModule actualIOModule;

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
            actualTemplates = TestHelper.LoadTestTemplates(TestHelper.StaticTestTemplatesPath);

            actualSystem = actualTemplates.SystemTemplates[0];

            actualEquipment = actualTemplates.EquipmentTemplates[0];

            actualSubScope = actualTemplates.SubScopeTemplates[0];
            
            actualController = actualTemplates.ControllerTemplates[0];

            actualDevice = null;
            foreach (TECDevice dev in actualTemplates.Catalogs.Devices)
            {
                if (dev.Name == "Test Device") actualDevice = dev;
            }

            actualManufacturer = null;
            foreach (TECManufacturer man in actualTemplates.Catalogs.Manufacturers)
            {
                if (man.Name == "Test Manufacturer") actualManufacturer = man;
            }

            actualTag = null;
            foreach (TECTag tag in actualTemplates.Catalogs.Tags)
            {
                if (tag.Text == "Test Tag") actualTag = tag;
            }

            actualConnectionType = null;
            foreach(TECConnectionType connectionType in actualTemplates.Catalogs.ConnectionTypes)
            {
                if (connectionType.Name == "Test ConnectionType") actualConnectionType = connectionType;
            }
            actualConduitType = null;
            foreach (TECConduitType conduitType in actualTemplates.Catalogs.ConduitTypes)
            {
                if (conduitType.Name == "Test ConduitType") actualConduitType = conduitType;
            }
            actualAssociatedCost = null;
            foreach (TECAssociatedCost cost in actualTemplates.Catalogs.AssociatedCosts)
            {
                if (cost.Name == "Test Cost") actualAssociatedCost = cost;
            }
            actualIOModule = null;
            foreach (TECIOModule ioModule in actualTemplates.Catalogs.IOModules)
            {
                if (ioModule.Name == "Test IO Module") actualIOModule = ioModule;
            }
        }

        [TestMethod]
        public void Load_Templates_LaborConsts()
        {
            //Arrange
            TECLabor actualLabor = actualTemplates.Labor;

            //Assert
            Assert.AreEqual(10.0, actualLabor.PMCoef);
            Assert.AreEqual(10.1, actualLabor.PMRate);
            Assert.AreEqual(11.0, actualLabor.ENGCoef);
            Assert.AreEqual(11.2, actualLabor.ENGRate);
            Assert.AreEqual(12.0, actualLabor.CommCoef);
            Assert.AreEqual(12.3, actualLabor.CommRate);
            Assert.AreEqual(13.0, actualLabor.SoftCoef);
            Assert.AreEqual(13.4, actualLabor.SoftRate);
            Assert.AreEqual(14.0, actualLabor.GraphCoef);
            Assert.AreEqual(14.5, actualLabor.GraphRate);
        }

        [TestMethod]
        public void Load_Templates_SubconstractorConsts()
        {
            //Arrange
            TECLabor actualLabor = actualTemplates.Labor;

            //Assert
            Assert.AreEqual(954.9, actualLabor.ElectricalRate);
            Assert.AreEqual(614.15, actualLabor.ElectricalSuperRate);
            Assert.AreEqual(6870.1, actualLabor.ElectricalNonUnionRate);
            Assert.AreEqual(46.12, actualLabor.ElectricalSuperNonUnionRate);
        }

        [TestMethod]
        public void Load_Templates_System()
        {
            //Arrange
            TECEquipment sysEquipment = actualSystem.Equipment[0];
            TECSubScope sysSubScope = sysEquipment.SubScope[0];
            TECDevice childDevice = sysSubScope.Devices[0];
            TECPoint sysPoint = sysSubScope.Points[0];
            TECManufacturer childMan = childDevice.Manufacturer;

            //Assert
            Assert.AreEqual("Test System", actualSystem.Name);
            Assert.AreEqual("System Description", actualSystem.Description);
            Assert.AreEqual(12.3, actualSystem.BudgetPriceModifier);
            Assert.AreEqual("System Tag", actualSystem.Tags[0].Text);

            Assert.AreEqual("System Equipment", sysEquipment.Name);
            Assert.AreEqual("Child Equipment", sysEquipment.Description);
            Assert.AreEqual(654, sysEquipment.Quantity);
            Assert.AreEqual(65.4, sysEquipment.BudgetUnitPrice);
            Assert.AreEqual("Equipment Tag", sysEquipment.Tags[0].Text);

            Assert.AreEqual("System SubScope", sysSubScope.Name);
            Assert.AreEqual("Child SubScope", sysSubScope.Description);
            Assert.AreEqual(486, sysSubScope.Quantity);
            Assert.AreEqual("SubScope Tag", sysSubScope.Tags[0].Text);

            Assert.AreEqual("Child Device", childDevice.Name);
            Assert.AreEqual("Child Device", childDevice.Description);
            Assert.AreEqual(89.3, childDevice.Cost);
            Assert.AreEqual("TwoC18", childDevice.ConnectionType.Name);
            Assert.AreEqual("Device Tag", childDevice.Tags[0].Text);

            Assert.AreEqual("System Point", sysPoint.Name);
            Assert.AreEqual("Child Point", sysPoint.Description);
            Assert.AreEqual(34, sysPoint.Quantity);
            Assert.AreEqual(PointTypes.Serial, sysPoint.Type);

            Assert.AreEqual("Child Manufacturer (Child Device)", childMan.Name);
            Assert.AreEqual(0.3, childMan.Multiplier);
        }

        [TestMethod]
        public void Load_Templates_Equipment()
        {
            //Arrange
            TECSubScope equipSubScope = actualEquipment.SubScope[0];
            TECDevice childDevice = equipSubScope.Devices[0];
            TECPoint equipPoint = equipSubScope.Points[0];
            TECManufacturer childMan = childDevice.Manufacturer;

            //Assert
            Assert.AreEqual("Test Equipment", actualEquipment.Name);
            Assert.AreEqual("Equipment Description", actualEquipment.Description);
            Assert.AreEqual(64.1, actualEquipment.BudgetUnitPrice);
            Assert.AreEqual("Equipment Tag", actualEquipment.Tags[0].Text);

            Assert.AreEqual("Equipment SubScope", equipSubScope.Name);
            Assert.AreEqual("Child SubScope", equipSubScope.Description);
            Assert.AreEqual(346, equipSubScope.Quantity);
            Assert.AreEqual("SubScope Tag", equipSubScope.Tags[0].Text);

            Assert.AreEqual("Child Device", childDevice.Name);
            Assert.AreEqual("Child Device", childDevice.Description);
            Assert.AreEqual(89.3, childDevice.Cost);
            Assert.AreEqual("TwoC18", childDevice.ConnectionType.Name);
            Assert.AreEqual("Device Tag", childDevice.Tags[0].Text);

            Assert.AreEqual("Equipment Point", equipPoint.Name);
            Assert.AreEqual("Child Point", equipPoint.Description);
            Assert.AreEqual(81, equipPoint.Quantity);
            Assert.AreEqual(PointTypes.AI, equipPoint.Type);

            Assert.AreEqual("Child Manufacturer (Child Device)", childMan.Name);
            Assert.AreEqual(0.3, childMan.Multiplier);
        }

        [TestMethod]
        public void Load_Templates_SubScope()
        {
            //Arrange
            TECDevice childDevice = actualSubScope.Devices[0];
            TECPoint ssPoint = actualSubScope.Points[0];
            TECManufacturer childMan = childDevice.Manufacturer;

            //Assert
            Assert.AreEqual("Test SubScope", actualSubScope.Name);
            Assert.AreEqual("SubScope Description", actualSubScope.Description);
            Assert.AreEqual("SubScope Tag", actualSubScope.Tags[0].Text);
            Assert.AreEqual("Test SubScope", actualSubScope.Name);
            Assert.AreEqual("Test Cost", actualSubScope.AssociatedCosts[0].Name);

            Assert.AreEqual("Child Device", childDevice.Name);
            Assert.AreEqual("Child Device", childDevice.Description);
            Assert.AreEqual(89.3, childDevice.Cost);
            Assert.AreEqual("TwoC18", childDevice.ConnectionType.Name);
            Assert.AreEqual("Device Tag", childDevice.Tags[0].Text);

            Assert.AreEqual("SubScope Point", ssPoint.Name);
            Assert.AreEqual("Child Point", ssPoint.Description);
            Assert.AreEqual(349, ssPoint.Quantity);
            Assert.AreEqual(PointTypes.BO, ssPoint.Type);

            Assert.AreEqual("Child Manufacturer (Child Device)", childMan.Name);
            Assert.AreEqual(0.3, childMan.Multiplier);
        }

        [TestMethod]
        public void Load_Templates_Device()
        {
            //Arrange
            TECManufacturer childMan = actualDevice.Manufacturer;

            //Assert
            Assert.AreEqual("Test Device", actualDevice.Name);
            Assert.AreEqual("Device Description", actualDevice.Description);
            Assert.AreEqual(72.9, actualDevice.Cost);
            Assert.AreEqual("Cat6", actualDevice.ConnectionType.Name);
            Assert.AreEqual("Device Tag", actualDevice.Tags[0].Text);

            Assert.AreEqual("Child Manufacturer (Test Device)", childMan.Name);
            Assert.AreEqual(0.123, childMan.Multiplier);
        }

        [TestMethod]
        public void Load_Templates_Manufacturer()
        {
            //Assert
            Assert.AreEqual("Test Manufacturer", actualManufacturer.Name);
            Assert.AreEqual(0.65, actualManufacturer.Multiplier);
        }

        [TestMethod]
        public void Load_Templates_Tag()
        {
            //Assert
            Assert.AreEqual("Test Tag", actualTag.Text);
        }

        [TestMethod]
        public void Load_Templates_Controller()
        {
            //Assert
            Assert.AreEqual("Test Controller", actualController.Name);
            Assert.AreEqual("test description", actualController.Description);
            Assert.AreEqual(101, actualController.Cost);
            Assert.AreEqual(2, actualController.IO.Count);
            Assert.AreEqual(IOType.AI, actualController.IO[0].Type);
            Assert.AreEqual("Test Manufacturer", actualController.Manufacturer.Name);
        }

        [TestMethod]
        public void Load_Templates_ConnectionType()
        {
            Assert.AreEqual("Test ConnectionType", actualConnectionType.Name);
            Assert.AreEqual(10, actualConnectionType.Cost);
            Assert.AreEqual(12, actualConnectionType.Labor);
            Assert.AreEqual("Test Cost", actualConnectionType.AssociatedCosts[0].Name);
            Assert.AreEqual(2, actualConnectionType.AssociatedCosts.Count);
        }

        [TestMethod]
        public void Load_Templates_ConduitType()
        {
            Assert.AreEqual("Test ConduitType", actualConduitType.Name);
            Assert.AreEqual(12, actualConduitType.Cost);
            Assert.AreEqual(13, actualConduitType.Labor);
            Assert.AreEqual("Test Cost", actualConduitType.AssociatedCosts[0].Name);

        }

        [TestMethod]
        public void Load_Templates_AssociatedCost()
        {
            Assert.AreEqual("Test Cost", actualAssociatedCost.Name);
            Assert.AreEqual(42, actualAssociatedCost.Cost);
        }

        [TestMethod]
        public void Load_Templates_MiscCost()
        {
            //Arrange
            TECMiscCost actualCost = actualTemplates.MiscCostTemplates[0];

            //Assert
            Assert.AreEqual("Test Misc Cost", actualCost.Name);
            Assert.AreEqual(654.9648, actualCost.Cost);
            Assert.AreEqual(19, actualCost.Quantity);
        }


        [TestMethod]
        public void Load_Templates_MiscWiring()
        {
            //Arrange
            TECMiscWiring actualCost = actualTemplates.MiscWiringTemplates[0];

            //Assert
            Assert.AreEqual("Test Misc Wiring", actualCost.Name);
            Assert.AreEqual(654.9648, actualCost.Cost);
            Assert.AreEqual(19, actualCost.Quantity);
        }


        [TestMethod]
        public void Load_Templates_IOModules()
        {
            //Arrange
            TECIOModule actualIOModule = actualTemplates.Catalogs.IOModules[0];

            //Assert
            Assert.AreEqual("Test IO Module", actualIOModule.Name);
            Assert.AreEqual(42, actualIOModule.Cost);
            Assert.AreEqual(2, actualIOModule.IOPerModule);
        }

        [TestMethod]
        public void Load_Templates_PanelType()
        {
            //Arrange
            TECPanelType actualCost = actualTemplates.Catalogs.PanelTypes[0];

            //Assert
            Assert.AreEqual("Test Panel Type", actualCost.Name);
            Assert.AreEqual(654.9648, actualCost.Cost);
        }

        [TestMethod]
        public void Load_Templates_Panel()
        {
            //Arrange
            TECPanel actualPanel = actualTemplates.PanelTemplates[0];
            TECPanelType actualPanelType = actualPanel.Type;

            //Assert
            foreach(TECPanel panel in actualTemplates.PanelTemplates)
            {
                if (panel.Name == "Controlled Panel")
                {
                    Assert.Fail();
                }
            }

            Assert.AreEqual("Test Panel", actualPanel.Name);
            Assert.AreEqual("Test Panel Type", actualPanelType.Name);
        }

        [TestMethod]
        public void Load_Templates_ControlledScope()
        {
            //Arrange
            TECControlledScope actualConScope = actualTemplates.ControlledScopeTemplates[0];

            //Assert
            Assert.AreEqual("Test Controlled Scope", actualConScope.Name);
            Assert.AreEqual("Test Controlled Description", actualConScope.Description);
            Assert.AreEqual(420, actualConScope.Controllers[0].ChildrenConnections[0].Length);
            Assert.AreEqual("Controlled System", actualConScope.Systems[0].Name);
            Assert.AreEqual("Controlled Controller", actualConScope.Controllers[0].Name);
            Assert.AreEqual("Controlled Panel", actualConScope.Panels[0].Name);
        }

        [TestMethod]
        public void Load_Templates_ControlledScope_Linking()
        {
            //Arrange
            TECControlledScope actualConScope = actualTemplates.ControlledScopeTemplates[0];
            var connectionsInSystemsLinked = true;
            var connectionsInControllersLinked = true;
            var controllersInPanelsLinked = true;

            foreach(TECSystem system in actualConScope.Systems)
            {
                foreach(TECEquipment equipment in system.Equipment)
                {
                    foreach(TECSubScope subScope in equipment.SubScope)
                    {
                        if (!actualConScope.Controllers[0].ChildrenConnections.Contains(subScope.Connection))
                        {
                            connectionsInSystemsLinked = false;
                        }
                    }
                }
            }
            foreach(TECPanel panel in actualConScope.Panels)
            {
                foreach(TECController controller in panel.Controllers)
                {
                    if (!actualConScope.Controllers.Contains(controller))
                    {
                        controllersInPanelsLinked = false;
                    }
                }
            }

            Assert.IsTrue(connectionsInSystemsLinked);
            Assert.IsTrue(connectionsInControllersLinked);
            Assert.IsTrue(controllersInPanelsLinked);
        }

        [TestMethod]
        public void Load_Templates_Linked_Devices()
        {
            foreach(TECSystem system in actualTemplates.SystemTemplates)
            {
                foreach(TECEquipment equipment in system.Equipment)
                {
                    foreach(TECSubScope subScope in equipment.SubScope)
                    {
                        foreach(TECDevice device in subScope.Devices)
                        {
                            if (!actualTemplates.Catalogs.Devices.Contains(device))
                            {
                                Assert.Fail("Devices in system templates not linked");
                            }
                        }
                    }
                }
            }
            foreach (TECEquipment equipment in actualTemplates.EquipmentTemplates)
            {
                foreach (TECSubScope subScope in equipment.SubScope)
                {
                    foreach (TECDevice device in subScope.Devices)
                    {
                        if (!actualTemplates.Catalogs.Devices.Contains(device))
                        {
                            Assert.Fail("Devices in equipment templates not linked");
                        }
                    }
                }
            }
            foreach (TECSubScope subScope in actualTemplates.SubScopeTemplates)
            {
                foreach (TECDevice device in subScope.Devices)
                {
                    if (!actualTemplates.Catalogs.Devices.Contains(device))
                    {
                        Assert.Fail("Devices in subscope templates not linked");
                    }
                }
            }
            Assert.IsTrue(true, "All Devices Linked");
        }

        [TestMethod]
        public void Load_Templates_Linked_AssociatedCosts()
        {
            foreach (TECSystem system in actualTemplates.SystemTemplates)
            {
                foreach (TECAssociatedCost cost in system.AssociatedCosts)
                {
                    if (!actualTemplates.Catalogs.AssociatedCosts.Contains(cost))
                    { Assert.Fail("Associated costs in system templates not linked"); }
                }
                foreach (TECEquipment equipment in system.Equipment)
                {
                    foreach (TECAssociatedCost cost in equipment.AssociatedCosts)
                    {
                        if (!actualTemplates.Catalogs.AssociatedCosts.Contains(cost))
                        { Assert.Fail("Associated costs in system templates not linked"); }
                    }
                    foreach (TECSubScope subScope in equipment.SubScope)
                    {
                        foreach (TECAssociatedCost cost in subScope.AssociatedCosts)
                        {
                            if (!actualTemplates.Catalogs.AssociatedCosts.Contains(cost))
                            { Assert.Fail("Associated costs in system templates not linked"); }
                        }
                        foreach (TECDevice device in subScope.Devices)
                        {
                            foreach (TECAssociatedCost cost in device.AssociatedCosts)
                            {
                                if (!actualTemplates.Catalogs.AssociatedCosts.Contains(cost))
                                { Assert.Fail("Associated costs in system templates not linked"); }
                            }
                        }
                    }
                }
            }
            foreach (TECEquipment equipment in actualTemplates.EquipmentTemplates)
            {
                foreach (TECAssociatedCost cost in equipment.AssociatedCosts)
                {
                    if (!actualTemplates.Catalogs.AssociatedCosts.Contains(cost))
                    { Assert.Fail("Associated costs in equipment templates not linked"); }
                }
                foreach (TECSubScope subScope in equipment.SubScope)
                {
                    foreach (TECAssociatedCost cost in subScope.AssociatedCosts)
                    {
                        if (!actualTemplates.Catalogs.AssociatedCosts.Contains(cost))
                        { Assert.Fail("Associated costs in equipment templates not linked"); }
                    }
                    foreach (TECDevice device in subScope.Devices)
                    {
                        foreach (TECAssociatedCost cost in device.AssociatedCosts)
                        {
                            if (!actualTemplates.Catalogs.AssociatedCosts.Contains(cost))
                            { Assert.Fail("Associated costs in equipment templates not linked"); }
                        }
                    }
                }
            }
            foreach (TECSubScope subScope in actualTemplates.SubScopeTemplates)
            {
                foreach (TECAssociatedCost cost in subScope.AssociatedCosts)
                {
                    if (!actualTemplates.Catalogs.AssociatedCosts.Contains(cost))
                    { Assert.Fail("Associated costs in subscope templates not linked"); }
                }
                foreach (TECDevice device in subScope.Devices)
                {
                    foreach (TECAssociatedCost cost in device.AssociatedCosts)
                    {
                        if (!actualTemplates.Catalogs.AssociatedCosts.Contains(cost))
                        { Assert.Fail("Associated costs in subscope templates not linked"); }
                    }
                }
            }
            foreach(TECDevice device in actualTemplates.Catalogs.Devices)
            {
                foreach (TECAssociatedCost cost in device.AssociatedCosts)
                {
                    if (!actualTemplates.Catalogs.AssociatedCosts.Contains(cost))
                    { Assert.Fail("Associated costs in device catalog not linked"); }
                }
            }
            foreach(TECConduitType conduitType in actualTemplates.Catalogs.ConduitTypes)
            {
                foreach (TECAssociatedCost cost in conduitType.AssociatedCosts)
                {
                    if (!actualTemplates.Catalogs.AssociatedCosts.Contains(cost))
                    { Assert.Fail("Associated costs in conduit type catalog not linked"); }
                }
            }
            foreach (TECConnectionType connectionType in actualTemplates.Catalogs.ConnectionTypes)
            {
                foreach (TECAssociatedCost cost in connectionType.AssociatedCosts)
                {
                    if (!actualTemplates.Catalogs.AssociatedCosts.Contains(cost))
                    { Assert.Fail("Associated costs in connection type catalog not linked"); }
                }
            }
           
            Assert.IsTrue(true, "All Associated costs Linked");
        }

        [TestMethod]
        public void Load_Templates_Linked_Manufacturers()
        {
            foreach(TECDevice device in actualTemplates.Catalogs.Devices)
            {
                if (!actualTemplates.Catalogs.Manufacturers.Contains(device.Manufacturer))
                {
                    Assert.Fail("Manufacturers not linked in device catalog");
                }
            }
            foreach(TECController controller in actualTemplates.ControllerTemplates)
            {
                if (!actualTemplates.Catalogs.Manufacturers.Contains(controller.Manufacturer))
                {
                    Assert.Fail("Manufacturers not linked in controller templates");
                }
            }
            Assert.IsTrue(true, "All Manufacturers linked");
        }

        [TestMethod]
        public void Load_Templates_Linked_Tags()
        {
            foreach (TECSystem system in actualTemplates.SystemTemplates)
            {
                foreach (TECTag tag in system.Tags)
                {
                    if (!actualTemplates.Catalogs.Tags.Contains(tag))
                    { Assert.Fail("Tags in system templates not linked"); }
                }
                foreach (TECEquipment equipment in system.Equipment)
                {
                    foreach (TECTag tag in equipment.Tags)
                    {
                        if (!actualTemplates.Catalogs.Tags.Contains(tag))
                        { Assert.Fail("Tags in system templates not linked"); }
                    }
                    foreach (TECSubScope subScope in equipment.SubScope)
                    {
                        foreach (TECTag tag in subScope.Tags)
                        {
                            if (!actualTemplates.Catalogs.Tags.Contains(tag))
                            { Assert.Fail("Tags in system templates not linked"); }
                        }
                        foreach (TECDevice device in subScope.Devices)
                        {
                            foreach (TECTag tag in device.Tags)
                            {
                                if (!actualTemplates.Catalogs.Tags.Contains(tag))
                                { Assert.Fail("Tags in system templates not linked"); }
                            }
                        }
                    }
                }
            }
            foreach (TECEquipment equipment in actualTemplates.EquipmentTemplates)
            {
                foreach (TECTag tag in equipment.Tags)
                {
                    if (!actualTemplates.Catalogs.Tags.Contains(tag))
                    { Assert.Fail("Tags in equipment templates not linked"); }
                }
                foreach (TECSubScope subScope in equipment.SubScope)
                {
                    foreach (TECTag tag in subScope.Tags)
                    {
                        if (!actualTemplates.Catalogs.Tags.Contains(tag))
                        { Assert.Fail("Tags in equipment templates not linked"); }
                    }
                    foreach (TECDevice device in subScope.Devices)
                    {
                        foreach (TECTag tag in device.Tags)
                        {
                            if (!actualTemplates.Catalogs.Tags.Contains(tag))
                            { Assert.Fail("Tags in equipment templates not linked"); }
                        }
                    }
                }
            }
            foreach (TECSubScope subScope in actualTemplates.SubScopeTemplates)
            {
                foreach (TECTag tag in subScope.Tags)
                {
                    if (!actualTemplates.Catalogs.Tags.Contains(tag))
                    { Assert.Fail("Tags in subscope templates not linked"); }
                }
                foreach (TECDevice device in subScope.Devices)
                {
                    foreach (TECTag tag in device.Tags)
                    {
                        if (!actualTemplates.Catalogs.Tags.Contains(tag))
                        { Assert.Fail("Tags in subscope templates not linked"); }
                    }
                }
            }
            foreach (TECDevice device in actualTemplates.Catalogs.Devices)
            {
                foreach (TECTag tag in device.Tags)
                {
                    if (!actualTemplates.Catalogs.Tags.Contains(tag))
                    { Assert.Fail("Tags in device catalog not linked"); }
                }
            }
            foreach (TECConduitType conduitType in actualTemplates.Catalogs.ConduitTypes)
            {
                foreach (TECTag tag in conduitType.Tags)
                {
                    if (!actualTemplates.Catalogs.Tags.Contains(tag))
                    { Assert.Fail("Tags in conduit type catalog not linked"); }
                }
            }
            foreach (TECConnectionType connectionType in actualTemplates.Catalogs.ConnectionTypes)
            {
                foreach (TECTag tag in connectionType.Tags)
                {
                    if (!actualTemplates.Catalogs.Tags.Contains(tag))
                    { Assert.Fail("Tags in connection type catalog not linked"); }
                }
            }

            Assert.IsTrue(true, "All Tags Linked");
        }

        [TestMethod]
        public void Load_Templates_Linked_ConnectionTypes()
        {
            foreach (TECDevice device in actualTemplates.Catalogs.Devices)
            {
                if (!actualTemplates.Catalogs.ConnectionTypes.Contains(device.ConnectionType))
                {
                    Assert.Fail("ConnectionTypes not linked in device catalog");
                }
            }

            Assert.IsTrue(true, "All Connection types linked");
        }

        
    }
}
