using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using EstimatingUtilitiesLibrary.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using static Tests.CostTestingUtilities;

namespace Tests
{
    [TestClass]
    public class SaveNewTemplatesTests
    {
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
            expectedSystem = expectedTemplates.SystemTemplates.First(sys => sys.Name == "Test System");
            expectedEquipment = expectedTemplates.EquipmentTemplates.First(sys => sys.Name == "Test Equipment");
            expectedSubScope = expectedTemplates.SubScopeTemplates.First(sys => sys.Name == "Test SubScope");
            expectedDevice = expectedTemplates.Catalogs.Devices.First(item => item.Name == "Test Device");
            expectedManufacturer = expectedTemplates.Catalogs.Manufacturers.First(item => item.Label == "Test Manufacturer");
            expectedTag = expectedTemplates.Catalogs.Tags[0];
            expectedController = expectedTemplates.ControllerTemplates.First(sys => sys.Name == "Test Controller");
            expectedAssociatedCost = expectedTemplates.Catalogs.AssociatedCosts[0];
            expectedConnectionType = expectedTemplates.Catalogs.ConnectionTypes[0];
            expectedConduitType = expectedTemplates.Catalogs.ConduitTypes[0];

            path = Path.GetTempFileName();

            //Act
            DatabaseManager<TECTemplates> manager = new DatabaseManager<TECTemplates>(path);
            manager.New(expectedTemplates);
            actualTemplates = manager.Load();

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

            foreach (TECTag tag in actualTemplates.Catalogs.Tags)
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
            
            File.Delete(path);
        }

        [TestMethod]
        public void SaveNew_Templates_LaborConstants()
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
        public void SaveNew_Templates_SubcontractLaborConstants()
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
        public void SaveNew_Templates_System()
        {
            //Arrange
            TECEquipment expectedSysEquipment = expectedSystem.Equipment.First(item => item.Name == "System Equipment");
            TECSubScope expectedSysSubScope = expectedSysEquipment.SubScope.First(item => item.Name == "System SubScope");
            TECDevice expectedChildDevice = expectedSysSubScope.Devices[0] as TECDevice;
            TECPoint expectedSysPoint = expectedSysSubScope.Points.First(item => item.Label == "System Point");
            TECManufacturer expectedChildMan = expectedChildDevice.Manufacturer;

            TECEquipment actualSysEquipment = actualSystem.Equipment.First(item => item.Name == "System Equipment");
            TECSubScope actualSysSubScope = actualSysEquipment.SubScope.First(item => item.Name == "System SubScope");
            TECDevice actualChildDevice = actualSysSubScope.Devices[0] as TECDevice;
            TECPoint actualSysPoint = actualSysSubScope.Points.First(item => item.Label == "System Point");
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
        public void SaveNew_Templates_Equipment()
        {
            //Arrange
            TECSubScope actualEquipSubScope = actualEquipment.SubScope.First(item => item.Name == "Equipment SubScope");
            TECDevice actualChildDevice = actualEquipSubScope.Devices[0] as TECDevice;
            TECPoint actualEquipPoint = actualEquipSubScope.Points[0];
            TECManufacturer actualChildMan = actualChildDevice.Manufacturer;

            TECSubScope expectedEquipSubScope = expectedEquipment.SubScope.First(item => item.Name == "Equipment SubScope");
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
            Assert.AreEqual(expectedChildDevice.Cost, actualChildDevice.Cost, DELTA);
            Assert.AreEqual(expectedChildDevice.ConnectionTypes[0].Guid, actualChildDevice.ConnectionTypes[0].Guid);
            Assert.AreEqual(expectedChildDevice.Tags[0].Label, actualChildDevice.Tags[0].Label);

            Assert.AreEqual(expectedEquipPoint.Label, actualEquipPoint.Label);
            Assert.AreEqual(expectedEquipPoint.Quantity, actualEquipPoint.Quantity);
            Assert.AreEqual(expectedEquipPoint.Type, actualEquipPoint.Type);

            Assert.AreEqual(expectedChildMan.Label, actualChildMan.Label);
            Assert.AreEqual(expectedChildMan.Multiplier, actualChildMan.Multiplier);
        }

        [TestMethod]
        public void SaveNew_Templates_SubScope()
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
            Assert.AreEqual(expectedChildDevice.Cost, actualChildDevice.Cost, DELTA);
            Assert.AreEqual(expectedChildDevice.ConnectionTypes[0].Guid, actualChildDevice.ConnectionTypes[0].Guid);
            Assert.AreEqual(expectedChildDevice.Tags[0].Label, actualChildDevice.Tags[0].Label);

            Assert.AreEqual(expectedSSPoint.Label, actualSSPoint.Label);
            Assert.AreEqual(expectedSSPoint.Quantity, actualSSPoint.Quantity);
            Assert.AreEqual(expectedSSPoint.Type, actualSSPoint.Type);

            Assert.AreEqual(expectedChildMan.Label, actualChildMan.Label);
            Assert.AreEqual(expectedChildMan.Multiplier, actualChildMan.Multiplier);
        }

