using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECManufacturer : TECScope
    {
        #region Properties
        public double Multiplier
        {
            get { return _multiplier; }
            set
            {
                var temp = this.Copy();
                _multiplier = value;
                NotifyPropertyChanged("Multiplier", temp, this);
            }
        }
        private double _multiplier;
        #endregion //Properties

        #region Constructors
        public TECManufacturer(string name, string description, double multiplier, Guid guid) : base (name, description, guid)
        {
            _multiplier = multiplier;
        }
        public TECManufacturer(string name, string description, double multiplier) : this(name, description, multiplier, Guid.NewGuid()) { }
        public TECManufacturer() : this("", "", 1) { }

        public TECManufacturer(TECManufacturer manSource) : this(manSource.Name, manSource.Description, manSource.Multiplier, manSource.Guid) { }
        #endregion //Constructors

        #region methods
        public override Object Copy()
        {
            TECManufacturer outMan = new TECManufacturer(this.Name, this.Description, this.Multiplier);
            outMan._guid = this.Guid;
            return outMan;
        }

        public override Object DragDropCopy()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
