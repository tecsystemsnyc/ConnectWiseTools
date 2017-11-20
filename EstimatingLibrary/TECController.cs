using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
        public List<TECIO> AllNetworkIOList
        {
            get { return getTotalNetworkIO().ListIO(); }
        }
        public IOCollection AvailableNetworkIO
        {
            get { return getAvailableNetworkIO(); }
        }
        public IEnumerable<TECNetworkConnection> ChildNetworkConnections
        {
            get
            {
                return getNetworkConnections();
            }
        }
        public IOCollection TotalIO
        {
            get { return getTotalIO(); }
        }
        public IOCollection AvailableIO
        {
            get { return getAvailableIO(); }
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
            foreach (TECIOModule module in controllerSource.IOModules)
            {
                this.IOModules.Add(module);
            }
        }
        #endregion

        #region Connection Methods
        public bool CanAddNetworkConnection(IOType ioType)
        {
            return (AvailableNetworkIO.Contains(ioType));
        }
        public TECNetworkConnection AddNetworkConnection(bool isTypical, 
            IEnumerable<TECElectricalMaterial> connectionTypes, IOType ioType)
        {
            if (CanAddNetworkConnection(ioType))
            {
                TECNetworkConnection netConnect = new TECNetworkConnection(isTypical);
                netConnect.ParentController = this;
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
            return getAvailableIO().Contains(subScope.IO) 
                || getPotentialIO().Contains(subScope.IO);
        }
        public TECSubScopeConnection AddSubScope(TECSubScope subScope)
        {
            if (CanConnectSubScope(subScope))
            {
                bool connectionIsTypical = (this.IsTypical || subScope.IsTypical);
                if (!subScope.IsNetwork)
                {
                    if (getAvailableIO().Contains(subScope.IO))
                    {
                        return addConnection(subScope, connectionIsTypical);
                    }
                    else if(getPotentialIO().Contains(subScope.IO))
                    {
                        foreach(TECIOModule module in Type.IOModules)
                        {
                            if (this.IOModules.Count(item => { return item == module; }) <
                                Type.IOModules.Count(item => { return item == module; }))
                            {
                                if(new IOCollection(module.IO).Contains(subScope.IO))
                                {
                                    this.IOModules.Add(module);
                                    return addConnection(subScope, connectionIsTypical);
                                }
                            }
                        }
                        foreach (TECIOModule module in Type.IOModules)
                        {
                            if (this.IOModules.Count(item => { return item == module; }) <
                                Type.IOModules.Count(item => { return item == module; }))
                            {
                                this.IOModules.Add(module);
                            }
                        }
                        return addConnection(subScope, connectionIsTypical);
                    }
                    else
                    {
                        throw new InvalidOperationException("Attempted to connect subscope which could not be connected.");

                    }
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

            TECSubScopeConnection addConnection(TECSubScope toConnect, bool isTypical)
            {
                TECSubScopeConnection connection = new TECSubScopeConnection(isTypical);
                connection.ParentController = this;
                connection.SubScope = subScope;
                addChildConnection(connection);
                subScope.Connection = connection;
                return connection;
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

        private List<TECNetworkConnection> getNetworkConnections()
        {
            List<TECNetworkConnection> networkConnections = new List<TECNetworkConnection>();
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

        #region Module Methods
        public bool CanAddModule(TECIOModule module)
        {
            return (this.Type.IOModules.Count(mod => (mod == module)) >
                this.IOModules.Count(mod => (mod == module)));
        }
        public void AddModule(TECIOModule module)
        {
            if (CanAddModule(module))
            {
                IOModules.Add(module);
            } 
            else
            {
                throw new InvalidOperationException("Controller can't accept IOModule.");
            }
        }
        #endregion

        #region Methods
        #region Event Handlers
        private void collectionChanged(object sender,
            System.Collections.Specialized.NotifyCollectionChangedEventArgs e, string propertyName)
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
            if (propertyName == "ChildrenConnections")
            {
                raisePropertyChanged("ChildNetworkConnections");
            }
        }
        #endregion
        public Object DragDropCopy(TECScopeManager scopeManager)
        {
            var outController = new TECController(this, this.IsTypical);
            ModelLinkingHelper.LinkScopeItem(outController, scopeManager);
            return outController;
        }
        
        protected override CostBatch getCosts()
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
        protected override SaveableMap propertyObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.propertyObjects());
            saveList.AddRange(this.ChildrenConnections, "ChildrenConnections");
            saveList.AddRange(this.IOModules, "IOModules");
            saveList.Add(this.Type, "Type");
            return saveList;
        }
        protected override SaveableMap linkedObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.linkedObjects());
            saveList.AddRange(this.IOModules, "IOModules");
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
        private IOCollection getTotalNetworkIO()
        {
            IOCollection networkIO = getTotalIO();
            foreach(TECIO io in networkIO.ListIO())
            {
                if (!TECIO.NetworkIO.Contains(io.Type))
                {
                    networkIO.RemoveIO(io);
                }
            }
            return networkIO;
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
        private IOCollection getPotentialIO()
        {
            IOCollection potentialIO = new IOCollection();
            foreach(TECIOModule module in Type.IOModules)
            {
                foreach(TECIO io in module.IO)
                {
                    potentialIO.AddIO(io);
                }
            }
            foreach(TECIOModule module in IOModules)
            {
                foreach(TECIO io in module.IO)
                {
                    potentialIO.RemoveIO(io);
                }
            }
            return potentialIO;
        }

        public INetworkConnectable Copy(INetworkConnectable item, bool isTypical, Dictionary<Guid, Guid> guidDictionary)
        {
            return new TECController(item as TECController, isTypical, guidDictionary);
        }
        
        #endregion
    }
}
