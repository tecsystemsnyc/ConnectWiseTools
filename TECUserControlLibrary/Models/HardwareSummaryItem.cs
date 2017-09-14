using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.Models
{
    public class HardwareSummaryItem : TECObject
    {
        #region Fields
        private TECHardware _hardware;

        private int _quantity;

        private double _totalCost;
        private double _totalLabor;
        #endregion

        //Constructor
        public HardwareSummaryItem(TECHardware hardware) : base(Guid.NewGuid())
        {
            _hardware = hardware;
            _quantity = 1;
            updateTotals();
        }

        #region Properties
        public TECHardware Hardware
        {
            get { return _hardware; }
        }

        public int Quantity
        {
            get { return _quantity; }
            private set
            {
                _quantity = value;
                RaisePropertyChanged("Quantity");
            }
        }

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
        #endregion

        #region Methods
        public CostBatch Increment()
        {
            Quantity++;
            return updateTotals();
        }
        public CostBatch Decrement()
        {
            Quantity--;
            return updateTotals();
        }

        private CostBatch updateTotals()
        {
            double newCost = (Hardware.Cost * Quantity);
            double newLabor = (Hardware.Labor * Quantity);

            double deltaCost = newCost - TotalCost;
            double deltaLabor = newLabor - TotalLabor;

            TotalCost = newCost;
            TotalLabor = newLabor;

            return new CostBatch(deltaCost, deltaLabor, CostType.TEC);
        }
        #endregion
    }
}
