using EstimatingLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            throw new NotImplementedException();
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
