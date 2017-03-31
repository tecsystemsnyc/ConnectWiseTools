using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public enum ControllerType
    {
        IsServer = 1, IsBMS, IsNetworked, IsStandalone
    };

    public class TECController : TECScope
    {
        #region Properties
        //---Stored---
        private double _cost;
        private TECNetworkConnection _parentConnection;
        private ObservableCollection<TECConnection> _childrenConnections;
        private ObservableCollection<TECIO> _io;
        private TECManufacturer _manufacturer;
        private ControllerType _type;

        public double Cost
        {
            get { return _cost; }
            set
            {
                var temp = this.Copy();
                _cost = value;
                NotifyPropertyChanged("Cost", temp, this);
            }
        }
        public TECNetworkConnection ParentConnection
        {
            get { return _parentConnection; }
            set
            {
                var temp = Copy();
                _parentConnection = value;
                NotifyPropertyChanged("ParentConnection", temp, this);
                RaisePropertyChanged("ParentController");
                RaisePropertyChanged("NetworkIO");
            }
        }
        public ObservableCollection<TECConnection> ChildrenConnections
        {
            get { return _childrenConnections; }
            set
            {
                var temp = this.Copy();
                ChildrenConnections.CollectionChanged -= collectionChanged;
                _childrenConnections = value;
                ChildrenConnections.CollectionChanged += collectionChanged;
                NotifyPropertyChanged("ChildrenConnections", temp, this);
                RaisePropertyChanged("ChildNetworkConnections");
            }
        }
        public ObservableCollection<TECIO> IO
        {
            get { return _io; }
            set
            {
                var temp = this.Copy();
                IO.CollectionChanged -= IO_CollectionChanged;
                _io = value;
                NotifyPropertyChanged("IO", temp, this);
                IO.CollectionChanged += IO_CollectionChanged;
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
        public ControllerType Type
        {
            get { return _type; }
            set
            {
                var temp = Copy();
                _type = value;
                NotifyPropertyChanged("Type", temp, this);
                RaisePropertyChanged("IsServer");
                RaisePropertyChanged("IsBMS");
                RaisePropertyChanged("IsNetworked");
                RaisePropertyChanged("IsStandalone");
            }
        }

        public double MaterialCost
        {
            get { return getMaterialCost(); }
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
            private set { }
        }

        public TECController ParentController
        {
            get
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
            set
            {
                if (ParentConnection != null)
                {
                    ParentController.RemoveController(this);
                }

                if (value != null)
                {
                    value.AddController(this);
                }

                RaisePropertyChanged("ParentController");
            }
        }

        public ObservableCollection<TECNetworkConnection> ChildNetworkConnections
        {
            get
            {
                ObservableCollection<TECNetworkConnection> networkConnections = new ObservableCollection<TECNetworkConnection>();
                foreach (TECConnection connection in ChildrenConnections)
                {
                    if(connection is TECNetworkConnection)
                    {
                        networkConnections.Add(connection as TECNetworkConnection);
                    }
                }
                return networkConnections;
            }
        }

        public bool IsServer
        {
            get
            {
                return (Type == ControllerType.IsServer);
            }
        }
        public bool IsBMS
        {
            get
            {
                return ((Type == ControllerType.IsServer) || (Type == ControllerType.IsBMS));
            }
        }
        public bool IsNetworked
        {
            get
            {
                return ((Type == ControllerType.IsServer) || (Type == ControllerType.IsBMS) || (Type == ControllerType.IsNetworked));
            }
        }
        public bool IsStandalone
        {
            get
            {
                return (Type == ControllerType.IsStandalone);
            }
        }

        #endregion
        
        #region Constructors
        public TECController(Guid guid) : base(guid)
        {
            _cost = 0;
            _io = new ObservableCollection<TECIO>();
            _childrenConnections = new ObservableCollection<TECConnection>();
            ChildrenConnections.CollectionChanged += collectionChanged;
            IO.CollectionChanged += IO_CollectionChanged;
        }
        
        public TECController() : this(Guid.NewGuid()) { }
        public TECController(TECController controllerSource, Dictionary<Guid, Guid> guidDictionary = null) : this()
        {
            if (guidDictionary != null)
            { guidDictionary[_guid] = controllerSource.Guid; }
            copyPropertiesFromScope(controllerSource);
            foreach (TECIO io in controllerSource.IO)
            {
                TECIO ioToAdd = new TECIO(io);
                _io.Add(new TECIO(io));
            }
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
            _manufacturer = controllerSource.Manufacturer;
            _cost = controllerSource.Cost;
        }
        
        #endregion 

        #region Event Handlers
        private void IO_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach(Object item in e.NewItems)
                {
                    if(item is TECIO)
                    {
                        NotifyPropertyChanged("Add", this, item);
                    } 
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (Object item in e.OldItems)
                {
                    if (item is TECIO)
                    {
                        NotifyPropertyChanged("Remove", this, item);
                    }
                }
            }
        }

        private void collectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    NotifyPropertyChanged("Add", this, item, typeof(TECController), typeof(TECConnection));
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyPropertyChanged("Remove", this, item, typeof(TECController), typeof(TECConnection));
                }
            }
            if (sender == ChildrenConnections)
            {
                RaisePropertyChanged("ChildNetworkConnections");
            }
        }
        #endregion

        #region Connection Methods

        public TECNetworkConnection AddController(TECController controller, TECConnection connection = null)
        {
            if (controller != this)
            {
                if (connection != null)
                {
                    foreach (TECNetworkConnection conn in ChildrenConnections)
                    {
                        if (connection == conn)
                        {
                            conn.ChildrenControllers.Add(controller);
                            controller.ParentConnection = conn;
                            return conn;
                        }
                    }
                    throw new ArgumentOutOfRangeException("Passed connection does not exist in controller.");
                }
                else
                {
                    TECNetworkConnection netConnect = new TECNetworkConnection();
                    netConnect.ParentController = this;
                    netConnect.ChildrenControllers.Add(controller);
                    ChildrenConnections.Add(netConnect);
                    controller.ParentConnection = netConnect;
                    return netConnect;
                }
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
            foreach (TECNetworkConnection connection in ChildrenConnections)
            {
                if (connection.ChildrenControllers.Contains(controller))
                {
                    exists = true;
                    controller.ParentConnection = null;
                    connection.ChildrenControllers.Remove(controller);
                }
            }
            if (!exists)
            {
                throw new ArgumentOutOfRangeException("Passed controller does not exist in any connection in controller.");
            }
        }
        public void RemoveSubScope(TECSubScope subScope)
        {
            TECSubScopeConnection connectionToRemove = null;
            foreach (TECSubScopeConnection connection in ChildrenConnections)
            {
                if (connection.SubScope == subScope)
                {
                    subScope.Connection = null;
                    connection.SubScope = null;
                    connection.ParentController = null;
                    connectionToRemove = connection;
                }
            }
            if (connectionToRemove != null)
            {
                ChildrenConnections.Remove(connectionToRemove);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Passed subscope does not exist in any connection in controller.");
            }
        }
        #endregion

        #region Methods
        public override Object Copy()
        {
            TECController outController = new TECController(this.Guid);
            outController.copyPropertiesFromScope(this);
            outController._cost = Cost;
            outController._manufacturer = Manufacturer;
            foreach (TECIO io in this.IO)
            {
                outController.IO.Add(io.Copy() as TECIO);
            }
            foreach (TECConnection connection in ChildrenConnections)
            {
                var outConnection = connection.Copy() as TECConnection;
                outConnection.ParentController = outController;
                outController.ChildrenConnections.Add(outConnection);
            }

            return outController;
        }
        public override Object DragDropCopy()
        {
            var outController = new TECController(this);
            return outController;
        }
        private ObservableCollection<IOType> getAvailableIO()
        {
            var availableIO = new ObservableCollection<IOType>();
            foreach (TECIO type in this.IO)
            {
                for(var x = 0; x < type.Quantity; x++)
                {
                    availableIO.Add(type.Type);
                }
            }

            foreach (TECSubScopeConnection connected in ChildrenConnections)
            {
                foreach(TECDevice device in connected.SubScope.Devices)
                {
                    availableIO.Remove(device.IOType);
                }
            }
            return availableIO;
        }
        private ObservableCollection<IOType> getNetworkIO()
        {
            var outIO = new ObservableCollection<IOType>();
            foreach (TECIO io in this.IO)
            {
                var type = io.Type;
                if(type != IOType.AI && type != IOType.AO && type != IOType.DI && type != IOType.DO)
                {
                    for (var x = 0; x < io.Quantity; x++)
                    {
                        outIO.Add(type);
                    }
                }
            }

            return outIO;
        }
        public int NumberOfIOType(IOType ioType)
        {
            int outNum = 0;

            foreach(TECIO type in IO)
            {
                if(type.Type == ioType)
                {
                    outNum = type.Quantity;
                }
            }

            return outNum;
        }
        public List<IOType> getUniqueIO()
        {
            var outList = new List<IOType>();

            foreach(TECIO io in this.IO)
            {
                if (!outList.Contains(io.Type))
                {
                    outList.Add(io.Type);
                }
            }
            return outList;
        }

        private double getMaterialCost()
        {
            double matCost = 0;
            matCost += this.Cost * this.Manufacturer.Multiplier;
            foreach (TECAssociatedCost cost in this.AssociatedCosts)
            {
                matCost += cost.Cost;
            }
            return matCost;
        }
        #endregion
    }
}
