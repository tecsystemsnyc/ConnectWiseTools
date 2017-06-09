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

        private ObservableCollection<TECController> _possibleParents;
        public ObservableCollection<TECController> PossibleParents
        {
            get { return _possibleParents; }
            set
            {
                ObservableCollection<TECController> newParents = new ObservableCollection<TECController>();

                foreach (TECController possibleParent in value)
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

                _possibleParents = newParents;
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
                if (value)
                {
                    Controller.NetworkType = NetworkType.Server;
                }
                else
                {
                    Controller.NetworkType = NetworkType.DDC;
                }
            }
        }

        public TECConnectionType WireType
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

        public TECConduitType ConduitType
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
        #endregion

        public NetworkController(TECController controller)
        {
            Controller = controller;
            NetworkConnections = new ObservableCollection<TECNetworkConnection>();
            foreach(TECConnection connection in controller.ChildrenConnections)
            {
                if (connection is TECNetworkConnection)
                {
                    NetworkConnections.Add(connection as TECNetworkConnection);
                }
            }
            controller.ChildrenConnections.CollectionChanged += ChildrenConnections_CollectionChanged;
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
        #endregion

        private bool isDescendantOf(TECController descendantController, TECController ancestorController)
        {
            if (descendantController.ParentController == ancestorController)
            {
                return true;
            }
            else if (descendantController.ParentController == null)
            {
                return false;
            }
            else
            {
                return (isDescendantOf(descendantController.ParentController, ancestorController));
            }
        }

        public override object Copy()
        {
            throw new NotImplementedException();
        }
    }
}
