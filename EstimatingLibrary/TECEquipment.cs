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
    public class TECEquipment : TECLocated, INotifyCostChanged, INotifyPointChanged, DragDropComponent
    {
        #region Properties
        private ObservableCollection<TECSubScope> _subScope;

        public event Action<List<TECPoint>> PointChanged;

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

                RaisePropertyChanged("SubScopeQuantity");
                subscribeToSubScope();
            }
        }
        
        public int PointNumber
        {
            get
            {
                return getPointNumber();
            }
        }
        #endregion //Properties

        public event Action<List<TECCost>> CostChanged;

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
            RaisePropertyChanged("SubScopeQuantity");
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    NotifyPropertyChanged(Change.Add, "SubScope", this, item);
                    checkForTotalsInSubScope(item as TECSubScope);
                    if ((item as TECSubScope).Location == null)
                    {
                        (item as TECSubScope).SetLocationFromParent(this.Location);
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyPropertyChanged(Change.Remove, "SubScope", this, item);
                    //NotifyPropertyChanged("RemovedSubScope", this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                NotifyPropertyChanged(Change.Edit, "SubScope", this, sender);
            }
            subscribeToSubScope();
        }
        private void SubScopeChanged(string name)
        {
            if (name == "PointNumber")
            {
                RaisePropertyChanged("PointNumber");
            }
            else if (name == "TotalDevices")
            {
                RaisePropertyChanged("TotalDevices");
            }
            else if (name == "Length")
            {
                RaisePropertyChanged("SubLength");
            }
        }
        private void subscribeToSubScope()
        {
            foreach (TECSubScope scope in this.SubScope)
            {
                scope.PropertyChanged += (scopeSender, args) => this.SubScopeChanged(args.PropertyName);
            }
        }

        private void checkForTotalsInSubScope(TECSubScope subScope)
        {
            if (subScope.Points.Count > 0)
            {
                RaisePropertyChanged("PointNumber");
            }
            if (subScope.Devices.Count > 0)
            {
                RaisePropertyChanged("TotalDevices");
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

        public void NotifyCostChanged(List<TECCost> costs)
        {
            CostChanged?.Invoke(costs);
        }

        public void NotifyPointChanged(List<TECPoint> points)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
