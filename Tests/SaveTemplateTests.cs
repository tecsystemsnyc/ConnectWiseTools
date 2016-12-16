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
        TECTemplates templates;
        ChangeStack testStack;
        string path;

        [TestInitialize]
        public void TestInitialize()
        {
            //Arrange
            templates = TestHelper.CreateTestTemplates();
            testStack = new ChangeStack(templates);
            path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".tdb";
            EstimatingLibraryDatabase.SaveTemplatesToNewDB(path, templates);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //File.Delete(path);
            Console.WriteLine("SaveTemplates test templates: " + path);
        }

        #region Save System

        [TestMethod]
        public void Save_Templates_Add_System()
        {
            //Act
            TECSystem expectedSystem = new TECSystem("New system", "New system desc", 123.5, new ObservableCollection<TECEquipment>());
            expectedSystem.Quantity = 1235;

            templates.SystemTemplates.Add(expectedSystem);

            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

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
            Assert.AreEqual(expectedSystem.BudgetPrice, actualSystem.BudgetPrice);
        }

        [TestMethod]
        public void Save_Templates_Remove_System()
        {
            //Act
            int oldNumSystems = templates.SystemTemplates.Count;
            TECSystem systemToRemove = templates.SystemTemplates[0];

            templates.SystemTemplates.Remove(systemToRemove);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECTemplates expectedTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

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
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

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
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

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
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

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
            expectedSystem.BudgetPrice = 9876543.21;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

            TECSystem actualSystem = null;
            foreach (TECSystem system in actualTemplates.SystemTemplates)
            {
                if (system.Guid == expectedSystem.Guid)
                {
                    actualSystem = system;
                }
            }

            //Assert
            Assert.AreEqual(expectedSystem.BudgetPrice, actualSystem.BudgetPrice);
        }
        #endregion Edit System
        #endregion Save System

        #region Save Equipment
        [TestMethod]
        public void Save_Templates_Add_Equipment()
        {
            //Act
            TECEquipment expectedEquipment = new TECEquipment("New Equipment", "New Equipment desc", 123.5, new ObservableCollection<TECSubScope>());
            expectedEquipment.Quantity = 1235;

            templates.EquipmentTemplates.Add(expectedEquipment);

            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

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
            Assert.AreEqual(expectedEquipment.BudgetPrice, actualEquipment.BudgetPrice);
        }

        [TestMethod]
        public void Save_Templates_Remove_Equipment()
        {
            //Act
            int oldNumEquipments = templates.EquipmentTemplates.Count;
            TECEquipment EquipmentToRemove = templates.EquipmentTemplates[0];

            templates.EquipmentTemplates.Remove(EquipmentToRemove);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECTemplates expectedTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

            //Assert
            foreach (TECEquipment Equipment in templates.EquipmentTemplates)
            {
                if (Equipment.Guid == EquipmentToRemove.Guid)
                {
                    Assert.Fail();
                }
            }

            Assert.AreEqual((oldNumEquipments - 1), templates.EquipmentTemplates.Count);
        }

        [TestMethod]
        public void Save_Templates_Equipment_Name()
        {
            //Act
            TECEquipment expectedEquipment = templates.EquipmentTemplates[0];
            expectedEquipment.Name = "Save Equipment Name";
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

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
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

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
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

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
            expectedEquipment.BudgetPrice = 9876543.21;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

            TECEquipment actualEquipment = null;
            foreach (TECEquipment Equipment in actualTemplates.EquipmentTemplates)
            {
                if (Equipment.Guid == expectedEquipment.Guid)
                {
                    actualEquipment = Equipment;
                }
            }

            //Assert
            Assert.AreEqual(expectedEquipment.BudgetPrice, actualEquipment.BudgetPrice);
        }
        #endregion Save Equipment

        #region Save SubScope
        [TestMethod]
        public void Save_Templates_Add_SubScope()
        {
            //Act
            TECSubScope expectedSubScope = new TECSubScope("New SubScope", "New SubScope desc", new ObservableCollection<TECDevice>(), new ObservableCollection<TECPoint>());
            expectedSubScope.Quantity = 1235;

            templates.SubScopeTemplates.Add(expectedSubScope);

            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

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

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

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
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

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
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

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
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

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
        #endregion Save SubScope

        #region Save Device
        [TestMethod]
        public void Save_Templates_Add_Device()
        {
            //Act
            TECDevice expectedDevice = new TECDevice("New Device", "New Device desc", 11.54, ConnectionType.WireTHHN12, new TECManufacturer(), Guid.NewGuid());

            templates.DeviceCatalog.Add(expectedDevice);

            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

            TECDevice actualDevice = null;
            foreach (TECDevice device in actualTemplates.DeviceCatalog)
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
            Assert.AreEqual(expectedDevice.ConnectionType, actualDevice.ConnectionType);
            Assert.AreEqual(expectedDevice.Quantity, actualDevice.Quantity);
        }

        [TestMethod]
        public void Save_Templates_Remove_Device()
        {
            //Act
            int oldNumDevices = templates.DeviceCatalog.Count;
            TECDevice deviceToRemove = templates.DeviceCatalog[0];

            templates.DeviceCatalog.Remove(deviceToRemove);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

            //Assert
            foreach (TECDevice dev in actualTemplates.DeviceCatalog)
            {
                if (dev.Guid == deviceToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual((oldNumDevices - 1), actualTemplates.SubScopeTemplates.Count);
        }

        [TestMethod]
        public void Save_Templates_Device_Name()
        {
            //Act
            TECDevice expectedDevice = templates.DeviceCatalog[0];
            expectedDevice.Name = "Save Device Name";
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

            TECDevice actualDevice = null;
            foreach (TECDevice Device in actualTemplates.DeviceCatalog)
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
            TECDevice expectedDevice = templates.DeviceCatalog[0];
            expectedDevice.Description = "Save Device Description";
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

            TECDevice actualDevice = null;
            foreach (TECDevice Device in actualTemplates.DeviceCatalog)
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
            TECDevice expectedDevice = templates.DeviceCatalog[0];
            expectedDevice.Cost = 46.89;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

            TECDevice actualDevice = null;
            foreach (TECDevice Device in actualTemplates.DeviceCatalog)
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
            TECDevice expectedDevice = templates.DeviceCatalog[0];
            expectedDevice.ConnectionType = ConnectionType.WireTHHN12;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

            TECDevice actualDevice = null;
            foreach (TECDevice Device in actualTemplates.DeviceCatalog)
            {
                if (Device.Guid == expectedDevice.Guid)
                {
                    actualDevice = Device;
                }
            }

            //Assert
            Assert.AreEqual(expectedDevice.ConnectionType, actualDevice.ConnectionType);
        }
        #endregion Save Device


    }
}