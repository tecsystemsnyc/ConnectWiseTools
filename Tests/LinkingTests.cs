using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using System.IO;

namespace Tests
{
    [TestClass]
    public class LinkingTests
    {
        static TECBid bid;
        static string path;

        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext TestContext)
        {
            path = Path.GetTempFileName();
            TestDBHelper.CreateTestBid(path);
            bid = TestHelper.LoadTestBid(path);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            File.Delete(path);
        }

        #region Catalog Linking
        [TestMethod]
        public void DeviceLinking()
        {
            foreach(TECDevice device in bid.Catalogs.Devices)
            {
                foreach(TECConnectionType connectionType in device.ConnectionTypes)
                {
                    if (!bid.Catalogs.ConnectionTypes.Contains(connectionType))
                    {
                        Assert.Fail("Connection type in device not linked.");
                    }
                }
                if (!bid.Catalogs.Manufacturers.Contains(device.Manufacturer))
                {
                    Assert.Fail("Manufacturer in device not linked.");
                }
                checkScopeChildrenCatalogLinks(device, bid.Catalogs);
            }
        }

        [TestMethod]
        public void IOModuleLinking()
        {
            foreach(TECIOModule module in bid.Catalogs.IOModules)
            {
                if (!bid.Catalogs.Manufacturers.Contains(module.Manufacturer))
                {
                    Assert.Fail("Manufacturer in IO module not linked.");
                }
                checkScopeChildrenCatalogLinks(module, bid.Catalogs);
            }
        }

        [TestMethod]
        public void ConnectionTypeLinking()
        {
            foreach(TECConnectionType connectionType in bid.Catalogs.ConnectionTypes)
            {
                foreach(TECCost cost in connectionType.RatedCosts)
                {
                    if (!bid.Catalogs.AssociatedCosts.Contains(cost))
                    {
                        Assert.Fail("Rated cost in connection type not linked.");
                    }
                }
                checkScopeChildrenCatalogLinks(connectionType, bid.Catalogs);
            }
        }

        [TestMethod]
        public void ConduitTypeLinking()
        {
            foreach(TECConduitType conduitType in bid.Catalogs.ConduitTypes)
            {
                foreach (TECCost cost in conduitType.RatedCosts)
                {
                    if (!bid.Catalogs.AssociatedCosts.Contains(cost))
                    {
                        Assert.Fail("Rated cost in conduit type not linked.");
                    }
                }
                checkScopeChildrenCatalogLinks(conduitType, bid.Catalogs);
            }
        }
        #endregion

