using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECController : TECScope
    {
        private double _cost;
        private ObservableCollection<TECConnection> _connections;
        private ObservableCollection<ConnectionType> _types;

        public double Cost
        {
            get { return _cost; }
            set
            {
                var temp = this.Copy();
                _cost = value;
                NotifyPropertyChanged("Cost", temp, this);
            }
        }
        public ObservableCollection<TECConnection> Connections
        {
            get { return _connections; }
            set
            {
                var temp = this.Copy();
                _connections = value;
                NotifyPropertyChanged("Connections", temp, this);
            }
        }
        public ObservableCollection<ConnectionType> Types
        {
            get { return _types; }
            set
            {
                var temp = this.Copy();
                _types = value;
                NotifyPropertyChanged("Types", temp, this);
            }
        }

        public TECController(string name, string desciption, Guid guid, double cost, ObservableCollection<TECConnection> connections, ObservableCollection<ConnectionType> types) : base(name, desciption, guid)
        {
            _cost = cost;
            _connections = connections;
            _types = types;
        }

        public TECController() : this("", "", Guid.NewGuid(), 0, new ObservableCollection<TECConnection>(), new ObservableCollection<ConnectionType>())
        {
        }

        #region Methods
        public override Object Copy()
        {
            TECController outController = new TECController(this.Name,
                this.Description,
                this.Guid,
                this.Cost,
                this.Connections,
                this.Types);
            
            return outController;
        }

        public override Object DragDropCopy()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
