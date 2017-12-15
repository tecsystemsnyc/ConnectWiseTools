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

            templates.SubScopeTemplates.Add(templateSS);
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
            throw new NotImplementedException();
        }

        [TestMethod]
        public void ChangeItemTest()
        {
            throw new NotImplementedException();
        }

        //////ADDDD SAVE LOAD TESTS
    }
}