        [TestMethod]
        public void ScopeChildrenLinking()
        {
            foreach (TECSystem typical in bid.Systems)
            {
                checkScopeChildrenCatalogLinks(typical, bid.Catalogs);
                checkScopeLocationLinks(typical, bid);
                foreach (TECSystem instance in typical.SystemInstances)
                {
                    checkScopeChildrenCatalogLinks(instance, bid.Catalogs);
                    checkScopeLocationLinks(instance, bid);
                    foreach (TECEquipment equip in instance.Equipment)
                    {
                        checkScopeChildrenCatalogLinks(equip, bid.Catalogs);
                        checkScopeLocationLinks(equip, bid);
                        foreach (TECSubScope ss in equip.SubScope)
                        {
                            checkScopeChildrenCatalogLinks(ss, bid.Catalogs);
                            checkScopeLocationLinks(ss, bid);
                            foreach (TECPoint point in ss.Points)
                            {
                                checkScopeChildrenCatalogLinks(point, bid.Catalogs);
                                checkScopeLocationLinks(point, bid);
                            }
                        }
                    }
                }
                foreach (TECController control in typical.Controllers)
                {
                    checkScopeChildrenCatalogLinks(control, bid.Catalogs);
                    checkScopeLocationLinks(control, bid);
                }
                foreach (TECPanel panel in typical.Panels)
                {
                    checkScopeChildrenCatalogLinks(panel, bid.Catalogs);
                    checkScopeLocationLinks(panel, bid);
                }
                foreach (TECEquipment equip in typical.Equipment)
                {
                    checkScopeChildrenCatalogLinks(equip, bid.Catalogs);
                    checkScopeLocationLinks(equip, bid);
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        checkScopeChildrenCatalogLinks(ss, bid.Catalogs);
                        checkScopeLocationLinks(ss, bid);
                        foreach (TECPoint point in ss.Points)
                        {
                            checkScopeChildrenCatalogLinks(point, bid.Catalogs);
                            checkScopeLocationLinks(point, bid);
                        }
                    }
                }
            }
            foreach(TECController control in bid.Controllers)
            {
                checkScopeChildrenCatalogLinks(control, bid.Catalogs);
                checkScopeLocationLinks(control, bid);
            }
            foreach(TECPanel panel in bid.Panels)
            {
                checkScopeChildrenCatalogLinks(panel, bid.Catalogs);
                checkScopeLocationLinks(panel, bid);
            }
        }

        #region System Linking
        [TestMethod]
        public void TypicalDictionaryLinking()
        {
            foreach(TECSystem typical in bid.Systems)
            {
                ObservableItemToInstanceList<TECScope> list = typical.CharactersticInstances;
                int scopeFound = 0;
                foreach(TECEquipment equip in typical.Equipment)
                {
                    if (!list.ContainsKey(equip))
                    {
                        Assert.Fail("Equipment in typical not in characteristic instances.");
                    }
                    else { scopeFound++; }
                    foreach(TECSubScope ss in equip.SubScope)
                    {
                        if (!list.ContainsKey(ss))
                        {
                            Assert.Fail("Subscope in typical not in characteristic instances.");
                        }
                        else { scopeFound++; }
                        foreach(TECPoint point in ss.Points)
                        {
                            if (!list.ContainsKey(point))
                            {
                                Assert.Fail("Point in typical not in characteristic instances.");
                            }
                            else { scopeFound++; }
                        }
                    }
                }
                foreach(TECController controller in typical.Controllers)
                {
                    if (!list.ContainsKey(controller))
                    {
                        Assert.Fail("Controller in typical not in characteristic instances.");
                    }
                    else { scopeFound++; }
                }
                foreach (TECPanel panel in typical.Panels)
                {
                    if (!list.ContainsKey(panel))
                    {
                        Assert.Fail("Panel in typical not in characteristic instances.");
                    }
                    else { scopeFound++; }
                }
                Assert.AreEqual(list.Count, scopeFound, "Number of scope found doesn't match the number of scope in characteristic instances.");
            }
        }

        [TestMethod]
        public void InstanceDictionaryLinking()
        {
            foreach(TECSystem typical in bid.Systems)
            {
                ObservableItemToInstanceList<TECScope> list = typical.CharactersticInstances;
                foreach (TECSystem instance in typical.SystemInstances)
                {
                    int scopeFound = 0;
                    foreach (TECEquipment equip in instance.Equipment)
                    {
                        if (!list.ContainsValue(equip))
                        {
                            Assert.Fail("Equipment in instance not in characteristic instances.");
                        }
                        else { scopeFound++; }
                        foreach (TECSubScope ss in equip.SubScope)
                        {
                            if (!list.ContainsValue(ss))
                            {
                                Assert.Fail("Subscope in instance not in characteristic instances.");
                            }
                            else { scopeFound++; }
                            foreach (TECPoint point in ss.Points)
                            {
                                if (!list.ContainsValue(point))
                                {
                                    Assert.Fail("Point in instance not in characteristic instances.");
                                }
                                else { scopeFound++; }
                            }
                        }
                    }
                    foreach (TECController controller in instance.Controllers)
                    {
                        if (!list.ContainsValue(controller))
                        {
                            Assert.Fail("Controller in instance not in characteristic instances.");
                        }
                        else { scopeFound++; }
                    }
                    foreach (TECPanel panel in instance.Panels)
                    {
                        if (!list.ContainsValue(panel))
                        {
                            Assert.Fail("Panel in instance not in characteristic instances.");
                        }
                        else { scopeFound++; }
                    }
                    Assert.AreEqual(list.Count, scopeFound, "Number of scope found doesn't match the number of scope in characteristic instances.");
                }
            }
        }
        #endregion

        [TestMethod]
        //Checks controller manufacturer is in catalogs.
        public void ControllerLinking()
        {
            foreach (TECSystem typical in bid.Systems)
            {
                foreach (TECSystem instance in typical.SystemInstances)
                {
                    foreach (TECController controller in instance.Controllers)
                    {
                        Assert.IsTrue(bid.Catalogs.Manufacturers.Contains(controller.Manufacturer));
                    }
                }
                foreach (TECController controller in typical.Controllers)
                {
                    Assert.IsTrue(bid.Catalogs.Manufacturers.Contains(controller.Manufacturer));
                }
            }
            foreach (TECController controller in bid.Controllers)
            {
                Assert.IsTrue(bid.Catalogs.Manufacturers.Contains(controller.Manufacturer));
            }
        }

        [TestMethod]
        //Checks panel connected to a controller in a bid and panel type in panel is in catalogs.
        public void PanelLinking()
        {
            foreach(TECPanel panel in bid.Panels)
            {
                foreach(TECController panelControl in panel.Controllers)
                {
                    Assert.IsTrue(bid.Controllers.Contains(panelControl), "Controller in panel not found in bid.");
                }
                Assert.IsTrue(bid.Catalogs.PanelTypes.Contains(panel.Type));
            }
            foreach(TECSystem typical in bid.Systems)
            {
                foreach(TECPanel panel in typical.Panels)
                {
                    foreach (TECController panelControl in panel.Controllers)
                    {
                        Assert.IsTrue(typical.Controllers.Contains(panelControl), "Controller in panel not found in typical.");
                    }
                    Assert.IsTrue(bid.Catalogs.PanelTypes.Contains(panel.Type));
                }
                foreach(TECSystem instance in typical.SystemInstances)
                {
                    foreach (TECPanel panel in instance.Panels)
                    {
                        foreach (TECController panelControl in panel.Controllers)
                        {
                            Assert.IsTrue(instance.Controllers.Contains(panelControl), "Controller in panel not found in instance.");
                        }
                        Assert.IsTrue(bid.Catalogs.PanelTypes.Contains(panel.Type));
                    }
                }
            }
        }

        [TestMethod]
        //Checks every device in subscope is in catalogs.
        public void SubScopeLinking()
        {
            foreach (TECSystem typical in bid.Systems)
            {
                foreach (TECSystem instance in typical.SystemInstances)
                {
                    foreach (TECEquipment equip in instance.Equipment)
                    {
                        foreach (TECSubScope ss in equip.SubScope)
                        {
                            foreach (TECDevice dev in ss.Devices)
                            {
                                Assert.IsTrue(bid.Catalogs.Devices.Contains(dev));
                            }
                        }
                    }
                }
                foreach (TECEquipment equip in typical.Equipment)
                {
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        foreach (TECDevice dev in ss.Devices)
                        {
                            Assert.IsTrue(bid.Catalogs.Devices.Contains(dev));
                        }
                    }
                }
            }
        }

        #region Connection Linking
        [TestMethod]
        //Checks every conduit type in connection is in catalogs.
        public void ConnectionLinking()
        {
            foreach (TECSystem typical in bid.Systems)
            {
                foreach (TECSystem instance in typical.SystemInstances)
                {
                    foreach (TECController controller in instance.Controllers)
                    {
                        foreach (TECConnection connection in controller.ChildrenConnections)
                        {
                            Assert.IsTrue(bid.Catalogs.ConduitTypes.Contains(connection.ConduitType) || connection.ConduitType == null);
                        }
                    }
                }
                foreach (TECController controller in typical.Controllers)
                {
                    foreach (TECConnection connection in controller.ChildrenConnections)
                    {
                        Assert.IsTrue(bid.Catalogs.ConduitTypes.Contains(connection.ConduitType) || connection.ConduitType == null);
                    }
                }
            }
            foreach (TECController controller in bid.Controllers)
            {
                foreach (TECConnection connection in controller.ChildrenConnections)
                {
                    Assert.IsTrue(bid.Catalogs.ConduitTypes.Contains(connection.ConduitType) || connection.ConduitType == null);
                }
            }
        }

        [TestMethod]
        public void SubScopeConnectionLinking()
        {
            foreach(TECController controller in bid.Controllers)
            {
                foreach(TECConnection connection in controller.ChildrenConnections)
                {
                    var subScopeConnection = connection as TECSubScopeConnection;
                    if(subScopeConnection != null)
                    {
                        Assert.IsTrue(TestHelper.IsInBid(subScopeConnection.SubScope, bid));
                    }
                }
            }
            foreach(TECSystem typical in bid.Systems)
            {
                foreach (TECController controller in typical.Controllers)
                {
                    foreach (TECConnection connection in controller.ChildrenConnections)
                    {
                        var subScopeConnection = connection as TECSubScopeConnection;
                        if (subScopeConnection != null)
                        {
                            Assert.IsTrue(TestHelper.IsInBid(subScopeConnection.SubScope, bid));
                        }
                    }
                }
                foreach(TECSystem instance in typical.SystemInstances)
                {
                    foreach (TECController controller in instance.Controllers)
                    {
                        foreach (TECConnection connection in controller.ChildrenConnections)
                        {
                            var subScopeConnection = connection as TECSubScopeConnection;
                            if (subScopeConnection != null)
                            {
                                Assert.IsTrue(TestHelper.IsInBid(subScopeConnection.SubScope, bid));
                            }
                        }
                    }
                }
            }
            
        }

        [TestMethod]
        //Checks every controller in network connections for a two-way connection.
        public void NetworkConnectionLinking()
        {
            List<TECController> allControllers = new List<TECController>();
            foreach (TECSystem typical in bid.Systems)
            {
                foreach (TECSystem instance in typical.SystemInstances)
                {
                    foreach (TECController controller in instance.Controllers)
                    {
                        allControllers.Add(controller);
                    }
                }
            }
            foreach (TECController controller in bid.Controllers)
            {
                allControllers.Add(controller);
            }

            foreach (TECController controller in allControllers)
            {
                foreach (TECConnection connection in controller.ChildrenConnections)
                {
                    TECNetworkConnection netConnect = connection as TECNetworkConnection;
                    if (netConnect != null)
                    {
                        Assert.IsTrue(netConnect.ParentController == controller);
                        foreach (TECController childControl in netConnect.ChildrenControllers)
                        {
                            Assert.IsTrue(childControl.ParentConnection == netConnect);
                            Assert.IsTrue(allControllers.Contains(childControl));
                        }
                    }
                }
            }
        }
        #endregion

        private void checkScopeChildrenCatalogLinks(TECScope scope, TECCatalogs catalogs)
        {
            foreach(TECCost cost in scope.AssociatedCosts)
            {
                if (!catalogs.AssociatedCosts.Contains(cost))
                {
                    Assert.Fail("Associated cost in scope not linked.");
                }
            }
            foreach(TECTag tag in scope.Tags)
            {
                if (!catalogs.Tags.Contains(tag))
                {
                    Assert.Fail("Tag in scope not linked.");
                }
            }
        }
        private void checkScopeLocationLinks(TECScope scope, TECBid bid)
        {
            if (scope.Location != null && !bid.Locations.Contains(scope.Location))
            {
                Assert.Fail("Location in scope not linked.");
            }
        }
    }
}
