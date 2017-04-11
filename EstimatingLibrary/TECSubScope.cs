using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECSubScope : TECScope
    {
        #region Properties
        private ObservableCollection<TECDevice> _devices;
        public ObservableCollection<TECDevice> Devices
        {
            get { return _devices; }
            set
            {
                if (Devices != null)
                {
                    Devices.CollectionChanged -= Devices_CollectionChanged;
                }
                var temp = this.Copy();
                _devices = value;
                NotifyPropertyChanged("Devices", temp, this);
                Devices.CollectionChanged += Devices_CollectionChanged;
            }
        }

        private ObservableCollection<TECPoint> _points;
        public ObservableCollection<TECPoint> Points
        {
            get { return _points; }
            set
            {
                if (Points != null)
                {
                    Points.CollectionChanged -= PointsCollectionChanged;
                }
                var temp = this.Copy();
                _points = value;
                Points.CollectionChanged += PointsCollectionChanged;
                NotifyPropertyChanged("Points", temp, this);
            }
        }
        
        private TECSubScopeConnection _connection { get; set; }
        public TECSubScopeConnection Connection {
            get { return _connection; }
            set
            {
                _connection = value;
                RaisePropertyChanged("Connection");
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

        public ObservableCollection<TECConnectionType> ConnectionTypes
        {
            get { return getConnectionTypes(); }
        }
        public List<TECConnectionType> AvailableConnections
        {
            get { return getAvailableConnectionTypes(); }
        }
        public ObservableCollection<PointTypes> AllPointTypes
        {
            get { return getAllPointTypes(); }
        }
        public ObservableCollection<IOType> AllIOTypes
        {
            get { return getAllIOTypes(); }
        }
        
        #endregion //Properties

        #region Constructors
        public TECSubScope(Guid guid) : base(guid)
        {
            _devices = new ObservableCollection<TECDevice>();
            _points = new ObservableCollection<TECPoint>();
            subscribeToDevices();
            Devices.CollectionChanged += Devices_CollectionChanged;
            Points.CollectionChanged += PointsCollectionChanged;
        }
        
        public TECSubScope() : this(Guid.NewGuid()) { }

        //Copy Constructor
        public TECSubScope(TECSubScope sourceSubScope, Dictionary<Guid, Guid> guidDictionary = null) : this()
        {
            if(guidDictionary != null)
            { guidDictionary[_guid] = sourceSubScope.Guid; }
            
            foreach(TECDevice device in sourceSubScope.Devices)
            { Devices.Add(new TECDevice(device)); }
            foreach(TECPoint point in sourceSubScope.Points)
            { Points.Add(new TECPoint(point)); }
            
            this.copyPropertiesFromScope(sourceSubScope);
        }
        #endregion //Constructors

        #region Num Point Types
        private int _ai;
        private int _ao;
        private int _bi;
        private int _bo;
        private int _serial;

        public int AI { get { return _ai; } }
        public int AO { get { return _ao; } }
        public int BI { get { return _bi; } }
        public int BO { get { return _bo; } }
        public int Serial { get { return _serial; } }
        #endregion //Num Point Types

        #region Event Handlers
        private void PointsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _ai = 0;
            _ao = 0;
            _bi = 0;
            _bo = 0;
            _serial = 0;
            foreach(TECPoint point in Points)
            {
                if      (point.Type == PointTypes.AI) { _ai++; }
                else if (point.Type == PointTypes.AO) { _ao++; }
                else if (point.Type == PointTypes.BI) { _bi++; }
                else if (point.Type == PointTypes.BO) { _bo++; }
                else if (point.Type == PointTypes.Serial) { _serial++; }
                else
                {
                    string message = "Invalid Point Type in PointsColllectionChanged in TECSubScope";
                    throw new InvalidCastException(message);
                }
            }

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    NotifyPropertyChanged("Add", this, item);
                }
                RaisePropertyChanged("TotalPoints");
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyPropertyChanged("Remove", this, item);
                }
                RaisePropertyChanged("TotalPoints");
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                NotifyPropertyChanged("Edit", this, sender);
            }
        } 
        private void Devices_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    NotifyPropertyChanged("AddCatalog", this, item);
                    ((TECDevice)item).PropertyChanged += DeviceChanged;
                    RaisePropertyChanged("TotalDevices");
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyPropertyChanged("RemoveCatalog", this, item);
                    ((TECDevice)item).PropertyChanged -= DeviceChanged;
                    RaisePropertyChanged("TotalDevices");
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                NotifyPropertyChanged("Edit", this, sender);
            }
        } 
        private void DeviceChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;
            if (e.PropertyName == "Quantity")
            {
                NotifyPropertyChanged("ChildChanged", (object)this, (object)args.NewValue);
            }
        }
        #endregion

        #region Methods

        public override Object Copy()
        {
            TECSubScope outScope = new TECSubScope();
            outScope._guid = Guid;
            foreach (TECDevice device in this.Devices)
            { outScope.Devices.Add(device.Copy() as TECDevice); }
            foreach (TECPoint point in this.Points)
            { outScope.Points.Add(point.Copy() as TECPoint); }

            outScope.copyPropertiesFromScope(this);
            return outScope;
        }

        public override object DragDropCopy()
        {
            TECSubScope outScope = new TECSubScope(this);
            return outScope;
        }

        private ObservableCollection<TECConnectionType> getConnectionTypes()
        {
            var outTypes = new ObservableCollection<TECConnectionType>();
            foreach(TECDevice device in Devices)
            {
                outTypes.Add(device.ConnectionType);
            }
            return outTypes;
        }

        private double getMaterialCost()
        {
            double matCost = 0;

            foreach(TECDevice device in this.Devices)
            {
                matCost += device.Cost * device.Manufacturer.Multiplier;
                foreach(TECAssociatedCost cost in device.AssociatedCosts)
                {
                    matCost += cost.Cost;
                }
            }
            foreach(TECAssociatedCost cost in this.AssociatedCosts)
            {
                matCost += cost.Cost;
            }

            return matCost;
        }

        private double getLaborCost()
        {
            double labCost = 0;

            foreach (TECDevice device in this.Devices)
            {
                labCost += device.LaborCost;
            }
            foreach (TECAssociatedCost assCost in this.AssociatedCosts)
            {
                labCost += assCost.Labor;
            }

            return labCost;
        }

        private void subscribeToDevices()
        {
            foreach(TECDevice item in this._devices)
            {
                item.PropertyChanged += DeviceChanged;
            }
        }

        private void unSubscribeToDevices()
        {
            foreach (TECDevice item in this._devices)
            {
                item.PropertyChanged -= DeviceChanged;
            }
        }

        private List<TECConnectionType> getAvailableConnectionTypes()
        {
            var availableConnections = new List<TECConnectionType>();
            foreach (TECConnectionType conType in this.ConnectionTypes)
            {
                availableConnections.Add(conType);
            }

            return availableConnections;
        }

        private ObservableCollection<PointTypes> getAllPointTypes()
        {
            var allPointTypes = new ObservableCollection<PointTypes>();

            foreach(TECPoint point in Points)
            {
                allPointTypes.Add(point.Type);
            }
            
            return allPointTypes;
        }

        private ObservableCollection<IOType> getAllIOTypes()
        {
            var allIOTypes = new ObservableCollection<IOType>();

            foreach(TECDevice device in Devices)
            {
                allIOTypes.Add(device.IOType);
            }
            return allIOTypes;
        }
        #endregion
    }
}
