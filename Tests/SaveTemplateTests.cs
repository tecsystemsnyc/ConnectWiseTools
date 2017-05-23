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
    public class SaveTemplateTests
    {
        static bool DEBUG = true;

        static TECTemplates OGTemplates;
        TECTemplates templates;
        ChangeStack testStack;
        static string OGPath;
        string path;

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
            OGPath = Path.GetTempFileName();
            OGTemplates = TestHelper.CreateTestTemplates();
            EstimatingLibraryDatabase.SaveNew(OGPath, OGTemplates);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            ////Arrange
            //templates = TestHelper.CreateTestTemplates();
            //testStack = new ChangeStack(templates);
            //path = Path.GetTempFileName();
            //File.Delete(path);
            //path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".tdb";
            //EstimatingLibraryDatabase.SaveTemplatesToNewDB(path, templates);

            templates = OGTemplates.Copy() as TECTemplates;
            ModelLinkingHelper.LinkTemplates(templates);
            testStack = new ChangeStack(templates);
            path = Path.GetTempFileName();
            File.Copy(OGPath, path, true);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            if (DEBUG)
            {
                Console.WriteLine("SaveTemplates test templates: " + path);
            }
            else
            {
                File.Delete(path);
            }
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(OGPath);
        }

        #region Save Labor

        [TestMethod]
        public void Save_Templates_Labor_PMCoef()
        {
            //Act
            double expectedPM = 0.123;
            templates.Labor.PMCoef = expectedPM;
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;
            double actualPM = actualTemplates.Labor.PMCoef;

            //Assert
            Assert.AreEqual(expectedPM, actualPM);
        }

        [TestMethod]
        public void Save_Templates_Labor_PMRate()
        {
            //Act
            double expectedRate = 564.05;
            templates.Labor.PMRate = expectedRate;
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;
            double actualRate = actualTemplates.Labor.PMRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }

        [TestMethod]
        public void Save_Templates_Labor_ENGCoef()
        {
            //Act
            double expectedENG = 0.123;
            templates.Labor.ENGCoef = expectedENG;
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;
            double actualENG = actualTemplates.Labor.ENGCoef;

            //Assert
            Assert.AreEqual(expectedENG, actualENG);
        }

        [TestMethod]
        public void Save_Templates_Labor_ENGRate()
        {
            //Act
            double expectedRate = 564.05;
            templates.Labor.ENGRate = expectedRate;
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;
            double actualRate = actualTemplates.Labor.ENGRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }

        [TestMethod]
        public void Save_Templates_Labor_CommCoef()
        {
            //Act
            double expectedComm = 0.123;
            templates.Labor.CommCoef = expectedComm;
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;
            double actualComm = actualTemplates.Labor.CommCoef;

            //Assert
            Assert.AreEqual(expectedComm, actualComm);
        }

        [TestMethod]
        public void Save_Templates_Labor_CommRate()
        {
            //Act
            double expectedRate = 564.05;
            templates.Labor.CommRate = expectedRate;
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;
            double actualRate = actualTemplates.Labor.CommRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }

        [TestMethod]
        public void Save_Templates_Labor_SoftCoef()
        {
            //Act
            double expectedSoft = 0.123;
            templates.Labor.SoftCoef = expectedSoft;
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;
            double actualSoft = actualTemplates.Labor.SoftCoef;

            //Assert
            Assert.AreEqual(expectedSoft, actualSoft);
        }

        [TestMethod]
        public void Save_Templates_Labor_SoftRate()
        {
            //Act
            double expectedRate = 564.05;
            templates.Labor.SoftRate = expectedRate;
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;
            double actualRate = actualTemplates.Labor.SoftRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }

        [TestMethod]
        public void Save_Templates_Labor_GraphCoef()
        {
            //Act
            double expectedGraph = 0.123;
            templates.Labor.GraphCoef = expectedGraph;
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;
            double actualGraph = actualTemplates.Labor.GraphCoef;

            //Assert
            Assert.AreEqual(expectedGraph, actualGraph);
        }

        [TestMethod]
        public void Save_Templates_Labor_GraphRate()
        {
            //Act
            double expectedRate = 564.05;
            templates.Labor.GraphRate = expectedRate;
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;
            double actualRate = actualTemplates.Labor.GraphRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }

        [TestMethod]
        public void Save_Templates_Labor_ElecRate()
        {
            //Act
            double expectedRate = 0.123;
            templates.Labor.ElectricalRate = expectedRate;
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;
            double actualRate = actualTemplates.Labor.ElectricalRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }

        [TestMethod]
        public void Save_Templates_Labor_ElecNonUnionRate()
        {
            //Act
            double expectedRate = 0.456;
            templates.Labor.ElectricalNonUnionRate = expectedRate;
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;
            double actualRate = actualTemplates.Labor.ElectricalNonUnionRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }

        [TestMethod]
        public void Save_Templates_Labor_ElecSuperRate()
        {
            //Act
            double expectedRate = 0.123;
            templates.Labor.ElectricalSuperRate = expectedRate;
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;
            double actualRate = actualTemplates.Labor.ElectricalSuperRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }

        [TestMethod]
        public void Save_Templates_Labor_ElecSuperNonUnionRate()
        {
            //Act
            double expectedRate = 23.94;
            templates.Labor.ElectricalSuperNonUnionRate = expectedRate;
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;
            double actualRate = actualTemplates.Labor.ElectricalSuperNonUnionRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }


        #endregion Save Labor

        #region Save System

        [TestMethod]
        public void Save_Templates_Add_System()
        {
            //Act
            TECSystem expectedSystem = new TECSystem();
            expectedSystem.Name = "New system";
            expectedSystem.Description = "New system desc";
            expectedSystem.BudgetPriceModifier = 123.5;
            expectedSystem.Quantity = 1235;

            templates.SystemTemplates.Add(expectedSystem);

            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECSystem actualSystem = null;
            foreach (TECSystem system in actualTemplates.SystemTemplates)
            {
                if (expectedSystem.Guid == system.Guid)
                {
                    actualSystem = system;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedSystem.Name, actualSystem.Name);
            Assert.AreEqual(expectedSystem.Description, actualSystem.Description);
            Assert.AreEqual(expectedSystem.Quantity, actualSystem.Quantity);
            Assert.AreEqual(expectedSystem.BudgetPriceModifier, actualSystem.BudgetPriceModifier);
        }

        [TestMethod]
        public void Save_Templates_Remove_System()
        {
            //Act
            int oldNumSystems = templates.SystemTemplates.Count;
            TECSystem systemToRemove = templates.SystemTemplates[0];

            templates.SystemTemplates.Remove(systemToRemove);

            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates expectedTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            //Assert
            foreach (TECSystem system in templates.SystemTemplates)
            {
                if (system.Guid == systemToRemove.Guid)
                {
                    Assert.Fail();
                }
            }

            Assert.AreEqual((oldNumSystems - 1), templates.SystemTemplates.Count);
        }

        #region Edit System
        [TestMethod]
        public void Save_Templates_System_Name()
        {
            //Act
            TECSystem expectedSystem = templates.SystemTemplates[0];
            expectedSystem.Name = "Save System Name";
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECSystem actualSystem = null;
            foreach (TECSystem system in actualTemplates.SystemTemplates)
            {
                if (system.Guid == expectedSystem.Guid)
                {
                    actualSystem = system;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedSystem.Name, actualSystem.Name);
        }

        [TestMethod]
        public void Save_Templates_System_Description()
        {
            //Act
            TECSystem expectedSystem = templates.SystemTemplates[0];
            expectedSystem.Description = "Save System Description";
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECSystem actualSystem = null;
            foreach (TECSystem system in actualTemplates.SystemTemplates)
            {
                if (system.Guid == expectedSystem.Guid)
                {
                    actualSystem = system;
                }
            }

            //Assert
            Assert.AreEqual(expectedSystem.Description, actualSystem.Description);
        }

        [TestMethod]
        public void Save_Templates_System_Quantity()
        {
            //Act
            TECSystem expectedSystem = templates.SystemTemplates[0];
            expectedSystem.Quantity = 987654321;
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECSystem actualSystem = null;
            foreach (TECSystem system in actualTemplates.SystemTemplates)
            {
                if (system.Guid == expectedSystem.Guid)
                {
                    actualSystem = system;
                }
            }

            //Assert
            Assert.AreEqual(expectedSystem.Quantity, actualSystem.Quantity);
        }

        [TestMethod]
        public void Save_Templates_System_BudgetPrice()
        {
            //Act
            TECSystem expectedSystem = templates.SystemTemplates[0];
            expectedSystem.BudgetPriceModifier = 9876543.21;
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECSystem actualSystem = null;
            foreach (TECSystem system in actualTemplates.SystemTemplates)
            {
                if (system.Guid == expectedSystem.Guid)
                {
                    actualSystem = system;
                }
            }

            //Assert
            Assert.AreEqual(expectedSystem.BudgetPriceModifier, actualSystem.BudgetPriceModifier);
        }
        #endregion Edit System
        #endregion Save System

        #region Save Equipment
        [TestMethod]
        public void Save_Templates_Add_Equipment()
        {
            //Act
            TECEquipment expectedEquipment = new TECEquipment();
            expectedEquipment.Name = "New Equipment";
            expectedEquipment.Description = "New Equipment desc";
            expectedEquipment.BudgetUnitPrice = 123.5;
            expectedEquipment.Quantity = 1235;

            templates.EquipmentTemplates.Add(expectedEquipment);

            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECEquipment actualEquipment = null;
            foreach (TECEquipment Equipment in actualTemplates.EquipmentTemplates)
            {
                if (expectedEquipment.Guid == Equipment.Guid)
                {
                    actualEquipment = Equipment;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedEquipment.Name, actualEquipment.Name);
            Assert.AreEqual(expectedEquipment.Description, actualEquipment.Description);
            Assert.AreEqual(expectedEquipment.Quantity, actualEquipment.Quantity);
            Assert.AreEqual(expectedEquipment.BudgetUnitPrice, actualEquipment.BudgetUnitPrice);
        }

        [TestMethod]
        public void Save_Templates_Remove_Equipment()
        {
            //Act
            int oldNumEquipments = templates.EquipmentTemplates.Count;
            TECEquipment EquipmentToRemove = templates.EquipmentTemplates[0];

            templates.EquipmentTemplates.Remove(EquipmentToRemove);

            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            //Assert
            foreach (TECEquipment Equipment in actualTemplates.EquipmentTemplates)
            {
                if (Equipment.Guid == EquipmentToRemove.Guid)
                {
                    Assert.Fail();
                }
            }

            Assert.AreEqual((oldNumEquipments - 1), actualTemplates.EquipmentTemplates.Count);
        }

        [TestMethod]
        public void Save_Templates_Add_Equipment_InSystem()
        {
            //Act
            TECEquipment expectedEquipment = new TECEquipment();
            expectedEquipment.Name = "New System Equipment";
            expectedEquipment.Description = "System equipment description";
            expectedEquipment.BudgetUnitPrice = 468.3;
            expectedEquipment.Quantity = 5;

            TECSystem sysToModify = templates.SystemTemplates[0];

            sysToModify.Equipment.Add(expectedEquipment);

            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECSystem actualSystem = null;
            foreach (TECSystem sys in actualTemplates.SystemTemplates)
            {
                if (sys.Guid == sysToModify.Guid)
                {
                    actualSystem = sys;
                    break;
                }
            }

            TECEquipment actualEquipment = null;
            foreach (TECEquipment equip in actualSystem.Equipment)
            {
                if (equip.Guid == expectedEquipment.Guid)
                {
                    actualEquipment = equip;
                    break;
                }
            }

            //Assert
            Assert.IsNotNull(actualEquipment);
            Assert.AreEqual(expectedEquipment.Name, actualEquipment.Name);
            Assert.AreEqual(expectedEquipment.Description, actualEquipment.Description);
            Assert.AreEqual(expectedEquipment.Quantity, actualEquipment.Quantity);
            Assert.AreEqual(expectedEquipment.BudgetUnitPrice, actualEquipment.BudgetUnitPrice);
            foreach (TECEquipment equip in actualTemplates.EquipmentTemplates)
            {
                if (equip.Guid == actualEquipment.Guid) Assert.Fail();
            }
        }

        [TestMethod]
        public void Save_Templates_Remove_Equipment_FromSystem()
        {
            //Act
            TECSystem sysToModify = null;
            TECEquipment equipToRemove = null;
            int oldNumEquip = 0;
            foreach (TECSystem sys in templates.SystemTemplates)
            {
                if (sys.Equipment.Count > 0)
                {
                    sysToModify = sys;
                    equipToRemove = sysToModify.Equipment[0];
                    oldNumEquip = sysToModify.Equipment.Count;
                    break;
                }
            }

            sysToModify.Equipment.Remove(equipToRemove);

            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            //Assert
            TECSystem actualSystem = null;
            foreach (TECSystem sys in actualTemplates.SystemTemplates)
            {
                if (sys.Guid == sysToModify.Guid)
                {
                    actualSystem = sys;
                    foreach (TECEquipment equip in actualSystem.Equipment)
                    {
                        if (equip.Guid == equipToRemove.Guid)
                        {
                            Assert.Fail();
                        }
                    }
                    break;
                }
            }

            Assert.AreEqual((oldNumEquip - 1), actualSystem.Equipment.Count);
        }

        [TestMethod]
        public void Save_Templates_Equipment_Name()
        {
            //Act
            TECEquipment expectedEquipment = templates.EquipmentTemplates[0];
            expectedEquipment.Name = "Save Equipment Name";
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECEquipment actualEquipment = null;
            foreach (TECEquipment Equipment in actualTemplates.EquipmentTemplates)
            {
                if (Equipment.Guid == expectedEquipment.Guid)
                {
                    actualEquipment = Equipment;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedEquipment.Name, actualEquipment.Name);
        }

        [TestMethod]
        public void Save_Templates_Equipment_Description()
        {
            //Act
            TECEquipment expectedEquipment = templates.EquipmentTemplates[0];
            expectedEquipment.Description = "Save Equipment Description";
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECEquipment actualEquipment = null;
            foreach (TECEquipment Equipment in actualTemplates.EquipmentTemplates)
            {
                if (Equipment.Guid == expectedEquipment.Guid)
                {
                    actualEquipment = Equipment;
                }
            }

            //Assert
            Assert.AreEqual(expectedEquipment.Description, actualEquipment.Description);
        }

        [TestMethod]
        public void Save_Templates_Equipment_Quantity()
        {
            //Act
            TECEquipment expectedEquipment = templates.EquipmentTemplates[0];
            expectedEquipment.Quantity = 987654321;
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECEquipment actualEquipment = null;
            foreach (TECEquipment Equipment in actualTemplates.EquipmentTemplates)
            {
                if (Equipment.Guid == expectedEquipment.Guid)
                {
                    actualEquipment = Equipment;
                }
            }

            //Assert
            Assert.AreEqual(expectedEquipment.Quantity, actualEquipment.Quantity);
        }

        [TestMethod]
        public void Save_Templates_Equipment_BudgetPrice()
        {
            //Act
            TECEquipment expectedEquipment = templates.EquipmentTemplates[0];
            expectedEquipment.BudgetUnitPrice = 9876543.21;
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECEquipment actualEquipment = null;
            foreach (TECEquipment Equipment in actualTemplates.EquipmentTemplates)
            {
                if (Equipment.Guid == expectedEquipment.Guid)
                {
                    actualEquipment = Equipment;
                }
            }

            //Assert
            Assert.AreEqual(expectedEquipment.BudgetUnitPrice, actualEquipment.BudgetUnitPrice);
        }
        #endregion Save Equipment

        #region Save SubScope
        [TestMethod]
        public void Save_Templates_Add_SubScope()
        {
            //Act
            TECSubScope expectedSubScope = new TECSubScope();
            expectedSubScope.Name = "New SubScope";
            expectedSubScope.Description = "New SubScope desc";
            expectedSubScope.Quantity = 1235;

            templates.SubScopeTemplates.Add(expectedSubScope);

            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECSubScope actualSubScope = null;
            foreach (TECSubScope subScope in actualTemplates.SubScopeTemplates)
            {
                if (expectedSubScope.Guid == subScope.Guid)
                {
                    actualSubScope = subScope;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedSubScope.Name, actualSubScope.Name);
            Assert.AreEqual(expectedSubScope.Description, actualSubScope.Description);
            Assert.AreEqual(expectedSubScope.Quantity, actualSubScope.Quantity);
        }

        [TestMethod]
        public void Save_Templates_Remove_SubScope()
        {
            //Act
            int oldNumSubScopes = templates.SubScopeTemplates.Count;
            TECSubScope SubScopeToRemove = templates.SubScopeTemplates[0];

            templates.SubScopeTemplates.Remove(SubScopeToRemove);

            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            //Assert
            foreach (TECSubScope SubScope in actualTemplates.SubScopeTemplates)
            {
                if (SubScope.Guid == SubScopeToRemove.Guid)
                {
                    Assert.Fail();
                }
            }

            Assert.AreEqual((oldNumSubScopes - 1), actualTemplates.SubScopeTemplates.Count);
        }

        [TestMethod]
        public void Save_Templates_SubScope_Name()
        {
            //Act
            TECSubScope expectedSubScope = templates.SubScopeTemplates[0];
            expectedSubScope.Name = "Save SubScope Name";
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECSubScope actualSubScope = null;
            foreach (TECSubScope SubScope in actualTemplates.SubScopeTemplates)
            {
                if (SubScope.Guid == expectedSubScope.Guid)
                {
                    actualSubScope = SubScope;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedSubScope.Name, actualSubScope.Name);
        }

        [TestMethod]
        public void Save_Templates_SubScope_Description()
        {
            //Act
            TECSubScope expectedSubScope = templates.SubScopeTemplates[0];
            expectedSubScope.Description = "Save SubScope Description";
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECSubScope actualSubScope = null;
            foreach (TECSubScope SubScope in actualTemplates.SubScopeTemplates)
            {
                if (SubScope.Guid == expectedSubScope.Guid)
                {
                    actualSubScope = SubScope;
                }
            }

            //Assert
            Assert.AreEqual(expectedSubScope.Description, actualSubScope.Description);
        }

        [TestMethod]
        public void Save_Templates_SubScope_Quantity()
        {
            //Act
            TECSubScope expectedSubScope = templates.SubScopeTemplates[0];
            expectedSubScope.Quantity = 987654321;
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECSubScope actualSubScope = null;
            foreach (TECSubScope SubScope in actualTemplates.SubScopeTemplates)
            {
                if (SubScope.Guid == expectedSubScope.Guid)
                {
                    actualSubScope = SubScope;
                }
            }

            //Assert
            Assert.AreEqual(expectedSubScope.Quantity, actualSubScope.Quantity);
        }

        [TestMethod]
        public void Save_Templates_SubScope_AssociatedCosts()
        {
            //Act
            TECSubScope expectedSubScope = templates.SubScopeTemplates[0];

            TECCost expectedCost = templates.Catalogs.AssociatedCosts[1];
            expectedSubScope.AssociatedCosts.Add(templates.Catalogs.AssociatedCosts[1]);
            int expectedNumCosts = expectedSubScope.AssociatedCosts.Count;
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECSubScope actualSubScope = null;
            TECCost actualCost = null;
            foreach (TECSubScope SubScope in actualTemplates.SubScopeTemplates)
            {
                if (SubScope.Guid == expectedSubScope.Guid)
                {
                    actualSubScope = SubScope;
                    foreach (TECCost cost in actualSubScope.AssociatedCosts)
                    {
                        if (cost.Guid == expectedCost.Guid)
                        {
                            actualCost = cost;
                            break;
                        }
                    }
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedNumCosts, actualSubScope.AssociatedCosts.Count);
            Assert.AreEqual(expectedCost.Name, actualCost.Name);
            Assert.AreEqual(expectedCost.Cost, actualCost.Cost);

        }
        #endregion Save SubScope

        #region Save Device
        [TestMethod]
        public void Save_Templates_Add_Device()
        {
            //Act
            ObservableCollection<TECConnectionType> types = new ObservableCollection<TECConnectionType>();
            types.Add(templates.Catalogs.ConnectionTypes[0]);
            TECDevice expectedDevice = new TECDevice(Guid.NewGuid(), 
                types,
                templates.Catalogs.Manufacturers[0]);
            expectedDevice.Name = "New Device";
            expectedDevice.Description = "New Device desc";
            expectedDevice.Cost = 11.54;

            templates.Catalogs.Devices.Add(expectedDevice);

            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECDevice actualDevice = null;
            foreach (TECDevice device in actualTemplates.Catalogs.Devices)
            {
                if (device.Guid == expectedDevice.Guid)
                {
                    actualDevice = device;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedDevice.Name, actualDevice.Name);
            Assert.AreEqual(expectedDevice.Description, actualDevice.Description);
            Assert.AreEqual(expectedDevice.Cost, actualDevice.Cost);
            Assert.AreEqual(expectedDevice.ConnectionTypes[0].Name, actualDevice.ConnectionTypes[0].Name);
            Assert.AreEqual(expectedDevice.Quantity, actualDevice.Quantity);
        }

        [TestMethod]
        public void Save_Templates_Remove_Device()
        {
            //Act
            int oldNumDevices = templates.Catalogs.Devices.Count;
            TECDevice deviceToRemove = templates.Catalogs.Devices[0];

            templates.Catalogs.Devices.Remove(deviceToRemove);

            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            //Assert
            foreach (TECDevice dev in actualTemplates.Catalogs.Devices)
            {
                if (dev.Guid == deviceToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual((oldNumDevices - 1), actualTemplates.Catalogs.Devices.Count);
        }

        [TestMethod]
        public void Save_Templates_Device_Name()
        {
            //Act
            TECDevice expectedDevice = templates.Catalogs.Devices[0];
            expectedDevice.Name = "Save Device Name";
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECDevice actualDevice = null;
            foreach (TECDevice Device in actualTemplates.Catalogs.Devices)
            {
                if (Device.Guid == expectedDevice.Guid)
                {
                    actualDevice = Device;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedDevice.Name, actualDevice.Name);
        }

        [TestMethod]
        public void Save_Templates_Device_Description()
        {
            //Act
            TECDevice expectedDevice = templates.Catalogs.Devices[0];
            expectedDevice.Description = "Save Device Description";
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECDevice actualDevice = null;
            foreach (TECDevice Device in actualTemplates.Catalogs.Devices)
            {
                if (Device.Guid == expectedDevice.Guid)
                {
                    actualDevice = Device;
                }
            }

            //Assert
            Assert.AreEqual(expectedDevice.Description, actualDevice.Description);
        }

        [TestMethod]
        public void Save_Templates_Device_Cost()
        {
            //Act
            TECDevice expectedDevice = templates.Catalogs.Devices[0];
            expectedDevice.Cost = 46.89;
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECDevice actualDevice = null;
            foreach (TECDevice Device in actualTemplates.Catalogs.Devices)
            {
                if (Device.Guid == expectedDevice.Guid)
                {
                    actualDevice = Device;
                }
            }

            //Assert
            Assert.AreEqual(expectedDevice.Cost, actualDevice.Cost);
        }

        [TestMethod]
        public void Save_Templates_Device_ConnectionType()
        {
            //Act
            TECDevice expectedDevice = templates.Catalogs.Devices[0];
            var testConnectionType = new TECConnectionType();
            testConnectionType.Name = "Test Add Connection Type Device";
            templates.Catalogs.ConnectionTypes.Add(testConnectionType);
            expectedDevice.ConnectionTypes.Add(testConnectionType);
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECDevice actualDevice = null;
            foreach (TECDevice Device in actualTemplates.Catalogs.Devices)
            {
                if (Device.Guid == expectedDevice.Guid)
                {
                    actualDevice = Device;
                }
            }

            //Assert
            foreach(TECConnectionType expectedConnectionType in expectedDevice.ConnectionTypes)
            {
                bool found = false;
                foreach(TECConnectionType actualConnectionType in actualDevice.ConnectionTypes)
                {
                    if (expectedConnectionType.Guid == actualConnectionType.Guid)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    Assert.Fail("Connectiontype not found in device.");
                }
            }
        }

        [TestMethod]
        public void Save_Templates_Device_Manufacturer()
        {
            //Act
            TECDevice expectedDevice = templates.Catalogs.Devices[0];
            TECManufacturer manToAdd = new TECManufacturer();
            manToAdd.Name = "Test";
            manToAdd.Multiplier = 1;
            templates.Catalogs.Manufacturers.Add(manToAdd);
            expectedDevice.Manufacturer = manToAdd;
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECDevice actualDevice = null;
            foreach (TECDevice Device in actualTemplates.Catalogs.Devices)
            {
                if (Device.Guid == expectedDevice.Guid)
                {
                    actualDevice = Device;
                }
            }

            //Assert
            Assert.AreEqual(expectedDevice.Manufacturer.Guid, actualDevice.Manufacturer.Guid);
        }
        #endregion Save Device

        #region Save Controller
        [TestMethod]
        public void Save_Templates_Add_Controller()
        {
            //Act
            TECController expectedController = new TECController(Guid.NewGuid(), templates.Catalogs.Manufacturers[0]);
            expectedController.Name = "Test Controller";
            expectedController.Description = "Test description";
            expectedController.Cost = 100;
            expectedController.Manufacturer = templates.Catalogs.Manufacturers[0];

            templates.ControllerTemplates.Add(expectedController);

            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECController actualController = null;
            foreach (TECController controller in actualTemplates.ControllerTemplates)
            {
                if (controller.Guid == expectedController.Guid)
                {
                    actualController = controller;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedController.Name, actualController.Name);
            Assert.AreEqual(expectedController.Description, actualController.Description);
            Assert.AreEqual(expectedController.Cost, actualController.Cost);
        }

        [TestMethod]
        public void Save_Templates_Remove_Controller()
        {
            //Act
            int oldNumControllers = templates.ControllerTemplates.Count;
            TECController controllerToRemove = templates.ControllerTemplates[0];

            templates.ControllerTemplates.Remove(controllerToRemove);

            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            //Assert
            foreach (TECController controller in actualTemplates.ControllerTemplates)
            {
                if (controller.Guid == controllerToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual((oldNumControllers - 1), actualTemplates.ControllerTemplates.Count);

        }

        [TestMethod]
        public void Save_Templates_Controller_Name()
        {
            //Act
            TECController expectedController = templates.ControllerTemplates[0];
            expectedController.Name = "Test save controller name";
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECController actualController = null;
            foreach (TECController controller in actualTemplates.ControllerTemplates)
            {
                if (controller.Guid == expectedController.Guid)
                {
                    actualController = controller;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedController.Name, actualController.Name);
        }

        [TestMethod]
        public void Save_Templates_Controller_Description()
        {
            //Act
            TECController expectedController = templates.ControllerTemplates[0];
            expectedController.Description = "Save Device Description";
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECController actualController = null;
            foreach (TECController controller in actualTemplates.ControllerTemplates)
            {
                if (controller.Guid == expectedController.Guid)
                {
                    actualController = controller;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedController.Description, actualController.Description);
        }

        [TestMethod]
        public void Save_Templates_Controller_Cost()
        {
            //Act
            TECController expectedController = templates.ControllerTemplates[0];
            expectedController.Cost = 46.89;
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECController actualController = null;
            foreach (TECController controller in actualTemplates.ControllerTemplates)
            {
                if (controller.Guid == expectedController.Guid)
                {
                    actualController = controller;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedController.Cost, actualController.Cost);
        }

        [TestMethod]
        public void Save_Templates_Controller_Manufacturer()
        {
            //Act
            TECController expectedController = templates.ControllerTemplates[0];
            var testManufacturer = new TECManufacturer();
            templates.Catalogs.Manufacturers.Add(testManufacturer);
            expectedController.Manufacturer = testManufacturer;
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECController actualController = null;
            foreach (TECController controller in actualTemplates.ControllerTemplates)
            {
                if (controller.Guid == expectedController.Guid)
                {
                    actualController = controller;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedController.Manufacturer.Guid, actualController.Manufacturer.Guid);
        }

        #region Controller IO
        [TestMethod]
        public void Save_Templates_Controller_Add_IO()
        {
            //Act
            TECController expectedController = templates.ControllerTemplates[0];
            var testio = new TECIO();
            testio.Type = IOType.BACnetIP;
            expectedController.IO.Add(testio);
            bool hasBACnetIP = false;
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;
            TECController actualController = null;
            foreach (TECController controller in actualTemplates.ControllerTemplates)
            {
                if (controller.Guid == expectedController.Guid)
                {
                    actualController = controller;
                    break;
                }
            }

            //Assert
            foreach (TECIO io in actualController.IO)
            {
                if (io.Type == IOType.BACnetIP)
                {
                    hasBACnetIP = true;
                }
            }

            Assert.IsTrue(hasBACnetIP);

        }

        [TestMethod]
        public void Save_Templates_Controller_Remove_IO()
        {
            //Act
            TECController expectedController = templates.ControllerTemplates[0];
            int oldNumIO = expectedController.IO.Count;
            TECIO ioToRemove = expectedController.IO[0];

            expectedController.IO.Remove(ioToRemove);

            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECController actualController = null;
            foreach (TECController con in actualTemplates.ControllerTemplates)
            {
                if (con.Guid == expectedController.Guid)
                {
                    actualController = con;
                    break;
                }
            }

            //Assert
            foreach (TECIO io in actualController.IO)
            {
                if (io.Type == ioToRemove.Type) { Assert.Fail(); }
            }

            Assert.AreEqual((oldNumIO - 1), actualController.IO.Count);
        }

        [TestMethod]
        public void Save_Templates_Controller_IO_Quantity()
        {
            //Act
            TECController expectedController = templates.ControllerTemplates[0];
            TECIO ioToChange = expectedController.IO[0];
            ioToChange.Quantity = 69;

            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECController actualController = null;
            foreach (TECController con in actualTemplates.ControllerTemplates)
            {
                if (con.Guid == expectedController.Guid)
                {
                    actualController = con;
                    break;
                }
            }

            //Assert
            foreach (TECIO io in actualController.IO)
            {
                if (io.Type == ioToChange.Type)
                {
                    Assert.AreEqual(ioToChange.Quantity, io.Quantity);
                    break;
                }
            }
        }
        #endregion Controller IO

        #endregion

        #region Save Manufacturer
        [TestMethod]
        public void Save_Templates_Add_Manufacturer()
        {
            //Act
            int oldNumManufacturers = templates.Catalogs.Manufacturers.Count;
            TECManufacturer expectedManufacturer = new TECManufacturer();
            expectedManufacturer.Name = "Test Add Manufacturer";
            expectedManufacturer.Multiplier = 21.34;

            templates.Catalogs.Manufacturers.Add(expectedManufacturer);

            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECManufacturer actualManufacturer = null;
            foreach (TECManufacturer man in actualTemplates.Catalogs.Manufacturers)
            {
                if (man.Guid == expectedManufacturer.Guid)
                {
                    actualManufacturer = man;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedManufacturer.Name, actualManufacturer.Name);
            Assert.AreEqual(expectedManufacturer.Multiplier, actualManufacturer.Multiplier);
            Assert.AreEqual((oldNumManufacturers + 1), actualTemplates.Catalogs.Manufacturers.Count);

        }

        [TestMethod]
        public void Save_Templates_Manufacturer_Name()
        {
            //Act
            TECManufacturer expectedManufacturer = templates.Catalogs.Manufacturers[0];
            expectedManufacturer.Name = "Test save manufacturer name";
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECManufacturer actualMan = null;
            foreach (TECManufacturer man in actualTemplates.Catalogs.Manufacturers)
            {
                if (man.Guid == expectedManufacturer.Guid)
                {
                    actualMan = man;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedManufacturer.Name, actualMan.Name);
        }

        [TestMethod]
        public void Save_Templates_Manufacturer_Multiplier()
        {
            //Act
            TECManufacturer expectedManufacturer = templates.Catalogs.Manufacturers[0];
            expectedManufacturer.Multiplier = 987.41;
            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECManufacturer actualMan = null;
            foreach (TECManufacturer man in actualTemplates.Catalogs.Manufacturers)
            {
                if (man.Guid == expectedManufacturer.Guid)
                {
                    actualMan = man;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedManufacturer.Multiplier, actualMan.Multiplier);


        }
        #endregion SaveManufacturer

        #region Save Tag
        [TestMethod]
        public void Save_Templates_Add_Tag()
        {
            //Act
            int oldNumTags = templates.Catalogs.Tags.Count;
            TECTag expectedTag = new TECTag();
            expectedTag.Text = "Test add tag";

            templates.Catalogs.Tags.Add(expectedTag);

            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECTag actualTag = null;
            foreach (TECTag tag in actualTemplates.Catalogs.Tags)
            {
                if (tag.Guid == expectedTag.Guid)
                {
                    actualTag = tag;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedTag.Text, actualTag.Text);
            Assert.AreEqual((oldNumTags + 1), actualTemplates.Catalogs.Tags.Count);
        }

        [TestMethod]
        public void Save_Templates_Remove_Tag()
        {
            //Act
            int oldNumTags = templates.Catalogs.Tags.Count;
            TECTag tagToRemove = templates.Catalogs.Tags[0];

            templates.Catalogs.Tags.Remove(tagToRemove);

            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            //Assert
            foreach (TECTag tag in actualTemplates.Catalogs.Tags)
            {
                if (tag.Guid == tagToRemove.Guid) { Assert.Fail(); }
            }

            Assert.AreEqual((oldNumTags - 1), actualTemplates.Catalogs.Tags.Count);
        }
        #endregion Save Tag

        #region Save Connection Type
        [TestMethod]
        public void Save_Templates_Add_ConnectionType()
        {
            //Act
            int oldNumConnectionTypes = templates.Catalogs.ConnectionTypes.Count;
            TECConnectionType expectedConnectionType = new TECConnectionType();
            expectedConnectionType.Name = "Test Add Connection Type";
            expectedConnectionType.Cost = 21.34;

            templates.Catalogs.ConnectionTypes.Add(expectedConnectionType);

            TECCost expectedCost = templates.Catalogs.AssociatedCosts[0];
            expectedConnectionType.AssociatedCosts.Add(expectedCost);
            int expectedCostCount = expectedConnectionType.AssociatedCosts.Count;

            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECConnectionType actualConnectionType = null;
            TECCost actualCost = null;
            foreach (TECConnectionType connectionType in actualTemplates.Catalogs.ConnectionTypes)
            {
                if (connectionType.Guid == expectedConnectionType.Guid)
                {
                    actualConnectionType = connectionType;
                    actualCost = actualConnectionType.AssociatedCosts[0];
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedConnectionType.Name, actualConnectionType.Name);
            Assert.AreEqual(expectedConnectionType.Cost, actualConnectionType.Cost);
            Assert.AreEqual((oldNumConnectionTypes + 1), actualTemplates.Catalogs.ConnectionTypes.Count);
            Assert.AreEqual(expectedCostCount, actualConnectionType.AssociatedCosts.Count);
            Assert.AreEqual(expectedCost.Cost, actualCost.Cost);
            Assert.AreEqual(expectedCost.Name, actualCost.Name);

        }

        #endregion

        #region Save Conduit Type
        [TestMethod]
        public void Save_Templates_Add_ConduitType()
        {
            //Act
            int oldNumConduitTypes = templates.Catalogs.ConduitTypes.Count;
            TECConduitType expectedConduitType = new TECConduitType();
            expectedConduitType.Name = "Test Add Conduit Type";
            expectedConduitType.Cost = 21.34;

            templates.Catalogs.ConduitTypes.Add(expectedConduitType);

            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECConduitType actualConnectionType = null;
            foreach (TECConduitType conduitType in actualTemplates.Catalogs.ConduitTypes)
            {
                if (conduitType.Guid == expectedConduitType.Guid)
                {
                    actualConnectionType = conduitType;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedConduitType.Name, actualConnectionType.Name);
            Assert.AreEqual(expectedConduitType.Cost, actualConnectionType.Cost);
            Assert.AreEqual((oldNumConduitTypes + 1), actualTemplates.Catalogs.ConduitTypes.Count);

        }

        [TestMethod]
        public void Save_Templates_Remove_ConduitType()
        {
            //Act
            int oldNumConduitTypes = templates.Catalogs.ConduitTypes.Count;
            TECConduitType conduitTypeToRemove = templates.Catalogs.ConduitTypes[0];

            templates.Catalogs.ConduitTypes.Remove(conduitTypeToRemove);

            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            //Assert
            foreach (TECConduitType conduitType in actualTemplates.Catalogs.ConduitTypes)
            {
                if (conduitType.Guid == conduitTypeToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual((oldNumConduitTypes - 1), actualTemplates.Catalogs.ConduitTypes.Count);
        }
        #endregion

        #region Save Associated Costs
        [TestMethod]
        public void Save_Templates_Add_AssociatedCost()
        {
            //Act
            int oldNumAssociatedCosts = templates.Catalogs.AssociatedCosts.Count;
            TECCost expectedAssociatedCost = new TECCost();
            expectedAssociatedCost.Name = "Test Associated Cost";
            expectedAssociatedCost.Cost = 21.34;

            templates.Catalogs.AssociatedCosts.Add(expectedAssociatedCost);

            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECCost actualCost = null;
            foreach (TECCost cost in actualTemplates.Catalogs.AssociatedCosts)
            {
                if (cost.Guid == expectedAssociatedCost.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedAssociatedCost.Name, actualCost.Name);
            Assert.AreEqual(expectedAssociatedCost.Cost, actualCost.Cost);
            Assert.AreEqual((oldNumAssociatedCosts + 1), actualTemplates.Catalogs.AssociatedCosts.Count);

        }

        [TestMethod]
        public void Save_Templates_Remove_AssociatedCost()
        {
            //Act
            int oldNumAssociatedCosts = templates.Catalogs.AssociatedCosts.Count;
            TECCost costToRemove = templates.Catalogs.AssociatedCosts[0];

            templates.Catalogs.AssociatedCosts.Remove(costToRemove);

            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            //Assert
            foreach (TECCost cost in actualTemplates.Catalogs.AssociatedCosts)
            {
                if (cost.Guid == costToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual((oldNumAssociatedCosts - 1), actualTemplates.Catalogs.AssociatedCosts.Count);
        }
        #endregion

        #region Save Misc Cost
        [TestMethod]
        public void Save_Templates_Add_MiscCost()
        {
            //Act
            TECMisc expectedCost = new TECMisc();
            expectedCost.Name = "Add cost addition";
            expectedCost.Cost = 978.3;
            expectedCost.Quantity = 21;

            templates.MiscCostTemplates.Add(expectedCost);

            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECMisc actualCost = null;
            foreach (TECMisc cost in actualTemplates.MiscCostTemplates)
            {
                if (cost.Guid == expectedCost.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedCost.Name, actualCost.Name);
            Assert.AreEqual(expectedCost.Cost, actualCost.Cost);
            Assert.AreEqual(expectedCost.Quantity, actualCost.Quantity);
        }

        [TestMethod]
        public void Save_Templates_Remove_MiscCost()
        {
            //Act
            TECMisc costToRemove = templates.MiscCostTemplates[0];
            templates.MiscCostTemplates.Remove(costToRemove);

            EstimatingLibraryDatabase.Update(path, testStack);
            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            //Assert
            foreach (TECMisc cost in actualTemplates.MiscCostTemplates)
            {
                if (cost.Guid == costToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual(templates.MiscCostTemplates.Count, actualTemplates.MiscCostTemplates.Count);
        }

        [TestMethod]
        public void Save_Templates_MiscCost_Name()
        {
            //Act
            TECMisc expectedCost = templates.MiscCostTemplates[0];
            expectedCost.Name = "Test Save Cost Name";

            EstimatingLibraryDatabase.Update(path, testStack);
            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECMisc actualCost = null;
            foreach (TECMisc cost in actualTemplates.MiscCostTemplates)
            {
                if (cost.Guid == expectedCost.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedCost.Name, actualCost.Name);
        }

        [TestMethod]
        public void Save_Templates_MiscCost_Cost()
        {
            //Act
            TECMisc expectedCost = templates.MiscCostTemplates[0];
            expectedCost.Cost = 489.1238;

            EstimatingLibraryDatabase.Update(path, testStack);
            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECMisc actualCost = null;
            foreach (TECMisc cost in actualTemplates.MiscCostTemplates)
            {
                if (cost.Guid == expectedCost.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedCost.Cost, actualCost.Cost);
        }

        #endregion

        #region Save Misc Wiring
        [TestMethod]
        public void Save_Templates_Add_MiscWiring()
        {
            //Act
            TECMisc expectedCost = new TECMisc();
            expectedCost.Name = "Add cost addition";
            expectedCost.Cost = 978.3;
            expectedCost.Quantity = 21;

            templates.MiscWiringTemplates.Add(expectedCost);

            EstimatingLibraryDatabase.Update(path, testStack);
            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECMisc actualCost = null;
            foreach (TECMisc cost in actualTemplates.MiscWiringTemplates)
            {
                if (cost.Guid == expectedCost.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedCost.Name, actualCost.Name);
            Assert.AreEqual(expectedCost.Cost, actualCost.Cost);
            Assert.AreEqual(expectedCost.Quantity, actualCost.Quantity);
        }

        [TestMethod]
        public void Save_Templates_Remove_MiscWiring()
        {
            //Act
            TECMisc costToRemove = templates.MiscWiringTemplates[0];
            templates.MiscWiringTemplates.Remove(costToRemove);

            EstimatingLibraryDatabase.Update(path, testStack);
            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            //Assert
            foreach (TECMisc cost in actualTemplates.MiscWiringTemplates)
            {
                if (cost.Guid == costToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual(templates.MiscWiringTemplates.Count, templates.MiscWiringTemplates.Count);
        }

        [TestMethod]
        public void Save_Templates_MiscWiring_Name()
        {
            //Act
            TECMisc expectedCost = templates.MiscWiringTemplates[0];
            expectedCost.Name = "Test Save Cost Name";

            EstimatingLibraryDatabase.Update(path, testStack);
            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECMisc actualCost = null;
            foreach (TECMisc cost in actualTemplates.MiscWiringTemplates)
            {
                if (cost.Guid == expectedCost.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedCost.Name, actualCost.Name);
        }

        [TestMethod]
        public void Save_Templates_MiscWiring_Cost()
        {
            //Act
            TECMisc expectedCost = templates.MiscWiringTemplates[0];
            expectedCost.Cost = 489.1238;

            EstimatingLibraryDatabase.Update(path, testStack);
            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECMisc actualCost = null;
            foreach (TECMisc cost in actualTemplates.MiscWiringTemplates)
            {
                if (cost.Guid == expectedCost.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedCost.Cost, actualCost.Cost);
        }

        [TestMethod]
        public void Save_Templates_MiscWiring_Quantity()
        {
            //Act
            TECMisc expectedCost = templates.MiscWiringTemplates[0];
            expectedCost.Quantity = 492;

            EstimatingLibraryDatabase.Update(path, testStack);
            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECMisc actualCost = null;
            foreach (TECMisc cost in actualTemplates.MiscWiringTemplates)
            {
                if (cost.Guid == expectedCost.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedCost.Quantity, actualCost.Quantity);
        }
        #endregion

        #region Save Panel Type
        [TestMethod]
        public void Save_Templates_Add_PanelType()
        {
            //Act
            TECPanelType expectedCost = new TECPanelType();
            expectedCost.Name = "Add cost addition";
            expectedCost.Cost = 978.3;

            templates.Catalogs.PanelTypes.Add(expectedCost);

            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECPanelType actualCost = null;
            foreach (TECPanelType cost in actualTemplates.Catalogs.PanelTypes)
            {
                if (cost.Guid == expectedCost.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedCost.Name, actualCost.Name);
            Assert.AreEqual(expectedCost.Cost, actualCost.Cost);
        }

        [TestMethod]
        public void Save_Templates_Remove_PanelType()
        {
            //Act
            TECPanelType costToRemove = templates.Catalogs.PanelTypes[0];
            templates.Catalogs.PanelTypes.Remove(costToRemove);

            EstimatingLibraryDatabase.Update(path, testStack);
            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            //Assert
            foreach (TECPanelType cost in actualTemplates.Catalogs.PanelTypes)
            {
                if (cost.Guid == costToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual(templates.Catalogs.PanelTypes.Count, actualTemplates.Catalogs.PanelTypes.Count);
        }

        [TestMethod]
        public void Save_Templates_PanelType_Name()
        {
            //Act
            TECPanelType expectedCost = templates.Catalogs.PanelTypes[0];
            expectedCost.Name = "Test Save Cost Name";

            EstimatingLibraryDatabase.Update(path, testStack);
            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECPanelType actualCost = null;
            foreach (TECPanelType cost in actualTemplates.Catalogs.PanelTypes)
            {
                if (cost.Guid == expectedCost.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedCost.Name, actualCost.Name);
        }

        [TestMethod]
        public void Save_Templates_PanelType_Cost()
        {
            //Act
            TECPanelType expectedCost = templates.Catalogs.PanelTypes[0];
            expectedCost.Cost = 489.1238;

            EstimatingLibraryDatabase.Update(path, testStack);
            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECPanelType actualCost = null;
            foreach (TECPanelType cost in actualTemplates.Catalogs.PanelTypes)
            {
                if (cost.Guid == expectedCost.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedCost.Cost, actualCost.Cost);
        }

        #endregion

        #region Save IOModule
        [TestMethod]
        public void Save_Templates_Add_IOModule()
        {
            //Act
            TECIOModule expectedModule = new TECIOModule();
            expectedModule.Name = "Add IO Module";
            expectedModule.Cost = 978.3;
            expectedModule.Manufacturer = templates.Catalogs.Manufacturers[0];

            templates.Catalogs.IOModules.Add(expectedModule);

            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECIOModule actualCost = null;
            foreach (TECIOModule cost in actualTemplates.Catalogs.IOModules)
            {
                if (cost.Guid == expectedModule.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedModule.Name, actualCost.Name);
            Assert.AreEqual(expectedModule.Cost, actualCost.Cost);
        }

        [TestMethod]
        public void Save_Templates_Remove_IOModule()
        {
            //Act
            TECIOModule costToRemove = templates.Catalogs.IOModules[0];
            templates.Catalogs.IOModules.Remove(costToRemove);

            EstimatingLibraryDatabase.Update(path, testStack);
            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            //Assert
            foreach (TECIOModule cost in actualTemplates.Catalogs.IOModules)
            {
                if (cost.Guid == costToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual(templates.Catalogs.IOModules.Count, actualTemplates.Catalogs.IOModules.Count);
        }

        [TestMethod]
        public void Save_Templates_IOModule_Name()
        {
            //Act
            TECIOModule expectedCost = templates.Catalogs.IOModules[0];
            expectedCost.Name = "Test Save IOModule Name";

            EstimatingLibraryDatabase.Update(path, testStack);
            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECIOModule actualCost = null;
            foreach (TECIOModule cost in actualTemplates.Catalogs.IOModules)
            {
                if (cost.Guid == expectedCost.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedCost.Name, actualCost.Name);
        }

        [TestMethod]
        public void Save_Templates_IOModule_Cost()
        {
            //Act
            TECIOModule expectedCost = templates.Catalogs.IOModules[0];
            expectedCost.Cost = 489.1238;

            EstimatingLibraryDatabase.Update(path, testStack);
            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECIOModule actualCost = null;
            foreach (TECIOModule cost in actualTemplates.Catalogs.IOModules)
            {
                if (cost.Guid == expectedCost.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedCost.Cost, actualCost.Cost);
        }

        #endregion

        #region Save Panel
        [TestMethod]
        public void Save_Templates_Add_Panel()
        {
            //Act
            TECPanel expectedPanel = new TECPanel(templates.Catalogs.PanelTypes[0]);
            expectedPanel.Name = "Test Add Controller";
            expectedPanel.Description = "Test description";
            templates.PanelTemplates.Add(expectedPanel);

            EstimatingLibraryDatabase.Update(path, testStack);
            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECPanel actualpanel = null;
            foreach (TECPanel panel in actualTemplates.PanelTemplates)
            {
                if (panel.Guid == expectedPanel.Guid)
                {
                    actualpanel = panel;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedPanel.Name, actualpanel.Name);
            Assert.AreEqual(expectedPanel.Description, actualpanel.Description);
        }

        [TestMethod]
        public void Save_Templates_Remove_Panel()
        {
            //Act
            int oldNumPanels = templates.PanelTemplates.Count;
            TECPanel panelToRemove = templates.PanelTemplates[0];

            templates.PanelTemplates.Remove(panelToRemove);

            EstimatingLibraryDatabase.Update(path, testStack);
            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            //Assert
            foreach (TECPanel panel in actualTemplates.PanelTemplates)
            {
                if (panel.Guid == panelToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual((oldNumPanels - 1), actualTemplates.PanelTemplates.Count);

        }

        [TestMethod]
        public void Save_Templates_Panel_Name()
        {
            //Act
            TECPanel expectedPanel = templates.PanelTemplates[0];
            expectedPanel.Name = "Test save panel name";
            EstimatingLibraryDatabase.Update(path, testStack);
            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECPanel actualPanel = null;
            foreach (TECPanel panel in actualTemplates.PanelTemplates)
            {
                if (panel.Guid == expectedPanel.Guid)
                {
                    actualPanel = panel;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedPanel.Name, actualPanel.Name);
        }
        #endregion

        #region Save ConntrolledScope
        [TestMethod]
        public void Save_Templates_Add_ControlledScope()
        {
            //Act
            TECSystem expectedScope = new TECSystem();
            expectedScope.Name = "New controlled scope";
            expectedScope.Description = "New controlled scope desc";
            templates.SystemTemplates.Add(expectedScope);

            var scopeEquipment = new TECEquipment();
            scopeEquipment.Name = "Test Scope System";
            scopeEquipment.Description = "Test scope system description";
            scopeEquipment.SubScope.Add(new TECSubScope());

            expectedScope.Equipment.Add(scopeEquipment);

            var scopeController = new TECController(templates.Catalogs.Manufacturers[0]);
            scopeController.Name = "Test Scope Controller";
            expectedScope.Controllers.Add(scopeController);
            scopeController.AddSubScope(scopeEquipment.SubScope[0]);

            var scopePanel = new TECPanel(templates.Catalogs.PanelTypes[0]);
            scopePanel.Name = "Test Scope Name";
            expectedScope.Panels.Add(scopePanel);

            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            TECSystem actualScope = null;
            foreach (TECSystem scope in actualTemplates.SystemTemplates)
            {
                if (expectedScope.Guid == scope.Guid)
                {
                    actualScope = scope;
                    break;
                }
            }

            TECSubScopeConnection actualSSConnection = null;
            foreach (TECSubScopeConnection ssConnect in actualScope.Controllers[0].ChildrenConnections)
            {
                if (ssConnect.Guid == scopeController.ChildrenConnections[0].Guid)
                {
                    actualSSConnection = ssConnect;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedScope.Name, actualScope.Name);
            Assert.AreEqual(expectedScope.Description, actualScope.Description);
            Assert.AreEqual(expectedScope.Equipment.Count, actualScope.Equipment.Count);
            Assert.AreEqual(expectedScope.Controllers.Count, actualScope.Controllers.Count);
            Assert.AreEqual(expectedScope.Controllers[0].ChildrenConnections.Count, actualScope.Controllers[0].ChildrenConnections.Count);
            Assert.AreEqual(expectedScope.Panels.Count, actualScope.Panels.Count);
            Assert.IsTrue(actualSSConnection.SubScope == actualScope.Equipment[0].SubScope[0]);
        }

        [TestMethod]
        public void Save_Templates_Remove_ControlledScope()
        {
            //Act
            int oldNumScope = templates.SystemTemplates.Count;
            TECSystem scopeToRemove = templates.SystemTemplates[0];

            templates.SystemTemplates.Remove(scopeToRemove);

            EstimatingLibraryDatabase.Update(path, testStack);

            TECTemplates expectedTemplates = EstimatingLibraryDatabase.Load(path) as TECTemplates;

            //Assert
            foreach (TECSystem scope in templates.SystemTemplates)
            {
                if (scope.Guid == scopeToRemove.Guid)
                {
                    Assert.Fail();
                }
            }

            Assert.AreEqual((oldNumScope - 1), templates.SystemTemplates.Count);
        }

        #endregion
    }
}