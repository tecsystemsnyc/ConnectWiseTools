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
            Assert.AreEqual(templateSS.Devices[0], refSS.Devices[0], "Devices didn't sync properly between SubScope.");
            Assert.AreEqual(templateSS.Points[0].Label, refSS.Points[0].Label, "Points didn't sync properly between SubScope.");
            Assert.AreEqual(templateSS.Points[0].Type, refSS.Points[0].Type, "Points didn't sync properly between SubScope.");
            Assert.AreEqual(templateSS.Points[0].Quantity, refSS.Points[0].Quantity, "Points didn't sync properly between SubScope.");
            Assert.AreEqual(templateSS.AssociatedCosts[0], refSS.AssociatedCosts[0], "Associated costs didn't sync properly between SubScope.");
            Assert.AreEqual(templateSS.Tags[0], refSS.Tags[0], "Tags didn't sync properly between SubScope.");
        }
        #endregion

        //////ADDDD SAVE LOAD TESTS
    }
}