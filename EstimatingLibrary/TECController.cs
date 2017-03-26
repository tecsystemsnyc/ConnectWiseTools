using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECController : TECScope
    {
        #region Properties
        //---Stored---
        private double _cost;
        private TECNetworkConnection _parentConnection;
        private ObservableCollection<TECConnection> _childrenConnections;
        private ObservableCollection<TECIO> _io;
        private TECManufacturer _manufacturer;
        private bool _isServer;
        private bool _isBMS;

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
                if (value == null)
                {
                    _parentConnection.ChildrenControllers.Remove(this);
                    if (_parentConnection.ChildrenControllers.Count < 1)
                    {
                        _parentConnection.ParentController.ChildrenConnections.Remove(_parentConnection);
                    }
                }
                _parentConnection = value;
                RaisePropertyChanged("ParentConnection");
            }
        }
        public ObservableCollection<TECConnection> ChildrenConnections
        {
            get { return _childrenConnections; }
            set
            {
                var temp = this.Copy();
                _childrenConnections = value;
                NotifyPropertyChanged("ChildrenConnections", temp, this);
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
        public bool IsServer
        {
            get { return _isServer; }
            set
            {
                var temp = this.Copy();
                _isServer = value;
                NotifyPropertyChanged("IsServer", temp, this);
                if (IsServer)
                {
                    IsBMS = true;
                }
            }
        }
        public bool IsBMS
        {
            get { return _isBMS; }
            set
            {
                var temp = this.Copy();
                _isBMS = value;
                NotifyPropertyChanged("IsBMS", temp, this);
                if (!IsBMS)
                {
                    IsServer = false;
                }
            }
        }

        //---Derived---
        public List<IOType> AvailableIO
        {
            get { return getAvailableIO(); }
        }
        public List<IOType> NetworkIO
        {
            get { return getNetworkIO(); }
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
                _io.Add(new TECIO(io));
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
                        (item as TECIO).PropertyChanged += IOPropertyChanged;
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
                        (item as TECIO).PropertyChanged -= IOPropertyChanged;
                        NotifyPropertyChanged("Remove", this, item);
                    }
                }
            }
        }
        private void IOPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;
            if (e.PropertyName == "Quantity")
            {
                NotifyPropertyChanged("ChildChanged", (object)this, (object)args.NewValue);
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
        }
        #endregion

        #region Connection Methods

        public TECNetworkConnection AddController(TECController controller, TECConnection connection = null)
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
                ChildrenConnections.Add(netConnect);
                netConnect.ParentController = this;
                netConnect.ChildrenControllers.Add(controller);
                controller.ParentConnection = netConnect;
                return netConnect;
            }
        }
        public TECSubScopeConnection AddSubScope(TECSubScope subScope)
        {
            TECSubScopeConnection connection = new TECSubScopeConnection();
            ChildrenConnections.Add(connection);
            connection.ParentController = this;
            connection.SubScope = subScope;
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
            outController._guid = this.Guid;
            outController.copyPropertiesFromScope(this);
            outController._cost = Cost;
            outController._manufacturer = Manufacturer;

            foreach (TECIO io in this.IO)
            {
                outController.IO.Add(io);
            }
            //foreach(TECConnection connection in ChildrenConnections)
            //{
            //    outController.ChildrenConnections.Add(connection.Copy() as TECConnection);
            //}
            
            return outController;
        }
        public override Object DragDropCopy()
        {
            var outController = new TECController(this);
            
            return outController;
        }
        private List<IOType> getAvailableIO()
        {
            var availableIO = new List<IOType>();
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
        private List<IOType> getNetworkIO()
        {
            var outIO = new List<IOType>();
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
        #endregion
    }
}
