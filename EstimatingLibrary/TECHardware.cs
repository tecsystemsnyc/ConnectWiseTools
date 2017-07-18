using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECHardware : TECCost
    {
        private TECManufacturer _manufacturer;

        public TECManufacturer Manufacturer
        {
            get { return _manufacturer; }
            set
            {
                var temp = this.Copy();
                _manufacturer = value;
                NotifyPropertyChanged("Manufacturer", temp, this);
                NotifyPropertyChanged("ChildChanged", (object)this, (object)value);
            }
        }
        public double ExtendedCost
        {
            get { return Cost * Manufacturer.Multiplier; }
        }

        public TECHardware(Guid guid, TECManufacturer manufacturer) : base(guid)
        {
            _manufacturer = manufacturer;
        }

        protected void copyPropertiesFromHardware(TECHardware hardware)
        {
            copyPropertiesFromCost(hardware);
            _manufacturer = hardware.Manufacturer;
        }


    }
}
