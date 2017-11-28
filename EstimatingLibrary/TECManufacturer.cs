using EstimatingLibrary.Interfaces;
using System;

namespace EstimatingLibrary
{
    public class TECManufacturer : TECLabeled, ICatalog<TECManufacturer>
    {
        #region Properties
        public double Multiplier
        {
            get { return _multiplier; }
            set
            {
                var old = Multiplier;
                _multiplier = value;
                notifyCombinedChanged(Change.Edit, "Multiplier", this, value, old);
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
        public TECManufacturer CatalogCopy()
        {
            return new TECManufacturer(this);
        }

        public override object DragDropCopy(TECScopeManager manager)
        {
            return this;
        }
    }
}
