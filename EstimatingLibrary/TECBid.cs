using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECBid : TECObject
    {
        #region Properties
        private string _name;
        private string _bidNumber;
        private DateTime _dueDate;
        private string _salesperson;
        private string _estimator;
        private Guid _guid;
        private TECLabor _labor;
        private TECBidParameters _parameters;

        private ObservableCollection<TECScopeBranch> _scopeTree { get; set; }
        private ObservableCollection<TECSystem> _systems { get; set; }
        private ObservableCollection<TECDevice> _deviceCatalog { get; set; }
        private ObservableCollection<TECManufacturer> _manufacturerCatalog { get; set; }
        private ObservableCollection<TECNote> _notes { get; set; }
        private ObservableCollection<TECExclusion> _exclusions { get; set; }
        private ObservableCollection<TECTag> _tags { get; set; }
        private ObservableCollection<TECDrawing> _drawings { get; set; }
        private ObservableCollection<TECLocation> _locations { get; set; }
        private ObservableCollection<TECConnection> _connections { get; set; }
        private ObservableCollection<TECController> _controllers { get; set; }
        private ObservableCollection<TECProposalScope> _proposalScope { get; set; }
        private ObservableCollection<TECConnectionType> _connectionTypes { get; set; }
        private ObservableCollection<TECConduitType> _conduitTypes { get; set; }
        private ObservableCollection<TECAssociatedCost> _associatedCostsCatalog { get; set; }
        private ObservableCollection<TECCostAddition> _costAdditions { get; set; }

        public string Name {
            get { return _name; }
            set
            {
                var temp = Copy();
                _name = value;
                // Call RaisePropertyChanged whenever the property is updated
                NotifyPropertyChanged("Name", temp, this);
            }
        }
        public string BidNumber
        {
            get { return _bidNumber; }
            set
            {
                var temp = Copy();
                _bidNumber = value;
                NotifyPropertyChanged("BidNumber", temp, this);
            }
        }
        public DateTime DueDate {
            get { return _dueDate; }
            set
            {
                var temp = Copy();
                _dueDate = value;
                // Call RaisePropertyChanged whenever the property is updated
                NotifyPropertyChanged("DueDate", temp, this);
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
                var temp = Copy();
                _salesperson = value;
                NotifyPropertyChanged("Salesperson", temp, this);
            }
        }
        public string Estimator
        {
            get { return _estimator; }
            set
            {
                var temp = Copy();
                _estimator = value;
                NotifyPropertyChanged("Estimator", temp, this);
            }
        }
        public Guid Guid
        {
            get { return _guid; }
        }

        public TECLabor Labor
        {
            get { return _labor; }
            set
            {
                var temp = Copy();
                _labor = value;
                NotifyPropertyChanged("Labor", temp, this);
                Labor.PropertyChanged += objectPropertyChanged;
                Labor.NumPoints = getPointNumber();
            }
        }

        public TECBidParameters Parameters
        {
            get { return _parameters; }
            set
            {
                var temp = Copy();
                _parameters = value;
                NotifyPropertyChanged("Parameters", temp, this);
                Parameters.PropertyChanged += objectPropertyChanged;
            }
        }
        
        public double MaterialCost
        {
            get
            {
                return EstimateCalculator.GetMaterialCost(this);
            }
        }
        public double Tax
        {
            get
            {  return EstimateCalculator.GetTax(this); }
        }
        public double TECSubtotal
        {
            get
            {
                return EstimateCalculator.GetTECSubtotal(this);
            }
        }

        public double SubcontractorLaborCost
        {
            get { return EstimateCalculator.GetElectricalLaborCost(this); }
        }
        public double ElectricalMaterialCost
        {
            get
            { return EstimateCalculator.GetElectricalMaterialCost(this); }
        }
        public double SubcontractorSubtotal
        {
            get
            {
                return EstimateCalculator.GetSubcontractorSubtotal(this);
            }
        }

        public double TotalPrice
        {
            get
            {
                return EstimateCalculator.GetTotalPrice(this);
            }
        }

        public double BudgetPrice
        {
            get { return EstimateCalculator.GetBudgetPrice(this); }
        }
        public int TotalPointNumber
        {
            get
            {
                return getPointNumber();
            }
        }
        public double PricePerPoint
        {
            get { return EstimateCalculator.GetPricePerPoint(this); }
        }
        public double Margin
        {
            get { return EstimateCalculator.GetMargin(this); }
        }

        public ObservableCollection<TECScopeBranch> ScopeTree {
            get { return _scopeTree; }
            set
            {
                var temp = this.Copy();
                ScopeTree.CollectionChanged -= CollectionChanged;
                _scopeTree = value;
                ScopeTree.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("ScopeTree", temp, this);
            }
        }
        public ObservableCollection<TECSystem> Systems {
            get { return _systems; }
            set
            {
                var temp = this.Copy();
                Systems.CollectionChanged -= CollectionChanged;
                _systems = value;
                Systems.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("Systems", temp, this);
                updatePoints();
            }
        }
        public ObservableCollection<TECDevice> DeviceCatalog {
            get { return _deviceCatalog; }
            set
            {
                var temp = this.Copy();
                DeviceCatalog.CollectionChanged -= CollectionChanged;
                _deviceCatalog = value;
                DeviceCatalog.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("DeviceCatalog", temp, this);
            }
        }
        public ObservableCollection<TECManufacturer> ManufacturerCatalog {
            get { return _manufacturerCatalog; }
            set
            {
                var temp = this.Copy();
                ManufacturerCatalog.CollectionChanged -= CollectionChanged;
                _manufacturerCatalog = value;
                ManufacturerCatalog.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("ManufacturerCatalog", temp, this);
            }
        }
        public ObservableCollection<TECNote> Notes {
            get { return _notes; }
            set
            {
                var temp = this.Copy();
                Notes.CollectionChanged -= CollectionChanged;
                _notes = value;
                Notes.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("Notes", temp, this);
            }
        }
        public ObservableCollection<TECExclusion> Exclusions {
            get { return _exclusions; }
            set
            {
                var temp = this.Copy();
                Exclusions.CollectionChanged -= CollectionChanged;
                _exclusions = value;
                Exclusions.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("Exclusions", temp, this);
            }
        }
        public ObservableCollection<TECTag> Tags {
            get { return _tags; }
            set
            {
                var temp = this.Copy();
                Tags.CollectionChanged -= CollectionChanged;
                _tags = value;
                Tags.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("Tags", temp, this);
            }
        }
        public ObservableCollection<TECDrawing> Drawings {
            get { return _drawings; }
            set
            {
                var temp = this.Copy();
                Drawings.CollectionChanged -= CollectionChanged;
                _drawings = value;
                Drawings.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("Drawings", temp, this);
            }
        }
        public ObservableCollection<TECLocation> Locations
        {
            get { return _locations; }
            set
            {
                var temp = this.Copy();
                Locations.CollectionChanged -= CollectionChanged;
                _locations = value;
                Locations.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("Locations", temp, this);
            }
        }
        public ObservableCollection<TECConnection> Connections
        {
            get { return _connections; }
            set
            {
                var temp = this.Copy();
                Connections.CollectionChanged -= CollectionChanged;
                _connections = value;
                Connections.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("Connections", temp, this);
            }
        }
        public ObservableCollection<TECController> Controllers
        {
            get { return _controllers; }
            set
            {
                var temp = this.Copy();
                Controllers.CollectionChanged -= CollectionChanged;
                _controllers = value;
                Controllers.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("Controllers", temp, this);
            }
        }
        public ObservableCollection<TECProposalScope> ProposalScope
        {
            get { return _proposalScope; }
            set { _proposalScope = value; }
        }
        public ObservableCollection<TECConnectionType> ConnectionTypes
        {
            get { return _connectionTypes; }
            set
            {
                var temp = this.Copy();
                ConnectionTypes.CollectionChanged -= CollectionChanged;
                _connectionTypes = value;
                ConnectionTypes.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("ConnectionTypes", temp, this);
            }
        }
        public ObservableCollection<TECConduitType> ConduitTypes
        {
            get { return _conduitTypes; }
            set
            {
                var temp = this.Copy();
                ConduitTypes.CollectionChanged -= CollectionChanged;
                _conduitTypes = value;
                ConduitTypes.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("ConduitTypes", temp, this);
            }
        }
        public ObservableCollection<TECAssociatedCost> AssociatedCostsCatalog
        {
            get { return _associatedCostsCatalog; }
            set
            {
                var temp = this.Copy();
                AssociatedCostsCatalog.CollectionChanged -= CollectionChanged;
                _associatedCostsCatalog = value;
                AssociatedCostsCatalog.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("AssociatedCostsCatalog", temp, this);
            }
        }
        public ObservableCollection<TECCostAddition> CostAdditions
        {
            get { return _costAdditions; }
            set
            {
                var temp = this.Copy();
                CostAdditions.CollectionChanged -= CollectionChanged;
                _costAdditions = value;
                CostAdditions.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("CostAdditions", temp, this);
            }
        }


        #endregion //Properties

        #region Constructors
        public TECBid(Guid guid)
        {
            _guid = guid;
            _name = "";
            _bidNumber = "";
            _salesperson = "";
            _estimator = "";
            _scopeTree = new ObservableCollection<TECScopeBranch>();
            _systems = new ObservableCollection<TECSystem>();
            _deviceCatalog = new ObservableCollection<TECDevice>();
            _manufacturerCatalog = new ObservableCollection<TECManufacturer>();
            _notes = new ObservableCollection<TECNote>();
            _exclusions = new ObservableCollection<TECExclusion>();
            _tags = new ObservableCollection<TECTag>();
            _drawings = new ObservableCollection<TECDrawing>();
            _locations = new ObservableCollection<TECLocation>();
            _controllers = new ObservableCollection<TECController>();
            _connections = new ObservableCollection<TECConnection>();
            _proposalScope = new ObservableCollection<TECProposalScope>();
            _connectionTypes = new ObservableCollection<TECConnectionType>();
            _conduitTypes = new ObservableCollection<TECConduitType>();
            _associatedCostsCatalog = new ObservableCollection<TECAssociatedCost>();
            _costAdditions = new ObservableCollection<TECCostAddition>();
            _labor = new TECLabor();
            _parameters = new TECBidParameters();
            Parameters.PropertyChanged += objectPropertyChanged;
            Labor.PropertyChanged += objectPropertyChanged;

            Systems.CollectionChanged += CollectionChanged;
            ScopeTree.CollectionChanged += CollectionChanged;
            DeviceCatalog.CollectionChanged += CollectionChanged;
            ManufacturerCatalog.CollectionChanged += CollectionChanged;
            Notes.CollectionChanged += CollectionChanged;
            Exclusions.CollectionChanged += CollectionChanged;
            Tags.CollectionChanged += CollectionChanged;
            Drawings.CollectionChanged += CollectionChanged;
            Locations.CollectionChanged += CollectionChanged;
            Controllers.CollectionChanged += CollectionChanged;
            Connections.CollectionChanged += CollectionChanged;
            ProposalScope.CollectionChanged += CollectionChanged;
            ConnectionTypes.CollectionChanged += CollectionChanged;
            ConduitTypes.CollectionChanged += CollectionChanged;
            AssociatedCostsCatalog.CollectionChanged += CollectionChanged;
            CostAdditions.CollectionChanged += CollectionChanged;

            registerSystems();

            Labor.NumPoints = getPointNumber();
        }

        public TECBid() : this(Guid.NewGuid())
        {
            foreach(string item in Defaults.Scope)
            {
                var branchToAdd = new TECScopeBranch();
                branchToAdd.Name = item;
                ScopeTree.Add(new TECScopeBranch(branchToAdd));
            }
            foreach (string item in Defaults.Exclusions)
            {
                var exclusionToAdd = new TECExclusion();
                exclusionToAdd.Text = item;
                Exclusions.Add(new TECExclusion(exclusionToAdd));
            }
            foreach (string item in Defaults.Notes)
            {
                var noteToAdd = new TECNote();
                noteToAdd.Text = item;
                Notes.Add(new TECNote(noteToAdd));
            }
        }

        //Copy Constructor
        public TECBid(TECBid bidSource) : this(Guid.NewGuid())
        {
            _name = bidSource.Name;
            _bidNumber = bidSource.BidNumber;
            _dueDate = bidSource.DueDate;
            _salesperson = bidSource.Salesperson;
            _estimator = bidSource.Estimator;

            _labor = bidSource.Labor.Copy() as TECLabor;
            _parameters = bidSource.Parameters.Copy() as TECBidParameters;
            Parameters.PropertyChanged += objectPropertyChanged;
            Labor.PropertyChanged += objectPropertyChanged;

            foreach (TECScopeBranch branch in bidSource.ScopeTree)
            { ScopeTree.Add(branch.Copy() as TECScopeBranch); }
            foreach (TECSystem system in bidSource.Systems)
            { Systems.Add(system.Copy() as TECSystem); }
            foreach (TECNote note in bidSource.Notes)
            { Notes.Add(note.Copy() as TECNote); }
            foreach (TECExclusion exclusion in bidSource.Exclusions)
            { Exclusions.Add(exclusion.Copy() as TECExclusion); }
            foreach(TECAssociatedCost cost in bidSource.AssociatedCostsCatalog)
            {  AssociatedCostsCatalog.Add(cost.Copy() as TECAssociatedCost); }
            foreach(TECConduitType conduitType in bidSource.ConduitTypes)
            { ConduitTypes.Add(conduitType.Copy() as TECConduitType); }
            foreach(TECConnectionType connectionType in bidSource.ConnectionTypes)
            { ConnectionTypes.Add(connectionType.Copy() as TECConnectionType); }
            foreach(TECTag tag in bidSource.Tags)
            { Tags.Add(tag.Copy() as TECTag); }
            foreach (TECLocation location in bidSource.Locations)
            { Locations.Add(location.Copy() as TECLocation); }
            foreach (TECDrawing drawing in bidSource.Drawings)
            { Drawings.Add(drawing.Copy() as TECDrawing); }
            foreach(TECManufacturer manufacturer in bidSource.ManufacturerCatalog)
            { ManufacturerCatalog.Add(manufacturer.Copy() as TECManufacturer); }
            foreach(TECController controller in bidSource.Controllers)
            { Controllers.Add(controller.Copy() as TECController); }
            foreach(TECDevice device in bidSource.DeviceCatalog)
            { DeviceCatalog.Add(device.Copy() as TECDevice); }
            foreach(TECConnection connection in bidSource.Connections)
            { Connections.Add(connection.Copy() as TECConnection); }
            foreach(TECProposalScope propScope in bidSource.ProposalScope)
            { ProposalScope.Add(propScope.Copy() as TECProposalScope); }
            foreach(TECCostAddition cost in bidSource.CostAdditions)
            {
                CostAdditions.Add(cost.Copy() as TECCostAddition);
            }
        }

        #endregion //Constructors

        #region Methods

        #region Event Handlers
        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    if (item is TECProposalScope)
                    {
                        NotifyPropertyChanged("MetaAdd", this, item);
                    }
                    else
                    {
                        NotifyPropertyChanged("Add", this, item);
                        if (item is TECCostAddition)
                        {
                            updateElectricalMaterial();
                            (item as TECCostAddition).PropertyChanged += objectPropertyChanged;
                        }
                        if (item is TECSystem)
                        {
                            var sys = item as TECSystem;
                            addProposalScope(sys);
                            sys.PropertyChanged += System_PropertyChanged;
                            checkForTotalsInSystem(sys);
                        }
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    if (item is TECProposalScope)
                    {
                        NotifyPropertyChanged("MetaRemove", this, item);
                    }
                    else
                    {
                        if (item is TECCostAddition)
                        {
                            updateElectricalMaterial();
                            (item as TECCostAddition).PropertyChanged -= objectPropertyChanged;
                        }
                        NotifyPropertyChanged("Remove", this, item);
                        if (item is TECScope)
                        {
                            checkForVisualsToRemove((TECScope)item);
                        }
                        if (item is TECSystem)
                        {
                            var sys = item as TECSystem;
                            sys.PropertyChanged -= System_PropertyChanged;
                            removeProposalScope(sys);
                        }
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                NotifyPropertyChanged("Edit", this, sender);
            }
        }

        private void System_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TotalPoints")
            {
                updatePoints();
            }
            else if (e.PropertyName == "TotalDevices")
            {
                updateDevices();
            }
            else if (e.PropertyName == "SubLength")
            {
                updateElectricalMaterial();
            }
        }

        private void objectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("ChildChanged", this, sender);
            if (sender is TECLabor)
            { updateFromLabor(); }
            else if (sender is TECBidParameters)
            { updateFromParameters(); }
            else if (sender is TECCostAddition)
            { updateElectricalMaterial(); }
        }

        #endregion


        private int getPointNumber()
        {
            int totalPoints = 0;
            foreach(TECSystem sys in Systems)
            {
                foreach(TECEquipment equip in sys.Equipment)
                {
                    foreach(TECSubScope sub in equip.SubScope)
                    {
                        foreach(TECPoint point in sub.Points)
                        { totalPoints += point.Quantity; }
                    }
                }
            }
            return totalPoints;
        }
        
        public override object Copy()
        {
            TECBid bid = new TECBid(this);
            bid._guid = Guid;
            return bid;
        }
        
        private void checkForVisualsToRemove(TECScope item)
        {
            foreach(TECDrawing drawing in this.Drawings)
            {
                foreach(TECPage page in drawing.Pages)
                {
                    var vScopeToRemove = new List<TECVisualScope>();
                    var vConnectionsToRemove = new List<TECVisualConnection>();
                    foreach(TECVisualScope vScope in page.PageScope)
                    {
                        if(vScope.Scope == item)
                        {
                            vScopeToRemove.Add(vScope);
                            foreach(TECVisualConnection vConnection in page.Connections)
                            {
                                if((vConnection.Scope1 == vScope) || (vConnection.Scope2 == vScope))
                                {
                                    vConnectionsToRemove.Add(vConnection);
                                }
                            }
                        }
                    }
                    foreach(TECVisualScope vScope in vScopeToRemove)
                    {
                        page.PageScope.Remove(vScope);
                    }
                    foreach(TECVisualConnection vConnection in vConnectionsToRemove)
                    {
                        page.Connections.Remove(vConnection);
                    }
                }
            }
        }

        private void addProposalScope(TECSystem system)
        {
            this.ProposalScope.Add(new TECProposalScope(system));
        }
        private void removeProposalScope(TECSystem system)
        {
            List<TECProposalScope> scopeToRemove = new List<TECProposalScope>();
            foreach(TECProposalScope pScope in this.ProposalScope)
            {
                if(pScope.Scope == system)
                {
                    scopeToRemove.Add(pScope);
                }
            }
            foreach(TECProposalScope pScope in scopeToRemove)
            {
                this.ProposalScope.Remove(pScope);
            }
            
        }

        private void registerSystems()
        {
            foreach(TECSystem system in Systems)
            {
                system.PropertyChanged += System_PropertyChanged;
            }
        }

        private void updatePoints()
        {
            Labor.NumPoints = getPointNumber();
            RaisePropertyChanged("TECSubtotal");
            RaisePropertyChanged("SubcontractorSubtotal");
            updateTotal();
        }
        private void updateDevices()
        {
            RaisePropertyChanged("MaterialCost");
            RaisePropertyChanged("SubcontractorLaborCost");
            RaisePropertyChanged("Tax");
            RaisePropertyChanged("TECSubtotal");
            updateTotal();
        }
        private void updateElectricalMaterial()
        {
            RaisePropertyChanged("ElectricalMaterialCost");
            RaisePropertyChanged("SubcontractorSubtotal");
            RaisePropertyChanged("SubcontractorLaborCost");
            RaisePropertyChanged("PricePerPoint");
            RaisePropertyChanged("TotalPrice");
        }
        private void updateFromParameters()
        {
            RaisePropertyChanged("Tax");
            RaisePropertyChanged("TECSubtotal");
            RaisePropertyChanged("SubcontractorSubtotal");
            RaisePropertyChanged("SubcontractorLaborCost");
            updateTotal();
        }
        private void updateFromLabor()
        {
            RaisePropertyChanged("Tax");
            RaisePropertyChanged("TECSubtotal");
            RaisePropertyChanged("SubcontractorSubtotal");
            RaisePropertyChanged("SubcontractorLaborCost");
            RaisePropertyChanged("PricePerPoint");
            updateTotal();
        }
        private void updateAll()
        {
            RaisePropertyChanged("Tax");
            RaisePropertyChanged("TECSubtotal");
            RaisePropertyChanged("SubcontractorSubtotal");
            RaisePropertyChanged("SubcontractorLaborCost");
            RaisePropertyChanged("ElectricalMaterialCost");
        }

        private void updateTotal()
        {
            RaisePropertyChanged("TotalPrice");
            RaisePropertyChanged("PricePerPoint");
            RaisePropertyChanged("Margin");
        }

        private void checkForTotalsInSystem(TECSystem system)
        {
            foreach(TECEquipment equip in system.Equipment)
            {
                foreach(TECSubScope sub in equip.SubScope)
                {
                    updateElectricalMaterial();
                    if (sub.Points.Count > 0)
                    {
                        updatePoints();
                    }
                    if(sub.Devices.Count > 0)
                    {
                        updateDevices();
                    }
                }
            }
        }

        #endregion

    }
}
