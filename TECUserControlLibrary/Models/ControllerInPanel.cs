using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.Models
{ 
    public class ControllerInPanel : TECObject
    {
        private TECPanel _panel;
        public TECPanel Panel
        {
            get { return _panel; }
            set
            {
                handlePanelSelection(_panel, value);
                _panel = value;
                RaisePropertyChanged("Panel");
            }
        }

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
        
        public ControllerInPanel(TECController controller, TECPanel panel)
        {
            _controller = controller;
            _panel = panel;
        }

        private void handlePanelSelection(TECPanel originalPanel, TECPanel selectedPanel)
        {
            if (selectedPanel != null)
            {
                selectedPanel.Controllers.Add(Controller);
            }
            else if (selectedPanel == null)
            {
                if(originalPanel != null)
                {
                    originalPanel.Controllers.Remove(Controller);
                }
            }
        }

        public override object Copy()
        {
            throw new NotImplementedException();
        }
    }
}
