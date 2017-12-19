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
        //[TestMethod]
        //public void ReferenceEquipmentInSystem()
        //{
        //    //Arrange
        //    TECTemplates templates = new TECTemplates();

        //    Guid equipGuid = Guid.NewGuid();
        //    TECEquipment refEquip = new TECEquipment(equipGuid, false);

        //    templates.EquipmentTemplates.Add(refEquip);

        //    TECSystem system = new TECSystem(false);
        //    TECEquipment childEquip = new TECEquipment(equipGuid, false);

        //    system.Equipment.Add(childEquip);

        //    templates.SystemTemplates.Add(system);

        //    //Act
        //    //ModelLinkingHelper.LinkTemplates(templates);

        //    //Assert
        //    Assert.AreEqual(refEquip, system.Equipment[0]);

        //    throw new NotImplementedException();
        //}

        //[TestMethod]
        //public void ReferenceSubScopeInReferenceEquipment()
        //{
        //    //Arrange
        //    TECTemplates templates = new TECTemplates();

        //    Guid ssGuid = Guid.NewGuid();
        //    TECSubScope refSS = new TECSubScope(ssGuid, false);

        //    templates.SubScopeTemplates.Add(refSS);

        //    TECEquipment refEquip = new TECEquipment(false);
        //    TECSubScope childSS = new TECSubScope(ssGuid, false);

        //    refEquip.SubScope.Add(childSS);

        //    templates.EquipmentTemplates.Add(refEquip);

        //    //Act
        //    //ModelLinkingHelper.LinkTemplates(templates);

        //    //Assert
        //    Assert.AreEqual(refSS, refEquip.SubScope[0]);

        //    throw new NotImplementedException();
        //}

        //[TestMethod]
        //public void ReferenceSubScopeInInstanceEquipment()
        //{
        //    //Arrange
        //    TECTemplates templates = new TECTemplates();

        //    Guid ssGuid = Guid.NewGuid();

        //    TECSubScope refSubScope = new TECSubScope(ssGuid, false);

        //    templates.SubScopeTemplates.Add(refSubScope);

        //    TECSystem system = new TECSystem(false);
        //    TECEquipment equip = new TECEquipment(false);
        //    TECSubScope ss = new TECSubScope(ssGuid, false);
        //    system.Equipment.Add(equip);
        //    equip.SubScope.Add(ss);

        //    templates.SystemTemplates.Add(system);

        //    //Act
        //    //ModelLinkingHelper.LinkTemplates(templates);

        //    //Assert
        //    Assert.AreEqual(refSubScope, equip.SubScope[0]);

        //    throw new NotImplementedException();
        //}
    }
}
