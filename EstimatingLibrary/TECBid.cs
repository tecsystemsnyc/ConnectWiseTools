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
        private ObservableCollection<TECNote> _notes { get; set; }
        private ObservableCollection<TECExclusion> _exclusions { get; set; }
        private ObservableCollection<TECDrawing> _drawings { get; set; }
        private ObservableCollection<TECLocation> _locations { get; set; }
        private ObservableCollection<TECController> _controllers { get; set; }
        private ObservableCollection<TECProposalScope> _proposalScope { get; set; }
        private ObservableCollection<TECMiscCost> _miscCosts { get; set; }
        private ObservableCollection<TECMiscWiring> _miscWiring { get; set; }
        private ObservableCollection<TECPanel> _panels { get; set; }

        public string Name {
            get { return _name; }
            set
            {
                if(_name != value)
                {
                    var temp = Copy();
                    _name = value;
                    // Call RaisePropertyChanged whenever the property is updated
                    NotifyPropertyChanged("Name", temp, this);
                }
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

        public TECBidParameters Parameters
        {
            get { return _parameters; }
            set
            {
                var temp = Copy();
                Parameters.PropertyChanged -= objectPropertyChanged;
                _parameters = value;
                NotifyPropertyChanged("Parameters", temp, this);
                Parameters.PropertyChanged += objectPropertyChanged;
            }
        }

        #region Cost and Labor

        public double PMPointLaborHours
        {
            get { return EstimateCalculator.GetPMPointHours(this); }
        }
        public double PMLaborHours
        {
            get { return EstimateCalculator.GetPMTotalHours(this); }
        }
        public double PMLaborCost
        {
            get { return EstimateCalculator.GetPMLaborCost(this); }
        }

        public double ENGPointLaborHours
        {
            get { return EstimateCalculator.GetENGPointHours(this); }
        }
        public double ENGLaborHours
        {
            get { return EstimateCalculator.GetENGTotalHours(this); }
        }
        public double ENGLaborCost
        {
            get { return EstimateCalculator.GetENGLaborCost(this); }
        }

        public double SoftPointLaborHours
        {
            get { return EstimateCalculator.GetSoftPointHours(this); }
        }
        public double SoftLaborHours
        {
            get { return EstimateCalculator.GetSoftTotalHours(this); }
        }
        public double SoftLaborCost
        {
            get { return EstimateCalculator.GetSoftLaborCost(this); }
        }

        public double CommPointLaborHours
        {
            get { return EstimateCalculator.GetCommPointHours(this); }
        }
        public double CommLaborHours
        {
            get { return EstimateCalculator.GetCommTotalHours(this); }
        }
        public double CommLaborCost
        {
            get { return EstimateCalculator.GetCommLaborCost(this); }
        }

        public double GraphPointLaborHours
        {
            get { return EstimateCalculator.GetGraphPointHours(this); }
        }
        public double GraphLaborHours
        {
            get { return EstimateCalculator.GetGraphTotalHours(this); }
        }
        public double GraphLaborCost
        {
            get { return EstimateCalculator.GetGraphLaborCost(this); }
        }

        public double TECLaborHours
        {
            get { return EstimateCalculator.GetTECLaborHours(this); }
        }
        public double TECLaborCost
        {
            get { return EstimateCalculator.GetTECLaborCost(this); }
        }

        public double TotalLaborCost
        {
            get { return EstimateCalculator.GetTotalLaborCost(this); }
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
        
        public double ElectricalLaborHours
        {
            get { return EstimateCalculator.GetElectricalLaborHours(this); }
        }
        public double ElectricalLaborCost
        {
            get { return EstimateCalculator.GetElectricalLaborCost(this); }
        }
        public double ElectricalSuperLaborHours
        {
            get { return EstimateCalculator.GetElectricalSuperLaborHours(this); }
        }
        public double ElectricalSuperLaborCost
        {
            get { return EstimateCalculator.GetElectricalSuperLaborCost(this); }
        }

        public double SubcontractorLaborHours
        {
            get { return EstimateCalculator.GetSubcontractorLaborHours(this); }
        }
        public double SubcontractorLaborCost
        {
            get { return EstimateCalculator.GetSubcontractorLaborCost(this); }
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

        public double TotalCost
        {
            get { return EstimateCalculator.GetTotalCost(this); }
        }
        #endregion

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
                registerSystems();
                Systems.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("Systems", temp, this);
                updatePoints();
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
                registerControllers();
            }
        }
        public ObservableCollection<TECProposalScope> ProposalScope
        {
            get { return _proposalScope; }
            set { _proposalScope = value; }
        }
        public ObservableCollection<TECMiscCost> MiscCosts
        {
            get { return _miscCosts; }
            set
            {
                var temp = this.Copy();
                MiscCosts.CollectionChanged -= CollectionChanged;
                _miscCosts = value;
                MiscCosts.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("MiscCosts", temp, this);
            }
        }
        public ObservableCollection<TECMiscWiring> MiscWiring
        {
            get { return _miscWiring; }
            set
            {
                var temp = this.Copy();
                MiscWiring.CollectionChanged -= CollectionChanged;
                _miscWiring = value;
                MiscWiring.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("MiscWiring", temp, this);
            }
        }
        public ObservableCollection<TECPanel> Panels
        {
            get { return _panels; }
            set
            {
                var temp = this.Copy();
                Panels.CollectionChanged -= CollectionChanged;
                _panels = value;
                Panels.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("Panels", temp, this);
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
            _notes = new ObservableCollection<TECNote>();
            _exclusions = new ObservableCollection<TECExclusion>();
            _drawings = new ObservableCollection<TECDrawing>();
            _locations = new ObservableCollection<TECLocation>();
            _controllers = new ObservableCollection<TECController>();
            _proposalScope = new ObservableCollection<TECProposalScope>();
            _miscWiring = new ObservableCollection<TECMiscWiring>();
            _miscCosts = new ObservableCollection<TECMiscCost>();
            _panels = new ObservableCollection<TECPanel>();
            _labor = new TECLabor();
            _parameters = new TECBidParameters();
            Parameters.PropertyChanged += objectPropertyChanged;
            Labor.PropertyChanged += objectPropertyChanged;

            Systems.CollectionChanged += CollectionChanged;
            ScopeTree.CollectionChanged += CollectionChanged;
            Notes.CollectionChanged += CollectionChanged;
            Exclusions.CollectionChanged += CollectionChanged;
            Drawings.CollectionChanged += CollectionChanged;
            Locations.CollectionChanged += CollectionChanged;
            Controllers.CollectionChanged += CollectionChanged;
            ProposalScope.CollectionChanged += CollectionChanged;
            MiscCosts.CollectionChanged += CollectionChanged;
            MiscWiring.CollectionChanged += CollectionChanged;
            Panels.CollectionChanged += CollectionChanged;

            registerSystems();
            registerControllers();
        }

        public TECBid() : base()
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
            _parameters.Overhead = 20;
            _parameters.Profit = 20;
            _parameters.SubcontractorMarkup = 20;
        }

        //Copy Constructor
        public TECBid(TECBid bidSource) : base()
        {
            _name = bidSource.Name;
            _bidNumber = bidSource.BidNumber;
            _dueDate = bidSource.DueDate;
            _salesperson = bidSource.Salesperson;
            _estimator = bidSource.Estimator;

            _catalogs = bidSource.Catalogs.Copy() as TECCatalogs;
            _labor = new TECLabor(bidSource.Labor);
            _parameters = new TECBidParameters(bidSource.Parameters);
            Parameters.PropertyChanged += objectPropertyChanged;
            Labor.PropertyChanged += objectPropertyChanged;

            foreach (TECScopeBranch branch in bidSource.ScopeTree)
            { ScopeTree.Add(new TECScopeBranch(branch)); }
            foreach (TECSystem system in bidSource.Systems)
            { Systems.Add(new TECSystem(system)); }
            foreach (TECNote note in bidSource.Notes)
            { Notes.Add(new TECNote(note)); }
            foreach (TECExclusion exclusion in bidSource.Exclusions)
            { Exclusions.Add(new TECExclusion(exclusion)); }
            foreach (TECLocation location in bidSource.Locations)
            { Locations.Add(new TECLocation(location)); }
            foreach (TECDrawing drawing in bidSource.Drawings)
            { Drawings.Add(new TECDrawing(drawing)); }
            foreach(TECController controller in bidSource.Controllers)
            { Controllers.Add(new TECController(controller)); }
            foreach(TECProposalScope propScope in bidSource.ProposalScope)
            { ProposalScope.Add(new TECProposalScope(propScope)); }
            foreach(TECMiscCost cost in bidSource.MiscCosts)
            { MiscCosts.Add(new TECMiscCost(cost)); }
            foreach (TECMiscWiring wiring in bidSource.MiscWiring)
            { MiscWiring.Add(new TECMiscWiring(wiring)); }
            foreach (TECPanel panel in bidSource.Panels)
            { Panels.Add(new TECPanel(panel)); }
        }

        #endregion //Constructors

        #region Methods

        public void addControlledScope(TECControlledScope controlledScope)
        {
            //var watch = System.Diagnostics.Stopwatch.StartNew();
            Dictionary<Guid, Guid> guidDictionary = new Dictionary<Guid, Guid>();
            var systemCollection = new ObservableCollection<TECSystem>();
            var controllerCollection = new ObservableCollection<TECController>();
            var panelCollection = new ObservableCollection<TECPanel>();
            foreach (TECSystem system in controlledScope.Systems)
            {
                systemCollection.Add(new TECSystem(system, guidDictionary));
            }
            foreach (TECController controller in controlledScope.Controllers)
            {
                controllerCollection.Add(new TECController(controller, guidDictionary));
            }
            foreach (TECPanel panel in controlledScope.Panels)
            {
                panelCollection.Add(new TECPanel(panel, guidDictionary));
            }
            //watch.Stop();
            //Console.WriteLine("Assemble Copy Data: " + watch.ElapsedMilliseconds);

            //watch = System.Diagnostics.Stopwatch.StartNew();
            ModelLinkingHelper.LinkControlledScopeObjects(systemCollection, controllerCollection,
              panelCollection, this, guidDictionary);
            //watch.Stop();
            //Console.WriteLine("Link Data: " + watch.ElapsedMilliseconds);

            //watch = System.Diagnostics.Stopwatch.StartNew();
            foreach (TECController controller in controllerCollection)
            {
                Controllers.Add(controller);
            }
            //watch.Stop();
            //Console.WriteLine("Add Controllers: " + watch.ElapsedMilliseconds);

            //watch = System.Diagnostics.Stopwatch.StartNew();
            foreach (TECPanel panel in panelCollection)
            {
                Panels.Add(panel);
            }
            //watch.Stop();
            //Console.WriteLine("Add Panels: " + watch.ElapsedMilliseconds);

            //watch = System.Diagnostics.Stopwatch.StartNew();
            foreach (TECSystem system in systemCollection)
            {
                Systems.Add(system);
            }
            //watch.Stop();
            //Console.WriteLine("Add Systems: " + watch.ElapsedMilliseconds);
        }

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
                        if (item is TECCost)
                        { 
                            updateElectricalMaterial();
                            (item as TECObject).PropertyChanged += objectPropertyChanged;
                        }
                        else if (item is TECSystem)
                        {
                            var sys = item as TECSystem;
                            addProposalScope(sys);
                            sys.PropertyChanged += System_PropertyChanged;
                            checkForTotalsInSystem(sys);
                        }
                        else if (item is TECController)
                        {
                            registerController(item as TECController);
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
                        NotifyPropertyChanged("Remove", this, item);
                        if (item is TECScope)
                        {
                            checkForVisualsToRemove((TECScope)item);
                        }

                        if (item is TECCost)
                        {
                            updateElectricalMaterial();
                            (item as TECCost).PropertyChanged -= objectPropertyChanged;
                        }
                        else if (item is TECSystem)
                        {
                            var sys = item as TECSystem;
                            sys.PropertyChanged -= System_PropertyChanged;
                            removeProposalScope(sys);
                            handleSystemSubScopeRemoval(item as TECSystem);
                        }
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                NotifyPropertyChanged("Edit", this, sender, typeof(TECBid), typeof(TECSystem));
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
            else if(e.PropertyName == "RemovedSubScope")
            {
                var args = e as PropertyChangedExtendedEventArgs<object>;
                if(args.NewValue is TECEquipment)
                {
                    handleEquipmentSubScopeRemoval(args.NewValue as TECEquipment);
                } else
                {
                    handleSubScopeRemovalInConnections(args.NewValue as TECSubScope);
                }
            }
        }
        private void objectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //NotifyPropertyChanged("ChildChanged", this, sender);
            if (sender is TECLabor)
            {
                updateFromLabor();
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
                    NotifyPropertyChanged("Edit", (object)this, (object)Labor);
                }
            }
            else if (sender is TECBidParameters)
            { updateFromParameters(); }
            else if (sender is TECCost)
            { updateElectricalMaterial(); }
            else if (sender is TECConnection)
            {
                if(e.PropertyName == "Length" ||
                    e.PropertyName == "ConduitType" ||
                    e.PropertyName == "ConnectionType" ||
                    e.PropertyName == "IncludeStubUp")
                {
                    updateElectricalMaterial();
                }
            }
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
            TECBid bid = new TECBid(Guid);
            
            bid._name = this.Name;
            bid._bidNumber = this.BidNumber;
            bid._dueDate = this.DueDate;
            bid._salesperson = this.Salesperson;
            bid._estimator = this.Estimator;

            bid._labor = this.Labor.Copy() as TECLabor;
            bid._parameters = this.Parameters.Copy() as TECBidParameters;
            bid.Parameters.PropertyChanged += bid.objectPropertyChanged;
            bid.Labor.PropertyChanged += bid.objectPropertyChanged;

            foreach (TECScopeBranch branch in this.ScopeTree)
            { bid.ScopeTree.Add(branch.Copy() as TECScopeBranch); }
            foreach (TECSystem system in this.Systems)
            { bid.Systems.Add(system.Copy() as TECSystem); }
            foreach (TECNote note in this.Notes)
            { bid.Notes.Add(note.Copy() as TECNote); }
            foreach (TECExclusion exclusion in this.Exclusions)
            { bid.Exclusions.Add(exclusion.Copy() as TECExclusion); }
            foreach (TECLocation location in this.Locations)
            { bid.Locations.Add(location.Copy() as TECLocation); }
            foreach (TECDrawing drawing in this.Drawings)
            { bid.Drawings.Add(drawing.Copy() as TECDrawing); }
            foreach (TECController controller in this.Controllers)
            { bid.Controllers.Add(controller.Copy() as TECController); }
            foreach (TECProposalScope propScope in this.ProposalScope)
            { bid.ProposalScope.Add(propScope.Copy() as TECProposalScope); }
            foreach (TECMiscCost cost in this.MiscCosts)
            { bid.MiscCosts.Add(cost.Copy() as TECMiscCost); }
            foreach (TECMiscWiring wiring in this.MiscWiring)
            { bid.MiscWiring.Add(wiring.Copy() as TECMiscWiring); }
            foreach (TECPanel panel in this.Panels)
            { bid.Panels.Add(panel.Copy() as TECPanel); }
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
            RaisePropertyChanged("TotalPointNumber");
            updateTECLabor();
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
            updateElectricalLabor();
            updateTotal();
        }
        private void updateElectricalLabor()
        {
            RaisePropertyChanged("ElectricalLaborHours");
            RaisePropertyChanged("ElectricalLaborCost");
            RaisePropertyChanged("ElectricalSuperLaborHours");
            RaisePropertyChanged("ElectricalSuperLaborCost");
            RaisePropertyChanged("SubcontractorLaborHours");
            RaisePropertyChanged("SubcontractorLaborCost");
        }
        private void updateTECLabor()
        {
            RaisePropertyChanged("PMPointLaborHours");
            RaisePropertyChanged("PMLaborHours");
            RaisePropertyChanged("PMLaborCost");

            RaisePropertyChanged("ENGPointLaborHours");
            RaisePropertyChanged("ENGLaborHours");
            RaisePropertyChanged("ENGLaborCost");

            RaisePropertyChanged("SoftPointLaborHours");
            RaisePropertyChanged("SoftLaborHours");
            RaisePropertyChanged("SoftLaborCost");

            RaisePropertyChanged("CommPointLaborHours");
            RaisePropertyChanged("CommLaborHours");
            RaisePropertyChanged("CommLaborCost");

            RaisePropertyChanged("GraphPointLaborHours");
            RaisePropertyChanged("GraphLaborHours");
            RaisePropertyChanged("GraphLaborCost");

            RaisePropertyChanged("TECLaborHours");
            RaisePropertyChanged("TECLaborCost");
            RaisePropertyChanged("TECSubtotal");
            RaisePropertyChanged("TotalLaborCost");
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
            updateTECLabor();
            updateElectricalLabor();
            RaisePropertyChanged("SubcontractorSubtotal");
            RaisePropertyChanged("SubcontractorLaborCost");
            updateTotal();
        }
        private void updateAll()
        {
            RaisePropertyChanged("Tax");
            updateTECLabor();
            RaisePropertyChanged("SubcontractorSubtotal");
            RaisePropertyChanged("SubcontractorLaborCost");
            RaisePropertyChanged("ElectricalMaterialCost");
            updateTotal();
        }
        private void updateTotal()
        {
            RaisePropertyChanged("TotalPrice");
            RaisePropertyChanged("TotalCost");
            RaisePropertyChanged("PricePerPoint");
            RaisePropertyChanged("Margin");
        }

        private void checkForTotalsInSystem(TECSystem system)
        {
            bool subChanged = false;
            bool pointsChanged = false;
            bool devicesChanged = false;

            foreach(TECEquipment equip in system.Equipment)
            {
                foreach(TECSubScope sub in equip.SubScope)
                {
                    subChanged = true;
                    if (sub.Points.Count > 0)
                    {
                        pointsChanged = true;
                    }
                    if(sub.Devices.Count > 0)
                    {
                        devicesChanged = true;
                    }
                }
            }

            if (subChanged)
            {
                updateElectricalMaterial();
            }
            if (pointsChanged)
            {
                updatePoints();
            }
            if (devicesChanged)
            {
                updateDevices();
            }

        }

        private void handleSystemSubScopeRemoval(TECSystem system)
        {
            foreach(TECEquipment equipment in system.Equipment)
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
        
        private void registerControllers()
        {
            foreach(TECController controller in Controllers)
            {
                registerController(controller);
            }
        }
        private void registerController(TECController controller)
        {
            controller.ChildrenConnections.CollectionChanged += ChildrenConnections_CollectionChanged;
            foreach (TECConnection connection in controller.ChildrenConnections)
            {
                connection.PropertyChanged += objectPropertyChanged;
            }
            if(controller.ChildrenConnections.Count > 0)
            {
                updateElectricalMaterial();
            }
        }
        
        private void ChildrenConnections_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach(TECConnection item in e.NewItems)
                {
                    item.PropertyChanged += objectPropertyChanged;
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (TECConnection item in e.OldItems)
                {
                    item.PropertyChanged -= objectPropertyChanged;
                }
            }
            updateElectricalMaterial();
        }
        #endregion

    }
}
