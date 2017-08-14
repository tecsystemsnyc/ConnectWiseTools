using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using EstimatingUtilitiesLibrary;
using EstimatingUtilitiesLibrary.DatabaseHelpers;
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
    public class SaveAsBidTests
    {
        private const bool DEBUG = true;

        static TECBid expectedBid;
        static TECLabor expectedLabor;
        static TECSystem expectedSystem;
        static TECSystem expectedSystem1;
        static TECEquipment expectedEquipment;
        static TECSubScope expectedSubScope;
        static TECDevice expectedDevice;
        static TECManufacturer expectedManufacturer;
        static TECPoint expectedPoint;
        static TECScopeBranch expectedBranch;
        static TECLabeled expectedNote;
        static TECLabeled expectedExclusion;
        static TECLabeled expectedTag;
        //static TECDrawing expectedDrawing;
        //static TECPage expectedPage;
        //static TECVisualScope expectedVisualScope;
        static TECController expectedController;

        static string path;

        static TECBid actualBid;
        static TECLabor actualLabor;
        static TECSystem actualSystem;
        static TECSystem actualSystem1;
        static TECEquipment actualEquipment;
        static TECSubScope actualSubScope;
        static TECDevice actualDevice;
        static ObservableCollection<TECDevice> actualDevices;
        static TECManufacturer actualManufacturer;
        static TECPoint actualPoint;
        static TECScopeBranch actualBranch;
        static TECLabeled actualNote;
        static TECLabeled actualExclusion;
        static TECLabeled actualTag;
        //static TECDrawing actualDrawing;
        //static TECPage actualPage;
        //static TECVisualScope actualVisualScope;
        static TECController actualController;


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
            //Arrange
            expectedBid = TestHelper.CreateTestBid();
            expectedLabor = expectedBid.Labor;
            foreach (TECSystem system in expectedBid.Systems)
            {
                if (system.Equipment.Count >  0)
                {
                    expectedSystem = system;
                    break;
                }
            }
            foreach(TECSystem system in expectedBid.Systems)
            {
                if(system != expectedSystem)
                {
                    expectedSystem1 = system;
                    break;
                }
            }
            expectedEquipment = expectedBid.RandomEquipment();
            expectedSubScope = expectedBid.RandomSubScope();
            expectedDevice = expectedBid.Catalogs.Devices.RandomObject();

            expectedManufacturer = expectedBid.Catalogs.Manufacturers.RandomObject();
            expectedPoint = expectedBid.RandomPoint();

            expectedBranch = null;
            foreach (TECScopeBranch branch in expectedBid.ScopeTree)
            {
                if (branch.Label == "Branch 1")
                {
                    expectedBranch = branch;
                    break;
                }
            }

            expectedNote = expectedBid.Notes.RandomObject();
            expectedExclusion = expectedBid.Exclusions.RandomObject();
            expectedTag = expectedBid.Catalogs.Tags.RandomObject();

            //expectedDrawing = expectedBid.Drawings[0];
            //expectedPage = expectedDrawing.Pages[0];
            //expectedVisualScope = expectedPage.PageScope[0];

            expectedController = expectedBid.Controllers.RandomObject();
            
            path = Path.GetTempFileName();

            //Act
            DatabaseSaver.Save(expectedBid, path);
            actualBid = DatabaseLoader.Load(path) as TECBid;
            actualLabor = actualBid.Labor;

            foreach (TECSystem sys in actualBid.Systems)
            {
                if (sys.Guid == expectedSystem.Guid)
                {
                    actualSystem = sys;
                }
                else if (sys.Guid == expectedSystem1.Guid)
                {
                    actualSystem1 = sys;
                }
                if (actualSystem != null && actualSystem1 != null)
                {
                    break;
                }
            }

            actualEquipment = TestHelper.FindObjectInSystems(actualBid.Systems, expectedEquipment) as TECEquipment;
            actualSubScope = TestHelper.FindObjectInSystems(actualBid.Systems, expectedSubScope) as TECSubScope;
            actualDevices = actualSubScope.Devices;
            actualDevice = TestHelper.FindObjectInSystems(actualBid.Systems, expectedDevice) as TECDevice;
            actualPoint = TestHelper.FindObjectInSystems(actualBid.Systems, expectedPoint) as TECPoint;
            foreach (TECManufacturer man in actualBid.Catalogs.Manufacturers)
            {
                if (man.Guid == expectedManufacturer.Guid)
                {
                    actualManufacturer = man;
                    break;
                }
            }
            
            foreach (TECScopeBranch branch in actualBid.ScopeTree)
            {
                if (branch.Guid == expectedBranch.Guid)
                {
                    actualBranch = branch;
                    break;
                }
            }

            foreach (TECLabeled note in actualBid.Notes)
            {
                if (note.Guid == expectedNote.Guid)
                {
                    actualNote = note;
                    break;
                }
            }

            foreach (TECLabeled exclusion in actualBid.Exclusions)
            {
                if (exclusion.Guid == expectedExclusion.Guid)
                {
                    actualExclusion = exclusion;
                    break;
                }
            }

            foreach (TECLabeled tag in actualBid.Catalogs.Tags)
            {
                if (tag.Guid == expectedTag.Guid)
                {
                    actualTag = tag;
                    break;
                }
            }

            //foreach (TECDrawing drawing in actualBid.Drawings)
            //{
            //    if (drawing.Guid == expectedDrawing.Guid)
            //    {
            //        actualDrawing = drawing;
            //        break;
            //    }
            //}

            //foreach (TECPage page in actualDrawing.Pages)
            //{
            //    if (page.Guid == expectedPage.Guid)
            //    {
            //        actualPage = page;
            //        break;
            //    }
            //}

            //foreach (TECVisualScope vs in actualPage.PageScope)
            //{
            //    if (vs.Guid == expectedVisualScope.Guid)
            //    {
            //        actualVisualScope = vs;
            //        break;
            //    }
            //}

            foreach (TECController con in actualBid.Controllers)
            {
                if (con.Guid == expectedController.Guid)
                {
                    actualController = con;
                    break;
                }
            }
            
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            if (DEBUG)
            {
                Console.WriteLine("SaveAs test bid saved to: " + path);
            }
            else
            {
                File.Delete(path);
            }


        }

        [TestMethod]
        public void SaveAs_Bid_Info()
        {
            //Assert
            Assert.AreEqual(expectedBid.Name, actualBid.Name);
            Assert.AreEqual(expectedBid.BidNumber, actualBid.BidNumber);
            Assert.AreEqual(expectedBid.DueDate, actualBid.DueDate);
            Assert.AreEqual(expectedBid.Salesperson, actualBid.Salesperson);
            Assert.AreEqual(expectedBid.Estimator, actualBid.Estimator);
        }

        [TestMethod]
        public void SaveAs_Bid_LaborConstants()
        {
            //Assert
            Assert.AreEqual(expectedLabor.PMCoef, actualLabor.PMCoef);
            Assert.AreEqual(expectedLabor.PMRate, actualLabor.PMRate);

            Assert.AreEqual(expectedLabor.ENGCoef, actualLabor.ENGCoef);
            Assert.AreEqual(expectedLabor.ENGRate, actualLabor.ENGRate);

            Assert.AreEqual(expectedLabor.CommCoef, actualLabor.CommCoef);
            Assert.AreEqual(expectedLabor.CommRate, actualLabor.CommRate);

            Assert.AreEqual(expectedLabor.SoftCoef, actualLabor.SoftCoef);
            Assert.AreEqual(expectedLabor.SoftRate, actualLabor.SoftRate);

            Assert.AreEqual(expectedLabor.GraphCoef, actualLabor.GraphCoef);
            Assert.AreEqual(expectedLabor.GraphRate, actualLabor.GraphRate);
        }

        [TestMethod]
        public void SaveAs_Bid_SubContracterConstants()
        {
            //Assert
            Assert.AreEqual(expectedLabor.ElectricalRate, actualLabor.ElectricalRate);
            Assert.AreEqual(expectedLabor.ElectricalNonUnionRate, actualLabor.ElectricalNonUnionRate);
            Assert.AreEqual(expectedLabor.ElectricalSuperRate, actualLabor.ElectricalSuperRate);
            Assert.AreEqual(expectedLabor.ElectricalSuperNonUnionRate, actualLabor.ElectricalSuperNonUnionRate);

            Assert.AreEqual(expectedLabor.ElectricalIsOnOvertime, actualLabor.ElectricalIsOnOvertime);
            Assert.AreEqual(expectedLabor.ElectricalIsUnion, actualLabor.ElectricalIsUnion);
        }

        [TestMethod]
        public void SaveAs_Bid_UserAdjustments()
        {
            //Assert
            Assert.AreEqual(expectedLabor.PMExtraHours, actualLabor.PMExtraHours);
            Assert.AreEqual(expectedLabor.ENGExtraHours, actualLabor.ENGExtraHours);
            Assert.AreEqual(expectedLabor.CommExtraHours, actualLabor.CommExtraHours);
            Assert.AreEqual(expectedLabor.SoftExtraHours, actualLabor.SoftExtraHours);
            Assert.AreEqual(expectedLabor.GraphExtraHours, actualLabor.GraphExtraHours);
        }

        [TestMethod]
        public void SaveAs_Bid_Parameters()
        {
            Assert.AreEqual(expectedBid.Parameters.IsTaxExempt, actualBid.Parameters.IsTaxExempt);
        }

        [TestMethod]
        public void SaveAs_Bid_System()
        {
            //Assert
            Assert.AreEqual(expectedSystem.Name, actualSystem.Name);
            Assert.AreEqual(expectedSystem.Description, actualSystem.Description);
            Assert.AreEqual(expectedSystem.Instances.Count, actualSystem.Instances.Count);
            Assert.AreEqual(expectedSystem.Equipment.Count, actualSystem.Equipment.Count);
            Assert.AreEqual(expectedSystem.Controllers.Count, actualSystem.Controllers.Count);
            Assert.AreEqual(expectedSystem.Panels.Count, actualSystem.Panels.Count);
            Assert.AreEqual(expectedSystem.ScopeBranches.Count, actualSystem.ScopeBranches.Count);
            Assert.AreEqual(expectedSystem.AssociatedCosts.Count, actualSystem.AssociatedCosts.Count);
            Assert.AreEqual(expectedSystem.TypicalInstanceDictionary.GetFullDictionary().Count, actualSystem.TypicalInstanceDictionary.GetFullDictionary().Count);
            Assert.AreEqual(expectedSystem.MiscCosts.Count, actualSystem.MiscCosts.Count);
        }

        [TestMethod]
        public void SaveAs_Bid_Equipment()
        {
            //Assert
            Assert.AreEqual(expectedEquipment.Name, actualEquipment.Name);
            Assert.AreEqual(expectedEquipment.Description, actualEquipment.Description);
        }

        [TestMethod]
        public void SaveAs_Bid_SubScope()
        {
            //Assert
            Assert.AreEqual(expectedSubScope.Name, actualSubScope.Name);
            Assert.AreEqual(expectedSubScope.Description, actualSubScope.Description);
        }

        [TestMethod]
        public void SaveAs_Bid_Device()
        {
            //Assert
            Assert.AreEqual(expectedDevice.Name, actualDevice.Name);
            Assert.AreEqual(expectedDevice.Description, actualDevice.Description);
            int actualQuantity = 0;
            foreach (TECDevice device in actualDevices)
            {
                if (device.Guid == actualDevice.Guid)
                {
                    actualQuantity++;
                }
            }
            int expectedQuantity = 0;
            foreach (TECDevice device in expectedSubScope.Devices)
            {
                if (device.Guid == expectedDevice.Guid)
                {
                    expectedQuantity++;
                }
            }
            Assert.AreEqual(expectedQuantity, actualQuantity);
            Assert.AreEqual(expectedDevice.Cost, actualDevice.Cost);

            foreach(TECElectricalMaterial expectedConnectionType in expectedDevice.ConnectionTypes)
            {
                bool found = false;
                foreach (TECElectricalMaterial actualConnectionType in actualDevice.ConnectionTypes)
                {
                    if (actualConnectionType.Guid == expectedConnectionType.Guid)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    Assert.Fail("ConnectionType not found on device.");
                }
            }

            Assert.AreEqual(actualManufacturer.Guid, actualDevice.Manufacturer.Guid);
        }

        [TestMethod]
        public void SaveAs_Bid_Manufacturer()
        {
            //Assert
            Assert.AreEqual(expectedManufacturer.Label, actualManufacturer.Label);
            Assert.IsTrue(TestHelper.areDoublesEqual(expectedManufacturer.Multiplier, actualManufacturer.Multiplier),
                "Expected: " + expectedManufacturer.Multiplier + " Actual: " + actualManufacturer.Multiplier);

            Assert.AreEqual(expectedDevice.Manufacturer.Label, expectedDevice.Manufacturer.Label);
            Assert.IsTrue(TestHelper.areDoublesEqual(expectedDevice.Manufacturer.Multiplier, expectedDevice.Manufacturer.Multiplier));
            Assert.AreEqual(expectedDevice.Manufacturer.Guid, expectedDevice.Manufacturer.Guid);
        }

        [TestMethod]
        public void SaveAs_Bid_Point()
        {
            //Assert
            Assert.AreEqual(expectedPoint.Label, actualPoint.Label);
            Assert.AreEqual(expectedPoint.Quantity, actualPoint.Quantity);
            Assert.AreEqual(expectedPoint.Type, actualPoint.Type);
        }

        [TestMethod]
        public void SaveAs_Bid_Location()
        {
            //Assert
            Assert.AreEqual(expectedBid.Locations.Count, actualBid.Locations.Count);
            Assert.AreEqual(expectedSystem.Location.Guid, actualSystem.Location.Guid);
            Assert.AreEqual(expectedSystem1.Location.Guid, actualSystem1.Location.Guid);
            Assert.AreEqual(expectedEquipment.Location.Guid, actualEquipment.Location.Guid);
            Assert.AreEqual(expectedSubScope.Location.Guid, actualSubScope.Location.Guid);
        }

        [TestMethod]
        public void SaveAs_Bid_ScopeBranch()
        {
            //Assert
            Assert.AreEqual(expectedBranch.Label, actualBranch.Label);
            Assert.AreEqual(expectedBranch.Guid, actualBranch.Guid);

            Assert.AreEqual(expectedBranch.Branches[0].Label, actualBranch.Branches[0].Label);
            Assert.AreEqual(expectedBranch.Branches[0].Guid, actualBranch.Branches[0].Guid);

            Assert.AreEqual(expectedBranch.Branches[0].Branches[0].Label, actualBranch.Branches[0].Branches[0].Label);
            Assert.AreEqual(expectedBranch.Branches[0].Branches[0].Guid, actualBranch.Branches[0].Branches[0].Guid);
        }

        [TestMethod]
        public void SaveAs_Bid_Note()
        {
            //Assert
            Assert.AreEqual(expectedNote.Label, actualNote.Label);
        }

        [TestMethod]
        public void SaveAs_Bid_Exclusion()
        {
            //Assert
            Assert.AreEqual(expectedExclusion.Label, actualExclusion.Label);
        }

        [TestMethod]
        public void SaveAs_Bid_Tag()
        {
            //Assert
            Assert.AreEqual(expectedTag.Label, actualTag.Label);

            string expectedText = actualTag.Label;
            Guid expectedGuid = actualTag.Guid;

            Assert.AreEqual(expectedSystem.Tags[0].Guid, actualSystem.Tags[0].Guid);
            Assert.AreEqual(expectedSystem.Tags[0].Label, actualSystem.Tags[0].Label);

            Assert.AreEqual(expectedEquipment.Tags[0].Guid, actualEquipment.Tags[0].Guid);
            Assert.AreEqual(expectedEquipment.Tags[0].Label, actualEquipment.Tags[0].Label);

            Assert.AreEqual(expectedSubScope.Tags[0].Guid, actualSubScope.Tags[0].Guid);
            Assert.AreEqual(expectedSubScope.Tags[0].Label, actualSubScope.Tags[0].Label);

            Assert.AreEqual(expectedDevice.Tags[0].Guid, actualDevice.Tags[0].Guid);
            Assert.AreEqual(expectedDevice.Tags[0].Label, actualDevice.Tags[0].Label);
        }

        //[TestMethod]
        //public void SaveAs_Bid_Drawing()
        //{
        //    //Assert
        //    Assert.AreEqual(expectedDrawing.Label, actualDrawing.Label);
        //    Assert.AreEqual(expectedDrawing.Description, actualDrawing.Description);
        //}

        //[TestMethod]
        //public void SaveAs_Bid_Page()
        //{
        //    //Assert
        //    Assert.AreEqual(expectedPage.PageNum, actualPage.PageNum);
        //}

        //[TestMethod]
        //public void SaveAs_Bid_VisScope()
        //{
        //    //Assert
        //    Assert.AreEqual(expectedVisualScope.X, actualVisualScope.X);
        //    Assert.AreEqual(expectedVisualScope.Y, actualVisualScope.Y);

        //    Assert.AreEqual(expectedVisualScope.Scope.Guid, actualVisualScope.Scope.Guid);
        //}

        
        [TestMethod]
        public void SaveAs_Bid_Controller()
        {
            //Assert
            Assert.AreEqual(expectedController.Name, actualController.Name);
            Assert.AreEqual(expectedController.Description, actualController.Description);
            Assert.AreEqual(expectedController.Type.Guid, actualController.Type.Guid);

            foreach (TECIO expectedIO in expectedController.Type.IO)
            {
                bool ioExists = false;
                foreach (TECIO actualIO in actualController.Type.IO)
                {
                    if ((expectedIO.Type == actualIO.Type) && (expectedIO.Quantity == actualIO.Quantity))
                    {
                        ioExists = true;
                        break;
                    }
                }
                Assert.IsTrue(ioExists);
            }
        }

        [TestMethod]
        public void SaveAs_Bid_SubScopeConnection()
        {
            //Arrange
            TECController expectedConnectedController = null;
            TECSubScopeConnection expectedConnection = null;
            foreach (TECController controller in expectedBid.Controllers) {
                foreach(TECConnection connection in controller.ChildrenConnections)
                {
                    if(connection is TECSubScopeConnection)
                    {
                        expectedConnectedController = controller;
                        expectedConnection = connection as TECSubScopeConnection;
                        break;
                    }
                }
                if(expectedConnectedController != null)
                {
                    break;
                }
            }
            TECController actualConnectedController =  TestHelper.FindControllerInController(actualBid.Controllers, expectedConnectedController);
            TECSubScopeConnection actualConnection = TestHelper.FindConnectionInController(actualConnectedController, expectedConnection) as TECSubScopeConnection;

            //Assert
            Assert.AreEqual(expectedConnection.Guid, actualConnection.Guid);
            Assert.AreEqual(expectedConnection.ConduitType.Guid, actualConnection.ConduitType.Guid);
            Assert.AreEqual(expectedConnection.Length, actualConnection.Length);
            Assert.AreEqual(expectedConnection.ParentController.Guid, actualConnection.ParentController.Guid);
            Assert.AreEqual(expectedConnection.SubScope.Guid, actualConnection.SubScope.Guid);

        }

        [TestMethod]
        public void SaveAs_Bid_MiscCost()
        {
            //Arrange
            TECMisc expectedCost = expectedBid.MiscCosts[0];
            TECMisc actualCost = null;
            foreach (TECMisc misc in actualBid.MiscCosts)
            {
                if(misc.Guid == expectedCost.Guid)
                {
                    actualCost = misc;
                }
            }

            Assert.AreEqual(expectedCost.Name, actualCost.Name);
            Assert.AreEqual(expectedCost.Cost, actualCost.Cost);
            Assert.AreEqual(expectedCost.Quantity, actualCost.Quantity);
        }

        [TestMethod]
        public void SaveAs_Bid_Panel()
        {
            //Arrange
            TECPanel expectedPanel = expectedBid.Panels.RandomObject();
            TECPanel actualPanel = null;
            foreach (TECPanel panel in actualBid.Panels)
            {
                if(panel.Guid == expectedPanel.Guid)
                {
                    actualPanel = panel;
                    break;
                }
            }
            
            Assert.AreEqual(expectedPanel.Name, actualPanel.Name);
            Assert.AreEqual(expectedPanel.Type.Guid, actualPanel.Type.Guid);
        }

        [TestMethod]
        public void SaveAs_Bid_PanelType()
        {
            //Arrange
            TECPanelType expectedCost = expectedBid.Catalogs.PanelTypes[0];
            TECPanelType actualCost = expectedBid.Catalogs.PanelTypes[0];

            Assert.AreEqual(expectedCost.Guid, expectedBid.Catalogs.PanelTypes[0].Guid);
            Assert.AreEqual(expectedCost.Name, expectedBid.Catalogs.PanelTypes[0].Name);
            Assert.AreEqual(expectedCost.Cost, expectedBid.Catalogs.PanelTypes[0].Cost);
        }

        [TestMethod]
        public void SaveAs_Bid_ControlledScope()
        {
            Assert.AreEqual(expectedSystem.Guid, actualSystem.Guid);
            Assert.AreEqual(expectedSystem.Equipment.Count, actualSystem.Equipment.Count);
            Assert.AreEqual(expectedSystem.Controllers.Count, actualSystem.Controllers.Count);
            Assert.AreEqual(expectedSystem.Panels.Count, actualSystem.Panels.Count);

            foreach(TECPanel panel in expectedSystem.Panels)
            {
                foreach(TECController controller in panel.Controllers)
                {
                    foreach(TECPanel obervedPanel in actualSystem.Panels)
                    {
                        if(obervedPanel.Guid == panel.Guid)
                        {
                            bool containsController = false;
                            foreach(TECController observedController in obervedPanel.Controllers)
                            {
                                if(observedController.Guid == controller.Guid)
                                {
                                    containsController = true;
                                }
                            }
                            Assert.IsTrue(containsController);
                        }
                    }
                }
            }
            Assert.AreEqual(expectedSystem.Panels.Count, actualSystem.Panels.Count);

        }

        [TestMethod]
        public void SaveAs_Bid_ControlledScopeInstances()
        {
            TECBid saveBid = new TECBid();
            saveBid.Catalogs = TestHelper.CreateTestCatalogs();
            TECSystem system = TestHelper.CreateTestSystem(saveBid.Catalogs);
            saveBid.Systems.Add(system);
            
            //Act
            path = Path.GetTempFileName();
            DatabaseSaver.Save(saveBid, path);
            TECBid loadedBid = DatabaseLoader.Load(path) as TECBid;
            TECSystem loadedSystem = loadedBid.Systems[0];
            
            Assert.AreEqual(system.Instances.Count, loadedSystem.Instances.Count);
            foreach(TECSystem loadedInstance in loadedSystem.Instances)
            {
                foreach(TECSystem saveInstance in system.Instances)
                {
                    if(loadedInstance.Guid == saveInstance.Guid)
                    {
                        Assert.AreEqual(loadedInstance.Equipment.Count, saveInstance.Equipment.Count);
                        Assert.AreEqual(loadedInstance.Panels.Count, saveInstance.Panels.Count);
                        Assert.AreEqual(loadedInstance.Controllers.Count, saveInstance.Controllers.Count);
                    }
                }
            }
        }

        [TestMethod]
        public void SaveAs_Bid_Estimate()
        {
            TECBid saveBid = TestHelper.CreateTestBid();
            var watcher = new ChangeWatcher(saveBid);
            TECEstimator estimate = new TECEstimator(saveBid, watcher);
            var expectedTotalCost = estimate.TotalCost;
            double delta = 0.0001;
            
            //Act
            path = Path.GetTempFileName();
            DatabaseSaver.Save(saveBid, path);
            TECBid loadedBid = DatabaseLoader.Load(path) as TECBid;
            var loadedWatcher = new ChangeWatcher(saveBid);
            TECEstimator loadedEstimate = new TECEstimator(loadedBid, loadedWatcher);

            Assert.AreEqual(expectedTotalCost, loadedEstimate.TotalCost, delta);
        }
    }
}
