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
        public TECManufacturer(Guid guid) : base (guid)
        {
            _multiplier = 1;
        }
        public TECManufacturer() : this(Guid.NewGuid()) { }

        public TECManufacturer(TECManufacturer manSource) : this(manSource.Guid)
        {
            _name = manSource.Name;
            _multiplier = manSource.Multiplier;
        }
        #endregion //Constructors

        #region methods
        public override Object Copy()
        {
            TECManufacturer outMan = new TECManufacturer(this);
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
