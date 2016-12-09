using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class SaveTemplateTests
    {
        TECTemplates templates;
        ChangeStack testStack;
        string path;

        [TestInitialize]
        public void TestInitialize()
        {
            //Arrange
            templates = TestHelper.CreateTestTemplates();
            testStack = new ChangeStack(templates);
            path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".tdb";
            EstimatingLibraryDatabase.SaveTemplatesToNewDB(path, templates);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //File.Delete(path);
            Console.WriteLine("SaveTemplates test templates: " + path);
        }

        #region Save System

        [TestMethod]
        public void Save_Templates_Add_System()
        {
            //Act
            TECSystem expectedSystem = new TECSystem("New system", "New system desc", 123.5, new ObservableCollection<TECEquipment>());
            expectedSystem.Quantity = 1235;

            templates.SystemTemplates.Add(expectedSystem);

            EstimatingLibraryDatabase.UpdateTemplatesToDB(path, testStack);

            TECTemplates actualTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(path);

            TECSystem actualSystem = null;
            foreach (TECSystem system in actualTemplates.SystemTemplates)
            {
                if (expectedSystem.Guid == system.Guid)
                {
                    actualSystem = system;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedSystem.Name, actualSystem.Name);
            Assert.AreEqual(expectedSystem.Description, actualSystem.Description);
            Assert.AreEqual(expectedSystem.Quantity, actualSystem.Quantity);
            Assert.AreEqual(expectedSystem.BudgetPrice, actualSystem.BudgetPrice);
        }

        [TestMethod]
        public void Save_Templates_Remove_System()
        {
            Assert.Fail();
        }

        #endregion Save System
    }
}
