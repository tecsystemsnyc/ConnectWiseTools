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

        static TECBid OGBid;
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
            OGBid = TestHelper.CreateTestBid();
            EstimatingLibraryDatabase.SaveNew(OGPath, OGBid);
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

            bid = OGBid.Copy() as TECBid;
            ModelLinkingHelper.LinkBid(bid);
            testStack = new ChangeStack(bid);
            path = Path.GetTempFileName();
            File.Copy(OGPath, path, true);
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

        [ClassCleanup]
        public static void ClassCleanup()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(OGPath);
        }

        #region Save BidInfo
        [TestMethod]
        public void Save_BidInfo_Name()
        {
            //Act
            string expectedName = "Save Name";
            bid.Name = expectedName;
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;
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
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;
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
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;
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
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;
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
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;
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
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;
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
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualbid = EstimatingLibraryDatabase.Load(path) as TECBid;
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
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;
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
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;
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
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualbid = EstimatingLibraryDatabase.Load(path) as TECBid;
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
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;
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
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;
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
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualbid = EstimatingLibraryDatabase.Load(path) as TECBid;
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
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;
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
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;
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
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualbid = EstimatingLibraryDatabase.Load(path) as TECBid;
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
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;
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
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;
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
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualbid = EstimatingLibraryDatabase.Load(path) as TECBid;
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
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;
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
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;
            double actualRate = actualBid.Labor.ElectricalRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }

        [TestMethod]
        public void Save_Bid_Labor_ElecNonUnionRate()
        {
            //Act
            double expectedRate = 0.456;
            bid.Labor.ElectricalNonUnionRate = expectedRate;
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;
            double actualRate = actualBid.Labor.ElectricalNonUnionRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }

        [TestMethod]
        public void Save_Bid_Labor_ElecSuperRate()
        {
            //Act
            double expectedRate = 0.123;
            bid.Labor.ElectricalSuperRate = expectedRate;
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;
            double actualRate = actualBid.Labor.ElectricalSuperRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }

        [TestMethod]
        public void Save_Bid_Labor_ElecSuperNonUnionRate()
        {
            //Act
            double expectedRate = 23.94;
            bid.Labor.ElectricalSuperNonUnionRate = expectedRate;
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;
            double actualRate = actualBid.Labor.ElectricalSuperNonUnionRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }

        [TestMethod]
        public void Save_Bid_Labor_ElecIsOnOT()
        {
            //Act
            bid.Labor.ElectricalIsOnOvertime = !bid.Labor.ElectricalIsOnOvertime;
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

            //Assert
            Assert.AreEqual(bid.Labor.ElectricalIsOnOvertime, actualBid.Labor.ElectricalIsOnOvertime);
        }

        [TestMethod]
        public void Save_Bid_Labor_ElecIsUnion()
        {
            //Act
            bid.Labor.ElectricalIsUnion = !bid.Labor.ElectricalIsUnion;
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

            //Assert
            Assert.AreEqual(bid.Labor.ElectricalIsUnion, actualBid.Labor.ElectricalIsUnion);
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
            expectedSystem.BudgetPriceModifier = 123.5;
            expectedSystem.Quantity = 1235;

            bid.Systems.Add(expectedSystem);

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            Assert.AreEqual(expectedSystem.BudgetPriceModifier, actualSystem.BudgetPriceModifier);
        }

        [TestMethod]
        public void Save_Bid_Remove_System()
        {
            //Act
            int oldNumSystems = bid.Systems.Count;
            TECSystem systemToRemove = bid.Systems[0];

            bid.Systems.Remove(systemToRemove);

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid finalBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            expectedSystem.BudgetPriceModifier = 9876543.21;
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

            TECSystem actualSystem = null;
            foreach (TECSystem system in actualBid.Systems)
            {
                if (system.Guid == expectedSystem.Guid)
                {
                    actualSystem = system;
                }
            }

            //Assert
            Assert.AreEqual(expectedSystem.BudgetPriceModifier, actualSystem.BudgetPriceModifier);
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
            expectedEquipment.BudgetUnitPrice = 465543.54;
            expectedEquipment.Quantity = 46554354;

            bid.Systems[0].Equipment.Add(expectedEquipment);

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            Assert.AreEqual(expectedEquipment.BudgetUnitPrice, actualEquipment.BudgetUnitPrice);
        }

        [TestMethod]
        public void Save_Bid_Remove_Equipment()
        {
            //Act
            TECSystem systemToModify = null; 
            foreach(TECSystem system in bid.Systems)
            {
                if(system.Equipment.Count > 0)
                {
                    systemToModify = system;
                }
            }
            int oldNumEquip = systemToModify.Equipment.Count();
            TECEquipment equipToRemove = systemToModify.Equipment[0];

            systemToModify.Equipment.Remove(equipToRemove);

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid finalBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            TECEquipment expectedEquip = bid.RandomEquipment();
            expectedEquip.Name = "Save Equip Name";
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            TECEquipment expectedEquip = bid.RandomEquipment();
            expectedEquip.Description = "Save Equip Description";
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            TECEquipment expectedEquip = bid.RandomEquipment();
            expectedEquip.Quantity = 987654321;
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            TECEquipment expectedEquip = bid.RandomEquipment();
            expectedEquip.BudgetUnitPrice = 9876543.21;
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            Assert.AreEqual(expectedEquip.BudgetUnitPrice, actualEquip.BudgetUnitPrice);
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
            

            bid.RandomEquipment().SubScope.Add(expectedSubScope);

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            TECEquipment equipToModify = bid.RandomEquipment();
            int oldNumSubScope = equipToModify.SubScope.Count();
            TECSubScope subScopeToRemove = equipToModify.SubScope[0];

            equipToModify.SubScope.Remove(subScopeToRemove);

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            TECSubScope expectedSubScope = bid.RandomSubScope();
            expectedSubScope.Name = "Save SubScope Name";
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            TECSubScope expectedSubScope = bid.RandomSubScope();
            expectedSubScope.Description = "Save SubScope Description";
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            TECSubScope expectedSubScope = bid.RandomSubScope();
            expectedSubScope.Quantity = 987654321;
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            TECDevice expectedDevice = bid.Catalogs.Devices.RandomObject();

            TECSubScope subScopeToModify = bid.RandomSubScope();

            //Makes a copy, as devices can only be added via drag drop.
            subScopeToModify.Devices = new ObservableCollection<TECDevice>();
            int expectedQuantity = 5;
            subScopeToModify.Devices.Add(expectedDevice);
            subScopeToModify.Devices.Add(expectedDevice);
            subScopeToModify.Devices.Add(expectedDevice);
            subScopeToModify.Devices.Add(expectedDevice);
            subScopeToModify.Devices.Add(expectedDevice);

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            Assert.AreEqual(expectedDevice.ConnectionTypes.Count, actualDevice.ConnectionTypes.Count);
        }

        [TestMethod]
        public void Save_Bid_Remove_Device()
        {
            //Act
            TECSubScope ssToModify = bid.RandomSubScope();
            while (ssToModify.Devices.Count == 0)
            {
                ssToModify = bid.RandomSubScope();
            }

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

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
                if (deviceToRemove.Guid == dev.Guid) Assert.Fail("Device not removed properly.");
            }
            bool devFound = false;
            foreach (TECDevice dev in actualBid.Catalogs.Devices)
            {
                if (deviceToRemove.Guid == dev.Guid) devFound = true;
            }
            if (!devFound) Assert.Fail();

            Assert.AreEqual(bid.Catalogs.Devices.Count(), actualBid.Catalogs.Devices.Count());
            Assert.AreEqual((oldNumDevices - numThisDevice), modifiedSubScope.Devices.Count);
        }

        [TestMethod]
        public void Save_Bid_LowerQuantity_Device()
        {
            //Act
            TECSubScope ssToModify = bid.RandomSubScope();
            while (ssToModify.Devices.Count == 0)
            {
                ssToModify = bid.RandomSubScope();
            }

            TECDevice deviceToRemove = ssToModify.Devices[0];

            int oldNumDevices = 0;

            foreach (TECDevice dev in ssToModify.Devices)
            {
                if (dev.Guid == deviceToRemove.Guid) oldNumDevices++;
            }

            ssToModify.Devices.Remove(deviceToRemove);

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            foreach (TECDevice dev in actualBid.Catalogs.Devices)
            {
                if (deviceToRemove.Guid == dev.Guid) devFound = true;
            }
            if (!devFound) Assert.Fail();

            Assert.AreEqual(bid.Catalogs.Devices.Count(), actualBid.Catalogs.Devices.Count());
            Assert.AreEqual((oldNumDevices - 1), modifiedSubScope.Devices.Count);
        }

        #region Edit Device
        [TestMethod]
        public void Save_Bid_Device_Quantity()
        {
            //Act
            TECSubScope ssToModify = bid.RandomSubScope();
            while(ssToModify.Devices.Count == 0)
            {
                ssToModify = bid.RandomSubScope();
            }
            TECDevice expectedDevice = ssToModify.Devices[0];

            int expectedNumDevices = 0;

            foreach (TECDevice dev in ssToModify.Devices)
            {
                if (dev.Guid == expectedDevice.Guid) expectedNumDevices++;
            }

            ssToModify.Devices.Add(new TECDevice(expectedDevice));
            expectedNumDevices++;

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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

            TECSubScope subScopeToModify = bid.RandomSubScope();
            subScopeToModify.Points.Add(expectedPoint);

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            TECSubScope ssToModify = bid.RandomSubScope();
            int oldNumPoints = ssToModify.Points.Count();
            TECPoint pointToRemove = ssToModify.Points[0];
            ssToModify.Points.Remove(pointToRemove);

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            TECPoint expectedPoint = bid.RandomPoint();
            expectedPoint.Name = "Point name save test";
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            TECPoint expectedPoint = bid.RandomPoint();
            expectedPoint.Description = "Point Description save test";
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            TECPoint expectedPoint = bid.RandomPoint();
            expectedPoint.Quantity = 7463;
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            TECPoint expectedPoint = bid.RandomPoint();
            expectedPoint.Type = PointTypes.BI;
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            TECTag tagToAdd = bid.Catalogs.Tags[1];
            TECSystem systemToEdit = bid.Systems.RandomObject();

            systemToEdit.Tags.Add(tagToAdd);

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid finalBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            TECTag tagToAdd = bid.Catalogs.Tags.RandomObject();
            TECEquipment equipmentToEdit = bid.RandomEquipment();

            equipmentToEdit.Tags.Add(tagToAdd);

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid finalBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            TECTag tagToAdd = bid.Catalogs.Tags.RandomObject();
            TECSubScope subScopeToEdit = bid.RandomSubScope();

            subScopeToEdit.Tags.Add(tagToAdd);

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid finalBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            TECTag tagToAdd = bid.Catalogs.Tags.RandomObject();
            TECPoint PointToEdit = bid.RandomPoint();

            PointToEdit.Tags.Add(tagToAdd);

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid finalBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            TECTag tagToAdd = bid.Catalogs.Tags.RandomObject();
            TECController ControllerToEdit = bid.Controllers.RandomObject();

            ControllerToEdit.Tags.Add(tagToAdd);

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid finalBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            TECScopeBranch branchToModify = bid.ScopeTree.RandomObject();
            branchToModify.Branches.Add(expectedBranch);

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            TECScopeBranch branchToRemove = bid.ScopeTree.RandomObject();
            bid.ScopeTree.Remove(branchToRemove);

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

            //Assert
            foreach (TECScopeBranch branch in actualBid.ScopeTree)
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
            TECScopeBranch branchToRemove = branchToModify.Branches.RandomObject();
            branchToModify.Branches.Remove(branchToRemove);

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            TECScopeBranch expectedBranch = bid.ScopeTree.RandomObject();
            expectedBranch.Name = "Test Branch Save";

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            TECScopeBranch expectedBranch = bid.ScopeTree.RandomObject();
            expectedBranch.Description = "Test Branch Save";

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            TECLocation locationToRemove = bid.Locations.RandomObject();
            bid.Locations.Remove(locationToRemove);

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            TECLocation expectedLocation = bid.Locations.RandomObject();
            expectedLocation.Name = "Location Name Save";

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            TECLocation expectedLocation = bid.Locations.RandomObject();

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

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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

        //    EstimatingLibraryDatabase.Update(path, testStack, false);

        //    TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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

        //    EstimatingLibraryDatabase.Update(path, testStack, false);

        //    TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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

        //    EstimatingLibraryDatabase.Update(path, testStack, false);

        //    TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            TECController expectedController = new TECController(Guid.NewGuid(), bid.Catalogs.Manufacturers[0]);
            expectedController.Name = "Test Add Controller";
            expectedController.Description = "Test description";
            expectedController.Cost = 100;

            bid.Controllers.Add(expectedController);

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
            EstimatingLibraryDatabase.Update(path, testStack, false);
            watch.Stop();
            Console.WriteLine(" UpdateBidToDD: " + watch.ElapsedMilliseconds);
            watch = System.Diagnostics.Stopwatch.StartNew();
            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;
            watch.Stop();
            Console.WriteLine(" Load: " + watch.ElapsedMilliseconds);
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

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

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
        
        #region Save Misc Cost
        [TestMethod]
        public void Save_Bid_Add_MiscCost()
        {
            //Act
            TECMisc expectedCost = new TECMisc();
            expectedCost.Name = "Add cost addition";
            expectedCost.Cost = 978.3;
            expectedCost.Quantity = 21;

            bid.MiscCosts.Add(expectedCost);

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

            TECMisc actualCost = null;
            foreach (TECMisc cost in actualBid.MiscCosts)
            {
                if (cost.Guid == expectedCost.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedCost.Name, actualCost.Name);
            Assert.AreEqual(expectedCost.Cost, actualCost.Cost);
            Assert.AreEqual(expectedCost.Quantity, actualCost.Quantity);
        }

        [TestMethod]
        public void Save_Bid_Remove_MiscCost()
        {
            //Act
            TECMisc costToRemove = bid.MiscCosts[0];
            bid.MiscCosts.Remove(costToRemove);

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

            //Assert
            foreach (TECMisc cost in actualBid.MiscCosts)
            {
                if (cost.Guid == costToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual(bid.MiscCosts.Count, actualBid.MiscCosts.Count);
        }

        [TestMethod]
        public void Save_Bid_MiscCost_Name()
        {
            //Act
            TECMisc expectedCost = bid.MiscCosts[0];
            expectedCost.Name = "Test Save Cost Name";

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

            TECMisc actualCost = null;
            foreach (TECMisc cost in actualBid.MiscCosts)
            {
                if (cost.Guid == expectedCost.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedCost.Name, actualCost.Name);
        }

        [TestMethod]
        public void Save_Bid_MiscCost_Cost()
        {
            //Act
            TECMisc expectedCost = bid.MiscCosts[0];
            expectedCost.Cost = 489.1238;

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

            TECMisc actualCost = null;
            foreach (TECMisc cost in actualBid.MiscCosts)
            {
                if (cost.Guid == expectedCost.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedCost.Cost, actualCost.Cost);
        }

        [TestMethod]
        public void Save_Bid_MiscCost_Quantity()
        {
            //Act
            TECMisc expectedCost = bid.MiscCosts[0];
            expectedCost.Quantity = 492;

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

            TECMisc actualCost = null;
            foreach (TECMisc cost in actualBid.MiscCosts)
            {
                if (cost.Guid == expectedCost.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedCost.Quantity, actualCost.Quantity);
        }
        #endregion

        #region Save Panel Type
        [TestMethod]
        public void Save_Bid_Add_PanelType()
        {
            //Act
            TECPanelType expectedCost = new TECPanelType();
            expectedCost.Name = "Add cost addition";
            expectedCost.Cost = 978.3;

            bid.Catalogs.PanelTypes.Add(expectedCost);

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

            TECPanelType actualCost = null;
            foreach (TECPanelType cost in bid.Catalogs.PanelTypes)
            {
                if (cost.Guid == expectedCost.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedCost.Name, actualCost.Name);
            Assert.AreEqual(expectedCost.Cost, actualCost.Cost);
        }
        
        #endregion

        #region Save Panel
        [TestMethod]
        public void Save_Bid_Add_Panel()
        {
            //Act
            TECPanel expectedPanel = new TECPanel(bid.Catalogs.PanelTypes[0]);
            expectedPanel.Name = "Test Add Controller";
            expectedPanel.Description = "Test description";
            bid.Panels.Add(expectedPanel);

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

            TECPanel actualpanel = null;
            foreach (TECPanel panel in actualBid.Panels)
            {
                if (panel.Guid == expectedPanel.Guid)
                {
                    actualpanel = panel;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedPanel.Name, actualpanel.Name);
            Assert.AreEqual(expectedPanel.Description, actualpanel.Description);
        }

        [TestMethod]
        public void Save_Bid_Remove_Panel()
        {
            //Act
            int oldNumPanels = bid.Panels.Count;
            TECPanel panelToRemove = bid.Panels[0];

            bid.Panels.Remove(panelToRemove);

            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

            //Assert
            foreach (TECPanel panel in actualBid.Panels)
            {
                if (panel.Guid == panelToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual((oldNumPanels - 1), actualBid.Panels.Count);

        }

        [TestMethod]
        public void Save_Bid_Panel_Name()
        {
            //Act
            TECPanel expectedPanel = bid.Panels[0];
            expectedPanel.Name = "Test save panel name";
            EstimatingLibraryDatabase.Update(path, testStack, false);

            TECBid actualBid = EstimatingLibraryDatabase.Load(path) as TECBid;

            TECPanel actualPanel = null;
            foreach (TECPanel panel in actualBid.Panels)
            {
                if (panel.Guid == expectedPanel.Guid)
                {
                    actualPanel = panel;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedPanel.Name, actualPanel.Name);
        }
        #endregion

        #region Add Controlled Scope
        [TestMethod]
        public void Save_Bid_Add_ControlledScope()
        {
           
        }
        #endregion

    }
}
