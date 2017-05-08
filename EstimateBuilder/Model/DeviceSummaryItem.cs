using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimateBuilder.Model
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
                RaisePropertyChanged("Total");
            }
        }

        public double Total
        {
            get
            {
                return (Device.Cost * Device.Manufacturer.Multiplier * Quantity);
            }
        }

        public DeviceSummaryItem(TECDevice device)
        {
            _device = device;
            _quantity = 1;
            device.PropertyChanged += Device_PropertyChanged;
            device.Manufacturer.PropertyChanged += Device_PropertyChanged;
        }

        private void Device_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Cost" || e.PropertyName == "Multiplier" || e.PropertyName == "Manufacturer")
            {
                RaisePropertyChanged("Total");
            }
        }

        public override object Copy()
        {
            throw new NotImplementedException();
        }
    }
}
