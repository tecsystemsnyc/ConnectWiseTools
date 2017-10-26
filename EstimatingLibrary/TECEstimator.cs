using DebugLibrary;
using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECEstimator : TECObject
    {
        TECParameters parameters;
        TECExtraLabor extraLabor;
        const double ZERO = 0;

        #region Cost Base
        private CostBatch allCosts;
        private int pointNumber;
        #endregion

        #region Cost and Labor

        public double TECFieldHours
        {
            get { return allCosts.GetLabor(CostType.TEC); }
        }
        public double TECFieldLaborCost
        {
            get
            {
                { return TECFieldHours * parameters.CommRate; }
            }
        }
        
        public double PMPointLaborHours
        {
            get { return getPMPointHours(); }
        }
        public double PMLaborHours
        {
            get { return getPMTotalHours(); }
        }
        public double PMLaborCost
        {
            get { return getPMLaborCost(); }
        }

        public double ENGPointLaborHours
        {
            get { return getENGPointHours(); }
        }
        public double ENGLaborHours
        {
            get { return getENGTotalHours(); }
        }
        public double ENGLaborCost
        {
            get { return getENGLaborCost(); }
        }

        public double SoftPointLaborHours
        {
            get { return getSoftPointHours(); }
        }
        public double SoftLaborHours
        {
            get { return getSoftTotalHours(); }
        }
        public double SoftLaborCost
        {
            get { return getSoftLaborCost(); }
        }

        public double CommPointLaborHours
        {
            get { return getCommPointHours(); }
        }
        public double CommLaborHours
        {
            get { return getCommTotalHours(); }
        }
        public double CommLaborCost
        {
            get { return getCommLaborCost(); }
        }

        public double GraphPointLaborHours
        {
            get { return getGraphPointHours(); }
        }
        public double GraphLaborHours
        {
            get { return getGraphTotalHours(); }
        }
        public double GraphLaborCost
        {
            get { return getGraphLaborCost(); }
        }

        public double TECLaborHours
        {
            get { return getTECLaborHours(); }
        }
        public double TECLaborCost
        {
            get { return getTECLaborCost(); }
        }

        public double TotalLaborCost
        {
            get { return getTotalLaborCost(); }
        }

        public double TECCost
        {
            get { return getTECCost(); }
        }

        public double TECMaterialCost
        {
            get
            {
                return allCosts.GetCost(CostType.TEC);
            }
        }
        public double TECShipping
        {
            get { return getTECShipping(); }
        }
        public double TECWarranty
        {
            get { return getTECWarranty(); }
        }
        public double Tax
        {
            get
            { return getTax(); }
        }
        public double TECSubtotal
        {
            get
            {
                return getTECSubtotal();
            }
        }

        public double ElectricalLaborHours
        {
            get { return allCosts.GetLabor(CostType.Electrical); }
        }
        public double ElectricalLaborCost
        {
            get { return getElectricalLaborCost(); }
        }
        public double ElectricalSuperLaborHours
        {
            get { return getElectricalSuperLaborHours(); }
        }
        public double ElectricalSuperLaborCost
        {
            get { return getElectricalSuperLaborCost(); }
        }

        public double SubcontractorLaborHours
        {
            get { return getSubcontractorLaborHours(); }
        }
        public double SubcontractorLaborCost
        {
            get { return getSubcontractorLaborCost(); }
        }

        public double ElectricalMaterialCost
        {
            get
            { return allCosts.GetCost(CostType.Electrical); }
        }
        public double ElectricalShipping
        {
            get { return getElectricalShipping(); }
        }
        public double ElectricalWarranty
        {
            get { return getElectricalWarranty(); }
        }
        public double SubcontractorSubtotal
        {
            get
            {
                return getSubcontractorSubtotal();
            }
        }

        public double TotalPrice
        {
            get
            {
                return getTotalPrice();
            }
        }
        
        public int TotalPointNumber
        {
            get
            {
                return pointNumber;
            }
        }
        public double PricePerPoint
        {
            get { return getPricePerPoint(); }
        }
        public double Margin
        {
            get { return getMargin(); }
        }

        public double TotalCost
        {
            get { return getTotalCost(); }
        }

        public double Markup
        {
            get { return TotalPrice - TotalCost; }
        }
        #endregion

        public TECEstimator(TECBid Bid, ChangeWatcher watcher) : base(Guid.NewGuid())
        {
            parameters = Bid.Parameters;
            extraLabor = Bid.ExtraLabor;
            getInitialValues(Bid);
            watcher.CostChanged += CostChanged;
            watcher.PointChanged += PointChanged;
        }
        public TECEstimator(TECObject initalObject, TECParameters parameters, ChangeWatcher watcher) : base(Guid.NewGuid())
        {
            this.parameters = parameters;
            extraLabor = new TECExtraLabor(Guid.NewGuid());
            getInitialValues(initalObject);
            watcher.CostChanged += CostChanged;
            watcher.PointChanged += PointChanged;
        }
        
        private void CostChanged(CostBatch change)
        {
            addCost(change);
        }
        
        private void PointChanged(int pointNum)
        {
            addPoints(pointNum);
        }
        private void getInitialValues(object obj)
        {
            pointNumber = 0;
            allCosts = new CostBatch();
            if(obj is INotifyPointChanged pointContainer)
            {
                addPoints(pointContainer.PointNumber);

            }
            if(obj is INotifyCostChanged costContainer)
            {
                addCost(costContainer.CostBatch);
            }
            
        }
        
        #region Update From Changes
        private void addCost(CostBatch costsToAdd)
        {
            allCosts += costsToAdd;

            raiseMaterial();
            raiseTECLabor();
            
            raiseElectricalMaterial();
            raiseElectricalLabor();
        }
        private void addPoints(int poitNum)
        {
            pointNumber += poitNum;
            raiseFromPoints(); 
        }
        #endregion

        #region Calculate Derivatives
        private double getTECShipping()
        {
            return (TECMaterialCost * parameters.Shipping);
        }

        private double getTECWarranty()
        {
            return (TECMaterialCost * parameters.Warranty);
        }

        /// <summary>
        /// Returns TEC material costs of devices and their associated costs
        /// </summary>
        private double getExtendedMaterialCost()
        {
            return (TECMaterialCost + TECShipping + TECWarranty + Tax);
        }
        /// <summary>
        /// Returns TEC labor costs of associated costs
        /// </summary>
        private double getMaterialLabor()
        {
            double laborHours = allCosts.GetLabor(CostType.TEC);
            double cost = laborHours * parameters.CommRate;
            return cost;
        }
        /// <summary>
        /// Returns tax from the TEC materials cost at 8.75% if tax is not exempt
        /// </summary>
        private double getTax()
        {
            double outTax = 0;

            if (!parameters.IsTaxExempt)
            {
                outTax += parameters.Tax * TECMaterialCost;
            }

            return outTax;
        }

        /// <summary>
        /// Returns cost of all TEC material and labor with escalation and tax
        /// </summary>
        private double getTECCost()
        {
            double outCost = 0;
            outCost += getTECLaborCost();
            outCost += getExtendedMaterialCost();
            outCost += outCost * parameters.Escalation / 100;
            outCost += getTax();

            return outCost;
        }
        /// <summary>
        /// Returns TEC Cost plus profit
        /// </summary>
        private double getTECSubtotal()
        {
            double outCost = 0;
            outCost += getTECCost();
            outCost += outCost * parameters.Overhead / 100;
            outCost += outCost * parameters.Profit / 100;

            return outCost;
        }

        private double getElectricalShipping()
        {
            return (ElectricalMaterialCost * parameters.SubcontractorShipping);
        }

        private double getElectricalWarranty()
        {
            return (ElectricalMaterialCost * parameters.SubcontractorWarranty);
        }

        private double getExtendedElectricalMaterialCost()
        {
            return (ElectricalMaterialCost + ElectricalShipping + ElectricalWarranty);
        }

        #region Labor
        /// <summary>
        /// Returns PM labor hours based on points
        /// </summary>
        private double getPMPointHours()
        {
            double hours = pointNumber * parameters.PMExtenedCoef;

            return hours;
        }
        /// <summary>
        /// Returns total PM labor hours
        /// </summary>
        private double getPMTotalHours()
        {
            double hours = getPMPointHours();
            hours += extraLabor.PMExtraHours;

            return hours;
        }
        /// <summary>
        /// Returns PM labor cost
        /// </summary>
        private double getPMLaborCost()
        {
            double cost = getPMTotalHours() * parameters.PMRate;

            return cost;
        }

        /// <summary>
        /// Returns ENG labor hours based on points
        /// </summary>
        private double getENGPointHours()
        {
            double hours = pointNumber * parameters.ENGExtenedCoef;

            return hours;
        }
        /// <summary>
        /// Returns total ENG labor hours
        /// </summary>
        private double getENGTotalHours()
        {
            double hours = getENGPointHours();
            hours += extraLabor.ENGExtraHours;

            return hours;
        }
        /// <summary>
        /// Returns ENG labor cost
        /// </summary>
        private double getENGLaborCost()
        {
            double cost = getENGTotalHours() * parameters.ENGRate;

            return cost;
        }

        /// <summary>
        /// Returns PM labor hours based on points
        /// </summary>
        private double getCommPointHours()
        {
            double hours = pointNumber * parameters.CommExtenedCoef;

            return hours;
        }
        /// <summary>
        /// Returns total PM labor hours
        /// </summary>
        private double getCommTotalHours()
        {
            double hours = getCommPointHours();
            hours += extraLabor.CommExtraHours;

            return hours;
        }
        /// <summary>
        /// Returns PM labor cost
        /// </summary>
        private double getCommLaborCost()
        {
            double cost = getCommTotalHours() * parameters.CommRate;

            return cost;
        }

        /// <summary>
        /// Returns Soft labor hours based on points
        /// </summary>
        private double getSoftPointHours()
        {
            double hours = pointNumber * parameters.SoftExtenedCoef;

            return hours;
        }
        /// <summary>
        /// Returns total Soft labor hours
        /// </summary>
        private double getSoftTotalHours()
        {
            double hours = getSoftPointHours();
            hours += extraLabor.SoftExtraHours;

            return hours;
        }
        /// <summary>
        /// Returns Soft labor cost
        /// </summary>
        private double getSoftLaborCost()
        {
            double cost = getSoftTotalHours() * parameters.SoftRate;

            return cost;
        }

        /// <summary>
        /// Returns Graph labor hours based on points
        /// </summary>
        private double getGraphPointHours()
        {
            double hours = pointNumber * parameters.GraphExtenedCoef;

            return hours;
        }
        /// <summary>
        /// Returns total Graph labor hours
        /// </summary>
        private double getGraphTotalHours()
        {
            double hours = getGraphPointHours();
            hours += extraLabor.GraphExtraHours;

            return hours;
        }
        /// <summary>
        /// Returns Graph labor cost
        /// </summary>
        private double getGraphLaborCost()
        {
            double cost = getGraphTotalHours() * parameters.GraphRate;

            return cost;
        }


        /// <summary>
        /// Returns all TEC labor hours
        /// </summary>
        private double getTECLaborHours()
        {
            double outLabor = 0;
            outLabor += getPMTotalHours();
            outLabor += getENGTotalHours();
            outLabor += getCommTotalHours();
            outLabor += getSoftTotalHours();
            outLabor += getGraphTotalHours();
            outLabor += allCosts.GetLabor(CostType.TEC);
            return outLabor;
        }
        /// <summary>
        /// Returns all TEC labor cost
        /// </summary>
        private double getTECLaborCost()
        {
            double outCost = 0;
            outCost += getPMLaborCost();
            outCost += getENGLaborCost();
            outCost += getCommLaborCost();
            outCost += getSoftLaborCost();
            outCost += getGraphLaborCost();
            outCost += getMaterialLabor();
            return outCost;
        }

        /// <summary>
        /// Returns the Journeyman electrical labor cost
        /// </summary>
        private double getElectricalLaborCost()
        {
            double electricalHours = allCosts.GetLabor(CostType.Electrical);
            double electricalRate = parameters.ElectricalEffectiveRate;
            double cost = electricalHours * electricalRate;

            return cost;
        }
        /// <summary>
        /// Returns the electrical super labor hours
        /// </summary>
        private double getElectricalSuperLaborHours()
        {
            return allCosts.GetLabor(CostType.Electrical) * parameters.ElectricalSuperRatio;
        }
        /// <summary>
        /// Returns the electrical super labor cost
        /// </summary>
        private double getElectricalSuperLaborCost()
        {
            double cost = getElectricalSuperLaborHours() * parameters.ElectricalSuperEffectiveRate;

            return cost;
        }
        /// <summary>
        /// Returns the electrical labor hours of all wire, conduit, and their associated costs 
        /// </summary>
        private double getTotalElectricalLaborHours()
        {
            double laborCost = allCosts.GetLabor(CostType.Electrical) + getElectricalSuperLaborHours();
            return laborCost;
        }
        /// <summary>
        /// Returns the electrical labor cost of all wire, conduit, and their associated costs 
        /// </summary>
        private double getTotalElectricalLaborCost()
        {
            double electricalLaborCost = getElectricalLaborCost();
            double electricalSuperLaborCost = getElectricalSuperLaborCost();
            double laborCost = electricalLaborCost + electricalSuperLaborCost;
            return laborCost;
        }

        /// <summary>
        /// Returns the subcontractor labor hours
        /// </summary>
        private double getSubcontractorLaborHours()
        {
            double laborHours = getTotalElectricalLaborHours();
            return laborHours;
        }
        /// <summary>
        /// Returns the subcontractor labor cost
        /// </summary>
        private double getSubcontractorLaborCost()
        {
            double laborHours = getTotalElectricalLaborCost();
            return laborHours;
        }

        /// <summary>
        /// Returns the total labor cost
        /// </summary>
        private double getTotalLaborCost()
        {
            double cost = getSubcontractorLaborCost() + getTECLaborCost();
            return cost;
        }
        #endregion

        /// <summary>
        /// Returns the electrical material and labor costs with escalation 
        /// </summary>
        private double getSubcontractorCost()
        {
            double subcontractorLaborCost = getSubcontractorLaborCost();
            double extendedElectricalMaterialCost = getExtendedElectricalMaterialCost();
            double subcontractorEscalation = parameters.SubcontractorEscalation;

            double outCost = (subcontractorLaborCost + extendedElectricalMaterialCost) * (1 + (subcontractorEscalation) / 100);

            return outCost;
        }
        /// <summary>
        /// Returns the electrical total with markup 
        /// </summary>
        private double getSubcontractorSubtotal()
        {
            double subContractorCost = getSubcontractorCost();
            double subContractorMarkup = parameters.SubcontractorMarkup;
            double outCost = subContractorCost * (1 + (subContractorMarkup / 100));
            return outCost;
        }
        /// <summary>
        /// Returns the total cost
        /// </summary>
        private double getTotalCost()
        {
            return getSubcontractorCost() + getTECCost();
        }
        /// <summary>
        /// Returns the final sell price 
        /// </summary>
        private double getTotalPrice()
        {
            double tecSubtotal = getTECSubtotal();
            double subcontractSubtotal = getSubcontractorSubtotal();

            double outPrice = tecSubtotal + subcontractSubtotal;
            if (parameters.RequiresBond)
            {
                outPrice *= parameters.BondRate;
            }
            return outPrice;
        }

        #region Metrics
        /// <summary>
        /// Returns the final price per point 
        /// </summary>
        private double getPricePerPoint()
        {
            return (getTotalPrice() / pointNumber);
        }
        /// <summary>
        /// Returns the Margin based on sell price and cost 
        /// </summary>
        private double getMargin()
        {
            double margin;
            double totalPrice = getTotalPrice();
            double tecCost = getTECCost();
            double subCost = getSubcontractorCost();
            margin = (totalPrice - tecCost - subCost) / totalPrice;
            return margin * 100;
        }
        #endregion

        #endregion

        #region Raise Properties
        private void raiseFromPoints()
        {
            raisePropertyChanged("TotalPointNumber");
            raiseTECLabor();
        }
        private void raiseElectricalMaterial()
        {
            raisePropertyChanged("ElectricalMaterialCost");
            raisePropertyChanged("ElectricalShipping");
            raisePropertyChanged("ElectricalWarranty");
            raiseSubcontractorTotals();
        }
        private void raiseMaterial()
        {
            raisePropertyChanged("TECMaterialCost");
            raisePropertyChanged("TECShipping");
            raisePropertyChanged("TECWarranty");
            raisePropertyChanged("Tax");
            raiseTECTotals();
        }
        private void raiseTECLabor()
        {
            raisePropertyChanged("TECFieldHours");
            raisePropertyChanged("TECFieldLaborCost");

            raisePropertyChanged("PMPointLaborHours");
            raisePropertyChanged("PMLaborHours");
            raisePropertyChanged("PMLaborCost");

            raisePropertyChanged("ENGPointLaborHours");
            raisePropertyChanged("ENGLaborHours");
            raisePropertyChanged("ENGLaborCost");

            raisePropertyChanged("SoftPointLaborHours");
            raisePropertyChanged("SoftLaborHours");
            raisePropertyChanged("SoftLaborCost");

            raisePropertyChanged("CommPointLaborHours");
            raisePropertyChanged("CommLaborHours");
            raisePropertyChanged("CommLaborCost");

            raisePropertyChanged("GraphPointLaborHours");
            raisePropertyChanged("GraphLaborHours");
            raisePropertyChanged("GraphLaborCost");

            raisePropertyChanged("TECLaborHours");
            raisePropertyChanged("TECLaborCost");
            raiseTECTotals();
            raiseLabor();
        }
        private void raiseElectricalLabor()
        {
            raisePropertyChanged("ElectricalLaborHours");
            raisePropertyChanged("ElectricalLaborCost");
            raisePropertyChanged("ElectricalSuperLaborHours");
            raisePropertyChanged("ElectricalSuperLaborCost");
            raiseSubcontractorLabor();
            raiseLabor();
        }
        private void raiseSubcontractorLabor()
        {
            raisePropertyChanged("SubcontractorLaborHours");
            raisePropertyChanged("SubcontractorLaborCost");
            raiseSubcontractorTotals();
            raiseLabor();
        }
        private void raiseTECTotals()
        {
            raisePropertyChanged("TECSubtotal");
            raiseTotals();
        }
        private void raiseSubcontractorTotals()
        {
            raisePropertyChanged("SubcontractorSubtotal");
            raiseTotals();
        }
        private void raiseLabor()
        {
            raisePropertyChanged("TotalLaborCost");
        }
        private void raiseTotals()
        {
            raisePropertyChanged("TotalCost");
            raisePropertyChanged("TotalPrice");
            raisePropertyChanged("PricePerPoint");
            raisePropertyChanged("Margin");
            raisePropertyChanged("Markup");
        }
        private void raiseAll()
        {
            raisePropertyChanged("TotalPointNumber");
            raisePropertyChanged("ElectricalMaterialCost");
            raisePropertyChanged("MaterialCost");
            raisePropertyChanged("Tax");
            raisePropertyChanged("PMPointLaborHours");
            raisePropertyChanged("PMLaborHours");
            raisePropertyChanged("PMLaborCost");

            raisePropertyChanged("ENGPointLaborHours");
            raisePropertyChanged("ENGLaborHours");
            raisePropertyChanged("ENGLaborCost");

            raisePropertyChanged("SoftPointLaborHours");
            raisePropertyChanged("SoftLaborHours");
            raisePropertyChanged("SoftLaborCost");

            raisePropertyChanged("CommPointLaborHours");
            raisePropertyChanged("CommLaborHours");
            raisePropertyChanged("CommLaborCost");

            raisePropertyChanged("GraphPointLaborHours");
            raisePropertyChanged("GraphLaborHours");
            raisePropertyChanged("GraphLaborCost");

            raisePropertyChanged("TECLaborHours");
            raisePropertyChanged("TECLaborCost");
            raisePropertyChanged("TECSubtotal");
            raisePropertyChanged("ElectricalLaborHours");
            raisePropertyChanged("ElectricalLaborCost");
            raisePropertyChanged("ElectricalSuperLaborHours");
            raisePropertyChanged("ElectricalSuperLaborCost");
            raisePropertyChanged("SubcontractorLaborHours");
            raisePropertyChanged("SubcontractorLaborCost");
            raisePropertyChanged("TECSubtotal");
            raisePropertyChanged("SubcontractorSubtotal");
            raisePropertyChanged("TotalLaborCost");
            raisePropertyChanged("TotalCost");
            raisePropertyChanged("TotalPrice");
            raisePropertyChanged("PricePerPoint");
            raisePropertyChanged("Margin");
        }
        #endregion
        
    }
}
