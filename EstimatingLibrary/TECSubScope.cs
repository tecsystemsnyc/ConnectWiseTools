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
    public class TECSubScope : TECLocated, INotifyCostChanged, PointComponent, DragDropComponent
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
                var old = Devices;
                _devices = value;
                NotifyPropertyChanged(Change.Edit, "Devices", this, value, old);
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
                var old = Points;
                _points = value;
                Points.CollectionChanged += PointsCollectionChanged;
                NotifyPropertyChanged(Change.Edit, "Points", this, value, old);
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

        public ObservableCollection<TECElectricalMaterial> ConnectionTypes
        {
            get { return getConnectionTypes(); }
        }
        public List<TECElectricalMaterial> AvailableConnections
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
            ObservableItemToInstanceList<TECObject> characteristicReference = null) : this()
        {
            if (guidDictionary != null)
            { guidDictionary[_guid] = sourceSubScope.Guid; }
            foreach (TECDevice device in sourceSubScope.Devices)
            { _devices.Add(new TECDevice(device)); }
            var subWatch = System.Diagnostics.Stopwatch.StartNew();
            foreach (TECPoint point in sourceSubScope.Points)
            {
                var toAdd = new TECPoint(point);
                characteristicReference?.AddItem(point,toAdd);
                Points.Add(toAdd);
            }
            this.copyPropertiesFromScope(sourceSubScope);
        }
        #endregion //Constructors

        #region Num Point Types
        //private int _ai;
        //private int _ao;
        //private int _bi;
        //private int _bo;
        //private int _serial;

        //public int AI { get { return _ai; } }
        //public int AO { get { return _ao; } }
        //public int BI { get { return _bi; } }
        //public int BO { get { return _bo; } }
        //public int Serial { get { return _serial; } }

        #endregion //Num Point Types

        #region Event Handlers
        private void PointsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //_ai = 0;
            //_ao = 0;
            //_bi = 0;
            //_bo = 0;
            //_serial = 0;
            //foreach (TECPoint point in Points)
            //{
            //    if (point.Type == PointTypes.AI) { _ai++; }
            //    else if (point.Type == PointTypes.AO) { _ao++; }
            //    else if (point.Type == PointTypes.BI) { _bi++; }
            //    else if (point.Type == PointTypes.BO) { _bo++; }
            //    else if (point.Type == PointTypes.Serial) { _serial++; }
            //    else
            //    {
            //        string message = "Invalid Point Type in PointsColllectionChanged in TECSubScope";
            //        throw new InvalidCastException(message);
            //    }
            //}

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    NotifyPropertyChanged(Change.Add, "Points", this, item);
                }
                RaisePropertyChanged("PointNumber");
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyPropertyChanged(Change.Remove, "Points", this, item);
                }
                RaisePropertyChanged("PointNumber");
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                NotifyPropertyChanged(Change.Edit, "Points", this, sender);
            }
        }
        private void Devices_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    NotifyPropertyChanged(Change.Add, "Devices", this, item);
                    ((TECDevice)item).PropertyChanged += DeviceChanged;
                    RaisePropertyChanged("TotalDevices");
                    //var old = generateOldINotifyCostChanged(Change.Add, item as TECDevice);
                    //NotifyPropertyChanged<object>("INotifyCostChangedChanged", old, this);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyPropertyChanged(Change.Remove, "Devices", this, item);
                    ((TECDevice)item).PropertyChanged -= DeviceChanged;
                    RaisePropertyChanged("TotalDevices");
                    //var old = generateOldINotifyCostChanged(Change.Remove, item as TECDevice);
                    //NotifyPropertyChanged<object>("INotifyCostChangedChanged", old, this);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                NotifyPropertyChanged(Change.Edit, "Devices", this, sender);
            }
        }
        private void DeviceChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyChangedExtendedEventArgs args = e as PropertyChangedExtendedEventArgs;
            if (e.PropertyName == "Quantity")
            {
                //NotifyPropertyChanged("ChildChanged", (object)this, (object)args.NewValue);
            }
            if(args != null)
            {
                if(e.PropertyName == "Cost" || e.PropertyName == "Manufacturer")
                {
                    //var old = this.Copy() as TECSubScope;
                    //old.Devices.Remove(args.NewValue as TECDevice);
                    //old.Devices.Add(args.OldValue as TECDevice);
                    //NotifyPropertyChanged("INotifyCostChangedChanged", old, this);
                }
            }
        }
        #endregion

        #region Methods
        public object DragDropCopy(TECScopeManager scopeManager)
        {
            TECSubScope outScope = new TECSubScope(this);
            ModelLinkingHelper.LinkScopeItem(outScope, scopeManager);
            return outScope;
        }

        private ObservableCollection<TECElectricalMaterial> getConnectionTypes()
        {
            var outTypes = new ObservableCollection<TECElectricalMaterial>();
            foreach (TECDevice device in Devices)
            {
                foreach(TECElectricalMaterial type in device.ConnectionTypes)
                {
                    outTypes.Add(type);
                }
            }
            return outTypes;
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

        private List<TECElectricalMaterial> getAvailableConnectionTypes()
        {
            var availableConnections = new List<TECElectricalMaterial>();
            foreach (TECElectricalMaterial conType in this.ConnectionTypes)
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

        public void LinkConnection(TECSubScopeConnection connection)
        {
            _connection = connection;
        }
        #endregion
    }
}
