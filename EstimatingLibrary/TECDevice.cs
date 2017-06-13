using EstimatingLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECDevice : TECCost
    {
        #region Properties
        private ObservableCollection<TECConnectionType> _connectionTypes;
        private IOType _ioType;
        private TECManufacturer _manufacturer;
        
        public ObservableCollection<TECConnectionType> ConnectionTypes
        {
            get { return _connectionTypes; }
            set
            {
                if(ConnectionTypes != null)
                {
                    ConnectionTypes.CollectionChanged -= ConnectionTypes_CollectionChanged;
                }
                var temp = this.Copy();
                _connectionTypes = value; if (ConnectionTypes != null)
                {
                    ConnectionTypes.CollectionChanged += ConnectionTypes_CollectionChanged;
                }
                NotifyPropertyChanged("ConnectionTypes", temp, this);
                NotifyPropertyChanged("ChildChanged", (object)this, (object)value);
            }
        }
        
        public IOType IOType
        {
            get { return _ioType; }
            set
            {
                var temp = this.Copy();
                _ioType = value;
                NotifyPropertyChanged("IOType", temp, this);
                NotifyPropertyChanged("ChildChanged", (object)this, (object)value);
            }
        }
        public TECManufacturer Manufacturer
        {
            get { return _manufacturer; }
            set
            {
                var temp = this.Copy();
                _manufacturer = value;
                NotifyPropertyChanged("Manufacturer", temp, this);
                NotifyPropertyChanged("ChildChanged", (object)this, (object)value);
            }
        }
        public double MaterialCost
        {
            get { return getMaterialCost(); }
        }
        public double LaborCost
        {
            get { return getLaborCost(); }
        }

        public double ElectricalCost
        {
            get { return 0; }
        }
        public double ElectricalLabor
        {
            get { return 0; }
        }
        #endregion//Properties

        #region Constructors
        public TECDevice(Guid guid, ObservableCollection<TECConnectionType> connectionTypes, TECManufacturer manufacturer) : base(guid)
        {
            _cost = 0;
            _connectionTypes = connectionTypes;
            _manufacturer = manufacturer;
            _type = CostType.TEC;
            ConnectionTypes.CollectionChanged += ConnectionTypes_CollectionChanged;
        }
        public TECDevice(ObservableCollection<TECConnectionType> connectionTypes, TECManufacturer manufacturer) : this(Guid.NewGuid(), connectionTypes, manufacturer) { }

        //Copy Constructor
        public TECDevice(TECDevice deviceSource)
            : this(deviceSource.Guid, deviceSource.ConnectionTypes, deviceSource.Manufacturer)
        {
            this.copyPropertiesFromScope(deviceSource);
            _cost = deviceSource.Cost;
            _ioType = deviceSource.IOType;
        }
        #endregion //Constructors

        #region Methods
        public override Object Copy()
        {
            TECDevice outDevice = new TECDevice(this);
            return outDevice;
        }

        public override Object DragDropCopy()
        {
            return this;
        }

        private double getMaterialCost()
        {
            double matCost = 0;
            foreach (TECCost cost in this.AssociatedCosts)
            {
                matCost += cost.Cost;
            }
            return matCost;
        }
        private double getLaborCost()
        {
            double cost = 0;
            foreach (TECCost assCost in this.AssociatedCosts)
            {
                cost += assCost.Labor;
            }
            return cost;
        }

        private void ConnectionTypes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (TECConnectionType type in e.NewItems)
                {
                    NotifyPropertyChanged<object>("AddCatalog", this, type);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (TECConnectionType type in e.OldItems)
                {
                    NotifyPropertyChanged<object>("RemoveCatalog", this, type);
                }
            }
        }

        #endregion
    }
}
