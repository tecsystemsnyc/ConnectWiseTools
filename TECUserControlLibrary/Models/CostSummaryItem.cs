using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.Models
{
    public class CostSummaryItem : TECObject
    {
        private TECCost _cost;
        public TECCost Cost
        {
            get { return _cost; }
        }

        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                RaisePropertyChanged("Quantity");
                updateTotals();
            }
        }

        private double _totalCost;
        public double TotalCost
        {
            get
            {
                return _totalCost;
            }
            set
            {
                double old = _totalCost;
                _totalCost = value;
                NotifyPropertyChanged("TotalCost", old, _totalCost);
            }
        }

        private double _totalLabor;
        public double TotalLabor
        {
            get
            {
                return _totalLabor;
            }
            set
            {
                double old = _totalLabor;
                _totalLabor = value;
                NotifyPropertyChanged("TotalLabor", old, _totalLabor);
            }
        }

        private TECSystem system;

        public CostSummaryItem(TECCost cost)
        {
            _cost = cost;
            _quantity = cost.Quantity;
            Cost.PropertyChanged += Cost_PropertyChanged;
            updateTotals();
            system = null;
        }

        public CostSummaryItem(TECMisc cost, TECSystem typicalSystem)
        {
            _cost = cost;
            _quantity = cost.Quantity * typicalSystem.SystemInstances.Count;
            Cost.PropertyChanged += Cost_PropertyChanged;
            typicalSystem.SystemInstances.CollectionChanged += SystemInstances_CollectionChanged;
            updateTotals();
            system = typicalSystem;
        }

        private void updateTotals()
        {
            TotalCost = (Cost.Cost * Quantity);
            TotalLabor = (Cost.Labor * Quantity);
        }

        private void Cost_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Cost" || e.PropertyName == "Labor")
            {
                updateTotals();
            }
            else if (e.PropertyName == "Quantity")
            {
                if (e is PropertyChangedExtendedEventArgs<object>)
                {
                    PropertyChangedExtendedEventArgs<object> args = e as PropertyChangedExtendedEventArgs<object>;
                    if (system == null)
                    {
                        Quantity -= (args.OldValue as TECCost).Quantity;
                        Quantity += (args.NewValue as TECCost).Quantity;
                    }
                    else
                    {
                        Quantity -= (args.OldValue as TECCost).Quantity * system.SystemInstances.Count;
                        Quantity += (args.NewValue as TECCost).Quantity * system.SystemInstances.Count;
                    }
                }
            }
        }

        private void SystemInstances_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach(object item in e.NewItems)
                {
                    Quantity += Cost.Quantity;
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach(object item in e.OldItems)
                {
                    Quantity -= Cost.Quantity;
                }
            }
        }

        public override object Copy()
        {
            throw new NotImplementedException();
        }
    }
}
