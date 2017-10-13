using EstimatingLibrary;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.ViewModels
{
    public class SystemConnectionsVM : ViewModelBase
    {
        #region Fields
        private ObservableCollection<TECController> _controllers;
        private ObservableCollection<TECSubScopeConnection> _connections;
        private TECController _selectedController;

        private bool isTypical;
        #endregion

        #region Properties
        public ObservableCollection<TECController> Controllers
        {
            get
            {
                return _controllers;
            }
            set
            {
                _controllers = value;
                RaisePropertyChanged("Controllers");
            }
        }
        public ObservableCollection<TECSubScopeConnection> Connections
        {
            get
            {
                return _connections;
            }
            set
            {
                _connections = value;
                RaisePropertyChanged("Connections");
            }
        }
        public TECController SelectedController
        {
            get
            {
                return _selectedController;
            }
            set
            {
                _selectedController = value;
                RaisePropertyChanged("SelectedController");
            }
        }
        #endregion

        public SystemConnectionsVM(TECSystem system)
        {
            if (system is TECTypical)
            {
                isTypical = true;
            }
            else
            {
                isTypical = false;
            }
        }
    }
}
