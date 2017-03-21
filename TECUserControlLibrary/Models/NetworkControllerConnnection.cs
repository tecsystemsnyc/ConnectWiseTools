using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.Models
{
    public class NetworkControllerConnnection : TECObject
    {
        #region Properties
        private TECController _controller;
        public TECController Controller
        {
            get { return _controller; }
            set
            {
                _controller = value;
                RaisePropertyChanged("Controller");
            }
        }

        private TECController _parentController;
        public TECController ParentController
        {
            get { return _parentController; }
            set
            {
                TECController oldParent = _parentController;
                _parentController = value;
                RaisePropertyChanged("ParentController");
                handleParentControllerChanged(oldParent, ParentController);
                RaisePropertyChanged("IsConnected");
            }
        }

        private TECConnection _connection;
        public TECConnection Connection
        {
            get { return _connection; }
            set
            {
                var oldConnection = _connection;
                _connection = value;
                NotifyPropertyChanged("Connection", oldConnection, _connection);
            }
        }

        private ObservableCollection<TECConnectionType> _possibleConnectionTypes;
        public ObservableCollection<TECConnectionType> PossibleConnectionTypes
        {
            get { return _possibleConnectionTypes; }
            set
            {
                _possibleConnectionTypes = value;
                RaisePropertyChanged("PossibleConnectionTypes");
            }
        }

        public ObservableCollection<IOType> PossibleIOType
        {
            get
            {
                ObservableCollection<IOType> possibleIO = new ObservableCollection<IOType>();
                foreach(IOType io in Controller.NetworkIO)
                {
                    foreach(IOType parentIO in ParentController.NetworkIO)
                    {
                        if (io == parentIO)
                        {
                            possibleIO.Add(io);
                        }
                    }
                }
                return possibleIO;
            }
        }
        
        public bool IsConnected
        {
            get
            {
                return isConnected(Controller);
            }
        }
        #endregion

        public NetworkControllerConnnection(TECController controller)
        {
            Controller = controller;

            Connection = getParentConnection(controller);

            if (Connection != null)
            {
                ParentController = Connection.Controller;
            }
        }

        #region Methods
        private void handleParentControllerChanged(TECController oldParent, TECController newParent)
        {
            if (oldParent == null)
            {
                if (newParent != null)
                {
                    Connection = new TECConnection();
                    Connection.Controller = newParent;
                    Controller.Connections.Add(Connection);
                }
            }
            else if (oldParent != null)
            {
                //Unlink old parent
                oldParent.Connections.Remove(Connection);
                if (newParent != null)
                {
                    //Link new parent
                    Connection.Controller = newParent;
                    newParent.Connections.Add(Connection);
                }
                else
                {
                    //If new parent is null, remove old connection
                    Connection = null;
                    Controller.Connections.Remove(getParentConnection(Controller));
                }
            }
        }
        private TECConnection getParentConnection(TECController controller)
        {
            foreach (TECConnection connection in controller.Connections)
            {
                if (controller != connection.Controller)
                {
                    return connection;
                }
            }
            return null;
        }
        private bool isConnected(TECController controller)
        {
            if (controller.IsServer)
            {
                return true;
            }
            else if (getParentConnection(controller) == null)
            {
                return false;
            }
            else
            {
                bool parentIsConnected = isConnected(getParentConnection(controller).Controller);
                return parentIsConnected;
            }
        }

        public override object Copy()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
