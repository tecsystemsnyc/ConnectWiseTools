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
    public class TECBid : TECScopeManager, INotifyCostChanged, INotifyPointChanged, IRelatable
    {
        #region Fields
        private string _name;
        private string _bidNumber;
        private DateTime _dueDate;
        private string _salesperson;
        private string _estimator;
        private TECParameters _parameters;
        private TECExtraLabor _extraLabor;

        public event Action<CostBatch> CostChanged;
        public event Action<int> PointChanged;

        private ObservableCollection<TECScopeBranch> _scopeTree;
        private ObservableCollection<TECTypical> _systems;
        private ObservableCollection<TECLabeled> _notes;
        private ObservableCollection<TECLabeled> _exclusions;
        private ObservableCollection<TECLabeled> _locations;
        private ObservableCollection<TECController> _controllers;
        private ObservableCollection<TECMisc> _miscCosts;
        private ObservableCollection<TECPanel> _panels;
        #endregion

        #region Properties


        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    var old = _name;
                    _name = value;
                    notifyCombinedChanged(Change.Edit, "Name", this, value, old);
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
                notifyCombinedChanged(Change.Edit, "BidNumber", this, value, old);
            }
        }
        public DateTime DueDate
        {
            get { return _dueDate; }
            set
            {
                var old = DueDate;
                _dueDate = value;
                notifyCombinedChanged(Change.Edit, "DueDate", this, value, old);
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
                notifyCombinedChanged(Change.Edit, "Salesperson", this, value, old);
            }
        }
        public string Estimator
        {
            get { return _estimator; }
            set
            {
                var old = Estimator;
                _estimator = value;
                notifyCombinedChanged(Change.Edit, "Estimator", this, value, old);
            }
        }

        public TECParameters Parameters
        {
            get { return _parameters; }
            set
            {
                var old = Parameters;
                _parameters = value;
                notifyCombinedChanged(Change.Edit, "Parameters", this, value, old);
            }
        }
        public TECExtraLabor ExtraLabor
        {
            get { return _extraLabor; }
            set
            {
                var old = ExtraLabor;
                _extraLabor = value;
                notifyCombinedChanged(Change.Edit, "ExtraLabor", this, value, old);
                CostChanged?.Invoke(value.CostBatch - old.CostBatch);
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
                notifyCombinedChanged(Change.Edit, "ScopeTree", this, value, old);
            }
        }
        public ObservableCollection<TECTypical> Systems
        {
            get { return _systems; }
            set
            {
                var old = Systems;
                Systems.CollectionChanged -= (sender, args) => collectionChanged(sender, args, "Systems");
                _systems = value;
                registerSystems();
                Systems.CollectionChanged += (sender, args) => collectionChanged(sender, args, "Systems");
                notifyCombinedChanged(Change.Edit, "Systems", this, value, old);
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
                notifyCombinedChanged(Change.Edit, "Notes", this, value, old);
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
                notifyCombinedChanged(Change.Edit, "Exclusions", this, value, old);
            }
        }
        public ObservableCollection<TECLabeled> Locations
        {
            get { return _locations; }
            set
            {
                var old = Locations;
                Locations.CollectionChanged -= (sender, args) => collectionChanged(sender, args, "Locations");
                Locations.CollectionChanged -= locationsCollectionChanged;
                _locations = value;
                Locations.CollectionChanged += (sender, args) => collectionChanged(sender, args, "Locations");
                Locations.CollectionChanged += locationsCollectionChanged;
                notifyCombinedChanged(Change.Edit, "Locations", this, value, old);
            }
        }
        
        public ReadOnlyObservableCollection<TECController> Controllers
        {
            get { return new ReadOnlyObservableCollection<TECController>(_controllers); }
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
                notifyCombinedChanged(Change.Edit, "MiscCosts", this, value, old);
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
                notifyCombinedChanged(Change.Edit, "Panels", this, value, old);
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

        public SaveableMap PropertyObjects
        {
            get
            {
                return propertyObjects();
            }
        }

        public SaveableMap LinkedObjects
        {
            get
            {
                return new SaveableMap();
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
            _systems = new ObservableCollection<TECTypical>();
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
            Locations.CollectionChanged += locationsCollectionChanged;
            MiscCosts.CollectionChanged += (sender, args) => collectionChanged(sender, args, "MiscCosts");
            Panels.CollectionChanged += (sender, args) => collectionChanged(sender, args, "Panels");

            registerSystems();
        }

        public TECBid() : this(Guid.NewGuid())
        {
            foreach (string item in Defaults.Scope)
            {
                var branchToAdd = new TECScopeBranch(false);
                branchToAdd.Label = item;
                ScopeTree.Add(new TECScopeBranch(branchToAdd, false));
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
        #region Add/Remove Object Methods
        public void AddController(TECController controller)
        {
            _controllers.Add(controller);
            notifyCombinedChanged(Change.Add, "Controllers", this, controller);
            CostChanged?.Invoke(controller.CostBatch);
        }
        public void RemoveController(TECController controller)
        {
            _controllers.Remove(controller);
            notifyCombinedChanged(Change.Remove, "Controllers", this, controller);
            CostChanged?.Invoke(-controller.CostBatch);
            controller.RemoveAllConnections();
        }
        public void SetControllers(IEnumerable<TECController> newControllers)
        {
            IEnumerable<TECController> oldControllers = Controllers;
            _controllers = new ObservableCollection<TECController>(newControllers);
            notifyCombinedChanged(Change.Edit, "Controllers", this, newControllers, oldControllers);
        }
        #endregion

        #region Event Handlers
        private void collectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e, string collectionName)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                int pointNumber = 0;
                CostBatch costs = new CostBatch();
                bool pointChanged = false;
                bool costChanged = false;
                foreach (object item in e.NewItems)
                {
                    if (item is INotifyPointChanged pointItem)
                    {
                        pointNumber += pointItem.PointNumber;
                        pointChanged = true;
                    }
                    if (item is INotifyCostChanged costItem)
                    {
                        costs += costItem.CostBatch;
                        costChanged = true;
                    }
                    notifyCombinedChanged(Change.Add, collectionName, this, item);
                    if (item is TECTypical typical)
                    {
                        typical.PropertyChanged += system_PropertyChanged;
                        costChanged = false;
                        pointChanged = false;
                    }
                }
                if (pointChanged) PointChanged?.Invoke(pointNumber);
                if (costChanged) CostChanged?.Invoke(costs);
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                int pointNumber = 0;
                CostBatch costs = new CostBatch();
                bool pointChanged = false;
                bool costChanged = false;
                foreach (object item in e.OldItems)
                {
                    if (item is INotifyPointChanged pointItem)
                    {
                        pointNumber += pointItem.PointNumber;
                        pointChanged = true;
                    }
                    if (item is INotifyCostChanged costItem)
                    {
                        costs += costItem.CostBatch;
                        costChanged = true;
                    }
                    notifyCombinedChanged(Change.Remove, collectionName, this, item);
                    if (item is TECTypical typ)
                    {
                        var sys = item as TECSystem;
                        sys.PropertyChanged -= system_PropertyChanged;
                        handleSystemSubScopeRemoval(item as TECSystem);
                        if (typ.Instances.Count == 0)
                        {
                            costChanged = false;
                            pointChanged = false;
                        }
                    }
                }
                if (pointChanged) PointChanged?.Invoke(pointNumber);
                if (costChanged) CostChanged?.Invoke(costs * -1);
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                notifyCombinedChanged(Change.Edit, collectionName, this, e.NewItems, e.OldItems);
            }
        }
        private void locationsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
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
            foreach(TECTypical system in this.Systems)
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
        private SaveableMap propertyObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.Add(this.Parameters, "Parameters");
            saveList.Add(this.Catalogs, "Catalogs");
            saveList.Add(this.ExtraLabor, "ExtraLabor");
            saveList.AddRange(this.ScopeTree, "ScopeTree");
            saveList.AddRange(this.Notes, "Notes");
            saveList.AddRange(this.Exclusions, "Exclusions");
            saveList.AddRange(this.Systems, "Systems");
            saveList.AddRange(this.Controllers, "Controllers");
            saveList.AddRange(this.Panels, "Panels");
            saveList.AddRange(this.MiscCosts, "MiscCosts");
            saveList.AddRange(this.Locations, "Locations");
            return saveList;
        }

        private int pointNumber()
        {
            int totalPoints = 0;
            foreach (TECSystem sys in Systems)
            {
                totalPoints += sys.PointNumber;
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
            foreach(TECTypical typical in this.Systems)
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
