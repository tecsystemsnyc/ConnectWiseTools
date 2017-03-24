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
                _parentConnection = value;
                RaisePropertyChanged("ParentConnection");
            }
        }
        public ObservableCollection<TECConnection> ChildrenConnections
        {
            get { return _childrenConnections; }
            set
            {
                _childrenConnections = value;
                RaisePropertyChanged("ChildrenConnections");
            }
        }
        public ObservableCollection<TECIO> IO
        {
            get { return _io; }
            set
            {
                var temp = this.Copy();
                IO.CollectionChanged -= CollectionChanged;
                _io = value;
                NotifyPropertyChanged("IO", temp, this);
                IO.CollectionChanged += CollectionChanged;
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
            IO.CollectionChanged += CollectionChanged;
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
        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach(Object item in e.NewItems)
                {
                    if(item is TECIO)
                    {
                        (item as TECIO).PropertyChanged += ObjectPropertyChanged;
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
                        (item as TECIO).PropertyChanged -= ObjectPropertyChanged;
                        NotifyPropertyChanged("Remove", this, item);
                    }
                }
            }
        }
        private void ObjectPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;
            if (e.PropertyName == "Quantity")
            {
                NotifyPropertyChanged("ChildChanged", (object)this, (object)args.NewValue);
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
                foreach(TECSubScope ss in connected.SubScope)
                {
                    foreach(TECDevice device in ss.Devices)
                    {
                        availableIO.Remove(device.IOType);
                    }
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