        [TestMethod]
        public void SaveNew_Templates_Device()
        {
            //Arrange
            TECManufacturer actualChildMan = actualDevice.Manufacturer;
            TECManufacturer expectedChildMan = expectedDevice.Manufacturer;

            //Assert
            Assert.AreEqual(expectedDevice.Name, actualDevice.Name);
            Assert.AreEqual(expectedDevice.Description, actualDevice.Description);
            Assert.AreEqual(expectedDevice.Cost, actualDevice.Cost, DELTA);
            Assert.AreEqual(expectedDevice.ConnectionTypes[0].Guid, actualDevice.ConnectionTypes[0].Guid);
            Assert.AreEqual(expectedDevice.Tags[0].Label, actualDevice.Tags[0].Label);
            Assert.AreEqual(expectedDevice.Manufacturer.Guid, actualDevice.Manufacturer.Guid);

            Assert.AreEqual(expectedChildMan.Label, actualChildMan.Label);
            Assert.AreEqual(expectedChildMan.Multiplier, actualChildMan.Multiplier, DELTA);
        }

        [TestMethod]
        public void SaveNew_Templates_Manufacturer()
        {
            //Assert
            Assert.AreEqual(expectedManufacturer.Label, actualManufacturer.Label);
            Assert.AreEqual(expectedManufacturer.Multiplier, actualManufacturer.Multiplier, DELTA);
        }

        [TestMethod]
        public void SaveNew_Templates_Tag()
        {
            //Assert
            Assert.AreEqual(expectedTag.Label, actualTag.Label);
        }

        [TestMethod]
        public void SaveNew_Templates_Controller()
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
        public void SaveNew_Templates_AssociatedCost()
        {
            //Assert
            Assert.AreEqual(expectedAssociatedCost.Name, actualAssociatedCost.Name);
            Assert.AreEqual(expectedAssociatedCost.Cost, actualAssociatedCost.Cost, DELTA);
        }

        [TestMethod]
        public void SaveNew_Templates_ConnectionType()
        {
            //Assert
            Assert.AreEqual(expectedConnectionType.Name, actualConnectionType.Name);
            Assert.AreEqual(expectedConnectionType.Cost, actualConnectionType.Cost, DELTA);
            Assert.AreEqual(expectedConnectionType.Labor, actualConnectionType.Labor);
            Assert.AreEqual(expectedConnectionType.RatedCosts.Count, actualConnectionType.RatedCosts.Count);
        }

        [TestMethod]
        public void SaveNew_Templates_ConduitType()
        {
            //Assert
            Assert.AreEqual(expectedConduitType.Name, actualConduitType.Name);
            Assert.AreEqual(expectedConduitType.Cost, actualConduitType.Cost, DELTA);
            Assert.AreEqual(expectedConduitType.Labor, actualConduitType.Labor);
            Assert.AreEqual(expectedConnectionType.RatedCosts.Count, actualConnectionType.RatedCosts.Count);
        }

