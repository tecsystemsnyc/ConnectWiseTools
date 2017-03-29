using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                Systems.CollectionChanged -= CollectionChanged;
                _systems = value;
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
                Controllers.CollectionChanged -= CollectionChanged;
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
                Panels.CollectionChanged -= CollectionChanged;
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
        }
        public TECControlledScope() : this(Guid.NewGuid()) { }
        public TECControlledScope(TECControlledScope source) : this()
        {
            copyPropertiesFromScope(source);
            foreach (TECSystem system in source._systems)
            {
                _systems.Add(new TECSystem(system));
            }
            foreach (TECController controller in source._controllers)
            {
                _controllers.Add(new TECController(controller));
            }
            foreach (TECPanel panel in source._panels)
            {
                _panels.Add(new TECPanel(panel));
            }
        }
        
        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    if(item != null)
                    {
                        NotifyPropertyChanged("Add", this, item);
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    if (item != null) { NotifyPropertyChanged("Remove", this, item); }
                }
            }
        }
        
        public override object Copy()
        {
            var outScope = new TECControlledScope(_guid);
            outScope.copyPropertiesFromScope(this);
            foreach(TECController controller in Controllers)
            {
                outScope.Controllers.Add(controller.Copy() as TECController);
            }
            foreach(TECPanel panel in Panels)
            {
                outScope.Panels.Add(panel.Copy() as TECPanel);
            }
            foreach(TECSystem system in Systems)
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
        
    }
}
