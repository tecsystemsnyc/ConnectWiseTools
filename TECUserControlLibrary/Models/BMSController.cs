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
                _controller = value;
                RaisePropertyChanged("Controller");
            }
        }
        public ObservableCollection<TECController> PossibleParents
        {
            get { return _possibleParents; }
            set
            {
                ObservableCollection<TECController> newParents = new ObservableCollection<TECController>();
                foreach (TECController possibleParent in value)
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
                _possibleParents = newParents;
                RaisePropertyChanged("PossibleParents");
            }
        }

        //---Derived---
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
        #endregion

        public BMSController(TECController controller)
        {
            Controller = controller;
            PossibleParents = new ObservableCollection<TECController>();
        }

        public override object Copy()
        {
            throw new NotImplementedException();
        }
    }
}
