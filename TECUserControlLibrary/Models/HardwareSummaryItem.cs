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
        private TECHardware _hardware;
        public TECHardware Hardware
        {
            get { return _hardware; }
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

        public HardwareSummaryItem(TECHardware hardware) : base (Guid.NewGuid())
        {
            _hardware = hardware;
            _quantity = 1;
            updateTotals();
        }

        public CostObject Increment()
        {
            Quantity++;
            return updateTotals();
        }

        public CostObject Decrement()
        {
            Quantity--;
            return updateTotals();
        }

        private CostObject updateTotals()
        {
            double newCost = (Hardware.Cost * Hardware.Manufacturer.Multiplier * Quantity);
            double newLabor = (Hardware.Labor * Quantity);
            
            double deltaCost = newCost - TotalCost;
            double deltaLabor = newLabor - TotalLabor;

            TotalCost = newCost;
            TotalLabor = newLabor;

            return new CostObject(deltaCost, deltaLabor);
        }
    }
}
