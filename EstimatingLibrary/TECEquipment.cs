using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECEquipment : TECScope
    {
        #region Properties
        private ObservableCollection<TECSubScope> _subScope;
        public ObservableCollection<TECSubScope> SubScope
        {
            get { return _subScope; }
            set
            {
                var temp = this.Copy();
                _subScope = value;
                NotifyPropertyChanged("SubScope", temp, this);
                RaisePropertyChanged("SubScopeQuantity");
                subscribeToSubScope();
            }
        }

        public double BudgetPrice
        {
            get { return _budgetPrice; }
            set
            {
                var temp = this.Copy();
                if (value < 0)
                {
                    _budgetPrice = -1;
                }
                else
                {
                    _budgetPrice = value;
                }
                NotifyPropertyChanged("BudgetPrice", temp, this);
                RaisePropertyChanged("TotalBudgetPrice");
            }
        }
        private double _budgetPrice;

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
            get { return (Quantity * BudgetPrice); }
        }

        public double MaterialCost
        {
            get { return getMaterialCost(); }
        }
        public double LaborCost
        {
            get { return getLaborCost(); }
        }

        #endregion //Properties

        #region Constructors
        public TECEquipment(Guid guid) : base(guid)
        {
            _budgetPrice = -1;
            _subScope = new ObservableCollection<TECSubScope>(); ;

            SubScope.CollectionChanged += SubScope_CollectionChanged;
        }
        public TECEquipment() : this(Guid.NewGuid()) { }

        //Copy Constructor
        public TECEquipment(TECEquipment equipmentSource) : this()
        {
            foreach(TECSubScope subScope in equipmentSource.SubScope)
            {
                SubScope.Add(new TECSubScope(subScope));
            }
            _budgetPrice = equipmentSource.BudgetPrice;
            _name = equipmentSource.Name;
            _description = equipmentSource.Description;

            _location = equipmentSource.Location;
            _quantity = equipmentSource.Quantity;
            _associatedCosts = equipmentSource.AssociatedCosts;
            _tags = new ObservableCollection<TECTag>(equipmentSource.Tags);
        }
        #endregion //Constructors

        #region Methods
        public override Object Copy()
        {
            TECEquipment outEquip = new TECEquipment(this);
            outEquip._guid = this.Guid;
            return outEquip;
        }

        public override object DragDropCopy()
        {
            TECEquipment outEquip = new TECEquipment(this);
            return outEquip;
        }

        private double getMaterialCost()
        {
            double cost = 0;
            foreach(TECSubScope sub in this.SubScope)
            {
                cost += sub.MaterialCost;
            }
            return cost;
        }
        private double getLaborCost()
        {
            double cost = 0;
            foreach (TECSubScope sub in this.SubScope)
            {
                cost += sub.LaborCost;
            }
            return cost;
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
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyPropertyChanged("Remove", this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                NotifyPropertyChanged("Edit", this, sender);
            }
            subscribeToSubScope();
        }

        private void SubScopeChanged(string name)
        {
            if (name == "Quantity")
            {
                RaisePropertyChanged("SubScopeQuantity");
            }
            else if(name == "TotalPoints")
            {
                RaisePropertyChanged("TotalPoints");
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
                RaisePropertyChanged("TotalPoints");
            }
            if (subScope.Devices.Count > 0)
            {
                RaisePropertyChanged("TotalDevices");
            }
        }

        #endregion

    }
}
