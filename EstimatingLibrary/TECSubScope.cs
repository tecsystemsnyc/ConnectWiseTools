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
    public class TECSubScope : TECLocated, INotifyPointChanged, IDragDropable, ITypicalable, INetworkConnectable
    {
        #region Properties
        private ObservableCollection<ITECConnectable> _devices;
        private ObservableCollection<TECPoint> _points;
        private TECConnection _connection { get; set; }

        public ObservableCollection<ITECConnectable> Devices
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
        public IOCollection AvailableNetworkIO
        {
            get { return getNetworkIO(); }
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
            _devices = new ObservableCollection<ITECConnectable>();
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
            foreach (ITECConnectable device in sourceSubScope.Devices)
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
                foreach (ITECConnectable item in e.NewItems)
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
                foreach (TECDevice item in e.OldItems)
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
            else if(TECIO.NetworkIO.Contains(point.Type) && Points.Count > 0)
            {
                return false;
            }
            else if (TECIO.PointIO.Contains(point.Type))
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
        }
        public void RemovePoint(TECPoint point)
        {
            Points.Remove(point);
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

        public void LinkConnection(TECSubScopeConnection connection)
        {
            _connection = connection;
        }

        override protected CostBatch getCosts()
        {
            CostBatch costs = base.getCosts();
            foreach (TECDevice dev in Devices)
            {
                costs += dev.CostBatch;
            }
            return costs;
        }
        protected override SaveableMap saveObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.saveObjects());
            List<TECObject> deviceList = new List<TECObject>();
            foreach (ITECConnectable item in this.Devices)
            {
                deviceList.Add(item as TECObject);
            }
            saveList.AddRange(deviceList, "Devices");
            saveList.AddRange(this.Points, "Points");
            return saveList;
        }
        protected override SaveableMap relatedObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.relatedObjects());
            List<TECObject> deviceList = new List<TECObject>();
            foreach (ITECConnectable item in this.Devices)
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
