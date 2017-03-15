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
                _panel = value;
                handlePanelSelection(value);
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
        
        public ControllerInPanel()
        {

        }

        private void handlePanelSelection(TECPanel panel)
        {
            if (panel != null)
            {
                Panel.Controllers.Add(Controller);
            }
            else
            {
                Panel.Controllers.Remove(Controller);
            }
            
        }

        public override object Copy()
        {
            throw new NotImplementedException();
        }
    }
}
