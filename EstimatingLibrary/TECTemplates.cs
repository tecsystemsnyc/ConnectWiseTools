using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECTemplates : TECObject
    {
        #region Properties
        private Guid _guid;
        private TECLabor _labor;
        private ObservableCollection<TECSystem> _systemTemplates;
        private ObservableCollection<TECEquipment> _equipmentTemplates;
        private ObservableCollection<TECSubScope> _subScopeTemplates;
        private ObservableCollection<TECDevice> _deviceCatalog;
        private ObservableCollection<TECManufacturer> _manufacturerCatalog;
        private ObservableCollection<TECTag> _tags;
        private ObservableCollection<TECController> _controllerTemplates;
        private ObservableCollection<TECConnectionType> _connectionTypeCatalog;
        private ObservableCollection<TECConduitType> _conduitTypeCatalog;
        private ObservableCollection<TECAssociatedCost> _associatedCostsCatalog;
        private ObservableCollection<TECControlledScope> _controlledScopeTemplates;
        private ObservableCollection<TECMiscCost> _miscCostTemplates;
        private ObservableCollection<TECMiscWiring> _miscWiringTemplates;
        private ObservableCollection<TECPanelType> _panelTypeCatalog;
        private ObservableCollection<TECPanel> _panelTemplates;
        private ObservableCollection<TECIOModule> _ioModuleCatalog;

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
            }
        }
        public ObservableCollection<TECSystem> SystemTemplates
        {
            get { return _systemTemplates; }
            set
            {
                var temp = this.Copy();
                SystemTemplates.CollectionChanged -= CollectionChanged;
                _systemTemplates = value;
                SystemTemplates.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("SystemTemplates", temp, this);
            }
        }
        public ObservableCollection<TECEquipment> EquipmentTemplates
        {
            get { return _equipmentTemplates; }
            set
            {
                var temp = this.Copy();
                EquipmentTemplates.CollectionChanged -= CollectionChanged;
                _equipmentTemplates = value;
                EquipmentTemplates.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("EquipmentTemplates", temp, this);
            }
        }
        public ObservableCollection<TECSubScope> SubScopeTemplates
        {
            get { return _subScopeTemplates; }
            set
            {
                var temp = this.Copy();
                SubScopeTemplates.CollectionChanged -= CollectionChanged;
                _subScopeTemplates = value;
                SubScopeTemplates.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("SubScopeTemplates", temp, this);
            }
        }
        public ObservableCollection<TECDevice> DeviceCatalog
        {
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
        public ObservableCollection<TECManufacturer> ManufacturerCatalog
        {
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
        public ObservableCollection<TECController> ControllerTemplates
        {
            get { return _controllerTemplates; }
            set
            {
                var temp = this.Copy();
                ControllerTemplates.CollectionChanged -= CollectionChanged;
                _controllerTemplates = value;
                ControllerTemplates.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("ControllerTemplates", temp, this);
            }
        }
        public ObservableCollection<TECConnectionType> ConnectionTypeCatalog
        {
            get { return _connectionTypeCatalog; }
            set
            {
                var temp = this.Copy();
                ConnectionTypeCatalog.CollectionChanged -= CollectionChanged;
                _connectionTypeCatalog = value;
                ConnectionTypeCatalog.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("ConnectionTypes", temp, this);
            }
        }
        public ObservableCollection<TECConduitType> ConduitTypeCatalog
        {
            get { return _conduitTypeCatalog; }
            set
            {
                var temp = this.Copy();
                ConduitTypeCatalog.CollectionChanged -= CollectionChanged;
                _conduitTypeCatalog = value;
                ConduitTypeCatalog.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("ConduitTypeCatalog", temp, this);
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
        public ObservableCollection<TECControlledScope> ControlledScopeTemplates
        {
            get { return _controlledScopeTemplates; }
            set
            {
                var temp = this.Copy();
                ControlledScopeTemplates.CollectionChanged -= CollectionChanged;
                _controlledScopeTemplates = value;
                ControlledScopeTemplates.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("ControlledScopeTemplates", temp, this);
            }
        }
        public ObservableCollection<TECMiscCost> MiscCostTemplates
        {
            get { return _miscCostTemplates; }
            set
            {
                var temp = this.Copy();
                MiscCostTemplates.CollectionChanged -= CollectionChanged;
                _miscCostTemplates = value;
                MiscCostTemplates.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("MiscCostTemplates", temp, this);
            }
        }
        public ObservableCollection<TECMiscWiring> MiscWiringTemplates
        {
            get { return _miscWiringTemplates; }
            set
            {
                var temp = this.Copy();
                MiscWiringTemplates.CollectionChanged -= CollectionChanged;
                _miscWiringTemplates = value;
                MiscWiringTemplates.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("MiscWiringTemplates", temp, this);
            }
        }
        public ObservableCollection<TECPanelType> PanelTypeCatalog
        {
            get { return _panelTypeCatalog; }
            set
            {
                var temp = this.Copy();
                PanelTypeCatalog.CollectionChanged -= CollectionChanged;
                _panelTypeCatalog = value;
                PanelTypeCatalog.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("PanelTypeCatalog", temp, this);
            }
        }
        public ObservableCollection<TECPanel> PanelTemplates
        {
            get { return _panelTemplates; }
            set
            {
                var temp = this.Copy();
                PanelTemplates.CollectionChanged -= CollectionChanged;
                _panelTemplates = value;
                PanelTemplates.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("PanelTemplates", temp, this);
            }
        }
        public ObservableCollection<TECIOModule> IOModuleCatalog
        {
            get { return _ioModuleCatalog; }
            set
            {
                var temp = Copy();
                _ioModuleCatalog = value;
                NotifyPropertyChanged("IOModuleCatalog", temp, this);
            }
        }


        #endregion //Properties

        #region Constructors

        public TECTemplates() : this(Guid.NewGuid()) {}

        public TECTemplates(Guid guid)
        {
            _guid = guid;

            _labor = new TECLabor();

            _systemTemplates = new ObservableCollection<TECSystem>();
            _equipmentTemplates = new ObservableCollection<TECEquipment>();
            _subScopeTemplates = new ObservableCollection<TECSubScope>();
            _deviceCatalog = new ObservableCollection<TECDevice>();
            _tags = new ObservableCollection<TECTag>();
            _manufacturerCatalog = new ObservableCollection<TECManufacturer>();
            _controllerTemplates = new ObservableCollection<TECController>();
            _connectionTypeCatalog = new ObservableCollection<TECConnectionType>();
            _conduitTypeCatalog = new ObservableCollection<TECConduitType>();
            _associatedCostsCatalog = new ObservableCollection<TECAssociatedCost>();
            _miscWiringTemplates = new ObservableCollection<TECMiscWiring>();
            _miscCostTemplates = new ObservableCollection<TECMiscCost>();
            _panelTypeCatalog = new ObservableCollection<TECPanelType>();
            _controlledScopeTemplates = new ObservableCollection<TECControlledScope>();
            _panelTemplates = new ObservableCollection<TECPanel>();

            SystemTemplates.CollectionChanged += CollectionChanged;
            EquipmentTemplates.CollectionChanged += CollectionChanged;
            SubScopeTemplates.CollectionChanged += CollectionChanged;
            DeviceCatalog.CollectionChanged += CollectionChanged;
            Tags.CollectionChanged += CollectionChanged;
            ManufacturerCatalog.CollectionChanged += CollectionChanged;
            ControllerTemplates.CollectionChanged += CollectionChanged;
            ConnectionTypeCatalog.CollectionChanged += CollectionChanged;
            ConduitTypeCatalog.CollectionChanged += CollectionChanged;
            AssociatedCostsCatalog.CollectionChanged += CollectionChanged;
            MiscWiringTemplates.CollectionChanged += CollectionChanged;
            MiscCostTemplates.CollectionChanged += CollectionChanged;
            PanelTemplates.CollectionChanged += CollectionChanged;
            PanelTypeCatalog.CollectionChanged += CollectionChanged;
            ControlledScopeTemplates.CollectionChanged += CollectionChanged;

            Labor.PropertyChanged += objectPropertyChanged;
        }

        public TECTemplates(TECTemplates templatesSource) : this(templatesSource.Guid)
        {
            if (_labor != null)
            { _labor = templatesSource.Labor;
              Labor.PropertyChanged += objectPropertyChanged;
            }
            foreach (TECSystem system in templatesSource.SystemTemplates)
            { SystemTemplates.Add(new TECSystem(system)); }
            foreach (TECEquipment equip in templatesSource.EquipmentTemplates)
            { EquipmentTemplates.Add(new TECEquipment(equip)); }
            foreach (TECSubScope subScope in templatesSource.SubScopeTemplates)
            { SubScopeTemplates.Add(new TECSubScope(subScope)); }
            foreach (TECDevice device in templatesSource.DeviceCatalog)
            { DeviceCatalog.Add(new TECDevice(device)); }
            foreach (TECTag tag in templatesSource.Tags)
            { Tags.Add(new TECTag(tag)); }
            foreach (TECManufacturer man in templatesSource.ManufacturerCatalog)
            { ManufacturerCatalog.Add(new TECManufacturer(man)); }
            foreach(TECConnectionType connectionType in templatesSource.ConnectionTypeCatalog)
            { ConnectionTypeCatalog.Add(new TECConnectionType(connectionType)); }
            foreach (TECConduitType conduitType in templatesSource.ConduitTypeCatalog)
            { ConduitTypeCatalog.Add(new TECConduitType(conduitType)); }
            foreach(TECAssociatedCost cost in templatesSource.AssociatedCostsCatalog)
            { AssociatedCostsCatalog.Add(new TECAssociatedCost(cost)); }
            foreach(TECController controller in templatesSource.ControllerTemplates)
            { ControllerTemplates.Add(new TECController(controller)); }
            foreach(TECMiscCost cost in templatesSource.MiscCostTemplates)
            {
                MiscCostTemplates.Add(new TECMiscCost(cost));
            }
            foreach(TECMiscWiring wiring in templatesSource.MiscWiringTemplates)
            {
                MiscWiringTemplates.Add(new TECMiscWiring(wiring));
            }
            foreach(TECPanel panel in templatesSource.PanelTemplates)
            {
                PanelTemplates.Add(new TECPanel(panel));
            }
            foreach (TECPanelType panelType in templatesSource.PanelTypeCatalog)
            {
                PanelTypeCatalog.Add(new TECPanelType(panelType));
            }
            foreach(TECControlledScope scope in templatesSource.ControlledScopeTemplates)
            {
                ControlledScopeTemplates.Add(new TECControlledScope(scope));
            }
            foreach(TECIOModule module in templatesSource.IOModuleCatalog)
            {
                //IOModuleCatalog.Add(new TECIOModule(module));
            }
        }

        #endregion //Constructors

        #region Collection Changed
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
                NotifyPropertyChanged("Edit", this, sender);
            }
        }

        private void objectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("ChildChanged", this, sender);
        }
        #endregion

        public override object Copy()
        {
            var outTemplate = new TECTemplates();
            outTemplate._guid = _guid;

            if (_labor != null)
            {
                outTemplate._labor = this.Labor;
                outTemplate.Labor.PropertyChanged += outTemplate.objectPropertyChanged;
            }
            foreach (TECSystem system in this.SystemTemplates)
            { outTemplate.SystemTemplates.Add(system.Copy() as TECSystem); }
            foreach (TECEquipment equip in this.EquipmentTemplates)
            { outTemplate.EquipmentTemplates.Add(equip.Copy() as TECEquipment); }
            foreach (TECSubScope subScope in this.SubScopeTemplates)
            { outTemplate.SubScopeTemplates.Add(subScope.Copy() as TECSubScope); }
            foreach (TECDevice device in this.DeviceCatalog)
            { outTemplate.DeviceCatalog.Add(device.Copy() as TECDevice); }
            foreach (TECTag tag in this.Tags)
            { outTemplate.Tags.Add(tag.Copy() as TECTag); }
            foreach (TECManufacturer man in this.ManufacturerCatalog)
            { outTemplate.ManufacturerCatalog.Add(man.Copy() as TECManufacturer); }
            foreach (TECConnectionType connectionType in this.ConnectionTypeCatalog)
            { outTemplate.ConnectionTypeCatalog.Add(connectionType.Copy() as TECConnectionType); }
            foreach (TECConduitType conduitType in this.ConduitTypeCatalog)
            { outTemplate.ConduitTypeCatalog.Add(conduitType.Copy() as TECConduitType); }
            foreach (TECAssociatedCost cost in this.AssociatedCostsCatalog)
            { outTemplate.AssociatedCostsCatalog.Add(cost.Copy() as TECAssociatedCost); }
            foreach (TECController controller in this.ControllerTemplates)
            { outTemplate.ControllerTemplates.Add(controller.Copy() as TECController); }
            foreach (TECMiscCost cost in this.MiscCostTemplates)
            {
                outTemplate.MiscCostTemplates.Add(cost.Copy() as TECMiscCost);
            }
            foreach (TECMiscWiring wiring in this.MiscWiringTemplates)
            {
                outTemplate.MiscWiringTemplates.Add(wiring.Copy() as TECMiscWiring);
            }
            foreach (TECPanel panel in this.PanelTemplates)
            {
                outTemplate.PanelTemplates.Add(panel.Copy() as TECPanel);
            }
            foreach (TECPanelType panelType in this.PanelTypeCatalog)
            {
                outTemplate.PanelTypeCatalog.Add(panelType.Copy() as TECPanelType);
            }
            foreach (TECControlledScope scope in this.ControlledScopeTemplates)
            {
                outTemplate.ControlledScopeTemplates.Add(scope.Copy() as TECControlledScope);
            }

            return outTemplate;
        }

    }
}
