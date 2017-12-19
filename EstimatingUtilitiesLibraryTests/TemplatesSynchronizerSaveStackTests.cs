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

            TECTag testTag = new TECTag();
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

            DeltaStacker stack = new DeltaStacker(watcher, templates);

            //Act
            TECSubScope refSS = ssSynchronizer.NewItem(templateSS);

            equip.SubScope.Add(refSS);

            TECPoint newPoint = refSS.Points[0];

            List<UpdateItem> expectedStack = new List<UpdateItem>();

            Dictionary<string, string> data;

            //Template Reference relationship
            data = new Dictionary<string, string>();
            data[TemplateReferenceTable.TemplateID.Name] = templateSS.Guid.ToString();
            data[TemplateReferenceTable.ReferenceID.Name] = refSS.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Add, TemplateReferenceTable.TableName, data));

            //New SubScope entry
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = refSS.Guid.ToString();
            data[SubScopeTable.Name.Name] = templateSS.Name;
            data[SubScopeTable.Description.Name] = templateSS.Description;
            expectedStack.Add(new UpdateItem(Change.Add, SubScopeTable.TableName, data));

            //Scope Tag relationship
            data = new Dictionary<string, string>();
            data[ScopeTagTable.ScopeID.Name] = refSS.Guid.ToString();
            data[ScopeTagTable.TagID.Name] = testTag.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Add, ScopeTagTable.TableName, data));

            //Scope Cost relationship
            data = new Dictionary<string, string>();
            data[ScopeAssociatedCostTable.ScopeID.Name] = refSS.Guid.ToString();
            data[ScopeAssociatedCostTable.AssociatedCostID.Name] = testCost.Guid.ToString();
            data[ScopeAssociatedCostTable.Quantity.Name] = "1";
            expectedStack.Add(new UpdateItem(Change.Add, ScopeAssociatedCostTable.TableName, data));

            //SubScope Device relationship
            data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = refSS.Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = testDevice.Guid.ToString();
            data[SubScopeDeviceTable.Quantity.Name] = "1";
            data[SubScopeDeviceTable.ScopeIndex.Name] = "0";
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
            expectedStack.Add(new UpdateItem(Change.Add, SubScopePointTable.TableName, data));

            //Equipment SubScope relationship
            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = equip.Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = refSS.Guid.ToString();
            data[EquipmentSubScopeTable.ScopeIndex.Name] = "0";
            expectedStack.Add(new UpdateItem(Change.Add, EquipmentSubScopeTable.TableName, data));

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

            TECTag testTag = new TECTag();
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

            DeltaStacker stack = new DeltaStacker(watcher, templates);

            //Act
            equip.SubScope.Remove(refSS);

            List<UpdateItem> expectedStack = new List<UpdateItem>();

            Dictionary<string, string> data;

            //Template Reference relationship
            data = new Dictionary<string, string>();
            data[TemplateReferenceTable.TemplateID.Name] = templateSS.Guid.ToString();
            data[TemplateReferenceTable.ReferenceID.Name] = refSS.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, TemplateReferenceTable.TableName, data));

            //Old SubScope entry
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = refSS.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, SubScopeTable.TableName, data));

            //Scope Tag relationship
            data = new Dictionary<string, string>();
            data[ScopeTagTable.ScopeID.Name] = refSS.Guid.ToString();
            data[ScopeTagTable.TagID.Name] = testTag.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, ScopeTagTable.TableName, data));

            //Scope Cost relationship
            data = new Dictionary<string, string>();
            data[ScopeAssociatedCostTable.ScopeID.Name] = refSS.Guid.ToString();
            data[ScopeAssociatedCostTable.AssociatedCostID.Name] = testCost.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, ScopeAssociatedCostTable.TableName, data));

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

            //Equipment SubScope relationship
            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = equip.Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = refSS.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, EquipmentSubScopeTable.TableName, data));

            //Assert
            Assert.AreEqual(expectedStack.Count, stack.CleansedStack().Count, "Stack length is not what is expected.");
            SaveStackTests.CheckUpdateItems(expectedStack, stack);
        }

        [TestMethod]
        public void ChangeSubScopeTemplate()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            ChangeWatcher watcher = new ChangeWatcher(templates);

            TemplateSynchronizer<TECSubScope> ssSynchronizer = templates.SubScopeSynchronizer;

            TECSubScope templateSS = new TECSubScope(false);
            templates.SubScopeTemplates.Add(templateSS);

            TECSubScope refSS = ssSynchronizer.NewItem(templateSS);

            TECEquipment equip = new TECEquipment(false);
            templates.EquipmentTemplates.Add(equip);

            equip.SubScope.Add(refSS);

            DeltaStacker stack = new DeltaStacker(watcher, templates);

            //Act
            templateSS.Name = "New Name";
            templateSS.Description = "New Description";

            List<UpdateItem> expectedStack = new List<UpdateItem>();

            Dictionary<string, string> data;
            Tuple<string, string> pk;

            //Template SubScope name change
            data = new Dictionary<string, string>();
            pk = new Tuple<string, string>(SubScopeTable.ID.Name, templateSS.Guid.ToString());
            data[SubScopeTable.Name.Name] = templateSS.Name;
            expectedStack.Add(new UpdateItem(Change.Edit, SubScopeTable.TableName, data, pk));

            //Reference SubScope name change
            data = new Dictionary<string, string>();
            pk = new Tuple<string, string>(SubScopeTable.ID.Name, refSS.Guid.ToString());
            data[SubScopeTable.Name.Name] = refSS.Name;
            expectedStack.Add(new UpdateItem(Change.Edit, SubScopeTable.TableName, data, pk));

            //Template SubScope description change
            data = new Dictionary<string, string>();
            pk = new Tuple<string, string>(SubScopeTable.ID.Name, templateSS.Guid.ToString());
            data[SubScopeTable.Description.Name] = templateSS.Description;
            expectedStack.Add(new UpdateItem(Change.Edit, SubScopeTable.TableName, data, pk));

            //Reference SubScope description change
            data = new Dictionary<string, string>();
            pk = new Tuple<string, string>(SubScopeTable.ID.Name, refSS.Guid.ToString());
            data[SubScopeTable.Description.Name] = refSS.Description;
            expectedStack.Add(new UpdateItem(Change.Edit, SubScopeTable.TableName, data, pk));

            //Assert
            Assert.AreEqual(expectedStack.Count, stack.CleansedStack().Count, "Stack length is not what is expected.");
            SaveStackTests.CheckUpdateItems(expectedStack, stack);
        }

        [TestMethod]
        public void AddAssociatedCostToSubScopeTemplate()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            ChangeWatcher watcher = new ChangeWatcher(templates);

            TemplateSynchronizer<TECSubScope> ssSynchronizer = templates.SubScopeSynchronizer;

            TECCost testCost = new TECCost(CostType.TEC);
            templates.Catalogs.AssociatedCosts.Add(testCost);

            TECSubScope templateSS = new TECSubScope(false);
            templates.SubScopeTemplates.Add(templateSS);

            TECSubScope refSS = ssSynchronizer.NewItem(templateSS);

            TECEquipment equip = new TECEquipment(false);
            templates.EquipmentTemplates.Add(equip);

            equip.SubScope.Add(refSS);

            DeltaStacker stack = new DeltaStacker(watcher, templates);

            //Act
            templateSS.AssociatedCosts.Add(testCost);

            List<UpdateItem> expectedStack = new List<UpdateItem>();

            Dictionary<string, string> data;

            //Template SubScope Cost relationship
            data = new Dictionary<string, string>();
            data[ScopeAssociatedCostTable.ScopeID.Name] = templateSS.Guid.ToString();
            data[ScopeAssociatedCostTable.AssociatedCostID.Name] = testCost.Guid.ToString();
            data[ScopeAssociatedCostTable.Quantity.Name] = "1";
            expectedStack.Add(new UpdateItem(Change.Add, ScopeAssociatedCostTable.TableName, data));

            //Reference SubScope Cost relationship
            data = new Dictionary<string, string>();
            data[ScopeAssociatedCostTable.ScopeID.Name] = refSS.Guid.ToString();
            data[ScopeAssociatedCostTable.AssociatedCostID.Name] = testCost.Guid.ToString();
            data[ScopeAssociatedCostTable.Quantity.Name] = "1";
            expectedStack.Add(new UpdateItem(Change.Add, ScopeAssociatedCostTable.TableName, data));

            //Assert
            Assert.AreEqual(expectedStack.Count, stack.CleansedStack().Count, "Stack length is not what is expected.");
            SaveStackTests.CheckUpdateItems(expectedStack, stack);
        }

        [TestMethod]
        public void RemoveAssociatedCostFromSubScopeTemplate()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            ChangeWatcher watcher = new ChangeWatcher(templates);

            TemplateSynchronizer<TECSubScope> ssSynchronizer = templates.SubScopeSynchronizer;

            TECCost testCost = new TECCost(CostType.TEC);
            templates.Catalogs.AssociatedCosts.Add(testCost);

            TECSubScope templateSS = new TECSubScope(false);
            templates.SubScopeTemplates.Add(templateSS);
            templateSS.AssociatedCosts.Add(testCost);

            TECSubScope refSS = ssSynchronizer.NewItem(templateSS);

            TECEquipment equip = new TECEquipment(false);
            templates.EquipmentTemplates.Add(equip);

            equip.SubScope.Add(refSS);

            DeltaStacker stack = new DeltaStacker(watcher, templates);

            //Act
            templateSS.AssociatedCosts.Remove(testCost);

            List<UpdateItem> expectedStack = new List<UpdateItem>();

            Dictionary<string, string> data;

            //Template SubScope Cost relationship
            data = new Dictionary<string, string>();
            data[ScopeAssociatedCostTable.ScopeID.Name] = templateSS.Guid.ToString();
            data[ScopeAssociatedCostTable.AssociatedCostID.Name] = testCost.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, ScopeAssociatedCostTable.TableName, data));

            //Reference SubScope Cost relationship
            data = new Dictionary<string, string>();
            data[ScopeAssociatedCostTable.ScopeID.Name] = refSS.Guid.ToString();
            data[ScopeAssociatedCostTable.AssociatedCostID.Name] = testCost.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, ScopeAssociatedCostTable.TableName, data));

            //Assert
            Assert.AreEqual(expectedStack.Count, stack.CleansedStack().Count, "Stack length is not what is expected.");
            SaveStackTests.CheckUpdateItems(expectedStack, stack);
        }

        [TestMethod]
        public void AddTagToSubScopeTemplate()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            ChangeWatcher watcher = new ChangeWatcher(templates);

            TemplateSynchronizer<TECSubScope> ssSynchronizer = templates.SubScopeSynchronizer;

            TECTag testTag = new TECTag();
            templates.Catalogs.Tags.Add(testTag);

            TECSubScope templateSS = new TECSubScope(false);
            templates.SubScopeTemplates.Add(templateSS);

            TECSubScope refSS = ssSynchronizer.NewItem(templateSS);

            TECEquipment equip = new TECEquipment(false);
            templates.EquipmentTemplates.Add(equip);

            equip.SubScope.Add(refSS);

            DeltaStacker stack = new DeltaStacker(watcher, templates);

            //Act
            templateSS.Tags.Add(testTag);

            List<UpdateItem> expectedStack = new List<UpdateItem>();

            Dictionary<string, string> data;

            //Template SubScope Tag relationship
            data = new Dictionary<string, string>();
            data[ScopeTagTable.ScopeID.Name] = templateSS.Guid.ToString();
            data[ScopeTagTable.TagID.Name] = testTag.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Add, ScopeTagTable.TableName, data));

            //Reference SubScope Tag relationship
            data = new Dictionary<string, string>();
            data[ScopeTagTable.ScopeID.Name] = refSS.Guid.ToString();
            data[ScopeTagTable.TagID.Name] = testTag.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Add, ScopeTagTable.TableName, data));

            //Assert
            Assert.AreEqual(expectedStack.Count, stack.CleansedStack().Count, "Stack length is not what is expected.");
            SaveStackTests.CheckUpdateItems(expectedStack, stack);
        }

        [TestMethod]
        public void RemoveTagFromSubScopeTemplate()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            ChangeWatcher watcher = new ChangeWatcher(templates);

            TemplateSynchronizer<TECSubScope> ssSynchronizer = templates.SubScopeSynchronizer;

            TECTag testTag = new TECTag();
            templates.Catalogs.Tags.Add(testTag);

            TECSubScope templateSS = new TECSubScope(false);
            templates.SubScopeTemplates.Add(templateSS);
            templateSS.Tags.Add(testTag);

            TECSubScope refSS = ssSynchronizer.NewItem(templateSS);

            TECEquipment equip = new TECEquipment(false);
            templates.EquipmentTemplates.Add(equip);

            equip.SubScope.Add(refSS);

            DeltaStacker stack = new DeltaStacker(watcher, templates);

            //Act
            templateSS.Tags.Remove(testTag);

            List<UpdateItem> expectedStack = new List<UpdateItem>();

            Dictionary<string, string> data;

            //Template SubScope Tag relationship
            data = new Dictionary<string, string>();
            data[ScopeTagTable.ScopeID.Name] = templateSS.Guid.ToString();
            data[ScopeTagTable.TagID.Name] = testTag.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, ScopeTagTable.TableName, data));

            //Reference SubScope Tag relationship
            data = new Dictionary<string, string>();
            data[ScopeTagTable.ScopeID.Name] = refSS.Guid.ToString();
            data[ScopeTagTable.TagID.Name] = testTag.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, ScopeTagTable.TableName, data));

            //Assert
            Assert.AreEqual(expectedStack.Count, stack.CleansedStack().Count, "Stack length is not what is expected.");
            SaveStackTests.CheckUpdateItems(expectedStack, stack);
        }

        [TestMethod]
        public void AddDeviceToSubScopeTemplate()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            ChangeWatcher watcher = new ChangeWatcher(templates);

            TemplateSynchronizer<TECSubScope> ssSynchronizer = templates.SubScopeSynchronizer;

            TECManufacturer testMan = new TECManufacturer();
            templates.Catalogs.Manufacturers.Add(testMan);

            TECDevice testDevice = new TECDevice(new List<TECConnectionType>(), testMan);
            templates.Catalogs.Devices.Add(testDevice);

            TECSubScope templateSS = new TECSubScope(false);
            templates.SubScopeTemplates.Add(templateSS);

            TECSubScope refSS = ssSynchronizer.NewItem(templateSS);

            TECEquipment equip = new TECEquipment(false);
            templates.EquipmentTemplates.Add(equip);

            equip.SubScope.Add(refSS);

            DeltaStacker stack = new DeltaStacker(watcher, templates);

            //Act
            templateSS.Devices.Add(testDevice);

            List<UpdateItem> expectedStack = new List<UpdateItem>();

            Dictionary<string, string> data;

            //Template SubScope Device relationship
            data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = templateSS.Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = testDevice.Guid.ToString();
            data[SubScopeDeviceTable.Quantity.Name] = "1";
            data[SubScopeDeviceTable.ScopeIndex.Name] = "0";
            expectedStack.Add(new UpdateItem(Change.Add, SubScopeDeviceTable.TableName, data));

            //Reference SubScope Device relationship
            data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = refSS.Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = testDevice.Guid.ToString();
            data[SubScopeDeviceTable.Quantity.Name] = "1";
            data[SubScopeDeviceTable.ScopeIndex.Name] = "0";
            expectedStack.Add(new UpdateItem(Change.Add, SubScopeDeviceTable.TableName, data));

            //Assert
            Assert.AreEqual(expectedStack.Count, stack.CleansedStack().Count, "Stack length is not what is expected.");
            SaveStackTests.CheckUpdateItems(expectedStack, stack);
        }

        [TestMethod]
        public void RemoveDeviceFromSubScopeTemplate()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            ChangeWatcher watcher = new ChangeWatcher(templates);

            TemplateSynchronizer<TECSubScope> ssSynchronizer = templates.SubScopeSynchronizer;

            TECManufacturer testMan = new TECManufacturer();
            templates.Catalogs.Manufacturers.Add(testMan);

            TECDevice testDevice = new TECDevice(new List<TECConnectionType>(), testMan);
            templates.Catalogs.Devices.Add(testDevice);

            TECSubScope templateSS = new TECSubScope(false);
            templates.SubScopeTemplates.Add(templateSS);
            templateSS.Devices.Add(testDevice);

            TECSubScope refSS = ssSynchronizer.NewItem(templateSS);

            TECEquipment equip = new TECEquipment(false);
            templates.EquipmentTemplates.Add(equip);

            equip.SubScope.Add(refSS);

            DeltaStacker stack = new DeltaStacker(watcher, templates);

            //Act
            templateSS.Devices.Remove(testDevice);

            List<UpdateItem> expectedStack = new List<UpdateItem>();

            Dictionary<string, string> data;

            //Template SubScope Device relationship
            data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = templateSS.Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = testDevice.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, SubScopeDeviceTable.TableName, data));

            //Reference SubScope Device relationship
            data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = refSS.Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = testDevice.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, SubScopeDeviceTable.TableName, data));

            //Assert
            Assert.AreEqual(expectedStack.Count, stack.CleansedStack().Count, "Stack length is not what is expected.");
            SaveStackTests.CheckUpdateItems(expectedStack, stack);
        }

        [TestMethod]
        public void AddPointToSubScopeTemplate()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            ChangeWatcher watcher = new ChangeWatcher(templates);

            TemplateSynchronizer<TECSubScope> ssSynchronizer = templates.SubScopeSynchronizer;

            TECPoint testPoint = new TECPoint(false);
            testPoint.Label = "Test Point";
            testPoint.Type = IOType.AI;
            testPoint.Quantity = 5;

            TECSubScope templateSS = new TECSubScope(false);
            templates.SubScopeTemplates.Add(templateSS);

            TECSubScope refSS = ssSynchronizer.NewItem(templateSS);

            TECEquipment equip = new TECEquipment(false);
            templates.EquipmentTemplates.Add(equip);

            equip.SubScope.Add(refSS);

            DeltaStacker stack = new DeltaStacker(watcher, templates);

            //Act
            templateSS.Points.Add(testPoint);

            TECPoint refPoint = refSS.Points[0];

            List<UpdateItem> expectedStack = new List<UpdateItem>();

            Dictionary<string, string> data;

            //Template Point Entry
            data = new Dictionary<string, string>();
            data[PointTable.ID.Name] = testPoint.Guid.ToString();
            data[PointTable.Name.Name] = testPoint.Label;
            data[PointTable.Type.Name] = testPoint.Type.ToString();
            data[PointTable.Quantity.Name] = testPoint.Quantity.ToString();
            expectedStack.Add(new UpdateItem(Change.Add, PointTable.TableName, data));

            //Template SubScope Point relationship
            data = new Dictionary<string, string>();
            data[SubScopePointTable.SubScopeID.Name] = templateSS.Guid.ToString();
            data[SubScopePointTable.PointID.Name] = testPoint.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Add, SubScopePointTable.TableName, data));

            //Reference Point Entry
            data = new Dictionary<string, string>();
            data[PointTable.ID.Name] = refPoint.Guid.ToString();
            data[PointTable.Name.Name] = testPoint.Label;
            data[PointTable.Type.Name] = testPoint.Type.ToString();
            data[PointTable.Quantity.Name] = testPoint.Quantity.ToString();
            expectedStack.Add(new UpdateItem(Change.Add, PointTable.TableName, data));

            //Reference SubScope Point relationship
            data = new Dictionary<string, string>();
            data[SubScopePointTable.SubScopeID.Name] = refSS.Guid.ToString();
            data[SubScopePointTable.PointID.Name] = refPoint.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Add, SubScopePointTable.TableName, data));

            //Assert
            Assert.AreEqual(expectedStack.Count, stack.CleansedStack().Count, "Stack length is not what is expected.");
            SaveStackTests.CheckUpdateItems(expectedStack, stack);
        }

        [TestMethod]
        public void RemovePointFromSubScopeTemplate()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            ChangeWatcher watcher = new ChangeWatcher(templates);

            TemplateSynchronizer<TECSubScope> ssSynchronizer = templates.SubScopeSynchronizer;

            TECPoint testPoint = new TECPoint(false);
            testPoint.Label = "Test Point";
            testPoint.Type = IOType.AI;
            testPoint.Quantity = 5;

            TECSubScope templateSS = new TECSubScope(false);
            templates.SubScopeTemplates.Add(templateSS);
            templateSS.Points.Add(testPoint);

            TECSubScope refSS = ssSynchronizer.NewItem(templateSS);

            TECPoint refPoint = refSS.Points[0];

            TECEquipment equip = new TECEquipment(false);
            templates.EquipmentTemplates.Add(equip);

            equip.SubScope.Add(refSS);

            DeltaStacker stack = new DeltaStacker(watcher, templates);

            //Act
            templateSS.Points.Remove(testPoint);

            List<UpdateItem> expectedStack = new List<UpdateItem>();

            Dictionary<string, string> data;

            //Template Point Entry
            data = new Dictionary<string, string>();
            data[PointTable.ID.Name] = testPoint.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, PointTable.TableName, data));

            //Template SubScope Point relationship
            data = new Dictionary<string, string>();
            data[SubScopePointTable.SubScopeID.Name] = templateSS.Guid.ToString();
            data[SubScopePointTable.PointID.Name] = testPoint.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, SubScopePointTable.TableName, data));

            //Reference Point Entry
            data = new Dictionary<string, string>();
            data[PointTable.ID.Name] = refPoint.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, PointTable.TableName, data));

            //Reference SubScope Point relationship
            data = new Dictionary<string, string>();
            data[SubScopePointTable.SubScopeID.Name] = refSS.Guid.ToString();
            data[SubScopePointTable.PointID.Name] = refPoint.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, SubScopePointTable.TableName, data));

            //Assert
            Assert.AreEqual(expectedStack.Count, stack.CleansedStack().Count, "Stack length is not what is expected.");
            SaveStackTests.CheckUpdateItems(expectedStack, stack);
        }

        [TestMethod]
        public void ChangePointInSubScopeTemplate()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            ChangeWatcher watcher = new ChangeWatcher(templates);

            TemplateSynchronizer<TECSubScope> ssSynchronizer = templates.SubScopeSynchronizer;

            TECPoint testPoint = new TECPoint(false);
            testPoint.Label = "Test Point";
            testPoint.Type = IOType.AI;
            testPoint.Quantity = 5;

            TECSubScope templateSS = new TECSubScope(false);
            templates.SubScopeTemplates.Add(templateSS);
            templateSS.Points.Add(testPoint);

            TECSubScope refSS = ssSynchronizer.NewItem(templateSS);

            TECPoint refPoint = refSS.Points[0];

            TECEquipment equip = new TECEquipment(false);
            templates.EquipmentTemplates.Add(equip);

            equip.SubScope.Add(refSS);

            DeltaStacker stack = new DeltaStacker(watcher, templates);

            //Act
            TECPoint original = refSS.Points[0];
            testPoint.Label = "Different Label";
            TECPoint labeled = refSS.Points[0];
            testPoint.Type = IOType.AO;
            TECPoint typed = refSS.Points[0];
            testPoint.Quantity = 69;
            TECPoint quantitied = refSS.Points[0];
            
            List<UpdateItem> expectedStack = new List<UpdateItem>();

            Dictionary<string, string> data;
            Tuple<string, string> pk;

            //Template Point name change
            data = new Dictionary<string, string>();
            pk = new Tuple<string, string>(PointTable.ID.Name, testPoint.Guid.ToString());
            data[PointTable.Name.Name] = testPoint.Label;
            expectedStack.Add(new UpdateItem(Change.Edit, PointTable.TableName, data, pk));

            //Remove old reference point
            data = new Dictionary<string, string>();
            data[PointTable.ID.Name] = original.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, PointTable.TableName, data));

            //Remove old reference relationship
            data = new Dictionary<string, string>();
            data[SubScopePointTable.SubScopeID.Name] = refSS.Guid.ToString();
            data[SubScopePointTable.PointID.Name] = original.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, SubScopePointTable.TableName, data));

            //Add new reference point
            data = new Dictionary<string, string>();
            data[PointTable.ID.Name] = labeled.Guid.ToString();
            data[PointTable.Name.Name] = labeled.Label;
            data[PointTable.Type.Name] = labeled.Type.ToString();
            data[PointTable.Quantity.Name] = labeled.Quantity.ToString();
            expectedStack.Add(new UpdateItem(Change.Add, PointTable.TableName, data));

            //Add new reference relationship
            data = new Dictionary<string, string>();
            data[SubScopePointTable.SubScopeID.Name] = refSS.Guid.ToString();
            data[SubScopePointTable.PointID.Name] = labeled.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Add, SubScopePointTable.TableName, data));


            //Template Point type change
            data = new Dictionary<string, string>();
            pk = new Tuple<string, string>(PointTable.ID.Name, testPoint.Guid.ToString());
            data[PointTable.Type.Name] = testPoint.Type.ToString();
            expectedStack.Add(new UpdateItem(Change.Edit, PointTable.TableName, data, pk));

            //Remove old reference point
            data = new Dictionary<string, string>();
            data[PointTable.ID.Name] = labeled.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, PointTable.TableName, data));

            //Remove old reference relationship
            data = new Dictionary<string, string>();
            data[SubScopePointTable.SubScopeID.Name] = refSS.Guid.ToString();
            data[SubScopePointTable.PointID.Name] = labeled.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, SubScopePointTable.TableName, data));

            //Add new reference point
            data = new Dictionary<string, string>();
            data[PointTable.ID.Name] = typed.Guid.ToString();
            data[PointTable.Name.Name] = typed.Label;
            data[PointTable.Type.Name] = typed.Type.ToString();
            data[PointTable.Quantity.Name] = typed.Quantity.ToString();
            expectedStack.Add(new UpdateItem(Change.Add, PointTable.TableName, data));

            //Add new reference relationship
            data = new Dictionary<string, string>();
            data[SubScopePointTable.SubScopeID.Name] = refSS.Guid.ToString();
            data[SubScopePointTable.PointID.Name] = typed.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Add, SubScopePointTable.TableName, data));



            //Template Point quantity change
            data = new Dictionary<string, string>();
            pk = new Tuple<string, string>(PointTable.ID.Name, testPoint.Guid.ToString());
            data[PointTable.Quantity.Name] = testPoint.Quantity.ToString();
            expectedStack.Add(new UpdateItem(Change.Edit, PointTable.TableName, data, pk));

            //Remove old reference point
            data = new Dictionary<string, string>();
            data[PointTable.ID.Name] = typed.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, PointTable.TableName, data));

            //Remove old reference relationship
            data = new Dictionary<string, string>();
            data[SubScopePointTable.SubScopeID.Name] = refSS.Guid.ToString();
            data[SubScopePointTable.PointID.Name] = typed.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, SubScopePointTable.TableName, data));

            //Add new reference point
            data = new Dictionary<string, string>();
            data[PointTable.ID.Name] = quantitied.Guid.ToString();
            data[PointTable.Name.Name] = quantitied.Label;
            data[PointTable.Type.Name] = quantitied.Type.ToString();
            data[PointTable.Quantity.Name] = quantitied.Quantity.ToString();
            expectedStack.Add(new UpdateItem(Change.Add, PointTable.TableName, data));

            //Add new reference relationship
            data = new Dictionary<string, string>();
            data[SubScopePointTable.SubScopeID.Name] = refSS.Guid.ToString();
            data[SubScopePointTable.PointID.Name] = quantitied.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Add, SubScopePointTable.TableName, data));
            
            //Assert
            Assert.AreEqual(expectedStack.Count, stack.CleansedStack().Count, "Stack length is not what is expected.");
            SaveStackTests.CheckUpdateItems(expectedStack, stack);
        }
        #endregion

        #region Equipment
        [TestMethod]
        public void AddReferenceEquipment()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            ChangeWatcher watcher = new ChangeWatcher(templates);

            TemplateSynchronizer<TECEquipment> equipSynchronizer = templates.EquipmentSynchronizer;

            TECCost testCost = new TECCost(CostType.TEC);
            templates.Catalogs.AssociatedCosts.Add(testCost);

            TECTag testTag = new TECTag();
            templates.Catalogs.Tags.Add(testTag);

            TECSubScope testSS = new TECSubScope(false);
            testSS.Name = "Test SS";
            testSS.Description = "SS Desc";

            TECEquipment templateEquip = new TECEquipment(false);
            templateEquip.Name = "Test Equip";
            templateEquip.Description = "Test Desc";
            templates.EquipmentTemplates.Add(templateEquip);
            templateEquip.AssociatedCosts.Add(testCost);
            templateEquip.Tags.Add(testTag);
            templateEquip.SubScope.Add(testSS);

            TECSystem sys = new TECSystem(false);
            templates.SystemTemplates.Add(sys);

            DeltaStacker stack = new DeltaStacker(watcher, templates);

            //Act
            TECEquipment refEquip = equipSynchronizer.NewItem(templateEquip);

            sys.Equipment.Add(refEquip);

            TECSubScope newSS = refEquip.SubScope[0];

            List<UpdateItem> expectedStack = new List<UpdateItem>();

            Dictionary<string, string> data;

            //SubScope Template Reference relationship
            data = new Dictionary<string, string>();
            data[TemplateReferenceTable.TemplateID.Name] = testSS.Guid.ToString();
            data[TemplateReferenceTable.ReferenceID.Name] = newSS.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Add, TemplateReferenceTable.TableName, data));

            //Equipment Template Reference relationship
            data = new Dictionary<string, string>();
            data[TemplateReferenceTable.TemplateID.Name] = templateEquip.Guid.ToString();
            data[TemplateReferenceTable.ReferenceID.Name] = refEquip.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Add, TemplateReferenceTable.TableName, data));

            //New Equipment entry
            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = refEquip.Guid.ToString();
            data[EquipmentTable.Name.Name] = templateEquip.Name;
            data[EquipmentTable.Description.Name] = templateEquip.Description;
            expectedStack.Add(new UpdateItem(Change.Add, EquipmentTable.TableName, data));

            //Scope Tag relationship
            data = new Dictionary<string, string>();
            data[ScopeTagTable.ScopeID.Name] = refEquip.Guid.ToString();
            data[ScopeTagTable.TagID.Name] = testTag.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Add, ScopeTagTable.TableName, data));

            //Scope Cost relationship
            data = new Dictionary<string, string>();
            data[ScopeAssociatedCostTable.ScopeID.Name] = refEquip.Guid.ToString();
            data[ScopeAssociatedCostTable.AssociatedCostID.Name] = testCost.Guid.ToString();
            data[ScopeAssociatedCostTable.Quantity.Name] = "1";
            expectedStack.Add(new UpdateItem(Change.Add, ScopeAssociatedCostTable.TableName, data));

            //New SubScope entry
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = newSS.Guid.ToString();
            data[SubScopeTable.Name.Name] = testSS.Name;
            data[SubScopeTable.Description.Name] = testSS.Description;
            expectedStack.Add(new UpdateItem(Change.Add, SubScopeTable.TableName, data));

            //Equipment Subscope relationship
            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = refEquip.Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = newSS.Guid.ToString();
            data[EquipmentSubScopeTable.ScopeIndex.Name] = "0";
            expectedStack.Add(new UpdateItem(Change.Add, EquipmentSubScopeTable.TableName, data));

            //System Equipment relationship
            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = sys.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = refEquip.Guid.ToString();
            data[SystemEquipmentTable.ScopeIndex.Name] = "0";
            expectedStack.Add(new UpdateItem(Change.Add, SystemEquipmentTable.TableName, data));
            
            //Assert
            Assert.AreEqual(expectedStack.Count, stack.CleansedStack().Count, "Stack length is not what is expected.");
            SaveStackTests.CheckUpdateItems(expectedStack, stack);
        }

        [TestMethod]
        public void RemoveReferenceEquipment()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            ChangeWatcher watcher = new ChangeWatcher(templates);

            TemplateSynchronizer<TECEquipment> equipSynchronizer = templates.EquipmentSynchronizer;

            TECCost testCost = new TECCost(CostType.TEC);
            templates.Catalogs.AssociatedCosts.Add(testCost);

            TECTag testTag = new TECTag();
            templates.Catalogs.Tags.Add(testTag);

            TECSubScope testSS = new TECSubScope(false);
            testSS.Name = "Test SS";
            testSS.Description = "SS Desc";

            TECEquipment templateEquip = new TECEquipment(false);
            templateEquip.Name = "Test Equip";
            templateEquip.Description = "Test Desc";
            templates.EquipmentTemplates.Add(templateEquip);
            templateEquip.AssociatedCosts.Add(testCost);
            templateEquip.Tags.Add(testTag);
            templateEquip.SubScope.Add(testSS);

            TECSystem sys = new TECSystem(false);
            templates.SystemTemplates.Add(sys);

            TECEquipment refEquip = equipSynchronizer.NewItem(templateEquip);
            sys.Equipment.Add(refEquip);

            TECSubScope newSS = refEquip.SubScope[0];

            DeltaStacker stack = new DeltaStacker(watcher, templates);

            //Act
            sys.Equipment.Remove(refEquip);

            List<UpdateItem> expectedStack = new List<UpdateItem>();

            Dictionary<string, string> data;

            //Equipment Template Reference relationship
            data = new Dictionary<string, string>();
            data[TemplateReferenceTable.TemplateID.Name] = templateEquip.Guid.ToString();
            data[TemplateReferenceTable.ReferenceID.Name] = refEquip.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, TemplateReferenceTable.TableName, data));

            //SubScope Template Reference relationship
            data = new Dictionary<string, string>();
            data[TemplateReferenceTable.TemplateID.Name] = testSS.Guid.ToString();
            data[TemplateReferenceTable.ReferenceID.Name] = newSS.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, TemplateReferenceTable.TableName, data));

            //Old Equipment entry
            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = refEquip.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, EquipmentTable.TableName, data));

            //Scope Tag relationship
            data = new Dictionary<string, string>();
            data[ScopeTagTable.ScopeID.Name] = refEquip.Guid.ToString();
            data[ScopeTagTable.TagID.Name] = testTag.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, ScopeTagTable.TableName, data));

            //Scope Cost relationship
            data = new Dictionary<string, string>();
            data[ScopeAssociatedCostTable.ScopeID.Name] = refEquip.Guid.ToString();
            data[ScopeAssociatedCostTable.AssociatedCostID.Name] = testCost.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, ScopeAssociatedCostTable.TableName, data));

            //Old SubScope entry
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = newSS.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, SubScopeTable.TableName, data));

            //Equipment Subscope relationship
            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = refEquip.Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = newSS.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, EquipmentSubScopeTable.TableName, data));

            //System Equipment relationship
            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = sys.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = refEquip.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, SystemEquipmentTable.TableName, data));


            //Assert
            Assert.AreEqual(expectedStack.Count, stack.CleansedStack().Count, "Stack length is not what is expected.");
            SaveStackTests.CheckUpdateItems(expectedStack, stack);
        }

        [TestMethod]
        public void ChangeEquipmentTemplate()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            ChangeWatcher watcher = new ChangeWatcher(templates);

            TemplateSynchronizer<TECEquipment> equipSynchronizer = templates.EquipmentSynchronizer;

            TECEquipment templateEquip = new TECEquipment(false);
            templates.EquipmentTemplates.Add(templateEquip);

            TECEquipment refEquip = equipSynchronizer.NewItem(templateEquip);

            TECSystem sys = new TECSystem(false);
            templates.SystemTemplates.Add(sys);

            sys.Equipment.Add(refEquip);

            DeltaStacker stack = new DeltaStacker(watcher, templates);

            //Act
            templateEquip.Name = "New Name";
            templateEquip.Description = "New Description";

            List<UpdateItem> expectedStack = new List<UpdateItem>();

            Dictionary<string, string> data;
            Tuple<string, string> pk;

            //Template Equipment name change
            data = new Dictionary<string, string>();
            pk = new Tuple<string, string>(EquipmentTable.ID.Name, templateEquip.Guid.ToString());
            data[EquipmentTable.Name.Name] = templateEquip.Name;
            expectedStack.Add(new UpdateItem(Change.Edit, EquipmentTable.TableName, data, pk));

            //Reference Equipment name change
            data = new Dictionary<string, string>();
            pk = new Tuple<string, string>(EquipmentTable.ID.Name, refEquip.Guid.ToString());
            data[EquipmentTable.Name.Name] = refEquip.Name;
            expectedStack.Add(new UpdateItem(Change.Edit, EquipmentTable.TableName, data, pk));

            //Template Equipment description change
            data = new Dictionary<string, string>();
            pk = new Tuple<string, string>(EquipmentTable.ID.Name, templateEquip.Guid.ToString());
            data[EquipmentTable.Description.Name] = templateEquip.Description;
            expectedStack.Add(new UpdateItem(Change.Edit, EquipmentTable.TableName, data, pk));

            //Reference Equipment description change
            data = new Dictionary<string, string>();
            pk = new Tuple<string, string>(EquipmentTable.ID.Name, refEquip.Guid.ToString());
            data[EquipmentTable.Description.Name] = refEquip.Description;
            expectedStack.Add(new UpdateItem(Change.Edit, EquipmentTable.TableName, data, pk));

            //Assert
            Assert.AreEqual(expectedStack.Count, stack.CleansedStack().Count, "Stack length is not what is expected.");
            SaveStackTests.CheckUpdateItems(expectedStack, stack);
        }

        [TestMethod]
        public void AddAssociatedCostToEquipmentTemplate()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            ChangeWatcher watcher = new ChangeWatcher(templates);

            TemplateSynchronizer<TECEquipment> equipSynchronizer = templates.EquipmentSynchronizer;

            TECCost testCost = new TECCost(CostType.TEC);
            templates.Catalogs.AssociatedCosts.Add(testCost);

            TECEquipment templateEquip = new TECEquipment(false);
            templates.EquipmentTemplates.Add(templateEquip);

            TECEquipment refEquip = equipSynchronizer.NewItem(templateEquip);

            TECSystem sys = new TECSystem(false);
            templates.SystemTemplates.Add(sys);

            sys.Equipment.Add(refEquip);

            DeltaStacker stack = new DeltaStacker(watcher, templates);

            //Act
            templateEquip.AssociatedCosts.Add(testCost);

            List<UpdateItem> expectedStack = new List<UpdateItem>();

            Dictionary<string, string> data;

            //Template Equipment Cost relationship
            data = new Dictionary<string, string>();
            data[ScopeAssociatedCostTable.ScopeID.Name] = templateEquip.Guid.ToString();
            data[ScopeAssociatedCostTable.AssociatedCostID.Name] = testCost.Guid.ToString();
            data[ScopeAssociatedCostTable.Quantity.Name] = "1";
            expectedStack.Add(new UpdateItem(Change.Add, ScopeAssociatedCostTable.TableName, data));

            //Reference Equipment Cost relationship
            data = new Dictionary<string, string>();
            data[ScopeAssociatedCostTable.ScopeID.Name] = refEquip.Guid.ToString();
            data[ScopeAssociatedCostTable.AssociatedCostID.Name] = testCost.Guid.ToString();
            data[ScopeAssociatedCostTable.Quantity.Name] = "1";
            expectedStack.Add(new UpdateItem(Change.Add, ScopeAssociatedCostTable.TableName, data));
            
            //Assert
            Assert.AreEqual(expectedStack.Count, stack.CleansedStack().Count, "Stack length is not what is expected.");
            SaveStackTests.CheckUpdateItems(expectedStack, stack);
        }

        [TestMethod]
        public void RemoveAssociatedCostFromEquipmentTemplate()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            ChangeWatcher watcher = new ChangeWatcher(templates);

            TemplateSynchronizer<TECEquipment> equipSynchronizer = templates.EquipmentSynchronizer;

            TECCost testCost = new TECCost(CostType.TEC);
            templates.Catalogs.AssociatedCosts.Add(testCost);

            TECEquipment templateEquip = new TECEquipment(false);
            templates.EquipmentTemplates.Add(templateEquip);
            templateEquip.AssociatedCosts.Add(testCost);

            TECEquipment refEquip = equipSynchronizer.NewItem(templateEquip);

            TECSystem sys = new TECSystem(false);
            templates.SystemTemplates.Add(sys);

            sys.Equipment.Add(refEquip);

            DeltaStacker stack = new DeltaStacker(watcher, templates);

            //Act
            templateEquip.AssociatedCosts.Remove(testCost);

            List<UpdateItem> expectedStack = new List<UpdateItem>();

            Dictionary<string, string> data;

            //Template Equipment Cost relationship
            data = new Dictionary<string, string>();
            data[ScopeAssociatedCostTable.ScopeID.Name] = templateEquip.Guid.ToString();
            data[ScopeAssociatedCostTable.AssociatedCostID.Name] = testCost.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, ScopeAssociatedCostTable.TableName, data));

            //Reference Equipment Cost relationship
            data = new Dictionary<string, string>();
            data[ScopeAssociatedCostTable.ScopeID.Name] = refEquip.Guid.ToString();
            data[ScopeAssociatedCostTable.AssociatedCostID.Name] = testCost.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, ScopeAssociatedCostTable.TableName, data));

            //Assert
            Assert.AreEqual(expectedStack.Count, stack.CleansedStack().Count, "Stack length is not what is expected.");
            SaveStackTests.CheckUpdateItems(expectedStack, stack);
        }

        [TestMethod]
        public void AddTagToEquipmentTemplate()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            ChangeWatcher watcher = new ChangeWatcher(templates);

            TemplateSynchronizer<TECEquipment> equipSynchronizer = templates.EquipmentSynchronizer;

            TECTag testTag = new TECTag();
            templates.Catalogs.Tags.Add(testTag);

            TECEquipment templateEquip = new TECEquipment(false);
            templates.EquipmentTemplates.Add(templateEquip);

            TECEquipment refEquip = equipSynchronizer.NewItem(templateEquip);

            TECSystem sys = new TECSystem(false);
            templates.SystemTemplates.Add(sys);

            sys.Equipment.Add(refEquip);

            DeltaStacker stack = new DeltaStacker(watcher, templates);

            //Act
            templateEquip.Tags.Add(testTag);

            List<UpdateItem> expectedStack = new List<UpdateItem>();

            Dictionary<string, string> data;

            //Template Equipment Tag relationship
            data = new Dictionary<string, string>();
            data[ScopeTagTable.ScopeID.Name] = templateEquip.Guid.ToString();
            data[ScopeTagTable.TagID.Name] = testTag.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Add, ScopeTagTable.TableName, data));

            //Reference Equipment Tag relationship
            data = new Dictionary<string, string>();
            data[ScopeTagTable.ScopeID.Name] = refEquip.Guid.ToString();
            data[ScopeTagTable.TagID.Name] = testTag.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Add, ScopeTagTable.TableName, data));

            //Assert
            Assert.AreEqual(expectedStack.Count, stack.CleansedStack().Count, "Stack length is not what is expected.");
            SaveStackTests.CheckUpdateItems(expectedStack, stack);
        }

        [TestMethod]
        public void RemoveTagFromEquipmentTemplate()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            ChangeWatcher watcher = new ChangeWatcher(templates);

            TemplateSynchronizer<TECEquipment> equipSynchronizer = templates.EquipmentSynchronizer;

            TECTag testTag = new TECTag();
            templates.Catalogs.Tags.Add(testTag);

            TECEquipment templateEquip = new TECEquipment(false);
            templates.EquipmentTemplates.Add(templateEquip);

            TECEquipment refEquip = equipSynchronizer.NewItem(templateEquip);

            TECSystem sys = new TECSystem(false);
            templates.SystemTemplates.Add(sys);

            sys.Equipment.Add(refEquip);

            DeltaStacker stack = new DeltaStacker(watcher, templates);

            //Act
            templateEquip.Tags.Add(testTag);

            List<UpdateItem> expectedStack = new List<UpdateItem>();

            Dictionary<string, string> data;

            //Template Equipment Tag relationship
            data = new Dictionary<string, string>();
            data[ScopeTagTable.ScopeID.Name] = templateEquip.Guid.ToString();
            data[ScopeTagTable.TagID.Name] = testTag.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Add, ScopeTagTable.TableName, data));

            //Reference Equipment Tag relationship
            data = new Dictionary<string, string>();
            data[ScopeTagTable.ScopeID.Name] = refEquip.Guid.ToString();
            data[ScopeTagTable.TagID.Name] = testTag.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Add, ScopeTagTable.TableName, data));

            //Assert
            Assert.AreEqual(expectedStack.Count, stack.CleansedStack().Count, "Stack length is not what is expected.");
            SaveStackTests.CheckUpdateItems(expectedStack, stack);
        }

        [TestMethod]
        public void AddSubScopeToEquipmentTemplate()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            ChangeWatcher watcher = new ChangeWatcher(templates);

            TemplateSynchronizer<TECEquipment> equipSynchronizer = templates.EquipmentSynchronizer;

            TECEquipment templateEquip = new TECEquipment(false);
            templates.EquipmentTemplates.Add(templateEquip);

            TECSubScope ss = new TECSubScope(false);
            ss.Name = "Test SubScope";
            ss.Description = "SS Desc";

            TECEquipment refEquip = equipSynchronizer.NewItem(templateEquip);

            TECSystem sys = new TECSystem(false);
            templates.SystemTemplates.Add(sys);

            sys.Equipment.Add(refEquip);

            DeltaStacker stack = new DeltaStacker(watcher, templates);

            //Act
            templateEquip.SubScope.Add(ss);

            TECSubScope newSS = refEquip.SubScope[0];

            List<UpdateItem> expectedStack = new List<UpdateItem>();

            Dictionary<string, string> data;
            
            //New SubScope entry (template Equipment)
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = ss.Guid.ToString();
            data[SubScopeTable.Name.Name] = ss.Name;
            data[SubScopeTable.Description.Name] = ss.Description;
            expectedStack.Add(new UpdateItem(Change.Add, SubScopeTable.TableName, data));

            //Equipment SubScope relationship
            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = templateEquip.Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = ss.Guid.ToString();
            data[EquipmentSubScopeTable.ScopeIndex.Name] = "0";
            expectedStack.Add(new UpdateItem(Change.Add, EquipmentSubScopeTable.TableName, data));

            //Template Reference relationship
            data = new Dictionary<string, string>();
            data[TemplateReferenceTable.TemplateID.Name] = ss.Guid.ToString();
            data[TemplateReferenceTable.ReferenceID.Name] = newSS.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Add, TemplateReferenceTable.TableName, data));

            //New SubScope entry (reference Equipment)
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = newSS.Guid.ToString();
            data[SubScopeTable.Name.Name] = newSS.Name;
            data[SubScopeTable.Description.Name] = newSS.Description;
            expectedStack.Add(new UpdateItem(Change.Add, SubScopeTable.TableName, data));

            //Equipment SubScope relationship
            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = refEquip.Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = newSS.Guid.ToString();
            data[EquipmentSubScopeTable.ScopeIndex.Name] = "0";
            expectedStack.Add(new UpdateItem(Change.Add, EquipmentSubScopeTable.TableName, data));


            //Assert
            Assert.AreEqual(expectedStack.Count, stack.CleansedStack().Count, "Stack length is not what is expected.");
            SaveStackTests.CheckUpdateItems(expectedStack, stack);
        }

        [TestMethod]
        public void RemoveSubScopeFromEquipmentTemplate()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            ChangeWatcher watcher = new ChangeWatcher(templates);

            TemplateSynchronizer<TECEquipment> equipSynchronizer = templates.EquipmentSynchronizer;

            TECSubScope ss = new TECSubScope(false);
            ss.Name = "Test SubScope";
            ss.Description = "SS Desc";

            TECEquipment templateEquip = new TECEquipment(false);
            templates.EquipmentTemplates.Add(templateEquip);
            templateEquip.SubScope.Add(ss);

            TECEquipment refEquip = equipSynchronizer.NewItem(templateEquip);

            TECSubScope refSS = refEquip.SubScope[0];

            TECSystem sys = new TECSystem(false);
            templates.SystemTemplates.Add(sys);

            sys.Equipment.Add(refEquip);

            DeltaStacker stack = new DeltaStacker(watcher, templates);

            //Act
            templateEquip.SubScope.Remove(ss);


            foreach (var item in stack.CleansedStack())
            {
                Console.WriteLine(String.Format("{0} into table {1}, {2} pieces of data", item.Change, item.Table, item.FieldData.Count));
            }

            List<UpdateItem> expectedStack = new List<UpdateItem>();

            Dictionary<string, string> data;

            //Template SubScope entry
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = ss.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, SubScopeTable.TableName, data));

            //Template Equipment SubScope relationship
            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = templateEquip.Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = ss.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, EquipmentSubScopeTable.TableName, data));

            //Template Reference relationship
            data = new Dictionary<string, string>();
            data[TemplateReferenceTable.TemplateID.Name] = ss.Guid.ToString();
            data[TemplateReferenceTable.ReferenceID.Name] = refSS.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, TemplateReferenceTable.TableName, data));

            //Reference SubScope entry
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = refSS.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, SubScopeTable.TableName, data));

            //Reference Equipment SubScope relationship
            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = refEquip.Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = refSS.Guid.ToString();
            expectedStack.Add(new UpdateItem(Change.Remove, EquipmentSubScopeTable.TableName, data));
            
            //Assert
            Assert.AreEqual(expectedStack.Count, stack.CleansedStack().Count, "Stack length is not what is expected.");
            SaveStackTests.CheckUpdateItems(expectedStack, stack);
        }

        [TestMethod]
        public void ChangeSubScopeInEquipmentTemplate()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            ChangeWatcher watcher = new ChangeWatcher(templates);

            TemplateSynchronizer<TECEquipment> equipSynchronizer = templates.EquipmentSynchronizer;

            TECSubScope testSS = new TECSubScope(false);
            testSS.Name = "Test SubScope";
            testSS.Description = "Test Description";

            TECEquipment templateEquip = new TECEquipment(false);
            templates.EquipmentTemplates.Add(templateEquip);
            templateEquip.SubScope.Add(testSS);

            TECEquipment refEquip = equipSynchronizer.NewItem(templateEquip);

            TECSubScope refSS = refEquip.SubScope[0];

            TECSystem sys = new TECSystem(false);
            templates.SystemTemplates.Add(sys);

            sys.Equipment.Add(refEquip);

            DeltaStacker stack = new DeltaStacker(watcher, templates);

            //Act
            testSS.Name = "Different Name";
            testSS.Description = "Different Description";

            List<UpdateItem> expectedStack = new List<UpdateItem>();

            Dictionary<string, string> data;
            Tuple<string, string> pk;

            //Template SubScope name change
            data = new Dictionary<string, string>();
            pk = new Tuple<string, string>(SubScopeTable.ID.Name, testSS.Guid.ToString());
            data[SubScopeTable.Name.Name] = testSS.Name;
            expectedStack.Add(new UpdateItem(Change.Edit, SubScopeTable.TableName, data, pk));

            //Reference SubScope name change
            data = new Dictionary<string, string>();
            pk = new Tuple<string, string>(SubScopeTable.ID.Name, refSS.Guid.ToString());
            data[SubScopeTable.Name.Name] = refSS.Name;
            expectedStack.Add(new UpdateItem(Change.Edit, SubScopeTable.TableName, data, pk));

            //Template SubScope description change
            data = new Dictionary<string, string>();
            pk = new Tuple<string, string>(SubScopeTable.ID.Name, testSS.Guid.ToString());
            data[SubScopeTable.Description.Name] = testSS.Description;
            expectedStack.Add(new UpdateItem(Change.Edit, SubScopeTable.TableName, data, pk));

            //Reference SubScope description change
            data = new Dictionary<string, string>();
            pk = new Tuple<string, string>(SubScopeTable.ID.Name, refSS.Guid.ToString());
            data[SubScopeTable.Description.Name] = refSS.Description;
            expectedStack.Add(new UpdateItem(Change.Edit, SubScopeTable.TableName, data, pk));

            //Assert
            Assert.AreEqual(expectedStack.Count, stack.CleansedStack().Count, "Stack length is not what is expected.");
            SaveStackTests.CheckUpdateItems(expectedStack, stack);
        }
        #endregion
    }
}
