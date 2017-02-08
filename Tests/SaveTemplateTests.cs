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
            expectedDevice.ConnectionType = ConnectionType.WireTHHN12;
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
            Assert.AreEqual(expectedDevice.ConnectionType, actualDevice.ConnectionType);
        }

        [TestMethod]
        public void Save_Templates_Device_Manufacturer()
        {
            //Act
            TECDevice expectedDevice = templates.DeviceCatalog[0];
            TECManufacturer manToAdd = new TECManufacturer("Test", 1.0);
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
            TECController expectedController = new TECController("Test Controller", "Test description", Guid.NewGuid(), 100);

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

        #region Controller IO
        [TestMethod]
        public void Save_Templates_Controller_Add_IO()
        {
            //Act
            TECController expectedController = templates.ControllerTemplates[0];
            expectedController.IO.Add(new TECIO(IOType.BACnetIP));
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


    }
}