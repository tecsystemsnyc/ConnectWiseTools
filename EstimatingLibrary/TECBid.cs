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
        private Guid _infoGuid;
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
        public Guid InfoGuid
        {
            get { return _infoGuid; }
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
            }
        }

        public TECBidParameters Parameters
        {
            get
            {
                return _parameters;
            }
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
            get { return getMaterialCost(); }
        }

        public double TECSubtotal
        {
            get
            {
                return getTECSubtotal();
            }
        }
        public double SubcontractorSubtotal
        {
            get
            {
                return getSubcontractorSubtotal();
            }
        }

        public double TotalPrice
        {
            get
            {
                return getTotalPrice();
            }
        }

        public double BudgetPrice
        {
            get { return getBudgetPrice(); }
        }
        public int TotalPointNumber
        {
            get
            {
                return getPointNumber();
            }
        }

        public double ElectricalMaterialCost
        {
            get
            {
                return getElectricalMaterialCost();
            }
        }
        public double Tax
        {
            get
            {
                return getTax();
            }
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

        #endregion //Properties

        #region Constructors
        public TECBid(
            string name,
            string bidNumber,
            DateTime dueDate,
            string salesperson,
            string estimator,
            ObservableCollection<TECScopeBranch> scopeTree,
            ObservableCollection<TECSystem> systems, 
            ObservableCollection<TECDevice> deviceCatalog,
            ObservableCollection<TECManufacturer> manufacturerCatalog,
            ObservableCollection<TECNote> notes, 
            ObservableCollection<TECExclusion> exclusions,
            ObservableCollection<TECTag> tags, 
            Guid infoGuid)
        {
            _name = name;
            _bidNumber = bidNumber;
            _dueDate = dueDate;
            _salesperson = salesperson;
            _estimator = estimator;
            _scopeTree = scopeTree;
            _systems = systems;
            _deviceCatalog = deviceCatalog;
            _manufacturerCatalog = manufacturerCatalog;
            _notes = notes;
            _exclusions = exclusions;
            _tags = tags;
            _infoGuid = infoGuid;
            _labor = new TECLabor();
            _drawings = new ObservableCollection<TECDrawing>();
            _locations = new ObservableCollection<TECLocation>();
            _controllers = new ObservableCollection<TECController>();
            _connections = new ObservableCollection<TECConnection>();
            _proposalScope = new ObservableCollection<TECProposalScope>();
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
        }
        public TECBid(
            string name, 
            string bidNumber,
            DateTime dueDate,
            string salesperson,
            string estimator,
            ObservableCollection<TECScopeBranch> scopeTree,
            ObservableCollection<TECSystem> systems,
            ObservableCollection<TECDevice> deviceCatalog,
            ObservableCollection<TECManufacturer> manufacturerCatalog,
            ObservableCollection<TECNote> notes,
            ObservableCollection<TECExclusion> exclusions,
            ObservableCollection<TECTag> tags)
            : this(name, bidNumber, dueDate, salesperson, estimator, scopeTree, systems, deviceCatalog, manufacturerCatalog, notes, exclusions, tags, Guid.NewGuid()) { }
        public TECBid() : 
            this("", "", new DateTime(), "", "", new ObservableCollection<TECScopeBranch>(), new ObservableCollection<TECSystem>(), new ObservableCollection<TECDevice>(), new ObservableCollection<TECManufacturer>(), new ObservableCollection<TECNote>(), new ObservableCollection<TECExclusion>(), new ObservableCollection<TECTag>())
        {
            foreach(string item in Defaults.Scope)
            { ScopeTree.Add(new TECScopeBranch(item, "", new ObservableCollection<TECScopeBranch>())); }
            foreach (string item in Defaults.Exclusions)
            { Exclusions.Add(new TECExclusion(item)); }
            foreach (string item in Defaults.Notes)
            { Notes.Add(new TECNote(item)); }

            
        }

        //Copy Constructor
        public TECBid(TECBid bidSource) : this(bidSource.Name, bidSource.BidNumber, bidSource.DueDate, bidSource.Salesperson, bidSource.Estimator, new ObservableCollection<TECScopeBranch>(), new ObservableCollection<TECSystem>(), bidSource.DeviceCatalog, bidSource.ManufacturerCatalog, new ObservableCollection<TECNote>(), new ObservableCollection<TECExclusion>(), bidSource.Tags)
        {
            _labor = new TECLabor(bidSource.Labor);
            foreach (TECScopeBranch branch in bidSource.ScopeTree)
            {
                ScopeTree.Add(new TECScopeBranch(branch));
            }
            foreach (TECSystem system in bidSource.Systems)
            {
                Systems.Add(new TECSystem(system));
            }
            foreach (TECNote note in bidSource.Notes)
            {
                Notes.Add(new TECNote(note));
            }
            foreach (TECExclusion exclusion in bidSource.Exclusions)
            {
                Exclusions.Add(new TECExclusion(exclusion));
            }
        }

        #endregion //Constructors

        #region Methods
        private double getMaterialCost()
        {
            double cost = 0;
            foreach(TECSystem system in this.Systems)
            {
                cost += system.MaterialCost;
            }
            return cost;
        }

        private double getTECCost()
        {
            double outCost = 0;
            outCost += Labor.TECSubTotal;
            outCost += MaterialCost;
            outCost *= Parameters.Escalation;
            outCost *= Parameters.Overhead;
            outCost += Tax;

            return outCost;
        }
        private double getTECSubtotal()
        {
            double outCost = 0;
            outCost += getTECCost();

            outCost *= Parameters.Profit;

            return outCost;
        }

        private double getSubcontractorCost()
        {
            double outCost = 0;
            outCost += Labor.SubcontractorSubTotal;
            outCost += ElectricalMaterialCost;
            outCost *= Parameters.SubcontractorEscalation;

            return outCost;
        }

        private double getSubcontractorSubtotal()
        {
            double outCost = 0;
            outCost += getSubcontractorCost();
            outCost *= Parameters.SubcontractorMarkup;

            return outCost;
        }

        private double getTax()
        {
            double outTax = 0;

            if (!Parameters.IsTaxExempt)
            {
                outTax += 1.0875 * MaterialCost;
            }

            return outTax;
        }

        private double getTotalPrice()
        {
            double outPrice = 0;

            outPrice += TECSubtotal;
            outPrice += SubcontractorSubtotal;

            return outPrice;
        }

        
        private double getBudgetPrice()
        {
            double price = 0;
            foreach (TECSystem system in this.Systems)
            {
                if (system.TotalBudgetPrice >= 0)
                {
                    price += system.TotalBudgetPrice;
                }
            }
            return price;
        }

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
                        {
                            totalPoints += point.Quantity;
                        }
                    }
                }
            }

            return totalPoints;
        }

        //ONLY RETURNS TOTAL LENGTH AT THE MOMENT
        private double getElectricalMaterialCost()
        {
            
            double cost = 0;

            foreach(TECConnection conn in Connections)
            {
                cost += conn.Length;
            }

            return cost;

        }


        public override object Copy()
        {
            TECBid bid = new TECBid(this);
            bid._infoGuid = InfoGuid;
            return bid;
        }

        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    if(item is TECProposalScope)
                    {
                        NotifyPropertyChanged("MetaAdd", this, item);
                    }
                    else
                    {
                        NotifyPropertyChanged("Add", this, item);
                        if (item is TECSystem)
                        {
                            addProposalScope(item as TECSystem);
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
                        if (item is TECSystem)
                        {
                            removeProposalScope(item as TECSystem);
                        }
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                NotifyPropertyChanged("Edit", this, sender);
            }
        }
        
        private void objectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("ChildChanged", (object)this, (object)Labor);
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
        #endregion

    }
}
