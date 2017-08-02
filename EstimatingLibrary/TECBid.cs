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
    public class TECBid : TECScopeManager
    {
        #region Properties
        private string _name;
        private string _bidNumber;
        private DateTime _dueDate;
        private string _salesperson;
        private string _estimator;
        private TECBidParameters _parameters;

        private ObservableCollection<TECScopeBranch> _scopeTree { get; set; }
        private ObservableCollection<TECSystem> _systems { get; set; }
        private ObservableCollection<TECLabeled> _notes { get; set; }
        private ObservableCollection<TECLabeled> _exclusions { get; set; }
        private ObservableCollection<TECLabeled> _locations { get; set; }
        private ObservableCollection<TECController> _controllers { get; set; }
        private ObservableCollection<TECMisc> _miscCosts { get; set; }
        private ObservableCollection<TECPanel> _panels { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    var old = _name;
                    _name = value;
                    // Call RaisePropertyChanged whenever the property is updated
                    NotifyPropertyChanged(Change.Edit, "Name", this, value, old);
                }
            }
        }
        public string BidNumber
        {
            get { return _bidNumber; }
            set
            {
                var old = BidNumber;
                _bidNumber = value;
                NotifyPropertyChanged(Change.Edit, "BidNumber", this, value, old);
            }
        }
        public DateTime DueDate
        {
            get { return _dueDate; }
            set
            {
                var old = DueDate;
                _dueDate = value;
                // Call RaisePropertyChanged whenever the property is updated
                NotifyPropertyChanged(Change.Edit, "DueDate", this, value, old);
            }
        }
        public string DueDateString
        {
            get { return _dueDate.ToString("O"); }
        }
        public string Salesperson
        {
            get { return _salesperson; }
            set
            {
                var old = Salesperson;
                _salesperson = value;
                NotifyPropertyChanged(Change.Edit, "Salesperson", this, value, old);
            }
        }
        public string Estimator
        {
            get { return _estimator; }
            set
            {
                var old = Estimator;
                _estimator = value;
                NotifyPropertyChanged(Change.Edit, "Estimator", this, value, old);
            }
        }

        public TECBidParameters Parameters
        {
            get { return _parameters; }
            set
            {
                var old = Parameters;
                Parameters.PropertyChanged -= objectPropertyChanged;
                _parameters = value;
                NotifyPropertyChanged(Change.Edit, "Parameters", this, value, old);
                Parameters.PropertyChanged += objectPropertyChanged;
            }
        }
        public override TECLabor Labor
        {
            get
            {
                return base.Labor;
            }

            set
            {
                if(Labor != null)
                {
                    Labor.PropertyChanged -= objectPropertyChanged;
                }
                base.Labor = value;
                Labor.PropertyChanged += objectPropertyChanged;
                
            }
        }

        public ObservableCollection<TECScopeBranch> ScopeTree
        {
            get { return _scopeTree; }
            set
            {
                var old = ScopeTree;
                ScopeTree.CollectionChanged -= (sender, args) => CollectionChanged(sender, args, "ScopeTree");
                _scopeTree = value;
                ScopeTree.CollectionChanged += (sender, args) => CollectionChanged(sender, args, "ScopeTree");
                NotifyPropertyChanged(Change.Edit, "ScopeTree", this, value, old);
            }
        }
        public ObservableCollection<TECSystem> Systems
        {
            get { return _systems; }
            set
            {
                var old = Systems;
                Systems.CollectionChanged -= (sender, args) => CollectionChanged(sender, args, "Systems");
                _systems = value;
                registerSystems();
                Systems.CollectionChanged += (sender, args) => CollectionChanged(sender, args, "Systems");
                NotifyPropertyChanged(Change.Edit, "Systems", this, value, old);
            }
        }
        public ObservableCollection<TECLabeled> Notes
        {
            get { return _notes; }
            set
            {
                var old = Notes;
                Notes.CollectionChanged -= (sender, args) => CollectionChanged(sender, args, "Notes");
                _notes = value;
                Notes.CollectionChanged += (sender, args) => CollectionChanged(sender, args, "Notes");
                NotifyPropertyChanged(Change.Edit, "Notes", this, value, old);
            }
        }
        public ObservableCollection<TECLabeled> Exclusions
        {
            get { return _exclusions; }
            set
            {
                var old = Exclusions;
                Exclusions.CollectionChanged -= (sender, args) => CollectionChanged(sender, args, "Exclusions");
                _exclusions = value;
                Exclusions.CollectionChanged += (sender, args) => CollectionChanged(sender, args, "Exclusions");
                NotifyPropertyChanged(Change.Edit, "Exclusions", this, value, old);
            }
        }
        public ObservableCollection<TECLabeled> Locations
        {
            get { return _locations; }
            set
            {
                var old = Locations;
                Locations.CollectionChanged -= (sender, args) => CollectionChanged(sender, args, "Locations");
                Locations.CollectionChanged -= Locations_CollectionChanged;
                _locations = value;
                Locations.CollectionChanged += (sender, args) => CollectionChanged(sender, args, "Locations");
                Locations.CollectionChanged += Locations_CollectionChanged;
                NotifyPropertyChanged(Change.Edit, "Locations", this, value, old);
            }
        }
        
        public ObservableCollection<TECController> Controllers
        {
            get { return _controllers; }
            set
            {
                var old = Controllers;
                Controllers.CollectionChanged -= (sender, args) => CollectionChanged(sender, args, "Controllers");
                _controllers = value;
                Controllers.CollectionChanged += (sender, args) => CollectionChanged(sender, args, "Controllers");
                NotifyPropertyChanged(Change.Edit, "Controllers", this, value, old);
            }
        }
        public ObservableCollection<TECMisc> MiscCosts
        {
            get { return _miscCosts; }
            set
            {
                var old = MiscCosts;
                MiscCosts.CollectionChanged -= (sender, args) => CollectionChanged(sender, args, "MiscCosts");
                _miscCosts = value;
                MiscCosts.CollectionChanged += (sender, args) => CollectionChanged(sender, args, "MiscCosts");
                NotifyPropertyChanged(Change.Edit, "MiscCosts", this, value, old);
            }
        }
        public ObservableCollection<TECPanel> Panels
        {
            get { return _panels; }
            set
            {
                var old = Panels;
                Panels.CollectionChanged -= (sender, args) => CollectionChanged(sender, args, "Panels");
                _panels = value;
                Panels.CollectionChanged += (sender, args) => CollectionChanged(sender, args, "Panels");
                NotifyPropertyChanged(Change.Edit, "Panels", this, value, old);
            }
        }

        public int TotalPointNumber
        {
            get
            {
                return getPointNumber();
            }
        }

        private TECEstimator _estimate;
        public TECEstimator Estimate
        {
            get { return _estimate; }
            set
            {
                _estimate = value;
                RaisePropertyChanged("Estimate");
            }
        }
        #endregion //Properties

        #region Constructors
        public TECBid(Guid guid) : base(guid)
        {
            _name = "";
            _bidNumber = "";
            _salesperson = "";
            _estimator = "";
            _scopeTree = new ObservableCollection<TECScopeBranch>();
            _systems = new ObservableCollection<TECSystem>();
            _notes = new ObservableCollection<TECLabeled>();
            _exclusions = new ObservableCollection<TECLabeled>();
            _locations = new ObservableCollection<TECLabeled>();
            _controllers = new ObservableCollection<TECController>();
            _miscCosts = new ObservableCollection<TECMisc>();
            _panels = new ObservableCollection<TECPanel>();
            _labor = new TECLabor();
            _parameters = new TECBidParameters();
            _estimate = new TECEstimator(this);
            Parameters.PropertyChanged += objectPropertyChanged;
            Labor.PropertyChanged += objectPropertyChanged;

            Systems.CollectionChanged += (sender, args) => CollectionChanged(sender, args, "Systems");
            ScopeTree.CollectionChanged += (sender, args) => CollectionChanged(sender, args, "ScopeTree");
            Notes.CollectionChanged += (sender, args) => CollectionChanged(sender, args, "Notes");
            Exclusions.CollectionChanged += (sender, args) => CollectionChanged(sender, args, "Exclusions");
            Locations.CollectionChanged += (sender, args) => CollectionChanged(sender, args, "Locations");
            Locations.CollectionChanged += Locations_CollectionChanged;
            Controllers.CollectionChanged += (sender, args) => CollectionChanged(sender, args, "Controllers");
            MiscCosts.CollectionChanged += (sender, args) => CollectionChanged(sender, args, "MiscCosts");
            Panels.CollectionChanged += (sender, args) => CollectionChanged(sender, args, "Panels");

            registerSystems();
        }

        public TECBid() : this(Guid.NewGuid())
        {
            foreach (string item in Defaults.Scope)
            {
                var branchToAdd = new TECScopeBranch();
                branchToAdd.Label = item;
                ScopeTree.Add(new TECScopeBranch(branchToAdd));
            }
            foreach (string item in Defaults.Exclusions)
            {
                var exclusionToAdd = new TECLabeled();
                exclusionToAdd.Label = item;
                Exclusions.Add(new TECLabeled(exclusionToAdd));
            }
            foreach (string item in Defaults.Notes)
            {
                var noteToAdd = new TECLabeled();
                noteToAdd.Label = item;
                Notes.Add(new TECLabeled(noteToAdd));
            }
            _parameters.Overhead = 20;
            _parameters.Profit = 20;
            _parameters.SubcontractorMarkup = 20;
        }

        #endregion //Constructors

        #region Methods
        #region Event Handlers
        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e, string collectionName)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    NotifyPropertyChanged(Change.Add, collectionName, this, item);
                    if (item is TECCost)
                    {
                        (item as TECObject).PropertyChanged += objectPropertyChanged;
                    }
                    else if (item is TECSystem)
                    {
                        var sys = item as TECSystem;
                        sys.PropertyChanged += System_PropertyChanged;
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyPropertyChanged(Change.Remove, collectionName, this, item);
                    if (item is TECCost)
                    {
                        (item as TECCost).PropertyChanged -= objectPropertyChanged;
                    }
                    else if (item is TECSystem)
                    {
                        var sys = item as TECSystem;
                        sys.PropertyChanged -= System_PropertyChanged;
                        handleSystemSubScopeRemoval(item as TECSystem);
                    }
                    else if (item is TECController)
                    {
                        (item as TECController).RemoveAllConnections();
                    }
                }
                
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                NotifyPropertyChanged(Change.Edit, collectionName, this, e.NewItems, e.OldItems);
            }
        }
        private void Locations_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (TECLabeled location in e.OldItems)
                {
                    removeLocationFromScope(location);
                }
            }
        }
        private void System_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RemovedSubScope")
            {
                var args = e as PropertyChangedExtendedEventArgs;
                if (args.Sender is TECEquipment)
                {
                    handleEquipmentSubScopeRemoval(args.Value as TECEquipment);
                }
                else
                {
                    handleSubScopeRemovalInConnections(args.Value as TECSubScope);
                }
            }
        }
        private void objectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is TECLabor)
            {
                List<string> userAdjustmentPropertyNames = new List<string>()
                {
                    "PMExtraHours",
                    "SoftExtraHours",
                    "GraphExtraHours",
                    "ENGExtraHours",
                    "CommExtraHours"
                };
                if (userAdjustmentPropertyNames.Contains(e.PropertyName))
                {
                    NotifyPropertyChanged(Change.Edit, "Labor", this, Labor);
                }
            }
        }
        #endregion

        private int getPointNumber()
        {
            int totalPoints = 0;
            foreach (TECSystem sys in Systems)
            {
                foreach (TECEquipment equip in sys.Equipment)
                {
                    foreach (TECSubScope sub in equip.SubScope)
                    {
                        foreach (TECPoint point in sub.Points)
                        { totalPoints += point.Quantity; }
                    }
                }
            }
            return totalPoints;
        }
        
        private void registerSystems()
        {
            foreach (TECSystem system in Systems)
            {
                system.PropertyChanged += System_PropertyChanged;
            }
        }

        private void handleSystemSubScopeRemoval(TECSystem system)
        {
            foreach (TECEquipment equipment in system.Equipment)
            {
                handleEquipmentSubScopeRemoval(equipment);
            }
        }
        private void handleEquipmentSubScopeRemoval(TECEquipment equipment)
        {
            foreach (TECSubScope subScope in equipment.SubScope)
            {
                handleSubScopeRemovalInConnections(subScope);
            }
        }
        private void handleSubScopeRemovalInConnections(TECSubScope subScope)
        {
            foreach (TECController controller in Controllers)
            {
                ObservableCollection<TECSubScope> subScopeToRemove = new ObservableCollection<TECSubScope>();
                foreach (TECConnection connection in controller.ChildrenConnections)
                {
                    if (connection is TECSubScopeConnection)
                    {
                        if ((connection as TECSubScopeConnection).SubScope == subScope)
                        {
                            subScopeToRemove.Add(subScope as TECSubScope);
                        }
                    }

                }
                foreach (TECSubScope sub in subScopeToRemove)
                {
                    controller.RemoveSubScope(sub);
                }
            }
        }

        public void RefreshEstimate()
        {
            Estimate = new TECEstimator(this);
        }

        private void removeLocationFromScope(TECLabeled location)
        {
            foreach(TECSystem typical in this.Systems)
            {
                if (typical.Location == location) typical.Location = null;
                foreach(TECSystem instance in typical.SystemInstances)
                {
                    if (instance.Location == location) instance.Location = null;
                    foreach(TECEquipment equip in instance.Equipment)
                    {
                        if (equip.Location == location) equip.Location = null;
                        foreach(TECSubScope ss in equip.SubScope)
                        {
                            if (ss.Location == location) ss.Location = null;
                        }
                    }
                }
                foreach (TECEquipment equip in typical.Equipment)
                {
                    if (equip.Location == location) equip.Location = null;
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        if (ss.Location == location) ss.Location = null;
                    }
                }
            }
        }

        #endregion
    }
}
