using EstimatingLibrary.Utilities;
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
                var old = SubScope;
                _subScope = value;
                RaisePropertyChanged("SubScope");
                NotifyCombinedChanged(Change.Edit, "SubScope", this, value, old);
            }
        }

        //---Derived---
        public ObservableCollection<TECElectricalMaterial> ConnectionTypes
        {
            get
            {
                var outConnectionTypes = new ObservableCollection<TECElectricalMaterial>();
                if (SubScope != null)
                {
                    foreach (TECDevice dev in SubScope.Devices)
                    {
                        foreach(TECElectricalMaterial type in dev.ConnectionTypes)
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

        public override List<TECCost> Costs { get { return costs(); } }
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
        private void addCostsFromElectricalMaterial(TECElectricalMaterial material, List<TECCost> costs)
        {
            TECCost thisCost = new TECCost();
            thisCost.Type = material.Type;
            thisCost.Cost = material.Cost * Length;
            thisCost.Labor = material.Labor * Length;
            costs.Add(thisCost);
            foreach (TECCost cost in material.AssociatedCosts)
            {
                costs.Add(cost);
            }
            foreach (TECCost cost in material.RatedCosts)
            {
                TECCost ratedCost = new TECCost();
                ratedCost.Type = cost.Type;
                ratedCost.Cost = cost.Cost * Length;
                ratedCost.Labor = cost.Labor * Length;
                costs.Add(ratedCost);
            }
        }
        private List<TECCost> costs()
        {
            var outCosts = new List<TECCost>();
            TECCost thisCost = new TECCost();

            foreach (TECElectricalMaterial connectionType in ConnectionTypes)
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
                    ratedCost.Cost = cost.Cost * ConduitLength;
                    ratedCost.Labor = cost.Labor * ConduitLength;
                    outCosts.Add(ratedCost);
                }
            }
            return outCosts;
        }
        #endregion

    }
}
