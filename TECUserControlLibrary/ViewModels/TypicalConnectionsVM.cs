using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels
{
    public class TypicalConnectionsVM : ViewModelBase, IDropTarget
    {
        #region Fields
        private TECTypical typical;

        private ObservableCollection<TECController> _controllers;
        private ObservableCollection<TypicalSubScope> _subScope;
        private ObservableCollection<TECSubScope> _unconnectedSubScope;
        private TECController _selectedController;

        private UpdateConnectionVM _updateVM;
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
        public ObservableCollection<TypicalSubScope> SubScope
        {
            get { return _subScope; }
            set
            {
                _subScope = value;
                RaisePropertyChanged("SubScope");
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
                handleControllerSelected(value);
            }
        }

        public UpdateConnectionVM UpdateVM
        {
            get { return _updateVM; }
            private set
            {
                _updateVM = value;
                RaisePropertyChanged("UpdateVM");
                Update?.Invoke(value);
            }
        }

        public ICommand UpdateAllCommand { get; private set; }
        #endregion

        public TypicalConnectionsVM(TECTypical typical)
        {
            this.typical = typical;
            initializeCollections();
            foreach (TECSubScope ss in typical.GetAllSubScope())
            {
                if (ss.ParentConnection == null && ss.Connection == null)
                {
                    UnconnectedSubScope.Add(ss);
                }
                
            }
            foreach (TECController controller in typical.Controllers)
            {
                Controllers.Add(controller);
            }
            UpdateAllCommand = new RelayCommand(updateAllExecute);
        }

        public event Action<UpdateConnectionVM> Update;

        public void DragOver(IDropInfo dropInfo)
        {
            TECSubScope subScope = dropInfo.Data as TECSubScope;
            if (subScope != null && SelectedController != null && SelectedController.CanConnectSubScope(subScope)
                && UIHelpers.TargetCollectionIsType(dropInfo, typeof(TypicalSubScope)))
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            TECSubScope subScope = dropInfo.Data as TECSubScope;
            SelectedController.AddSubScope(subScope);
        }

        private void initializeCollections()
        {
            _controllers = new ObservableCollection<TECController>();
            _subScope = new ObservableCollection<TypicalSubScope>();
            _unconnectedSubScope = new ObservableCollection<TECSubScope>();
        }

        private void updateAllExecute()
        {
            UpdateVM = new UpdateConnectionVM(SubScope);
        }

        private void handleControllerSelected(TECController controller)
        {
            ObservableCollection<TypicalSubScope> typSS = new ObservableCollection<TypicalSubScope>();
            foreach (TECConnection connection in controller.ChildrenConnections)
            {
                if (connection is TECSubScopeConnection ssConnect)
                {
                    typSS.Add(new TypicalSubScope(ssConnect.SubScope, typical.TypicalInstanceDictionary.GetInstancesOfType(ssConnect.SubScope)));
                }
            }
            SubScope = typSS;
        }
    }
}
