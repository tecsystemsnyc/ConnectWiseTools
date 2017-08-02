using EstimatingLibrary.Utilities;
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
                var old =  IOPerModule;
                _ioPerModule = value;
                NotifyPropertyChanged(Change.Edit, "IOPerModule", this, value, old);
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
    }
}
