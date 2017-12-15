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

        //////ADDDD SAVE LOAD TESTS
    }
}