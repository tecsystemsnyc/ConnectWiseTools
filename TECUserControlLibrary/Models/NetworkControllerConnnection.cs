using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                if (value == Controller)
                {
                    MessageBox.Show("Cannot network controller back to itself.");
                }
                else
                {
                    TECController oldParent = _parentController;
                    _parentController = value;
                    handleParentControllerChanged(oldParent, ParentController);
                    RaisePropertyChanged("ParentController");
                    RaisePropertyChanged("IsConnected");
                    RaisePropertyChanged("PossibleIOType");
                }
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
            if (oldParent != null)
            {
                //Unlink old parent and old connection
                oldParent.Connections.Remove(Connection);
                Controller.Connections.Remove(Connection);
            }

            if (newParent != null)
            {
                if (newParent.IsBMS)
                {
                    bool isAlreadyParent = false;
                    foreach (TECConnection connection in newParent.Connections)
                    {
                        if (connection.Controller == newParent)
                        {
                            isAlreadyParent = true;
                            Connection = connection;
                            Connection.Scope.Add(Controller);
                            Controller.Connections.Add(connection);
                            break;
                        }
                    }
                    
                    if (!isAlreadyParent)
                    {
                        //Link new parent
                        Connection = new TECConnection();
                        Connection.Controller = newParent;
                        Connection.Scope.Add(Controller);
                        Controller.Connections.Add(Connection);

                        newParent.Connections.Add(Connection);
                    }
                }
                else
                {
                    if (getParentConnection(newParent) != null)
                    {
                        Connection = getParentConnection(newParent);
                        ParentController = getParentConnection(newParent).Controller;
                        Controller.Connections.Add(Connection);
                    }
                    else
                    {
                        ParentController = null;
                    }
                }
            }
            else
            {
                Connection = null;
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
        private bool isConnected(TECController controller, List<TECController> searchedControllers = null)
        {
            if (searchedControllers == null)
            {
                searchedControllers = new List<TECController>();
            }

            if (controller.IsServer)
            {
                return true;
            }
            else if (getParentConnection(controller) == null)
            {
                return false;
            }
            else if (searchedControllers.Contains(controller))
            {
                return false;
            }
            else
            {
                searchedControllers.Add(controller);
                TECController parentController = getParentConnection(controller).Controller;
                if (parentController == null)
                {

                }
                bool parentIsConnected = isConnected(parentController, searchedControllers);
                return parentIsConnected;
            }
        }
        public void RefreshBMSConnection()
        {
            RaisePropertyChanged("IsConnected");
        }

        public override object Copy()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
