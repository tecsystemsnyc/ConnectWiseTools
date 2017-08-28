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
        TECBid bid;
        const double ZERO = 0;

        #region Cost Base
        private CostBatch cost;
        private int pointNumber;
        #endregion

        #region Cost and Labor

        public double TECFieldHours
        {
            get { return cost.GetLabor(CostType.TEC); }
        }
        public double TECFieldLaborCost
        {
            get
            {
                { return TECFieldHours * bid.Parameters.CommRate; }
            }
        }
        
        public double PMPointLaborHours
        {
            get { return getPMPointHours(bid); }
        }
        public double PMLaborHours
        {
            get { return getPMTotalHours(bid); }
        }
        public double PMLaborCost
        {
            get { return getPMLaborCost(bid); }
        }

        public double ENGPointLaborHours
        {
            get { return getENGPointHours(bid); }
        }
        public double ENGLaborHours
        {
            get { return getENGTotalHours(bid); }
        }
        public double ENGLaborCost
        {
            get { return getENGLaborCost(bid); }
        }

        public double SoftPointLaborHours
        {
            get { return getSoftPointHours(bid); }
        }
        public double SoftLaborHours
        {
            get { return getSoftTotalHours(bid); }
        }
        public double SoftLaborCost
        {
            get { return getSoftLaborCost(bid); }
        }

        public double CommPointLaborHours
        {
            get { return getCommPointHours(bid); }
        }
        public double CommLaborHours
        {
            get { return getCommTotalHours(bid); }
        }
        public double CommLaborCost
        {
            get { return getCommLaborCost(bid); }
        }

        public double GraphPointLaborHours
        {
            get { return getGraphPointHours(bid); }
        }
        public double GraphLaborHours
        {
            get { return getGraphTotalHours(bid); }
        }
        public double GraphLaborCost
        {
            get { return getGraphLaborCost(bid); }
        }

        public double TECLaborHours
        {
            get { return getTECLaborHours(bid); }
        }
        public double TECLaborCost
        {
            get { return getTECLaborCost(bid); }
        }

        public double TotalLaborCost
        {
            get { return getTotalLaborCost(bid); }
        }

        public double TECCost
        {
            get { return getTECCost(bid); }
        }

        public double TECMaterialCost
        {
            get
            {
                return tecCost.Cost;
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
            { return getTax(bid); }
        }
        public double TECSubtotal
        {
            get
            {
                return getTECSubtotal(bid);
            }
        }

        public double ElectricalLaborHours
        {
            get { return electricalCost.Labor; }
        }
        public double ElectricalLaborCost
        {
            get { return getElectricalLaborCost(bid); }
        }
        public double ElectricalSuperLaborHours
        {
            get { return getElectricalSuperLaborHours(); }
        }
        public double ElectricalSuperLaborCost
        {
            get { return getElectricalSuperLaborCost(bid); }
        }

        public double SubcontractorLaborHours
        {
            get { return getSubcontractorLaborHours(bid); }
        }
        public double SubcontractorLaborCost
        {
            get { return getSubcontractorLaborCost(bid); }
        }

        public double ElectricalMaterialCost
        {
            get
            { return electricalCost.Cost; }
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
                return getSubcontractorSubtotal(bid);
            }
        }

        public double TotalPrice
        {
            get
            {
                return getTotalPrice(bid);
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
            get { return getPricePerPoint(bid); }
        }
        public double Margin
        {
            get { return getMargin(bid); }
        }

        public double TotalCost
        {
            get { return getTotalCost(bid); }
        }
        #endregion

        public TECEstimator(TECBid Bid, ChangeWatcher watcher) : base(Guid.NewGuid())
        {
            bid = Bid;
            getInitialValues();
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
        private void getInitialValues()
        {
            pointNumber = 0;
            costs = new CostBatch();

            addPoints(bid.PointNumber);
            addCost(bid.Costs);
        }
        
        #region Update From Changes
        private void addCost(CostBatch costsToAdd)
        {
            cost += costsToAdd;

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
            return (TECMaterialCost * 0.03);
        }

        private double getTECWarranty()
        {
            return (TECMaterialCost * 0.05);
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
        private double getMaterialLabor(TECBid bid)
        {
            double laborHours = tecCost.Labor;
            double cost = tecCost.Labor * bid.Parameters.CommRate;
            return cost;
        }
        /// <summary>
        /// Returns tax from the TEC materials cost at 8.75% if tax is not exempt
        /// </summary>
        private double getTax(TECBid bid)
        {
            double outTax = 0;

            if (!bid.Parameters.IsTaxExempt)
            {
                outTax += .0875 * TECMaterialCost;
            }

            return outTax;
        }

        /// <summary>
        /// Returns cost of all TEC material and labor with escalation and tax
        /// </summary>
        private double getTECCost(TECBid bid)
        {
            double outCost = 0;
            outCost += getTECLaborCost(bid);
            outCost += getExtendedMaterialCost();
            outCost += outCost * bid.Parameters.Escalation / 100;
            outCost += getTax(bid);

            return outCost;
        }
        /// <summary>
        /// Returns TEC Cost plus profit
        /// </summary>
        private double getTECSubtotal(TECBid bid)
        {
            double outCost = 0;
            outCost += getTECCost(bid);
            outCost += outCost * bid.Parameters.Overhead / 100;
            outCost += outCost * bid.Parameters.Profit / 100;

            return outCost;
        }

        private double getElectricalShipping()
        {
            return (ElectricalMaterialCost * 0.03);
        }

        private double getElectricalWarranty()
        {
            return (ElectricalMaterialCost * 0.05);
        }

        private double getExtendedElectricalMaterialCost()
        {
            return (ElectricalMaterialCost + ElectricalShipping + ElectricalWarranty);
        }

        #region Labor
        /// <summary>
        /// Returns PM labor hours based on points
        /// </summary>
        private double getPMPointHours(TECBid bid)
        {
            double hours = pointNumber * bid.Parameters.PMCoef;

            return hours;
        }
        /// <summary>
        /// Returns total PM labor hours
        /// </summary>
        private double getPMTotalHours(TECBid bid)
        {
            double hours = getPMPointHours(bid);
            hours += bid.Parameters.PMExtraHours;

            return hours;
        }
        /// <summary>
        /// Returns PM labor cost
        /// </summary>
        private double getPMLaborCost(TECBid bid)
        {
            double cost = getPMTotalHours(bid) * bid.Parameters.PMRate;

            return cost;
        }

        /// <summary>
        /// Returns ENG labor hours based on points
        /// </summary>
        private double getENGPointHours(TECBid bid)
        {
            double hours = pointNumber * bid.Parameters.ENGCoef;

            return hours;
        }
        /// <summary>
        /// Returns total ENG labor hours
        /// </summary>
        private double getENGTotalHours(TECBid bid)
        {
            double hours = getENGPointHours(bid);
            hours += bid.Parameters.ENGExtraHours;

            return hours;
        }
        /// <summary>
        /// Returns ENG labor cost
        /// </summary>
        private double getENGLaborCost(TECBid bid)
        {
            double cost = getENGTotalHours(bid) * bid.Parameters.ENGRate;

            return cost;
        }

        /// <summary>
        /// Returns PM labor hours based on points
        /// </summary>
        private double getCommPointHours(TECBid bid)
        {
            double hours = pointNumber * bid.Parameters.CommCoef;

            return hours;
        }
        /// <summary>
        /// Returns total PM labor hours
        /// </summary>
        private double getCommTotalHours(TECBid bid)
        {
            double hours = getCommPointHours(bid);
            hours += bid.Parameters.CommExtraHours;

            return hours;
        }
        /// <summary>
        /// Returns PM labor cost
        /// </summary>
        private double getCommLaborCost(TECBid bid)
        {
            double cost = getCommTotalHours(bid) * bid.Parameters.CommRate;

            return cost;
        }

        /// <summary>
        /// Returns Soft labor hours based on points
        /// </summary>
        private double getSoftPointHours(TECBid bid)
        {
            double hours = pointNumber * bid.Parameters.SoftCoef;

            return hours;
        }
        /// <summary>
        /// Returns total Soft labor hours
        /// </summary>
        private double getSoftTotalHours(TECBid bid)
        {
            double hours = getSoftPointHours(bid);
            hours += bid.Parameters.SoftExtraHours;

            return hours;
        }
        /// <summary>
        /// Returns Soft labor cost
        /// </summary>
        private double getSoftLaborCost(TECBid bid)
        {
            double cost = getSoftTotalHours(bid) * bid.Parameters.SoftRate;

            return cost;
        }

        /// <summary>
        /// Returns Graph labor hours based on points
        /// </summary>
        private double getGraphPointHours(TECBid bid)
        {
            double hours = pointNumber * bid.Parameters.GraphCoef;

            return hours;
        }
        /// <summary>
        /// Returns total Graph labor hours
        /// </summary>
        private double getGraphTotalHours(TECBid bid)
        {
            double hours = getGraphPointHours(bid);
            hours += bid.Parameters.GraphExtraHours;

            return hours;
        }
        /// <summary>
        /// Returns Graph labor cost
        /// </summary>
        private double getGraphLaborCost(TECBid bid)
        {
            double cost = getGraphTotalHours(bid) * bid.Parameters.GraphRate;

            return cost;
        }


        /// <summary>
        /// Returns all TEC labor hours
        /// </summary>
        private double getTECLaborHours(TECBid bid)
        {
            double outLabor = 0;
            outLabor += getPMTotalHours(bid);
            outLabor += getENGTotalHours(bid);
            outLabor += getCommTotalHours(bid);
            outLabor += getSoftTotalHours(bid);
            outLabor += getGraphTotalHours(bid);
            outLabor += tecCost.Labor;
            return outLabor;
        }
        /// <summary>
        /// Returns all TEC labor cost
        /// </summary>
        private double getTECLaborCost(TECBid bid)
        {
            double outCost = 0;
            outCost += getPMLaborCost(bid);
            outCost += getENGLaborCost(bid);
            outCost += getCommLaborCost(bid);
            outCost += getSoftLaborCost(bid);
            outCost += getGraphLaborCost(bid);
            outCost += getMaterialLabor(bid);
            return outCost;
        }

        /// <summary>
        /// Returns the Journeyman electrical labor cost
        /// </summary>
        private double getElectricalLaborCost(TECBid bid)
        {
            double electricalHours = electricalCost.Labor;
            double electricalRate = bid.Parameters.ElectricalEffectiveRate;
            double cost = electricalHours * electricalRate;

            return cost;
        }
        /// <summary>
        /// Returns the electrical super labor hours
        /// </summary>
        private double getElectricalSuperLaborHours()
        {
            return electricalCost.Labor * bid.Parameters.ElectricalSuperRatio;
        }
        /// <summary>
        /// Returns the electrical super labor cost
        /// </summary>
        private double getElectricalSuperLaborCost(TECBid bid)
        {
            double cost = getElectricalSuperLaborHours() * bid.Parameters.ElectricalSuperEffectiveRate;

            return cost;
        }
        /// <summary>
        /// Returns the electrical labor hours of all wire, conduit, and their associated costs 
        /// </summary>
        private double getTotalElectricalLaborHours()
        {
            double laborCost = electricalCost.Labor + getElectricalSuperLaborHours();
            return laborCost;
        }
        /// <summary>
        /// Returns the electrical labor cost of all wire, conduit, and their associated costs 
        /// </summary>
        private double getTotalElectricalLaborCost(TECBid bid)
        {
            double electricalLaborCost = getElectricalLaborCost(bid);
            double electricalSuperLaborCost = getElectricalSuperLaborCost(bid);
            double laborCost = electricalLaborCost + electricalSuperLaborCost;
            return laborCost;
        }

        /// <summary>
        /// Returns the subcontractor labor hours
        /// </summary>
        private double getSubcontractorLaborHours(TECBid bid)
        {
            double laborHours = getTotalElectricalLaborHours();
            return laborHours;
        }
        /// <summary>
        /// Returns the subcontractor labor cost
        /// </summary>
        private double getSubcontractorLaborCost(TECBid bid)
        {
            double laborHours = getTotalElectricalLaborCost(bid);
            return laborHours;
        }

        /// <summary>
        /// Returns the total labor cost
        /// </summary>
        private double getTotalLaborCost(TECBid bid)
        {
            double cost = getSubcontractorLaborCost(bid) + getTECLaborCost(bid);
            return cost;
        }
        #endregion

        /// <summary>
        /// Returns the electrical material and labor costs with escalation 
        /// </summary>
        private double getSubcontractorCost(TECBid bid)
        {
            double subcontractorLaborCost = getSubcontractorLaborCost(bid);
            double extendedElectricalMaterialCost = getExtendedElectricalMaterialCost();
            double subcontractorEscalation = bid.Parameters.SubcontractorEscalation;

            double outCost = (subcontractorLaborCost + extendedElectricalMaterialCost) * (1 + (subcontractorEscalation) / 100);

            return outCost;
        }
        /// <summary>
        /// Returns the electrical total with markup 
        /// </summary>
        private double getSubcontractorSubtotal(TECBid bid)
        {
            double subContractorCost = getSubcontractorCost(bid);
            double subContractorMarkup = bid.Parameters.SubcontractorMarkup;
            double outCost = subContractorCost * (1 + (subContractorMarkup / 100));
            return outCost;
        }
        /// <summary>
        /// Returns the total cost
        /// </summary>
        private double getTotalCost(TECBid bid)
        {
            return getSubcontractorCost(bid) + getTECCost(bid);
        }
        /// <summary>
        /// Returns the final sell price 
        /// </summary>
        private double getTotalPrice(TECBid bid)
        {
            double tecSubtotal = getTECSubtotal(bid);
            double subcontractSubtotal = getSubcontractorSubtotal(bid);

            double outPrice = tecSubtotal + subcontractSubtotal;
            if (bid.Parameters.RequiresBond)
            {
                outPrice *= 1.013;
            }
            return outPrice;
        }

        #region Metrics
        /// <summary>
        /// Returns the final price per point 
        /// </summary>
        private double getPricePerPoint(TECBid bid)
        {
            return (getTotalPrice(bid) / pointNumber);
        }
        /// <summary>
        /// Returns the Margin based on sell price and cost 
        /// </summary>
        private double getMargin(TECBid bid)
        {
            double margin;
            double totalPrice = getTotalPrice(bid);
            double tecCost = getTECCost(bid);
            double subCost = getSubcontractorCost(bid);
            margin = (totalPrice - tecCost - subCost) / totalPrice;
            return margin * 100;
        }
        #endregion

        #endregion

        #region Raise Properties
        private void raiseFromPoints()
        {
            RaisePropertyChanged("TotalPointNumber");
            raiseTECLabor();
        }
        private void raiseElectricalMaterial()
        {
            RaisePropertyChanged("ElectricalMaterialCost");
            RaisePropertyChanged("ElectricalShipping");
            RaisePropertyChanged("ElectricalWarranty");
            raiseSubcontractorTotals();
        }
        private void raiseMaterial()
        {
            RaisePropertyChanged("TECMaterialCost");
            RaisePropertyChanged("TECShipping");
            RaisePropertyChanged("TECWarranty");
            RaisePropertyChanged("Tax");
            raiseTECTotals();
        }
        private void raiseTECLabor()
        {
            RaisePropertyChanged("TECFieldHours");
            RaisePropertyChanged("TECFieldLaborCost");

            RaisePropertyChanged("PMPointLaborHours");
            RaisePropertyChanged("PMLaborHours");
            RaisePropertyChanged("PMLaborCost");

            RaisePropertyChanged("ENGPointLaborHours");
            RaisePropertyChanged("ENGLaborHours");
            RaisePropertyChanged("ENGLaborCost");

            RaisePropertyChanged("SoftPointLaborHours");
            RaisePropertyChanged("SoftLaborHours");
            RaisePropertyChanged("SoftLaborCost");

            RaisePropertyChanged("CommPointLaborHours");
            RaisePropertyChanged("CommLaborHours");
            RaisePropertyChanged("CommLaborCost");

            RaisePropertyChanged("GraphPointLaborHours");
            RaisePropertyChanged("GraphLaborHours");
            RaisePropertyChanged("GraphLaborCost");

            RaisePropertyChanged("TECLaborHours");
            RaisePropertyChanged("TECLaborCost");
            raiseTECTotals();
            raiseLabor();
        }
        private void raiseElectricalLabor()
        {
            RaisePropertyChanged("ElectricalLaborHours");
            RaisePropertyChanged("ElectricalLaborCost");
            RaisePropertyChanged("ElectricalSuperLaborHours");
            RaisePropertyChanged("ElectricalSuperLaborCost");
            raiseSubcontractorLabor();
            raiseLabor();
        }
        private void raiseSubcontractorLabor()
        {
            RaisePropertyChanged("SubcontractorLaborHours");
            RaisePropertyChanged("SubcontractorLaborCost");
            raiseSubcontractorTotals();
            raiseLabor();
        }
        private void raiseTECTotals()
        {
            RaisePropertyChanged("TECSubtotal");
            raiseTotals();
        }
        private void raiseSubcontractorTotals()
        {
            RaisePropertyChanged("SubcontractorSubtotal");
            raiseTotals();
        }
        private void raiseLabor()
        {
            RaisePropertyChanged("TotalLaborCost");
        }
        private void raiseTotals()
        {
            RaisePropertyChanged("TotalCost");
            RaisePropertyChanged("TotalPrice");
            RaisePropertyChanged("PricePerPoint");
            RaisePropertyChanged("Margin");
        }
        private void raiseAll()
        {
            RaisePropertyChanged("TotalPointNumber");
            RaisePropertyChanged("ElectricalMaterialCost");
            RaisePropertyChanged("MaterialCost");
            RaisePropertyChanged("Tax");
            RaisePropertyChanged("PMPointLaborHours");
            RaisePropertyChanged("PMLaborHours");
            RaisePropertyChanged("PMLaborCost");

            RaisePropertyChanged("ENGPointLaborHours");
            RaisePropertyChanged("ENGLaborHours");
            RaisePropertyChanged("ENGLaborCost");

            RaisePropertyChanged("SoftPointLaborHours");
            RaisePropertyChanged("SoftLaborHours");
            RaisePropertyChanged("SoftLaborCost");

            RaisePropertyChanged("CommPointLaborHours");
            RaisePropertyChanged("CommLaborHours");
            RaisePropertyChanged("CommLaborCost");

            RaisePropertyChanged("GraphPointLaborHours");
            RaisePropertyChanged("GraphLaborHours");
            RaisePropertyChanged("GraphLaborCost");

            RaisePropertyChanged("TECLaborHours");
            RaisePropertyChanged("TECLaborCost");
            RaisePropertyChanged("TECSubtotal");
            RaisePropertyChanged("ElectricalLaborHours");
            RaisePropertyChanged("ElectricalLaborCost");
            RaisePropertyChanged("ElectricalSuperLaborHours");
            RaisePropertyChanged("ElectricalSuperLaborCost");
            RaisePropertyChanged("SubcontractorLaborHours");
            RaisePropertyChanged("SubcontractorLaborCost");
            RaisePropertyChanged("TECSubtotal");
            RaisePropertyChanged("SubcontractorSubtotal");
            RaisePropertyChanged("TotalLaborCost");
            RaisePropertyChanged("TotalCost");
            RaisePropertyChanged("TotalPrice");
            RaisePropertyChanged("PricePerPoint");
            RaisePropertyChanged("Margin");
        }
        #endregion
        
    }
}