        [TestMethod]
        public void SaveNew_Templates_Panel()
        {
            //Arrange
            TECPanel expectedPanel = expectedTemplates.PanelTemplates.First(item => item.Name == "Test Panel");
            TECPanel actualPanel = actualTemplates.PanelTemplates.First(item => item.Name == "Test Panel");

            Assert.AreEqual(expectedPanel.Name, actualPanel.Name);
            Assert.AreEqual(expectedPanel.Type.Guid, actualPanel.Type.Guid);
        }

        [TestMethod]
        public void SaveNew_Templates_Misc()
        {
            //Arrange
            TECMisc expectedTECCost = null, expectedElecCost = null;
            foreach(TECMisc misc in expectedTemplates.MiscCostTemplates)
            {
                if (misc.Type == CostType.TEC)
                {
                    expectedTECCost = misc;
                }
                else if (misc.Type == CostType.Electrical)
                {
                    expectedElecCost = misc;
                }
            }

            TECMisc actualTECCost = null, actualElecCost = null;
            foreach(TECMisc misc in actualTemplates.MiscCostTemplates)
            {
                if (misc.Guid == expectedTECCost.Guid)
                {
                    actualTECCost = misc;
                }
                else if (misc.Guid == expectedElecCost.Guid)
                {
                    actualElecCost = misc;
                }
            }

            //Assert
            Assert.AreEqual(expectedTECCost.Name, actualTECCost.Name);
            Assert.AreEqual(expectedTECCost.Cost, actualTECCost.Cost, DELTA);
            Assert.AreEqual(expectedTECCost.Quantity, actualTECCost.Quantity);

            Assert.AreEqual(expectedElecCost.Name, actualElecCost.Name);
            Assert.AreEqual(expectedElecCost.Cost, actualElecCost.Cost, DELTA);
            Assert.AreEqual(expectedElecCost.Quantity, actualElecCost.Quantity);
        }

        [TestMethod]
        public void SaveNew_Templates_PanelType()
        {
            //Arrange
            TECPanelType expectedPanelType = expectedTemplates.Catalogs.PanelTypes[0];
            TECPanelType actualPanelType = actualTemplates.Catalogs.PanelTypes[0];

            Assert.AreEqual(expectedPanelType.Name, actualPanelType.Name);
            Assert.AreEqual(expectedPanelType.Cost, actualPanelType.Cost, DELTA);
        }

        [TestMethod]
        public void SaveNew_Templates_IOModule()
        {
            //Arrange
            TECIOModule expectedIOModule = expectedTemplates.Catalogs.IOModules[0];
            TECIOModule actualIOModule = actualTemplates.Catalogs.IOModules[0];

            Assert.AreEqual(expectedIOModule.Name, actualIOModule.Name);
            Assert.AreEqual(expectedIOModule.Cost, actualIOModule.Cost, DELTA);
        }

