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
    public class TemplatesSynchronizerTests
    {
        #region Base Class Tests
        private TECSubScope copySubScope(TECSubScope template)
        {
            return new TECSubScope(template, false);
        }
        private void syncSubScope(TECSubScope templateSS, TECSubScope toSync, TECChangedEventArgs args)
        {
            toSync.CopyPropertiesFromScope(templateSS);
        }

        [TestMethod]
        public void NewItemTest()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();

            TemplateSynchronizer<TECSubScope> syncronizer = new TemplateSynchronizer<TECSubScope>(copySubScope, syncSubScope, templates);

            TECSubScope templateSS = new TECSubScope(false);
            templateSS.Name = "Template SubScope";
            
            syncronizer.NewGroup(templateSS);

            //Act
            TECSubScope copySS = syncronizer.NewItem(templateSS);

            //Assert
            Assert.AreEqual(templateSS.Name, copySS.Name);
            Assert.AreNotEqual(templateSS.Guid, copySS.Guid);
        }

        [TestMethod]
        public void NewItemNoGroupTest()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();

            TemplateSynchronizer<TECSubScope> syncronizer = new TemplateSynchronizer<TECSubScope>(copySubScope, syncSubScope, templates);

            TECSubScope templateSS = new TECSubScope(false);
            templateSS.Name = "Template SubScope";

            //Act
            TECSubScope copySS = syncronizer.NewItem(templateSS);

            //Assert
            Assert.AreEqual(templateSS.Name, copySS.Name);
            Assert.AreNotEqual(templateSS.Guid, copySS.Guid);
        }

        [TestMethod] 
        public void ChangeTemplateTest()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();

            TemplateSynchronizer<TECSubScope> synchronizer = new TemplateSynchronizer<TECSubScope>(copySubScope, syncSubScope, templates);

            TECSubScope templateSS = new TECSubScope(false);
            templateSS.Name = "Template SubScope";

            TECSubScope copySS = synchronizer.NewItem(templateSS);

            //Act
            templateSS.Description = "Test Description";

            //Assert
            Assert.AreEqual(templateSS.Description, copySS.Description);
        }

        [TestMethod]
        public void ChangeInstanceTest()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();

            TemplateSynchronizer<TECSubScope> synchronizer = new TemplateSynchronizer<TECSubScope>(copySubScope, syncSubScope, templates);

            TECSubScope templateSS = new TECSubScope(false);
            templateSS.Name = "Template SubScope";

            TECSubScope copySS = synchronizer.NewItem(templateSS);

            //Act
            copySS.Description = "Test Description";

            //Assert
            Assert.AreEqual(templateSS.Description, copySS.Description);
        }

        [TestMethod]
        public void LinkExistingTest()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();

            TemplateSynchronizer<TECSubScope> syncronizer = new TemplateSynchronizer<TECSubScope>(copySubScope, syncSubScope, templates);

            TECSubScope templateSS = new TECSubScope(false);
            templateSS.Name = "Template SubScope";

            List<TECSubScope> newReferenceSS = new List<TECSubScope>();
            newReferenceSS.Add(new TECSubScope(false));
            newReferenceSS.Add(new TECSubScope(false));

            //Act
            syncronizer.LinkExisting(templateSS, newReferenceSS);
            templateSS.Description = "Test Description";

            //Assert
            foreach(TECSubScope refSS in newReferenceSS)
            {
                Assert.AreEqual(templateSS.Description, refSS.Description);
            }
        }
        
        #endregion

        #region TECTemplates Integration Tests
        [TestMethod]
        public void SubScopeTemplateChanged()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();

            TECSubScope templateSS = new TECSubScope(false);
            templateSS.Name = "Template SubScope";
            templates.SubScopeTemplates.Add(templateSS);

            TemplateSynchronizer<TECSubScope> ssSynchronizer = templates.SubScopeSynchronizer;
            TECSubScope refSS = ssSynchronizer.NewItem(templateSS);

            TECEquipment equip = new TECEquipment(false);
            templates.EquipmentTemplates.Add(equip);
            equip.SubScope.Add(refSS);

            TECDevice dev = new TECDevice(new List<TECConnectionType>(), new TECManufacturer());
            templates.Catalogs.Devices.Add(dev);

            TECPoint point = new TECPoint(false);
            point.Label = "Test Point";
            point.Type = IOType.AI;
            point.Quantity = 5;

            TECCost cost = new TECCost(CostType.TEC);
            templates.Catalogs.AssociatedCosts.Add(cost);

            TECLabeled tag = new TECLabeled();
            templates.Catalogs.Tags.Add(tag);

            //Act
            templateSS.Description = "Test Description";
            templateSS.Devices.Add(dev);
            templateSS.AddPoint(point);
            templateSS.AssociatedCosts.Add(cost);
            templateSS.Tags.Add(tag);

            //Assert
            Assert.AreEqual(templateSS.Description, refSS.Description, "Description didn't sync properly between SubScope.");
            Assert.AreEqual(templateSS.Devices[0], refSS.Devices[0], "Devices didn't sync properly between SubScope.");
            Assert.AreEqual(templateSS.Points[0].Label, refSS.Points[0].Label, "Points didn't sync properly between SubScope.");
            Assert.AreEqual(templateSS.Points[0].Type, refSS.Points[0].Type, "Points didn't sync properly between SubScope.");
            Assert.AreEqual(templateSS.Points[0].Quantity, refSS.Points[0].Quantity, "Points didn't sync properly between SubScope.");
            Assert.AreEqual(templateSS.AssociatedCosts[0], refSS.AssociatedCosts[0], "Associated costs didn't sync properly between SubScope.");
            Assert.AreEqual(templateSS.Tags[0], refSS.Tags[0], "Tags didn't sync properly between SubScope.");
        }

        [TestMethod]
        public void SubScopeReferenceChanged()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();

            TECSubScope templateSS = new TECSubScope(false);
            templateSS.Name = "Template SubScope";
            templates.SubScopeTemplates.Add(templateSS);

            TemplateSynchronizer<TECSubScope> ssSynchronizer = templates.SubScopeSynchronizer;
            TECSubScope refSS = ssSynchronizer.NewItem(templateSS);

            TECEquipment equip = new TECEquipment(false);
            templates.EquipmentTemplates.Add(equip);
            equip.SubScope.Add(refSS);

            TECDevice dev = new TECDevice(new List<TECConnectionType>(), new TECManufacturer());
            templates.Catalogs.Devices.Add(dev);

            TECPoint point = new TECPoint(false);
            point.Label = "Test Point";
            point.Type = IOType.AI;
            point.Quantity = 5;

            TECCost cost = new TECCost(CostType.TEC);
            templates.Catalogs.AssociatedCosts.Add(cost);

            TECLabeled tag = new TECLabeled();
            templates.Catalogs.Tags.Add(tag);

            //Act
            refSS.Description = "Test Description";
            refSS.Devices.Add(dev);
            refSS.AddPoint(point);
            refSS.AssociatedCosts.Add(cost);
            refSS.Tags.Add(tag);

            //Assert
            Assert.AreEqual(refSS.Devices[0], templateSS.Devices[0], "Devices didn't sync properly between SubScope.");
            Assert.AreEqual(refSS.Points[0].Label, templateSS.Points[0].Label, "Points didn't sync properly between SubScope.");
            Assert.AreEqual(refSS.Points[0].Type, templateSS.Points[0].Type, "Points didn't sync properly between SubScope.");
            Assert.AreEqual(refSS.Points[0].Quantity, templateSS.Points[0].Quantity, "Points didn't sync properly between SubScope.");
            Assert.AreEqual(refSS.AssociatedCosts[0], templateSS.AssociatedCosts[0], "Associated costs didn't sync properly between SubScope.");
            Assert.AreEqual(refSS.Tags[0], templateSS.Tags[0], "Tags didn't sync properly between SubScope.");
        }

        [TestMethod]
        public void EquipmentTemplateChanged()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();

            TECEquipment templateEquip = new TECEquipment(false);
            templateEquip.Name = "Template Equip";
            templates.EquipmentTemplates.Add(templateEquip);

            TemplateSynchronizer<TECEquipment> equipSynchronizer = templates.EquipmentSynchronizer;
            TECEquipment refEquip = equipSynchronizer.NewItem(templateEquip);

            TECSystem sys = new TECSystem(false);
            templates.SystemTemplates.Add(sys);
            sys.Equipment.Add(refEquip);

            TECDevice dev = new TECDevice(new List<TECConnectionType>(), new TECManufacturer());
            templates.Catalogs.Devices.Add(dev);

            TECPoint point = new TECPoint(false);
            point.Label = "Test Point";
            point.Type = IOType.AI;
            point.Quantity = 5;

            TECCost cost = new TECCost(CostType.TEC);
            templates.Catalogs.AssociatedCosts.Add(cost);

            TECLabeled tag = new TECLabeled();
            templates.Catalogs.Tags.Add(tag);

            TECSubScope ss = new TECSubScope(false);
            ss.Description = "Test Description";
            ss.Devices.Add(dev);
            ss.AddPoint(point);
            ss.AssociatedCosts.Add(cost);
            ss.Tags.Add(tag);

            templates.SubScopeTemplates.Add(ss);

            //Act
            templateEquip.Description = "Test Description";
            templateEquip.SubScope.Add(ss);
            templateEquip.AssociatedCosts.Add(cost);
            templateEquip.Tags.Add(tag);

            //Assert
            Assert.AreEqual(templateEquip.Description, refEquip.Description, "Description didn't sync properly between Equipment.");

            Assert.IsNotNull(refEquip.SubScope[0], "SubScope didn't sync properly between Equipment.");

            TECSubScope templateSubScope = templateEquip.SubScope[0];
            TECSubScope refSubScope = refEquip.SubScope[0];

            Assert.AreEqual(templateSubScope.Description, refSubScope.Description, "Description didn't sync properly between SubScope in Equipment.");
            Assert.AreEqual(templateSubScope.Devices[0], refSubScope.Devices[0], "Devices didn't sync properly between SubScope in Equipment.");
            Assert.AreEqual(templateSubScope.Points[0].Label, refSubScope.Points[0].Label, "Points didn't sync properly between SubScope in Equipment.");
            Assert.AreEqual(templateSubScope.Points[0].Type, refSubScope.Points[0].Type, "Points didn't sync properly between SubScope in Equipment.");
            Assert.AreEqual(templateSubScope.Points[0].Quantity, refSubScope.Points[0].Quantity, "Points didn't sync properly between SubScope in Equipment.");
            Assert.AreEqual(templateSubScope.AssociatedCosts[0], refSubScope.AssociatedCosts[0], "AssociatedCosts didn't sync properly between SubScope in Equipment.");
            Assert.AreEqual(templateSubScope.Tags[0], refSubScope.Tags[0], "Tags didn't sync properly between SubScope.");

            Assert.AreEqual(templateEquip.AssociatedCosts[0], refEquip.AssociatedCosts[0], "AssociatedCosts didn't sync properly between Equipment.");
            Assert.AreEqual(templateEquip.Tags[0], refEquip.Tags[0], "Tags didn't sync properly in Equipment.");
        }

        [TestMethod]
        public void EquipmentReferenceChanged()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();

            TECEquipment templateEquip = new TECEquipment(false);
            templateEquip.Name = "Template Equip";
            templates.EquipmentTemplates.Add(templateEquip);

            TemplateSynchronizer<TECEquipment> equipSynchronizer = templates.EquipmentSynchronizer;
            TECEquipment refEquip = equipSynchronizer.NewItem(templateEquip);

            TECSystem sys = new TECSystem(false);
            templates.SystemTemplates.Add(sys);
            sys.Equipment.Add(refEquip);

            TECDevice dev = new TECDevice(new List<TECConnectionType>(), new TECManufacturer());
            templates.Catalogs.Devices.Add(dev);

            TECPoint point = new TECPoint(false);
            point.Label = "Test Point";
            point.Type = IOType.AI;
            point.Quantity = 5;

            TECCost cost = new TECCost(CostType.TEC);
            templates.Catalogs.AssociatedCosts.Add(cost);

            TECLabeled tag = new TECLabeled();
            templates.Catalogs.Tags.Add(tag);

            TECSubScope ss = new TECSubScope(false);
            ss.Description = "Test Description";
            ss.Devices.Add(dev);
            ss.AddPoint(point);
            ss.AssociatedCosts.Add(cost);
            ss.Tags.Add(tag);

            templates.SubScopeTemplates.Add(ss);

            //Act
            refEquip.Description = "Test Description";
            refEquip.SubScope.Add(ss);
            refEquip.AssociatedCosts.Add(cost);
            refEquip.Tags.Add(tag);

            //Assert
            Assert.AreEqual(refEquip.Description, templateEquip.Description, "Description didn't sync properly between Equipment.");

            Assert.IsNotNull(templateEquip.SubScope[0], "SubScope didn't sync properly between Equipment.");

            TECSubScope templateSubScope = templateEquip.SubScope[0];
            TECSubScope refSubScope = refEquip.SubScope[0];

            Assert.AreEqual(refSubScope.Description, templateSubScope.Description, "Description didn't sync properly between SubScope in Equipment.");
            Assert.AreEqual(refSubScope.Devices[0], templateSubScope.Devices[0], "Devices didn't sync properly between SubScope in Equipment.");
            Assert.AreEqual(refSubScope.Points[0].Label, templateSubScope.Points[0].Label, "Points didn't sync properly between SubScope in Equipment.");
            Assert.AreEqual(refSubScope.Points[0].Type, templateSubScope.Points[0].Type, "Points didn't sync properly between SubScope in Equipment.");
            Assert.AreEqual(refSubScope.Points[0].Quantity, templateSubScope.Points[0].Quantity, "Points didn't sync properly between SubScope in Equipment.");
            Assert.AreEqual(refSubScope.AssociatedCosts[0], templateSubScope.AssociatedCosts[0], "AssociatedCosts didn't sync properly between SubScope in Equipment.");
            Assert.AreEqual(refSubScope.Tags[0], templateSubScope.Tags[0], "Tags didn't sync properly between SubScope.");

            Assert.AreEqual(refEquip.AssociatedCosts[0], templateEquip.AssociatedCosts[0], "AssociatedCosts didn't sync properly between Equipment.");
            Assert.AreEqual(refEquip.Tags[0], templateEquip.Tags[0], "Tags didn't sync properly in Equipment.");
        }

        [TestMethod]
        public void TemplateSubScopeRemoved()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            TemplateSynchronizer<TECSubScope> synchronizer = templates.SubScopeSynchronizer;

            TECSubScope templateSS = new TECSubScope(false);
            templateSS.Name = "First Name";
            templates.SubScopeTemplates.Add(templateSS);

            TECEquipment templateEquip = new TECEquipment(false);
            templates.EquipmentTemplates.Add(templateEquip);
            TECSubScope ss1 = synchronizer.NewItem(templateSS);
            TECSubScope ss2 = synchronizer.NewItem(templateSS);
            templateEquip.SubScope.Add(ss1);
            templateEquip.SubScope.Add(ss2);

            //Act
            templates.SubScopeTemplates.Remove(templateSS);

            ss2.Name = "Second Name";

            //Assert
            Assert.IsFalse(synchronizer.Contains(templateSS));
            Assert.IsFalse(synchronizer.Contains(ss1));
            Assert.IsFalse(synchronizer.Contains(ss2));
            Assert.AreEqual("First Name", ss1.Name);
            Assert.AreEqual("Second Name", ss2.Name);
        }
        
        [TestMethod]
        public void ReferenceSubScopeRemoved()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            TemplateSynchronizer<TECSubScope> synchronizer = templates.SubScopeSynchronizer;

            TECSubScope templateSS = new TECSubScope(false);
            templates.SubScopeTemplates.Add(templateSS);

            TECEquipment templateEquip = new TECEquipment(false);
            templates.EquipmentTemplates.Add(templateEquip);
            TECSubScope refSS = synchronizer.NewItem(templateSS);
            templateEquip.SubScope.Add(refSS);

            //Act
            templateEquip.SubScope.Remove(refSS);

            //Assert
            Assert.IsFalse(synchronizer.Contains(refSS));
            Assert.IsTrue(synchronizer.Contains(templateSS));
        }

        [TestMethod]
        public void TemplateEquipmentRemoved()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            TemplateSynchronizer<TECEquipment> synchronizer = templates.EquipmentSynchronizer;

            TECEquipment templateEquip = new TECEquipment(false);
            templateEquip.Name = "First Name";
            templates.EquipmentTemplates.Add(templateEquip);

            TECSystem templateSys = new TECSystem(false);
            templates.SystemTemplates.Add(templateSys);
            TECEquipment equip1 = synchronizer.NewItem(templateEquip);
            TECEquipment equip2 = synchronizer.NewItem(templateEquip);
            templateSys.Equipment.Add(equip1);
            templateSys.Equipment.Add(equip2);

            //Act
            templates.EquipmentTemplates.Remove(templateEquip);

            equip2.Name = "Second Name";

            //Assert
            Assert.IsFalse(synchronizer.Contains(templateEquip));
            Assert.IsFalse(synchronizer.Contains(equip1));
            Assert.IsFalse(synchronizer.Contains(equip2));
            Assert.AreEqual("First Name", equip1.Name);
            Assert.AreEqual("Second Name", equip2.Name);
        }

        [TestMethod]
        public void ReferenceEquipmentRemoved()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void SubScopeRemovedFromTemplateEquipment()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void SubScopeRemovedFromReferenceEquipment()
        {
            throw new NotImplementedException();
        }
        #endregion

        //////ADD DD SAVE LOAD TESTS
    }
}