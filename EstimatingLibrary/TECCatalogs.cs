using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECCatalogs : TECObject
    {
        private ObservableCollection<TECConnectionType> _connectionTypes;
        private ObservableCollection<TECConduitType> _conduitTypes;
        private ObservableCollection<TECAssociatedCost> _associatedCosts;
        private ObservableCollection<TECPanelType> _panelTypes;
        private ObservableCollection<TECIOModule> _ioModules;
        private ObservableCollection<TECDevice> _devices;
        private ObservableCollection<TECManufacturer> _manufacturers;
        private ObservableCollection<TECTag> _tags;

        public ObservableCollection<TECIOModule> IOModules
        {
            get { return _ioModules; }
            set
            {
                var temp = Copy();
                IOModules.CollectionChanged -= CollectionChanged;
                _ioModules = value;
                IOModules.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("IOModules", temp, this);
            }
        }
        public ObservableCollection<TECDevice> Devices
        {
            get { return _devices; }
            set
            {
                var temp = this.Copy();
                Devices.CollectionChanged -= CollectionChanged;
                _devices = value;
                Devices.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("Devices", temp, this);
            }
        }
        public ObservableCollection<TECManufacturer> Manufacturers
        {
            get { return _manufacturers; }
            set
            {
                var temp = this.Copy();
                Manufacturers.CollectionChanged -= CollectionChanged;
                _manufacturers = value;
                Manufacturers.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("Manufacturers", temp, this);
            }
        }
        public ObservableCollection<TECPanelType> PanelTypes
        {
            get { return _panelTypes; }
            set
            {
                var temp = this.Copy();
                PanelTypes.CollectionChanged -= CollectionChanged;
                _panelTypes = value;
                PanelTypes.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("PanelTypes", temp, this);
            }
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
        public ObservableCollection<TECAssociatedCost> AssociatedCosts
        {
            get { return _associatedCosts; }
            set
            {
                var temp = this.Copy();
                AssociatedCosts.CollectionChanged -= CollectionChanged;
                _associatedCosts = value;
                AssociatedCosts.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("AssociatedCosts", temp, this);
            }
        }
        public ObservableCollection<TECTag> Tags
        {
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
        
        public TECCatalogs()
        {
            instantiateCOllections();
        }

        private void instantiateCOllections()
        {
            _conduitTypes = new ObservableCollection<TECConduitType>();
            _connectionTypes = new ObservableCollection<TECConnectionType>();
            _associatedCosts = new ObservableCollection<TECAssociatedCost>();
            _panelTypes = new ObservableCollection<TECPanelType>();
            _ioModules = new ObservableCollection<TECIOModule>();
            _devices = new ObservableCollection<TECDevice>();
            _manufacturers = new ObservableCollection<TECManufacturer>();
            _tags = new ObservableCollection<TECTag>();

            registerInitialCollectionChanges();
        }
        private void registerInitialCollectionChanges()
        {
            ConduitTypes.CollectionChanged += CollectionChanged;
            ConnectionTypes.CollectionChanged += CollectionChanged;
            AssociatedCosts.CollectionChanged += CollectionChanged;
            PanelTypes.CollectionChanged += CollectionChanged;
            IOModules.CollectionChanged += CollectionChanged;
            Devices.CollectionChanged += CollectionChanged;
            Manufacturers.CollectionChanged += CollectionChanged;
            Tags.CollectionChanged += CollectionChanged;
        }

        public override object Copy()
        {
            TECCatalogs catalogs = new TECCatalogs();
            foreach (TECAssociatedCost cost in this.AssociatedCosts)
            { catalogs.AssociatedCosts.Add(cost.Copy() as TECAssociatedCost); }
            foreach (TECConduitType conduitType in this.ConduitTypes)
            { catalogs.ConduitTypes.Add(conduitType.Copy() as TECConduitType); }
            foreach (TECConnectionType connectionType in this.ConnectionTypes)
            { catalogs.ConnectionTypes.Add(connectionType.Copy() as TECConnectionType); }
            foreach (TECTag tag in this.Tags)
            { catalogs.Tags.Add(tag.Copy() as TECTag); }
            foreach (TECManufacturer manufacturer in this.Manufacturers)
            { catalogs.Manufacturers.Add(manufacturer.Copy() as TECManufacturer); }
            foreach (TECDevice device in this.Devices)
            { catalogs.Devices.Add(device.Copy() as TECDevice); }
            foreach (TECPanelType panelType in this.PanelTypes)
            { catalogs.PanelTypes.Add(panelType.Copy() as TECPanelType); }
            foreach (TECIOModule module in IOModules)
            { catalogs.IOModules.Add(module.Copy() as TECIOModule); }
            return catalogs;
        }

        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    NotifyPropertyChanged("Add", this, item);
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
                //Change order
            }
        }

    }
}
