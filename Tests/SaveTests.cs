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
using static Tests.TestHelper;

namespace Tests
{
    [TestClass]
    public class SaveTests
    {
        [TestMethod]
        public void SaveAs_Bid()
        {
            //Arrange
            TECBid expectedBid = CreateTestBid();
            TECSystem expectedSystem = expectedBid.Systems[0];
            TECEquipment expectedEquipment = expectedSystem.Equipment[0];
            TECSubScope expectedSubScope = expectedEquipment.SubScope[0];
            TECDevice expectedDevice = expectedSubScope.Devices[0];
            TECPoint expectedPoint = expectedSubScope.Points[0];

            string path = Path.GetTempFileName();

            //Act
            EstimatingLibraryDatabase.SaveBidToNewDB(path, expectedBid);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            TECSystem actualSystem = actualBid.Systems[0];
            TECEquipment actualEquipment = actualSystem.Equipment[0];
            TECSubScope actualSubScope = actualEquipment.SubScope[0];
            TECDevice actualDevice = actualSubScope.Devices[0];
            TECPoint actualPoint = actualSubScope.Points[0];

            //Assert
            Assert.AreEqual(expectedBid.Name, actualBid.Name);
            Assert.AreEqual(expectedBid.BidNumber, actualBid.BidNumber);
            Assert.AreEqual(expectedBid.DueDate, actualBid.DueDate);
            Assert.AreEqual(expectedBid.Salesperson, actualBid.Salesperson);
            Assert.AreEqual(expectedBid.Estimator, actualBid.Estimator);

            Assert.AreEqual(expectedSystem.Name, actualSystem.Name);
            Assert.AreEqual(expectedSystem.Description, actualSystem.Description);
            Assert.AreEqual(expectedSystem.Quantity, actualSystem.Quantity);
            Assert.AreEqual(expectedSystem.BudgetPrice, actualSystem.BudgetPrice);

            Assert.AreEqual(expectedEquipment.Name, actualEquipment.Name);
            Assert.AreEqual(expectedEquipment.Description, actualEquipment.Description);
            Assert.AreEqual(expectedEquipment.Quantity, actualEquipment.Quantity);
            Assert.AreEqual(expectedEquipment.BudgetPrice, actualEquipment.BudgetPrice);

            Assert.AreEqual(expectedSubScope.Name, actualSubScope.Name);
            Assert.AreEqual(expectedSubScope.Description, actualSubScope.Description);
            Assert.AreEqual(expectedSubScope.Quantity, actualSubScope.Quantity);

            Assert.AreEqual(expectedDevice.Name, actualDevice.Name);
            Assert.AreEqual(expectedDevice.Description, actualDevice.Description);
            Assert.AreEqual(expectedDevice.Quantity, actualDevice.Quantity);
            Assert.AreEqual(expectedDevice.Cost, actualDevice.Cost);
            Assert.AreEqual(expectedDevice.Wire, actualDevice.Wire);

            Assert.AreEqual(expectedPoint.Name, actualPoint.Name);
            Assert.AreEqual(expectedPoint.Description, actualPoint.Description);
            Assert.AreEqual(expectedPoint.Quantity, actualPoint.Quantity);
            Assert.AreEqual(expectedPoint.Type, actualPoint.Type);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        #region Save BidInfo
        [TestMethod]
        public void Save_BidInfo_Name()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            string expectedName = "Save Name";
            bid.Name = expectedName;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            string actualName = actualBid.Name;

            //Assert
            Assert.AreEqual(expectedName, actualName);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        [TestMethod]
        public void Save_BidInfo_BidNo()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            string expectedBidNo = "Save BidNo";
            bid.BidNumber = expectedBidNo;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            string actualBidNo = actualBid.BidNumber;

            //Assert
            Assert.AreEqual(expectedBidNo, actualBidNo);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        [TestMethod]
        public void Save_BidInfo_DueDate()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            DateTime expectedDueDate = DateTime.Now;
            bid.DueDate = expectedDueDate;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            DateTime actualDueDate = actualBid.DueDate;

            //Assert
            Assert.AreEqual(expectedDueDate, actualDueDate);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        [TestMethod]
        public void Save_BidInfo_Salesperson()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            string expectedSalesperson = "Save Salesperson";
            bid.Salesperson = expectedSalesperson;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            string actualSalesperson = actualBid.Salesperson;

            //Assert
            Assert.AreEqual(expectedSalesperson, actualSalesperson);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        [TestMethod]
        public void Save_BidInfo_Estimator()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            string expectedEstimator = "Save Estimator";
            bid.Estimator = expectedEstimator;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            string actualEstimator = actualBid.Estimator;

            //Assert
            Assert.AreEqual(expectedEstimator, actualEstimator);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }
        #endregion Save BidInfo

        #region Save System
        [TestMethod]
        public void Save_Bid_Add_System()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            TECSystem expectedSystem = new TECSystem("New system", "New system desc", 123.5, new ObservableCollection<TECEquipment>());
            expectedSystem.Quantity = 1235;

            bid.Systems.Add(expectedSystem);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        [TestMethod]
        public void Save_Bid_Remove_System()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            int oldNumSystems = bid.Systems.Count;
            TECSystem systemToRemove = bid.Systems[0];

