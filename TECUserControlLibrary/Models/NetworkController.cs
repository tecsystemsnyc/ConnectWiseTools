using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.Models
{
    public class NetworkController : TECObject
    {
        #region Properties
        public TECController Controller { get; private set; }

        public TECController ParentController
        {
            get { return Controller.GetParentController(); }
            set
            {
                Controller.SetParentController(value, defaultWireType);
                RaisePropertyChanged("ParentController");
                RefreshIsConnected();
            }
        }
        
        private ObservableCollection<TECController> _possibleParents;
        public ObservableCollection<TECController> PossibleParents
        {
            get { return _possibleParents; }
            private set
            {
                _possibleParents = value;
                RaisePropertyChanged("PossibleParents");
            }
        }
        
        public ObservableCollection<TECNetworkConnection> NetworkConnections { get; private set; }

        public bool IsServer
        {
            get
            {
                return Controller.NetworkType == NetworkType.Server;
            }
            set
            {
                if (value != IsServer)
                {
                    if (value)
                    {
                        Controller.NetworkType = NetworkType.Server;
                    }
                    else
                    {
                        Controller.NetworkType = NetworkType.DDC;
                    }
                    RaisePropertyChanged("IsServer");
                    RaisePropertyChanged("IsConnected");
                }
            }
        }

        public TECElectricalMaterial WireType
        {
            get
            {
                if (Controller.ParentConnection != null)
                {
                    return Controller.ParentConnection.ConnectionType;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (Controller.ParentConnection != null)
                {
                    Controller.ParentConnection.ConnectionType = value;
                    RaisePropertyChanged("WireType");
                }
            }
        }

        public TECElectricalMaterial ConduitType
        {
            get
            {
                if (Controller.ParentConnection != null)
                {
                    return Controller.ParentConnection.ConduitType;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (Controller.ParentConnection != null)
                {
                    Controller.ParentConnection.ConduitType = value;
                    RaisePropertyChanged("ConduitType");
                }
            }
        }

        public double WireLength
        {
            get
            {
                if (Controller.ParentConnection != null)
                {
                    return Controller.ParentConnection.Length;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (Controller.ParentConnection != null)
                {
                    Controller.ParentConnection.Length = value;
                    RaisePropertyChanged("WireLength");
                }
            }
        }

        public double ConduitLength
        {
            get
            {
                if (Controller.ParentConnection != null)
                {
                    return Controller.ParentConnection.ConduitLength;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (Controller.ParentConnection != null)
                {
                    Controller.ParentConnection.ConduitLength = value;
                    RaisePropertyChanged("ConduitLength");
                }
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

        private TECElectricalMaterial defaultWireType;
        #endregion

        public NetworkController(TECController controller, TECElectricalMaterial defaultWireType) : base(Guid.NewGuid())
        {
            Controller = controller;
            _possibleParents = new ObservableCollection<TECController>();
            NetworkConnections = new ObservableCollection<TECNetworkConnection>();
            foreach(TECConnection connection in controller.ChildrenConnections)
            {
                if (connection is TECNetworkConnection)
                {
                    NetworkConnections.Add(connection as TECNetworkConnection);
                }
            }
            controller.ChildrenConnections.CollectionChanged += ChildrenConnections_CollectionChanged;
            controller.PropertyChanged += Controller_PropertyChanged;
            this.defaultWireType = defaultWireType;
            if (controller.ParentConnection != null)
            {
                controller.ParentConnection.PropertyChanged += ParentConnection_PropertyChanged;
            }
        }

        public void RefreshIsConnected()
        {
            IsConnected = isConnected(this.Controller);
        }

        public void SetPossibleParents(ObservableCollection<TECController> possibleParents, TECController noneController)
        {
            ObservableCollection<TECController> newParents = new ObservableCollection<TECController>();
            newParents.Add(noneController);

            foreach (TECController possibleParent in possibleParents)
            {
                if (possibleParent != null && Controller != possibleParent && !isDescendantOf(possibleParent, Controller))
                {
                    foreach (IOType thisType in Controller.NetworkIO)
                    {
                        foreach (IOType parentType in possibleParent.NetworkIO)
                        {
                            if (thisType == parentType)
                            {
                                newParents.Add(possibleParent);
                                break;
                            }
                        }
                        if (newParents.Contains(possibleParent))
                        {
                            break;
                        }
                    }
                }
            }

            ObservableCollection<TECController> controllerToRemove = new ObservableCollection<TECController>();
            foreach(TECController controller in PossibleParents)
            {
                if (!newParents.Contains(controller))
                {
                    controllerToRemove.Add(controller);
                }
                else
                {
                    newParents.Remove(controller);
                }
            }
            foreach(TECController controller in controllerToRemove)
            {
                PossibleParents.Remove(controller);
            }
            foreach(TECController controller in newParents)
            {
                PossibleParents.Add(controller);
            }
        }

        #region Event Handlers
        private void ChildrenConnections_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach(object item in e.NewItems)
                {
                    if (item is TECNetworkConnection)
                    {
                        NetworkConnections.Add(item as TECNetworkConnection);
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach(object item in e.OldItems)
                {
                    if (item is TECNetworkConnection)
                    {
                        NetworkConnections.Remove(item as TECNetworkConnection);
                    }
                }
            }
        }
        private void Controller_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e is PropertyChangedExtendedEventArgs<object>)
            {
                var args = e as PropertyChangedExtendedEventArgs<object>;

                if (args.PropertyName == "ParentConnection")
                {
                    if ((args.OldValue as TECController).ParentConnection != null)
                    {
                        (args.OldValue as TECController).ParentConnection.PropertyChanged -= ParentConnection_PropertyChanged;
                    }
                    RaisePropertyChanged("ParentController");
                    RefreshIsConnected();
                    if ((args.NewValue as TECController).ParentConnection != null)
                    {
                        (args.NewValue as TECController).ParentConnection.PropertyChanged += ParentConnection_PropertyChanged;
                    }
                }
            }
        }
        private void ParentConnection_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Length")
            {
                RaisePropertyChanged("WireLength");
            }
            else if (e.PropertyName == "ConduitLength")
            {
                RaisePropertyChanged("ConduitLength");
            }
        }
        #endregion

        private bool isDescendantOf(TECController descendantController, TECController ancestorController)
        {
            if (descendantController.GetParentController() == ancestorController)
            {
                return true;
            }
            else if (descendantController.GetParentController() == null)
            {
                return false;
            }
            else
            {
                return (isDescendantOf(descendantController.GetParentController(), ancestorController));
            }
        }

        private bool isConnected(TECController controller, List<TECController> searchedControllers = null)
        {
            if (searchedControllers == null)
            {
                searchedControllers = new List<TECController>();
            }

            if (controller.NetworkType == NetworkType.Server)
            {
                return true;
            }
            else if (controller.ParentConnection == null || searchedControllers.Contains(controller))
            {
                return false;
            }
            else
            {
                searchedControllers.Add(controller);
                TECController parentController = controller.ParentConnection.ParentController;
                if (parentController == null)
                {
                    throw new NullReferenceException("Parent controller to passed controller is null, but parent connection isn't.");
                }
                bool parentIsConnected = isConnected(parentController, searchedControllers);
                return parentIsConnected;
            }
        }

        public override object Copy()
        {
            throw new NotImplementedException();
        }
    }
}
