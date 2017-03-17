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
        private TECSubScope _subScope;
        public TECSubScope SubScope
        {
            get { return _subScope; }
            set
            {
                _subScope = value;
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
                handleControllerSelection(value);
                RaisePropertyChanged("Controller");
            }
        }

        private TECConnection _connection;
        public TECConnection Connection
        {
            get { return _connection; }
            set
            {
                if(_connection != null)
                {
                    var temp = _connection.Copy();
                    _connection = value;
                    NotifyPropertyChanged("Connection", temp as object, value as object);
                }
                else
                {
                    _connection = value;
                    NotifyPropertyChanged("Connection", null, value as object);
                }
                
            }
        }

        private TECSystem _parentSystem;
        public TECSystem ParentSystem
        {
            get { return _parentSystem; }
            set
            {
                _parentSystem = value;
                RaisePropertyChanged("ParentSystem");
            }
        }

        private TECEquipment _parentEquipment;
        public TECEquipment ParentEquipment
        {
            get { return _parentEquipment; }
            set
            {
                _parentEquipment = value;
                RaisePropertyChanged("ParentEquipment");
            }
        }

        public SubScopeConnection()
        {
            _connection = null;
            _controller = null;
            _subScope = null;
        }

        private void handleControllerSelection(TECController controller)
        {
            if (controller != null)
            {
                var connection = new TECConnection();
                connection.Controller = controller;
                connection.Scope.Add(SubScope);
                Connection = connection;
            }
            else
            {
                Connection = null;
            }
        }

        public override object Copy()
        {
            throw new NotImplementedException();
        }
    }
}
