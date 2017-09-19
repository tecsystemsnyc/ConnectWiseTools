using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public abstract class TECHardware : TECCost
    {
        #region Fields
        private TECManufacturer _manufacturer;
        private double _price;
        #endregion

        #region Constructors
        public TECHardware(Guid guid, TECManufacturer manufacturer, CostType type) : base(guid, type)
        {
            _price = 0;
            _manufacturer = manufacturer;
        }
        #endregion

        #region Properties
        public TECManufacturer Manufacturer
        {
            get { return _manufacturer; }
            set
            {
                if(_manufacturer != value)
                {
                    var old = Manufacturer;
                    var originalCost = CostBatch;
                    _manufacturer = value;
                    notifyCombinedChanged(Change.Edit, "Manufacturer", this, value, old);
                    notifyCostChanged(CostBatch - originalCost);
                }
            }
        }
        public override double Cost
        {
            get
            {
                return Price * Manufacturer.Multiplier;
            }
        }
        public double Price
        {
            get { return _price; }
            set
            {
                var old = Price;
                _price = value;
                notifyCombinedChanged(Change.Edit, "Price", this, value, old);
                notifyCostChanged(new CostBatch(value - old, 0, Type));
            }
        }
        #endregion
        
        #region Methods
        protected void copyPropertiesFromHardware(TECHardware hardware)
        {
            copyPropertiesFromCost(hardware);
            _manufacturer = hardware.Manufacturer;
            _price = hardware.Price;
        }
        
        protected override SaveableMap saveObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.saveObjects());
            saveList.Add(this.Manufacturer, "Manufacturer");
            return saveList;
        }
        protected override SaveableMap relatedObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.relatedObjects());
            saveList.Add(this.Manufacturer, "Manufacturer");
            return saveList;
        }
        #endregion
    }
}
