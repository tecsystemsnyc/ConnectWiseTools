using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{

    public abstract class TECConnection : TECObject, INotifyCostChanged, ISaveable
    {
        #region Properties
        protected double _length;
        protected double _conduitLength;
        protected TECController _parentController;
        protected TECElectricalMaterial _conduitType;

        public double Length
        {
            get { return _length; }
            set
            {
                var old = Length;
                var originalCost = this.CostBatch;
                _length = value;
                NotifyCombinedChanged(Change.Edit, "Length", this, value, old);
                NotifyCostChanged(CostBatch - originalCost);
            }
        }
        public double ConduitLength
        {
            get { return _conduitLength; }
            set
            {
                var old = ConduitLength;
                _conduitLength = value;
                NotifyCombinedChanged(Change.Edit, "ConduitLength", this, value, old);
                CostBatch previous = ConduitType != null ? ConduitType.GetCosts(old) : new CostBatch();
                CostBatch current = ConduitType != null ? ConduitType.GetCosts(value) : new CostBatch();
                NotifyCostChanged(current - previous);
            }
        }
        public TECController ParentController
        {
            get { return _parentController; }
            set
            {
                _parentController = value;
                RaisePropertyChanged("ParentController");
            }
        }
        public TECElectricalMaterial ConduitType
        {
            get { return _conduitType; }
            set
            {
                var old = ConduitType;
                _conduitType = value;
                NotifyCombinedChanged(Change.Edit, "ConduitType", this, value, old);
                CostBatch previous = old != null ? old.GetCosts(ConduitLength) : new CostBatch();
                CostBatch current = value != null ? value.GetCosts(ConduitLength) : new CostBatch();
                NotifyCostChanged(current - previous);
            }
        }

        public CostBatch CostBatch
        {
            get { return getCosts(); }
        }

        public SaveableMap SaveObjects
        {
            get { return saveObjects(); }
        }
        public SaveableMap RelatedObjects
        {
            get { return relatedObjects(); }
        }
        #endregion //Properties

        public event Action<CostBatch> CostChanged;

        #region Constructors 
        public TECConnection(Guid guid) : base(guid)
        {
            _length = 0;
            _conduitLength = 0;
        }
        public TECConnection() : this(Guid.NewGuid()) { }
        public TECConnection(TECConnection connectionSource, Dictionary<Guid, Guid> guidDictionary = null) : this()
        {
            if (guidDictionary != null)
            { guidDictionary[_guid] = connectionSource.Guid; }

            _length = connectionSource.Length;
            _conduitLength = connectionSource.ConduitLength;
            if (connectionSource.ConduitType != null)
            { _conduitType = connectionSource.ConduitType; }
        }
        #endregion //Constructors

        public void NotifyCostChanged(CostBatch costs)
        {
            CostChanged?.Invoke(costs);
        }

        protected abstract CostBatch getCosts();
        protected virtual SaveableMap saveObjects()
        {
            SaveableMap saveList = new SaveableMap();
            if(this.ConduitType != null)
            {
                saveList.Add(this.ConduitType, "ConduitType");
            }
            return saveList;
        }
        protected virtual SaveableMap relatedObjects()
        {
            SaveableMap relatedList = new SaveableMap();
            if (this.ConduitType != null)
            {
                relatedList.Add(this.ConduitType, "ConduitType");
            }
            return relatedList;
        }
    }
}
