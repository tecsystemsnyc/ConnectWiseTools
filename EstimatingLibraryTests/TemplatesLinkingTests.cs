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
        public void ReferenceEquipmentInSystem()
        {

        }

        public void ReferenceSubScopeInReferenceEquipment()
        {

        }

        [TestMethod]
        public void ReferenceSubScopeInInstanceEquipment()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();

            Guid ssGuid = Guid.NewGuid();

            TECSubScope refSubScope = new TECSubScope(ssGuid, false);

            templates.SubScopeTemplates.Add(refSubScope);

            TECSystem system = new TECSystem(false);
            TECEquipment equip = new TECEquipment(false);
            TECSubScope ss = new TECSubScope(ssGuid, false);
            system.Equipment.Add(equip);
            equip.SubScope.Add(ss);

            templates.SystemTemplates.Add(system);

            //Act
            ModelLinkingHelper.LinkTemplates(templates);

            //Assert
            Assert.AreEqual(equip.SubScope[0], refSubScope);
        }
    }
}
