using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstimatingLibrary.Interfaces;

namespace EstimatingLibrary
{
    public class TECEquipment : TECScope, CostComponent, PointComponent
    {
        #region Properties
        private ObservableCollection<TECSubScope> _subScope;
        public ObservableCollection<TECSubScope> SubScope
        {
            get { return _subScope; }
            set
            {
                var temp = this.Copy();
                if (SubScope != null)
                {
                    SubScope.CollectionChanged -= SubScope_CollectionChanged;
                }
                _subScope = value;
                SubScope.CollectionChanged += SubScope_CollectionChanged;
                NotifyPropertyChanged("SubScope", temp, this);
                RaisePropertyChanged("SubScopeQuantity");
                subscribeToSubScope();
            }
        }

        public double BudgetUnitPrice
        {
            get { return _budgetUnitPrice; }
            set
            {
                var temp = this.Copy();
                if (value < 0)
                {
                    _budgetUnitPrice = -1;
                }
                else
                {
                    _budgetUnitPrice = value;
                }
                NotifyPropertyChanged("BudgetUnitPrice", temp, this);
                RaisePropertyChanged("TotalBudgetPrice");
            }
        }
        private double _budgetUnitPrice;

        new public int Quantity
        {
            get { return _quantity; }
            set
            {
                var temp = this.Copy();
                _quantity = value;
                RaisePropertyChanged("TotalBudgetPrice");
                NotifyPropertyChanged("Quantity", temp, this);
            }
        }
        public int SubScopeQuantity
        {
            get
            {
                int quantitySS = 0;
                foreach (TECSubScope ss in SubScope)
                {
                    quantitySS += ss.Quantity;
                }
                return quantitySS;
            }
        }

        public double TotalBudgetPrice
        {
            get
            {
                if (Quantity > 0)
                {
                    return (Quantity * BudgetUnitPrice);
                }
                else
                {
                    return -1;
                }
            }
        }

        public int PointNumber
        {
            get
            {
                return getPointNumber();
            }
        }

        public List<TECCost> Costs
        {
            get
            {
                return getCosts();
            }
        }

        private List<TECCost> getCosts()
        {
            var outCosts = new List<TECCost>();
            foreach(TECSubScope ss in SubScope)
            {
                foreach(TECCost cost in ss.Costs)
                {
                    outCosts.Add(cost);
                }
            }
            foreach(TECCost cost in AssociatedCosts)
            {
                outCosts.Add(cost);
            }
            return outCosts;
        }

        #endregion //Properties

        #region Constructors
        public TECEquipment(Guid guid) : base(guid)
        {
            _budgetUnitPrice = -1;
            _subScope = new ObservableCollection<TECSubScope>();
            SubScope.CollectionChanged += SubScope_CollectionChanged;
            base.PropertyChanged += TECEquipment_PropertyChanged;
        }

        public TECEquipment() : this(Guid.NewGuid()) { }

        //Copy Constructor
        public TECEquipment(TECEquipment equipmentSource, Dictionary<Guid, Guid> guidDictionary = null,
            ObservableItemToInstanceList<TECScope> characteristicReference = null) : this()
        {
            if (guidDictionary != null)
            { guidDictionary[_guid] = equipmentSource.Guid; }
            foreach (TECSubScope subScope in equipmentSource.SubScope)
            {
                var toAdd = new TECSubScope(subScope, guidDictionary, characteristicReference);
                characteristicReference?.AddItem(subScope,toAdd);
                SubScope.Add(toAdd);
            }
            _budgetUnitPrice = equipmentSource.BudgetUnitPrice;
            this.copyPropertiesFromScope(equipmentSource);
            
        }
        #endregion //Constructors

        #region Methods
        public override Object Copy()
        {
            TECEquipment outEquip = new TECEquipment();
            outEquip._guid = this.Guid;
            foreach (TECSubScope subScope in this.SubScope)
            { outEquip.SubScope.Add(subScope.Copy() as TECSubScope); }
            outEquip._budgetUnitPrice = this.BudgetUnitPrice;

            outEquip.copyPropertiesFromScope(this);
            return outEquip;
        }

        public override object DragDropCopy()
        {
            TECEquipment outEquip = new TECEquipment(this);
            return outEquip;
        }

        private void SubScope_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged("SubScopeQuantity");
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    NotifyPropertyChanged("Add", this, item);
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
                    NotifyPropertyChanged("Remove", this, item);
                    NotifyPropertyChanged("RemovedSubScope", this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                NotifyPropertyChanged("Edit", this, sender, typeof(TECEquipment), typeof(TECSubScope));
            }
            subscribeToSubScope();
        }
        private void SubScopeChanged(string name)
        {
            if (name == "Quantity")
            {
                RaisePropertyChanged("SubScopeQuantity");
            }
            else if (name == "PointNumber")
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
            if (e.PropertyName == "ObjectPropertyChanged")
            {
                var args = e as PropertyChangedExtendedEventArgs<object>;
                var oldNew = args.NewValue as Tuple<object, object>;
                foreach (TECSubScope subScope in this.SubScope)
                {
                    if (subScope.Location == oldNew.Item1)
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
        #endregion

    }
}
