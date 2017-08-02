using EstimatingLibrary.Utilities;
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
        private TECElectricalMaterial _connectionType;
        private IOType _ioType;

        public ObservableCollection<TECController> ChildrenControllers
        {
            get { return _childrenControllers; }
            set
            {
                var old = ChildrenControllers;
                ChildrenControllers.CollectionChanged -= ChildrenControllers_CollectionChanged;
                _childrenControllers = value;
                ChildrenControllers.CollectionChanged += ChildrenControllers_CollectionChanged;
                NotifyPropertyChanged(Change.Edit, "ChildrenControllers", this, value, old);
                RaisePropertyChanged("PossibleIO");
            }
        }
        public TECElectricalMaterial ConnectionType
        {
            get { return _connectionType; }
            set
            {
                var old = ConnectionType;
                _connectionType = value;
                NotifyPropertyChanged(Change.Edit, "ConnectionType", this, value, old);
                //NotifyPropertyChanged("ChildChanged", (object)this, (object)value);
            }
        }
        public IOType IOType
        {
            get { return _ioType; }
            set
            {
                var old = IOType;
                _ioType = value;
                NotifyPropertyChanged(Change.Edit, "IOType", this, value, old);
                //NotifyPropertyChanged("ChildChanged", (object)this, (object)value);
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
                    foreach (IOType io in ParentController.NetworkIO)
                    {
                        IO.Add(io);
                    }

                    //If any IO aren't in children controllers, remove them.
                    foreach (TECController child in ChildrenControllers)
                    {
                        List<IOType> ioToRemove = new List<IOType>();
                        foreach (IOType io in IO)
                        {
                            if (!child.NetworkIO.Contains(io))
                            {
                                ioToRemove.Add(io);
                            }
                        }
                        foreach (IOType io in ioToRemove)
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
            foreach (TECController controller in connectionSource.ChildrenControllers)
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

        #region Event Handlers
        private void ChildrenControllers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    NotifyPropertyChanged(Change.Add, "ChildrenControllers", this, item);
                    RaisePropertyChanged("PossibleIO");
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyPropertyChanged(Change.Remove, "ChildrenControllers", this, item);
                    RaisePropertyChanged("PossibleIO");
                }
            }
        }


        #endregion
    }
}
