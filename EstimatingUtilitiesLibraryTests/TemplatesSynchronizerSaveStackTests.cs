using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using EstimatingUtilitiesLibrary;
using EstimatingUtilitiesLibrary.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingUtilitiesLibraryTests
{
    [TestClass]
    public class TemplatesSynchronizerSaveStackTests
    {
        #region SubScope
        [TestMethod]
        public void AddReferenceSubScope()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            ChangeWatcher watcher = new ChangeWatcher(templates);

            TemplateSynchronizer<TECSubScope> ssSynchronizer = templates.SubScopeSynchronizer;

            TECCost testCost = new TECCost(CostType.TEC);
            templates.Catalogs.AssociatedCosts.Add(testCost);

            TECLabeled testTag = new TECLabeled();
            templates.Catalogs.Tags.Add(testTag);

            TECManufacturer testMan = new TECManufacturer();
            templates.Catalogs.Manufacturers.Add(testMan);

            TECDevice testDevice = new TECDevice(new List<TECConnectionType>(), testMan);
            templates.Catalogs.Devices.Add(testDevice);

            TECPoint testPoint = new TECPoint(false);
            testPoint.Label = "Test Point";
            testPoint.Type = IOType.AI;
            testPoint.Quantity = 5;

            TECSubScope templateSS = new TECSubScope(false);
            templateSS.Name = "Test SS";
            templateSS.Description = "Test Desc";
            templates.SubScopeTemplates.Add(templateSS);
            templateSS.AssociatedCosts.Add(testCost);
            templateSS.Tags.Add(testTag);
            templateSS.Devices.Add(testDevice);
            templateSS.Points.Add(testPoint);

            DeltaStacker stack = new DeltaStacker(watcher, templates);
            
            //Act
            TECSubScope refSS = ssSynchronizer.NewItem(templateSS);

            TECPoint newPoint = refSS.Points[0];

            List<UpdateItem> expectedStack = new List<UpdateItem>();

            Dictionary<string, string> data;

            //New SubScope entry
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = refSS.Guid.ToString();
            data[SubScopeTable.Name.Name] = templateSS.Name;
            data[SubScopeTable.Description.Name] = templateSS.Description;
            expectedStack.Add(new UpdateItem(Change.Add, SubScopeTable.TableName, data));

            //Scope Cost relationship
            data = new Dictionary<string, string>();
            data[ScopeAssociatedCostTable.ScopeID.Name] = refSS.Guid.ToString();
            data[ScopeAssociatedCostTable.AssociatedCostID.Name] = testCost.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Add, ScopeAssociatedCostTable.TableName, data));

            //Scope Tag relationship
            data = new Dictionary<string, string>();
            data[ScopeTagTable.ScopeID.Name] = refSS.Guid.ToString();
            data[ScopeTagTable.TagID.Name] = testTag.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Add, ScopeTagTable.TableName, data));

            //SubScope Device relationship
            data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = refSS.Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = testDevice.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Add, SubScopeDeviceTable.TableName, data));

            //New Point entry
            data = new Dictionary<string, string>();
            data[PointTable.ID.Name] = newPoint.Guid.ToString();
            data[PointTable.Name.Name] = testPoint.Label;
            data[PointTable.Quantity.Name] = testPoint.Quantity.ToString();
            data[PointTable.Type.Name] = testPoint.Type.ToString();
            expectedStack.Add(new UpdateItem(Change.Add, PointTable.TableName, data));

            //SubScope Point relationship
            data = new Dictionary<string, string>();
            data[SubScopePointTable.SubScopeID.Name] = refSS.Guid.ToString();
            data[SubScopePointTable.PointID.Name] = newPoint.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Add, PointTable.TableName, data));

            //Template Reference relationship
            data = new Dictionary<string, string>();
            data[TemplateReferenceTable.TemplateID.Name] = templateSS.Guid.ToString();
            data[TemplateReferenceTable.ReferenceID.Name] = refSS.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Add, TemplateReferenceTable.TableName, data));

            //Assert
            Assert.AreEqual(expectedStack.Count, stack.CleansedStack().Count, "Stack length is not what is expected.");
            SaveStackTests.CheckUpdateItems(expectedStack, stack);
        }

        [TestMethod]
        public void RemoveReferenceSubScope()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            ChangeWatcher watcher = new ChangeWatcher(templates);

            TemplateSynchronizer<TECSubScope> ssSynchronizer = templates.SubScopeSynchronizer;

            TECCost testCost = new TECCost(CostType.TEC);
            templates.Catalogs.AssociatedCosts.Add(testCost);

            TECLabeled testTag = new TECLabeled();
            templates.Catalogs.Tags.Add(testTag);

            TECManufacturer testMan = new TECManufacturer();
            templates.Catalogs.Manufacturers.Add(testMan);

            TECDevice testDevice = new TECDevice(new List<TECConnectionType>(), testMan);
            templates.Catalogs.Devices.Add(testDevice);

            TECPoint testPoint = new TECPoint(false);
            testPoint.Label = "Test Point";
            testPoint.Type = IOType.AI;
            testPoint.Quantity = 5;

            TECSubScope templateSS = new TECSubScope(false);
            templateSS.Name = "Test SS";
            templateSS.Description = "Test Desc";
            templates.SubScopeTemplates.Add(templateSS);
            templateSS.AssociatedCosts.Add(testCost);
            templateSS.Tags.Add(testTag);
            templateSS.Devices.Add(testDevice);
            templateSS.Points.Add(testPoint);

            TECEquipment equip = new TECEquipment(false);
            templates.EquipmentTemplates.Add(equip);

            TECSubScope refSS = ssSynchronizer.NewItem(templateSS);
            TECPoint newPoint = refSS.Points[0];
            equip.SubScope.Add(refSS);

            DeltaStacker stack = new DeltaStacker(watcher);

            //Act
            equip.SubScope.Remove(refSS);

            List<UpdateItem> expectedStack = new List<UpdateItem>();

            Dictionary<string, string> data;

            //Old SubScope entry
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = refSS.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, SubScopeTable.TableName, data));

            //Scope Cost relationship
            data = new Dictionary<string, string>();
            data[ScopeAssociatedCostTable.ScopeID.Name] = refSS.Guid.ToString();
            data[ScopeAssociatedCostTable.AssociatedCostID.Name] = testCost.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, ScopeAssociatedCostTable.TableName, data));

            //Scope Tag relationship
            data = new Dictionary<string, string>();
            data[ScopeTagTable.ScopeID.Name] = refSS.Guid.ToString();
            data[ScopeTagTable.TagID.Name] = testTag.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, ScopeTagTable.TableName, data));

            //SubScope Device relationship
            data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = refSS.Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = testDevice.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, SubScopeDeviceTable.TableName, data));

            //Old Point entry
            data = new Dictionary<string, string>();
            data[PointTable.ID.Name] = newPoint.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, PointTable.TableName, data));

            //SubScope Point relationship
            data = new Dictionary<string, string>();
            data[SubScopePointTable.SubScopeID.Name] = refSS.Guid.ToString();
            data[SubScopePointTable.PointID.Name] = newPoint.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, SubScopePointTable.TableName, data));

            //Template Reference relationship
            data = new Dictionary<string, string>();
            data[TemplateReferenceTable.TemplateID.Name] = templateSS.Guid.ToString();
            data[TemplateReferenceTable.ReferenceID.Name] = refSS.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, TemplateReferenceTable.TableName, data));

            //Assert
            Assert.AreEqual(expectedStack.Count, stack.CleansedStack().Count, "Stack length is not what is expected.");
            SaveStackTests.CheckUpdateItems(expectedStack, stack);
        }

        [TestMethod]
        public void SubScopeTemplateChanged()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            ChangeWatcher watcher = new ChangeWatcher(templates);

            throw new NotImplementedException();
        }
        #endregion
    }
}
