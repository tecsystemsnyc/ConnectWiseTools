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
            }
        }

        public double MaterialCost
        {
            get { return getMaterialCost(); }
        }
        public double LaborCost
        {
            get { return getLaborCost(); }
        }

        public ObservableCollection<TECScopeBranch> ScopeTree { get; set; }
        public ObservableCollection<TECSystem> Systems { get; set; }
        public ObservableCollection<TECDevice> DeviceCatalog { get; set; }
        public ObservableCollection<TECManufacturer> ManufacturerCatalog { get; set; }

        public ObservableCollection<TECNote> Notes { get; set; }
        public ObservableCollection<TECExclusion> Exclusions { get; set; }

        public ObservableCollection<TECTag> Tags { get; set; }

        public ObservableCollection<TECDrawing> Drawings { get; set; }
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
            ScopeTree = scopeTree;
            Systems = systems;
            DeviceCatalog = deviceCatalog;
            ManufacturerCatalog = manufacturerCatalog;
            Notes = notes;
            Exclusions = exclusions;
            Tags = tags;
            _infoGuid = infoGuid;
            _labor = new TECLabor();
            Drawings = new ObservableCollection<TECDrawing>();
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
            this("", "", new DateTime(), "", "", new ObservableCollection<TECScopeBranch>(), new ObservableCollection<TECSystem>(), new ObservableCollection<TECDevice>(), new ObservableCollection<TECManufacturer>(), new ObservableCollection<TECNote>(), new ObservableCollection<TECExclusion>(), new ObservableCollection<TECTag>()) { }

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
        private double getLaborCost()
        {
            double cost = 0;
            foreach (TECSystem system in this.Systems)
            {
                cost += system.LaborCost;
            }
            return cost;
        }
        public override object Copy()
        {
            TECBid bid = new TECBid(this);
            bid._infoGuid = InfoGuid;
            return bid;
        }
        #endregion


    }
}
