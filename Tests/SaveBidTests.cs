using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class SaveBidTests
    {
        const bool DEBUG = true;

        TECBid bid;
        ChangeStack testStack;
        static string OGPath;
        string path;

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
            OGPath = Path.GetTempFileName();
            EstimatingLibraryDatabase.SaveBidToNewDB(OGPath, TestHelper.CreateTestBid());
        }

        [TestInitialize]
        public void TestInitialize()
        {
            //Arrange
            //var watch = System.Diagnostics.Stopwatch.StartNew();
            //bid = TestHelper.CreateTestBid();
            //watch.Stop();
            //Console.WriteLine("CreateTestBid: " + watch.ElapsedMilliseconds);
            //watch = System.Diagnostics.Stopwatch.StartNew();
            //testStack = new ChangeStack(bid);
            //watch.Stop();
            //Console.WriteLine("Creating Stack: " + watch.ElapsedMilliseconds);
            //watch = System.Diagnostics.Stopwatch.StartNew();
            //path = Path.GetTempFileName();
            //File.Delete(path);
            //path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            //EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);
            //watch.Stop();
            //Console.WriteLine("SaveBidToNewDB: " + watch.ElapsedMilliseconds);

            bid = TestHelper.CreateTestBid();
            testStack = new ChangeStack(bid);
            path = Path.GetTempFileName();
            File.Delete(path);
            File.Copy(OGPath, path);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            if (DEBUG)
            {
                Console.WriteLine("SaveBid test bid: " + path);
            }
            else
            {
                File.Delete(path);
            }
        }

        #region Save BidInfo
        [TestMethod]
        public void Save_BidInfo_Name()
        {
            //Act
            string expectedName = "Save Name";
            bid.Name = expectedName;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            string actualName = actualBid.Name;

            //Assert
            Assert.AreEqual(expectedName, actualName);
        }

        [TestMethod]
        public void Save_BidInfo_BidNo()
        {
            //Act
            string expectedBidNo = "Save BidNo";
            bid.BidNumber = expectedBidNo;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            string actualBidNo = actualBid.BidNumber;

            //Assert
            Assert.AreEqual(expectedBidNo, actualBidNo);
        }

        [TestMethod]
        public void Save_BidInfo_DueDate()
        {
            //Act
            DateTime expectedDueDate = DateTime.Now;
            bid.DueDate = expectedDueDate;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            DateTime actualDueDate = actualBid.DueDate;

            //Assert
            Assert.AreEqual(expectedDueDate, actualDueDate);
        }

        [TestMethod]
        public void Save_BidInfo_Salesperson()
        {
            //Act
            string expectedSalesperson = "Save Salesperson";
            bid.Salesperson = expectedSalesperson;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            string actualSalesperson = actualBid.Salesperson;

            //Assert
            Assert.AreEqual(expectedSalesperson, actualSalesperson);
        }

        [TestMethod]
        public void Save_BidInfo_Estimator()
        {
            //Act
            string expectedEstimator = "Save Estimator";
            bid.Estimator = expectedEstimator;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            string actualEstimator = actualBid.Estimator;

            //Assert
            Assert.AreEqual(expectedEstimator, actualEstimator);
        }
        #endregion Save BidInfo

        #region Save Labor
        [TestMethod]
        public void Save_Bid_Labor_PMCoef()
        {
            //Act
            double expectedPM = 0.123;
            bid.Labor.PMCoef = expectedPM;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            double actualPM = actualBid.Labor.PMCoef;

            //Assert
            Assert.AreEqual(expectedPM, actualPM);
        }

        [TestMethod]
        public void Save_Bid_Labor_PMRate()
        {
            //Act
            double expectedRate = 564.05;
            bid.Labor.PMRate = expectedRate;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualbid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            double actualRate = actualbid.Labor.PMRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }

        [TestMethod]
        public void Save_Bid_Labor_PMExtraHours()
        {
            //Act
            double expectedHours = 457.69;
            bid.Labor.PMExtraHours = expectedHours;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            double actualHours = actualBid.Labor.PMExtraHours;

            //Assert
            Assert.AreEqual(expectedHours, actualHours);
        }

        [TestMethod]
        public void Save_Bid_Labor_ENGCoef()
        {
            //Act
            double expectedENG = 0.123;
            bid.Labor.ENGCoef = expectedENG;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            double actualENG = actualBid.Labor.ENGCoef;

            //Assert
            Assert.AreEqual(expectedENG, actualENG);
        }

        [TestMethod]
        public void Save_Bid_Labor_ENGRate()
        {
            //Act
            double expectedRate = 564.05;
            bid.Labor.ENGRate = expectedRate;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualbid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            double actualRate = actualbid.Labor.ENGRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }

        [TestMethod]
        public void Save_Bid_Labor_ENGExtraHours()
        {
            //Act
            double expectedHours = 457.69;
            bid.Labor.ENGExtraHours = expectedHours;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            double actualHours = actualBid.Labor.ENGExtraHours;

            //Assert
            Assert.AreEqual(expectedHours, actualHours);
        }

        [TestMethod]
        public void Save_Bid_Labor_CommCoef()
        {
            //Act
            double expectedComm = 0.123;
            bid.Labor.CommCoef = expectedComm;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            double actualComm = actualBid.Labor.CommCoef;

            //Assert
            Assert.AreEqual(expectedComm, actualComm);
        }

        [TestMethod]
        public void Save_Bid_Labor_CommRate()
        {
            //Act
            double expectedRate = 564.05;
            bid.Labor.CommRate = expectedRate;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualbid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            double actualRate = actualbid.Labor.CommRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }

        [TestMethod]
        public void Save_Bid_Labor_CommExtraHours()
        {
            //Act
            double expectedHours = 457.69;
            bid.Labor.CommExtraHours = expectedHours;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            double actualHours = actualBid.Labor.CommExtraHours;

            //Assert
            Assert.AreEqual(expectedHours, actualHours);
        }

        [TestMethod]
        public void Save_Bid_Labor_SoftCoef()
        {
            //Act
            double expectedSoft = 0.123;
            bid.Labor.SoftCoef = expectedSoft;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            double actualSoft = actualBid.Labor.SoftCoef;

            //Assert
            Assert.AreEqual(expectedSoft, actualSoft);
        }

        [TestMethod]
        public void Save_Bid_Labor_SoftRate()
        {
            //Act
            double expectedRate = 564.05;
            bid.Labor.SoftRate = expectedRate;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualbid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            double actualRate = actualbid.Labor.SoftRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }

        [TestMethod]
        public void Save_Bid_Labor_SoftExtraHours()
        {
            //Act
            double expectedHours = 457.69;
            bid.Labor.SoftExtraHours = expectedHours;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            double actualHours = actualBid.Labor.SoftExtraHours;

            //Assert
            Assert.AreEqual(expectedHours, actualHours);
        }

        [TestMethod]
        public void Save_Bid_Labor_GraphCoef()
        {
            //Act
            double expectedGraph = 0.123;
            bid.Labor.GraphCoef = expectedGraph;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            double actualGraph = actualBid.Labor.GraphCoef;

            //Assert
            Assert.AreEqual(expectedGraph, actualGraph);
        }

        [TestMethod]
        public void Save_Bid_Labor_GraphRate()
        {
            //Act
            double expectedRate = 564.05;
            bid.Labor.GraphRate = expectedRate;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualbid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            double actualRate = actualbid.Labor.GraphRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }

        [TestMethod]
        public void Save_Bid_Labor_GraphExtraHours()
        {
            //Act
            double expectedHours = 457.69;
            bid.Labor.GraphExtraHours = expectedHours;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            double actualHours = actualBid.Labor.GraphExtraHours;

            //Assert
            Assert.AreEqual(expectedHours, actualHours);
        }

        [TestMethod]
        public void Save_Bid_Labor_ElecRate()
        {
            //Act
            double expectedRate = 0.123;
            bid.Labor.ElectricalRate = expectedRate;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            double actualRate = actualBid.Labor.ElectricalRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }

        [TestMethod]
        public void Save_Bid_Labor_ElecSuperRate()
        {
            //Act
            double expectedRate = 0.123;
            bid.Labor.ElectricalSuperRate = expectedRate;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            double actualRate = actualBid.Labor.ElectricalSuperRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }

        #endregion Save Labor

        #region Save System
        [TestMethod]
        public void Save_Bid_Add_System()
        {
            //Act
            TECSystem expectedSystem = new TECSystem();
            expectedSystem.Name = "New system";
            expectedSystem.Description = "New system desc";
            expectedSystem.BudgetPrice = 123.5;
            expectedSystem.Quantity = 1235;

            bid.Systems.Add(expectedSystem);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECSystem actualSystem = null;
            foreach (TECSystem system in actualBid.Systems)
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
        public void Save_Bid_Remove_System()
        {
            //Act
            int oldNumSystems = bid.Systems.Count;
            TECSystem systemToRemove = bid.Systems[0];

            bid.Systems.Remove(systemToRemove);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid finalBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            //Assert
            foreach (TECSystem system in finalBid.Systems)
            {
                if (system.Guid == systemToRemove.Guid)
                {
                    Assert.Fail();
                }
            }

            Assert.AreEqual((oldNumSystems - 1), bid.Systems.Count);
        }

        #region Edit System
        [TestMethod]
        public void Save_Bid_System_Name()
        {
            //Act
            TECSystem expectedSystem = bid.Systems[0];
            expectedSystem.Name = "Save System Name";
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECSystem actualSystem = null;
            foreach (TECSystem system in actualBid.Systems)
            {
                if (system.Guid == expectedSystem.Guid)
                {
                    actualSystem = system;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedSystem.Name, actualSystem.Name);
        }

        [TestMethod]
        public void Save_Bid_System_Description()
        {
            //Act
            TECSystem expectedSystem = bid.Systems[0];
            expectedSystem.Description = "Save System Description";
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECSystem actualSystem = null;
            foreach (TECSystem system in actualBid.Systems)
            {
                if (system.Guid == expectedSystem.Guid)
                {
                    actualSystem = system;
                }
            }

            //Assert
            Assert.AreEqual(expectedSystem.Description, actualSystem.Description);
        }

        [TestMethod]
        public void Save_Bid_System_Quantity()
        {
            //Act
            TECSystem expectedSystem = bid.Systems[0];
            expectedSystem.Quantity = 987654321;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECSystem actualSystem = null;
            foreach (TECSystem system in actualBid.Systems)
            {
                if (system.Guid == expectedSystem.Guid)
                {
                    actualSystem = system;
                }
            }

            //Assert
            Assert.AreEqual(expectedSystem.Quantity, actualSystem.Quantity);
        }

        [TestMethod]
        public void Save_Bid_System_BudgetPrice()
        {
            //Act
            TECSystem expectedSystem = bid.Systems[0];
            expectedSystem.BudgetPrice = 9876543.21;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECSystem actualSystem = null;
            foreach (TECSystem system in actualBid.Systems)
            {
                if (system.Guid == expectedSystem.Guid)
                {
                    actualSystem = system;
                }
            }

            //Assert
            Assert.AreEqual(expectedSystem.BudgetPrice, actualSystem.BudgetPrice);
        }
        #endregion Edit System
        #endregion Save System

        #region Save Equipment
        [TestMethod]
        public void Save_Bid_Add_Equipment()
        {
            //Act
            TECEquipment expectedEquipment = new TECEquipment();
            expectedEquipment.Name = "New Equipment";
            expectedEquipment.Description = "New Description";
            expectedEquipment.BudgetPrice = 465543.54;
            expectedEquipment.Quantity = 46554354;

            bid.Systems[0].Equipment.Add(expectedEquipment);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECEquipment actualEquipment = null;
            foreach (TECEquipment equip in actualBid.Systems[0].Equipment)
            {
                if (expectedEquipment.Guid == equip.Guid)
                {
                    actualEquipment = equip;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedEquipment.Name, actualEquipment.Name);
            Assert.AreEqual(expectedEquipment.Description, actualEquipment.Description);
            Assert.AreEqual(expectedEquipment.Quantity, actualEquipment.Quantity);
            Assert.AreEqual(expectedEquipment.BudgetPrice, actualEquipment.BudgetPrice);
        }

        [TestMethod]
        public void Save_Bid_Remove_Equipment()
        {
            //Act
            TECSystem systemToModify = bid.Systems[0];
            int oldNumEquip = systemToModify.Equipment.Count();
            TECEquipment equipToRemove = systemToModify.Equipment[0];

            systemToModify.Equipment.Remove(equipToRemove);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid finalBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECSystem modifiedSystem = null;
            foreach (TECSystem system in bid.Systems)
            {
                if (system.Guid == systemToModify.Guid)
                {
                    modifiedSystem = system;
                    break;
                }
            }

            //Assert
            foreach (TECEquipment equip in modifiedSystem.Equipment)
            {
                if (equipToRemove.Guid == equip.Guid)
                {
                    Assert.Fail();
                }
            }

            Assert.AreEqual((oldNumEquip - 1), modifiedSystem.Equipment.Count);
        }

        #region Edit Equipment
        [TestMethod]
        public void Save_Bid_Equipment_Name()
        {
            //Act
            TECEquipment expectedEquip = bid.Systems[0].Equipment[0];
            expectedEquip.Name = "Save Equip Name";
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECEquipment actualEquip = null;
            foreach (TECSystem system in actualBid.Systems)
            {
                foreach (TECEquipment equip in system.Equipment)
                {
                    if (equip.Guid == expectedEquip.Guid)
                    {
                        actualEquip = equip;
                        break;
                    }
                }
                
            }

            //Assert
            Assert.AreEqual(expectedEquip.Name, actualEquip.Name);
        }

        [TestMethod]
        public void Save_Bid_Equipment_Description()
        {
            //Act
            TECEquipment expectedEquip = bid.Systems[0].Equipment[0];
            expectedEquip.Description = "Save Equip Description";
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECEquipment actualEquip = null;
            foreach (TECSystem system in actualBid.Systems)
            {
                foreach (TECEquipment equip in system.Equipment)
                {
                    if (equip.Guid == expectedEquip.Guid)
                    {
                        actualEquip = equip;
                        break;
                    }
                }

            }

            //Assert
            Assert.AreEqual(expectedEquip.Description, actualEquip.Description);
        }

        [TestMethod]
        public void Save_Bid_Equipment_Quantity()
        {
            //Act
            TECEquipment expectedEquip = bid.Systems[0].Equipment[0];
            expectedEquip.Quantity = 987654321;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECEquipment actualEquip = null;
            foreach (TECSystem system in actualBid.Systems)
            {
                foreach (TECEquipment equip in system.Equipment)
                {
                    if (equip.Guid == expectedEquip.Guid)
                    {
                        actualEquip = equip;
                        break;
                    }
                }

            }

            //Assert
            Assert.AreEqual(expectedEquip.Quantity, actualEquip.Quantity);
        }

        [TestMethod]
        public void Save_Bid_Equipment_BudgetPrice()
        {
            //Act
            TECEquipment expectedEquip = bid.Systems[0].Equipment[0];
            expectedEquip.BudgetPrice = 9876543.21;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECEquipment actualEquip = null;
            foreach (TECSystem system in actualBid.Systems)
            {
                foreach (TECEquipment equip in system.Equipment)
                {
                    if (equip.Guid == expectedEquip.Guid)
                    {
                        actualEquip = equip;
                        break;
                    }
                }
                if (actualEquip != null) break;
            }

            //Assert
            Assert.AreEqual(expectedEquip.BudgetPrice, actualEquip.BudgetPrice);
        }

        #endregion Edit Equipment

        #endregion Save Equipment

        #region Save SubScope
        [TestMethod]
        public void Save_Bid_Add_SubScope()
        {
            //Act
            TECSubScope expectedSubScope = new TECSubScope();
            expectedSubScope.Name = "New SubScope";
            expectedSubScope.Description = "New Description";
            expectedSubScope.Quantity = 235746543;

            bid.Systems[0].Equipment[0].SubScope.Add(expectedSubScope);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECSubScope actualSubScope = null;
            foreach (TECSystem system in actualBid.Systems)
            {
                foreach (TECEquipment equip in system.Equipment)
                {
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        if (ss.Guid == expectedSubScope.Guid)
                        {
                            actualSubScope = ss;
                            break;
                        }
                    }
                    if (actualSubScope != null) break;
                }
                if (actualSubScope != null) break;
            }

            //Assert
            Assert.AreEqual(expectedSubScope.Name, actualSubScope.Name);
            Assert.AreEqual(expectedSubScope.Description, actualSubScope.Description);
            Assert.AreEqual(expectedSubScope.Quantity, actualSubScope.Quantity);
        }

        [TestMethod]
        public void Save_Bid_Remove_SubScope()
        {
            //Act
            TECEquipment equipToModify = bid.Systems[0].Equipment[0];
            int oldNumSubScope = equipToModify.SubScope.Count();
            TECSubScope subScopeToRemove = equipToModify.SubScope[0];

            equipToModify.SubScope.Remove(subScopeToRemove);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECEquipment modifiedEquip = null;
            foreach (TECSystem sys in actualBid.Systems)
            {
                foreach (TECEquipment equip in sys.Equipment)
                {
                    if (equip.Guid == equipToModify.Guid)
                    {
                        modifiedEquip = equip;
                        break;
                    }
                }
                if (modifiedEquip != null) break;
            }

            //Assert
            foreach (TECSubScope ss in modifiedEquip.SubScope)
            {
                if (subScopeToRemove.Guid == ss.Guid) Assert.Fail();
            }

            Assert.AreEqual((oldNumSubScope - 1), modifiedEquip.SubScope.Count);
        }

        #region Edit SubScope
        [TestMethod]
        public void Save_Bid_SubScope_Name()
        {
            //Act
            TECSubScope expectedSubScope = bid.Systems[0].Equipment[0].SubScope[0];
            expectedSubScope.Name = "Save SubScope Name";
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECSubScope actualSubScope = null;
            foreach (TECSystem sys in actualBid.Systems)
            {
                foreach (TECEquipment equip in sys.Equipment)
                {
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        if (ss.Guid == expectedSubScope.Guid)
                        {
                            actualSubScope = ss;
                            break;
                        }
                    }
                    if (actualSubScope != null) break;
                }
                if (actualSubScope != null) break;
            }

            //Assert
            Assert.AreEqual(expectedSubScope.Name, actualSubScope.Name);
        }

        [TestMethod]
        public void Save_Bid_SubScope_Description()
        {
            //Act
            TECSubScope expectedSubScope = bid.Systems[0].Equipment[0].SubScope[0];
            expectedSubScope.Description = "Save SubScope Description";
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECSubScope actualSubScope = null;
            foreach (TECSystem sys in actualBid.Systems)
            {
                foreach (TECEquipment equip in sys.Equipment)
                {
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        if (ss.Guid == expectedSubScope.Guid)
                        {
                            actualSubScope = ss;
                            break;
                        }
                    }
                    if (actualSubScope != null) break;
                }
                if (actualSubScope != null) break;
            }

            //Assert
            Assert.AreEqual(expectedSubScope.Description, actualSubScope.Description);
        }

        [TestMethod]
        public void Save_Bid_SubScope_Quantity()
        {
            //Act
            TECSubScope expectedSubScope = bid.Systems[0].Equipment[0].SubScope[0];
            expectedSubScope.Quantity = 987654321;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECSubScope actualSubScope = null;
            foreach (TECSystem sys in actualBid.Systems)
            {
                foreach (TECEquipment equip in sys.Equipment)
                {
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        if (ss.Guid == expectedSubScope.Guid)
                        {
                            actualSubScope = ss;
                            break;
                        }
                    }
                    if (actualSubScope != null) break;
                }
                if (actualSubScope != null) break;
            }

            //Assert
            Assert.AreEqual(expectedSubScope.Quantity, actualSubScope.Quantity);
        }
        #endregion Edit SubScope
        #endregion Save SubScope

        #region Save Device

        [TestMethod]
        public void Save_Bid_Add_Device()
        {
            //Act
            TECDevice expectedDevice = null;
            //Devices can only be added from the device catalog.
            foreach (TECDevice dev in bid.DeviceCatalog)
            {
                if (dev.Name == "Device C1")
                {
                    expectedDevice = dev;
                    break;
                }
            }

            TECSubScope subScopeToModify = bid.Systems[0].Equipment[0].SubScope[0];

            //Makes a copy, as devices can only be added via drag drop.
            subScopeToModify.Devices = new ObservableCollection<TECDevice>();
            int expectedQuantity = 5;
            subScopeToModify.Devices.Add(new TECDevice(expectedDevice));
            subScopeToModify.Devices.Add(new TECDevice(expectedDevice));
            subScopeToModify.Devices.Add(new TECDevice(expectedDevice));
            subScopeToModify.Devices.Add(new TECDevice(expectedDevice));
            subScopeToModify.Devices.Add(new TECDevice(expectedDevice));

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECDevice actualDevice = null;
            int actualQuantity = 0;
            foreach (TECSystem sys in actualBid.Systems)
            {
                foreach (TECEquipment equip in sys.Equipment)
                {
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        if (ss.Guid == subScopeToModify.Guid)
                        {
                            foreach (TECDevice dev in ss.Devices)
                            {
                                if (dev.Guid == expectedDevice.Guid)
                                { actualQuantity++; }
                            }
                            foreach (TECDevice dev in ss.Devices)
                            {
                                if (dev.Guid == expectedDevice.Guid)
                                {
                                    actualDevice = dev;
                                    break;
                                }
                            }
                        }
                        if (actualDevice != null) break;
                    }
                    if (actualDevice != null) break;
                }
                if (actualDevice != null) break;
            }

            //Assert
            Assert.AreEqual(expectedDevice.Name, actualDevice.Name);
            Assert.AreEqual(expectedDevice.Description, actualDevice.Description);
            Assert.AreEqual(expectedQuantity, actualQuantity);
            Assert.AreEqual(expectedDevice.Cost, actualDevice.Cost);
            Assert.AreEqual(expectedDevice.ConnectionType.Guid, actualDevice.ConnectionType.Guid);
        }

        [TestMethod]
        public void Save_Bid_Remove_Device()
        {
            //Act
            TECSubScope ssToModify = bid.Systems[0].Equipment[0].SubScope[0];
            int oldNumDevices = ssToModify.Devices.Count();
            TECDevice deviceToRemove = ssToModify.Devices[0];

            int numThisDevice = 0;
            foreach (TECDevice dev in ssToModify.Devices)
            {
                if (dev == deviceToRemove)
                {
                    numThisDevice++;
                }
            }

            for (int i = 0; i < numThisDevice; i++)
            {
                ssToModify.Devices.Remove(deviceToRemove);
            }

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECSubScope modifiedSubScope = null;
            foreach (TECSystem sys in actualBid.Systems)
            {
                foreach (TECEquipment equip in sys.Equipment)
                {
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        if (ss.Guid == ssToModify.Guid)
                        {
                            modifiedSubScope = ss;
                            break;
                        }
                    }
                    if (modifiedSubScope != null) break;
                }
                if (modifiedSubScope != null) break;
            }

            //Assert
            foreach (TECDevice dev in modifiedSubScope.Devices)
            {
                if (deviceToRemove.Guid == dev.Guid) Assert.Fail();
            }
            bool devFound = false;
            foreach (TECDevice dev in actualBid.DeviceCatalog)
            {
                if (deviceToRemove.Guid == dev.Guid) devFound = true;
            }
            if (!devFound) Assert.Fail();

            Assert.AreEqual(bid.DeviceCatalog.Count(), actualBid.DeviceCatalog.Count());
            Assert.AreEqual((oldNumDevices - numThisDevice), modifiedSubScope.Devices.Count);
        }

        [TestMethod]
        public void Save_Bid_LowerQuantity_Device()
        {
            //Act
            TECSubScope ssToModify = bid.Systems[0].Equipment[0].SubScope[0];

            TECDevice deviceToRemove = ssToModify.Devices[0];

            int oldNumDevices = 0;
            
            foreach (TECDevice dev in ssToModify.Devices)
            {
                if (dev.Guid == deviceToRemove.Guid) oldNumDevices++;
            }

            ssToModify.Devices.Remove(deviceToRemove);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECSubScope modifiedSubScope = null;
            foreach (TECSystem sys in actualBid.Systems)
            {
                foreach (TECEquipment equip in sys.Equipment)
                {
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        if (ss.Guid == ssToModify.Guid)
                        {
                            modifiedSubScope = ss;
                            break;
                        }
                    }
                    if (modifiedSubScope != null) break;
                }
                if (modifiedSubScope != null) break;
            }

            //Assert
            bool devFound = false;
            foreach (TECDevice dev in actualBid.DeviceCatalog)
            {
                if (deviceToRemove.Guid == dev.Guid) devFound = true;
            }
            if (!devFound) Assert.Fail();

            Assert.AreEqual(bid.DeviceCatalog.Count(), actualBid.DeviceCatalog.Count());
            Assert.AreEqual((oldNumDevices - 1), modifiedSubScope.Devices.Count);
        }

        #region Edit Device
        [TestMethod]
        public void Save_Bid_Device_Quantity()
        {
            //Act
            TECSubScope ssToModify = bid.Systems[0].Equipment[0].SubScope[0];
            TECDevice expectedDevice = ssToModify.Devices[0];

            int expectedNumDevices = 0;

            foreach (TECDevice dev in ssToModify.Devices)
            {
                if (dev.Guid == expectedDevice.Guid) expectedNumDevices++;
            }

            ssToModify.Devices.Add(new TECDevice(expectedDevice));
            expectedNumDevices++;

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECSubScope modifiedSS = null;
            int actualQuantity = 0;

            foreach (TECSystem sys in actualBid.Systems)
            {
                foreach (TECEquipment equip in sys.Equipment)
                {
                    foreach (TECSubScope ss in equip.SubScope)
                    {

                        if (ss.Guid == ssToModify.Guid)
                        {
                            modifiedSS = ss;
                            break;
                        }
                    }
                    if (modifiedSS != null) break;
                }
                if (modifiedSS != null) break;
            }

            TECDevice actualDevice = null;
            foreach (TECDevice dev in modifiedSS.Devices)
            {
                if (dev.Guid == expectedDevice.Guid)
                {
                    actualQuantity++;
                }
            }
            foreach (TECDevice dev in modifiedSS.Devices)
            {
                if (expectedDevice.Guid == dev.Guid)
                {
                    actualDevice = dev;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedNumDevices, actualQuantity);
        }
        #endregion Edit Device

        #endregion Save Device

        #region Save Point

        [TestMethod]
        public void Save_Bid_Add_Point()
        {
            //Act
            TECPoint expectedPoint = new TECPoint();
            expectedPoint.Type = PointTypes.Serial;
            expectedPoint.Name = "New Point";
            expectedPoint.Description = "Point Description";
            expectedPoint.Quantity = 84300;

            TECSubScope subScopeToModify = bid.Systems[0].Equipment[0].SubScope[0];
            subScopeToModify.Points.Add(expectedPoint);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECPoint actualPoint = null;
            foreach (TECSystem sys in actualBid.Systems)
            {
                foreach (TECEquipment equip in sys.Equipment)
                {
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        if (ss.Guid == subScopeToModify.Guid)
                        {
                            foreach (TECPoint point in ss.Points)
                            {
                                if (expectedPoint.Guid == point.Guid)
                                {
                                    actualPoint = point;
                                    break;
                                }
                            }
                        }
                        if (actualPoint != null) break;
                    }
                    if (actualPoint != null) break;
                }
                if (actualPoint != null) break;
            }

            //Assert
            Assert.AreEqual(expectedPoint.Name, actualPoint.Name);
            Assert.AreEqual(expectedPoint.Description, actualPoint.Description);
            Assert.AreEqual(expectedPoint.Quantity, actualPoint.Quantity);
            Assert.AreEqual(expectedPoint.Type, actualPoint.Type);
        }

        [TestMethod]
        public void Save_Bid_Remove_Point()
        {
            //Act
            TECSubScope ssToModify = bid.Systems[0].Equipment[0].SubScope[0];
            int oldNumPoints = ssToModify.Points.Count();
            TECPoint pointToRemove = ssToModify.Points[0];
            ssToModify.Points.Remove(pointToRemove);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECSubScope modifiedSubScope = null;
            foreach (TECSystem sys in actualBid.Systems)
            {
                foreach (TECEquipment equip in sys.Equipment)
                {
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        if (ss.Guid == ssToModify.Guid)
                        {
                            modifiedSubScope = ss;
                            break;
                        }
                    }
                    if (modifiedSubScope != null) break;
                }
                if (modifiedSubScope != null) break;
            }

            //Assert
            foreach (TECPoint point in modifiedSubScope.Points)
            {
                if (pointToRemove.Guid == point.Guid) Assert.Fail();
            }

            Assert.AreEqual((oldNumPoints - 1), modifiedSubScope.Points.Count);
        }

        #region Edit Point
        [TestMethod]
        public void Save_Bid_Point_Name()
        {
            //Act
            TECPoint expectedPoint = bid.Systems[0].Equipment[0].SubScope[0].Points[0];
            expectedPoint.Name = "Point name save test";
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECPoint actualPoint = null;
            foreach (TECSystem sys in actualBid.Systems)
            {
                foreach (TECEquipment equip in sys.Equipment)
                {
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        foreach (TECPoint point in ss.Points)
                        {
                            if (point.Guid == expectedPoint.Guid)
                            {
                                actualPoint = point;
                                break;
                            }
                        }
                        if (actualPoint != null) break;
                    }
                    if (actualPoint != null) break;
                }
                if (actualPoint != null) break;
            }

            //Assert
            Assert.AreEqual(expectedPoint.Name, actualPoint.Name);
        }

        [TestMethod]
        public void Save_Bid_Point_Description()
        {
            //Act
            TECPoint expectedPoint = bid.Systems[0].Equipment[0].SubScope[0].Points[0];
            expectedPoint.Description = "Point Description save test";
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECPoint actualPoint = null;
            foreach (TECSystem sys in actualBid.Systems)
            {
                foreach (TECEquipment equip in sys.Equipment)
                {
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        foreach (TECPoint point in ss.Points)
                        {
                            if (point.Guid == expectedPoint.Guid)
                            {
                                actualPoint = point;
                                break;
                            }
                        }
                        if (actualPoint != null) break;
                    }
                    if (actualPoint != null) break;
                }
                if (actualPoint != null) break;
            }

            //Assert
            Assert.AreEqual(expectedPoint.Description, actualPoint.Description);
        }

        [TestMethod]
        public void Save_Bid_Point_Quantity()
        {
            //Act
            TECPoint expectedPoint = bid.Systems[0].Equipment[0].SubScope[0].Points[0];
            expectedPoint.Quantity = 7463;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECPoint actualPoint = null;
            foreach (TECSystem sys in actualBid.Systems)
            {
                foreach (TECEquipment equip in sys.Equipment)
                {
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        foreach (TECPoint point in ss.Points)
                        {
                            if (point.Guid == expectedPoint.Guid)
                            {
                                actualPoint = point;
                                break;
                            }
                        }
                        if (actualPoint != null) break;
                    }
                    if (actualPoint != null) break;
                }
                if (actualPoint != null) break;
            }

            //Assert
            Assert.AreEqual(expectedPoint.Quantity, actualPoint.Quantity);
        }

        [TestMethod]
        public void Save_Bid_Point_Type()
        {
            //Act
            TECPoint expectedPoint = bid.Systems[0].Equipment[0].SubScope[0].Points[0];
            expectedPoint.Type = PointTypes.BI;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECPoint actualPoint = null;
            foreach (TECSystem sys in actualBid.Systems)
            {
                foreach (TECEquipment equip in sys.Equipment)
                {
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        foreach (TECPoint point in ss.Points)
                        {
                            if (point.Guid == expectedPoint.Guid)
                            {
                                actualPoint = point;
                                break;
                            }
                        }
                        if (actualPoint != null) break;
                    }
                    if (actualPoint != null) break;
                }
                if (actualPoint != null) break;
            }

            //Assert
            Assert.AreEqual(expectedPoint.Type, actualPoint.Type);
        }
        #endregion Edit Point
        #endregion Save Point

        #region Save Tag
        [TestMethod]
        public void Save_Bid_Add_Tag_ToSystem()
        {
            TECTag tagToAdd = bid.Tags[1];
            TECSystem systemToEdit = bid.Systems[0];

            systemToEdit.Tags.Add(tagToAdd);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid finalBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECSystem finalSystem = null;
            foreach (TECSystem system in finalBid.Systems)
            {
                if (system.Guid == systemToEdit.Guid)
                {
                    finalSystem = system;
                    break;
                }
            }

            bool tagExists = false;
            foreach (TECTag tag in finalSystem.Tags)
            {
                if (tag.Guid == tagToAdd.Guid) { tagExists = true; }
            }

            Assert.IsTrue(tagExists);
        }

        [TestMethod]
        public void Save_Bid_Add_Tag_ToEquipment()
        {
            TECTag tagToAdd = bid.Tags[1];
            TECEquipment equipmentToEdit = bid.Systems[0].Equipment[0];

            equipmentToEdit.Tags.Add(tagToAdd);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid finalBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECEquipment finalEquipment = null;
            foreach (TECSystem system in finalBid.Systems)
            {
                foreach (TECEquipment equipment in system.Equipment)
                {
                    if (equipment.Guid == equipmentToEdit.Guid)
                    {
                        finalEquipment = equipment;
                        break;
                    }
                }
                if (finalEquipment != null)
                {
                    break;
                }
            }

            bool tagExists = false;
            foreach (TECTag tag in finalEquipment.Tags)
            {
                if (tag.Guid == tagToAdd.Guid) { tagExists = true; }
            }

            Assert.IsTrue(tagExists);
        }

        [TestMethod]
        public void Save_Bid_Add_Tag_ToSubScope()
        {
            TECTag tagToAdd = bid.Tags[1];
            TECSubScope subScopeToEdit = bid.Systems[0].Equipment[0].SubScope[0];

            subScopeToEdit.Tags.Add(tagToAdd);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid finalBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECSubScope finalSubScope = null;
            foreach (TECSystem system in finalBid.Systems)
            {
                foreach (TECEquipment equip in system.Equipment)
                {
                    foreach (TECSubScope subScope in equip.SubScope)
                    {
                        if (subScope.Guid == subScopeToEdit.Guid)
                        {
                            finalSubScope = subScope;
                            break;
                        }
                    }
                    if (finalSubScope != null) { break; }
                }
                if (finalSubScope != null) { break; }
            }

            bool tagExists = false;
            foreach (TECTag tag in finalSubScope.Tags)
            {
                if (tag.Guid == tagToAdd.Guid) { tagExists = true; }
            }

            Assert.IsTrue(tagExists);
        }

        [TestMethod]
        public void Save_Bid_Add_Tag_ToPoint()
        {
            TECTag tagToAdd = bid.Tags[1];
            TECPoint PointToEdit = bid.Systems[0].Equipment[0].SubScope[0].Points[0];

            PointToEdit.Tags.Add(tagToAdd);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid finalBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECPoint finalPoint = null;
            foreach (TECSystem system in finalBid.Systems)
            {
                foreach (TECEquipment equip in system.Equipment)
                {
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        foreach (TECPoint point in ss.Points)
                        {
                            if (point.Guid == PointToEdit.Guid)
                            {
                                finalPoint = point;
                                break;
                            }
                        }
                    }
                    if (finalPoint != null) { break; }
                }
                if (finalPoint != null) { break; }
            }

            bool tagExists = false;
            foreach (TECTag tag in finalPoint.Tags)
            {
                if (tag.Guid == tagToAdd.Guid) { tagExists = true; }
            }

            Assert.IsTrue(tagExists);
        }

        [TestMethod]
        public void Save_Bid_Add_Tag_ToController()
        {
            TECTag tagToAdd = bid.Tags[1];
            TECController ControllerToEdit = bid.Controllers[0];

            ControllerToEdit.Tags.Add(tagToAdd);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid finalBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECController finalController = null;
            foreach (TECController Controller in finalBid.Controllers)
            {
                if (Controller.Guid == ControllerToEdit.Guid)
                {
                    finalController = Controller;
                    break;
                }
            }

            bool tagExists = false;
            foreach (TECTag tag in finalController.Tags)
            {
                if (tag.Guid == tagToAdd.Guid) { tagExists = true; }
            }

            Assert.IsTrue(tagExists);
        }

        #endregion Save Tag

        #region Save Scope Branch

        [TestMethod]
        public void Save_Bid_Add_Branch()
        {
            //Act
            int oldNumBranches = bid.ScopeTree.Count();
            TECScopeBranch expectedBranch = new TECScopeBranch();
            expectedBranch.Name = "New Branch";
            expectedBranch.Description = "Branch description";
            bid.ScopeTree.Add(expectedBranch);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECScopeBranch actualBranch = null;
            foreach (TECScopeBranch branch in actualBid.ScopeTree)
            {
                if (branch.Guid == expectedBranch.Guid)
                {
                    actualBranch = branch;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedBranch.Name, actualBranch.Name);
            Assert.AreEqual(expectedBranch.Description, actualBranch.Description);
            Assert.AreEqual((oldNumBranches + 1), actualBid.ScopeTree.Count);
        }

        [TestMethod]
        public void Save_Bid_Add_Branch_InBranch()
        {
            //Act
            TECScopeBranch expectedBranch = new TECScopeBranch();
            expectedBranch.Name = "New Child";
            expectedBranch.Description = "Child Branch Description";
            TECScopeBranch branchToModify = bid.ScopeTree[0];
            branchToModify.Branches.Add(expectedBranch);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECScopeBranch modifiedBranch = null;
            foreach (TECScopeBranch branch in actualBid.ScopeTree)
            {
                if (branch.Guid == branchToModify.Guid)
                {
                    modifiedBranch = branch;
                    break;
                }
            }

            TECScopeBranch actualBranch = null;
            foreach (TECScopeBranch branch in modifiedBranch.Branches)
            {
                if (branch.Guid == expectedBranch.Guid)
                {
                    actualBranch = branch;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedBranch.Name, actualBranch.Name);
            Assert.AreEqual(expectedBranch.Description, actualBranch.Description);
        }

        [TestMethod]
        public void Save_Bid_Remove_Branch()
        {
            //Act
            int oldNumBranches = bid.ScopeTree.Count();
            TECScopeBranch branchToRemove = bid.ScopeTree[0];
            bid.ScopeTree.Remove(branchToRemove);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            //Assert
            foreach(TECScopeBranch branch in actualBid.ScopeTree)
            {
                if (branch.Guid == branchToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual((oldNumBranches - 1), actualBid.ScopeTree.Count);
        }

        [TestMethod]
        public void Save_Bid_Remove_Branch_FromBranch()
        {
            //Act
            TECScopeBranch branchToModify = null;

            foreach (TECScopeBranch branch in bid.ScopeTree)
            {
                if (branch.Branches.Count > 0)
                {
                    branchToModify = branch;
                    break;
                }
            } 

            int oldNumBranches = branchToModify.Branches.Count();
            TECScopeBranch branchToRemove = branchToModify.Branches[0];
            branchToModify.Branches.Remove(branchToRemove);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECScopeBranch modifiedBranch = null;
            foreach (TECScopeBranch branch in actualBid.ScopeTree)
            {
                if (branch.Guid == branchToModify.Guid)
                {
                    modifiedBranch = branch;
                    break;
                }
            }

            //Assert
            foreach (TECScopeBranch branch in modifiedBranch.Branches)
            {
                if (branch.Guid == branchToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual((oldNumBranches - 1), modifiedBranch.Branches.Count);
        }

        [TestMethod]
        public void Save_Bid_Branch_Name()
        {
            TECScopeBranch expectedBranch = bid.ScopeTree[0];
            expectedBranch.Name = "Test Branch Save";

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECScopeBranch actualBranch = null;
            foreach (TECScopeBranch branch in actualBid.ScopeTree)
            {
                if (branch.Guid == expectedBranch.Guid)
                {
                    actualBranch = branch;
                }
            }

            //Assert
            Assert.AreEqual(expectedBranch.Name, actualBranch.Name);
        }

        [TestMethod]
        public void Save_Bid_Branch_Description()
        {
            TECScopeBranch expectedBranch = bid.ScopeTree[0];
            expectedBranch.Description = "Test Branch Save";

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECScopeBranch actualBranch = null;
            foreach (TECScopeBranch branch in actualBid.ScopeTree)
            {
                if (branch.Guid == expectedBranch.Guid)
                {
                    actualBranch = branch;
                }
            }

            //Assert
            Assert.AreEqual(expectedBranch.Description, actualBranch.Description);
        }

        #endregion Save Scope Branch

        #region Save Location
        [TestMethod]
        public void Save_Bid_Add_Location()
        {
            //Act
            TECLocation expectedLocation = new TECLocation();
            expectedLocation.Name = "New Location";
            bid.Locations.Add(expectedLocation);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECLocation actualLocation = null;
            foreach (TECLocation loc in actualBid.Locations)
            {
                if (loc.Guid == expectedLocation.Guid)
                {
                    actualLocation = loc;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedLocation.Name, actualLocation.Name);
            Assert.AreEqual(expectedLocation.Guid, actualLocation.Guid);
        }

        [TestMethod]
        public void Save_Bid_Remove_Location()
        {
            //Act
            int oldNumLocations = bid.Locations.Count;
            TECLocation locationToRemove = bid.Locations[0];
            bid.Locations.Remove(locationToRemove);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            //Assert
            foreach (TECLocation loc in actualBid.Locations)
            {
                if (loc.Guid == locationToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual((oldNumLocations - 1), actualBid.Locations.Count);
        }

        [TestMethod]
        public void Save_Bid_Edit_Location_Name()
        {
            //Act
            TECLocation expectedLocation = bid.Locations[0];
            expectedLocation.Name = "Location Name Save";

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECLocation actualLocation = null;
            foreach (TECLocation loc in actualBid.Locations)
            {
                if (loc.Guid == expectedLocation.Guid)
                {
                    actualLocation = loc;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedLocation.Name, actualLocation.Name);
        }

        [TestMethod]
        public void Save_Bid_Add_Location_ToScope()
        {
            //Act
            TECLocation expectedLocation = bid.Locations[0];

            TECSystem sysToModify = null;
            foreach (TECSystem sys in bid.Systems)
            {
                if (sys.Description == "No Location")
                {
                    sysToModify = sys;
                    break;
                }
            }
            TECEquipment equipToModify = sysToModify.Equipment[0];
            TECSubScope ssToModify = equipToModify.SubScope[0];

            sysToModify.Location = expectedLocation;
            equipToModify.Location = expectedLocation;
            ssToModify.Location = expectedLocation;

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECLocation actualLocation = null;
            foreach (TECLocation loc in actualBid.Locations)
            {
                if (loc.Guid == expectedLocation.Guid)
                {
                    actualLocation = loc;
                    break;
                }
            }

            TECSystem actualSystem = null;
            foreach (TECSystem sys in actualBid.Systems)
            {
                if (sys.Guid == sysToModify.Guid)
                {
                    actualSystem = sys;
                    break;
                }
            }
            TECEquipment actualEquip = actualSystem.Equipment[0];
            TECSubScope actualSS = actualEquip.SubScope[0];

            //Assert
            Assert.AreEqual(expectedLocation.Name, actualLocation.Name);
            Assert.AreEqual(expectedLocation.Guid, actualLocation.Guid);

            Assert.AreEqual(actualLocation, actualSystem.Location);
            Assert.AreEqual(actualLocation, actualEquip.Location);
            Assert.AreEqual(actualLocation, actualSS.Location);
        }

        [TestMethod]
        public void Save_Bid_Remove_Location_FromScope()
        {
            //Act
            int expectedNumLocations = bid.Locations.Count;

            TECSystem expectedSys = null;
            foreach (TECSystem sys in bid.Systems)
            {
                if (sys.Description == "Locations all the way")
                {
                    expectedSys = sys;
                    break;
                }
            }
            TECEquipment expectedEquip = expectedSys.Equipment[0];
            TECSubScope expectedSS = expectedEquip.SubScope[0];

            expectedSys.Location = null;
            expectedEquip.Location = null;
            expectedSS.Location = null;

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            int actualNumLocations = actualBid.Locations.Count;

            TECSystem actualSys = null;
            foreach (TECSystem sys in actualBid.Systems)
            {
                if (sys.Guid == expectedSys.Guid)
                {
                    actualSys = sys;
                    break;
                }
            }
            TECEquipment actualEquip = actualSys.Equipment[0];
            TECSubScope actualSS = actualEquip.SubScope[0];

            //Assert
            Assert.AreEqual(expectedNumLocations, actualNumLocations);

            Assert.IsNull(actualSys.Location);
            Assert.IsNull(actualEquip.Location);
            Assert.IsNull(actualSS.Location);
        }

        [TestMethod]
        public void Save_Bid_Edit_Location_InScope()
        {
            //Act
            int expectedNumLocations = bid.Locations.Count;

            TECLocation expectedLocation = null;
            foreach (TECLocation loc in bid.Locations)
            {
                if (loc.Name == "Cellar")
                {
                    expectedLocation = loc;
                    break;
                }
            }

            TECSystem expectedSystem = null;
            foreach (TECSystem sys in bid.Systems)
            {
                if (sys.Name == "System 1")
                {
                    expectedSystem = sys;
                    break;
                }
            }

            expectedSystem.Location = expectedLocation;

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            int actualNumLocations = actualBid.Locations.Count;

            TECLocation actualLocation = null;
            foreach (TECLocation loc in actualBid.Locations)
            {
                if (loc.Guid == expectedLocation.Guid)
                {
                    actualLocation = loc;
                    break;
                }
            }

            TECSystem actualSystem = null;
            foreach (TECSystem sys in actualBid.Systems)
            {
                if (sys.Guid == expectedSystem.Guid)
                {
                    actualSystem = sys;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedNumLocations, actualNumLocations);

            Assert.AreEqual(expectedLocation.Name, actualLocation.Name);
            Assert.AreEqual(actualLocation, actualSystem.Location);
        }
        #endregion Save Location

        #region Save Note
        [TestMethod]
        public void Save_Bid_Add_Note()
        {
            //Act
            TECNote expectedNote = new TECNote();
            expectedNote.Text = "New Note";
            bid.Notes.Add(expectedNote);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECNote actualNote = null;
            foreach (TECNote note in actualBid.Notes)
            {
                if (note.Guid == expectedNote.Guid)
                {
                    actualNote = note;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedNote.Text, actualNote.Text);
        }

        [TestMethod]
        public void Save_Bid_Remove_Note()
        {
            //Act
            int oldNumNotes = bid.Notes.Count;
            TECNote noteToRemove = bid.Notes[0];
            bid.Notes.Remove(noteToRemove);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            //Assert
            foreach (TECNote note in actualBid.Notes)
            {
                if (note.Guid == noteToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual((oldNumNotes - 1), bid.Notes.Count);
        }

        [TestMethod]
        public void Save_Bid_Note_Text()
        {
            //Act
            TECNote expectedNote = bid.Notes[0];
            expectedNote.Text = "Test Save Text";

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECNote actualNote = null;
            foreach (TECNote note in actualBid.Notes)
            {
                if (note.Guid == expectedNote.Guid)
                {
                    actualNote = note;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedNote.Text, actualNote.Text);
        }
        #endregion Save Note

        #region Save Exclusion

        [TestMethod]
        public void Save_Bid_Add_Exclusion()
        {
            //Act
            TECExclusion expectedExclusion = new TECExclusion();
            expectedExclusion.Text = "New Exclusion";
            bid.Exclusions.Add(expectedExclusion);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECExclusion actualExclusion = null;
            foreach (TECExclusion Exclusion in actualBid.Exclusions)
            {
                if (Exclusion.Guid == expectedExclusion.Guid)
                {
                    actualExclusion = Exclusion;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedExclusion.Text, actualExclusion.Text);
        }

        [TestMethod]
        public void Save_Bid_Remove_Exclusion()
        {
            //Act
            int oldNumExclusions = bid.Exclusions.Count;
            TECExclusion ExclusionToRemove = bid.Exclusions[0];
            bid.Exclusions.Remove(ExclusionToRemove);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            //Assert
            foreach (TECExclusion Exclusion in actualBid.Exclusions)
            {
                if (Exclusion.Guid == ExclusionToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual((oldNumExclusions - 1), bid.Exclusions.Count);
        }

        [TestMethod]
        public void Save_Bid_Exclusion_Text()
        {
            //Act
            TECExclusion expectedExclusion = bid.Exclusions[0];
            expectedExclusion.Text = "Test Save Text";

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECExclusion actualExclusion = null;
            foreach (TECExclusion Exclusion in actualBid.Exclusions)
            {
                if (Exclusion.Guid == expectedExclusion.Guid)
                {
                    actualExclusion = Exclusion;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedExclusion.Text, actualExclusion.Text);
        }
        #endregion Save Exclusion

        #region Save Drawing
        //[TestMethod]
        //public void Save_Bid_Add_Drawing()
        //{
        //    //Act
        //    TECDrawing expectedDrawing = PDFConverter.convertPDFToDrawing(TestHelper.TestPDF2);
        //    expectedDrawing.Name = "New Drawing";
        //    expectedDrawing.Description = "New Drawing Description";

        //    bid.Drawings.Add(expectedDrawing);

        //    EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

        //    TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

        //    TECDrawing actualDrawing = null;
        //    foreach (TECDrawing drawing in actualBid.Drawings)
        //    {
        //        if (drawing.Guid == expectedDrawing.Guid)
        //        {
        //            actualDrawing = drawing;
        //            break;
        //        }
        //    }

        //    //Assert
        //    Assert.AreEqual(expectedDrawing.Name, actualDrawing.Name);
        //    Assert.AreEqual(expectedDrawing.Description, actualDrawing.Description);
        //    Assert.AreEqual(expectedDrawing.Pages.Count, actualDrawing.Pages.Count);

        //    byte[] expectedBytes = File.ReadAllBytes(expectedDrawing.Pages[0].Path);
        //    byte[] actualBytes = File.ReadAllBytes(actualDrawing.Pages[0].Path);

        //    Assert.AreEqual(expectedBytes.Length, actualBytes.Length);

        //    bool pagesAreEqual = true;
        //    int i = 0;
        //    foreach (byte b in expectedBytes)
        //    {
        //        if (b != actualBytes[i])
        //        {
        //            pagesAreEqual = false;
        //            break;
        //        }
        //        i++;
        //    }

        //    Assert.IsTrue(pagesAreEqual);
        //}
        #endregion Save Drawing

        #region Save Visual Scope
        //[TestMethod]
        //public void Save_Bid_Add_VS()
        //{
        //    //Act
        //    TECScope expectedScope = bid.Systems[0];
        //    TECVisualScope expectedVS = new TECVisualScope(expectedScope, 15, 743);
        //    bid.Drawings[0].Pages[0].PageScope.Add(expectedVS);

        //    EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

        //    TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

        //    TECVisualScope actualVS = null;
        //    foreach (TECVisualScope vs in actualBid.Drawings[0].Pages[0].PageScope)
        //    {
        //        if (expectedVS.Guid == vs.Guid)
        //        {
        //            actualVS = vs;
        //            break;
        //        }
        //    }

        //    //Assert
        //    Assert.AreEqual(expectedScope.Guid, actualVS.Scope.Guid);
        //    Assert.AreEqual(expectedVS.X, actualVS.X);
        //    Assert.AreEqual(expectedVS.Y, actualVS.Y);
        //}

        //[TestMethod]
        //public void Save_Bid_Remove_VS()
        //{
        //    //Act
        //    TECPage pageToModify = bid.Drawings[0].Pages[0];
        //    int oldNumVS = pageToModify.PageScope.Count;
        //    TECVisualScope vsToRemove = pageToModify.PageScope[0];
        //    bid.Drawings[0].Pages[0].PageScope.Remove(vsToRemove);

        //    EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

        //    TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

        //    TECPage actualPage = null;
        //    foreach (TECDrawing drawing in bid.Drawings)
        //    {
        //        foreach (TECPage page in drawing.Pages)
        //        {
        //            if (page.Guid == pageToModify.Guid)
        //            {
        //                actualPage = page;
        //                break;
        //            }
        //        }
        //        if (actualPage != null)
        //        {
        //            break;
        //        }
        //    }

        //    //Assert
        //    foreach (TECVisualScope vs in actualPage.PageScope)
        //    {
        //        if (vs.Guid == vsToRemove.Guid) Assert.Fail();
        //    }

        //    Assert.AreEqual((oldNumVS - 1), actualPage.PageScope.Count);
        //}
        #endregion Save Visual Scope

        #region Save Controller
        [TestMethod]
        public void Save_Bid_Add_Controller()
        {
            //Act
            TECController expectedController = new TECController(Guid.NewGuid());
            expectedController.Name = "Test Add Controller";
            expectedController.Description = "Test description";
            expectedController.Cost = 100;
            expectedController.Manufacturer = bid.ManufacturerCatalog[0];

            bid.Controllers.Add(expectedController);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECController actualController = null;
            foreach (TECController controller in actualBid.Controllers)
            {
                if (controller.Guid == expectedController.Guid)
                {
                    actualController = controller;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedController.Name, actualController.Name);
            Assert.AreEqual(expectedController.Description, actualController.Description);
            Assert.AreEqual(expectedController.Cost, actualController.Cost);
        }

        [TestMethod]
        public void Save_Bid_Remove_Controller()
        {
            //Act
            int oldNumControllers = bid.Controllers.Count;
            TECController controllerToRemove = bid.Controllers[0];

            bid.Controllers.Remove(controllerToRemove);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            //Assert
            foreach (TECController controller in actualBid.Controllers)
            {
                if (controller.Guid == controllerToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual((oldNumControllers - 1), actualBid.Controllers.Count);

        }
        
        [TestMethod]
        public void Save_Bid_Controller_Name()
        {
            //Act
            TECController expectedController = bid.Controllers[0];
            expectedController.Name = "Test save controller name";
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECController actualController = null;
            foreach (TECController controller in actualBid.Controllers)
            {
                if (controller.Guid == expectedController.Guid)
                {
                    actualController = controller;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedController.Name, actualController.Name);
        }

        [TestMethod]
        public void Save_Bid_Controller_Description()
        {
            //Act
            TECController expectedController = bid.Controllers[0];
            expectedController.Description = "Save Device Description";
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECController actualController = null;
            foreach (TECController controller in actualBid.Controllers)
            {
                if (controller.Guid == expectedController.Guid)
                {
                    actualController = controller;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedController.Description, actualController.Description);
        }

        [TestMethod]
        public void Save_Bid_Controller_Cost()
        {
            //Act
            TECController expectedController = bid.Controllers[0];
            expectedController.Cost = 46.89;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECController actualController = null;
            foreach (TECController controller in actualBid.Controllers)
            {
                if (controller.Guid == expectedController.Guid)
                {
                    actualController = controller;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedController.Cost, actualController.Cost);
        }


        #region Controller IO
        [TestMethod]
        public void Save_Bid_Controller_Add_IO()
        {
            var watchTotal = System.Diagnostics.Stopwatch.StartNew();
            //Act
            TECController expectedController = bid.Controllers[0];
            var testio = new TECIO();
            testio.Type = IOType.BACnetIP;
            expectedController.IO.Add(testio);
            bool hasBACnetIP = false;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);
            watch.Stop();
            Console.WriteLine(" UpdateBidToDD: " + watch.ElapsedMilliseconds);
            watch = System.Diagnostics.Stopwatch.StartNew();
            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            watch.Stop();
            Console.WriteLine(" LoadDBToBid: " + watch.ElapsedMilliseconds);
            TECController actualController = null;
            foreach (TECController controller in actualBid.Controllers)
            {
                if (controller.Guid == expectedController.Guid)
                {
                    actualController = controller;
                    break;
                }
            }

            //Assert
            foreach (TECIO io in actualController.IO)
            {
                if (io.Type == IOType.BACnetIP)
                {
                    hasBACnetIP = true;
                }
            }
            watchTotal.Stop();
            Console.WriteLine(" Test Total: " + watchTotal.ElapsedMilliseconds);

            Assert.IsTrue(hasBACnetIP);
        }

        [TestMethod]
        public void Save_Bid_Controller_Remove_IO()
        {
            //Act
            TECController expectedController = bid.Controllers[0];
            int oldNumIO = expectedController.IO.Count;
            TECIO ioToRemove = expectedController.IO[0];

            expectedController.IO.Remove(ioToRemove);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECController actualController = null;
            foreach (TECController con in actualBid.Controllers)
            {
                if (con.Guid == expectedController.Guid)
                {
                    actualController = con;
                    break;
                }
            }

            //Assert
            foreach (TECIO io in actualController.IO)
            {
                if (io.Type == ioToRemove.Type) { Assert.Fail(); }
            }

            Assert.AreEqual((oldNumIO - 1), actualController.IO.Count);
        }

        [TestMethod]
        public void Save_Bid_Controller_IO_Quantity()
        {
            //Act
            TECController expectedController = bid.Controllers[0];
            TECIO ioToChange = expectedController.IO[0];
            ioToChange.Quantity = 69;

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECController actualController = null;
            foreach (TECController con in actualBid.Controllers)
            {
                if (con.Guid == expectedController.Guid)
                {
                    actualController = con;
                    break;
                }
            }

            //Assert
            foreach (TECIO io in actualController.IO)
            {
                if (io.Type == ioToChange.Type)
                {
                    Assert.AreEqual(ioToChange.Quantity, io.Quantity);
                    break;
                }
            }
        }
        #endregion Controller IO


        #endregion

        #region Save Proposal Scope

        [TestMethod]
        public void Save_Bid_ProposalScope_IsProposed()
        {
            //Act
            TECProposalScope expectedPropScope = bid.ProposalScope[0];
            expectedPropScope.IsProposed = !expectedPropScope.IsProposed;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECProposalScope actualPropScope = null;
            foreach (TECProposalScope propScope in actualBid.ProposalScope)
            {
                if (propScope.Scope.Guid == expectedPropScope.Scope.Guid)
                {
                    actualPropScope = propScope;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedPropScope.IsProposed, actualPropScope.IsProposed);
        }

        [TestMethod]
        public void Save_Bid_ProposalScope_IsProposed_InProposalScope()
        {
            //Act
            TECProposalScope expectedPropScope = bid.ProposalScope[0].Children[0];
            expectedPropScope.IsProposed = !expectedPropScope.IsProposed;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECProposalScope actualPropScope = null;
            foreach (TECProposalScope propScope in actualBid.ProposalScope)
            {
                foreach (TECProposalScope child in propScope.Children)
                {
                    if (child.Scope.Guid == expectedPropScope.Scope.Guid)
                    {
                        actualPropScope = child;
                        break;
                    }
                }
                if (actualPropScope != null) break;
            }

            //Assert
            Assert.AreEqual(expectedPropScope.IsProposed, actualPropScope.IsProposed);
        }

        [TestMethod]
        public void Save_Bid_ProposalScope_Add_Note()
        {
            //Act
            TECProposalScope expectedPropScope = bid.ProposalScope[0];
            int oldNumNotes = expectedPropScope.Notes.Count;
            TECScopeBranch expectedNote = new TECScopeBranch();
            expectedNote.Name = "Added Prop Note";
            expectedPropScope.Notes.Add(expectedNote);
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECProposalScope actualPropScope = null;
            foreach (TECProposalScope propScope in actualBid.ProposalScope)
            {
                if (propScope.Scope.Guid == expectedPropScope.Scope.Guid)
                {
                    actualPropScope = propScope;
                    break;
                }
            }

            TECScopeBranch actualNote = null;
            foreach (TECScopeBranch note in actualPropScope.Notes)
            {
                if (note.Guid == expectedNote.Guid)
                {
                    actualNote = note;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedNote.Name, actualNote.Name);
            Assert.AreEqual((oldNumNotes + 1), actualPropScope.Notes.Count);
        }

        [TestMethod]
        public void Save_Bid_ProposalScope_Add_Note_InNote()
        {
            //Act
            TECScopeBranch noteToModify = null;
            foreach (TECProposalScope propScope in bid.ProposalScope)
            {
                if (propScope.Notes.Count > 0)
                {
                    noteToModify = propScope.Notes[0];
                    break;
                }
            }
            int oldNumNotes = noteToModify.Branches.Count;
            TECScopeBranch expectedNote = new TECScopeBranch();
            expectedNote.Name = "Added Prop Note Note";
            noteToModify.Branches.Add(expectedNote);
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECScopeBranch actualNote = null;
            foreach (TECProposalScope propScope in actualBid.ProposalScope)
            {
                foreach (TECScopeBranch note in propScope.Notes)
                {
                    if (note.Guid == noteToModify.Guid)
                    {
                        Assert.AreEqual((oldNumNotes + 1), note.Branches.Count);
                        foreach (TECScopeBranch noteNote in note.Branches)
                        {
                            if (noteNote.Guid == expectedNote.Guid)
                            {
                                actualNote = noteNote;
                                break;
                            }
                        }
                    }
                    if (actualNote != null) { break; }
                }
                if (actualNote != null) { break; }
            }

            //Assert
            Assert.AreEqual(expectedNote.Name, actualNote.Name);

        }

        [TestMethod]
        public void Save_Bid_ProposalScope_Remove_Note()
        {
            //Act
            TECProposalScope propScopeToModify = null;
            foreach (TECProposalScope propScope in bid.ProposalScope)
            {
                if (propScope.Notes.Count > 0)
                {
                    propScopeToModify = propScope;
                    break;
                }
            }
            int numOldNotes = propScopeToModify.Notes.Count;
            TECScopeBranch noteToRemove = propScopeToModify.Notes[0];
            propScopeToModify.Notes.Remove(noteToRemove);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECProposalScope actualPropScope = null;
            foreach (TECProposalScope propScope in actualBid.ProposalScope)
            {
                if (propScope.Scope.Guid == propScopeToModify.Scope.Guid)
                {
                    actualPropScope = propScope;
                    break;
                }
            }

            //Assert
            foreach (TECScopeBranch note in actualPropScope.Notes)
            {
                if (note.Guid == noteToRemove.Guid) { Assert.Fail(); }
            }

            Assert.AreEqual((numOldNotes - 1), actualPropScope.Notes.Count);
        }

        [TestMethod]
        public void Save_Bid_ProposalScope_Edit_Note()
        {
            //Act
            TECProposalScope expectedPropScope = null;
            foreach (TECProposalScope propScope in bid.ProposalScope)
            {
                if (propScope.Notes.Count > 0)
                {
                    expectedPropScope = propScope;
                    break;
                }
            }

            TECScopeBranch noteToModify = expectedPropScope.Notes[0];
            noteToModify.Name = "Edited note";

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECProposalScope actualPropScope = null;
            foreach (TECProposalScope propScope in actualBid.ProposalScope)
            {
                if (propScope.Scope.Guid == expectedPropScope.Scope.Guid)
                {
                    actualPropScope = propScope;
                    break;
                }
            }

            TECScopeBranch actualNote = null;
            foreach (TECScopeBranch note in actualPropScope.Notes)
            {
                if (note.Guid == noteToModify.Guid)
                {
                    actualNote = noteToModify;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(noteToModify.Name, actualNote.Name);
            Assert.IsNotNull(actualNote);
        }

        #endregion Save Proposal Scope
    }
}
