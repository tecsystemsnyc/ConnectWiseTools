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

        public TECIOModule(Guid guid) : base(guid)
        {
            _ioPerModule = 0;
        }
        public TECIOModule() : this(Guid.NewGuid()) { }
        public TECIOModule(TECIOModule ioModuleSource) : this()
        {
            copyPropertiesFromCost(this);
            _ioPerModule = ioModuleSource.IOPerModule;
        }


        public override object Copy()
        {
            var outObject = new TECIOModule(this.Guid);
            outObject.copyPropertiesFromCost(this);
            outObject._ioPerModule = this.IOPerModule;
            return outObject;
        }
    }
}
