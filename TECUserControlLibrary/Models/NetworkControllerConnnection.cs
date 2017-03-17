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
                handleParentControllerChanged(oldParent);
            }
        }

        private TECConnection _connection;
        public TECConnection Connection
        {
            get { return _connection; }
            set
            {
                _connection = value;
                RaisePropertyChanged("Connection");
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

        private bool _isConnected;
        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                RaisePropertyChanged("IsConnected");
            }
        }
        #endregion

        public NetworkControllerConnnection(TECController controller)
        {
            Controller = controller;

            Connection = new TECConnection();
            Connection.Scope.Add(controller);

            IsConnected = false;
        }

        #region Methods
        private void handleParentControllerChanged(TECController oldParent)
        {
            //Unlink old parent
            oldParent.Connections.Remove(Connection);

            //Link new parent
            Connection.Controller = ParentController;
            ParentController.Connections.Add(Connection);
        }

        public override object Copy()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
