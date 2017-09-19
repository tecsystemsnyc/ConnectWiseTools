using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.Models
{
    public class SubScopeConnection : TECObject
    {
        public bool IsTypical
        {
            get; private set;
        }

        private TECSubScope _subScope;
        public TECSubScope SubScope
        {
            get { return _subScope; }
            set
            {
                _subScope = value;
                raisePropertyChanged("Subscope");
            }
        }

        private TECController _controller;
        public TECController Controller
        {
            get { return _controller; }
            set
            {
                if (value != Controller)
                {
                    handleControllerSelection(value);
                    _controller = value;
                    raisePropertyChanged("Controller");
                }
                
            }
        }
        
        private TECEquipment _parentEquipment;
        public TECEquipment ParentEquipment
        {
            get { return _parentEquipment; }
            set
            {
                _parentEquipment = value;
                raisePropertyChanged("ParentEquipment");
            }
        }

        public TECElectricalMaterial ConduitType
        {
            get
            {
                if (SubScope.Connection == null)
                {
                    return null;
                }
                else
                {
                    return SubScope.Connection.ConduitType;
                }
            }
            set
            {
                handleConduitSelection(value);
                raisePropertyChanged("ConduitType");

            }
        }

        public SubScopeConnection(TECSubScope subscope, bool ssIsTypical) : base(Guid.NewGuid())
        {
            IsTypical = ssIsTypical;
            _subScope = subscope;
            SubScope.PropertyChanged += SubScope_PropertyChanged;
            _controller = null;
            if (subscope.Connection != null)
            {
                _controller = SubScope.Connection.ParentController;
                SubScope.Connection.PropertyChanged += Connection_PropertyChanged;
            }
        }

        private void SubScope_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "Connection")
            {
                if(SubScope.Connection != null)
                {
                    SubScope.Connection.PropertyChanged += Connection_PropertyChanged;
                    _controller = SubScope.Connection.ParentController;
                }
                raisePropertyChanged("Controller");
            }
        }

        private void Connection_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "ParentController")
            {
                _controller = (sender as TECSubScopeConnection).ParentController;
                raisePropertyChanged("Controller");
            }
        }

        private void handleControllerSelection(TECController controller)
        {
            if (Controller != null)
            {
                Controller.RemoveSubScope(SubScope);
            }
            if (controller != null)
            {
                controller.AddSubScope(SubScope);
                SubScope.Connection.PropertyChanged += Connection_PropertyChanged;
            }
            
            
        }

        private void handleConduitSelection(TECElectricalMaterial conduitType)
        {
            if (SubScope.Connection != null)
            {
                SubScope.Connection.ConduitType = conduitType;
            }
        }
    }
}
