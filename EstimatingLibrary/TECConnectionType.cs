using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    
    public class TECConnectionType : TECObject
    {
        #region Properties
        private Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }
        
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                var temp = Copy();
                _name = value;
                NotifyPropertyChanged("Name", temp, this);
            }
        }

        private double _cost;
        public double Cost
        {
            get { return _cost; }
            set
            {
                var temp = Copy();
                _cost = value;
                NotifyPropertyChanged("Cost", temp, this);
            }
        }

        private double _labor;
        public double Labor
        {
            get { return _cost; }
            set
            {
                var temp = Copy();
                _labor = value;
                NotifyPropertyChanged("Labor", temp, this);
            }
        }

        #endregion

        #region Initializers
        public TECConnectionType() : this(Guid.NewGuid())
        {
            
        }
        public TECConnectionType(Guid guid)
        {
            _guid = guid;
            _name = "";
            _cost = 0;
            _labor = 0;
        }
        #endregion

        public override object Copy()
        {
            var outType = new TECConnectionType();
            outType._guid = this._guid;
            outType._name = this._name;
            outType._cost = this._cost;
            outType._labor = this._labor;
            return outType;
        }
    }
}
