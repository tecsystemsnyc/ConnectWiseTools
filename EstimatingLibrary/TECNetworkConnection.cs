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
                _childrenControllers = value;
                RaisePropertyChanged("ChildrenControllers");
            }
        }
        public TECConnectionType ConnectionType
        {
            get { return _connectionType; }
            set
            {
                _connectionType = value;
                RaisePropertyChanged("ConnectionType");
            }
        }
        public IOType IOType
        {
            get { return _ioType; }
            set
            {
                _ioType = value;
                RaisePropertyChanged("IOType");
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
                _connectionType = new TECConnectionType(connectionSource.ConnectionType);
            }

            _ioType = connectionSource.IOType;
        }
        #endregion

        #region Methods
        public override object Copy()
        {
            throw new NotImplementedException();
        }
        #endregion Methods

        #region Event Handlers
        private void ChildrenControllers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    if (item is TECController)
                    {
                        (item as TECController).ParentConnection = this;
                    }
                    NotifyPropertyChanged("AddRelationship", this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    if (item is TECController)
                    {
                        (item as TECController).ParentConnection = null;
                    }
                    NotifyPropertyChanged("RemoveRelationship", this, item);
                }
            }
        }
        #endregion
    }
}
