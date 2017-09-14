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
        #endregion

        #region Constructors
        public TECHardware(Guid guid, TECManufacturer manufacturer, CostType type) : base(guid, type)
        {
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
                    NotifyCombinedChanged(Change.Edit, "Manufacturer", this, value, old);
                    NotifyCostChanged(CostBatch - originalCost);
                }
            }
        }
        public new double Cost
        {
            get
            {
                return Price * Manufacturer.Multiplier;
            }
        }
        public double Price
        {
            get
            {
                return base.Cost;
            }
            set
            {
                base.Cost = value;
            }
        }
        #endregion
        
        #region Methods
        protected void copyPropertiesFromHardware(TECHardware hardware)
        {
            copyPropertiesFromCost(hardware);
            _manufacturer = hardware.Manufacturer;
        }

        override protected CostBatch getCosts()
        {
            CostBatch costs = base.getCosts() - new CostBatch(Price, Labor, Type);
            costs += new CostBatch(Cost, Labor, Type);
            return costs;
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
