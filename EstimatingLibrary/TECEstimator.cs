﻿using DebugLibrary;
using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
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
        ChangeWatcher watcher;

        #region Cost Base
        private double tecLaborHours;
        private double tecMaterialCost;
        private double electricalLaborHours;
        private double electricalMaterialCost;
        private int pointNumber;
        #endregion

        #region Cost and Labor

        public double PMPointLaborHours
        {
            get { return GetPMPointHours(bid); }
        }
        public double PMLaborHours
        {
            get { return GetPMTotalHours(bid); }
        }
        public double PMLaborCost
        {
            get { return GetPMLaborCost(bid); }
        }

        public double ENGPointLaborHours
        {
            get { return GetENGPointHours(bid); }
        }
        public double ENGLaborHours
        {
            get { return GetENGTotalHours(bid); }
        }
        public double ENGLaborCost
        {
            get { return GetENGLaborCost(bid); }
        }

        public double SoftPointLaborHours
        {
            get { return GetSoftPointHours(bid); }
        }
        public double SoftLaborHours
        {
            get { return GetSoftTotalHours(bid); }
        }
        public double SoftLaborCost
        {
            get { return GetSoftLaborCost(bid); }
        }

        public double CommPointLaborHours
        {
            get { return GetCommPointHours(bid); }
        }
        public double CommLaborHours
        {
            get { return GetCommTotalHours(bid); }
        }
        public double CommLaborCost
        {
            get { return GetCommLaborCost(bid); }
        }

        public double GraphPointLaborHours
        {
            get { return GetGraphPointHours(bid); }
        }
        public double GraphLaborHours
        {
            get { return GetGraphTotalHours(bid); }
        }
        public double GraphLaborCost
        {
            get { return GetGraphLaborCost(bid); }
        }

        public double TECLaborHours
        {
            get { return GetTECLaborHours(bid); }
        }
        public double TECLaborCost
        {
            get { return GetTECLaborCost(bid); }
        }

        public double TotalLaborCost
        {
            get { return GetTotalLaborCost(bid); }
        }

        public double TECCost
        {
            get { return GetTECCost(bid); }
        }

        public double MaterialCost
        {
            get
            {
                return GetMaterialCost();
            }
        }
        public double Tax
        {
            get
            { return GetTax(bid); }
        }
        public double TECSubtotal
        {
            get
            {
                return GetTECSubtotal(bid);
            }
        }

        public double ElectricalLaborHours
        {
            get { return electricalLaborHours; }
        }
        public double ElectricalLaborCost
        {
            get { return GetElectricalLaborCost(bid); }
        }
        public double ElectricalSuperLaborHours
        {
            get { return GetElectricalSuperLaborHours(bid); }
        }
        public double ElectricalSuperLaborCost
        {
            get { return GetElectricalSuperLaborCost(bid); }
        }

        public double SubcontractorLaborHours
        {
            get { return GetSubcontractorLaborHours(bid); }
        }
        public double SubcontractorLaborCost
        {
            get { return GetSubcontractorLaborCost(bid); }
        }

        public double ElectricalMaterialCost
        {
            get
            { return GetElectricalMaterialCost(bid); }
        }
        public double SubcontractorSubtotal
        {
            get
            {
                return GetSubcontractorSubtotal(bid);
            }
        }

        public double TotalPrice
        {
            get
            {
                return GetTotalPrice(bid);
            }
        }

        public double BudgetPrice
        {
            get { return GetBudgetPrice(bid); }
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
            get { return GetPricePerPoint(bid); }
        }
        public double Margin
        {
            get { return GetMargin(bid); }
        }

        public double TotalCost
        {
            get { return GetTotalCost(bid); }
        }
        #endregion

        public TECEstimator(TECBid Bid)
        {
            bid = Bid;
            watcher = new ChangeWatcher(bid);
            watcher.Changed += Object_PropertyChanged;
        }

        private void Object_PropertyChanged(object sender, PropertyChangedEventArgs e) { handlePropertyChanged(e); }
        private void handlePropertyChanged(PropertyChangedEventArgs e)
        {
            string message = "Propertychanged: " + e.PropertyName;
            DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);

            if (e is PropertyChangedExtendedEventArgs<Object>)
            {
                PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;
                object oldValue = args.OldValue;
                object newValue = args.NewValue;
                if (e.PropertyName == "Add")
                {
                    message = "Add change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);
                    addCost(newValue);
                    addPoints(newValue);
                }
                else if (e.PropertyName == "Remove")
                {
                    message = "Remove change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);
                    removeCost(newValue);
                    removePoints(newValue);
                }
                else
                {
                    message = "Edit change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);
                    editCost(newValue, oldValue);
                    editPoints(newValue, oldValue);
                }
            }
            else
            {
                message = "Property not compatible: " + e.PropertyName;
                DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);
                
            }
        }

        private void getInitialValues()
        {
            pointNumber = 0;
            tecLaborHours = 0;
            tecMaterialCost = 0;
            electricalLaborHours = 0;
            electricalMaterialCost = 0;

            foreach(TECSystem system in bid.Systems)
            {
                addCost(system);
                addPoints(system);
            }
            foreach(TECPanel panel in bid.Panels)
            {
                addCost(panel);
            }
            foreach(TECController controller in bid.Controllers)
            {
                addCost(controller);
            }
            
        }

        #region Update From Changes
        private void addCost(object item)
        {
            if(item is CostComponent)
            {
                var cost = item as CostComponent;
                tecMaterialCost += cost.MaterialCost;
                tecLaborHours += cost.LaborCost;
                electricalMaterialCost += cost.ElectricalCost;
                electricalLaborHours += cost.ElectricalLabor;

                if (cost.MaterialCost != 0)
                { raiseMaterial(); }
                if (cost.LaborCost != 0)
                { raiseTECLabor(); }
                if (cost.ElectricalCost != 0)
                { raiseElectricalMaterial(); }
                if (cost.ElectricalLabor != 0)
                { raiseElectricalLabor(); }

            }
            else if (item is TECMiscCost)
            {
                var cost = item as TECMiscCost;
                tecMaterialCost += cost.Cost * cost.Quantity;
                if (cost.Cost != 0)
                { raiseMaterial(); }
            }
            else if (item is TECMiscWiring)
            {
                var cost = item as TECMiscWiring;
                electricalMaterialCost += cost.Cost * cost.Quantity;
                if (cost.Cost != 0)
                { raiseElectricalMaterial(); }
            }
            
        }
        private void removeCost(object item)
        {
            if (item is CostComponent)
            {
                var cost = item as CostComponent;
                tecMaterialCost -= cost.MaterialCost;
                tecLaborHours -= cost.LaborCost;
                electricalMaterialCost -= cost.ElectricalCost;
                electricalLaborHours -= cost.ElectricalLabor;
                if (cost.MaterialCost != 0)
                { raiseMaterial(); }
                if (cost.LaborCost != 0)
                { raiseTECLabor(); }
                if (cost.ElectricalCost != 0)
                { raiseElectricalMaterial(); }
                if (cost.ElectricalLabor != 0)
                { raiseElectricalLabor(); }
            }
            else if (item is TECMiscCost)
            {
                var cost = item as TECMiscCost;
                tecMaterialCost -= cost.Cost * cost.Quantity;
                if (cost.Cost != 0)
                { raiseMaterial(); }
            }
            else if (item is TECMiscWiring)
            {
                var cost = item as TECMiscWiring;
                electricalMaterialCost -= cost.Cost * cost.Quantity;
                if (cost.Cost != 0)
                { raiseElectricalMaterial(); }
            }
        }
        private void editCost(object newValue, object oldValue)
        {
            if(newValue.GetType() == oldValue.GetType())
            {
                if(newValue is TECConnection || newValue is TECMiscCost
                    || newValue is TECMiscWiring || newValue is TECDevice)
                {
                    addCost(newValue);
                    removeCost(oldValue);
                }
            }
        }
        
        private void addPoints(object item)
        {
            if(item is PointComponent)
            {
                pointNumber += (item as PointComponent).PointNumber;
                if ((item as PointComponent).PointNumber != 0)
                { raiseFromPoints(); }
            }
        }
        private void removePoints(object item)
        {
            if (item is PointComponent)
            {
                pointNumber -= (item as PointComponent).PointNumber;
                if ((item as PointComponent).PointNumber != 0)
                { raiseFromPoints(); }
            }
        }
        private void editPoints(object newValue, object oldValue)
        {
            if (newValue.GetType() == oldValue.GetType())
            {
                if (newValue is TECPoint)
                {
                    addPoints(newValue);
                    removePoints(oldValue);
                }
            }
        }
        #endregion

        #region Calculate Derivatives
        /// <summary>
        /// Returns TEC material costs of devices and their associated costs
        /// </summary>
        public double GetMaterialCost()
        {
            double shipping = 0.03;
            double warranty = 0.06;
            double cost = tecMaterialCost;
            
            cost += cost * shipping + cost * warranty;
            return cost;
        }
        /// <summary>
        /// Returns TEC labor costs of associated costs
        /// </summary>
        public double GetMaterialLabor(TECBid bid)
        {
            double laborHours = tecLaborHours;
            double cost = tecLaborHours * bid.Labor.CommRate;
            return cost;
        }
        /// <summary>
        /// Returns tax from the TEC materials cost at 8.75% if tax is not exempt
        /// </summary>
        public double GetTax(TECBid bid)
        {
            double outTax = 0;

            if (!bid.Parameters.IsTaxExempt)
            {
                outTax += .0875 * GetMaterialCost();
            }

            return outTax;
        }

        /// <summary>
        /// Returns cost of all TEC material and labor with escalation and tax
        /// </summary>
        public double GetTECCost(TECBid bid)
        {
            double outCost = 0;
            outCost += GetTECLaborCost(bid);
            outCost += GetMaterialLabor(bid);
            outCost += outCost * bid.Parameters.Escalation / 100;
            outCost += GetTax(bid);

            return outCost;
        }
        /// <summary>
        /// Returns TEC Cost plus profit
        /// </summary>
        public double GetTECSubtotal(TECBid bid)
        {
            double outCost = 0;
            outCost += GetTECCost(bid);
            outCost += outCost * bid.Parameters.Overhead / 100;
            outCost += outCost * bid.Parameters.Profit / 100;

            return outCost;
        }

        /// <summary>
        /// Returns the electrical material cost of all wire, conduit, and their associated costs 
        /// </summary>
        public double GetElectricalMaterialCost(TECBid bid)
        {
            double cost = electricalMaterialCost;
            double shipping = 0.03;
            double warranty = 0.05;
            
            cost += cost * shipping + cost * warranty;
            return cost;
        }

        #region Labor
        /// <summary>
        /// Returns PM labor hours based on points
        /// </summary>
        public double GetPMPointHours(TECBid bid)
        {
            double hours = pointNumber * bid.Labor.PMCoef;

            return hours;
        }
        /// <summary>
        /// Returns total PM labor hours
        /// </summary>
        public double GetPMTotalHours(TECBid bid)
        {
            double hours = GetPMPointHours(bid);
            hours += bid.Labor.PMExtraHours;

            return hours;
        }
        /// <summary>
        /// Returns PM labor cost
        /// </summary>
        public double GetPMLaborCost(TECBid bid)
        {
            double cost = GetPMTotalHours(bid) * bid.Labor.PMRate;

            return cost;
        }

        /// <summary>
        /// Returns ENG labor hours based on points
        /// </summary>
        public double GetENGPointHours(TECBid bid)
        {
            double hours = pointNumber * bid.Labor.ENGCoef;

            return hours;
        }
        /// <summary>
        /// Returns total ENG labor hours
        /// </summary>
        public double GetENGTotalHours(TECBid bid)
        {
            double hours = GetENGPointHours(bid);
            hours += bid.Labor.ENGExtraHours;

            return hours;
        }
        /// <summary>
        /// Returns ENG labor cost
        /// </summary>
        public double GetENGLaborCost(TECBid bid)
        {
            double cost = GetENGTotalHours(bid) * bid.Labor.ENGRate;

            return cost;
        }

        /// <summary>
        /// Returns PM labor hours based on points
        /// </summary>
        public double GetCommPointHours(TECBid bid)
        {
            double hours = pointNumber * bid.Labor.CommCoef;

            return hours;
        }
        /// <summary>
        /// Returns total PM labor hours
        /// </summary>
        public double GetCommTotalHours(TECBid bid)
        {
            double hours = GetCommPointHours(bid);
            hours += bid.Labor.CommExtraHours;

            return hours;
        }
        /// <summary>
        /// Returns PM labor cost
        /// </summary>
        public double GetCommLaborCost(TECBid bid)
        {
            double cost = GetCommTotalHours(bid) * bid.Labor.CommRate;

            return cost;
        }

        /// <summary>
        /// Returns Soft labor hours based on points
        /// </summary>
        public double GetSoftPointHours(TECBid bid)
        {
            double hours = pointNumber * bid.Labor.SoftCoef;

            return hours;
        }
        /// <summary>
        /// Returns total Soft labor hours
        /// </summary>
        public double GetSoftTotalHours(TECBid bid)
        {
            double hours = GetSoftPointHours(bid);
            hours += bid.Labor.SoftExtraHours;

            return hours;
        }
        /// <summary>
        /// Returns Soft labor cost
        /// </summary>
        public double GetSoftLaborCost(TECBid bid)
        {
            double cost = GetSoftTotalHours(bid) * bid.Labor.SoftRate;

            return cost;
        }

        /// <summary>
        /// Returns Graph labor hours based on points
        /// </summary>
        public double GetGraphPointHours(TECBid bid)
        {
            double hours = pointNumber * bid.Labor.GraphCoef;

            return hours;
        }
        /// <summary>
        /// Returns total Graph labor hours
        /// </summary>
        public double GetGraphTotalHours(TECBid bid)
        {
            double hours = GetGraphPointHours(bid);
            hours += bid.Labor.GraphExtraHours;

            return hours;
        }
        /// <summary>
        /// Returns Graph labor cost
        /// </summary>
        public double GetGraphLaborCost(TECBid bid)
        {
            double cost = GetGraphTotalHours(bid) * bid.Labor.GraphRate;

            return cost;
        }

        /// <summary>
        /// Returns all TEC labor hours
        /// </summary>
        public double GetTECLaborHours(TECBid bid)
        {
            double outLabor = 0;
            outLabor += GetPMTotalHours(bid);
            outLabor += GetENGTotalHours(bid);
            outLabor += GetCommTotalHours(bid);
            outLabor += GetSoftTotalHours(bid);
            outLabor += GetGraphTotalHours(bid);
            return outLabor;
        }
        /// <summary>
        /// Returns all TEC labor cost
        /// </summary>
        public double GetTECLaborCost(TECBid bid)
        {
            double outCost = 0;
            outCost += GetPMLaborCost(bid);
            outCost += GetENGLaborCost(bid);
            outCost += GetCommLaborCost(bid);
            outCost += GetSoftLaborCost(bid);
            outCost += GetGraphLaborCost(bid);
            outCost += GetMaterialLabor(bid);
            return outCost;
        }
        
        /// <summary>
        /// Returns the Journeyman electrical labor cost
        /// </summary>
        public double GetElectricalLaborCost(TECBid bid)
        {
            double cost = electricalLaborHours * bid.Labor.ElectricalEffectiveRate;

            return cost;
        }
        /// <summary>
        /// Returns the electrical super labor hours
        /// </summary>
        public double GetElectricalSuperLaborHours(TECBid bid)
        {
            double laborHours = electricalLaborHours;

            return laborHours / 7;
        }
        /// <summary>
        /// Returns the electrical super labor cost
        /// </summary>
        public double GetElectricalSuperLaborCost(TECBid bid)
        {
            double cost = GetElectricalSuperLaborHours(bid) * bid.Labor.ElectricalSuperEffectiveRate;

            return cost;
        }
        /// <summary>
        /// Returns the electrical labor hours of all wire, conduit, and their associated costs 
        /// </summary>
        public double GetTotalElectricalLaborHours(TECBid bid)
        {
            double laborCost = electricalLaborHours + GetElectricalSuperLaborHours(bid);
            return laborCost;
        }
        /// <summary>
        /// Returns the electrical labor cost of all wire, conduit, and their associated costs 
        /// </summary>
        public double GetTotalElectricalLaborCost(TECBid bid)
        {
            double laborCost = GetElectricalLaborCost(bid) + GetElectricalSuperLaborCost(bid);
            return laborCost;
        }

        /// <summary>
        /// Returns the subcontractor labor hours
        /// </summary>
        public double GetSubcontractorLaborHours(TECBid bid)
        {
            double laborHours = GetTotalElectricalLaborHours(bid);
            return laborHours;
        }
        /// <summary>
        /// Returns the subcontractor labor cost
        /// </summary>
        public double GetSubcontractorLaborCost(TECBid bid)
        {
            double laborHours = GetTotalElectricalLaborCost(bid);
            return laborHours;
        }

        /// <summary>
        /// Returns the total labor cost
        /// </summary>
        public double GetTotalLaborCost(TECBid bid)
        {
            double cost = GetSubcontractorLaborCost(bid) + GetTECLaborCost(bid);
            return cost;
        }
        #endregion

        /// <summary>
        /// Returns the electrical material and labor costs with escalation 
        /// </summary>
        public double GetSubcontractorCost(TECBid bid)
        {
            double outCost = 0;
            outCost += GetSubcontractorLaborCost(bid);
            outCost += GetElectricalMaterialCost(bid);
            outCost += outCost * bid.Parameters.SubcontractorEscalation / 100;

            return outCost;
        }
        /// <summary>
        /// Returns the electrical total with markup 
        /// </summary>
        public double GetSubcontractorSubtotal(TECBid bid)
        {
            double outCost = 0;
            outCost += GetSubcontractorCost(bid);
            outCost += outCost * bid.Parameters.SubcontractorMarkup / 100;
            return outCost;
        }
        /// <summary>
        /// Returns the total cost
        /// </summary>
        public  double GetTotalCost(TECBid bid)
        {
            return GetSubcontractorCost(bid) + GetTECCost(bid);
        }
        /// <summary>
        /// Returns the final sell price 
        /// </summary>
        public double GetTotalPrice(TECBid bid)
        {
            double outPrice = 0;

            outPrice += GetTECSubtotal(bid);
            outPrice += GetSubcontractorSubtotal(bid);
            if (bid.Parameters.RequiresBond)
            {
                outPrice *= 1.013;
            }
            return outPrice;
        }

        #region Budgeting
        /// <summary>
        /// Returns the budget price based on the user-assigned values in systems
        /// </summary>
        public double GetBudgetPrice(TECBid bid)
        {
            double price = 0;
            foreach (TECSystem system in bid.Systems)
            {
                if (system.TotalBudgetPrice >= 0)
                {
                    price += system.TotalBudgetPrice;
                }
            }
            return price;
        }
        #endregion
        #region Metrics
        /// <summary>
        /// Returns the final price per point 
        /// </summary>
        public double GetPricePerPoint(TECBid bid)
        {
            return (GetTotalPrice(bid) / pointNumber);
        }
        /// <summary>
        /// Returns the Margin based on sell price and cost 
        /// </summary>
        public double GetMargin(TECBid bid)
        {
            double margin;
            double totalPrice = GetTotalPrice(bid);
            double tecCost = GetTECCost(bid);
            double subCost = GetSubcontractorCost(bid);
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
            raiseSubcontractorTotals();
        }
        private void raiseMaterial()
        {
            RaisePropertyChanged("MaterialCost");
            RaisePropertyChanged("Tax");
            raiseTECTotals();
        }
        private void raiseTECLabor()
        {
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
            raiseTECTotals();
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
        public override object Copy()
        {
            throw new NotImplementedException();
        }
        
    }
}
 