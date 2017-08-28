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
    public class TECSubScope : TECLocated, INotifyPointChanged, IDragDropable
    {
        #region Properties
        private ObservableCollection<ITECConnectable> _devices;
        private ObservableCollection<TECPoint> _points;
        private TECSubScopeConnection _connection { get; set; }

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
                NotifyCombinedChanged(Change.Edit, "Devices", this, value, old);
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
                NotifyCombinedChanged(Change.Edit, "Points", this, value, old);
            }
        }
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
        public int PointNumber
        {
            get
            {
                return getPointNumber();
            }
        }
        
        #endregion //Properties

        #region Constructors
        public TECSubScope(Guid guid) : base(guid)
        {
            _devices = new ObservableCollection<ITECConnectable>();
            _points = new ObservableCollection<TECPoint>();
            Devices.CollectionChanged += Devices_CollectionChanged;
            Points.CollectionChanged += PointsCollectionChanged;
        }
        public TECSubScope() : this(Guid.NewGuid()) { }

        //Copy Constructor
        public TECSubScope(TECSubScope sourceSubScope, Dictionary<Guid, Guid> guidDictionary = null,
            ListDictionary<TECObject> characteristicReference = null) : this()
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

        #region Events
        public event Action<int> PointChanged;
        #endregion

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
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                int pointNumber = 0;
                foreach (TECPoint item in e.NewItems)
                {
                    pointNumber += item.PointNumber;
                    NotifyCombinedChanged(Change.Add, "Points", this, item);
                }
                PointChanged?.Invoke(pointNumber);
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                int pointNumber = 0;
                foreach (object item in e.OldItems)
                {
                    NotifyCombinedChanged(Change.Remove, "Points", this, item);
                }
                PointChanged?.Invoke(pointNumber * -1);
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                NotifyCombinedChanged(Change.Edit, "Points", this, sender);
            }
        }
        private void Devices_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                CostBatch costs = new CostBatch();
                foreach (TECDevice item in e.NewItems)
                {
                    NotifyCombinedChanged(Change.Add, "Devices", this, item);
                    costs += item.CostBatch;
                }
                NotifyCostChanged(costs);
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                CostBatch costs = new CostBatch();
                foreach (TECDevice item in e.OldItems)
                {
                    NotifyCombinedChanged(Change.Remove, "Devices", this, item);
                    costs += item.CostBatch;
                }
                NotifyCostChanged(costs * -1);
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                NotifyCombinedChanged(Change.Edit, "Devices", this, sender);
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
        #endregion
    }
}
