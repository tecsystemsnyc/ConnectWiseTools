using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECIOModule : TECHardware
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
        
        public TECIOModule(Guid guid, TECManufacturer manufacturer) : base(guid, manufacturer)
        {
            _ioPerModule = 0;
        }
        public TECIOModule(TECManufacturer manufacturer) : this(Guid.NewGuid(), manufacturer) { }
        public TECIOModule(TECIOModule ioModuleSource) : this(ioModuleSource.Manufacturer)
        {
            copyPropertiesFromHardware(this);
            _ioPerModule = ioModuleSource.IOPerModule;
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
