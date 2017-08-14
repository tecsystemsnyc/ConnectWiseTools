using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;

namespace EstimatingLibrary
{
    public class TECEquipment : TECLocated, INotifyPointChanged, DragDropComponent
    {
        #region Properties
        private ObservableCollection<TECSubScope> _subScope;

        public event Action<int> PointChanged;

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
                NotifyPropertyChanged(Change.Edit, "SubScope", this, value, old);
            }
        }
        
        public int PointNumber
        {
            get
            {
                return getPointNumber();
            }
        }
        override public List<TECCost> Costs
        {
            get { return costs(); }
        }
        public List<TECPoint> Points
        {
            get { return points(); }
        }
        #endregion //Properties

        #region Constructors
        public TECEquipment(Guid guid) : base(guid)
        {
            _subScope = new ObservableCollection<TECSubScope>();
            SubScope.CollectionChanged += SubScope_CollectionChanged;
            base.PropertyChanged += TECEquipment_PropertyChanged;
        }

        public TECEquipment() : this(Guid.NewGuid()) { }

        //Copy Constructor
        public TECEquipment(TECEquipment equipmentSource, Dictionary<Guid, Guid> guidDictionary = null,
            ListDictionary<TECObject> characteristicReference = null) : this()
        {
            if (guidDictionary != null)
            { guidDictionary[_guid] = equipmentSource.Guid; }
            foreach (TECSubScope subScope in equipmentSource.SubScope)
            {
                var toAdd = new TECSubScope(subScope, guidDictionary, characteristicReference);
                characteristicReference?.AddItem(subScope,toAdd);
                SubScope.Add(toAdd);
            }
            copyPropertiesFromScope(equipmentSource);
            
        }
        #endregion //Constructors

        #region Methods

        public object DragDropCopy(TECScopeManager scopeManager)
        {
            TECEquipment outEquip = new TECEquipment(this);
            ModelLinkingHelper.LinkScopeItem(outEquip, scopeManager);
            return outEquip;
        }

        private void SubScope_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                int pointNumber = 0;
                List<TECCost> costs = new List<TECCost>();
                foreach (TECSubScope item in e.NewItems)
                {
                    pointNumber += item.PointNumber;
                    costs.AddRange(item.Costs);
                    NotifyPropertyChanged(Change.Add, "SubScope", this, item);
                    if ((item as TECSubScope).Location == null)
                    {
                        (item as TECSubScope).SetLocationFromParent(this.Location);
                    }
                }
                PointChanged?.Invoke(pointNumber);
                NotifyCostChanged(costs);
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                int pointNumber = 0;
                List<TECCost> costs = new List<TECCost>();
                foreach (TECSubScope item in e.OldItems)
                {
                    pointNumber += item.PointNumber;
                    costs.AddRange(item.Costs);
                    NotifyPropertyChanged(Change.Remove, "SubScope", this, item);
                }
                PointChanged?.Invoke(pointNumber * -1);
                NotifyCostChanged(CostHelper.NegativeCosts(costs));
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                NotifyPropertyChanged(Change.Edit, "SubScope", this, sender);
            }
        }
        
        private void TECEquipment_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Location")
            {
                var args = e as TECChangedEventArgs;
                var location = args.Value as TECLabeled;
                if(location.Flavor == Flavor.Location)
                {
                    foreach (TECSubScope subScope in this.SubScope)
                    {
                        if (subScope.Location == location)
                        {
                            subScope.SetLocationFromParent(this.Location);
                        }
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
        private List<TECCost> costs()
        {
            List<TECCost> outCosts = new List<TECCost>();

            foreach(TECSubScope subScope in SubScope)
            {
                outCosts.AddRange(subScope.Costs);
            }
            outCosts.AddRange(this.AssociatedCosts);
            return outCosts;
        }
        private List<TECPoint> points()
        {
            List<TECPoint> outPoints = new List<TECPoint>();
            foreach(TECSubScope subScope in this.SubScope)
            {
                outPoints.AddRange(subScope.Points);
            }
            return outPoints;
        }

        public void NotifyPointChanged(List<TECPoint> points)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
