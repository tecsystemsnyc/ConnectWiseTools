using EstimatingLibrary;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.ViewModels
{
    public class SystemConnectionsVM : ViewModelBase
    {
        #region Fields
        private ObservableCollection<TECController> _controllers;
        private ObservableCollection<TECSubScope> _unconnectedSubScope;
        private TECController _selectedController;
        private ObservableCollection<TECSubScope> _selectedSubScope;
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
        public ObservableCollection<TECSubScope> UnconnectedSubScope
        {
            get
            {
                return _unconnectedSubScope;
            }
            set
            {
                _unconnectedSubScope = value;
                RaisePropertyChanged("UnconnectedSubScope");
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
            initializeCollections();
            foreach (TECSubScope ss in system.GetAllSubScope())
            { 
                if (ss.ParentConnection == null)
                {
                    UnconnectedSubScope.Add(ss);
                }
            }
            foreach (TECController controller in system.Controllers)
            {
                Controllers.Add(controller);
            }
        }

        public event Action<UpdateConnectionVM> Update;

        private void initializeCollections()
        {
            _controllers = new ObservableCollection<TECController>();
            _unconnectedSubScope = new ObservableCollection<TECSubScope>();
        }
    }
}
