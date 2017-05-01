using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECNetworkConnection : TECConnection
    {
        #region Properties
        //---Stored---
        private ObservableCollection<TECController> _childrenControllers;
        private TECConnectionType _connectionType;
        private IOType _ioType;

        public ObservableCollection<TECController> ChildrenControllers
        {
            get { return _childrenControllers; }
            set
            {
                var temp = this.Copy();
                ChildrenControllers.CollectionChanged -= ChildrenControllers_CollectionChanged;
                _childrenControllers = value;
                ChildrenControllers.CollectionChanged += ChildrenControllers_CollectionChanged;
                NotifyPropertyChanged("ChildrenControllers", temp, this);
                RaisePropertyChanged("PossibleIO");
            }
        }
        public TECConnectionType ConnectionType
        {
            get { return _connectionType; }
            set
            {
                var temp = this.Copy();
                _connectionType = value;
                NotifyPropertyChanged("ConnectionType", temp, this);
                NotifyPropertyChanged("ChildChanged", (object)this, (object)value);
            }
        }
        public IOType IOType
        {
            get { return _ioType; }
            set
            {
                var temp = this.Copy();
                _ioType = value;
                NotifyPropertyChanged("IOType", temp, this);
                NotifyPropertyChanged("ChildChanged", (object)this, (object)value);
            }
        }

        //---Derived---
        public ObservableCollection<IOType> PossibleIO
        {
            get
            {
                ObservableCollection<IOType> IO = new ObservableCollection<IOType>();
                if (ParentController != null)
                {
                    //Start off with all IO in the parent controller
                    foreach(IOType io in ParentController.NetworkIO)
                    {
                        IO.Add(io);
                    }

                    //If any IO aren't in children controllers, remove them.
                    foreach(TECController child in ChildrenControllers)
                    {
                        List<IOType> ioToRemove = new List<IOType>();
                        foreach(IOType io in IO)
                        {
                            if (!child.NetworkIO.Contains(io))
                            {
                                ioToRemove.Add(io);
                            }
                        }
                        foreach(IOType io in ioToRemove)
                        {
                            IO.Remove(io);
                        }
                    }
                }
                return IO;
            }
        }
        #endregion

        #region Constructors
        public TECNetworkConnection(Guid guid) : base(guid)
        {
            _childrenControllers = new ObservableCollection<TECController>();
            ChildrenControllers.CollectionChanged += ChildrenControllers_CollectionChanged;
        }
        public TECNetworkConnection() : this(Guid.NewGuid()) { }
        public TECNetworkConnection(TECNetworkConnection connectionSource, Dictionary<Guid, Guid> guidDictionary = null) : base(connectionSource, guidDictionary)
        {
            _childrenControllers = new ObservableCollection<TECController>();
            foreach(TECController controller in connectionSource.ChildrenControllers)
            {
                _childrenControllers.Add(new TECController(controller, guidDictionary));
            }

            if (connectionSource.ConnectionType != null)
            {
                _connectionType = connectionSource.ConnectionType;
            }

            _ioType = connectionSource.IOType;
        }
        #endregion

        #region Methods
        public override object Copy()
        {
            TECNetworkConnection connection = new TECNetworkConnection(this);
            connection._guid = this._guid;
            
            return connection;
        }
        protected override double getElectricalCost()
        {
            throw new NotImplementedException();
        }
        #endregion Methods

        #region Event Handlers
        private void ChildrenControllers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    NotifyPropertyChanged("AddRelationship", this, item);
                    RaisePropertyChanged("PossibleIO");
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyPropertyChanged("RemoveRelationship", this, item);
                    RaisePropertyChanged("PossibleIO");
                }
            }
        }

        
        #endregion
    }
}
