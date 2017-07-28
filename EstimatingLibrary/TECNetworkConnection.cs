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

        override public List<TECCost> Costs
        {
            get
            {
                return getCosts();
            }
        }
        private List<TECCost> getCosts()
        {
            var outCosts = new List<TECCost>();
            TECCost thisCost = new TECCost();

            if (ConnectionType != null)
            {
                thisCost = new TECCost();
                thisCost.Type = ConnectionType.Type;
                thisCost.Cost = ConnectionType.Cost * Length;
                thisCost.Labor = ConnectionType.Labor * Length;
                outCosts.Add(thisCost);
                foreach (TECCost cost in ConnectionType.AssociatedCosts)
                {
                    outCosts.Add(cost);
                }
                foreach (TECCost cost in ConnectionType.RatedCosts)
                {
                    TECCost ratedCost = new TECCost();
                    ratedCost.Type = cost.Type;
                    ratedCost.Cost = cost.Cost * Length;
                    ratedCost.Labor = cost.Labor * Length;
                    outCosts.Add(ratedCost);
                }
            }
            if (ConduitType != null)
            {
                thisCost = new TECCost();
                thisCost.Type = ConduitType.Type;
                thisCost.Cost = ConduitType.Cost * ConduitLength;
                thisCost.Labor = ConduitType.Labor * ConduitLength;
                outCosts.Add(thisCost);
                foreach (TECCost cost in ConduitType.AssociatedCosts)
                {
                    outCosts.Add(cost);
                }
                foreach (TECCost cost in ConduitType.RatedCosts)
                {
                    TECCost ratedCost = new TECCost();
                    ratedCost.Cost = cost.Cost * Length;
                    ratedCost.Labor = cost.Labor * Length;
                    outCosts.Add(ratedCost);
                }
            }
            return outCosts;
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

        #region Methods
        public override object Copy()
        {
            TECNetworkConnection connection = new TECNetworkConnection(this);
            connection._guid = this._guid;

            return connection;
        }
        #endregion Methods

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
