using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.ViewModels;
using static Tests.CostTestingUtilities;

namespace Tests
{
    [TestClass]
    public class ItemSummaryVMTests
    {
        #region Hardware Summary VM
        [TestMethod]
        public void AddControllerTypeToHardwareVM()
        {
            //Arrange
            HardwareSummaryVM hardwareVM = new HardwareSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECControllerType controllerType = catalogs.ControllerTypes.RandomObject();

            //Act
            List<CostObject> deltas = hardwareVM.AddHardware(controllerType);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            Total assocTECTotal = new Total();
            Total assocElecTotal = new Total();
            foreach (TECCost cost in controllerType.AssociatedCosts)
            {
                assocTECTotal += CalculateTotal(cost, CostType.TEC);
                assocElecTotal += CalculateTotal(cost, CostType.Electrical);
            }

            Total controllerTypeTotalTEC = CalculateTotal(controllerType, CostType.TEC);
            Total controllerTypeTotalElec = CalculateTotal(controllerType, CostType.Electrical);

            //Assert
            //Check hardware properties
            Assert.AreEqual(controllerType.ExtendedCost, hardwareVM.HardwareCost, DELTA, "HardwareCost property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(controllerType.Labor, hardwareVM.HardwareLabor, DELTA, "HardwareLabor property in HardwareSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(assocTECTotal.Cost, hardwareVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(assocTECTotal.Labor, hardwareVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(assocElecTotal.Cost, hardwareVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(assocElecTotal.Labor, hardwareVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal property in HardwareSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(controllerTypeTotalTEC.Cost, tecDelta.Cost, DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(controllerTypeTotalTEC.Labor, tecDelta.Labor, DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(controllerTypeTotalElec.Cost, elecDelta.Cost, DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(controllerTypeTotalElec.Labor, elecDelta.Labor, DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void RemoveControllerTypeFromHardwareVM()
        {
            //Arrange
            HardwareSummaryVM hardwareVM = new HardwareSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECControllerType controllerType = catalogs.ControllerTypes.RandomObject();

            //Act
            hardwareVM.AddHardware(controllerType);
            List<CostObject> deltas = hardwareVM.RemoveHardware(controllerType);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            Total controllerTypeTotalTEC = CalculateTotal(controllerType, CostType.TEC);
            Total controllerTypeTotalElec = CalculateTotal(controllerType, CostType.Electrical);

            //Assert
            //Check hardware properties
            Assert.AreEqual(0, hardwareVM.HardwareCost, DELTA, "HardwareCost property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.HardwareLabor, DELTA, "HardwareLabor property in HardwareSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(0, hardwareVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal property in HardwareSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(-controllerTypeTotalTEC.Cost, tecDelta.Cost, DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(-controllerTypeTotalTEC.Labor, tecDelta.Labor, DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(-controllerTypeTotalElec.Cost, elecDelta.Cost, DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(-controllerTypeTotalElec.Labor, elecDelta.Labor, DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void AddPanelTypeToHardwareVM()
        {
            //Arrange
            HardwareSummaryVM hardwareVM = new HardwareSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECPanelType panelType = catalogs.PanelTypes.RandomObject();

            //Act
            List<CostObject> deltas = hardwareVM.AddHardware(panelType);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            Total assocTECTotal = new Total();
            Total assocElecTotal = new Total();
            foreach (TECCost cost in panelType.AssociatedCosts)
            {
                assocTECTotal += CalculateTotal(cost, CostType.TEC);
                assocElecTotal += CalculateTotal(cost, CostType.Electrical);
            }

            Total panelTypeTotalTEC = CalculateTotal(panelType, CostType.TEC);
            Total panelTypeTotalElec = CalculateTotal(panelType, CostType.Electrical);

            //Assert
            //Check hardware properties
            Assert.AreEqual(panelType.ExtendedCost, hardwareVM.HardwareCost, DELTA, "HardwareCost property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(panelType.Labor, hardwareVM.HardwareLabor, DELTA, "HardwareLabor property in HardwareSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(assocTECTotal.Cost, hardwareVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(assocTECTotal.Labor, hardwareVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(assocElecTotal.Cost, hardwareVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(assocElecTotal.Labor, hardwareVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal property in HardwareSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(panelTypeTotalTEC.Cost, tecDelta.Cost, DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(panelTypeTotalTEC.Labor, tecDelta.Labor, DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(panelTypeTotalElec.Cost, elecDelta.Cost, DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(panelTypeTotalElec.Labor, elecDelta.Labor, DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void RemovePanelTypeFromHardwareVM()
        {
            //Arrange
            HardwareSummaryVM hardwareVM = new HardwareSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECPanelType panelType = catalogs.PanelTypes.RandomObject();

            //Act
            hardwareVM.AddHardware(panelType);
            List<CostObject> deltas = hardwareVM.RemoveHardware(panelType);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            Total panelTypeTotalTEC = CalculateTotal(panelType, CostType.TEC);
            Total panelTypeTotalElec = CalculateTotal(panelType, CostType.Electrical);

            //Assert
            //Check hardware properties
            Assert.AreEqual(0, hardwareVM.HardwareCost, DELTA, "HardwareCost property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.HardwareLabor, DELTA, "HardwareLabor property in HardwareSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(0, hardwareVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal property in HardwareSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(-panelTypeTotalTEC.Cost, tecDelta.Cost, DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(-panelTypeTotalTEC.Labor, tecDelta.Labor, DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(-panelTypeTotalElec.Cost, elecDelta.Cost, DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(-panelTypeTotalElec.Labor, elecDelta.Labor, DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void AddDeviceToHardwareVM()
        {
            //Arrange
            HardwareSummaryVM hardwareVM = new HardwareSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECDevice device = catalogs.Devices.RandomObject();

            //Act
            List<CostObject> deltas = hardwareVM.AddHardware(device);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            Total assocTECTotal = new Total();
            Total assocElecTotal = new Total();
            foreach (TECCost cost in device.AssociatedCosts)
            {
                assocTECTotal += CalculateTotal(cost, CostType.TEC);
                assocElecTotal += CalculateTotal(cost, CostType.Electrical);
            }

            Total deviceTotalTEC = CalculateTotal(device, CostType.TEC);
            Total deviceTotalElec = CalculateTotal(device, CostType.Electrical);

            //Assert
            //Check hardware properties
            Assert.AreEqual(device.ExtendedCost, hardwareVM.HardwareCost, DELTA, "HardwareCost property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(device.Labor, hardwareVM.HardwareLabor, DELTA, "HardwareLabor property in HardwareSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(assocTECTotal.Cost, hardwareVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(assocTECTotal.Labor, hardwareVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(assocElecTotal.Cost, hardwareVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(assocElecTotal.Labor, hardwareVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal property in HardwareSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(deviceTotalTEC.Cost, tecDelta.Cost, DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(deviceTotalTEC.Labor, tecDelta.Labor, DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(deviceTotalElec.Cost, elecDelta.Cost, DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(deviceTotalElec.Labor, elecDelta.Labor, DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void RemoveDeviceFromHardwareVM()
        {
            //Arrange
            HardwareSummaryVM hardwareVM = new HardwareSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECDevice device = catalogs.Devices.RandomObject();

            //Act
            hardwareVM.AddHardware(device);
            List<CostObject> deltas = hardwareVM.RemoveHardware(device);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            Total deviceTotalTEC = CalculateTotal(device, CostType.TEC);
            Total deviceTotalElec = CalculateTotal(device, CostType.Electrical);

            //Assert
            //Check hardware properties
            Assert.AreEqual(0, hardwareVM.HardwareCost, DELTA, "HardwareCost property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.HardwareLabor, DELTA, "HardwareLabor property in HardwareSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(0, hardwareVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal property in HardwareSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(-deviceTotalTEC.Cost, tecDelta.Cost, DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(-deviceTotalTEC.Labor, tecDelta.Labor, DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(-deviceTotalElec.Cost, elecDelta.Cost, DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(-deviceTotalElec.Labor, elecDelta.Labor, DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void AddTECCostToHardwareVM()
        {
            //Arrange
            HardwareSummaryVM hardwareVM = new HardwareSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECCost tecCost = null;
            foreach (TECCost cost in catalogs.AssociatedCosts)
            {
                if (cost.Type == CostType.TEC)
                {
                    tecCost = cost;
                    break;
                }
            }

            //Act
            List<CostObject> deltas = hardwareVM.AddCost(tecCost);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            //Assert
            //Check hardware properties
            Assert.AreEqual(0, hardwareVM.HardwareCost, DELTA, "HardwareCost property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.HardwareLabor, DELTA, "HardwareLabor property in HardwareSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(tecCost.Cost, hardwareVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(tecCost.Labor, hardwareVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal property in HardwareSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(tecCost.Cost, tecDelta.Cost, DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(tecCost.Labor, tecDelta.Labor, DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(0, elecDelta.Cost, DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(0, elecDelta.Labor, DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void RemoveTECCostFromHardwareVM()
        {
            //Arrange
            HardwareSummaryVM hardwareVM = new HardwareSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECCost tecCost = null;
            foreach (TECCost cost in catalogs.AssociatedCosts)
            {
                if (cost.Type == CostType.TEC)
                {
                    tecCost = cost;
                    break;
                }
            }

            //Act
            hardwareVM.AddCost(tecCost);
            List<CostObject> deltas = hardwareVM.RemoveCost(tecCost);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            //Assert
            //Check hardware properties
            Assert.AreEqual(0, hardwareVM.HardwareCost, DELTA, "HardwareCost property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.HardwareLabor, DELTA, "HardwareLabor property in HardwareSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(0, hardwareVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal property in HardwareSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(-tecCost.Cost, tecDelta.Cost, DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(-tecCost.Labor, tecDelta.Labor, DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(0, elecDelta.Cost, DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(0, elecDelta.Labor, DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void AddElecCostToHardwareVM()
        {
            //Arrange
            HardwareSummaryVM hardwareVM = new HardwareSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECCost elecCost = null;
            foreach (TECCost cost in catalogs.AssociatedCosts)
            {
                if (cost.Type == CostType.Electrical)
                {
                    elecCost = cost;
                    break;
                }
            }

            //Act
            List<CostObject> deltas = hardwareVM.AddCost(elecCost);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            //Assert
            //Check hardware properties
            Assert.AreEqual(0, hardwareVM.HardwareCost, DELTA, "HardwareCost property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.HardwareLabor, DELTA, "HardwareLabor property in HardwareSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(0, hardwareVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(elecCost.Cost, hardwareVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(elecCost.Labor, hardwareVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal property in HardwareSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(0, tecDelta.Cost, DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(0, tecDelta.Labor, DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(elecCost.Cost, elecDelta.Cost, DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(elecCost.Labor, elecDelta.Labor, DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void RemoveElecCostFromHardwareVM()
        {
            //Arrange
            HardwareSummaryVM hardwareVM = new HardwareSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECCost elecCost = null;
            foreach (TECCost cost in catalogs.AssociatedCosts)
            {
                if (cost.Type == CostType.Electrical)
                {
                    elecCost = cost;
                    break;
                }
            }

            //Act
            hardwareVM.AddCost(elecCost);
            List<CostObject> deltas = hardwareVM.RemoveCost(elecCost);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            //Assert
            //Check hardware properties
            Assert.AreEqual(0, hardwareVM.HardwareCost, DELTA, "HardwareCost property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.HardwareLabor, DELTA, "HardwareLabor property in HardwareSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(0, hardwareVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal property in HardwareSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(0, tecDelta.Cost, DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(0, tecDelta.Labor, DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(-elecCost.Cost, elecDelta.Cost, DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(-elecCost.Labor, elecDelta.Labor, DELTA, "Returned Elec labor delta is wrong.");
        }
        #endregion

        #region Length Summary VM
        [TestMethod]
        public void AddElectricalMaterialRunToLengthVM()
        {
            //Arrange
            LengthSummaryVM lengthVM = new LengthSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            List<TECElectricalMaterial> elecMats = new List<TECElectricalMaterial>();
            elecMats.AddRange(catalogs.ConnectionTypes);
            elecMats.AddRange(catalogs.ConduitTypes);
            TECElectricalMaterial elecMat = elecMats.RandomObject();
            double length = TestHelper.RandomDouble(1, 100);

            //Act
            List<CostObject> deltas = lengthVM.AddRun(elecMat, length);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            double expectedLengthCost = elecMat.Cost * length;
            double expectedLengthLabor = elecMat.Labor * length;

            Total expectedAssocTECTotal = new Total();
            Total expectedAssocElecTotal = new Total();
            foreach (TECCost cost in elecMat.AssociatedCosts)
            {
                expectedAssocTECTotal += CalculateTotal(cost, CostType.TEC);
                expectedAssocElecTotal += CalculateTotal(cost, CostType.Electrical);
            }

            Total expectedRatedTECTotal = new Total();
            Total expectedRatedElecTotal = new Total();
            foreach (TECCost cost in elecMat.RatedCosts)
            {
                expectedRatedTECTotal += CalculateTotal(cost, CostType.TEC);
                expectedRatedElecTotal += CalculateTotal(cost, CostType.Electrical);
            }
            expectedRatedTECTotal *= length;
            expectedRatedElecTotal *= length;

            double expectedTECCostDelta = expectedAssocTECTotal.Cost + expectedRatedTECTotal.Cost;
            double expectedTECLaborDelta = expectedAssocTECTotal.Labor + expectedRatedTECTotal.Labor;
            double expectedElecCostDelta = expectedLengthCost + expectedAssocElecTotal.Cost + expectedRatedElecTotal.Cost;
            double expectedElecLaborDelta = expectedLengthLabor + expectedAssocElecTotal.Labor + expectedRatedElecTotal.Labor;

            //Assert
            //Check length properties
            Assert.AreEqual(expectedLengthCost, lengthVM.LengthCostTotal, DELTA, "LengthCostTotal property in LengthSummaryVM is wrong.");
            Assert.AreEqual(expectedLengthLabor, lengthVM.LengthLaborTotal, DELTA, "LengthLaborTotal property in LengthSummaryVM is wrong.");
            //Check assoc properties
            Assert.AreEqual(expectedAssocTECTotal.Cost, lengthVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal property in LengthSummaryVM is wrong.");
            Assert.AreEqual(expectedAssocTECTotal.Labor, lengthVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal property in LengthSummaryVM is wrong.");
            Assert.AreEqual(expectedAssocElecTotal.Cost, lengthVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal property in LengthSummaryVM is wrong.");
            Assert.AreEqual(expectedAssocElecTotal.Labor, lengthVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal property in LengthSummaryVM is wrong.");
            //Check rated properties
            Assert.AreEqual(expectedRatedTECTotal.Cost, lengthVM.RatedTECCostTotal, DELTA, "RatedTECCostTotal property in LengthSummaryVM is wrong.");
            Assert.AreEqual(expectedRatedTECTotal.Labor, lengthVM.RatedTECLaborTotal, DELTA, "RatedTECLaborTotal property in LengthSummaryVM is wrong.");
            Assert.AreEqual(expectedRatedElecTotal.Cost, lengthVM.RatedElecCostTotal, DELTA, "RatedElecCostTotal property in LengthSummaryVM is wrong.");
            Assert.AreEqual(expectedRatedElecTotal.Labor, lengthVM.RatedElecLaborTotal, DELTA, "RatedElecLaborTotal property in LengthSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(expectedTECCostDelta, tecDelta.Cost, DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(expectedTECLaborDelta, tecDelta.Labor, DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(expectedElecCostDelta, elecDelta.Cost, DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(expectedElecLaborDelta, elecDelta.Labor, DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void RemoveElectricalMaterialRunFromLengthVM()
        {
            //Arrange
            LengthSummaryVM lengthVM = new LengthSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            List<TECElectricalMaterial> elecMats = new List<TECElectricalMaterial>();
            elecMats.AddRange(catalogs.ConnectionTypes);
            elecMats.AddRange(catalogs.ConduitTypes);
            TECElectricalMaterial elecMat = elecMats.RandomObject();
            double addLength = TestHelper.RandomDouble(1, 100);
            double removeLength = TestHelper.RandomDouble(1, addLength);
            double length = addLength - removeLength;

            //Act
            lengthVM.AddLength(elecMat, addLength);
            List<CostObject> deltas = lengthVM.RemoveLength(elecMat, removeLength);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            double expectedLengthCost = elecMat.Cost * length;
            double expectedLengthLabor = elecMat.Labor * length;
            double removedLengthCost = elecMat.Cost * removeLength;
            double removedLengthLabor = elecMat.Labor * removeLength;

            Total removedAssocTECTotal = new Total();
            Total removedAssocElecTotal = new Total();
            foreach (TECCost cost in elecMat.AssociatedCosts)
            {
                removedAssocTECTotal += CalculateTotal(cost, CostType.TEC);
                removedAssocElecTotal += CalculateTotal(cost, CostType.Electrical);
            }
            removedAssocTECTotal *= 1;
            removedAssocElecTotal *= 1;

            Total ratedTECTotal = new Total();
            Total ratedElecTotal = new Total();
            foreach (TECCost cost in elecMat.RatedCosts)
            {
                ratedTECTotal += CalculateTotal(cost, CostType.TEC);
                ratedElecTotal += CalculateTotal(cost, CostType.Electrical);
            }
            Total expectedRatedTECTotal = ratedTECTotal * length;
            Total expectedRatedElecTotal = ratedElecTotal * length;
            Total removedRatedTECTotal = ratedTECTotal * removeLength;
            Total removedRatedElecTotal = ratedTECTotal * removeLength;

            double expectedTECCostDelta = (-removedAssocTECTotal.Cost + -removedRatedTECTotal.Cost);
            double expectedTECLaborDelta = (-removedAssocTECTotal.Labor + -removedRatedTECTotal.Labor);
            double expectedElecCostDelta = (-removedAssocElecTotal.Cost + -removedRatedElecTotal.Cost + -removedLengthCost);
            double expectedElecLaborDelta = (-removedAssocElecTotal.Labor + -removedRatedElecTotal.Labor + -removedLengthLabor);

            //Assert
            //Check length properties
            Assert.AreEqual(expectedLengthCost, lengthVM.LengthCostTotal, DELTA, "LengthCostTotal property in LengthSummaryVM is wrong.");
            Assert.AreEqual(expectedLengthLabor, lengthVM.LengthLaborTotal, DELTA, "LengthLaborTotal property in LengthSummaryVM is wrong.");
            //Check assoc properties
            Assert.AreEqual(0, lengthVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal property in LengthSummaryVM is wrong.");
            Assert.AreEqual(0, lengthVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal property in LengthSummaryVM is wrong.");
            Assert.AreEqual(0, lengthVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal property in LengthSummaryVM is wrong.");
            Assert.AreEqual(0, lengthVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal property in LengthSummaryVM is wrong.");
            //Check rated properties
            Assert.AreEqual(expectedRatedTECTotal.Cost, lengthVM.RatedTECCostTotal, DELTA, "RatedTECCostTotal property in LengthSummaryVM is wrong.");
            Assert.AreEqual(expectedRatedTECTotal.Labor, lengthVM.RatedTECLaborTotal, DELTA, "RatedTECLaborTotal property in LengthSummaryVM is wrong.");
            Assert.AreEqual(expectedRatedElecTotal.Cost, lengthVM.RatedElecCostTotal, DELTA, "RatedElecCostTotal property in LengthSummaryVM is wrong.");
            Assert.AreEqual(expectedRatedElecTotal.Labor, lengthVM.RatedElecLaborTotal, DELTA, "RatedElecLaborTotal property in LengthSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(expectedTECCostDelta, tecDelta.Cost, DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(expectedTECLaborDelta, tecDelta.Labor, DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(expectedElecCostDelta, elecDelta.Cost, DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(expectedElecLaborDelta, elecDelta.Labor, DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void AddElectricalMaterialLengthToLengthVM()
        {
            //Arrange
            LengthSummaryVM lengthVM = new LengthSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            List<TECElectricalMaterial> elecMats = new List<TECElectricalMaterial>();
            elecMats.AddRange(catalogs.ConnectionTypes);
            elecMats.AddRange(catalogs.ConduitTypes);
            TECElectricalMaterial elecMat = elecMats.RandomObject();
            double length = TestHelper.RandomDouble(1, 100);

            //Act
            List<CostObject> deltas = lengthVM.AddRun(elecMat, length);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            double expectedLengthCost = elecMat.Cost * length;
            double expectedLengthLabor = elecMat.Labor * length;

            Total expectedRatedTECTotal = new Total();
            Total expectedRatedElecTotal = new Total();
            foreach (TECCost cost in elecMat.RatedCosts)
            {
                expectedRatedTECTotal += CalculateTotal(cost, CostType.TEC);
                expectedRatedElecTotal += CalculateTotal(cost, CostType.Electrical);
            }
            expectedRatedTECTotal *= length;
            expectedRatedElecTotal *= length;

            double expectedTECCostDelta = expectedRatedTECTotal.Cost;
            double expectedTECLaborDelta = expectedRatedTECTotal.Labor;
            double expectedElecCostDelta = expectedLengthCost + expectedRatedElecTotal.Cost;
            double expectedElecLaborDelta = expectedLengthLabor + expectedRatedElecTotal.Labor;

            //Assert
            //Check length properties
            Assert.AreEqual(expectedLengthCost, lengthVM.LengthCostTotal, DELTA, "LengthCostTotal property in LengthSummaryVM is wrong.");
            Assert.AreEqual(expectedLengthLabor, lengthVM.LengthLaborTotal, DELTA, "LengthLaborTotal property in LengthSummaryVM is wrong.");
            //Check assoc properties
            Assert.AreEqual(0, lengthVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal property in LengthSummaryVM is wrong.");
            Assert.AreEqual(0, lengthVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal property in LengthSummaryVM is wrong.");
            Assert.AreEqual(0, lengthVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal property in LengthSummaryVM is wrong.");
            Assert.AreEqual(0, lengthVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal property in LengthSummaryVM is wrong.");
            //Check rated properties
            Assert.AreEqual(expectedRatedTECTotal.Cost, lengthVM.RatedTECCostTotal, DELTA, "RatedTECCostTotal property in LengthSummaryVM is wrong.");
            Assert.AreEqual(expectedRatedTECTotal.Labor, lengthVM.RatedTECLaborTotal, DELTA, "RatedTECLaborTotal property in LengthSummaryVM is wrong.");
            Assert.AreEqual(expectedRatedElecTotal.Cost, lengthVM.RatedElecCostTotal, DELTA, "RatedElecCostTotal property in LengthSummaryVM is wrong.");
            Assert.AreEqual(expectedRatedElecTotal.Labor, lengthVM.RatedElecLaborTotal, DELTA, "RatedElecLaborTotal property in LengthSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(expectedTECCostDelta, tecDelta.Cost, DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(expectedTECLaborDelta, tecDelta.Labor, DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(expectedElecCostDelta, elecDelta.Cost, DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(expectedElecLaborDelta, elecDelta.Labor, DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void RemoveElectricalMaterialLengthFromLengthVM()
        {
            //Arrange
            LengthSummaryVM lengthVM = new LengthSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            List<TECElectricalMaterial> elecMats = new List<TECElectricalMaterial>();
            elecMats.AddRange(catalogs.ConnectionTypes);
            elecMats.AddRange(catalogs.ConduitTypes);
            TECElectricalMaterial elecMat = elecMats.RandomObject();
            double addLength = TestHelper.RandomDouble(1, 100);
            double removeLength = TestHelper.RandomDouble(1, addLength);
            double length = addLength - removeLength;

            //Act
            lengthVM.AddLength(elecMat, addLength);
            List<CostObject> deltas = lengthVM.RemoveRun(elecMat, removeLength);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            double expectedLengthCost = elecMat.Cost * length;
            double expectedLengthLabor = elecMat.Labor * length;
            double removedLengthCost = elecMat.Cost * removeLength;
            double removedLengthLabor = elecMat.Labor * removeLength;

            Total expectedAssocTECTotal = new Total();
            Total expectedAssocElecTotal = new Total();
            foreach (TECCost cost in elecMat.AssociatedCosts)
            {
                expectedAssocTECTotal += CalculateTotal(cost, CostType.TEC);
                expectedAssocElecTotal += CalculateTotal(cost, CostType.Electrical);
            }

            Total ratedTECTotal = new Total();
            Total ratedElecTotal = new Total();
            foreach (TECCost cost in elecMat.RatedCosts)
            {
                ratedTECTotal += CalculateTotal(cost, CostType.TEC);
                ratedElecTotal += CalculateTotal(cost, CostType.Electrical);
            }
            Total expectedRatedTECTotal = ratedTECTotal * length;
            Total expectedRatedElecTotal = ratedElecTotal * length;
            Total removedRatedTECTotal = ratedTECTotal * removeLength;
            Total removedRatedElecTotal = ratedTECTotal * removeLength;

            double expectedTECCostDelta = (-removedRatedTECTotal.Cost);
            double expectedTECLaborDelta = (-removedRatedTECTotal.Labor);
            double expectedElecCostDelta = (-removedRatedElecTotal.Cost + -removedLengthCost);
            double expectedElecLaborDelta = (-removedRatedElecTotal.Labor + -removedLengthLabor);

            //Assert
            //Check length properties
            Assert.AreEqual(expectedLengthCost, lengthVM.LengthCostTotal, DELTA, "LengthCostTotal property in LengthSummaryVM is wrong.");
            Assert.AreEqual(expectedLengthLabor, lengthVM.LengthLaborTotal, DELTA, "LengthLaborTotal property in LengthSummaryVM is wrong.");
            //Check assoc properties
            Assert.AreEqual(expectedAssocTECTotal.Cost, lengthVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal property in LengthSummaryVM is wrong.");
            Assert.AreEqual(expectedAssocTECTotal.Labor, lengthVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal property in LengthSummaryVM is wrong.");
            Assert.AreEqual(expectedAssocElecTotal.Cost, lengthVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal property in LengthSummaryVM is wrong.");
            Assert.AreEqual(expectedAssocElecTotal.Labor, lengthVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal property in LengthSummaryVM is wrong.");
            //Check rated properties
            Assert.AreEqual(expectedRatedTECTotal.Cost, lengthVM.RatedTECCostTotal, DELTA, "RatedTECCostTotal property in LengthSummaryVM is wrong.");
            Assert.AreEqual(expectedRatedTECTotal.Labor, lengthVM.RatedTECLaborTotal, DELTA, "RatedTECLaborTotal property in LengthSummaryVM is wrong.");
            Assert.AreEqual(expectedRatedElecTotal.Cost, lengthVM.RatedElecCostTotal, DELTA, "RatedElecCostTotal property in LengthSummaryVM is wrong.");
            Assert.AreEqual(expectedRatedElecTotal.Labor, lengthVM.RatedElecLaborTotal, DELTA, "RatedElecLaborTotal property in LengthSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(expectedTECCostDelta, tecDelta.Cost, DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(expectedTECLaborDelta, tecDelta.Labor, DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(expectedElecCostDelta, elecDelta.Cost, DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(expectedElecLaborDelta, elecDelta.Labor, DELTA, "Returned Elec labor delta is wrong.");
        }
        #endregion

        #region Misc Costs Summary VM
        [TestMethod]
        public void AddTECCostToMiscCostsVM()
        {
            //Arrange
            MiscCostsSummaryVM miscVM = new MiscCostsSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECCost tecCost = null;
            foreach (TECCost cost in catalogs.AssociatedCosts)
            {
                if (cost.Type == CostType.TEC)
                {
                    tecCost = cost;
                    break;
                }
            }

            //Act
            List<CostObject> deltas = miscVM.AddCost(tecCost);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            //Assert
            //Check Misc properties
            Assert.AreEqual(0, miscVM.MiscTECCostTotal, DELTA, "MiscTECCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.MiscTECLaborTotal, DELTA, "MiscTECLaborTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.MiscElecCostTotal, DELTA, "MiscElecCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.MiscElecLaborTotal, DELTA, "MiscElecLaborTotal in MiscCostsSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(tecCost.Cost, miscVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(tecCost.Labor, miscVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal in MiscCostsSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(tecCost.Cost, tecDelta.Cost, DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(tecCost.Labor, tecDelta.Labor, DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(0, elecDelta.Cost, DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(0, elecDelta.Labor, DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void RemoveTECCostFromMiscCostsVM()
        {
            //Arrange
            MiscCostsSummaryVM miscVM = new MiscCostsSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECCost tecCost = null;
            foreach (TECCost cost in catalogs.AssociatedCosts)
            {
                if (cost.Type == CostType.TEC)
                {
                    tecCost = cost;
                    break;
                }
            }

            //Act
            miscVM.AddCost(tecCost);
            List<CostObject> deltas = miscVM.RemoveCost(tecCost);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            //Assert
            //Check Misc properties
            Assert.AreEqual(0, miscVM.MiscTECCostTotal, DELTA, "MiscTECCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.MiscTECLaborTotal, DELTA, "MiscTECLaborTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.MiscElecCostTotal, DELTA, "MiscElecCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.MiscElecLaborTotal, DELTA, "MiscElecLaborTotal in MiscCostsSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(0, miscVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal in MiscCostsSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(-tecCost.Cost, tecDelta.Cost, DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(-tecCost.Labor, tecDelta.Labor, DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(0, elecDelta.Cost, DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(0, elecDelta.Labor, DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void AddElecCostToMiscCostsVM()
        {
            //Arrange
            MiscCostsSummaryVM miscVM = new MiscCostsSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECCost elecCost = null;
            foreach (TECCost cost in catalogs.AssociatedCosts)
            {
                if (cost.Type == CostType.Electrical)
                {
                    elecCost = cost;
                    break;
                }
            }

            //Act
            List<CostObject> deltas = miscVM.AddCost(elecCost);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            //Assert
            //Check Misc properties
            Assert.AreEqual(0, miscVM.MiscTECCostTotal, DELTA, "MiscTECCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.MiscTECLaborTotal, DELTA, "MiscTECLaborTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.MiscElecCostTotal, DELTA, "MiscElecCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.MiscElecLaborTotal, DELTA, "MiscElecLaborTotal in MiscCostsSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(0, miscVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(elecCost.Cost, miscVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(elecCost.Labor, miscVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal in MiscCostsSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(0, tecDelta.Cost, DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(0, tecDelta.Labor, DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(elecCost.Cost, elecDelta.Cost, DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(elecCost.Labor, elecDelta.Labor, DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void RemoveElecCostFromMiscCostsVM()
        {
            //Arrange
            MiscCostsSummaryVM miscVM = new MiscCostsSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECCost elecCost = null;
            foreach (TECCost cost in catalogs.AssociatedCosts)
            {
                if (cost.Type == CostType.Electrical)
                {
                    elecCost = cost;
                    break;
                }
            }

            //Act
            miscVM.AddCost(elecCost);
            List<CostObject> deltas = miscVM.RemoveCost(elecCost);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            //Assert
            //Check Misc properties
            Assert.AreEqual(0, miscVM.MiscTECCostTotal, DELTA, "MiscTECCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.MiscTECLaborTotal, DELTA, "MiscTECLaborTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.MiscElecCostTotal, DELTA, "MiscElecCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.MiscElecLaborTotal, DELTA, "MiscElecLaborTotal in MiscCostsSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(0, miscVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal in MiscCostsSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(0, tecDelta.Cost, DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(0, tecDelta.Labor, DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(-elecCost.Cost, elecDelta.Cost, DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(-elecCost.Labor, elecDelta.Labor, DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void AddTECMiscToMiscCostsVM()
        {
            //Arrange
            MiscCostsSummaryVM miscVM = new MiscCostsSummaryVM();
            TECMisc tecMisc = new TECMisc();
            tecMisc.Cost = TestHelper.RandomDouble(1, 100);
            tecMisc.Labor = TestHelper.RandomDouble(1, 100);
            tecMisc.Type = CostType.TEC;
            tecMisc.Quantity = TestHelper.RandomInt(2, 10);

            double expectedCost = tecMisc.Cost * tecMisc.Quantity;
            double expectedLabor = tecMisc.Labor * tecMisc.Quantity;

            //Act
            List<CostObject> deltas = miscVM.AddCost(tecMisc);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            //Assert
            //Check Misc properties
            Assert.AreEqual(expectedCost, miscVM.MiscTECCostTotal, DELTA, "MiscTECCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(expectedLabor, miscVM.MiscTECLaborTotal, DELTA, "MiscTECLaborTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.MiscElecCostTotal, DELTA, "MiscElecCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.MiscElecLaborTotal, DELTA, "MiscElecLaborTotal in MiscCostsSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(0, miscVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal in MiscCostsSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(expectedCost, tecDelta.Cost, DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(expectedLabor, tecDelta.Labor, DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(0, elecDelta.Cost, DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(0, elecDelta.Labor, DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void RemoveTECMiscFromMiscCostsVM()
        {
            //Arrange
            MiscCostsSummaryVM miscVM = new MiscCostsSummaryVM();
            TECMisc tecMisc = new TECMisc();
            tecMisc.Cost = TestHelper.RandomDouble(1, 100);
            tecMisc.Labor = TestHelper.RandomDouble(1, 100);
            tecMisc.Type = CostType.TEC;
            tecMisc.Quantity = TestHelper.RandomInt(2, 10);

            double expectedCost = tecMisc.Cost * tecMisc.Quantity;
            double expectedLabor = tecMisc.Labor * tecMisc.Quantity;

            //Act
            miscVM.AddCost(tecMisc);
            List<CostObject> deltas = miscVM.RemoveCost(tecMisc);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            //Assert
            //Check Misc properties
            Assert.AreEqual(0, miscVM.MiscTECCostTotal, DELTA, "MiscTECCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.MiscTECLaborTotal, DELTA, "MiscTECLaborTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.MiscElecCostTotal, DELTA, "MiscElecCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.MiscElecLaborTotal, DELTA, "MiscElecLaborTotal in MiscCostsSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(0, miscVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal in MiscCostsSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(-expectedCost, tecDelta.Cost, DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(-expectedLabor, tecDelta.Labor, DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(0, elecDelta.Cost, DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(0, elecDelta.Labor, DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void AddElecMiscToMiscCostsVM()
        {
            //Arrange
            MiscCostsSummaryVM miscVM = new MiscCostsSummaryVM();
            TECMisc elecMisc = new TECMisc();
            elecMisc.Cost = TestHelper.RandomDouble(1, 100);
            elecMisc.Labor = TestHelper.RandomDouble(1, 100);
            elecMisc.Type = CostType.Electrical;
            elecMisc.Quantity = TestHelper.RandomInt(2, 10);

            double expectedCost = elecMisc.Cost * elecMisc.Quantity;
            double expectedLabor = elecMisc.Labor * elecMisc.Quantity;

            //Act
            List<CostObject> deltas = miscVM.AddCost(elecMisc);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            //Assert
            //Check Misc properties
            Assert.AreEqual(0, miscVM.MiscTECCostTotal, DELTA, "MiscTECCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.MiscTECLaborTotal, DELTA, "MiscTECLaborTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(expectedCost, miscVM.MiscElecCostTotal, DELTA, "MiscElecCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(expectedLabor, miscVM.MiscElecLaborTotal, DELTA, "MiscElecLaborTotal in MiscCostsSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(0, miscVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal in MiscCostsSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(0, tecDelta.Cost, DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(0, tecDelta.Labor, DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(expectedCost, elecDelta.Cost, DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(expectedLabor, elecDelta.Labor, DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void RemoveElecMiscFromMiscCostsVM()
        {
            //Arrange
            MiscCostsSummaryVM miscVM = new MiscCostsSummaryVM();
            TECMisc elecMisc = new TECMisc();
            elecMisc.Cost = TestHelper.RandomDouble(1, 100);
            elecMisc.Labor = TestHelper.RandomDouble(1, 100);
            elecMisc.Type = CostType.TEC;
            elecMisc.Quantity = TestHelper.RandomInt(2, 10);

            double expectedCost = elecMisc.Cost * elecMisc.Quantity;
            double expectedLabor = elecMisc.Labor * elecMisc.Quantity;

            //Act
            miscVM.AddCost(elecMisc);
            List<CostObject> deltas = miscVM.RemoveCost(elecMisc);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            //Assert
            //Check Misc properties
            Assert.AreEqual(0, miscVM.MiscTECCostTotal, DELTA, "MiscTECCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.MiscTECLaborTotal, DELTA, "MiscTECLaborTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.MiscElecCostTotal, DELTA, "MiscElecCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.MiscElecLaborTotal, DELTA, "MiscElecLaborTotal in MiscCostsSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(0, miscVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal in MiscCostsSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(0, tecDelta.Cost, DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(0, tecDelta.Labor, DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(-expectedCost, elecDelta.Cost, DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(-expectedLabor, elecDelta.Labor, DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void ChangeCostQuantityInMiscCostsVM()
        {
            //Arrange
            MiscCostsSummaryVM miscVM = new MiscCostsSummaryVM();
            TECMisc misc = new TECMisc();
            misc.Cost = TestHelper.RandomDouble(1, 100);
            misc.Labor = TestHelper.RandomDouble(1, 100);
            misc.Type = CostType.TEC;
            misc.Quantity = TestHelper.RandomInt(1, 10);

            //Act
            miscVM.AddCost(misc);
            int deltaQuantity = TestHelper.RandomInt(1, 10);
            List<CostObject> deltas = miscVM.ChangeQuantity(misc, deltaQuantity);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            int finalQuantity = misc.Quantity + deltaQuantity;
            double expectedCost = finalQuantity * misc.Cost;
            double expectedLabor = finalQuantity * misc.Labor;

            double expectedDeltaCost = deltaQuantity * misc.Cost;
            double expectedDeltaLabor = deltaQuantity * misc.Labor;

            //Check Misc properties
            Assert.AreEqual(expectedCost, miscVM.MiscTECCostTotal, DELTA, "MiscTECCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(expectedLabor, miscVM.MiscTECLaborTotal, DELTA, "MiscTECLaborTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.MiscElecCostTotal, DELTA, "MiscElecCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.MiscElecLaborTotal, DELTA, "MiscElecLaborTotal in MiscCostsSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(0, miscVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal in MiscCostsSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(expectedDeltaCost, tecDelta.Cost, DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(expectedDeltaLabor, tecDelta.Labor, DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(0, elecDelta.Cost, DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(0, elecDelta.Labor, DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void UpdateCostInMiscCostsVM()
        {
            //Arrange
            MiscCostsSummaryVM miscVM = new MiscCostsSummaryVM();
            TECMisc misc = new TECMisc();
            misc.Cost = TestHelper.RandomDouble(1, 100);
            misc.Labor = TestHelper.RandomDouble(1, 100);
            misc.Type = CostType.TEC;
            misc.Quantity = TestHelper.RandomInt(1, 10);

            //Act
            miscVM.AddCost(misc);
            double deltaCost = TestHelper.RandomDouble(1, 100);
            double deltaLabor = TestHelper.RandomDouble(1, 100);
            misc.Cost += deltaCost;
            misc.Labor += deltaLabor;

            List<CostObject> deltas = miscVM.UpdateCost(misc);
            CostObject tecDelta = new CostObject(0, 0, CostType.TEC);
            CostObject elecDelta = new CostObject(0, 0, CostType.Electrical);
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    tecDelta += delta;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    elecDelta += delta;
                }
            }

            double expectedCost = misc.Cost * misc.Quantity;
            double expectedLabor = misc.Labor * misc.Quantity;

            double expectedDeltaCost = deltaCost * misc.Quantity;
            double expectedDeltaLabor = deltaLabor * misc.Quantity;

            //Assert
            //Check Misc properties
            Assert.AreEqual(expectedCost, miscVM.MiscTECCostTotal, DELTA, "MiscTECCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(expectedLabor, miscVM.MiscTECLaborTotal, DELTA, "MiscTECLaborTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.MiscElecCostTotal, DELTA, "MiscElecCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.MiscElecLaborTotal, DELTA, "MiscElecLaborTotal in MiscCostsSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(0, miscVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal in MiscCostsSummaryVM is wrong.");
            Assert.AreEqual(0, miscVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal in MiscCostsSummaryVM is wrong.");
            //Check returned deltas
            Assert.AreEqual(expectedDeltaCost, tecDelta.Cost, DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(expectedDeltaLabor, tecDelta.Labor, DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(0, elecDelta.Cost, DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(0, elecDelta.Labor, DELTA, "Returned Elec labor delta is wrong.");
        }
        #endregion
    }
}