            bid.Systems.Remove(systemToRemove);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        #region Edit System
        [TestMethod]
        public void Save_Bid_System_Name()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            TECSystem expectedSystem = bid.Systems[0];
            expectedSystem.Name = "Save System Name";
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        [TestMethod]
        public void Save_Bid_System_Description()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            TECSystem expectedSystem = bid.Systems[0];
            expectedSystem.Description = "Save System Description";
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        [TestMethod]
        public void Save_Bid_System_Quantity()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            TECSystem expectedSystem = bid.Systems[0];
            expectedSystem.Quantity = 987654321;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        [TestMethod]
        public void Save_Bid_System_BudgetPrice()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            TECSystem expectedSystem = bid.Systems[0];
            expectedSystem.BudgetPrice = 9876543.21;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }
        #endregion Edit System
        #endregion Save System

        #region Save Equipment
        [TestMethod]
        public void Save_Bid_Add_Equipment()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            TECEquipment expectedEquipment = new TECEquipment("New Equipment", "New Description", 465543.54, new ObservableCollection<TECSubScope>());
            expectedEquipment.Quantity = 46554354;

            bid.Systems[0].Equipment.Add(expectedEquipment);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        [TestMethod]
        public void Save_Bid_Remove_Equipment()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            TECSystem systemToModify = bid.Systems[0];
            int oldNumEquip = systemToModify.Equipment.Count();
            TECEquipment equipToRemove = systemToModify.Equipment[0];

            systemToModify.Equipment.Remove(equipToRemove);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        #region Edit Equipment
        [TestMethod]
        public void Save_Bid_Equipment_Name()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            TECEquipment expectedEquip = bid.Systems[0].Equipment[0];
            expectedEquip.Name = "Save Equip Name";
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        [TestMethod]
        public void Save_Bid_Equipment_Description()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            TECEquipment expectedEquip = bid.Systems[0].Equipment[0];
            expectedEquip.Description = "Save Equip Description";
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        [TestMethod]
        public void Save_Bid_Equipment_Quantity()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            TECEquipment expectedEquip = bid.Systems[0].Equipment[0];
            expectedEquip.Quantity = 987654321;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        [TestMethod]
        public void Save_Bid_Equipment_BudgetPrice()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            TECEquipment expectedEquip = bid.Systems[0].Equipment[0];
            expectedEquip.BudgetPrice = 9876543.21;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        #endregion Edit Equipment

        #endregion Save Equipment

        #region Save SubScope
        [TestMethod]
        public void Save_Bid_Add_SubScope()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            TECSubScope expectedSubScope = new TECSubScope("New SubScope", "New Description", new ObservableCollection<TECDevice>(), new ObservableCollection<TECPoint>());
            expectedSubScope.Quantity = 235746543;

            bid.Systems[0].Equipment[0].SubScope.Add(expectedSubScope);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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

            //Cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        [TestMethod]
        public void Save_Bid_Remove_SubScope()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            TECEquipment equipToModify = bid.Systems[0].Equipment[0];
            int oldNumSubScope = equipToModify.SubScope.Count();
            TECSubScope subScopeToRemove = equipToModify.SubScope[0];

            equipToModify.SubScope.Remove(subScopeToRemove);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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

            //Cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        #region Edit SubScope
        [TestMethod]
        public void Save_Bid_SubScope_Name()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            TECSubScope expectedSubScope = bid.Systems[0].Equipment[0].SubScope[0];
            expectedSubScope.Name = "Save SubScope Name";
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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

            //Cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        [TestMethod]
        public void Save_Bid_SubScope_Description()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            TECSubScope expectedSubScope = bid.Systems[0].Equipment[0].SubScope[0];
            expectedSubScope.Description = "Save SubScope Description";
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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

            //Cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        [TestMethod]
        public void Save_Bid_SubScope_Quantity()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            TECSubScope expectedSubScope = bid.Systems[0].Equipment[0].SubScope[0];
            expectedSubScope.Quantity = 987654321;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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

            //Cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }
        #endregion Edit SubScope
        #endregion Save SubScope

        #region Save Device

        [TestMethod]
        public void Save_Bid_Add_Device()
        {
            //Arrange
            TECBid bid = CreateTestBid();
            ChangeStack testStack = new ChangeStack(bid);
            string path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);

            //Act
            TECDevice expectedDevice = null;
            foreach (TECDevice dev in bid.DeviceCatalog)
            {
                if (dev.Name == "Device C1")
                {
                    expectedDevice = dev;
                    break;
                }
            }
            expectedDevice.Quantity = 5;

            TECSubScope subScopeToModify = bid.Systems[0].Equipment[0].SubScope[0];

            subScopeToModify.Devices.Add(new TECDevice(expectedDevice));

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECDevice actualDevice = null;
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
            Assert.AreEqual(expectedDevice.Quantity, actualDevice.Quantity);
            Assert.AreEqual(expectedDevice.Cost, actualDevice.Cost);
            Assert.AreEqual(expectedDevice.Wire, actualDevice.Wire);

            //Cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
        }

        [TestMethod]
        public void Save_Bid_Remove_Device()
        {
            Assert.Fail();
        }

        #endregion Save Device

        #region Save Point

        [TestMethod]
        public void Save_Bid_Add_Point()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void Save_Bid_Remove_Point()
        {
            Assert.Fail();
        }
        #endregion Save Point
    }
}
