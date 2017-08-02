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
                if(value != Panel)
                {
                    handlePanelSelection(_panel, value);
                    _panel = value;
                    RaisePropertyChanged("Panel");
                }
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

        public ControllerInPanel(TECController controller, TECPanel panel) : base(Guid.NewGuid())
        {
            _controller = controller;
            _panel = panel;
        }

        private void handlePanelSelection(TECPanel originalPanel, TECPanel selectedPanel)
        {
            if (originalPanel != null)
            {
                originalPanel.Controllers.Remove(Controller);
            }
            if (selectedPanel != null)
            {
                selectedPanel.Controllers.Add(Controller);
            }
        }
        

        public void UpdatePanel(TECPanel panel)
        {
            _panel = panel;
            RaisePropertyChanged("Panel");
        }
        
    }
}
