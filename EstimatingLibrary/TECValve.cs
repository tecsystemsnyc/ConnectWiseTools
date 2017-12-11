using EstimatingLibrary.Interfaces;
using System;
using System.Collections.ObjectModel;

namespace EstimatingLibrary
{
    public class TECValve: TECHardware, IEndDevice, ICatalog<TECValve>
    {
        #region Constants
        private const CostType COST_TYPE = CostType.TEC;
        #endregion

        #region Fields
        private TECDevice _actuator;
        private double _cv;
        private double _size;
        private string _style;

        #endregion

        #region Properties
        public TECDevice Actuator
        {
            get { return _actuator; }
            set
            {
                var old = Actuator;
                _actuator = value;
                notifyCombinedChanged(Change.Edit, "Actuator", this, _actuator, old);
                notifyCostChanged(value.CostBatch - old.CostBatch);
            }
        }
        public double Cv
        {
            get { return _cv; }
            set
            {
                var old = _cv;
                _cv = value;
                notifyCombinedChanged(Change.Edit, "Cv", this, _cv, old);
            }
        }
        public double Size
        {
            get { return _size; }
            set
            {
                var old = _size;
                _size = value;
                notifyCombinedChanged(Change.Edit, "Size", this, _size, old);
            }
        }
        public string Style
        {
            get { return _style; }
            set
            {
                var old = _style;
                _style = value;
                notifyCombinedChanged(Change.Edit, "Style", this, _style, old);
            }
        }
        
        public ObservableCollection<TECConnectionType> ConnectionTypes
        {
            get
            {
                return Actuator.ConnectionTypes;
            }
        }

        public override double Cost
        {
            get
            {
                return base.Cost + Actuator.Cost;
            }
        }
        #endregion

        public TECValve(TECManufacturer manufacturer, TECDevice actuator) : this (Guid.NewGuid(), manufacturer, actuator) {}
        public TECValve(Guid guid, TECManufacturer manufacturer, TECDevice actuator) : base(guid, manufacturer, COST_TYPE)
        {
            _style = "";
            _size = 0;
            _cv = 0;
            _actuator = actuator;
        }
        public TECValve(TECValve valveSource) : this(valveSource.Manufacturer, valveSource.Actuator)
        {
            this.copyPropertiesFromHardware(valveSource);

        }
        protected override SaveableMap propertyObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.propertyObjects());
            saveList.Add(this.Actuator, "Actuator");
            return saveList;
        }
        protected override SaveableMap linkedObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.linkedObjects());
            saveList.Add(this.Actuator, "Actuator");
            return saveList;
        }

        public TECValve CatalogCopy()
        {
            return new TECValve(this);
        }
    }
}
