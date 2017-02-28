using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECCostAddition : TECObject
    {
        #region Properties
        private Guid _guid;
        private string _name;
        private double _cost;
        private int _quantity;

        public Guid Guid
        {
            get { return _guid; }
        }
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
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                var temp = this.Copy();
                _quantity = value;
                NotifyPropertyChanged("Quantity", temp, this);
            }
        }

        #endregion

        public TECCostAddition(Guid guid)
        {
            _guid = guid;
            _name = "";
            _cost = 0;
            _quantity = 0;
        }
        public TECCostAddition() : this(Guid.NewGuid()) { }

        public override object Copy()
        {
            var outObject = new TECCostAddition();
            outObject._guid = _guid;
            outObject._name = _name;
            outObject._cost = _cost;
            outObject._quantity = _quantity;
            return outObject;
        }
    }
}
