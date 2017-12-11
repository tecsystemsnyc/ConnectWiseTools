using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace EstimatingLibrary
{
    public class TECSubScope : TECLocated, INotifyPointChanged, IDragDropable, ITypicalable, INetworkConnectable
    {
        #region Properties
        private ObservableCollection<IEndDevice> _devices;
        private ObservableCollection<TECPoint> _points;
        private TECConnection _connection;

        public ObservableCollection<IEndDevice> Devices
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
                notifyCombinedChanged(Change.Edit, "Devices", this, value, old);
                Devices.CollectionChanged += Devices_CollectionChanged;
            }
        }
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
                notifyCombinedChanged(Change.Edit, "Points", this, value, old);
            }
        }
        public TECConnection Connection
        {
            get { return _connection; }
            set
            {
                _connection = value;
                raisePropertyChanged("Connection");
            }
        }
        public TECNetworkConnection ParentConnection
        {
            get
            {
                if(Connection is TECNetworkConnection netConn)
                {
                    return netConn;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Connection = value;
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
        public int PointNumber
        {
            get
            {
                return getPointNumber();
            }
        }
        
        public bool IsTypical { get; private set; }

        //Derived
        public List<TECIO> AllNetworkIOList
        {
            get { return getNetworkIO().ListIO(); }
        }
        public IOCollection AvailableNetworkIO
        {
            get { return getNetworkIO(); }
        }
        public IOCollection IO
        {
            get
            {
                IOCollection ssIO = new IOCollection();
                foreach (TECPoint point in Points)
                {
                    for (int i = 0; i < point.Quantity; i++)
                    {
                        ssIO.AddIO(point.Type);
                    }
                }
                return ssIO;
            }
        }
        public bool IsNetwork
        {
            get { return isNetwork(); }
        }
        
        #endregion //Properties

        #region Constructors
        public TECSubScope(Guid guid, bool isTypical) : base(guid)
        {
            IsTypical = isTypical;
            _devices = new ObservableCollection<IEndDevice>();
            _points = new ObservableCollection<TECPoint>();
            Devices.CollectionChanged += Devices_CollectionChanged;
            Points.CollectionChanged += PointsCollectionChanged;
        }
        public TECSubScope(bool isTypical) : this(Guid.NewGuid(), isTypical) { }

        //Copy Constructor
        public TECSubScope(TECSubScope sourceSubScope, bool isTypical, Dictionary<Guid, Guid> guidDictionary = null,
            ObservableListDictionary<TECObject> characteristicReference = null) : this(isTypical)
        {
            if (guidDictionary != null)
            { guidDictionary[_guid] = sourceSubScope.Guid; }
            foreach (IEndDevice device in sourceSubScope.Devices)
            { _devices.Add(device); }
            foreach (TECPoint point in sourceSubScope.Points)
            {
                var toAdd = new TECPoint(point, isTypical);
                characteristicReference?.AddItem(point,toAdd);
                Points.Add(toAdd);
            }
            this.copyPropertiesFromScope(sourceSubScope);
        }
        #endregion //Constructors

        #region Events
        public event Action<int> PointChanged;
        #endregion
        
        #region Event Handlers
        private void PointsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            raisePropertyChanged("PointNumber");
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                int pointNumber = 0;
                foreach (TECPoint item in e.NewItems)
                {
                    pointNumber += item.PointNumber;
                    notifyCombinedChanged(Change.Add, "Points", this, item);
                }
                notifyPointChanged(pointNumber);
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                int pointNumber = 0;
                foreach (TECPoint item in e.OldItems)
                {
                    pointNumber += item.PointNumber;
                    notifyCombinedChanged(Change.Remove, "Points", this, item);
                }
                notifyPointChanged(pointNumber * -1);
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                notifyCombinedChanged(Change.Edit, "Points", this, sender);
            }
        }
        private void Devices_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                CostBatch costs = new CostBatch();
                foreach (IEndDevice item in e.NewItems)
                {
                    notifyCombinedChanged(Change.Add, "Devices", this, item);
                    if(item is INotifyCostChanged costly)
                    {
                        costs += costly.CostBatch;
                    }
                }
                notifyCostChanged(costs);
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                CostBatch costs = new CostBatch();
                foreach (IEndDevice item in e.OldItems)
                {
                    notifyCombinedChanged(Change.Remove, "Devices", this, item);
                    if (item is INotifyCostChanged costly)
                    {
                        costs += costly.CostBatch;
                    }
                }
                notifyCostChanged(costs * -1);
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                notifyCombinedChanged(Change.Edit, "Devices", this, sender);
            }
        }
        #endregion

        #region Methods
        public object DragDropCopy(TECScopeManager scopeManager)
        {
            TECSubScope outScope = new TECSubScope(this, this.IsTypical);
            ModelLinkingHelper.LinkScopeItem(outScope, scopeManager);
            return outScope;
        }
        
        public bool CanAddPoint(TECPoint point)
        {
            if(Points.Count == 0)
            {
                return true;
            }
            IOCollection networkIO = getNetworkIO();
            if(TECIO.NetworkIO.Contains(point.Type) && networkIO.Contains(point.Type))
            {
                return true;
            }
            else if (TECIO.PointIO.Contains(point.Type) && networkIO.ListIO().Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void AddPoint(TECPoint point)
        {
            if (CanAddPoint(point))
            {
                Points.Add(point);
            }
            else
            {
                throw new InvalidOperationException("Point incompatible with SubScope.");
            }
        }
        public void RemovePoint(TECPoint point)
        {
            Points.Remove(point);
        }

        public bool CanConnectToNetwork(TECNetworkConnection netConnect)
        {
            if (!this.IsNetwork) return false;

            IOCollection thisIO = new IOCollection(getNetworkIO());
            IOCollection netConnectIO = netConnect.IO;
            bool ioMatches = IOCollection.IOTypesMatch(thisIO, netConnectIO);

            bool connectionTypesMatch = (this.ConnectionTypes.Except(netConnect.ConnectionTypes).Count() == 0) 
                && (netConnect.ConnectionTypes.Except(this.ConnectionTypes).Count() == 0);

            return (ioMatches && connectionTypesMatch);
        }

        private ObservableCollection<TECElectricalMaterial> getConnectionTypes()
        {
            var outTypes = new ObservableCollection<TECElectricalMaterial>();
            foreach (IEndDevice device in Devices)
            {
                foreach(TECElectricalMaterial type in device.ConnectionTypes)
                {
                    outTypes.Add(type);
                }
            }
            return outTypes;
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
            Devices.CollectionChanged += Devices_CollectionChanged;
        }
        private bool isNetwork()
        {
            if(Points.Count != 1)
            {
                return false;
            } else
            {
                return TECIO.NetworkIO.Contains(Points[0].Type);
            }
        }

        protected override CostBatch getCosts()
        {
            CostBatch costs = base.getCosts();
            foreach (IEndDevice dev in Devices)
            {
                if(dev is INotifyCostChanged costDev)
                {
                    costs += costDev.CostBatch;
                }
                else
                {
                    throw new Exception("This contains an unsupported end device.");
                }
            }
            return costs;
        }
        protected override SaveableMap propertyObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.propertyObjects());
            List<TECObject> deviceList = new List<TECObject>();
            foreach (IEndDevice item in this.Devices)
            {
                deviceList.Add(item as TECObject);
            }
            saveList.AddRange(deviceList, "Devices");
            saveList.AddRange(this.Points, "Points");
            return saveList;
        }
        protected override SaveableMap linkedObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.linkedObjects());
            List<TECObject> deviceList = new List<TECObject>();
            foreach (IEndDevice item in this.Devices)
            {
                deviceList.Add(item as TECObject);
            }
            saveList.AddRange(deviceList.Distinct(), "Devices");
            return saveList;
        }
        protected override void notifyCostChanged(CostBatch costs)
        {
            if (!IsTypical)
            {
                base.notifyCostChanged(costs);
            }
        }
        private void notifyPointChanged(int numPoints)
        {
            if (!IsTypical)
            {
                PointChanged?.Invoke(numPoints);
            }
        }
        private IOCollection getNetworkIO()
        {
            IOCollection collection = new IOCollection();
            foreach (TECPoint point in Points.Where(point => TECIO.NetworkIO.Contains(point.Type)))
            {
                for (int i = 0; i < point.Quantity; i++)
                {
                    collection.AddIO(point.Type);
                }
            }
            return collection;
        }

        public INetworkConnectable Copy(INetworkConnectable item, bool isTypical, Dictionary<Guid, Guid> guidDictionary)
        {
            return new TECSubScope(item as TECSubScope, isTypical, guidDictionary);
        }
        #endregion
    }
}
