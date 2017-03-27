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

        public override object Copy()
        {
            var outObject = new TECIOModule();
            outObject.copyPropertiesFromCost(this);
            outObject.IOPerModule = this.IOPerModule;
            return outObject;
        }
    }
}
