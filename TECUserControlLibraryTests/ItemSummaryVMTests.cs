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
            TECControllerType controllerType = catalogs.ControllerTypes[0];

            //Act
            CostBatch delta = hardwareVM.AddHardware(controllerType);

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
            Assert.AreEqual(controllerType.Cost, hardwareVM.HardwareCost, DELTA, "HardwareCost property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(controllerType.Labor, hardwareVM.HardwareLabor, DELTA, "HardwareLabor property in HardwareSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(assocTECTotal.Cost, hardwareVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(assocTECTotal.Labor, hardwareVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(assocElecTotal.Cost, hardwareVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(assocElecTotal.Labor, hardwareVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal property in HardwareSummaryVM is wrong.");
            //Check returned delta
            Assert.AreEqual(controllerTypeTotalTEC.Cost, delta.GetCost(CostType.TEC), DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(controllerTypeTotalTEC.Labor, delta.GetLabor(CostType.TEC), DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(controllerTypeTotalElec.Cost, delta.GetCost(CostType.Electrical), DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(controllerTypeTotalElec.Labor, delta.GetLabor(CostType.Electrical), DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void RemoveControllerTypeFromHardwareVM()
        {
            //Arrange
            HardwareSummaryVM hardwareVM = new HardwareSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECControllerType controllerType = catalogs.ControllerTypes[0];

            //Act
            hardwareVM.AddHardware(controllerType);
            CostBatch delta = hardwareVM.RemoveHardware(controllerType);

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
            //Check returned delta
            Assert.AreEqual(-controllerTypeTotalTEC.Cost, delta.GetCost(CostType.TEC), DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(-controllerTypeTotalTEC.Labor, delta.GetLabor(CostType.TEC), DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(-controllerTypeTotalElec.Cost, delta.GetCost(CostType.Electrical), DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(-controllerTypeTotalElec.Labor, delta.GetLabor(CostType.Electrical), DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void AddPanelTypeToHardwareVM()
        {
            //Arrange
            HardwareSummaryVM hardwareVM = new HardwareSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECPanelType panelType = catalogs.PanelTypes[0];

            //Act
            CostBatch delta = hardwareVM.AddHardware(panelType);

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
            Assert.AreEqual(panelType.Cost, hardwareVM.HardwareCost, DELTA, "HardwareCost property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(panelType.Labor, hardwareVM.HardwareLabor, DELTA, "HardwareLabor property in HardwareSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(assocTECTotal.Cost, hardwareVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(assocTECTotal.Labor, hardwareVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(assocElecTotal.Cost, hardwareVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(assocElecTotal.Labor, hardwareVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal property in HardwareSummaryVM is wrong.");
            //Check returned delta
            Assert.AreEqual(panelTypeTotalTEC.Cost, delta.GetCost(CostType.TEC), DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(panelTypeTotalTEC.Labor, delta.GetLabor(CostType.TEC), DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(panelTypeTotalElec.Cost, delta.GetCost(CostType.Electrical), DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(panelTypeTotalElec.Labor, delta.GetLabor(CostType.Electrical), DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void RemovePanelTypeFromHardwareVM()
        {
            //Arrange
            HardwareSummaryVM hardwareVM = new HardwareSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECPanelType panelType = catalogs.PanelTypes[0];

            //Act
            hardwareVM.AddHardware(panelType);
            CostBatch delta = hardwareVM.RemoveHardware(panelType);

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
            //Check returned delta
            Assert.AreEqual(-panelTypeTotalTEC.Cost, delta.GetCost(CostType.TEC), DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(-panelTypeTotalTEC.Labor, delta.GetLabor(CostType.TEC), DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(-panelTypeTotalElec.Cost, delta.GetCost(CostType.Electrical), DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(-panelTypeTotalElec.Labor, delta.GetLabor(CostType.Electrical), DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void AddDeviceToHardwareVM()
        {
            //Arrange
            HardwareSummaryVM hardwareVM = new HardwareSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECDevice device = catalogs.Devices[0];

            //Act
            CostBatch delta = hardwareVM.AddHardware(device);

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
            Assert.AreEqual(device.Cost, hardwareVM.HardwareCost, DELTA, "HardwareCost property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(device.Labor, hardwareVM.HardwareLabor, DELTA, "HardwareLabor property in HardwareSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(assocTECTotal.Cost, hardwareVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(assocTECTotal.Labor, hardwareVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(assocElecTotal.Cost, hardwareVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(assocElecTotal.Labor, hardwareVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal property in HardwareSummaryVM is wrong.");
            //Check returned delta
            Assert.AreEqual(deviceTotalTEC.Cost, delta.GetCost(CostType.TEC), DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(deviceTotalTEC.Labor, delta.GetLabor(CostType.TEC), DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(deviceTotalElec.Cost, delta.GetCost(CostType.Electrical), DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(deviceTotalElec.Labor, delta.GetLabor(CostType.Electrical), DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void RemoveDeviceFromHardwareVM()
        {
            //Arrange
            HardwareSummaryVM hardwareVM = new HardwareSummaryVM();
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();
            TECDevice device = catalogs.Devices[0];

            //Act
            hardwareVM.AddHardware(device);
            CostBatch delta = hardwareVM.RemoveHardware(device);

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
            //Check returned delta
            Assert.AreEqual(-deviceTotalTEC.Cost, delta.GetCost(CostType.TEC), DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(-deviceTotalTEC.Labor, delta.GetLabor(CostType.TEC), DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(-deviceTotalElec.Cost, delta.GetCost(CostType.Electrical), DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(-deviceTotalElec.Labor, delta.GetLabor(CostType.Electrical), DELTA, "Returned Elec labor delta is wrong.");
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
            CostBatch delta = hardwareVM.AddCost(tecCost);

            //Assert
            //Check hardware properties
            Assert.AreEqual(0, hardwareVM.HardwareCost, DELTA, "HardwareCost property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.HardwareLabor, DELTA, "HardwareLabor property in HardwareSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(tecCost.Cost, hardwareVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(tecCost.Labor, hardwareVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal property in HardwareSummaryVM is wrong.");
            //Check returned delta
            Assert.AreEqual(tecCost.Cost, delta.GetCost(CostType.TEC), DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(tecCost.Labor, delta.GetLabor(CostType.TEC), DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(0, delta.GetCost(CostType.Electrical), DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(0, delta.GetLabor(CostType.Electrical), DELTA, "Returned Elec labor delta is wrong.");
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
            CostBatch delta = hardwareVM.RemoveCost(tecCost);

            //Assert
            //Check hardware properties
            Assert.AreEqual(0, hardwareVM.HardwareCost, DELTA, "HardwareCost property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.HardwareLabor, DELTA, "HardwareLabor property in HardwareSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(0, hardwareVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal property in HardwareSummaryVM is wrong.");
            //Check returned delta
            Assert.AreEqual(-tecCost.Cost, delta.GetCost(CostType.TEC), DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(-tecCost.Labor, delta.GetLabor(CostType.TEC), DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(0, delta.GetCost(CostType.Electrical), DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(0, delta.GetLabor(CostType.Electrical), DELTA, "Returned Elec labor delta is wrong.");
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
            CostBatch delta = hardwareVM.AddCost(elecCost);

            //Assert
            //Check hardware properties
            Assert.AreEqual(0, hardwareVM.HardwareCost, DELTA, "HardwareCost property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.HardwareLabor, DELTA, "HardwareLabor property in HardwareSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(0, hardwareVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(elecCost.Cost, hardwareVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(elecCost.Labor, hardwareVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal property in HardwareSummaryVM is wrong.");
            //Check returned delta
            Assert.AreEqual(0, delta.GetCost(CostType.TEC), DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(0, delta.GetLabor(CostType.TEC), DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(elecCost.Cost, delta.GetCost(CostType.Electrical), DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(elecCost.Labor, delta.GetLabor(CostType.Electrical), DELTA, "Returned Elec labor delta is wrong.");
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
            CostBatch delta = hardwareVM.RemoveCost(elecCost);

            //Assert
            //Check hardware properties
            Assert.AreEqual(0, hardwareVM.HardwareCost, DELTA, "HardwareCost property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.HardwareLabor, DELTA, "HardwareLabor property in HardwareSummaryVM is wrong.");
            //Check Assoc properties
            Assert.AreEqual(0, hardwareVM.AssocTECCostTotal, DELTA, "AssocTECCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocTECLaborTotal, DELTA, "AssocTECLaborTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecCostTotal, DELTA, "AssocElecCostTotal property in HardwareSummaryVM is wrong.");
            Assert.AreEqual(0, hardwareVM.AssocElecLaborTotal, DELTA, "AssocElecLaborTotal property in HardwareSummaryVM is wrong.");
            //Check returned delta
            Assert.AreEqual(0, delta.GetCost(CostType.TEC), DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(0, delta.GetLabor(CostType.TEC), DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(-elecCost.Cost, delta.GetCost(CostType.Electrical), DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(-elecCost.Labor, delta.GetLabor(CostType.Electrical), DELTA, "Returned Elec labor delta is wrong.");
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
            TECElectricalMaterial elecMat = elecMats[0];
            double length = 123.8;

            //Act
            CostBatch delta = lengthVM.AddRun(elecMat, length);

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
            //Check returned delta
            Assert.AreEqual(expectedTECCostDelta, delta.GetCost(CostType.TEC), DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(expectedTECLaborDelta, delta.GetLabor(CostType.TEC), DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(expectedElecCostDelta, delta.GetCost(CostType.Electrical), DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(expectedElecLaborDelta, delta.GetLabor(CostType.Electrical), DELTA, "Returned Elec labor delta is wrong.");
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
            TECElectricalMaterial elecMat = elecMats[0];
            double addLength = 736;
            double removeLength = 23.4;
            double length = addLength - removeLength;

            //Act
            lengthVM.AddRun(elecMat, addLength);
            CostBatch delta = lengthVM.RemoveRun(elecMat, removeLength);

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
            Total removedRatedElecTotal = ratedElecTotal * removeLength;

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
            //Check returned delta
            Assert.AreEqual(expectedTECCostDelta, delta.GetCost(CostType.TEC), DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(expectedTECLaborDelta, delta.GetLabor(CostType.TEC), DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(expectedElecCostDelta, delta.GetCost(CostType.Electrical), DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(expectedElecLaborDelta, delta.GetLabor(CostType.Electrical), DELTA, "Returned Elec labor delta is wrong.");
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
            TECElectricalMaterial elecMat = elecMats[0];
            double length = 435.2;

            //Act
            CostBatch delta = lengthVM.AddLength(elecMat, length);

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
            //Check returned delta
            Assert.AreEqual(expectedTECCostDelta, delta.GetCost(CostType.TEC), DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(expectedTECLaborDelta, delta.GetLabor(CostType.TEC), DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(expectedElecCostDelta, delta.GetCost(CostType.Electrical), DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(expectedElecLaborDelta, delta.GetLabor(CostType.Electrical), DELTA, "Returned Elec labor delta is wrong.");
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
            TECElectricalMaterial elecMat = elecMats[0];
            double addLength = 543.6;
            double removeLength = 23.4;
            double length = addLength - removeLength;

            //Act
            lengthVM.AddRun(elecMat, addLength);
            CostBatch delta = lengthVM.RemoveLength(elecMat, removeLength);

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
            Total removedRatedElecTotal = ratedElecTotal * removeLength;

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
            //Check returned delta
            Assert.AreEqual(expectedTECCostDelta, delta.GetCost(CostType.TEC), DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(expectedTECLaborDelta, delta.GetLabor(CostType.TEC), DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(expectedElecCostDelta, delta.GetCost(CostType.Electrical), DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(expectedElecLaborDelta, delta.GetLabor(CostType.Electrical), DELTA, "Returned Elec labor delta is wrong.");
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
            CostBatch delta = miscVM.AddCost(tecCost);

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
            //Check returned delta
            Assert.AreEqual(tecCost.Cost, delta.GetCost(CostType.TEC), DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(tecCost.Labor, delta.GetLabor(CostType.TEC), DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(0, delta.GetCost(CostType.Electrical), DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(0, delta.GetLabor(CostType.Electrical), DELTA, "Returned Elec labor delta is wrong.");
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
            CostBatch delta = miscVM.RemoveCost(tecCost);

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
            //Check returned delta
            Assert.AreEqual(-tecCost.Cost, delta.GetCost(CostType.TEC), DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(-tecCost.Labor, delta.GetLabor(CostType.TEC), DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(0, delta.GetCost(CostType.Electrical), DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(0, delta.GetLabor(CostType.Electrical), DELTA, "Returned Elec labor delta is wrong.");
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
            CostBatch delta = miscVM.AddCost(elecCost);

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
            //Check returned delta
            Assert.AreEqual(0, delta.GetCost(CostType.TEC), DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(0, delta.GetLabor(CostType.TEC), DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(elecCost.Cost, delta.GetCost(CostType.Electrical), DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(elecCost.Labor, delta.GetLabor(CostType.Electrical), DELTA, "Returned Elec labor delta is wrong.");
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
            CostBatch delta = miscVM.RemoveCost(elecCost);

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
            //Check returned delta
            Assert.AreEqual(0, delta.GetCost(CostType.TEC), DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(0, delta.GetLabor(CostType.TEC), DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(-elecCost.Cost, delta.GetCost(CostType.Electrical), DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(-elecCost.Labor, delta.GetLabor(CostType.Electrical), DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void AddTECMiscToMiscCostsVM()
        {
            //Arrange
            MiscCostsSummaryVM miscVM = new MiscCostsSummaryVM();
            TECMisc tecMisc = new TECMisc(CostType.TEC, false);
            tecMisc.Cost = 542.7;
            tecMisc.Labor = 467.4;
            tecMisc.Quantity = 3;

            double expectedCost = tecMisc.Cost * tecMisc.Quantity;
            double expectedLabor = tecMisc.Labor * tecMisc.Quantity;

            //Act
            CostBatch delta = miscVM.AddCost(tecMisc);

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
            //Check returned delta
            Assert.AreEqual(expectedCost, delta.GetCost(CostType.TEC), DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(expectedLabor, delta.GetLabor(CostType.TEC), DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(0, delta.GetCost(CostType.Electrical), DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(0, delta.GetLabor(CostType.Electrical), DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void RemoveTECMiscFromMiscCostsVM()
        {
            //Arrange
            MiscCostsSummaryVM miscVM = new MiscCostsSummaryVM();
            TECMisc tecMisc = new TECMisc(CostType.TEC, false);
            tecMisc.Cost = 5478.124;
            tecMisc.Labor = 14.6;
            tecMisc.Quantity = 3;

            double expectedCost = tecMisc.Cost * tecMisc.Quantity;
            double expectedLabor = tecMisc.Labor * tecMisc.Quantity;

            //Act
            miscVM.AddCost(tecMisc);
            CostBatch delta = miscVM.RemoveCost(tecMisc);

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
            //Check returned delta
            Assert.AreEqual(-expectedCost, delta.GetCost(CostType.TEC), DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(-expectedLabor, delta.GetLabor(CostType.TEC), DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(0, delta.GetCost(CostType.Electrical), DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(0, delta.GetLabor(CostType.Electrical), DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void AddElecMiscToMiscCostsVM()
        {
            //Arrange
            MiscCostsSummaryVM miscVM = new MiscCostsSummaryVM();
            TECMisc elecMisc = new TECMisc(CostType.Electrical, false);
            elecMisc.Cost = 129.3;
            elecMisc.Labor = 12.3;
            elecMisc.Quantity = 3;

            double expectedCost = elecMisc.Cost * elecMisc.Quantity;
            double expectedLabor = elecMisc.Labor * elecMisc.Quantity;

            //Act
            CostBatch delta = miscVM.AddCost(elecMisc);

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
            //Check returned delta
            Assert.AreEqual(0, delta.GetCost(CostType.TEC), DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(0, delta.GetLabor(CostType.TEC), DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(expectedCost, delta.GetCost(CostType.Electrical), DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(expectedLabor, delta.GetLabor(CostType.Electrical), DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void RemoveElecMiscFromMiscCostsVM()
        {
            //Arrange
            MiscCostsSummaryVM miscVM = new MiscCostsSummaryVM();
            TECMisc elecMisc = new TECMisc(CostType.Electrical, false);
            elecMisc.Cost = 395.4;
            elecMisc.Labor = 843.45;
            elecMisc.Quantity = 3;

            double expectedCost = elecMisc.Cost * elecMisc.Quantity;
            double expectedLabor = elecMisc.Labor * elecMisc.Quantity;

            //Act
            miscVM.AddCost(elecMisc);
            CostBatch delta = miscVM.RemoveCost(elecMisc);

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
            //Check returned delta
            Assert.AreEqual(0, delta.GetCost(CostType.TEC), DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(0, delta.GetLabor(CostType.TEC), DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(-expectedCost, delta.GetCost(CostType.Electrical), DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(-expectedLabor, delta.GetLabor(CostType.Electrical), DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void ChangeCostQuantityInMiscCostsVM()
        {
            //Arrange
            MiscCostsSummaryVM miscVM = new MiscCostsSummaryVM();
            TECMisc misc = new TECMisc(CostType.TEC, false);
            misc.Cost = 942.2;
            misc.Labor = 375.23;
            misc.Quantity = 3;

            //Act
            miscVM.AddCost(misc);
            int deltaQuantity = 2;
            CostBatch delta = miscVM.ChangeQuantity(misc, deltaQuantity);

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
            //Check returned delta
            Assert.AreEqual(expectedDeltaCost, delta.GetCost(CostType.TEC), DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(expectedDeltaLabor, delta.GetLabor(CostType.TEC), DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(0, delta.GetCost(CostType.Electrical), DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(0, delta.GetLabor(CostType.Electrical), DELTA, "Returned Elec labor delta is wrong.");
        }
        [TestMethod]
        public void UpdateCostInMiscCostsVM()
        {
            //Arrange
            MiscCostsSummaryVM miscVM = new MiscCostsSummaryVM();
            TECMisc misc = new TECMisc(CostType.TEC, false);
            misc.Cost = 129.3;
            misc.Labor = 532.54;
            misc.Quantity = 3;

            //Act
            miscVM.AddCost(misc);
            double deltaCost = 121;
            double deltaLabor = 45.6;
            misc.Cost += deltaCost;
            misc.Labor += deltaLabor;

            CostBatch delta = miscVM.UpdateCost(misc);

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
            //Check returned delta
            Assert.AreEqual(expectedDeltaCost, delta.GetCost(CostType.TEC), DELTA, "Returned TEC cost delta is wrong.");
            Assert.AreEqual(expectedDeltaLabor, delta.GetLabor(CostType.TEC), DELTA, "Returned TEC labor delta is wrong.");
            Assert.AreEqual(0, delta.GetCost(CostType.Electrical), DELTA, "Returned Elec cost delta is wrong.");
            Assert.AreEqual(0, delta.GetLabor(CostType.Electrical), DELTA, "Returned Elec labor delta is wrong.");
        }
        #endregion
    }
}
