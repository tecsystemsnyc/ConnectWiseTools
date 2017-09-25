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
        private ObservableCollection<TECElectricalMaterial> _connectionTypes;
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
        public ObservableCollection<TECElectricalMaterial> ConnectionTypes
        {
            get { return _connectionTypes; }
            set
            {
                if (ConnectionTypes != null)
                {
                    ConnectionTypes.CollectionChanged -= (sender, args) =>
                    ConnectionTypes_CollectionChanged(sender, args, "ConnectionTypes");
                }
                var old = ConnectionTypes;
                _connectionTypes = value; if (ConnectionTypes != null)
                {
                    ConnectionTypes.CollectionChanged += (sender, args) =>
                    ConnectionTypes_CollectionChanged(sender, args, "ConnectionTypes");
                }
                notifyCombinedChanged(Change.Edit, "ConnectionTypes", this, value, old);
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
            _connectionTypes = new ObservableCollection<TECElectricalMaterial>();
            Children.CollectionChanged += ChildrenControllers_CollectionChanged;
            ConnectionTypes.CollectionChanged += (sender, args) =>
                    ConnectionTypes_CollectionChanged(sender, args, "ConnectionTypes");
        }
        public TECNetworkConnection(bool isTypical) : this(Guid.NewGuid(), isTypical) { }
        public TECNetworkConnection(TECNetworkConnection connectionSource, bool isTypical, Dictionary<Guid, Guid> guidDictionary = null) : base(connectionSource, isTypical, guidDictionary)
        {
            _children = new ObservableCollection<INetworkConnectable>();
            foreach (INetworkConnectable item in connectionSource.Children)
            {
                _children.Add(item.Copy(item, isTypical, guidDictionary));
            }
            foreach(TECElectricalMaterial type in connectionSource.ConnectionTypes)
            {
                _connectionTypes.Add(type);
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
        private void ConnectionTypes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e, string propertyName)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (TECElectricalMaterial type in e.NewItems)
                {
                    CostBatch connectionTypeCost = type.GetCosts(this.Length);
                    notifyCombinedChanged(Change.Add, propertyName, this, type);
                    notifyCostChanged(connectionTypeCost);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (TECElectricalMaterial type in e.OldItems)
                {
                    CostBatch connectionTypeCost = type.GetCosts(this.Length);
                    notifyCombinedChanged(Change.Remove, propertyName, this, type);
                    notifyCostChanged(-connectionTypeCost);
                }
            }
        }
        #endregion

        #region Methods
        public bool CanAddINetworkConnectable(INetworkConnectable connectable)
        {
            return (connectable.CanConnectToNetwork(this));
        }
        public void AddINetworkConnectable(INetworkConnectable connectable)
        {
            if (CanAddINetworkConnectable(connectable))
            {
                Children.Add(connectable);
                connectable.ParentConnection = this;
            }
            else
            {
                throw new InvalidOperationException("Connectable not compatible with Network Connection.");
            }
        }
        public void RemoveINetworkConnectable(INetworkConnectable connectable)
        {
            if (Children.Contains(connectable))
            {
                Children.Remove(connectable);
                connectable.ParentConnection = null;
            }
            else
            {
                throw new InvalidOperationException("INetworkConnectable doesn't exist in Network Connection.");
            }
        }

        protected override SaveableMap saveObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.saveObjects());
            List<TECObject> objects = new List<TECObject>();
            foreach(INetworkConnectable netconnect in Children)
            {
                objects.Add(netconnect as TECObject);
            }
            saveList.AddRange(objects, "Children");
            return saveList;
        }
        protected override SaveableMap relatedObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.relatedObjects());
            List<TECObject> objects = new List<TECObject>();
            foreach (INetworkConnectable netconnect in Children)
            {
                objects.Add(netconnect as TECObject);
            }
            saveList.AddRange(objects, "Children");
            return saveList;
        }
        public override ObservableCollection<TECElectricalMaterial> GetConnectionTypes()
        {
            return ConnectionTypes;
        }
        #endregion 

    }
}
