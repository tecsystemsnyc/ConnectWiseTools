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
            set
            {
                _quantity = value;
                RaisePropertyChanged("Quantity");
                updateTotal();
            }
        }

        private double _total;
        public double Total
        {
            get
            {
                return _total;
            }
            set
            {
                double old = _total;
                _total = value;
                NotifyPropertyChanged(Change.Edit, "Total", this, _total, old);
            }
        }

        public HardwareSummaryItem(TECHardware hardware) : base (Guid.NewGuid())
        {
            _hardware = hardware;
            _quantity = 1;
            hardware.PropertyChanged += Hardware_PropertyChanged;
            hardware.Manufacturer.PropertyChanged += Hardware_PropertyChanged;
            updateTotal();
        }

        private void updateTotal()
        {
            Total = (Hardware.Cost * Hardware.Manufacturer.Multiplier * Quantity);
        }

        private void Hardware_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Cost" || e.PropertyName == "Multiplier" || e.PropertyName == "Manufacturer")
            {
                updateTotal();
            }
        }
    }
}
