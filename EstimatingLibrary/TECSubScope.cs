using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECSubScope : TECScope, CostComponent, PointComponent
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
        public TECSubScopeConnection Connection
        {
            get { return _connection; }
            set
            {
                _connection = value;
                RaisePropertyChanged("Connection");
            }
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

        public int PointNumber
        {
            get
            {
                return getPointNumber();
            }
        }

        public List<TECCost> Costs
        {
            get
            {
                return getCosts();
            }
        }
        private List<TECCost> getCosts()
        {
            var outCosts = new List<TECCost>();
            foreach(TECDevice dev in Devices)
            {
                outCosts.Add(dev);
                foreach(TECCost cost in dev.AssociatedCosts)
                {
                    outCosts.Add(cost);
                }
            }
            foreach(TECPoint point in Points)
            {
                foreach(TECCost cost in point.AssociatedCosts)
                {
                    outCosts.Add(cost);
                }
            }
            foreach(TECCost cost in AssociatedCosts)
            {
                outCosts.Add(cost);
            }
            return outCosts;
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
        public TECSubScope(TECSubScope sourceSubScope, Dictionary<Guid, Guid> guidDictionary = null,
            ObservableItemToInstanceList<TECScope> characteristicReference = null) : this()
        {
            if (characteristicReference == null)
            {
                characteristicReference = new ObservableItemToInstanceList<TECScope>();
            }
            if (guidDictionary != null)
            { guidDictionary[_guid] = sourceSubScope.Guid; }

            foreach (TECDevice device in sourceSubScope.Devices)
            { Devices.Add(new TECDevice(device)); }
            foreach (TECPoint point in sourceSubScope.Points)
            {
                var toAdd = new TECPoint(point);
                characteristicReference.AddItem(point,toAdd);
                Points.Add(toAdd);
            }

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
            foreach (TECPoint point in Points)
            {
                if (point.Type == PointTypes.AI) { _ai++; }
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
                RaisePropertyChanged("PointNumber");
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyPropertyChanged("Remove", this, item);
                }
                RaisePropertyChanged("PointNumber");
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
                    var old = generateOldCostComponent(Change.Add, item as TECDevice);
                    NotifyPropertyChanged<object>("CostComponentChanged", old, this);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyPropertyChanged("RemoveCatalog", this, item);
                    ((TECDevice)item).PropertyChanged -= DeviceChanged;
                    RaisePropertyChanged("TotalDevices");
                    var old = generateOldCostComponent(Change.Remove, item as TECDevice);
                    NotifyPropertyChanged<object>("CostComponentChanged", old, this);
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
            if(args != null)
            {
                if(e.PropertyName == "Cost" || e.PropertyName == "Manufacturer")
                {
                    var old = this.Copy() as TECSubScope;
                    old.Devices.Remove(args.NewValue as TECDevice);
                    old.Devices.Add(args.OldValue as TECDevice);
                    NotifyPropertyChanged("CostComponentChanged", old, this);
                }
            }
        }
        #endregion

        #region Methods

        public override Object Copy()
        {
            TECSubScope outScope = new TECSubScope();
            outScope._guid = Guid;
            var devices = new ObservableCollection<TECDevice>();
            foreach (TECDevice device in this.Devices)
            { devices.Add(device); }
            outScope._devices = devices;
            var points = new ObservableCollection<TECPoint>();
            foreach (TECPoint point in this.Points)
            { points.Add(point.Copy() as TECPoint); }
            outScope._points = points;
            outScope.reSubscribeToCollections();

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
            foreach (TECDevice device in Devices)
            {
                foreach(TECConnectionType type in device.ConnectionTypes)
                {
                    outTypes.Add(type);
                }
            }
            return outTypes;
        }

        private double getMaterialCost()
        {
            double matCost = 0;

            foreach (TECDevice device in this.Devices)
            {
                matCost += device.Cost * device.Manufacturer.Multiplier;
                foreach (TECCost cost in device.AssociatedCosts)
                {
                    matCost += cost.Cost;
                }
            }
            foreach (TECCost cost in this.AssociatedCosts)
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
            foreach (TECCost assCost in this.AssociatedCosts)
            {
                labCost += assCost.Labor;
            }

            return labCost;
        }
        private double getElectricalLabor()
        {
            double mountingLabor = 0;
            foreach(TECDevice device in Devices)
            {
                mountingLabor += .5;
            }
            return mountingLabor;
        }

        private void subscribeToDevices()
        {
            foreach (TECDevice item in this._devices)
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

            foreach (TECPoint point in Points)
            {
                allPointTypes.Add(point.Type);
            }

            return allPointTypes;
        }
        private ObservableCollection<IOType> getAllIOTypes()
        {
            var allIOTypes = new ObservableCollection<IOType>();

            foreach (TECDevice device in Devices)
            {
                allIOTypes.Add(device.IOType);
            }
            return allIOTypes;
        }

        private int getPointNumber()
        {
            var totalPoints = 0;
            foreach (TECPoint point in Points)
            {
                totalPoints += point.PointNumber;
            }
            return totalPoints;
        }

        private void reSubscribeToCollections()
        {
            Points.CollectionChanged += PointsCollectionChanged;
            subscribeToDevices();
            Devices.CollectionChanged += Devices_CollectionChanged;
        }

        private TECSubScope generateOldCostComponent(Change change, TECDevice device)
        {
            var old = this.Copy() as TECSubScope;
            var oldDevices = new ObservableCollection<TECDevice>();
            foreach(TECDevice oldDevice in old.Devices)
            {
                oldDevices.Add(oldDevice);
            }
            if(change == Change.Add)
            {
                oldDevices.Remove(device);
            } else
            {
                oldDevices.Add(device);
            }
            old._devices = oldDevices;
            return old;
        }
        #endregion
    }
}
