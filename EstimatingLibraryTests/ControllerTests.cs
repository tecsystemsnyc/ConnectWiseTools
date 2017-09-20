using EstimatingLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibraryTests
{
    [TestClass]
    public class ControllerTests
    {
        [TestMethod]
        public void Controller_AddSubScope()
        {
            TECController controller = new TECController(new TECControllerType(new TECManufacturer()), false);
            TECSubScope subScope = new TECSubScope(false);

            controller.AddSubScope(subScope);

            Assert.AreEqual(1, controller.ChildrenConnections.Count, "Connection not added to controller");
            Assert.AreNotEqual(null, subScope.Connection, "Connection not added to subscope");
        }

        [TestMethod]
        public void Controller_RemoveSubScope()
        {
            TECController controller = new TECController(new TECControllerType(new TECManufacturer()), false);
            TECSubScope subScope = new TECSubScope(false);

            controller.AddSubScope(subScope);
            controller.RemoveSubScope(subScope);

            Assert.AreEqual(0, controller.ChildrenConnections.Count, "Connection not removed from controller");
            Assert.AreEqual(null, subScope.Connection, "Connection not removed from subscope");
        }
    }
}
