using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibraryTests
{
    [TestClass]
    public class TemplatesLinkingTests
    {
        [TestMethod]
        public void LinkNetworkConnections()
        {
            //Arrange
            Guid childGuid = Guid.NewGuid();
            Guid subScopeGuid = Guid.NewGuid();

            TECTemplates templates = new TECTemplates();

            TECManufacturer man = new TECManufacturer();
            templates.Catalogs.Manufacturers.Add(man);

            TECControllerType type = new TECControllerType(man);
            type.IO.Add(new TECIO(IOType.BACnetIP));
            templates.Catalogs.ControllerTypes.Add(type);

            //Containing System
            TECSystem sys = new TECSystem(false);
            templates.SystemTemplates.Add(sys);

            //Parent Controller
            TECController parentController = new TECController(type, false);
            sys.AddController(parentController);

            TECNetworkConnection netConnect = parentController.AddNetworkConnection(false, new List<TECConnectionType>(), IOType.BACnetIP);

            //Daisy Controller
            TECController fakeChildController = new TECController(childGuid, type, false);
            netConnect.AddINetworkConnectable(fakeChildController);

            TECController realChildController = new TECController(childGuid, type, false);
            sys.AddController(realChildController);

            //Daisy SubScope
            TECEquipment equip = new TECEquipment(false);
            sys.Equipment.Add(equip);

            TECPoint fakePoint = new TECPoint(false);
            fakePoint.Type = IOType.BACnetIP;
            fakePoint.Quantity = 1;

            TECSubScope fakeSS = new TECSubScope(subScopeGuid, false);
            fakeSS.AddPoint(fakePoint);

            TECPoint realPoint = new TECPoint(false);
            realPoint.Type = IOType.BACnetIP;
            realPoint.Quantity = 1;

            TECSubScope realSS = new TECSubScope(subScopeGuid, false);
            realSS.AddPoint(realPoint);

            netConnect.AddINetworkConnectable(fakeSS);

            equip.SubScope.Add(realSS);

            //Act
            ModelLinkingHelper.LinkTemplates(templates, new Dictionary<Guid, List<Guid>>());

            //Assert
            Assert.IsTrue(netConnect.Children.Contains(realChildController), "Controller wasn't linked to parent network connection properly.");
            Assert.IsTrue(netConnect.Children.Contains(realSS), "SubScope wasn't linked to parent network connection properly.");
        }
    }
}
