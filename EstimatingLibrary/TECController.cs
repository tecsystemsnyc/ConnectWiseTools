using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECController : TECLocated, IDragDropable, ITypicalable, INetworkConnectable, INetworkParentable
    {
        #region Properties
        //---Stored---
        private TECNetworkConnection _parentConnection;
        private ObservableCollection<TECConnection> _childrenConnections;
        private TECControllerType _type;
        private bool _isServer;
        private ObservableCollection<TECIOModule> _ioModules;
        
        public TECNetworkConnection ParentConnection
        {
            get { return _parentConnection; }
            set
            {
                var old = ParentConnection;
                _parentConnection = value;
                raisePropertyChanged("ParentConnection");
                raisePropertyChanged("NetworkIO");
            }
        }
        public ObservableCollection<TECConnection> ChildrenConnections
        {
            get { return _childrenConnections; }
            set
            {
                var old = ChildrenConnections;
                ChildrenConnections.CollectionChanged -= (sender, args) => collectionChanged(sender, args, "ChildrenConnections");
                _childrenConnections = value;
                ChildrenConnections.CollectionChanged += (sender, args) => collectionChanged(sender, args, "ChildrenConnections");
                notifyCombinedChanged(Change.Edit, "ChildrenConnections", this, value, old);
                raisePropertyChanged("ChildNetworkConnections");
            }
        }
        public TECControllerType Type
        {
            get { return _type; }
            set
            {
                var old = Type;
                _type = value;
                notifyCombinedChanged(Change.Edit, "Type", this, value, old);
                notifyCostChanged(value.CostBatch - old.CostBatch);
            }
        }
        public bool IsServer
        {
            get { return _isServer; }
            set
            {
                var old = IsServer;
                _isServer = value;
                notifyCombinedChanged(Change.Edit, "IsServer", this, value, old);
            }
        }
        public ObservableCollection<TECIOModule> IOModules
        {
            get { return _ioModules; }
            set
            {
                var old = IOModules;
                IOModules.CollectionChanged -= (sender, args) => collectionChanged(sender, args, "IOModules");
                _ioModules = value;
                IOModules.CollectionChanged += (sender, args) => collectionChanged(sender, args, "IOModules");
                notifyCombinedChanged(Change.Edit, "IOModules", this, value, old);
            }
        }

        public bool IsTypical
        {
            get; private set;
        }

        //---Derived---
        public IOCollection AvailableNetworkIO
        {
            get { return getAvailableNetworkIO(); }
        }
        #endregion

        #region Constructors
        public TECController(Guid guid, TECControllerType type, bool isTypical) : base(guid)
        {
            _isServer = false;
            IsTypical = isTypical;
            _type = type;
            _childrenConnections = new ObservableCollection<TECConnection>();
            _ioModules = new ObservableCollection<TECIOModule>();
            ChildrenConnections.CollectionChanged += (sender, args) => collectionChanged(sender, args, "ChildrenConnections");
            IOModules.CollectionChanged += (sender, args) => collectionChanged(sender, args, "IOModules");
        }

        public TECController(TECControllerType type, bool isTypical) : this(Guid.NewGuid(), type, isTypical) { }
        public TECController(TECController controllerSource, bool isTypical, Dictionary<Guid, Guid> guidDictionary = null) : this(controllerSource.Type, isTypical)
        {
            if (guidDictionary != null)
            { guidDictionary[_guid] = controllerSource.Guid; }
            copyPropertiesFromLocated(controllerSource);
            foreach (TECConnection connection in controllerSource.ChildrenConnections)
            {
                if (connection is TECSubScopeConnection)
                {
                    TECSubScopeConnection connectionToAdd = new TECSubScopeConnection(connection as TECSubScopeConnection, isTypical, guidDictionary);
                    connectionToAdd.ParentController = this;
                    _childrenConnections.Add(connectionToAdd);
                }
                else if (connection is TECNetworkConnection)
                {

                    TECNetworkConnection connectionToAdd = new TECNetworkConnection(connection as TECNetworkConnection, isTypical, guidDictionary);
                    connectionToAdd.ParentController = this;
                    _childrenConnections.Add(connectionToAdd);
                }
            }
        }
        #endregion

        #region Connection Methods
        public bool CanAddNetworkConnection(IOType ioType)
        {
            return (AvailableNetworkIO.Contains(ioType));
        }
        public TECNetworkConnection AddNetworkConnection(bool isTypical, IEnumerable<TECElectricalMaterial> connectionTypes, IOType ioType)
        {
            if (CanAddNetworkConnection(ioType))
            {
                TECNetworkConnection netConnect = new TECNetworkConnection(isTypical);
                netConnect.ConnectionTypes = new ObservableCollection<TECElectricalMaterial>(connectionTypes);
                netConnect.IOType = ioType;
                addChildConnection(netConnect);
                return netConnect;
            }
            else
            {
                throw new InvalidOperationException("Network connection incompatible with controller.");
            }
        }
        public void RemoveNetworkConnection(TECNetworkConnection connection)
        {
            if (this.ChildrenConnections.Contains(connection))
            {
                List<INetworkConnectable> children = new List<INetworkConnectable>(connection.Children);
                foreach(INetworkConnectable child in children)
                {
                    connection.RemoveINetworkConnectable(child);
                }
                removeChildConnection(connection);
            }
            else
            {
                throw new InvalidOperationException("Network connection doesn't exist in controller.");
            }
        }

        public bool CanConnectToNetwork(TECNetworkConnection netConnect)
        {
            return (AvailableNetworkIO.Contains(netConnect.IOType));
        }

        public bool CanConnectSubScope(TECSubScope subScope)
        {
            IOCollection ssIO = new IOCollection();
            foreach (TECPoint point in subScope.Points)
            {
                for (int i = 0; i < point.Quantity; i++)
                {
                    ssIO.AddIO(point.Type);
                }
            }
            return getAvailableIO().Contains(ssIO.ListIO());
        }
        public TECSubScopeConnection AddSubScope(TECSubScope subScope)
        {
            if (CanConnectSubScope(subScope))
            {
                bool connectionIsTypical = (this.IsTypical || subScope.IsTypical);
                if (!subScope.IsNetwork)
                {
                    TECSubScopeConnection connection = new TECSubScopeConnection(connectionIsTypical);
                    connection.ParentController = this;
                    connection.SubScope = subScope;
                    addChildConnection(connection);
                    subScope.Connection = connection;
                    return connection;
                }
                else
                {
                    throw new InvalidOperationException("Can't connect network subscope without a known connection.");
                }
            }
            else
            {
                throw new InvalidOperationException("Subscope incompatible.");
            }
        }
        public void RemoveSubScope(TECSubScope subScope)
        {
            TECSubScopeConnection connectionToRemove = null;
            foreach (TECConnection connection in ChildrenConnections)
            {
                if (connection is TECSubScopeConnection)
                {
                    var subConnect = connection as TECSubScopeConnection;
                    if (subConnect.SubScope == subScope)
                    {
                        connectionToRemove = subConnect;
                    }
                }
            }
            if (connectionToRemove != null)
            {
                removeChildConnection(connectionToRemove);
                subScope.Connection = null;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Passed subscope does not exist in any connection in controller.");
            }
        }

        public void RemoveAllConnections()
        {
            ObservableCollection<TECConnection> connectionsToRemove = new ObservableCollection<TECConnection>();
            foreach(TECConnection connection in ChildrenConnections)
            {
                connectionsToRemove.Add(connection);
            }
            foreach(TECConnection connectToRemove in connectionsToRemove)
            {
                if (connectToRemove is TECNetworkConnection netConnect)
                {
                    RemoveNetworkConnection(netConnect);
                }
                else if (connectToRemove is TECSubScopeConnection)
                {
                    (connectToRemove as TECSubScopeConnection).SubScope.Connection = null;
                    (connectToRemove as TECSubScopeConnection).SubScope = null;
                    connectToRemove.ParentController = null;
                }
                else
                {
                    throw new NotImplementedException();
                }
                ChildrenConnections.Remove(connectToRemove);
            }
            if(ParentConnection != null)
            {
                ParentConnection.RemoveINetworkConnectable(this);
            }
        }

        public TECController GetParentController()
        {
            if (ParentConnection == null)
            {
                return null;
            }
            else
            {
                return ParentConnection.ParentController;
            }
        }

        private ObservableCollection<TECNetworkConnection> getNetworkConnections()
        {
            ObservableCollection<TECNetworkConnection> networkConnections = new ObservableCollection<TECNetworkConnection>();
            foreach (TECConnection connection in ChildrenConnections)
            {
                if (connection is TECNetworkConnection)
                {
                    networkConnections.Add(connection as TECNetworkConnection);
                }
            }
            return networkConnections;
        }
        #endregion

        #region Methods
        #region Event Handlers
        private void collectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e, string propertyName)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    notifyCombinedChanged(Change.Add, propertyName, this, item);
                    if (item is INotifyCostChanged cost && !(item is TECConnection) && !this.IsTypical)
                    {
                        notifyCostChanged(cost.CostBatch);
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    notifyCombinedChanged(Change.Remove, propertyName, this, item);
                    if (item is INotifyCostChanged cost && !(item is TECConnection) && !this.IsTypical)
                    {
                        notifyCostChanged(cost.CostBatch * -1);
                    }
                }
            }
        }
        #endregion
        public Object DragDropCopy(TECScopeManager scopeManager)
        {
            var outController = new TECController(this, this.IsTypical);
            ModelLinkingHelper.LinkScopeItem(outController, scopeManager);
            return outController;
        }
        
        override protected CostBatch getCosts()
        {
            if (!IsTypical)
            {
                CostBatch costs = base.getCosts();
                costs += Type.CostBatch;
                foreach (TECConnection connection in
                    ChildrenConnections.Where(connection => !connection.IsTypical))
                {
                    costs += connection.CostBatch;
                }
                foreach(TECIOModule module in IOModules)
                {
                    costs += module.CostBatch;
                }
                return costs;
            } else
            {
                return new CostBatch();
            }
        }
        protected override SaveableMap saveObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.saveObjects());
            saveList.AddRange(this.ChildrenConnections, "ChildrenConnections");
            saveList.Add(this.Type, "Type");
            return saveList;
        }
        protected override SaveableMap relatedObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.relatedObjects());
            saveList.Add(this.Type, "Type");
            return saveList;
        }
        protected override void notifyCostChanged(CostBatch costs)
        {
            if (!IsTypical)
            {
                base.notifyCostChanged(costs);
            }
        }

        private void addChildConnection(TECConnection connection)
        {
            ChildrenConnections.Add(connection);
            if (!connection.IsTypical && !this.IsTypical)
            {
                notifyCostChanged(connection.CostBatch);
            }
        }
        private void removeChildConnection(TECConnection connection)
        {
            ChildrenConnections.Remove(connection);
            if (!connection.IsTypical && !this.IsTypical)
            {
                notifyCostChanged(connection.CostBatch * -1);
            }
        }

        private IOCollection getTotalIO()
        {
            IOCollection totalIO = new IOCollection(this.Type.IO);
            foreach(TECIOModule module in this.IOModules)
            {
                totalIO.AddIO(module.IO);
            }
            return totalIO;
        }
        private IOCollection getUsedIO()
        {
            IOCollection usedIO = new IOCollection();
            foreach(TECConnection connection in ChildrenConnections)
            {
                usedIO += connection.IO;
            }
            return usedIO;
        }
        private IOCollection getUsedNetworkIO()
        {
            IOCollection usedIO = getUsedIO();
            foreach(TECIO io in usedIO.ListIO())
            {
                if (!TECIO.NetworkIO.Contains(io.Type))
                {
                    usedIO.RemoveIO(io);
                }
            }
            return usedIO;
        }
        private IOCollection getAvailableIO()
        {
            return (getTotalIO() - getUsedIO());
        }
        private IOCollection getAvailableNetworkIO()
        {
            IOCollection availableIO = getAvailableIO();
            foreach(TECIO io in availableIO.ListIO())
            {
                if (!TECIO.NetworkIO.Contains(io.Type))
                {
                    availableIO.RemoveIO(io);
                }
            }
            return availableIO;
        }

        public INetworkConnectable Copy(INetworkConnectable item, bool isTypical, Dictionary<Guid, Guid> guidDictionary)
        {
            return new TECController(item as TECController, isTypical, guidDictionary);
        }

        
        #endregion
    }
}
