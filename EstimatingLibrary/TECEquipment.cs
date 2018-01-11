using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EstimatingLibrary
{
    public class TECEquipment : TECLocated, INotifyPointChanged, IDragDropable, ITypicalable
    {
        #region Properties
        private ObservableCollection<TECSubScope> _subScope;

        public event Action<int> PointChanged;
        public event Action<object, System.Collections.Specialized.NotifyCollectionChangedEventArgs> SubScopeCollectionChanged;

        public ObservableCollection<TECSubScope> SubScope
        {
            get { return _subScope; }
            set
            {
                var old = SubScope;
                if (SubScope != null)
                {
                    SubScope.CollectionChanged -= SubScope_CollectionChanged;
                }
                _subScope = value;
                SubScope.CollectionChanged += SubScope_CollectionChanged;
                notifyCombinedChanged(Change.Edit, "SubScope", this, value, old);
            }
        }
        
        public int PointNumber
        {
            get
            {
                return getPointNumber();
            }
        }

        public bool IsTypical { get; private set; }
        #endregion //Properties

        #region Constructors
        public TECEquipment(Guid guid, bool isTypical) : base(guid)
        {
            IsTypical = isTypical;
            _subScope = new ObservableCollection<TECSubScope>();
            SubScope.CollectionChanged += SubScope_CollectionChanged;
            base.PropertyChanged += TECEquipment_PropertyChanged;
        }

        public TECEquipment(bool isTypical) : this(Guid.NewGuid(), isTypical) { }

        //Copy Constructor
        public TECEquipment(TECEquipment equipmentSource, bool isTypical, Dictionary<Guid, Guid> guidDictionary = null,
            ObservableListDictionary<TECObject> characteristicReference = null, TemplateSynchronizer<TECSubScope> ssSynchronizer = null) : this(isTypical)
        {
            if (guidDictionary != null)
            { guidDictionary[_guid] = equipmentSource.Guid; }
            foreach (TECSubScope subScope in equipmentSource.SubScope)
            {
                var toAdd = new TECSubScope(subScope, isTypical, guidDictionary, characteristicReference);
                if (ssSynchronizer != null && ssSynchronizer.Contains(subScope))
                {
                    ssSynchronizer.LinkNew(ssSynchronizer.GetTemplate(subScope), toAdd);
                }
                characteristicReference?.AddItem(subScope,toAdd);
                SubScope.Add(toAdd);
            }
            copyPropertiesFromScope(equipmentSource);
        }
        #endregion //Constructors

        #region Methods
        public object DragDropCopy(TECScopeManager scopeManager)
        {
            TECEquipment outEquip = new TECEquipment(this, this.IsTypical);
            ModelLinkingHelper.LinkScopeItem(outEquip, scopeManager);
            return outEquip;
        }

        private void SubScope_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SubScopeChanged?.Invoke(sender, e);
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                int pointNumber = 0;
                CostBatch costs = new CostBatch();
                foreach (TECSubScope item in e.NewItems)
                {
                    pointNumber += item.PointNumber;
                    costs += item.CostBatch;
                    if ((item as TECSubScope).Location == null)
                    {
                        (item as TECSubScope).SetLocationFromParent(this.Location);
                    }
                    notifyCombinedChanged(Change.Add, "SubScope", this, item);
                }
                notifyPointChanged(pointNumber);
                notifyCostChanged(costs);
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                int pointNumber = 0;
                CostBatch costs = new CostBatch();
                foreach (TECSubScope item in e.OldItems)
                {
                    pointNumber += item.PointNumber;
                    costs += item.CostBatch;
                    notifyCombinedChanged(Change.Remove, "SubScope", this, item);
                }
                notifyPointChanged(pointNumber * -1);
                notifyCostChanged(costs * -1);
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                notifyCombinedChanged(Change.Edit, "SubScope", this, sender);
            }
        }
        
        private void TECEquipment_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Location")
            {
                var args = e as TECChangedEventArgs;
                var location = args.Value as TECLabeled;
                foreach (TECSubScope subScope in this.SubScope)
                {
                    if (subScope.Location == location)
                    {
                        subScope.SetLocationFromParent(this.Location);
                    }
                }
            }
        }
        private int getPointNumber()
        {
            var totalPoints = 0;
            foreach (TECSubScope subScope in SubScope)
            {
                totalPoints += subScope.PointNumber;
            }
            return totalPoints;
        }
        protected override CostBatch getCosts()
        {
            CostBatch costs = base.getCosts();
            foreach(TECSubScope subScope in SubScope)
            {
                costs += subScope.CostBatch;
            }
            return costs;
        }
        protected override SaveableMap propertyObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.propertyObjects());
            saveList.AddRange(this.SubScope, "SubScope");
            return saveList;
        }
        protected override void notifyCostChanged(CostBatch costs)
        {
            if (!IsTypical)
            {
                base.notifyCostChanged(costs);
            }
        }

        private void notifyPointChanged(int numPoints)
        {
            if (!IsTypical)
            {
                PointChanged?.Invoke(numPoints);
            }
        }
        #endregion
    }
}