        [TestMethod]
        public void SaveNew_Templates_Synchronizer()
        {
            //Arrange
            TECSystem expectedSys = null, actualSys = null;
            TECEquipment expectedTempEquip = null, actualTempEquip = null;
            TECEquipment expectedRefEquip = null, actualRefEquip = null;
            TECSubScope expectedTempSS = null, actualTempSS = null;
            TECSubScope expectedRefSSInTemp = null, actualRefSSInTemp = null;
            TECSubScope expectedRefSSInRef = null, actualRefSSInRef = null;

            #region Expected
            foreach (TECSystem sys in expectedTemplates.SystemTemplates)
            {
                if (sys.Name == "Sync System")
                {
                    expectedSys = sys;
                    break;
                }
            }
            Assert.IsNotNull(expectedSys);

            foreach (TECEquipment equip in expectedTemplates.EquipmentTemplates)
            {
                if (equip.Name == "Sync Equip")
                {
                    expectedTempEquip = equip;
                    break;
                }
            }
            Assert.IsNotNull(expectedTempEquip);

            foreach (TECEquipment equip in expectedSys.Equipment)
            {
                if (equip.Name == "Sync Equip")
                {
                    expectedRefEquip = equip;
                    break;
                }
            }
            Assert.IsNotNull(expectedRefEquip);

            foreach (TECSubScope ss in expectedTemplates.SubScopeTemplates)
            {
                if (ss.Name == "Sync SS")
                {
                    expectedTempSS = ss;
                    break;
                }
            }
            Assert.IsNotNull(expectedTempSS);

            foreach (TECSubScope ss in expectedTempEquip.SubScope)
            {
                if (ss.Name == "Sync SS")
                {
                    expectedRefSSInTemp = ss;
                    break;
                }
            }
            Assert.IsNotNull(expectedRefSSInTemp);

            foreach (TECSubScope ss in expectedRefEquip.SubScope)
            {
                if (ss.Name == "Sync SS")
                {
                    expectedRefSSInRef = ss;
                    break;
                }
            }
            Assert.IsNotNull(expectedRefSSInRef);
            #endregion

            #region Actual
            foreach (TECSystem sys in actualTemplates.SystemTemplates)
            {
                if (sys.Name == "Sync System" && sys.Guid == expectedSys.Guid)
                {
                    actualSys = sys;
                    break;
                }
            }
            Assert.IsNotNull(actualSys);

            foreach (TECEquipment equip in actualTemplates.EquipmentTemplates)
            {
                if (equip.Name == "Sync Equip" && equip.Guid == expectedTempEquip.Guid)
                {
                    actualTempEquip = equip;
                    break;
                }
            }
            Assert.IsNotNull(actualTempEquip);

            foreach (TECEquipment equip in actualSys.Equipment)
            {
                if (equip.Name == "Sync Equip" && equip.Guid == expectedRefEquip.Guid)
                {
                    actualRefEquip = equip;
                    break;
                }
            }
            Assert.IsNotNull(actualRefEquip);

            foreach (TECSubScope ss in actualTemplates.SubScopeTemplates)
            {
                if (ss.Name == "Sync SS" && ss.Guid == expectedTempSS.Guid)
                {
                    actualTempSS = ss;
                    break;
                }
            }
            Assert.IsNotNull(actualTempSS);

            foreach (TECSubScope ss in actualTempEquip.SubScope)
            {
                if (ss.Name == "Sync SS" && ss.Guid == expectedRefSSInTemp.Guid)
                {
                    actualRefSSInTemp = ss;
                    break;
                }
            }
            Assert.IsNotNull(actualRefSSInTemp);

            foreach (TECSubScope ss in actualRefEquip.SubScope)
            {
                if (ss.Name == "Sync SS" && ss.Guid == expectedRefSSInRef.Guid)
                {
                    actualRefSSInRef = ss;
                    break;
                }
            }
            Assert.IsNotNull(actualRefSSInRef);
            #endregion

            //Assert
            TemplateSynchronizer<TECEquipment> equipSync = actualTemplates.EquipmentSynchronizer;
            TemplateSynchronizer<TECSubScope> ssSync = actualTemplates.SubScopeSynchronizer;

            Assert.IsTrue(equipSync.Contains(actualTempEquip));
            Assert.IsTrue(equipSync.Contains(actualRefEquip));
            Assert.IsTrue(equipSync.GetFullDictionary()[actualTempEquip].Contains(actualRefEquip));

            Assert.IsTrue(ssSync.Contains(actualTempSS));
            Assert.IsTrue(ssSync.Contains(actualRefSSInTemp));
            Assert.IsTrue(ssSync.Contains(actualRefSSInRef));
            Assert.IsTrue(ssSync.GetFullDictionary()[actualTempSS].Contains(actualRefSSInTemp));
            Assert.IsTrue(ssSync.GetFullDictionary()[actualRefSSInTemp].Contains(actualRefSSInRef));
        }
    }
}