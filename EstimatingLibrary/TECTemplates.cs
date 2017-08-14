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
    public class TECTemplates : TECScopeManager
    {
        #region Properties
        private ObservableCollection<TECSystem> _systemTemplates;
        private ObservableCollection<TECEquipment> _equipmentTemplates;
        private ObservableCollection<TECSubScope> _subScopeTemplates;
        private ObservableCollection<TECController> _controllerTemplates;
        private ObservableCollection<TECMisc> _miscCostTemplates;
        private ObservableCollection<TECPanel> _panelTemplates;

        public ObservableCollection<TECSystem> SystemTemplates
        {
            get { return _systemTemplates; }
            set
            {
                var old = SystemTemplates;
                SystemTemplates.CollectionChanged -= (sender, args) => CollectionChanged(sender, args, "SystemTemplates");
                _systemTemplates = value;
                SystemTemplates.CollectionChanged += (sender, args) => CollectionChanged(sender, args, "SystemTemplates");
                NotifyCombinedChanged(Change.Edit, "SystemTemplates", this, value, old);
            }
        }
        public ObservableCollection<TECEquipment> EquipmentTemplates
        {
            get { return _equipmentTemplates; }
            set
            {
                var old = EquipmentTemplates;
                EquipmentTemplates.CollectionChanged -= (sender, args) => CollectionChanged(sender, args, "EquipmentTemplates");
                _equipmentTemplates = value;
                EquipmentTemplates.CollectionChanged += (sender, args) => CollectionChanged(sender, args, "EquipmentTemplates");
                NotifyCombinedChanged(Change.Edit, "EquipmentTemplates", this, value, old);
            }
        }
        public ObservableCollection<TECSubScope> SubScopeTemplates
        {
            get { return _subScopeTemplates; }
            set
            {
                var old = SubScopeTemplates;
                SubScopeTemplates.CollectionChanged -= (sender, args) => CollectionChanged(sender, args, "SubScopeTemplates");
                _subScopeTemplates = value;
                SubScopeTemplates.CollectionChanged += (sender, args) => CollectionChanged(sender, args, "SubScopeTemplates");
                NotifyCombinedChanged(Change.Edit, "SubScopeTemplates", this, value, old);
            }
        }
        public ObservableCollection<TECController> ControllerTemplates
        {
            get { return _controllerTemplates; }
            set
            {
                var old = ControllerTemplates;
                ControllerTemplates.CollectionChanged -= (sender, args) => CollectionChanged(sender, args, "ControllerTemplates");
                _controllerTemplates = value;
                ControllerTemplates.CollectionChanged += (sender, args) => CollectionChanged(sender, args, "ControllerTemplates");
                NotifyCombinedChanged(Change.Edit, "ControllerTemplates", this, value, old);
            }
        }
        public ObservableCollection<TECMisc> MiscCostTemplates
        {
            get { return _miscCostTemplates; }
            set
            {
                var old = MiscCostTemplates;
                MiscCostTemplates.CollectionChanged -= (sender, args) => CollectionChanged(sender, args, "MiscCostTemplates");
                _miscCostTemplates = value;
                MiscCostTemplates.CollectionChanged += (sender, args) => CollectionChanged(sender, args, "MiscCostTemplates");
                NotifyCombinedChanged(Change.Edit, "MiscCostTemplates", this, value, old);
            }
        }
        public ObservableCollection<TECPanel> PanelTemplates
        {
            get { return _panelTemplates; }
            set
            {
                var old = PanelTemplates;
                PanelTemplates.CollectionChanged -= (sender, args) => CollectionChanged(sender, args, "PanelTemplates");
                _panelTemplates = value;
                PanelTemplates.CollectionChanged += (sender, args) => CollectionChanged(sender, args, "PanelTemplates");
                NotifyCombinedChanged(Change.Edit, "PanelTemplates", this, value, old);
            }
        }
        #endregion //Properties

        //For listening to a catalog changing
        public override TECCatalogs Catalogs
        {
            get
            {
                return base.Catalogs;
            }

            set
            {
                base.Catalogs.ScopeChildRemoved -= scopeChildRemoved;
                base.Catalogs = value;
                base.Catalogs.ScopeChildRemoved += scopeChildRemoved;
            }
        }

        #region Constructors
        public TECTemplates() : this(Guid.NewGuid()) { }
        public TECTemplates(Guid guid) : base(guid)
        {
            _labor = new TECLabor();

            _systemTemplates = new ObservableCollection<TECSystem>();
            _equipmentTemplates = new ObservableCollection<TECEquipment>();
            _subScopeTemplates = new ObservableCollection<TECSubScope>();
            _controllerTemplates = new ObservableCollection<TECController>();
            _miscCostTemplates = new ObservableCollection<TECMisc>();
            _panelTemplates = new ObservableCollection<TECPanel>();

            SystemTemplates.CollectionChanged += (sender, args) => CollectionChanged(sender, args, "SystemTemplates");
            EquipmentTemplates.CollectionChanged += (sender, args) => CollectionChanged(sender, args, "EquipmentTemplates");
            SubScopeTemplates.CollectionChanged += (sender, args) => CollectionChanged(sender, args, "SubScopeTemplates");
            ControllerTemplates.CollectionChanged += (sender, args) => CollectionChanged(sender, args, "ControllerTemplates");
            MiscCostTemplates.CollectionChanged += (sender, args) => CollectionChanged(sender, args, "MiscCostTemplates");
            PanelTemplates.CollectionChanged += (sender, args) => CollectionChanged(sender, args, "PanelTemplates");

            Catalogs.ScopeChildRemoved += scopeChildRemoved;
        }
        public TECTemplates(TECTemplates templatesSource) : this(templatesSource.Guid)
        {
            if (_labor != null)
            {
                _labor = templatesSource.Labor;
            }
            foreach (TECSystem system in templatesSource.SystemTemplates)
            { SystemTemplates.Add(new TECSystem(system)); }
            foreach (TECEquipment equip in templatesSource.EquipmentTemplates)
            { EquipmentTemplates.Add(new TECEquipment(equip)); }
            foreach (TECSubScope subScope in templatesSource.SubScopeTemplates)
            { SubScopeTemplates.Add(new TECSubScope(subScope)); }
            foreach (TECController controller in templatesSource.ControllerTemplates)
            { ControllerTemplates.Add(new TECController(controller)); }
            foreach (TECMisc cost in templatesSource.MiscCostTemplates)
            {
                MiscCostTemplates.Add(new TECMisc(cost));
            }
            foreach (TECPanel panel in templatesSource.PanelTemplates)
            {
                PanelTemplates.Add(new TECPanel(panel));
            }
        }
        #endregion //Constructors

        private void scopeChildRemoved(TECObject child)
        {
            foreach (TECElectricalMaterial type in Catalogs.ConnectionTypes)
            {
                removeChildFromScope(type, child);
                TECCost cost = child as TECCost;
                if (cost != null)
                {
                    type.RatedCosts.Remove(cost);
                }
            }
            foreach (TECElectricalMaterial type in Catalogs.ConduitTypes)
            {
                removeChildFromScope(type, child);
                TECCost cost = child as TECCost;
                if (cost != null)
                {
                    type.RatedCosts.Remove(cost);
                }
            }
            foreach (TECPanelType type in Catalogs.PanelTypes)
            {
                removeChildFromScope(type, child);
            }
            foreach (TECIOModule module in Catalogs.IOModules)
            {
                removeChildFromScope(module, child);
            }
            foreach (TECDevice dev in Catalogs.Devices)
            {
                removeChildFromScope(dev, child);
            }
            foreach (TECSystem sys in SystemTemplates)
            {
                removeChildFromScope(sys, child);
                foreach(TECEquipment equip in sys.Equipment)
                {
                    removeChildFromScope(equip, child);
                    foreach(TECSubScope ss in equip.SubScope)
                    {
                        removeChildFromScope(ss, child);
                    }
                }
                foreach(TECSystem instance in sys.SystemInstances)
                {
                    removeChildFromScope(sys, child);
                    foreach (TECEquipment equip in sys.Equipment)
                    {
                        removeChildFromScope(equip, child);
                        foreach (TECSubScope ss in equip.SubScope)
                        {
                            removeChildFromScope(ss, child);
                        }
                    }
                }
            }
            foreach(TECEquipment equip in EquipmentTemplates)
            {
                removeChildFromScope(equip, child);
                foreach (TECSubScope ss in equip.SubScope)
                {
                    removeChildFromScope(ss, child);
                }
            }
            foreach(TECSubScope ss in SubScopeTemplates)
            {
                removeChildFromScope(ss, child);
            }
            foreach(TECController controller in ControllerTemplates)
            {
                removeChildFromScope(controller, child);
            }
            foreach(TECPanel panel in PanelTemplates)
            {
                removeChildFromScope(panel, child);
            }
        }

        private void removeChildFromScope(TECScope scope, TECObject child)
        {
            TECCost cost = child as TECCost;
            TECLabeled tag = child as TECLabeled;
            if (cost != null)
            {
                scope.AssociatedCosts.Remove(cost);
            }
            else if (tag != null)
            {
                scope.Tags.Remove(tag);
            }
            else
            {
                throw new NotImplementedException("Scope child isn't cost or tag.");
            }
        }

        #region Collection Changed
        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e, string propertyName)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    NotifyCombinedChanged(Change.Add, propertyName, this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyCombinedChanged(Change.Remove, propertyName, this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                NotifyCombinedChanged(Change.Edit, propertyName, this, sender);
            }
        }
        #endregion
    }
}
