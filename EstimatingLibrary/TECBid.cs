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
    public class TECBid : TECScopeManager, INotifyCostChanged, INotifyPointChanged, ISaveable
    {
        #region Properties
        private string _name;
        private string _bidNumber;
        private DateTime _dueDate;
        private string _salesperson;
        private string _estimator;
        private TECParameters _parameters;
        private TECExtraLabor _extraLabor;

        public event Action<CostBatch> CostChanged;
        public event Action<int> PointChanged;

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
                    NotifyCombinedChanged(Change.Edit, "Name", this, value, old);
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
                NotifyCombinedChanged(Change.Edit, "BidNumber", this, value, old);
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
                NotifyCombinedChanged(Change.Edit, "DueDate", this, value, old);
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
                NotifyCombinedChanged(Change.Edit, "Salesperson", this, value, old);
            }
        }
        public string Estimator
        {
            get { return _estimator; }
            set
            {
                var old = Estimator;
                _estimator = value;
                NotifyCombinedChanged(Change.Edit, "Estimator", this, value, old);
            }
        }

        public TECParameters Parameters
        {
            get { return _parameters; }
            set
            {
                var old = Parameters;
                _parameters = value;
                NotifyCombinedChanged(Change.Edit, "Parameters", this, value, old);
            }
        }
        public TECExtraLabor ExtraLabor
        {
            get { return _extraLabor; }
            set
            {
                var old = ExtraLabor;
                _extraLabor = value;
                NotifyCombinedChanged(Change.Edit, "ExtraLabor", this, value, old);
            }

        }

        public ObservableCollection<TECScopeBranch> ScopeTree
        {
            get { return _scopeTree; }
            set
            {
                var old = ScopeTree;
                ScopeTree.CollectionChanged -= (sender, args) => collectionChanged(sender, args, "ScopeTree");
                _scopeTree = value;
                ScopeTree.CollectionChanged += (sender, args) => collectionChanged(sender, args, "ScopeTree");
                NotifyCombinedChanged(Change.Edit, "ScopeTree", this, value, old);
            }
        }
        public ObservableCollection<TECSystem> Systems
        {
            get { return _systems; }
            set
            {
                var old = Systems;
                Systems.CollectionChanged -= (sender, args) => collectionChanged(sender, args, "Systems");
                _systems = value;
                registerSystems();
                Systems.CollectionChanged += (sender, args) => collectionChanged(sender, args, "Systems");
                NotifyCombinedChanged(Change.Edit, "Systems", this, value, old);
            }
        }
        public ObservableCollection<TECLabeled> Notes
        {
            get { return _notes; }
            set
            {
                var old = Notes;
                Notes.CollectionChanged -= (sender, args) => collectionChanged(sender, args, "Notes");
                _notes = value;
                Notes.CollectionChanged += (sender, args) => collectionChanged(sender, args, "Notes");
                NotifyCombinedChanged(Change.Edit, "Notes", this, value, old);
            }
        }
        public ObservableCollection<TECLabeled> Exclusions
        {
            get { return _exclusions; }
            set
            {
                var old = Exclusions;
                Exclusions.CollectionChanged -= (sender, args) => collectionChanged(sender, args, "Exclusions");
                _exclusions = value;
                Exclusions.CollectionChanged += (sender, args) => collectionChanged(sender, args, "Exclusions");
                NotifyCombinedChanged(Change.Edit, "Exclusions", this, value, old);
            }
        }
        public ObservableCollection<TECLabeled> Locations
        {
            get { return _locations; }
            set
            {
                var old = Locations;
                Locations.CollectionChanged -= (sender, args) => collectionChanged(sender, args, "Locations");
                Locations.CollectionChanged -= locations_CollectionChanged;
                _locations = value;
                Locations.CollectionChanged += (sender, args) => collectionChanged(sender, args, "Locations");
                Locations.CollectionChanged += locations_CollectionChanged;
                NotifyCombinedChanged(Change.Edit, "Locations", this, value, old);
            }
        }
        
        public ObservableCollection<TECController> Controllers
        {
            get { return _controllers; }
            set
            {
                var old = Controllers;
                Controllers.CollectionChanged -= (sender, args) => collectionChanged(sender, args, "Controllers");
                _controllers = value;
                Controllers.CollectionChanged += (sender, args) => collectionChanged(sender, args, "Controllers");
                NotifyCombinedChanged(Change.Edit, "Controllers", this, value, old);
            }
        }
        public ObservableCollection<TECMisc> MiscCosts
        {
            get { return _miscCosts; }
            set
            {
                var old = MiscCosts;
                MiscCosts.CollectionChanged -= (sender, args) => collectionChanged(sender, args, "MiscCosts");
                _miscCosts = value;
                MiscCosts.CollectionChanged += (sender, args) => collectionChanged(sender, args, "MiscCosts");
                NotifyCombinedChanged(Change.Edit, "MiscCosts", this, value, old);
            }
        }
        public ObservableCollection<TECPanel> Panels
        {
            get { return _panels; }
            set
            {
                var old = Panels;
                Panels.CollectionChanged -= (sender, args) => collectionChanged(sender, args, "Panels");
                _panels = value;
                Panels.CollectionChanged += (sender, args) => collectionChanged(sender, args, "Panels");
                NotifyCombinedChanged(Change.Edit, "Panels", this, value, old);
            }
        }
        
        public CostBatch CostBatch
        {
            get { return getCosts();  }
        }
        public int PointNumber
        {
            get
            {
                return pointNumber();
            }
        }

        public List<TECObject> SaveObjects
        {
            get
            {
                return saveObjects();
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
            _extraLabor = new TECExtraLabor(this.Guid);
            _parameters = new TECParameters(this.Guid);

            Systems.CollectionChanged += (sender, args) => collectionChanged(sender, args, "Systems");
            ScopeTree.CollectionChanged += (sender, args) => collectionChanged(sender, args, "ScopeTree");
            Notes.CollectionChanged += (sender, args) => collectionChanged(sender, args, "Notes");
            Exclusions.CollectionChanged += (sender, args) => collectionChanged(sender, args, "Exclusions");
            Locations.CollectionChanged += (sender, args) => collectionChanged(sender, args, "Locations");
            Locations.CollectionChanged += locations_CollectionChanged;
            Controllers.CollectionChanged += (sender, args) => collectionChanged(sender, args, "Controllers");
            MiscCosts.CollectionChanged += (sender, args) => collectionChanged(sender, args, "MiscCosts");
            Panels.CollectionChanged += (sender, args) => collectionChanged(sender, args, "Panels");

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
        private void collectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e, string collectionName)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                int pointNumber = 0;
                CostBatch costs = new CostBatch();
                foreach (object item in e.NewItems)
                {
                    if (item is INotifyPointChanged pointItem) { pointNumber += pointItem.PointNumber; }
                    if (item is INotifyCostChanged costItem) { costs += costItem.CostBatch; }
                    NotifyCombinedChanged(Change.Add, collectionName, this, item);
                    if (item is TECSystem)
                    {
                        var sys = item as TECSystem;
                        sys.PropertyChanged += system_PropertyChanged;
                    }
                }
                PointChanged?.Invoke(pointNumber);
                CostChanged?.Invoke(costs);
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                int pointNumber = 0;
                CostBatch costs = new CostBatch();
                foreach (object item in e.OldItems)
                {
                    if(item is INotifyPointChanged pointItem) { pointNumber += pointItem.PointNumber; }
                    if(item is INotifyCostChanged costItem) { costs += costItem.CostBatch; }
                    NotifyCombinedChanged(Change.Remove, collectionName, this, item);
                    if (item is TECSystem)
                    {
                        var sys = item as TECSystem;
                        sys.PropertyChanged -= system_PropertyChanged;
                        handleSystemSubScopeRemoval(item as TECSystem);
                    }
                    else if (item is TECController)
                    {
                        (item as TECController).RemoveAllConnections();
                    }
                }
                PointChanged?.Invoke(pointNumber);
                CostChanged?.Invoke(costs * -1);
                
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                NotifyCombinedChanged(Change.Edit, collectionName, this, e.NewItems, e.OldItems);
            }
        }
        private void locations_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (TECLabeled location in e.OldItems)
                {
                    removeLocationFromScope(location);
                }
            }
        }
        private void system_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RemovedSubScope")
            {
                var args = e as TECChangedEventArgs;
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
        #endregion
        
        private CostBatch getCosts()
        {
            CostBatch costs = new CostBatch();
            foreach(TECMisc misc in this.MiscCosts)
            {
                costs += misc.CostBatch;
            }
            foreach(TECSystem system in this.Systems)
            {
                costs += system.CostBatch;
            }
            foreach(TECController controller in this.Controllers)
            {
                costs += controller.CostBatch;
            }
            foreach(TECPanel panel in this.Panels)
            {
                costs += panel.CostBatch;
            }
            return costs;
        }
        private List<TECObject> saveObjects()
        {
            List<TECObject> saveList = new List<TECObject>();
            saveList.Add(this.Parameters);
            saveList.Add(this.Catalogs);
            saveList.Add(this.ExtraLabor);
            saveList.AddRange(this.ScopeTree);
            saveList.AddRange(this.Notes);
            saveList.AddRange(this.Exclusions);
            saveList.AddRange(this.Systems);
            saveList.AddRange(this.Controllers);
            saveList.AddRange(this.Panels);
            saveList.AddRange(this.MiscCosts);
            saveList.AddRange(this.Locations);
            return saveList;
        }

        private int pointNumber()
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
                system.PropertyChanged += system_PropertyChanged;
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
        
        private void removeLocationFromScope(TECLabeled location)
        {
            foreach(TECSystem typical in this.Systems)
            {
                if (typical.Location == location) typical.Location = null;
                foreach(TECSystem instance in typical.Instances)
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
