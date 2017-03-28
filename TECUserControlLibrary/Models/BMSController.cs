using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.Models
{
    public class BMSController : TECObject
    {
        #region Properties
        //---Stored---
        private TECController _controller;
        private ObservableCollection<TECController> _possibleParents;

        public TECController Controller
        {
            get { return _controller; }
            set
            {
                Controller.PropertyChanged -= Controller_PropertyChanged;
                _controller = value;
                Controller.PropertyChanged += Controller_PropertyChanged;
                RaisePropertyChanged("Controller");
            }
        }
        public ObservableCollection<TECController> PossibleParents
        {
            get { return _possibleParents; }
            set
            {
                ObservableCollection<TECController> newParents = new ObservableCollection<TECController>();

                TECController noneController = new TECController();
                noneController.Name = "None";
                newParents.Add(noneController);

                foreach (TECController possibleParent in value)
                {
                    if (possibleParent != null && Controller != possibleParent)
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

        //---Derived---
        public TECController ParentController
        {
            get
            {
                return Controller.ParentController;
            }
            set
            {
                if (value != null)
                {
                    value.AddController(Controller);
                }
                else
                {
                    Controller.ParentController = null;
                }
                RaisePropertyChanged("ParentController");
            }
        }
        public ObservableCollection<string> PossibleIO
        {
            get
            {
                ObservableCollection<string> io = new ObservableCollection<string>();
                if (Controller.ParentConnection != null)
                {
                    foreach (IOType type in Controller.ParentConnection.PossibleIO)
                    {
                        io.Add(TECIO.convertTypeToString(type));
                    }
                }
                return io;
            }
            private set { }
        }
        public bool IsConnected
        {
            get { return isConnected(Controller); }
        }
        #endregion

        public BMSController(TECController controller, ObservableCollection<TECController> networkControllers)
        {
            Controller = controller;
            PossibleParents = networkControllers;

            Controller.PropertyChanged += Controller_PropertyChanged;
        }
        #region Methods

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

        #endregion

        #region Event Handlers
        private void Controller_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ParentController")
            {
                RaisePropertyChanged("ParentController");
            }
        }
        #endregion
    }
}
