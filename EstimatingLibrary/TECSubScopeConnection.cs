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
                if(SubScope != null)
                { SubScope.PropertyChanged -= SubScope_PropertyChanged; }
                _subScope = value;
                if (SubScope != null)
                { SubScope.PropertyChanged += SubScope_PropertyChanged; }
                RaisePropertyChanged("SubScope");
                NotifyPropertyChanged("RelationshipPropertyChanged", temp, oldNew);
            }
        }

        private void SubScope_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var args = e as PropertyChangedExtendedEventArgs<object>;
            if(e.PropertyName == "CostComponentChanged" && args != null)
            {
                var old = this.Copy() as TECSubScopeConnection;
                old.SubScope = args.OldValue as TECSubScope;
                NotifyPropertyChanged("CostComponentChanged", old, this);
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
        #endregion

        #region Constructors
        public TECSubScopeConnection(Guid guid) : base(guid)
        { 
        }
        public TECSubScopeConnection() : base(Guid.NewGuid()) { }
        public TECSubScopeConnection(TECSubScopeConnection connectionSource, Dictionary<Guid, Guid> guidDictionary = null) : base(connectionSource, guidDictionary)
        {
            if (connectionSource._subScope != null)
            { _subScope = new TECSubScope(connectionSource.SubScope, guidDictionary); }
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

        protected override double getElectricalCost()
        {
            double cost = 0;

            foreach (TECConnectionType type in ConnectionTypes)
            {
                cost += Length * type.Cost;
                foreach (TECAssociatedCost associatedCost in type.AssociatedCosts)
                {
                    cost += associatedCost.Cost;
                }
            }
            if (ConduitType != null)
            {
                cost += ConduitLength * ConduitType.Cost;
                foreach (TECAssociatedCost associatedCost in ConduitType.AssociatedCosts)
                {
                    cost += associatedCost.Cost;
                }
            }
            
            return cost;
        }
        protected override double getElectricalLabor()
        {
            double laborHours = 0;
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
                laborHours += Length * type.Labor;
                foreach (TECAssociatedCost associatedCost in type.AssociatedCosts)
                { laborHours += associatedCost.Labor; }
            }
            return laborHours;
        }
        #endregion

    }
}
