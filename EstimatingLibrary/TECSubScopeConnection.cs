using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECSubScopeConnection : TECConnection
    {
        #region Properties
        //---Stored---
        private TECSubScope _subScope;
        public TECSubScope SubScope
        {
            get { return _subScope; }
            set
            {
                var oldNew = Tuple.Create<Object, Object>(_subScope, value);
                var temp = Copy();
                _subScope = value;
                RaisePropertyChanged("SubScope");
                NotifyPropertyChanged("RelationshipPropertyChanged", temp, oldNew);
            }
        }

        private bool _includeStubUp;
        public bool IncludeStubUp
        {
            get { return _includeStubUp; }
            set
            {
                var temp = Copy();
                _includeStubUp = value;
                NotifyPropertyChanged("IncludeStubUp", temp, this);
            }
        }

        //---Derived---
        public ObservableCollection<TECConnectionType> ConnectionTypes
        {
            get
            {
                var outConnectionTypes = new ObservableCollection<TECConnectionType>();
                if (SubScope != null)
                {
                    foreach (TECDevice dev in SubScope.Devices)
                    {
                        outConnectionTypes.Add(dev.ConnectionType);
                    }
                }

                return outConnectionTypes;
            }
        }
        public ObservableCollection<IOType> IOTypes
        {
            get
            {
                var outIOTypes = new ObservableCollection<IOType>();

                foreach (TECDevice dev in SubScope.Devices)
                {
                    outIOTypes.Add(dev.IOType);
                }

                return outIOTypes;
            }
        }
        public int Terminations
        {
            get
            {
                return getTerminations();
            }
        }
        #endregion

        #region Constructors
        public TECSubScopeConnection(Guid guid) : base(guid)
        {
            _includeStubUp = false;
        }
        public TECSubScopeConnection() : base(Guid.NewGuid()) { }
        public TECSubScopeConnection(TECSubScopeConnection connectionSource, Dictionary<Guid, Guid> guidDictionary = null) : base(connectionSource, guidDictionary)
        {
            if (connectionSource._subScope != null)
            { _subScope = new TECSubScope(connectionSource.SubScope, guidDictionary); }
            _includeStubUp = connectionSource.IncludeStubUp;
        }
        #endregion Constructors

        #region Methods
        public override Object Copy()
        {
            TECSubScopeConnection connection = new TECSubScopeConnection(this);
            connection._guid = this._guid;
            if (_subScope != null)
            { connection._subScope = _subScope.Copy() as TECSubScope; }
            return connection;
        }
        private int getTerminations()
        {
            int terms = 0;
            foreach (TECConnectionType type in ConnectionTypes)
            {
                terms += 2;
            }
            return terms;
        }

        protected override double getElectricalCost()
        {
            double cost = 0;
            var terminations = 0;

            foreach (TECConnectionType type in ConnectionTypes)
            {
                cost += Length * type.Cost;
                terminations += 2;
                foreach (TECAssociatedCost associatedCost in type.AssociatedCosts)
                {
                    cost += associatedCost.Cost;
                }
            }
            if (ConduitType != null)
            {
                if (IncludeStubUp)
                {
                    cost += 15 * ConduitType.Cost;
                }
                else
                {
                    cost += ConduitLength * ConduitType.Cost;
                }
                foreach (TECAssociatedCost associatedCost in ConduitType.AssociatedCosts)
                {
                    cost += associatedCost.Cost;
                }
            }

            cost += terminations * .25;
            return cost;
        }
        protected override double getElectricalLabor()
        {
            double laborHours = 0;
            var terminations = 0;
            if (ConduitType != null)
            {
                laborHours += ConduitLength * ConduitType.Labor;
                foreach (TECAssociatedCost associatedCost in ConduitType.AssociatedCosts)
                {
                    laborHours += associatedCost.Labor;
                }
            }
            foreach (TECConnectionType type in ConnectionTypes)
            {
                terminations += 2;
                laborHours += Length * type.Labor;
                foreach (TECAssociatedCost associatedCost in type.AssociatedCosts)
                { laborHours += associatedCost.Labor; }
            }
            laborHours += terminations * .1;
            return laborHours;
        }
        #endregion

    }
}
