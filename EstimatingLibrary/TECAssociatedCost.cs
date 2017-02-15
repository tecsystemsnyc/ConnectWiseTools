using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECAssociatedCost : TECObject
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
                var temp = this.Copy();
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
                var temp = this.Copy();
                _cost = value;
                NotifyPropertyChanged("Cost", temp, this);
            }
        }
        #endregion

        #region Constructors
        public TECAssociatedCost(Guid guid)
        {
            _guid = guid;
            _name = "";
            _cost = 0;
        }

        public TECAssociatedCost() : this(Guid.NewGuid()) { }
        #endregion


        public override object Copy()
        {
            var outCost = new TECAssociatedCost(this.Guid);
            outCost._name = this.Name;
            outCost._cost = this.Cost;
            return outCost;
        }
    }
}
