using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using EstimatingUtilitiesLibrary.Database;
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
    public class SaveNewTemplatesTests
    {
        private const bool DEBUG = true;

        static TECTemplates expectedTemplates;
        static TECSystem expectedSystem;
        static TECEquipment expectedEquipment;
        static TECSubScope expectedSubScope;
        static TECDevice expectedDevice;
        static TECManufacturer expectedManufacturer;
        static TECLabeled expectedTag;
        static TECController expectedController;
        static TECCost expectedAssociatedCost;
        static TECElectricalMaterial expectedConnectionType;
        static TECElectricalMaterial expectedConduitType;

        static string path;

        static TECTemplates actualTemplates;
        static TECSystem actualSystem;
        static TECEquipment actualEquipment;
        static TECSubScope actualSubScope;
        static TECDevice actualDevice;
        static TECManufacturer actualManufacturer;
        static TECLabeled actualTag;
        static TECController actualController;
        static TECCost actualAssociatedCost;
        static TECElectricalMaterial actualConnectionType;
        static TECElectricalMaterial actualConduitType;

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
            expectedTemplates = TestHelper.CreateTestTemplates();
            expectedSystem = expectedTemplates.SystemTemplates[0];
            expectedEquipment = expectedTemplates.EquipmentTemplates[0];
            expectedSubScope = expectedTemplates.SubScopeTemplates[0];
            expectedDevice = expectedTemplates.Catalogs.Devices[0];
            expectedManufacturer = expectedTemplates.Catalogs.Manufacturers[0];
            expectedTag = expectedTemplates.Catalogs.Tags[0];
            expectedController = expectedTemplates.ControllerTemplates[0];
            expectedAssociatedCost = expectedTemplates.Catalogs.AssociatedCosts[0];
            expectedConnectionType = expectedTemplates.Catalogs.ConnectionTypes[0];
            expectedConduitType = expectedTemplates.Catalogs.ConduitTypes[0];

            path = Path.GetTempFileName();

            //Act
            DatabaseManager manager = new DatabaseManager(path);
            manager.New(expectedTemplates);
            actualTemplates = manager.Load() as TECTemplates;

            foreach (TECSystem sys in actualTemplates.SystemTemplates)
            {
                if (sys.Guid == expectedSystem.Guid)
                {
                    actualSystem = sys;
                    break;
                }
            }

            foreach (TECEquipment equip in actualTemplates.EquipmentTemplates)
            {
                if (equip.Guid == expectedEquipment.Guid)
                {
                    actualEquipment = equip;
                    break;
                }
            }

            foreach (TECSubScope ss in actualTemplates.SubScopeTemplates)
            {
                if (ss.Guid == expectedSubScope.Guid)
                {
                    actualSubScope = ss;
                    break;
                }
            }

            foreach (TECDevice dev in actualTemplates.Catalogs.Devices)
            {
                if (dev.Guid == expectedDevice.Guid)
                {
                    actualDevice = dev;
                    break;
                }
            }

            foreach (TECManufacturer man in actualTemplates.Catalogs.Manufacturers)
            {
                if (man.Guid == expectedManufacturer.Guid)
                {
                    actualManufacturer = man;
                    break;
                }
            }

            foreach (TECLabeled tag in actualTemplates.Catalogs.Tags)
            {
                if (tag.Guid == expectedTag.Guid)
                {
                    actualTag = tag;
                    break;
                }
            }

            foreach (TECController controller in actualTemplates.ControllerTemplates)
            {
                if (controller.Guid == expectedController.Guid)
                {
                    actualController = controller;
                    break;
                }
            }

            foreach (TECCost cost in actualTemplates.Catalogs.AssociatedCosts)
            {
                if (cost.Guid == expectedAssociatedCost.Guid)
                {
                    actualAssociatedCost = cost;
                    break;
                }
            }

            foreach (TECElectricalMaterial connectionType in actualTemplates.Catalogs.ConnectionTypes)
            {
                if (connectionType.Guid == expectedConnectionType.Guid)
                {
                    actualConnectionType = connectionType;
                    break;
                }
            }

            foreach (TECElectricalMaterial conduitType in actualTemplates.Catalogs.ConduitTypes)
            {
                if (conduitType.Guid == expectedConduitType.Guid)
                {
                    actualConduitType = conduitType;
                    break;
                }
            }
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            if (DEBUG)
            {
                Console.WriteLine("SaveAs test templates saved to: " + path);
            }
            else
            {
                File.Delete(path);
            }


        }

        [TestMethod]
        public void SaveAs_Templates_LaborConstants()
        {
            TECParameters expectedLabor = expectedTemplates.Parameters[0];
            TECParameters actualLabor = actualTemplates.Parameters[0];

            //Assert
            Assert.AreEqual(expectedLabor.PMCoef, actualLabor.PMCoef);
            Assert.AreEqual(expectedLabor.PMRate, actualLabor.PMRate);

            Assert.AreEqual(expectedLabor.ENGCoef, actualLabor.ENGCoef);
            Assert.AreEqual(expectedLabor.ENGRate, actualLabor.ENGRate);

            Assert.AreEqual(expectedLabor.CommCoef, actualLabor.CommCoef);
            Assert.AreEqual(expectedLabor.CommRate, actualLabor.CommRate);

            Assert.AreEqual(expectedLabor.SoftCoef, actualLabor.SoftCoef);
            Assert.AreEqual(expectedLabor.SoftRate, actualLabor.SoftRate);

            Assert.AreEqual(expectedLabor.GraphCoef, actualLabor.GraphCoef);
            Assert.AreEqual(expectedLabor.GraphRate, actualLabor.GraphRate);
        }

        [TestMethod]
        public void SaveAs_Templates_SubcontractLaborConstants()
        {
            TECParameters expectedLabor = expectedTemplates.Parameters[0];
            TECParameters actualLabor = actualTemplates.Parameters[0];

            //Assert
            Assert.AreEqual(expectedLabor.ElectricalRate, actualLabor.ElectricalRate);
            Assert.AreEqual(expectedLabor.ElectricalSuperRate, actualLabor.ElectricalSuperRate);
            Assert.AreEqual(expectedLabor.ElectricalNonUnionRate, actualLabor.ElectricalNonUnionRate);
            Assert.AreEqual(expectedLabor.ElectricalSuperNonUnionRate, actualLabor.ElectricalSuperNonUnionRate);
        }

        [TestMethod]
        public void SaveAs_Templates_System()
        {
            //Arrange
            TECEquipment expectedSysEquipment = expectedSystem.Equipment[0];
            TECSubScope expectedSysSubScope = expectedSysEquipment.SubScope[0];
            TECDevice expectedChildDevice = expectedSysSubScope.Devices[0] as TECDevice;
            TECPoint expectedSysPoint = expectedSysSubScope.Points[0];
            TECManufacturer expectedChildMan = expectedChildDevice.Manufacturer;

            TECEquipment actualSysEquipment = actualSystem.Equipment[0];
            TECSubScope actualSysSubScope = actualSysEquipment.SubScope[0];
            TECDevice actualChildDevice = actualSysSubScope.Devices[0] as TECDevice;
            TECPoint actualSysPoint = actualSysSubScope.Points[0];
            TECManufacturer actualChildMan = actualChildDevice.Manufacturer;

            //Assert
            Assert.AreEqual(expectedSystem.Name, actualSystem.Name);
            Assert.AreEqual(expectedSystem.Description, actualSystem.Description);
            Assert.AreEqual(expectedSystem.Tags[0].Label, actualSystem.Tags[0].Label);

            Assert.AreEqual(expectedSysEquipment.Name, actualSysEquipment.Name);
            Assert.AreEqual(expectedSysEquipment.Description, actualSysEquipment.Description);
            Assert.AreEqual(expectedSysEquipment.Tags[0].Label, actualSysEquipment.Tags[0].Label);

            Assert.AreEqual(expectedSysSubScope.Name, actualSysSubScope.Name);
            Assert.AreEqual(expectedSysSubScope.Description, actualSysSubScope.Description);
            Assert.AreEqual(expectedSysSubScope.Tags[0].Label, actualSysSubScope.Tags[0].Label);

            Assert.AreEqual(expectedChildDevice.Name, actualChildDevice.Name);
            Assert.AreEqual(expectedChildDevice.Description, actualChildDevice.Description);
            Assert.AreEqual(expectedChildDevice.Cost, actualChildDevice.Cost);
            Assert.AreEqual(expectedChildDevice.ConnectionTypes[0].Guid, actualChildDevice.ConnectionTypes[0].Guid);
            Assert.AreEqual(expectedChildDevice.Tags[0].Label, actualChildDevice.Tags[0].Label);

            Assert.AreEqual(expectedSysPoint.Label, actualSysPoint.Label);
            Assert.AreEqual(expectedSysPoint.Quantity, actualSysPoint.Quantity);
            Assert.AreEqual(expectedSysPoint.Type, actualSysPoint.Type);

            Assert.AreEqual(expectedChildMan.Label, actualChildMan.Label);
            Assert.AreEqual(expectedChildMan.Multiplier, actualChildMan.Multiplier);

            ////Controlled scope tests]
            //TECSystem expectedConScope = expectedSystem;
            //TECSystem actualConScope = actualSystem;
            //Assert.AreEqual(expectedConScope.Name, actualConScope.Name);
            //Assert.AreEqual(expectedConScope.Description, actualConScope.Description);
            //Assert.AreEqual(expectedConScope.Equipment[0].Name, actualConScope.Equipment[0].Name);
            //Assert.AreEqual(expectedConScope.Panels[0].Name, actualConScope.Panels[0].Name);
            //Assert.AreEqual(expectedConScope.Controllers[0].Name, actualConScope.Controllers[0].Name);
            //Assert.AreEqual(42, actualConScope.Controllers[0].ChildrenConnections[0].Length);
        }

        [TestMethod]
        public void SaveAs_Templates_Equipment()
        {
            //Arrange
            TECSubScope actualEquipSubScope = actualEquipment.SubScope[0];
            TECDevice actualChildDevice = actualEquipSubScope.Devices[0] as TECDevice;
            TECPoint actualEquipPoint = actualEquipSubScope.Points[0];
            TECManufacturer actualChildMan = actualChildDevice.Manufacturer;

            TECSubScope expectedEquipSubScope = expectedEquipment.SubScope[0];
            TECDevice expectedChildDevice = expectedEquipSubScope.Devices[0] as TECDevice;
            TECPoint expectedEquipPoint = expectedEquipSubScope.Points[0];
            TECManufacturer expectedChildMan = expectedChildDevice.Manufacturer;

            //Assert
            Assert.AreEqual(expectedEquipment.Name, actualEquipment.Name);
            Assert.AreEqual(expectedEquipment.Description, actualEquipment.Description);
            Assert.AreEqual(expectedEquipment.Tags[0].Label, actualEquipment.Tags[0].Label);

            Assert.AreEqual(expectedEquipSubScope.Name, actualEquipSubScope.Name);
            Assert.AreEqual(expectedEquipSubScope.Description, actualEquipSubScope.Description);
            Assert.AreEqual(expectedEquipSubScope.Tags[0].Label, actualEquipSubScope.Tags[0].Label);

            Assert.AreEqual(expectedChildDevice.Name, actualChildDevice.Name);
            Assert.AreEqual(expectedChildDevice.Description, actualChildDevice.Description);
            Assert.AreEqual(expectedChildDevice.Cost, actualChildDevice.Cost);
            Assert.AreEqual(expectedChildDevice.ConnectionTypes[0].Guid, actualChildDevice.ConnectionTypes[0].Guid);
            Assert.AreEqual(expectedChildDevice.Tags[0].Label, actualChildDevice.Tags[0].Label);

            Assert.AreEqual(expectedEquipPoint.Label, actualEquipPoint.Label);
            Assert.AreEqual(expectedEquipPoint.Quantity, actualEquipPoint.Quantity);
            Assert.AreEqual(expectedEquipPoint.Type, actualEquipPoint.Type);

            Assert.AreEqual(expectedChildMan.Label, actualChildMan.Label);
            Assert.AreEqual(expectedChildMan.Multiplier, actualChildMan.Multiplier);
        }

        [TestMethod]
        public void SaveAs_Templates_SubScope()
        {
            //Arrange
            TECDevice actualChildDevice = actualSubScope.Devices[0] as TECDevice;
            TECPoint actualSSPoint = actualSubScope.Points[0];
            TECManufacturer actualChildMan = actualChildDevice.Manufacturer;

            TECDevice expectedChildDevice = expectedSubScope.Devices[0] as TECDevice;
            TECPoint expectedSSPoint = expectedSubScope.Points[0];
            TECManufacturer expectedChildMan = expectedChildDevice.Manufacturer;

            //Assert
            Assert.AreEqual(expectedSubScope.Name, actualSubScope.Name);
            Assert.AreEqual(expectedSubScope.Description, actualSubScope.Description);
            Assert.AreEqual(expectedSubScope.Tags[0].Label, actualSubScope.Tags[0].Label);
            Assert.AreEqual(expectedSubScope.AssociatedCosts[0].Name, actualSubScope.AssociatedCosts[0].Name);

            Assert.AreEqual(expectedChildDevice.Name, actualChildDevice.Name);
            Assert.AreEqual(expectedChildDevice.Description, actualChildDevice.Description);
            Assert.AreEqual(expectedChildDevice.Cost, actualChildDevice.Cost);
            Assert.AreEqual(expectedChildDevice.ConnectionTypes[0].Guid, actualChildDevice.ConnectionTypes[0].Guid);
            Assert.AreEqual(expectedChildDevice.Tags[0].Label, actualChildDevice.Tags[0].Label);

            Assert.AreEqual(expectedSSPoint.Label, actualSSPoint.Label);
            Assert.AreEqual(expectedSSPoint.Quantity, actualSSPoint.Quantity);
            Assert.AreEqual(expectedSSPoint.Type, actualSSPoint.Type);

            Assert.AreEqual(expectedChildMan.Label, actualChildMan.Label);
            Assert.AreEqual(expectedChildMan.Multiplier, actualChildMan.Multiplier);
        }

        [TestMethod]
        public void SaveAs_Templates_Device()
        {
            //Arrange
            TECManufacturer actualChildMan = actualDevice.Manufacturer;
            TECManufacturer expectedChildMan = expectedDevice.Manufacturer;

            //Assert
            Assert.AreEqual(expectedDevice.Name, actualDevice.Name);
            Assert.AreEqual(expectedDevice.Description, actualDevice.Description);
            Assert.AreEqual(expectedDevice.Cost, actualDevice.Cost);
            Assert.AreEqual(expectedDevice.ConnectionTypes[0].Guid, actualDevice.ConnectionTypes[0].Guid);
            Assert.AreEqual(expectedDevice.Tags[0].Label, actualDevice.Tags[0].Label);
            Assert.AreEqual(expectedDevice.Manufacturer.Guid, actualDevice.Manufacturer.Guid);

            Assert.AreEqual(expectedChildMan.Label, actualChildMan.Label);
            Assert.AreEqual(expectedChildMan.Multiplier, actualChildMan.Multiplier);
        }

        [TestMethod]
        public void SaveAs_Templates_Manufacturer()
        {
            //Assert
            Assert.AreEqual(expectedManufacturer.Label, actualManufacturer.Label);
            Assert.AreEqual(expectedManufacturer.Multiplier, actualManufacturer.Multiplier);
        }

        [TestMethod]
        public void SaveAs_Templates_Tag()
        {
            //Assert
            Assert.AreEqual(expectedTag.Label, actualTag.Label);
        }

        [TestMethod]
        public void SaveAs_Templates_Controller()
        {
            //Assert
            Assert.AreEqual(expectedController.Name, actualController.Name);
            Assert.AreEqual(expectedController.Description, actualController.Description);
            Assert.AreEqual(expectedController.Type.Guid, actualController.Type.Guid);

            foreach (TECIO expectedIO in expectedController.Type.IO)
            {
                bool ioExists = false;
                foreach (TECIO actualIO in actualController.Type.IO)
                {
                    if ((expectedIO.Type == actualIO.Type) && (expectedIO.Quantity == actualIO.Quantity))
                    {
                        ioExists = true;
                        break;
                    }
                }
                Assert.IsTrue(ioExists);
            }
        }

        [TestMethod]
        public void SaveAs_Templates_AssociatedCost()
        {
            //Assert
            Assert.AreEqual(expectedAssociatedCost.Name, actualAssociatedCost.Name);
            Assert.AreEqual(expectedAssociatedCost.Cost, actualAssociatedCost.Cost);
        }

        [TestMethod]
        public void SaveAs_Templates_ConnectionType()
        {
            //Assert
            Assert.AreEqual(expectedConnectionType.Name, actualConnectionType.Name);
            Assert.AreEqual(expectedConnectionType.Cost, actualConnectionType.Cost);
            Assert.AreEqual(expectedConnectionType.Labor, actualConnectionType.Labor);
            Assert.AreEqual(expectedConnectionType.RatedCosts.Count, actualConnectionType.RatedCosts.Count);
        }

        [TestMethod]
        public void SaveAs_Templates_ConduitType()
        {
            //Assert
            Assert.AreEqual(expectedConduitType.Name, actualConduitType.Name);
            Assert.AreEqual(expectedConduitType.Cost, actualConduitType.Cost);
            Assert.AreEqual(expectedConduitType.Labor, actualConduitType.Labor);
            Assert.AreEqual(expectedConnectionType.RatedCosts.Count, actualConnectionType.RatedCosts.Count);
        }

        [TestMethod]
        public void SaveAs_Templates_Panel()
        {
            //Arrange
            TECPanel expectedPanel = expectedTemplates.PanelTemplates[0];
            TECPanel actualPanel = actualTemplates.PanelTemplates[0];

            Assert.AreEqual(expectedPanel.Name, actualPanel.Name);
            Assert.AreEqual(expectedPanel.Type.Guid, actualPanel.Type.Guid);
        }

        [TestMethod]
        public void SaveAs_Templates_MiscCost()
        {
            //Arrange
            TECMisc expectedCost = expectedTemplates.MiscCostTemplates[0];
            TECMisc actualCost = actualTemplates.MiscCostTemplates[0];

            Assert.AreEqual(expectedCost.Name, actualCost.Name);
            Assert.AreEqual(expectedCost.Cost, actualCost.Cost);
            Assert.AreEqual(expectedCost.Quantity, actualCost.Quantity);
        }

        [TestMethod]
        public void SaveAs_Templates_PanelType()
        {
            //Arrange
            TECPanelType expectedPanelType = expectedTemplates.Catalogs.PanelTypes[0];
            TECPanelType actualPanelType = actualTemplates.Catalogs.PanelTypes[0];

            Assert.AreEqual(expectedPanelType.Name, actualPanelType.Name);
            Assert.AreEqual(expectedPanelType.Cost, actualPanelType.Cost);
        }

        [TestMethod]
        public void SaveAs_Templates_IOModule()
        {
            //Arrange
            TECIOModule expectedIOModule = expectedTemplates.Catalogs.IOModules[0];
            TECIOModule actualIOModule = actualTemplates.Catalogs.IOModules[0];

            Assert.AreEqual(expectedIOModule.Name, actualIOModule.Name);
            Assert.AreEqual(expectedIOModule.Cost, actualIOModule.Cost);
        }
    }
}