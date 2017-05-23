using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.Models
{
    public class AssociatedCostSummaryItem : TECObject
    {
        private TECCost _associatedCost;
        public TECCost AssociatedCost
        {
            get { return _associatedCost; }
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
                NotifyPropertyChanged("Total", old, _totalCost);
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
                NotifyPropertyChanged("Total", old, _totalLabor);
            }
        }

        public AssociatedCostSummaryItem(TECCost assCost)
        {
            _associatedCost = assCost;
            _quantity = 1;
            AssociatedCost.PropertyChanged += AssociatedCost_PropertyChanged;
            updateTotals();
        }

        private void updateTotals()
        {
            TotalCost = (AssociatedCost.Cost * Quantity);
            TotalLabor = (AssociatedCost.Labor * Quantity);
        }

        private void AssociatedCost_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Cost" || e.PropertyName == "Labor")
            {
                updateTotals();
            }
        }

        public override object Copy()
        {
            throw new NotImplementedException();
        }
    }
}
