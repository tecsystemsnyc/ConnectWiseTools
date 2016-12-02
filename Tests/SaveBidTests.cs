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
        TECBid bid;
        ChangeStack testStack;
        string path;

        [TestInitialize]
        public void TestInitialize()
        {
            //Arrange
            bid = TestHelper.CreateTestBid();
            testStack = new ChangeStack(bid);
            path = Path.GetTempFileName();
            File.Delete(path);
            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".bdb";
            EstimatingLibraryDatabase.SaveBidToNewDB(path, bid);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete(path);
            //Console.WriteLine("SaveBid test bid: " + path);
        }

        #region Save BidInfo
        [TestMethod]
        public void Save_BidInfo_Name()
        {
            //Act
            string expectedName = "Save Name";
            bid.Name = expectedName;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            string actualEstimator = actualBid.Estimator;

            //Assert
            Assert.AreEqual(expectedEstimator, actualEstimator);
        }
        #endregion Save BidInfo

        #region Save Labor
        [TestMethod]
        public void Save_Labor_PMCoef()
        {
            //Act
            double expectedPM = 0.123;
            bid.Labor.PMCoef = expectedPM;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            double actualPM = actualBid.Labor.PMCoef;

            //Assert
            Assert.AreEqual(expectedPM, actualPM);
        }

        #endregion Save Labor

        #region Save System
        [TestMethod]
        public void Save_Bid_Add_System()
        {
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
        }

        [TestMethod]
        public void Save_Bid_Remove_System()
        {
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
        }

        #region Edit System
        [TestMethod]
        public void Save_Bid_System_Name()
        {
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
        }

        [TestMethod]
        public void Save_Bid_System_Description()
        {
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
        }

        [TestMethod]
        public void Save_Bid_System_Quantity()
        {
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
        }

        [TestMethod]
        public void Save_Bid_System_BudgetPrice()
        {
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
        }
        #endregion Edit System
        #endregion Save System

        #region Save Equipment
        [TestMethod]
        public void Save_Bid_Add_Equipment()
        {
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
        }

        [TestMethod]
        public void Save_Bid_Remove_Equipment()
        {
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
        }

        #region Edit Equipment
        [TestMethod]
        public void Save_Bid_Equipment_Name()
        {
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
        }

        [TestMethod]
        public void Save_Bid_Equipment_Description()
        {
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
        }

        [TestMethod]
        public void Save_Bid_Equipment_Quantity()
        {
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
        }

        [TestMethod]
        public void Save_Bid_Equipment_BudgetPrice()
        {
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
        }

        #endregion Edit Equipment

        #endregion Save Equipment

        #region Save SubScope
        [TestMethod]
        public void Save_Bid_Add_SubScope()
        {
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
        }

        [TestMethod]
        public void Save_Bid_Remove_SubScope()
        {
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
        }

        #region Edit SubScope
        [TestMethod]
        public void Save_Bid_SubScope_Name()
        {
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
        }

        [TestMethod]
        public void Save_Bid_SubScope_Description()
        {
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
        }

        [TestMethod]
        public void Save_Bid_SubScope_Quantity()
        {
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
            expectedDevice.Quantity = 5;

            TECSubScope subScopeToModify = bid.Systems[0].Equipment[0].SubScope[0];

            //Makes a copy, as devices can only be added via drag drop.
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
        }

        [TestMethod]
        public void Save_Bid_Remove_Device()
        {
            //Act
            TECSubScope ssToModify = bid.Systems[0].Equipment[0].SubScope[0];
            int oldNumDevices = ssToModify.Devices.Count();
            TECDevice deviceToRemove = ssToModify.Devices[0];

            ssToModify.Devices.Remove(deviceToRemove);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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
            Assert.AreEqual((oldNumDevices - 1), modifiedSubScope.Devices.Count);
        }

        #region Edit Device
        [TestMethod]
        public void Save_Bid_Device_Quantity()
        {
            //Act
            TECSubScope ssToModify = bid.Systems[0].Equipment[0].SubScope[0];
            TECDevice expectedDevice = ssToModify.Devices[0];
            expectedDevice.Quantity = 465456456;
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

            TECBid actualBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());

            TECSubScope modifiedSS = null;
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
                if (expectedDevice.Guid == dev.Guid)
                {
                    actualDevice = dev;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedDevice.Quantity, actualDevice.Quantity);
        }
        #endregion Edit Device

        #endregion Save Device

        #region Save Point

        [TestMethod]
        public void Save_Bid_Add_Point()
        {
            //Act
            TECPoint expectedPoint = new TECPoint(PointTypes.Serial, "New Point", "Point Description");
            expectedPoint.Quantity = 84300;

            TECSubScope subScopeToModify = bid.Systems[0].Equipment[0].SubScope[0];
            subScopeToModify.Points.Add(expectedPoint);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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
            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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

        #region Save Location
        [TestMethod]
        public void Save_Bid_Add_Location()
        {
            //Act
            TECLocation expectedLocation = new TECLocation("New Location");
            bid.Locations.Add(expectedLocation);

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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

            EstimatingLibraryDatabase.UpdateBidToDB(path, testStack);

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
    }
}
