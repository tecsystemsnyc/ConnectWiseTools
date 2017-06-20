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
        ChangeWatcher watcher;

        const double ZERO = 0;

        List<string> omitStrings = new List<string>(new string[]{"AddRelationship", "RemoveRelationship", "AddCatalog", "RemoveCatalog"});

        #region Cost Base
        private TECCost tecCost;
        private TECCost electricalCost;
        private int pointNumber;
        #endregion

        #region Cost and Labor

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

        public double BudgetPrice
        {
            get { return getBudgetPrice(bid); }
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

        public TECEstimator(TECBid Bid)
        {
            bid = Bid;
            getInitialValues();
            watcher = new ChangeWatcher(bid);
            watcher.InstanceChanged += Object_PropertyChanged;
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
                    if(!(oldValue is TECCatalogs))
                    {
                        if(newValue is TECSystem && oldValue is TECSystem)
                        {
                            handleInstanceAdded(newValue as TECSystem, oldValue as TECSystem);
                        }
                        else if (newValue is TECMisc && oldValue is TECSystem)
                        {
                            handleAddMiscInSystem(newValue as TECMisc, oldValue as TECSystem);
                        }
                        else
                        {
                            addCost(newValue);
                            addPoints(newValue);
                        }
                    }
                }
                else if (e.PropertyName == "Remove")
                {
                    message = "Remove change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);
                    if (!(oldValue is TECCatalogs))
                    {
                        if (newValue is TECSystem && oldValue is TECSystem)
                        {
                            handleInstanceRemoved(newValue as TECSystem, oldValue as TECSystem);
                        }
                        else if (newValue is TECMisc && oldValue is TECSystem)
                        {
                            handleRemoveMiscInSystem(newValue as TECMisc, oldValue as TECSystem);
                        }
                        else
                        {
                            removeCost(newValue);
                            removePoints(newValue);
                        }
                    }
                }
                else if (omitStrings.Contains(e.PropertyName)) { }
                else
                {
                    message = "Edit change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);
                    editCost(newValue, oldValue);
                    editPoints(newValue, oldValue);

                    if(bid.GetType().GetProperty(e.PropertyName) != null)
                    {
                        var list = bid.GetType().GetProperty(e.PropertyName).GetValue(bid, null) as IList;
                        if (list != null)
                        {
                            foreach (object item in list)
                            {
                                addCost(item);
                                addPoints(item);
                            }
                        }
                    }
                    if(newValue is TECBidParameters)
                    {
                        raiseMaterial();
                        raiseTECTotals();
                        raiseSubcontractorTotals();
                    }
                    else if (newValue is TECLabor)
                    {
                        raiseTECLabor();
                        raiseElectricalLabor();
                    }
                }
            }
            else
            {
                message = "Property not compatible: " + e.PropertyName;
                DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);

            }
        }

        private void handleRemoveMiscInSystem(TECMisc misc, TECSystem system)
        {
            foreach(TECSystem instance in system.SystemInstances)
            {
                removeCost(misc);
            }
        }

        private void handleAddMiscInSystem(TECMisc misc, TECSystem system)
        {
            foreach (TECSystem instance in system.SystemInstances)
            {
                addCost(misc);
            }
        }

        private void handleInstanceRemoved(TECSystem instance, TECSystem parent)
        {

            removeCost(instance);
            removePoints(instance);
            foreach (TECMisc misc in parent.MiscCosts)
            {
                removeCost(misc);
            }
        }

        private void handleInstanceAdded(TECSystem instance, TECSystem parent)
        {
            addCost(instance);
            addPoints(instance);
            foreach (TECMisc misc in parent.MiscCosts)
            {
                addCost(misc);
            }
        }

        private void getInitialValues()
        {
            pointNumber = 0;
            tecCost = new TECCost();
            electricalCost = new TECCost();

            foreach (TECSystem system in bid.Systems)
            {
                addCost(system);
                addPoints(system);
            }
            foreach (TECPanel panel in bid.Panels)
            {
                addCost(panel);
            }
            foreach (TECController controller in bid.Controllers)
            {
                addCost(controller);
            }
            foreach (TECMisc miscCost in bid.MiscCosts)
            {
                addCost(miscCost);
            }
        }

        #region Update From Changes
        private void addCost(object item)
        {
            if (item is CostComponent)
            {
                bool tecChanged = false;
                bool electricalChanged = false;
                var costComponent = item as CostComponent;
                foreach(TECCost cost in costComponent.Costs)
                {
                    if (cost.Type == CostType.TEC)
                    {
                        tecCost.Cost += cost.ExtendedCost;
                        tecCost.Labor += cost.Labor;
                        tecChanged = true;
                    }
                    else if (cost.Type == CostType.Electrical) 
                    {
                        electricalCost.Cost += cost.Cost;
                        electricalCost.Labor += cost.Labor;
                        electricalChanged = true;
                    }
                }
                if (tecChanged)
                {
                    raiseMaterial();
                    raiseLabor();
                }
                if (electricalChanged)
                {
                    raiseElectricalMaterial();
                    raiseElectricalLabor();
                }
            }
        }
        private void removeCost(object item)
        {
            if (item is CostComponent)
            {
                bool tecChanged = false;
                bool electricalChanged = false;
                var costComponent = item as CostComponent;
                foreach (TECCost cost in costComponent.Costs)
                {
                    if (cost.Type == CostType.TEC)
                    {
                        tecCost.Cost -= cost.ExtendedCost;
                        tecCost.Labor -= cost.Labor;
                        tecChanged = true;
                    }
                    else if (cost.Type == CostType.Electrical)
                    {
                        electricalCost.Cost -= cost.Cost;
                        electricalCost.Labor -= cost.Labor;
                        electricalChanged = true;
                    }
                }
                if (tecChanged)
                {
                    raiseMaterial();
                    raiseLabor();
                }
                if (electricalChanged)
                {
                    raiseElectricalMaterial();
                    raiseElectricalLabor();
                }
            }
        }
        private void editCost(object newValue, object oldValue)
        {
            if (newValue.GetType() == oldValue.GetType())
            {
                addCost(newValue);
                removeCost(oldValue);
            }
        }

        private void addPoints(object item)
        {
            if (item is PointComponent)
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
        public double getTECShipping()
        {
            return (TECMaterialCost * 0.03);
        }

        public double getTECWarranty()
        {
            return (TECMaterialCost * 0.05);
        }

        /// <summary>
        /// Returns TEC material costs of devices and their associated costs
        /// </summary>
        public double getExtendedMaterialCost()
        {
            return (TECMaterialCost + TECShipping + TECWarranty + Tax);
        }
        /// <summary>
        /// Returns TEC labor costs of associated costs
        /// </summary>
        public double getMaterialLabor(TECBid bid)
        {
            double laborHours = tecCost.Labor;
            double cost = tecCost.Labor * bid.Labor.CommRate;
            return cost;
        }
        /// <summary>
        /// Returns tax from the TEC materials cost at 8.75% if tax is not exempt
        /// </summary>
        public double getTax(TECBid bid)
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
        public double getTECCost(TECBid bid)
        {
            double outCost = 0;
            outCost += getTECLaborCost(bid);
            outCost += getMaterialLabor(bid);
            outCost += getExtendedMaterialCost();
            outCost += outCost * bid.Parameters.Escalation / 100;
            outCost += getTax(bid);

            return outCost;
        }
        /// <summary>
        /// Returns TEC Cost plus profit
        /// </summary>
        public double getTECSubtotal(TECBid bid)
        {
            double outCost = 0;
            outCost += getTECCost(bid);
            outCost += outCost * bid.Parameters.Overhead / 100;
            outCost += outCost * bid.Parameters.Profit / 100;

            return outCost;
        }
        
        public double getElectricalShipping()
        {
            return (ElectricalMaterialCost * 0.03);
        }

        public double getElectricalWarranty()
        {
            return (ElectricalMaterialCost * 0.05);
        }

        public double getExtendedElectricalMaterialCost()
        {
            return (ElectricalMaterialCost + ElectricalShipping + ElectricalWarranty);
        }

        #region Labor
        /// <summary>
        /// Returns PM labor hours based on points
        /// </summary>
        public double getPMPointHours(TECBid bid)
        {
            double hours = pointNumber * bid.Labor.PMCoef;

            return hours;
        }
        /// <summary>
        /// Returns total PM labor hours
        /// </summary>
        public double getPMTotalHours(TECBid bid)
        {
            double hours = getPMPointHours(bid);
            hours += bid.Labor.PMExtraHours;

            return hours;
        }
        /// <summary>
        /// Returns PM labor cost
        /// </summary>
        public double getPMLaborCost(TECBid bid)
        {
            double cost = getPMTotalHours(bid) * bid.Labor.PMRate;

            return cost;
        }

        /// <summary>
        /// Returns ENG labor hours based on points
        /// </summary>
        public double getENGPointHours(TECBid bid)
        {
            double hours = pointNumber * bid.Labor.ENGCoef;

            return hours;
        }
        /// <summary>
        /// Returns total ENG labor hours
        /// </summary>
        public double getENGTotalHours(TECBid bid)
        {
            double hours = getENGPointHours(bid);
            hours += bid.Labor.ENGExtraHours;

            return hours;
        }
        /// <summary>
        /// Returns ENG labor cost
        /// </summary>
        public double getENGLaborCost(TECBid bid)
        {
            double cost = getENGTotalHours(bid) * bid.Labor.ENGRate;

            return cost;
        }

        /// <summary>
        /// Returns PM labor hours based on points
        /// </summary>
        public double getCommPointHours(TECBid bid)
        {
            double hours = pointNumber * bid.Labor.CommCoef;

            return hours;
        }
        /// <summary>
        /// Returns total PM labor hours
        /// </summary>
        public double getCommTotalHours(TECBid bid)
        {
            double hours = getCommPointHours(bid);
            hours += bid.Labor.CommExtraHours;

            return hours;
        }
        /// <summary>
        /// Returns PM labor cost
        /// </summary>
        public double getCommLaborCost(TECBid bid)
        {
            double cost = getCommTotalHours(bid) * bid.Labor.CommRate;

            return cost;
        }

        /// <summary>
        /// Returns Soft labor hours based on points
        /// </summary>
        public double getSoftPointHours(TECBid bid)
        {
            double hours = pointNumber * bid.Labor.SoftCoef;

            return hours;
        }
        /// <summary>
        /// Returns total Soft labor hours
        /// </summary>
        public double getSoftTotalHours(TECBid bid)
        {
            double hours = getSoftPointHours(bid);
            hours += bid.Labor.SoftExtraHours;

            return hours;
        }
        /// <summary>
        /// Returns Soft labor cost
        /// </summary>
        public double getSoftLaborCost(TECBid bid)
        {
            double cost = getSoftTotalHours(bid) * bid.Labor.SoftRate;

            return cost;
        }

        /// <summary>
        /// Returns Graph labor hours based on points
        /// </summary>
        public double getGraphPointHours(TECBid bid)
        {
            double hours = pointNumber * bid.Labor.GraphCoef;

            return hours;
        }
        /// <summary>
        /// Returns total Graph labor hours
        /// </summary>
        public double getGraphTotalHours(TECBid bid)
        {
            double hours = getGraphPointHours(bid);
            hours += bid.Labor.GraphExtraHours;

            return hours;
        }
        /// <summary>
        /// Returns Graph labor cost
        /// </summary>
        public double getGraphLaborCost(TECBid bid)
        {
            double cost = getGraphTotalHours(bid) * bid.Labor.GraphRate;

            return cost;
        }

        public double getMiscLaborCost(TECBid bid)
        {
            double cost = tecCost.Labor * bid.Labor.CommRate;
            return cost;
        }

        /// <summary>
        /// Returns all TEC labor hours
        /// </summary>
        public double getTECLaborHours(TECBid bid)
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
        public double getTECLaborCost(TECBid bid)
        {
            double outCost = 0;
            outCost += getPMLaborCost(bid);
            outCost += getENGLaborCost(bid);
            outCost += getCommLaborCost(bid);
            outCost += getSoftLaborCost(bid);
            outCost += getGraphLaborCost(bid);
            outCost += getMaterialLabor(bid);
            outCost += getMiscLaborCost(bid);
            return outCost;
        }

        /// <summary>
        /// Returns the Journeyman electrical labor cost
        /// </summary>
        public double getElectricalLaborCost(TECBid bid)
        {
            double cost = electricalCost.Labor * bid.Labor.ElectricalEffectiveRate;

            return cost;
        }
        /// <summary>
        /// Returns the electrical super labor hours
        /// </summary>
        public double getElectricalSuperLaborHours()
        {
            double laborHours = electricalCost.Labor;

            return laborHours / 7;
        }
        /// <summary>
        /// Returns the electrical super labor cost
        /// </summary>
        public double getElectricalSuperLaborCost(TECBid bid)
        {
            double cost = getElectricalSuperLaborHours() * bid.Labor.ElectricalSuperEffectiveRate;

            return cost;
        }
        /// <summary>
        /// Returns the electrical labor hours of all wire, conduit, and their associated costs 
        /// </summary>
        public double getTotalElectricalLaborHours()
        {
            double laborCost = electricalCost.Labor + getElectricalSuperLaborHours();
            return laborCost;
        }
        /// <summary>
        /// Returns the electrical labor cost of all wire, conduit, and their associated costs 
        /// </summary>
        public double getTotalElectricalLaborCost(TECBid bid)
        {
            double laborCost = getElectricalLaborCost(bid) + getElectricalSuperLaborCost(bid);
            return laborCost;
        }

        /// <summary>
        /// Returns the subcontractor labor hours
        /// </summary>
        public double getSubcontractorLaborHours(TECBid bid)
        {
            double laborHours = getTotalElectricalLaborHours();
            return laborHours;
        }
        /// <summary>
        /// Returns the subcontractor labor cost
        /// </summary>
        public double getSubcontractorLaborCost(TECBid bid)
        {
            double laborHours = getTotalElectricalLaborCost(bid);
            return laborHours;
        }

        /// <summary>
        /// Returns the total labor cost
        /// </summary>
        public double getTotalLaborCost(TECBid bid)
        {
            double cost = getSubcontractorLaborCost(bid) + getTECLaborCost(bid);
            return cost;
        }
        #endregion

        /// <summary>
        /// Returns the electrical material and labor costs with escalation 
        /// </summary>
        public double getSubcontractorCost(TECBid bid)
        {
            double outCost = 0;
            outCost += getSubcontractorLaborCost(bid);
            outCost += getExtendedElectricalMaterialCost();
            outCost += outCost * bid.Parameters.SubcontractorEscalation / 100;

            return outCost;
        }
        /// <summary>
        /// Returns the electrical total with markup 
        /// </summary>
        public double getSubcontractorSubtotal(TECBid bid)
        {
            double outCost = 0;
            outCost += getSubcontractorCost(bid);
            outCost += outCost * bid.Parameters.SubcontractorMarkup / 100;
            return outCost;
        }
        /// <summary>
        /// Returns the total cost
        /// </summary>
        public double getTotalCost(TECBid bid)
        {
            return getSubcontractorCost(bid) + getTECCost(bid);
        }
        /// <summary>
        /// Returns the final sell price 
        /// </summary>
        public double getTotalPrice(TECBid bid)
        {
            double outPrice = 0;

            outPrice += getTECSubtotal(bid);
            outPrice += getSubcontractorSubtotal(bid);
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
        public double getBudgetPrice(TECBid bid)
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
        public double getPricePerPoint(TECBid bid)
        {
            return (getTotalPrice(bid) / pointNumber);
        }
        /// <summary>
        /// Returns the Margin based on sell price and cost 
        /// </summary>
        public double getMargin(TECBid bid)
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

        public override object Copy()
        {
            throw new NotImplementedException();
        }

    }
}
