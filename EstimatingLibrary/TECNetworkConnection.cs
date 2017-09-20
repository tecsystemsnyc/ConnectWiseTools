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
                notifyCombinedChanged(Change.Edit, "ChildrenControllers", this, value, old);
                raisePropertyChanged("PossibleIO");
            }
        }
        public TECElectricalMaterial ConnectionType
        {
            get { return _connectionType; }
            set
            {
                var old = ConnectionType;
                var originalCost = this.CostBatch;
                _connectionType = value;
                notifyCombinedChanged(Change.Edit, "ConnectionType", this, value, old);
                notifyCostChanged(CostBatch - originalCost);

            }
        }
        public IOType IOType
        {
            get { return _ioType; }
            set
            {
                var old = IOType;
                _ioType = value;
                notifyCombinedChanged(Change.Edit, "IOType", this, value, old);
                //notifyCombinedChanged("ChildChanged", (object)this, (object)value);
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
        public TECNetworkConnection(Guid guid, bool isTypical) : base(guid, isTypical)
        {
            _childrenControllers = new ObservableCollection<TECController>();
            ChildrenControllers.CollectionChanged += ChildrenControllers_CollectionChanged;
        }
        public TECNetworkConnection(bool isTypical) : this(Guid.NewGuid(), isTypical) { }
        public TECNetworkConnection(TECNetworkConnection connectionSource, bool isTypical, Dictionary<Guid, Guid> guidDictionary = null) : base(connectionSource, isTypical, guidDictionary)
        {
            _childrenControllers = new ObservableCollection<TECController>();
            foreach (TECController controller in connectionSource.ChildrenControllers)
            {
                _childrenControllers.Add(new TECController(controller, isTypical, guidDictionary));
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
                    notifyCombinedChanged(Change.Add, "ChildrenControllers", this, item);
                    raisePropertyChanged("PossibleIO");
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    notifyCombinedChanged(Change.Remove, "ChildrenControllers", this, item);
                    raisePropertyChanged("PossibleIO");
                }
            }
        }

        #endregion

        #region Methods
        protected override CostBatch getCosts()
        {
            CostBatch costs = new CostBatch();
            if (ConnectionType != null)
            {
                costs += ConnectionType.GetCosts(Length);
            }
            if (ConduitType != null)
            {
                costs += ConduitType.GetCosts(Length);
            }
            return costs;
        }
        protected override SaveableMap saveObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.saveObjects());
            saveList.Add(this.ConnectionType, "ConnectionType");
            saveList.AddRange(this.ChildrenControllers, "ChildrenControllers");
            return saveList;
        }
        protected override SaveableMap relatedObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.relatedObjects());
            saveList.Add(this.ConnectionType, "ConnectionTypes");
            saveList.AddRange(this.ChildrenControllers, "ChildrenControllers");
            return saveList;
        }
        #endregion 

    }
}
