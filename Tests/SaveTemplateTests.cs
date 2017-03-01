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
        static bool DEBUG = false;

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

            if (DEBUG)
            {
                Console.WriteLine("SaveTemplates test templates: " + path);
            }
            else
            {
                File.Delete(path);
            }


        }

        #region Save System

        [TestMethod]
        public void Save_Templates_Add_System()
        {
            //Act
            TECSystem expectedSystem = new TECSystem();
            expectedSystem.Name = "New system";
            expectedSystem.Description = "New system desc";
            expectedSystem.BudgetPrice = 123.5;
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

            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

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
            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

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
            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

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
            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

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
            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

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
            TECEquipment expectedEquipment = new TECEquipment();
            expectedEquipment.Name = "New Equipment";
            expectedEquipment.Description = "New Equipment desc";
            expectedEquipment.BudgetPrice = 123.5;
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

            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

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
            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

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
            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

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
            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

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
            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

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
            TECSubScope expectedSubScope = new TECSubScope();
            expectedSubScope.Name = "New SubScope";
            expectedSubScope.Description = "New SubScope desc";
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

            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

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
            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

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
            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

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
            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

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
            TECDevice expectedDevice = new TECDevice(Guid.NewGuid());
            expectedDevice.Name = "New Device";
            expectedDevice.Description = "New Device desc";
            expectedDevice.Cost = 11.54;

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
            Assert.AreEqual(expectedDevice.ConnectionType.Name, actualDevice.ConnectionType.Name);
            Assert.AreEqual(expectedDevice.Quantity, actualDevice.Quantity);
        }

        [TestMethod]
        public void Save_Templates_Remove_Device()
        {
            //Act
            int oldNumDevices = templates.DeviceCatalog.Count;
            TECDevice deviceToRemove = templates.DeviceCatalog[0];

            templates.DeviceCatalog.Remove(deviceToRemove);

            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

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
            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

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
            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

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
            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

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
            var testConnectionType = new TECConnectionType();
            testConnectionType.Name = "WireTHHN12";
            templates.ConnectionTypeCatalog.Add(testConnectionType);
            expectedDevice.ConnectionType = testConnectionType;
            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

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
            Assert.AreEqual(expectedDevice.ConnectionType.Guid, actualDevice.ConnectionType.Guid);
        }

        [TestMethod]
        public void Save_Templates_Device_Manufacturer()
        {
            //Act
            TECDevice expectedDevice = templates.DeviceCatalog[0];
            TECManufacturer manToAdd = new TECManufacturer();
            manToAdd.Name = "Test";
            manToAdd.Multiplier = 1;
            templates.ManufacturerCatalog.Add(manToAdd);
            expectedDevice.Manufacturer = manToAdd;
            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

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
            Assert.AreEqual(expectedDevice.Manufacturer.Guid, actualDevice.Manufacturer.Guid);
        }
        #endregion Save Device

        #region Save Controller
        [TestMethod]
        public void Save_Templates_Add_Controller()
        {
            //Act
            TECController expectedController = new TECController(Guid.NewGuid());
            expectedController.Name = "Test Controller";
            expectedController.Description = "Test description";
            expectedController.Cost = 100;

            templates.ControllerTemplates.Add(expectedController);

            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

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

            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

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
            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

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
            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

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
            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

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
            templates.ManufacturerCatalog.Add(testManufacturer);
            expectedController.Manufacturer = testManufacturer;
            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

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
            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);
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

            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

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

            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

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
            int oldNumManufacturers = templates.ManufacturerCatalog.Count;
            TECManufacturer expectedManufacturer = new TECManufacturer();
            expectedManufacturer.Name = "Test Add Manufacturer";
            expectedManufacturer.Multiplier = 21.34;

            templates.ManufacturerCatalog.Add(expectedManufacturer);

            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

            TECManufacturer actualManufacturer = null;
            foreach (TECManufacturer man in actualTemplates.ManufacturerCatalog)
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
            Assert.AreEqual((oldNumManufacturers + 1), actualTemplates.ManufacturerCatalog.Count);

        }

        [TestMethod]
        public void Save_Templates_Remove_Manufacturer()
        {
            //Act
            int oldNumManufacturers = templates.ManufacturerCatalog.Count;
            TECManufacturer manToRemove = templates.ManufacturerCatalog[0];

            templates.ManufacturerCatalog.Remove(manToRemove);

            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

            //Assert
            foreach (TECManufacturer man in actualTemplates.ManufacturerCatalog)
            {
                if (man.Guid == manToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual((oldNumManufacturers - 1), actualTemplates.ManufacturerCatalog.Count);
        }

        [TestMethod]
        public void Save_Templates_Manufacturer_Name()
        {
            //Act
            TECManufacturer expectedManufacturer = templates.ManufacturerCatalog[0];
            expectedManufacturer.Name = "Test save manufacturer name";
            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

            TECManufacturer actualMan = null;
            foreach (TECManufacturer man in actualTemplates.ManufacturerCatalog)
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
            TECManufacturer expectedManufacturer = templates.ManufacturerCatalog[0];
            expectedManufacturer.Multiplier = 987.41;
            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

            TECManufacturer actualMan = null;
            foreach (TECManufacturer man in actualTemplates.ManufacturerCatalog)
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
            int oldNumTags = templates.Tags.Count;
            TECTag expectedTag = new TECTag();
            expectedTag.Text = "Test add tag";

            templates.Tags.Add(expectedTag);

            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

            TECTag actualTag = null;
            foreach (TECTag tag in actualTemplates.Tags)
            {
                if (tag.Guid == expectedTag.Guid)
                {
                    actualTag = tag;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedTag.Text, actualTag.Text);
            Assert.AreEqual((oldNumTags + 1), actualTemplates.Tags.Count);
        }

        [TestMethod]
        public void Save_Templates_Remove_Tag()
        {
            //Act
            int oldNumTags = templates.Tags.Count;
            TECTag tagToRemove = templates.Tags[0];

            templates.Tags.Remove(tagToRemove);

            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);
            
            //Assert
            foreach (TECTag tag in actualTemplates.Tags)
            {
                if (tag.Guid == tagToRemove.Guid) { Assert.Fail(); }
            }

            Assert.AreEqual((oldNumTags - 1), actualTemplates.Tags.Count);
        }
        #endregion Save Tag

        #region Save Connection Type
        [TestMethod]
        public void Save_Templates_Add_ConnectionType()
        {
            //Act
            int oldNumConnectionTypes = templates.ConnectionTypeCatalog.Count;
            TECConnectionType expectedConnectionType = new TECConnectionType();
            expectedConnectionType.Name = "Test Add Connection Type";
            expectedConnectionType.Cost = 21.34;

            templates.ConnectionTypeCatalog.Add(expectedConnectionType);

            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

            TECConnectionType actualConnectionType = null;
            foreach (TECConnectionType connectionType in actualTemplates.ConnectionTypeCatalog)
            {
                if (connectionType.Guid == expectedConnectionType.Guid)
                {
                    actualConnectionType = connectionType;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedConnectionType.Name, actualConnectionType.Name);
            Assert.AreEqual(expectedConnectionType.Cost, actualConnectionType.Cost);
            Assert.AreEqual((oldNumConnectionTypes + 1), actualTemplates.ConnectionTypeCatalog.Count);

        }

        [TestMethod]
        public void Save_Templates_Remove_ConnectionType()
        {
            //Act
            int oldNumConnectionTypes = templates.ConnectionTypeCatalog.Count;
            TECConnectionType connectionTypeToRemove = templates.ConnectionTypeCatalog[0];

            templates.ConnectionTypeCatalog.Remove(connectionTypeToRemove);

            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

            //Assert
            foreach (TECConnectionType connectionType in actualTemplates.ConnectionTypeCatalog)
            {
                if (connectionType.Guid == connectionTypeToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual((oldNumConnectionTypes - 1), actualTemplates.ConnectionTypeCatalog.Count);
        }

        #endregion

        #region Save Conduit Type
        [TestMethod]
        public void Save_Templates_Add_ConduitType()
        {
            //Act
            int oldNumConduitTypes = templates.ConduitTypeCatalog.Count;
            TECConduitType expectedConduitType = new TECConduitType();
            expectedConduitType.Name = "Test Add Conduit Type";
            expectedConduitType.Cost = 21.34;

            templates.ConduitTypeCatalog.Add(expectedConduitType);

            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

            TECConduitType actualConnectionType = null;
            foreach (TECConduitType conduitType in actualTemplates.ConduitTypeCatalog)
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
            Assert.AreEqual((oldNumConduitTypes + 1), actualTemplates.ConduitTypeCatalog.Count);

        }

        [TestMethod]
        public void Save_Templates_Remove_ConduitType()
        {
            //Act
            int oldNumConduitTypes = templates.ConduitTypeCatalog.Count;
            TECConduitType conduitTypeToRemove = templates.ConduitTypeCatalog[0];

            templates.ConduitTypeCatalog.Remove(conduitTypeToRemove);

            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

            //Assert
            foreach (TECConduitType conduitType in actualTemplates.ConduitTypeCatalog)
            {
                if (conduitType.Guid == conduitTypeToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual((oldNumConduitTypes - 1), actualTemplates.ConduitTypeCatalog.Count);
        }
        #endregion

        #region Save Associated Costs
        [TestMethod]
        public void Save_Templates_Add_AssociatedCost()
        {
            //Act
            int oldNumAssociatedCosts = templates.AssociatedCostsCatalog.Count;
            TECAssociatedCost expectedAssociatedCost = new TECAssociatedCost();
            expectedAssociatedCost.Name = "Test Associated Cost";
            expectedAssociatedCost.Cost = 21.34;

            templates.AssociatedCostsCatalog.Add(expectedAssociatedCost);

            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

            TECAssociatedCost actualCost = null;
            foreach (TECAssociatedCost cost in actualTemplates.AssociatedCostsCatalog)
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
            Assert.AreEqual((oldNumAssociatedCosts + 1), actualTemplates.AssociatedCostsCatalog.Count);

        }

        [TestMethod]
        public void Save_Templates_Remove_AssociatedCost()
        {
            //Act
            int oldNumAssociatedCosts = templates.AssociatedCostsCatalog.Count;
            TECAssociatedCost costToRemove = templates.AssociatedCostsCatalog[0];

            templates.AssociatedCostsCatalog.Remove(costToRemove);

            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

            //Assert
            foreach (TECAssociatedCost cost in actualTemplates.AssociatedCostsCatalog)
            {
                if (cost.Guid == costToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual((oldNumAssociatedCosts - 1), actualTemplates.AssociatedCostsCatalog.Count);
        }
        #endregion
    }
}