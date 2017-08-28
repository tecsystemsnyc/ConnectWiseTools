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
            private set
            {
                _quantity = value;
                RaisePropertyChanged("Quantity");
            }
        }

        private double _totalCost;
        public double TotalCost
        {
            get
            {
                return _totalCost;
            }
            private set
            {
                _totalCost = value;
                RaisePropertyChanged("TotalCost");
            }
        }

        private double _totalLabor;
        public double TotalLabor
        {
            get
            {
                return _totalLabor;
            }
            private set
            {
                _totalLabor = value;
                RaisePropertyChanged("TotalLabor");
            }
        }

        public CostSummaryItem(TECCost cost) : base(Guid.NewGuid())
        {
            _cost = cost;
            if (cost is TECMisc misc)
            {
                _quantity = misc.Quantity;
            }
            else
            {
                _quantity = 1;
            }
            updateTotals();
        }

        public CostBatch AddQuantity(int quantity)
        {
            Quantity += quantity;
            return updateTotals();
        }
        public CostBatch RemoveQuantity(int quantity)
        {
            Quantity -= quantity;
            return updateTotals();
        }
        public CostBatch Refresh()
        {
            return updateTotals();
        }

        private CostBatch updateTotals()
        {
            double newCost = (Cost.Cost * Quantity);
            double newLabor = (Cost.Labor * Quantity);

            double deltaCost = newCost - TotalCost;
            double deltaLabor = newLabor - TotalLabor;

            TotalCost = newCost;
            TotalLabor = newLabor;

            return new CostBatch(deltaCost, deltaLabor, Cost.Type);
        }
    }
}
