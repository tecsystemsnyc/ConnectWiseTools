using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimateBuilder.Model
{
    public class AssociatedCostSummaryItem : TECObject
    {
        private TECAssociatedCost _associatedCost;
        public TECAssociatedCost AssociatedCost
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
            get { return _totalCost; }
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
            get { return _totalLabor; }
            set
            {
                double old = _totalLabor;
                _totalLabor = value;
                NotifyPropertyChanged("TotalLabor", old, _totalLabor);
            }
        }

        public AssociatedCostSummaryItem(TECAssociatedCost associatedCost)
        {
            _associatedCost = associatedCost;
            _quantity = 1;
            associatedCost.PropertyChanged += AssociatedCost_PropertyChanged;
            updateTotals();
        }

        private void updateTotals()
        {
            TotalCost = (Quantity * AssociatedCost.Cost);
            TotalLabor = (Quantity * AssociatedCost.Labor);
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
