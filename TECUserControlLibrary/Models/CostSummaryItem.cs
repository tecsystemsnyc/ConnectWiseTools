using EstimatingLibrary;
using EstimatingLibrary.Utilities;
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
                NotifyPropertyChanged(Change.Edit, "TotalCost", this, _totalCost, old);
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
                NotifyPropertyChanged(Change.Edit, "TotalLabor", this, _totalLabor, old);
            }
        }

        public CostSummaryItem(TECCost cost) : base(Guid.NewGuid())
        {
            _cost = cost;
            _quantity = 1;
            if(cost is TECMisc)
            {
                _quantity = (cost as TECMisc).Quantity;
            }
            Cost.PropertyChanged += Cost_PropertyChanged;
            updateTotals();
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
                var old = this.Copy();
                updateTotals();
                NotifyPropertyChanged(Change.Edit, "Total", this, old);
            }
        }

        public override object Copy()
        {
            CostSummaryItem item = new CostSummaryItem(Cost);
            item._quantity = Quantity;
            item._totalCost = TotalCost;
            item._totalLabor = TotalLabor;
            return item;
        }
    }
}
