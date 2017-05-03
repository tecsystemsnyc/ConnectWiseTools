using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECControlledScope : TECScope
    {
        private ObservableCollection<TECSystem> _systems { get; set; }
        public ObservableCollection<TECSystem> Systems
        {
            get { return _systems; }
            set
            {
                var temp = this.Copy();
                if (Systems != null)
                {
                    Systems.CollectionChanged -= CollectionChanged;
                }
                _systems = value;
                registerSystems();
                Systems.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("Systems", temp, this);
            }
        }

        private ObservableCollection<TECController> _controllers { get; set; }
        public ObservableCollection<TECController> Controllers
        {
            get { return _controllers; }
            set
            {
                var temp = this.Copy();
                if (Controllers != null)
                {
                    Controllers.CollectionChanged -= CollectionChanged;
                }
                _controllers = value;
                Controllers.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("Controllers", temp, this);
            }
        }

        private ObservableCollection<TECPanel> _panels { get; set; }
        public ObservableCollection<TECPanel> Panels
        {
            get { return _panels; }
            set
            {
                var temp = this.Copy();
                if (Panels != null)
                {
                    Panels.CollectionChanged -= CollectionChanged;
                }

                _panels = value;
                Panels.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("Panels", temp, this);
            }
        }

        public TECControlledScope(Guid guid) : base(guid)
        {
            _systems = new ObservableCollection<TECSystem>();
            _controllers = new ObservableCollection<TECController>();
            _panels = new ObservableCollection<TECPanel>();
            Systems.CollectionChanged += CollectionChanged;
            Controllers.CollectionChanged += CollectionChanged;
            Panels.CollectionChanged += CollectionChanged;
            registerSystems();
        }
        public TECControlledScope() : this(Guid.NewGuid()) { }
        public TECControlledScope(TECControlledScope source, Dictionary<Guid, Guid> guidDictionary = null) : this()
        {
            copyPropertiesFromScope(source);
            foreach (TECSystem system in source._systems)
            {
                _systems.Add(new TECSystem(system, guidDictionary));
            }
            foreach (TECController controller in source._controllers)
            {
                _controllers.Add(new TECController(controller, guidDictionary));
            }
            foreach (TECPanel panel in source._panels)
            {
                _panels.Add(new TECPanel(panel, guidDictionary));
            }
        }

        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    if (item != null)
                    {
                        NotifyPropertyChanged("Add", this, item);
                        if (item is TECSystem)
                        {
                            (item as TECSystem).PropertyChanged += System_PropertyChanged;
                        }
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    if (item != null)
                    {
                        NotifyPropertyChanged("Remove", this, item);
                        if (item is TECSystem)
                        {
                            (item as TECSystem).PropertyChanged -= System_PropertyChanged;
                        }
                    }
                }
            }
        }

        private void registerSystems()
        {
            foreach (TECSystem system in Systems)
            {
                system.PropertyChanged += System_PropertyChanged;
            }
        }
        private void System_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RemovedSubScope")
            {
                var args = e as PropertyChangedExtendedEventArgs<object>;
                if (args.NewValue is TECEquipment)
                {
                    handleEquipmentSubScopeRemoval(args.NewValue as TECEquipment);
                }
                else
                {
                    handleSubScopeRemovalInConnections(args.NewValue as TECSubScope);
                }
            }
        }

        public override object Copy()
        {
            var outScope = new TECControlledScope(_guid);
            outScope.copyPropertiesFromScope(this);
            foreach (TECController controller in Controllers)
            {
                outScope.Controllers.Add(controller.Copy() as TECController);
            }
            foreach (TECPanel panel in Panels)
            {
                outScope.Panels.Add(panel.Copy() as TECPanel);
            }
            foreach (TECSystem system in Systems)
            {
                outScope.Systems.Add(system.Copy() as TECSystem);
            }
            return outScope;
        }

        public override object DragDropCopy()
        {
            var outScope = new TECControlledScope(this);
            return outScope;
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
                foreach (TECSubScopeConnection connection in controller.ChildrenConnections)
                {
                    if (connection.SubScope == subScope)
                    {
                        subScopeToRemove.Add(subScope as TECSubScope);
                    }
                }
                foreach (TECSubScope sub in subScopeToRemove)
                {
                    controller.RemoveSubScope(sub);
                }
            }
        }

    }
}
