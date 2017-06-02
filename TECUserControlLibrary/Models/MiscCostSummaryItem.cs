using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.Models
{
    public class MiscCostSummaryItem : TECObject
    {
        private TECMisc _cost;
        public TECMisc Cost
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

        public MiscCostSummaryItem(TECMisc cost)
        {
            _cost = cost;
            _quantity = cost.Quantity;
            updateTotals();
            cost.PropertyChanged += Cost_PropertyChanged;
        }

        private void Cost_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Cost" || e.PropertyName == "Labor")
            {
                updateTotals();
            }
        }

        private void updateTotals()
        {
            TotalCost = (Cost.Cost * Quantity);
            TotalLabor = (Cost.Labor * Quantity);
        }

        public override object Copy()
        {
            throw new NotImplementedException();
        }
    }
}
