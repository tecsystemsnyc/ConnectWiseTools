using DebugLibrary;
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
        private const bool DEBUG_PROPERTIES = false;

        TECBid bid;
        ChangeWatcher watcher;

        #region Cost and Labor

        private double _pmPointLaborHours;
        public double PMPointLaborHours
        {
            get { return _pmPointLaborHours; }
            set
            {
                _pmPointLaborHours = value;
                RaisePropertyChanged("PMPointLaborHours");
            }
        }

        private double _pmLaborHours;
        public double PMLaborHours
        {
            get { return _pmLaborHours; }
            set
            {
                _pmLaborHours = value;
                RaisePropertyChanged("PMLabotHours");
            }
        }

        private double _pmLaborCost;
        public double PMLaborCost
        {
            get { return _pmLaborCost; }
            set
            {
                _pmLaborCost = value;
                RaisePropertyChanged("PMLaborCost");
            }
        }

        private double _engPointLaborHours;
        public double ENGPointLaborHours
        {
            get { return _engPointLaborHours; }
            set
            {
                _engPointLaborHours = value;
                RaisePropertyChanged("ENGPointLaborHours");
            }
        }
        private double _engLaborHours;
        public double ENGLaborHours
        {
            get { return _engLaborHours; }
            set
            {
                _engLaborHours = value;
                RaisePropertyChanged("ENGLaborHours");
            }
        }
        private double _engLaborCost;
        public double ENGLaborCost
        {
            get { return _engLaborCost; }
            set
            {
                _engLaborCost = value;
                RaisePropertyChanged("ENGLaborCost");
            }
        }

        private double _softPointLaborHours;
        public double SoftPointLaborHours
        {
            get { return _softPointLaborHours; }
            set
            {
                _softPointLaborHours = value;
                RaisePropertyChanged("SoftPointLaborHours");
            }
        }
        private double _softLaborHours;
        public double SoftLaborHours
        {
            get { return _softLaborHours; }
            set
            {
                _softLaborHours = value;
                RaisePropertyChanged("SoftLaborHours");
            }
        }
        private double _softLaborCost;
        public double SoftLaborCost
        {
            get { return _softLaborCost; }
            set
            {
                _softLaborCost = value;
                RaisePropertyChanged("SoftLaborCost");
            }
        }

        private double _commPointLaborHours;
        public double CommPointLaborHours
        {
            get { return _commPointLaborHours; }
            set
            {
                _commPointLaborHours = value;
                RaisePropertyChanged("CommPointLaborHours");
            }
        }
        private double _commLaborHours;
        public double CommLaborHours
        {
            get { return _commLaborHours; }
            set
            {
                _commLaborHours = value;
                RaisePropertyChanged("CommLaborHours");
            }
        }
        private double _commLaborCost;
        public double CommLaborCost
        {
            get { return _commLaborCost; }
            set
            {
                _commLaborCost = value;
                RaisePropertyChanged("CommLaborCost");
            }
        }

        private double _graphPointLaborHours;
        public double GraphPointLaborHours
        {
            get { return _graphPointLaborHours; }
            set
            {
                _graphPointLaborHours = value;
                RaisePropertyChanged("GraphPointLaborHours");
            }
        }
        private double _graphLaborHours;
        public double GraphLaborHours
        {
            get { return _graphLaborHours; }
        }
        private double _graphLaborCost;
        public double GraphLaborCost
        {
            get { return _graphLaborCost; }
        }

        private double _tecLaborHours;
        public double TECLaborHours
        {
            get { return _tecLaborHours; }
            set
            {
                _tecLaborHours = value;
                RaisePropertyChanged("TECLaborHours");
            }
        }
        private double _tecLaborCost;
        public double TECLaborCost
        {
            get { return _tecLaborCost; }
            set
            {
                _tecLaborCost = value;
                RaisePropertyChanged("TECLaborCost");
            }
        }
        private double _totalLaborCost;
        public double TotalLaborCost
        {
            get { return _totalLaborCost; }
            set
            {
                _tecLaborCost = value;
                RaisePropertyChanged("TotalLaborCost");
            }
        }

        private double _materialCost;
        public double MaterialCost
        {
            get { return _materialCost; }
            set
            {
                _materialCost = value;
                RaisePropertyChanged("MaterialCost");
            }
        }
        private double _tax;
        public double Tax
        {
            get { return _tax; }
            set
            {
                _tax = value;
                RaisePropertyChanged("Tax");
            }
        }
        private double _tecSubtotal;
        public double TECSubtotal
        {
            get { return _tecSubtotal; }
            set
            {
                _tecSubtotal = value;
                RaisePropertyChanged("TECSubtotal");
            }
        }

        private double _electricalLaborHours;
        public double ElectricalLaborHours
        {
            get { return _electricalLaborHours; }
            set
            {
                _electricalLaborHours = value;
                RaisePropertyChanged("ElectricalLaborHours");
            }
        }
        private double _electricalLaborCost;
        public double ElectricalLaborCost
        {
            get { return _electricalLaborCost; }
            set
            {
                _electricalLaborCost = value;
                RaisePropertyChanged("ElectricalLaborCost");
            }
        }
        private double _electricalSuperLaborHours;
        public double ElectricalSuperLaborHours
        {
            get { return _electricalSuperLaborHours; }
            set
            {
                _electricalSuperLaborHours = value;
                RaisePropertyChanged("ElectricalSuperLaborHours");
            }
        }
        private double _electricalSuperLaborCost;
        public double ElectricalSuperLaborCost
        {
            get { return _electricalSuperLaborCost; }
            set
            {
                _electricalSuperLaborCost = value;
                RaisePropertyChanged("ElectricalSuperLaborCost");
            }
        }

        private double _subcontractorLaborHours;
        public double SubcontractorLaborHours
        {
            get { return _subcontractorLaborHours; }
            set
            {
                _subcontractorLaborHours = value;
                RaisePropertyChanged("SubcontractorLaborHours");
            }
        }
        private double _subcontractorLaborCost;
        public double SubcontractorLaborCost
        {
            get { return _subcontractorLaborCost; }
            set
            {
                _subcontractorLaborCost = value;
                RaisePropertyChanged("SubcontractorLaborCost");
            }
        }

        private double _electricalMaterialCost;
        public double ElectricalMaterialCost
        {
            get { return _electricalMaterialCost; }
            set
            {
                _electricalMaterialCost = value;
                RaisePropertyChanged("ElectricalMaterialCost");
            }
        }
        private double _subcontractorSubtotal;
        public double SubcontractorSubtotal
        {
            get {  return _subcontractorSubtotal; }
            set
            {
                _subcontractorSubtotal = value;
                RaisePropertyChanged("SubcontractorSubtotal");
            }
        }

        private double _totalPrice;
        public double TotalPrice
        {
            get{ return _totalPrice; }
            set
            {
                _totalPrice = value;
                RaisePropertyChanged("Total Price");
            }
        }

        private double _budgetPrice;
        public double BudgetPrice
        {
            get { return _budgetPrice; }
            set
            {
                _budgetPrice = value;
                RaisePropertyChanged("BudgetPrice");
            }
        }
        private int _totalPointNumber;
        public int TotalPointNumber
        {
            get
            {
                return _totalPointNumber;
            }
            set
            {
                _totalPointNumber = value;
                RaisePropertyChanged("TotalPointNumber");
            }
        }
        private double _pricePerPoint;
        public double PricePerPoint
        {
            get { return _pricePerPoint; }
        }
        private double _margin;
        public double Margin
        {
            get { return _margin; }
            set
            {
                _margin = value;
                RaisePropertyChanged("Margin");
            }
        }

        private double _totalCost;
        public double TotalCost
        {
            get { return _totalCost; }
            set
            {
                _totalCost = value;
                RaisePropertyChanged("TotalCost");
            }
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
            DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);

            if (e is PropertyChangedExtendedEventArgs<Object>)
            {
                PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;
                object oldValue = args.OldValue;
                object newValue = args.NewValue;
                if (e.PropertyName == "Add")
                {
                    message = "Add change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);
                    addCost(newValue);
                    
                }
                else if (e.PropertyName == "Remove")
                {
                    message = "Remove change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);
                    
                }
                else if (e.PropertyName == "Edit")
                {
                    message = "Edit change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);
                    
                }
                else if (e.PropertyName == "ChildChanged")
                {
                    message = "Child change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);
                    
                }
                else if (e.PropertyName == "ObjectPropertyChanged")
                {
                    message = "Object changed: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);
                    
                }
                else if (e.PropertyName == "RelationshipPropertyChanged")
                {
                    message = "Object changed: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);
                    
                }
                else if (e.PropertyName == "MetaAdd")
                {
                    message = "MetaAdd change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);
                    
                }
                else if (e.PropertyName == "MetaRemove")
                {
                    message = "MetaRemove change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);
                    
                }
                else if (e.PropertyName == "AddRelationship")
                {
                    message = "Add relationship change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);
                    
                }
                else if (e.PropertyName == "RemoveRelationship")
                {
                    message = "Remove relationship change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);
                    
                }
                else if (e.PropertyName == "RemovedSubScope") { }
                else if (e.PropertyName == "AddCatalog")
                {
                    message = "Add change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);
                    
                }
                else if (e.PropertyName == "RemoveCatalog")
                {
                    message = "Remove change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);
                    
                }
                //else if (e.PropertyName == "Index Changed")
                //{

                //}
                else
                {
                    message = "Edit change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);
                }
            }
            else
            {
                message = "Property not compatible: " + e.PropertyName;
                DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);
                
            }
        }

        private void getInitialValues()
        {
            _totalPointNumber = bid.TotalPointNumber;
            _pmPointLaborHours = EstimateCalculator.GetPMPointHours(bid);
            _pmLaborHours = EstimateCalculator.GetPMTotalHours(bid);
            _pmLaborCost = EstimateCalculator.GetPMLaborCost(bid);
            _engPointLaborHours = EstimateCalculator.GetENGPointHours(bid);
            _engLaborHours = EstimateCalculator.GetENGTotalHours(bid);
            _engLaborCost = EstimateCalculator.GetENGLaborCost(bid);
            _softPointLaborHours = EstimateCalculator.GetSoftPointHours(bid);
            _softLaborHours = EstimateCalculator.GetSoftTotalHours(bid);
            _softLaborCost = EstimateCalculator.GetSoftLaborCost(bid);
            _commPointLaborHours = EstimateCalculator.GetCommPointHours(bid);
            _commLaborHours = EstimateCalculator.GetCommTotalHours(bid);
            _commLaborCost = EstimateCalculator.GetCommLaborCost(bid);
            _graphPointLaborHours = EstimateCalculator.GetGraphPointHours(bid);
            _graphLaborHours = EstimateCalculator.GetGraphTotalHours(bid);
            _graphLaborCost = EstimateCalculator.GetGraphLaborCost(bid);
            _tecLaborHours = EstimateCalculator.GetTECLaborHours(bid);
            _tecLaborCost = EstimateCalculator.GetTECLaborCost(bid);
            _totalLaborCost = EstimateCalculator.GetTotalLaborCost(bid);
            _materialCost =  EstimateCalculator.GetMaterialCost(bid);
            _tax = EstimateCalculator.GetTax(bid);
            _tecSubtotal = EstimateCalculator.GetTECSubtotal(bid);
            _electricalLaborHours = EstimateCalculator.GetElectricalLaborHours(bid);
            _electricalLaborCost = EstimateCalculator.GetElectricalLaborCost(bid);
            _electricalSuperLaborHours = EstimateCalculator.GetElectricalSuperLaborHours(bid);
            _electricalSuperLaborCost = EstimateCalculator.GetElectricalSuperLaborCost(bid);
            _subcontractorLaborHours = EstimateCalculator.GetSubcontractorLaborHours(bid);
            _subcontractorLaborCost = EstimateCalculator.GetSubcontractorLaborCost(bid);
            _electricalMaterialCost = EstimateCalculator.GetElectricalMaterialCost(bid);
            _subcontractorSubtotal = EstimateCalculator.GetSubcontractorSubtotal(bid);
            _totalPrice = EstimateCalculator.GetTotalPrice(bid);
            _budgetPrice = EstimateCalculator.GetBudgetPrice(bid);
            _pricePerPoint = EstimateCalculator.GetPricePerPoint(bid);
            _margin = EstimateCalculator.GetMargin(bid);
            _totalCost = EstimateCalculator.GetTotalCost(bid);
        }


        private void addCost(object item)
        {
            if(item is CostComponent)
            {
                var cost = item as CostComponent;
                MaterialCost += cost.MaterialCost;
                TECLaborHours += cost.LaborCost;
            }
            else if (item is TECMiscCost)
            {
                var cost = item as TECMiscCost;
                MaterialCost += cost.Cost * cost.Quantity;
            }
            else if (item is TECMiscWiring)
            {
                var cost = item as TECMiscCost;
                ElectricalMaterialCost += cost.Cost * cost.Quantity;
            }
            else if(item is TECConnection)
            {

            }

        }
        
        public override object Copy()
        {
            throw new NotImplementedException();
        }
        
    }
}
 