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
        private ObservableCollection<TECConnection> _connections { get; set; }
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
            _connections = new ObservableCollection<TECConnection>();
            _systems = new ObservableCollection<TECSystem>();
            _controllers = new ObservableCollection<TECController>();
            _panels = new ObservableCollection<TECPanel>();
            Connections.CollectionChanged += CollectionChanged;
            Systems.CollectionChanged += CollectionChanged;
            Controllers.CollectionChanged += CollectionChanged;
            Panels.CollectionChanged += CollectionChanged;
        }
        public TECControlledScope() : this(Guid.NewGuid()) { }
        
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
        }

        public override object Copy()
        {
            throw new NotImplementedException();
        }

        public override object DragDropCopy()
        {
            throw new NotImplementedException();
        }
    }
}
