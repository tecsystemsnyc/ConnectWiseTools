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

    public abstract class TECConnection : TECObject, INotifyCostChanged, IRelatable, ITypicalable
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
                notifyCombinedChanged(Change.Edit, "Length", this, value, old);
                notifyCostChanged(CostBatch - originalCost);
            }
        }
        public double ConduitLength
        {
            get { return _conduitLength; }
            set
            {
                var old = ConduitLength;
                _conduitLength = value;
                notifyCombinedChanged(Change.Edit, "ConduitLength", this, value, old);
                CostBatch previous = ConduitType != null ? ConduitType.GetCosts(old) : new CostBatch();
                CostBatch current = ConduitType != null ? ConduitType.GetCosts(value) : new CostBatch();
                notifyCostChanged(current - previous);
            }
        }
        public TECController ParentController
        {
            get { return _parentController; }
            set
            {
                _parentController = value;
                raisePropertyChanged("ParentController");
            }
        }
        public TECElectricalMaterial ConduitType
        {
            get { return _conduitType; }
            set
            {
                var old = ConduitType;
                _conduitType = value;
                notifyCombinedChanged(Change.Edit, "ConduitType", this, value, old);
                CostBatch previous = old != null ? old.GetCosts(ConduitLength) : new CostBatch();
                CostBatch current = value != null ? value.GetCosts(ConduitLength) : new CostBatch();
                notifyCostChanged(current - previous);
            }
        }

        public CostBatch CostBatch
        {
            get { return getCosts(); }
        }

        public SaveableMap PropertyObjects
        {
            get { return propertyObjects(); }
        }
        public SaveableMap LinkedObjects
        {
            get { return linkedObjects(); }
        }

        public bool IsTypical { get; private set; }

        public abstract IOCollection IO { get; }

        public List<TECIO> IOList
        {
            get { return IO.ListIO(); }
        }
        #endregion //Properties

        public event Action<CostBatch> CostChanged;

        #region Constructors 
        public TECConnection(Guid guid, bool isTypical) : base(guid)
        {
            IsTypical = isTypical;
            _length = 0;
            _conduitLength = 0;
        }
        public TECConnection(bool isTypical) : this(Guid.NewGuid(), isTypical) { }
        public TECConnection(TECConnection connectionSource, bool isTypical, Dictionary<Guid, Guid> guidDictionary = null) : this(isTypical)
        {
            if (guidDictionary != null)
            { guidDictionary[_guid] = connectionSource.Guid; }

            _length = connectionSource.Length;
            _conduitLength = connectionSource.ConduitLength;
            if (connectionSource.ConduitType != null)
            { _conduitType = connectionSource.ConduitType; }
        }
        #endregion //Constructors

        public void notifyCostChanged(CostBatch costs)
        {
            if (!IsTypical)
            {
                CostChanged?.Invoke(costs);
            }
        }

        protected virtual CostBatch getCosts()
        {
            CostBatch costs = new CostBatch();
            foreach (TECElectricalMaterial connectionType in GetConnectionTypes())
            {
                costs += connectionType.GetCosts(Length);
            }
            if (ConduitType != null)
            {
                costs += ConduitType.GetCosts(ConduitLength);
            }
            return costs;
        }
        protected virtual SaveableMap propertyObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(this.GetConnectionTypes(), "ConnectionTypes");
            if(this.ConduitType != null)
            {
                saveList.Add(this.ConduitType, "ConduitType");
            }
            return saveList;
        }
        protected virtual SaveableMap linkedObjects()
        {
            SaveableMap relatedList = new SaveableMap();
            relatedList.AddRange(this.GetConnectionTypes(), "ConnectionTypes");
            if (this.ConduitType != null)
            {
                relatedList.Add(this.ConduitType, "ConduitType");
            }
            return relatedList;
        }

        abstract public ObservableCollection<TECElectricalMaterial> GetConnectionTypes();
    }
}
