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
                        foreach(TECConnectionType type in dev.ConnectionTypes)
                        {
                            outConnectionTypes.Add(type);
                        }
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

            foreach (TECConnectionType connectionType in ConnectionTypes)
            {
                thisCost = new TECCost();
                thisCost.Type = connectionType.Type;
                thisCost.Cost = connectionType.Cost * Length;
                thisCost.Labor = connectionType.Labor * Length;
                outCosts.Add(thisCost);
                foreach (TECCost cost in connectionType.AssociatedCosts)
                {
                    outCosts.Add(cost);
                }
                foreach (TECCost cost in connectionType.RatedCosts)
                {
                    TECCost ratedCost = new TECCost();
                    ratedCost.Type = cost.Type;
                    ratedCost.Cost = cost.Cost * Length;
                    ratedCost.Labor = cost.Labor * Length;
                    outCosts.Add(ratedCost);
                }
            }
            if(ConduitType != null)
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
                    ratedCost.Cost = cost.Cost * ConduitLength;
                    ratedCost.Labor = cost.Labor * ConduitLength;
                    outCosts.Add(ratedCost);
                }
            }
            return outCosts;
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
        
        #endregion

    }
}
