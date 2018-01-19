using EstimatingLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TECUserControlLibrary.Interfaces;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibraryTests
{
    [TestClass]
    public class DeleteDeviceVMTests
    {
        [TestMethod]
        public void DeleteDeviceUserInputYes()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();

            TECManufacturer man = new TECManufacturer();
            templates.Catalogs.Manufacturers.Add(man);

            TECDevice dev = new TECDevice(new List<TECConnectionType>(), man);
            templates.Catalogs.Devices.Add(dev);

            TECSystem sys = new TECSystem(false);
            templates.SystemTemplates.Add(sys);
            TECEquipment sysEquip = new TECEquipment(false);
            sys.Equipment.Add(sysEquip);
            TECSubScope sysSS = new TECSubScope(false);
            sysEquip.SubScope.Add(sysSS);
            sysSS.Devices.Add(dev);

            TECEquipment equip = new TECEquipment(false);
            templates.EquipmentTemplates.Add(equip);
            TECSubScope equipSS = new TECSubScope(false);
            equip.SubScope.Add(equipSS);
            equipSS.Devices.Add(dev);

            TECSubScope ss = new TECSubScope(false);
            templates.SubScopeTemplates.Add(ss);
            ss.Devices.Add(dev);
            
            //Act
            Mock<IUserConfirmable> mockMessageBox = new Mock<IUserConfirmable>();
            mockMessageBox
                .Setup(x => x.Show(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>(), It.IsAny<MessageBoxImage>()))
                .Returns(MessageBoxResult.Yes);

            DeleteDeviceVM vm = new DeleteDeviceVM(dev, templates);
            vm.messageBox = mockMessageBox.Object;
            vm.DeleteCommand.Execute(null);

            //Assert
            Assert.IsFalse(templates.Catalogs.Devices.Contains(dev), "Device not removed from device templates properly.");
            Assert.IsFalse(sysSS.Devices.Contains(dev), "Device not removed from system template properly.");
            Assert.IsFalse(equipSS.Devices.Contains(dev), "Device not removed from equipment template properly.");
            Assert.IsFalse(ss.Devices.Contains(dev), "Device not removed from subscope template properly.");
        }

        [TestMethod]
        public void DeleteDeviceUserInputNo()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();

            TECManufacturer man = new TECManufacturer();
            templates.Catalogs.Manufacturers.Add(man);

            TECDevice dev = new TECDevice(new List<TECConnectionType>(), man);
            templates.Catalogs.Devices.Add(dev);

            TECSystem sys = new TECSystem(false);
            templates.SystemTemplates.Add(sys);
            TECEquipment sysEquip = new TECEquipment(false);
            sys.Equipment.Add(sysEquip);
            TECSubScope sysSS = new TECSubScope(false);
            sysEquip.SubScope.Add(sysSS);
            sysSS.Devices.Add(dev);

            TECEquipment equip = new TECEquipment(false);
            templates.EquipmentTemplates.Add(equip);
            TECSubScope equipSS = new TECSubScope(false);
            equip.SubScope.Add(equipSS);
            equipSS.Devices.Add(dev);

            TECSubScope ss = new TECSubScope(false);
            templates.SubScopeTemplates.Add(ss);
            ss.Devices.Add(dev);
            
            //Act
            Mock<IUserConfirmable> mockMessageBox = new Mock<IUserConfirmable>();
            mockMessageBox
                .Setup(x => x.Show(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>(), It.IsAny<MessageBoxImage>()))
                .Returns(MessageBoxResult.No);

            DeleteDeviceVM vm = new DeleteDeviceVM(dev, templates);
            vm.messageBox = mockMessageBox.Object;
            vm.DeleteCommand.Execute(null);

            //Assert
            Assert.IsTrue(templates.Catalogs.Devices.Contains(dev), "Device not removed from device templates properly.");
            Assert.IsTrue(sysSS.Devices.Contains(dev), "Device not removed from system template properly.");
            Assert.IsTrue(equipSS.Devices.Contains(dev), "Device not removed from equipment template properly.");
            Assert.IsTrue(ss.Devices.Contains(dev), "Device not removed from subscope template properly.");
        }

        [TestMethod]
        public void PopulateReplacementsWithNetworkConnection()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();

            TECConnectionType type = new TECConnectionType();
            templates.Catalogs.ConnectionTypes.Add(type);
            
            TECManufacturer man = new TECManufacturer();
            templates.Catalogs.Manufacturers.Add(man);

            TECIO controllerIO = new TECIO(IOType.BACnetIP);
            controllerIO.Quantity = 5;

            TECControllerType controllerType = new TECControllerType(man);
            controllerType.IO.Add(controllerIO);
            templates.Catalogs.ControllerTypes.Add(controllerType);

            List<TECConnectionType> deviceTypes = new List<TECConnectionType>();
            deviceTypes.Add(type);
            TECDevice dev = new TECDevice(deviceTypes, man);
            templates.Catalogs.Devices.Add(dev);

            TECSystem sys = new TECSystem(false);
            templates.SystemTemplates.Add(sys);

            TECEquipment sysEquip = new TECEquipment(false);
            sys.Equipment.Add(sysEquip);

            TECSubScope equipSS = new TECSubScope(false);
            sysEquip.SubScope.Add(equipSS);

            equipSS.Devices.Add(dev);

            //Device with no connection types


            //Device with wrong connection types
            //Device with right connection types

            throw new NotImplementedException();
        }

        [TestMethod]
        public void PopulateReplacementsWithoutNetworkConnection()
        {
            //Arrange

            throw new NotImplementedException();
        }

        [TestMethod]
        public void DeleteAndReplaceDevice()
        {
            throw new NotImplementedException();
        }
    }
}
