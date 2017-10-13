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
        private ObservableCollection<TECSubScope> _instanceSubScope;
        private ObservableCollection<TypicalSubScope> _typicalSubScope;
        private TECController _selectedController;
        private readonly bool _isTypical;
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
        public ObservableCollection<TECSubScope> InstanceSubScope
        {
            get
            {
                return _instanceSubScope;
            }
            set
            {
                _instanceSubScope = value;
                RaisePropertyChanged("InstanceSubScope");
            }
        }
        public ObservableCollection<TypicalSubScope> TypicalSubScope
        {
            get { return _typicalSubScope; }
            set
            {
                _typicalSubScope = value;
                RaisePropertyChanged("TypicalSubScope");
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
        public bool IsTypical { get { return _isTypical; } }
        #endregion

        public SystemConnectionsVM(TECSystem system, TECBid bid = null)
        {
            initializeCollections();
            if (system is TECTypical typical)
            {
                _isTypical = true;
                foreach (TECSubScope ss in system.GetAllSubScope()) {
                    TypicalSubScope.Add(new TypicalSubScope(ss, typical.TypicalInstanceDictionary.GetInstances(ss).ConvertAll(x => (TECSubScope)x)));
                }
            }
            else
            {
                _isTypical = false;
                foreach (TECSubScope ss in system.GetAllSubScope())
                { 
                    InstanceSubScope.Add(ss);
                }
            }
            foreach (TECController controller in system.Controllers)
            {
                Controllers.Add(controller);
            }
            if (bid != null)
            {
                foreach(TECController controller in bid.Controllers)
                {
                    Controllers.Add(controller);
                }
            }
        }

        private void initializeCollections()
        {
            _controllers = new ObservableCollection<TECController>();
            _instanceSubScope = new ObservableCollection<TECSubScope>();
            _typicalSubScope = new ObservableCollection<TypicalSubScope>();
        }
    }
}
