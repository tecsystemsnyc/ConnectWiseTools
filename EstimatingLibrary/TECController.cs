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
    public enum NetworkType
    {
        Unitary = 1, DDC, Server
    };

    public class TECController : TECLocated, INotifyCostChanged, DragDropComponent
    {
        #region Properties
        //---Stored---
        private TECNetworkConnection _parentConnection;
        private ObservableCollection<TECConnection> _childrenConnections;
        private TECControllerType _type;
        private NetworkType _networkType;
        
        public TECNetworkConnection ParentConnection
        {
            get { return _parentConnection; }
            set
            {
                var old = ParentConnection;
                _parentConnection = value;
                NotifyPropertyChanged(Change.Edit, "ParentConnection", this, value, old);
                RaisePropertyChanged("NetworkIO");
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
                NotifyPropertyChanged(Change.Edit, "ChildrenConnections", this, value, old);
                RaisePropertyChanged("ChildNetworkConnections");
            }
        }
        public TECControllerType Type
        {
            get { return _type; }
            set
            {
                var old = Type;
                _type = value;
                NotifyPropertyChanged(Change.Edit, "Type", this, value, old);
                //NotifyPropertyChanged("ChildChanged", (object)this, (object)value);
            }
        }
        public NetworkType NetworkType
        {
            get { return _networkType; }
            set
            {
                var old = NetworkType;
                _networkType = value;
                NotifyPropertyChanged(Change.Edit, "NetworkType", this, value, old);
            }
        }
        
        //---Derived---
        public ObservableCollection<IOType> AvailableIO
        {
            get { return getAvailableIO(); }
        }
        public ObservableCollection<IOType> NetworkIO
        {
            get
            { return getNetworkIO(); }
        }
        public bool IsGlobal;
        
        new public List<TECCost> Costs
        {
            get { return costs(); }
        }


        #endregion

        #region Constructors
        public TECController(Guid guid, TECControllerType type, bool isGlobal = true) : base(guid)
        {
            IsGlobal = isGlobal;
            _type = type;
            _childrenConnections = new ObservableCollection<TECConnection>();
            ChildrenConnections.CollectionChanged += (sender, args) => collectionChanged(sender, args, "ChildrenConnections");
        }

        public TECController(TECControllerType type, bool isGlobal = true) : this(Guid.NewGuid(), type, isGlobal) { }
        public TECController(TECController controllerSource, Dictionary<Guid, Guid> guidDictionary = null) : this(controllerSource.Type)
        {
            if (guidDictionary != null)
            { guidDictionary[_guid] = controllerSource.Guid; }
            copyPropertiesFromLocated(controllerSource);
            foreach (TECConnection connection in controllerSource.ChildrenConnections)
            {
                if (connection is TECSubScopeConnection)
                {
                    TECSubScopeConnection connectionToAdd = new TECSubScopeConnection(connection as TECSubScopeConnection, guidDictionary);
                    connectionToAdd.ParentController = this;
                    _childrenConnections.Add(connectionToAdd);
                }
                else if (connection is TECNetworkConnection)
                {

                    TECNetworkConnection connectionToAdd = new TECNetworkConnection(connection as TECNetworkConnection, guidDictionary);
                    connectionToAdd.ParentController = this;
                    _childrenConnections.Add(connectionToAdd);
                }
            }
        }

        #endregion

        #region Event Handlers
        private void collectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e, string propertyName)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    NotifyPropertyChanged(Change.Add, propertyName, this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyPropertyChanged(Change.Remove, propertyName, this, item);
                }
            }
            if (sender == ChildrenConnections)
            {
                RaisePropertyChanged("ChildNetworkConnections");
            }
        }
        #endregion

        #region Connection Methods
        public TECNetworkConnection AddController(TECController controller, TECNetworkConnection connection)
        {
            if (controller != this)
            {
                foreach (TECConnection conn in ChildrenConnections)
                {
                    if (conn is TECNetworkConnection)
                    {
                        TECNetworkConnection netConn = conn as TECNetworkConnection;
                        if (connection == netConn)
                        {
                            bool ioMatches = false;
                            foreach(IOType ioType in controller.NetworkIO)
                            {
                                if (connection.IOType == ioType)
                                {
                                    ioMatches = true;
                                    break;
                                }
                            }
                            if (ioMatches)
                            {
                                netConn.ChildrenControllers.Add(controller);
                                controller.ParentConnection = netConn;
                                return netConn;
                            }
                            else
                            {
                                throw new ArgumentException("Controller and connection do not have a matching IOType.");
                            }
                        }
                    }
                }
                throw new ArgumentOutOfRangeException("Passed connection does not exist in controller.");
            }
            else
            {
                return null;
            }
        }
        public TECNetworkConnection AddController(TECController controller, TECElectricalMaterial connectionType)
        {
            if (controller != this)
            {
                IOType ioType = 0;
                foreach (IOType thisType in this.NetworkIO)
                {
                    foreach (IOType otherType in controller.NetworkIO)
                    {
                        if (thisType == otherType)
                        {
                            ioType = thisType;
                            break;
                        }
                    }
                    if (ioType != 0) { break; }
                }
                if (ioType == 0)
                {
                    throw new ArgumentException("Controller and parent do not have a matching IOType.");
                }

                TECNetworkConnection netConnect = new TECNetworkConnection();
                netConnect.ParentController = this;
                netConnect.ChildrenControllers.Add(controller);
                netConnect.IOType = ioType;
                netConnect.ConnectionType = connectionType;
                ChildrenConnections.Add(netConnect);
                controller.ParentConnection = netConnect;
                return netConnect;
            }
            else
            {
                return null;
            }
        }
        public TECSubScopeConnection AddSubScope(TECSubScope subScope)
        {
            TECSubScopeConnection connection = new TECSubScopeConnection();
            connection.ParentController = this;
            connection.SubScope = subScope;
            ChildrenConnections.Add(connection);
            subScope.Connection = connection;
            return connection;
        }
        public void RemoveController(TECController controller)
        {
            bool exists = false;
            TECNetworkConnection connectionToRemove = null;
            foreach (TECConnection connection in ChildrenConnections)
            {
                if (connection is TECNetworkConnection)
                {
                    var netConnect = connection as TECNetworkConnection;
                    if (netConnect.ChildrenControllers.Contains(controller))
                    {
                        exists = true;
                        controller.ParentConnection = null;
                        netConnect.ChildrenControllers.Remove(controller);
                    }
                    if (netConnect.ChildrenControllers.Count < 1)
                    {
                        connectionToRemove = netConnect;
                    }
                }

            }
            if (connectionToRemove != null)
            {
                ChildrenConnections.Remove(connectionToRemove);
            }
            if (!exists)
            {
                throw new ArgumentOutOfRangeException("Passed controller does not exist in any connection in controller.");
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
                ChildrenConnections.Remove(connectionToRemove);
                subScope.Connection = null;
                connectionToRemove.SubScope = null;
                connectionToRemove.ParentController = null;
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
                if (connectToRemove is TECNetworkConnection)
                {
                    ObservableCollection<TECController> controllersToRemove = new ObservableCollection<TECController>();
                    foreach(TECController controller in (connectToRemove as TECNetworkConnection).ChildrenControllers)
                    {
                        controller.ParentConnection = null;
                        controllersToRemove.Add(controller);
                    }
                    foreach(TECController controller in controllersToRemove)
                    {
                        (connectToRemove as TECNetworkConnection).ChildrenControllers.Remove(controller);
                    }
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
            SetParentController(null, null);
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
        public void SetParentController(TECController controller, TECElectricalMaterial connectionType)
        {
            if (ParentConnection != null)
            {
                GetParentController().RemoveController(this);
            }

            if (controller != null)
            {
                controller.AddController(this, connectionType);
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
        public Object DragDropCopy(TECScopeManager scopeManager)
        {
            var outController = new TECController(this);
            ModelLinkingHelper.LinkScopeItem(outController, scopeManager);
            return outController;
        }
        private ObservableCollection<IOType> getAvailableIO()
        {
            var availableIO = new ObservableCollection<IOType>();
            foreach (TECIO type in this.Type.IO)
            {
                for (var x = 0; x < type.Quantity; x++)
                {
                    availableIO.Add(type.Type);
                }
            }

            foreach (TECSubScopeConnection connected in ChildrenConnections)
            {
                foreach (TECDevice device in connected.SubScope.Devices)
                {
                    availableIO.Remove(device.IOType);
                }
            }
            return availableIO;
        }
        private ObservableCollection<IOType> getNetworkIO()
        {
            var outIO = new ObservableCollection<IOType>();
            foreach (TECIO io in this.Type.IO)
            {
                var type = io.Type;
                if (type != IOType.AI && type != IOType.AO && type != IOType.DI && type != IOType.DO)
                {
                    for (var x = 0; x < io.Quantity; x++)
                    {
                        outIO.Add(type);
                    }
                }
            }

            return outIO;
        }


        private List<TECCost> costs()
        {
            var outCosts = new List<TECCost>();
            outCosts.Add(this.Type);
            outCosts.AddRange(this.AssociatedCosts);
            foreach (TECConnection connection in ChildrenConnections)
            {
                foreach (TECCost cost in connection.Costs)
                {
                    outCosts.Add(cost);
                }
            }
            return outCosts;
        }
        #endregion
    }
}
