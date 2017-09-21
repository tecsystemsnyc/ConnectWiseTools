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
        private ObservableCollection<INetworkConnectable> _children;
        private TECElectricalMaterial _connectionType;
        private IOType _ioType;

        public ObservableCollection<INetworkConnectable> Children
        {
            get { return _children; }
            set
            {
                var old = Children;
                Children.CollectionChanged -= ChildrenControllers_CollectionChanged;
                _children = value;
                Children.CollectionChanged += ChildrenControllers_CollectionChanged;
                notifyCombinedChanged(Change.Edit, "Children", this, value, old);
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
            }
        }

        public override IOCollection IO
        {
            get
            {
                IOCollection io = new IOCollection();
                io.AddIO(IOType);
                return io;
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
                    foreach (TECIO io in ParentController.AvailableNetworkIO.ListIO())
                    {
                        IO.Add(io.Type);
                    }

                    //If any IO aren't in children controllers, remove them.
                    foreach (INetworkConnectable child in Children)
                    {
                        List<IOType> ioToRemove = new List<IOType>();
                        foreach (IOType io in IO)
                        {
                            if (!child.AvailableNetworkIO.Contains(io))
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
            _children = new ObservableCollection<INetworkConnectable>();
            Children.CollectionChanged += ChildrenControllers_CollectionChanged;
        }
        public TECNetworkConnection(bool isTypical) : this(Guid.NewGuid(), isTypical) { }
        public TECNetworkConnection(TECNetworkConnection connectionSource, bool isTypical, Dictionary<Guid, Guid> guidDictionary = null) : base(connectionSource, isTypical, guidDictionary)
        {
            _children = new ObservableCollection<INetworkConnectable>();
            foreach (INetworkConnectable item in connectionSource.Children)
            {
                _children.Add(item.Copy(item, isTypical, guidDictionary));
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
                costs += ConduitType.GetCosts(ConduitLength);
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
