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
    public class TemplatesSyncronizerTests
    {
        #region Base Class Tests
        private TECSubScope copySubScope(TECSubScope template)
        {
            return new TECSubScope(template, false);
        }
        private void syncSubScope(TECSubScope templateSS, TECSubScope toSync)
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

            syncronizer.NewGroup(templateSS);

            List<TECSubScope> newReferenceSS = new List<TECSubScope>();

            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        [TestMethod]
        public void TemplateSubScopeRemoved()
        {
            throw new NotImplementedException();
        }
        
        [TestMethod]
        public void ReferenceSubScopeRemoved()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void TemplateEquipmentRemoved()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void ReferenceEquipmentRemoved()
        {
            throw new NotImplementedException();
        }
        #endregion

        //////ADDDD SAVE LOAD TESTS
    }
}