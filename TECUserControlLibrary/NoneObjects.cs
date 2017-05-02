using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary
{
    public class NoneObjects : TECObject
    {
        private TECConduitType _noneConduitType;
        private TECConnectionType _noneConnectionType;
        private TECPanel _nonePanel;
        private TECController _noneController;

        public TECConduitType NoneConduitType
        {
            get { return _noneConduitType; }
            set
            {
                _noneConduitType = value;
                RaisePropertyChanged("NoneConduitType");
            }
        }
        public TECConnectionType NoneConnectionType
        {
            get { return _noneConnectionType; }
            set
            {
                _noneConnectionType = value;
                RaisePropertyChanged("NoneConnectionType");
            }
        }
        public TECPanel NonePanel
        {
            get { return _nonePanel; }
            set
            {
                _nonePanel = value;
                RaisePropertyChanged("NonePanel");
            }
        }
        public TECController NoneController
        {
            get { return _noneController; }
            set
            {
                _noneController = value;
                RaisePropertyChanged("NoneController");
            }
        }

        public NoneObjects()
        {
            setupNoneValues();
        }

        private void setupNoneValues()
        {
            NoneConduitType = new TECConduitType();
            NoneConduitType.Name = "None";

            NoneConnectionType = new TECConnectionType();
            NoneConnectionType.Name = "None";

            NonePanel = new TECPanel();
            NonePanel.Name = "None";

            NoneController = new TECController();
            NoneController.Name = "None";
        }

        public override object Copy()
        {
            throw new NotImplementedException();
        }
    }
}
