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

                //foreach (TECDevice dev in SubScope.Devices)
                //{
                //    outIOTypes.Add(dev.IOType);
                //}

                return outIOTypes;
            }
        }
        #endregion

        #region Constructors
        public TECSubScopeConnection(Guid guid, bool isTypical) : base(guid, isTypical) { }
        public TECSubScopeConnection(bool isTypical) : this(Guid.NewGuid(), isTypical) { }
        public TECSubScopeConnection(TECSubScopeConnection connectionSource, Dictionary<Guid, Guid> guidDictionary = null) : base(connectionSource, guidDictionary)
        {
            if (connectionSource._subScope != null)
            { _subScope = new TECSubScope(connectionSource.SubScope, guidDictionary); }
        }
        #endregion Constructors

        #region Methods
        protected override CostBatch getCosts()
        {
            CostBatch costs = new CostBatch();
            foreach (TECElectricalMaterial connectionType in ConnectionTypes)
            {
                costs += connectionType.GetCosts(Length);
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
            saveList.Add(this.SubScope, "SubScope");
            return saveList;
        }
        protected override SaveableMap relatedObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.relatedObjects());
            saveList.Add(this.SubScope, "SubScope");
            return saveList;
        }
        #endregion
    }
}
