using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECIOModule : TECCost
    {
        private int _ioPerModule;
        public int IOPerModule
        {
            get { return _ioPerModule; }
            set
            {
                var temp = Copy();
                _ioPerModule = value;
                NotifyPropertyChanged("IOPerModule", temp, this);
            }
        }

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


        public TECIOModule(Guid guid) : base(guid)
        {
            _ioPerModule = 0;
        }
        public TECIOModule() : this(Guid.NewGuid()) { }
        public TECIOModule(TECIOModule ioModuleSource) : this()
        {
            copyPropertiesFromCost(this);
            _ioPerModule = ioModuleSource.IOPerModule;
            _manufacturer = ioModuleSource.Manufacturer;
        }


        public override object Copy()
        {
            var outObject = new TECIOModule(this.Guid);
            outObject.copyPropertiesFromCost(this);
            outObject._ioPerModule = this.IOPerModule;
            outObject._manufacturer = this.Manufacturer;
            return outObject;
        }
    }
}
