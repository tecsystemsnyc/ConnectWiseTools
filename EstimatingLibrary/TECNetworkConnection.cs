using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECNetworkConnection : TECConnection
    {
        #region Properties
        private ObservableCollection<TECController> _childrenControllers;
        private TECConnectionType _connectionType;
        private IOType _ioType;

        public ObservableCollection<TECController> ChildrenControllers
        {
            get { return _childrenControllers; }
            set
            {
                var temp = this.Copy();
                _childrenControllers = value;
                NotifyPropertyChanged("ChildrenControllers", temp, this);
            }
        }
        public TECConnectionType ConnectionType
        {
            get { return _connectionType; }
            set
            {
                var temp = this.Copy();
                _connectionType = value;
                NotifyPropertyChanged("ConnectionType", temp, this);
                NotifyPropertyChanged("ChildChanged", (object)this, (object)value);
            }
        }
        public IOType IOType
        {
            get { return _ioType; }
            set
            {
                var temp = this.Copy();
                _ioType = value;
                NotifyPropertyChanged("IOType", temp, this);
                NotifyPropertyChanged("ChildChanged", (object)this, (object)value);
            }
        }
        #endregion

        #region Constructors
        public TECNetworkConnection(Guid guid) : base(guid)
        {
            _childrenControllers = new ObservableCollection<TECController>();
            ChildrenControllers.CollectionChanged += ChildrenControllers_CollectionChanged;
        }
        public TECNetworkConnection() : this(Guid.NewGuid()) { }
        public TECNetworkConnection(TECNetworkConnection connectionSource, Dictionary<Guid, Guid> guidDictionary = null) : base(connectionSource, guidDictionary)
        {
            _childrenControllers = new ObservableCollection<TECController>();
            foreach(TECController controller in connectionSource.ChildrenControllers)
            {
                _childrenControllers.Add(new TECController(controller, guidDictionary));
            }

            if (connectionSource.ConnectionType != null)
            {
                _connectionType = connectionSource.ConnectionType;
            }

            _ioType = connectionSource.IOType;
        }
        #endregion

        #region Methods
        public override object Copy()
        {
            TECNetworkConnection connection = new TECNetworkConnection(this);
            connection._guid = this._guid;
            
            return connection;
        }
        #endregion Methods

        #region Event Handlers
        private void ChildrenControllers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    NotifyPropertyChanged("AddRelationship", this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyPropertyChanged("RemoveRelationship", this, item);
                }
            }
        }
        #endregion
    }
}
