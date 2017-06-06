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

        public NetworkController(TECController controller)
        {
            Controller = controller;
        }

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
