using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECSystem : TECScope
    {//TECSystem is the largest encapsulating object in the System-Equipment hierarchy, offering a specific structure for the needs of the estimating tool. A seperate hierarchy exists for TECScopeBranch as a more generic object.
        #region Properties
        private ObservableCollection<TECEquipment> _equipment;
        public ObservableCollection<TECEquipment> Equipment
        {
            get { return _equipment; }
            set
            {
                    var temp = this.Copy();
                    _equipment = value;
                    NotifyPropertyChanged("Equipment", temp, this);
                    subscribeToEquipment();
            }
        }

        public double BudgetPrice
        {
            get { return _budgetPrice; }
            set
            {
                var temp = this.Copy();
                if(_budgetPrice != value)
                {
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
                    RaisePropertyChanged("PriceWithEquipment");
                }
            }
        }
        private double _budgetPrice;

        public double PriceWithEquipment
        {
            get
            {
                double price = 0;
                bool priceExists = false;
                if (BudgetPrice >= 0 )
                {
                    price += BudgetPrice;
                    priceExists = true;
                }
                foreach (TECEquipment equip in Equipment)
                {
                    if (equip.TotalBudgetPrice >= 0)
                    {
                        price += (equip.TotalBudgetPrice);
                        priceExists = true;
                    }
                }
                if (priceExists)
                {
                    return price;
                }
                else
                {
                    return -1;
                }
            }
        }

        new public int Quantity
        {
            get { return _quantity; }
            set
            {
                var temp = this.Copy();
                _quantity = value;
                NotifyPropertyChanged("Quantity", temp, this);
                RaisePropertyChanged("TotalBudgetPrice");             
            }
        }

        public int EquipmentQuantity
        {
            get
            {
                int equipQuantity = 0;
                foreach (TECEquipment equip in Equipment)
                {
                    equipQuantity += equip.Quantity;
                }
                return equipQuantity;
            }
        }

        public int SubScopeQuantity
        {
            get
            {
                int ssQuantity = 0;
                foreach (TECEquipment equip in Equipment)
                {
                    ssQuantity += (equip.SubScopeQuantity * equip.Quantity);
                }
                return ssQuantity;
            }
        }

        public double TotalBudgetPrice
        {
            get { return (Quantity * PriceWithEquipment); }
        }
        public double MaterialCost
        {
            get { return getMaterialCost(); }
        }
        public double LaborCost
        {
            get { return getLaborCost(); }
        }

        public ObservableCollection<TECSubScope> SubScope
        {
            get
            {
                var outSubScope = new ObservableCollection<TECSubScope>();
                foreach(TECEquipment equip in Equipment)
                {
                    foreach(TECSubScope sub in equip.SubScope)
                    {
                        outSubScope.Add(sub);
                    }
                }
                return outSubScope;
            }
        }
        #endregion //Properties

        #region Constructors
        public TECSystem(Guid guid) : base(guid)
        {
            _budgetPrice = -1;
            _equipment = new ObservableCollection<TECEquipment>();

            Equipment.CollectionChanged += Equipment_CollectionChanged;
        }
        
        public TECSystem() : this(Guid.NewGuid()) { }

        //Copy Constructor
        public TECSystem(TECSystem sourceSystem) : this()
        {
            foreach (TECEquipment equipment in sourceSystem.Equipment)
            {
                Equipment.Add(new TECEquipment(equipment));
            }

            _name = sourceSystem.Name;
            _description = sourceSystem.Name;
            _budgetPrice = sourceSystem.BudgetPrice;
            _location = sourceSystem.Location;
            _quantity = sourceSystem.Quantity;
            _tags = new ObservableCollection<TECTag>(sourceSystem.Tags);

        }
        #endregion //Constructors

        #region Methods
        public override Object Copy()
        {
            TECSystem outSystem = new TECSystem(this);
            outSystem._guid = Guid;
            return outSystem;
        }

        public override object DragDropCopy()
        {
            TECSystem outSystem = new TECSystem(this);
            return outSystem;
        }

        private double getMaterialCost()
        {
            double cost = 0;
            foreach(TECEquipment equipment in this.Equipment)
            {
                cost += equipment.MaterialCost;
            }
            return cost;
        }
        private double getLaborCost()
        {
            double cost = 0;
            foreach (TECEquipment equipment in this.Equipment)
            {
                cost += equipment.LaborCost;
            }
            return cost;
        }

        private void EquipmentChanged(string name)
        {
            if(name == "Quantity")
            {
                RaisePropertyChanged("SubScopeQuantity");
                RaisePropertyChanged("TotalBudgetPrice");
                RaisePropertyChanged("PriceWithEquipment");
                RaisePropertyChanged("EquipmentQuantity");
            } else if (name == "SubScopeQuantity")
            {
                RaisePropertyChanged("SubScopeQuantity");
            } else if (name == "BudgetPrice")
            {
                RaisePropertyChanged("TotalBudgetPrice");
                RaisePropertyChanged("PriceWithEquipment");
            } else if (name == "TotalPoints")
            {
                RaisePropertyChanged("TotalPoints");
            } else if (name == "TotalDevices")
            {
                RaisePropertyChanged("TotalDevices");
            }
            else if (name == "SubLength")
            {
                RaisePropertyChanged("SubLength");
            }
        }

        private void Equipment_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged("EquipmentQuantity");
            RaisePropertyChanged("PriceWithEquipment");
            if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    NotifyPropertyChanged("Add", this, item);
                    checkForTotalsInEquipment(item as TECEquipment);
                }

            } else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
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

            subscribeToEquipment();
        }

        private void subscribeToEquipment()
        {
            foreach (TECEquipment scope in this.Equipment)
            {
                scope.PropertyChanged += (equipSender, args) => this.EquipmentChanged(args.PropertyName);
            }
        }

        private void checkForTotalsInEquipment(TECEquipment equipment)
        {
            foreach (TECSubScope sub in equipment.SubScope)
            {
                if (sub.Points.Count > 0)
                {
                    RaisePropertyChanged("TotalPoints");
                }
                if (sub.Devices.Count > 0)
                {
                    RaisePropertyChanged("TotalDevices");
                }
            }
        }

        #endregion

    }
}
