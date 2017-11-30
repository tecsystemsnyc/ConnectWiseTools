using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EstimatingLibrary
{
    public class TECNetworkConnection : TECConnection
    {
        #region Properties
        //---Stored---
        private ObservableCollection<INetworkConnectable> _children;
        private ObservableCollection<TECConnectionType> _connectionTypes;
        private IOType _ioType;

        public ObservableCollection<INetworkConnectable> Children
        {
            get { return _children; }
            set
            {
                var old = Children;
                Children.CollectionChanged -= Children_CollectionChanged;
                _children = value;
                Children.CollectionChanged += Children_CollectionChanged;
                notifyCombinedChanged(Change.Edit, "Children", this, value, old);
                raisePropertyChanged("PossibleIO");
            }
        }
        public ObservableCollection<TECConnectionType> ConnectionTypes
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
            _connectionTypes = new ObservableCollection<TECConnectionType>();
            Children.CollectionChanged += Children_CollectionChanged;
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
            foreach(TECConnectionType type in connectionSource.ConnectionTypes)
            {
                _connectionTypes.Add(type);
            }

            _ioType = connectionSource.IOType;
        }
        #endregion

        #region Event Handlers
        private void Children_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    notifyCombinedChanged(Change.Add, "Children", this, item);
                    raisePropertyChanged("PossibleIO");
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    notifyCombinedChanged(Change.Remove, "Children", this, item);
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
                connectable.ParentConnection = this;
                Children.Add(connectable);
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

        protected override SaveableMap propertyObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.propertyObjects());
            List<TECObject> objects = new List<TECObject>();
            foreach(INetworkConnectable netconnect in Children)
            {
                objects.Add(netconnect as TECObject);
            }
            saveList.AddRange(objects, "Children");
            return saveList;
        }
        protected override SaveableMap linkedObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.linkedObjects());
            List<TECObject> objects = new List<TECObject>();
            foreach (INetworkConnectable netconnect in Children)
            {
                objects.Add(netconnect as TECObject);
            }
            saveList.AddRange(objects, "Children");
            return saveList;
        }
        public override ObservableCollection<TECConnectionType> GetConnectionTypes()
        {
            return ConnectionTypes;
        }
        #endregion 

    }
}
