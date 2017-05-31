using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.Models
{
    public class DeviceSummaryItem : TECObject
    {
        private TECDevice _device;
        public TECDevice Device
        {
            get { return _device; }
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
                NotifyPropertyChanged("Total", old, _total);
            }
        }

        public DeviceSummaryItem(TECDevice device)
        {
            _device = device;
            _quantity = 1;
            device.PropertyChanged += Device_PropertyChanged;
            device.Manufacturer.PropertyChanged += Device_PropertyChanged;
            updateTotal();
        }

        private void updateTotal()
        {
            Total = (Device.Cost * Device.Manufacturer.Multiplier * Quantity);
        }

        private void Device_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Cost" || e.PropertyName == "Multiplier" || e.PropertyName == "Manufacturer")
            {
                updateTotal();
            }
        }

        public override object Copy()
        {
            throw new NotImplementedException();
        }
    }
}
