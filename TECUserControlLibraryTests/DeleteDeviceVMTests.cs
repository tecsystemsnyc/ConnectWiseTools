using EstimatingLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibraryTests
{
    [TestClass]
    public class DeleteDeviceVMTests
    {
        [TestMethod]
        public void DeleteDevice()
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
            DeleteDeviceVM vm = new DeleteDeviceVM(dev, templates);
            vm.DeleteCommand.Execute(null);

            //Assert
            Assert.IsFalse(templates.Catalogs.Devices.Contains(dev), "Device not removed from device templates properly.");
            Assert.IsFalse(sysSS.Devices.Contains(dev), "Device not removed from system template properly.");
            Assert.IsFalse(equipSS.Devices.Contains(dev), "Device not removed from equipment template properly.");
            Assert.IsFalse(ss.Devices.Contains(dev), "Device not removed from subscope template properly.");
        }

        [TestMethod]
        public void PopulateReplacements()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void DeleteAndReplaceDevice()
        {
            throw new NotImplementedException();
        }
    }
}
