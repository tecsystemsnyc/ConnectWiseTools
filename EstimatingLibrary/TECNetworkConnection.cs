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
        
        public ObservableCollection<IOType> PossibleIO
        {
            get { }
        }
        #endregion

        #region Constructors
        public TECNetworkConnection(Guid guid) : base(guid)
        {
            _childrenControllers = new ObservableCollection<TECController>();
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
    }
}
