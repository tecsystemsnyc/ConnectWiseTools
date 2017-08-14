using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECManufacturer : TECLabeled
    {
        #region Properties
        public double Multiplier
        {
            get { return _multiplier; }
            set
            {
                var old = Multiplier;
                _multiplier = value;
                NotifyCombinedChanged(Change.Edit, "Multiplier", this, value, old);
            }
        }
        private double _multiplier;
        #endregion //Properties

        #region Constructors
        public TECManufacturer(Guid guid) : base(guid)
        {
            _multiplier = 1;
        }
        public TECManufacturer() : this(Guid.NewGuid()) { }

        public TECManufacturer(TECManufacturer manSource) : this(manSource.Guid)
        {
            _label = manSource.Label;
            _multiplier = manSource.Multiplier;
        }
        #endregion //Constructors
    }
}
