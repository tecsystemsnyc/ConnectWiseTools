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
    public class SystemConnectionsVM : ViewModelBase, IDropTarget
    {
        #region Fields
        private TECTypical typical;

        private readonly ObservableCollection<TECElectricalMaterial> _conduitTypes;

        private ObservableCollection<TECController> _controllers;
        private ObservableCollection<SubScopeConnectionItem> _subScope;
        private ObservableCollection<TECSubScope> _unconnectedSubScope;
        private TECController _selectedController;

        private UpdateConnectionVM _updateVM;
        #endregion

        #region Properties
        public ObservableCollection<TECElectricalMaterial> ConduitTypes { get { return _conduitTypes; } }

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
        public ObservableCollection<SubScopeConnectionItem> SubScope
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

        public SystemConnectionsVM(TECTypical typical, IEnumerable<TECElectricalMaterial> conduitTypes)
        {
            this.typical = typical;
            _conduitTypes = new ObservableCollection<TECElectricalMaterial>(conduitTypes);
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
                && UIHelpers.TargetCollectionIsType(dropInfo, typeof(SubScopeConnectionItem)))
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            TECSubScope subScope = dropInfo.Data as TECSubScope;
            SelectedController.AddSubScope(subScope);
            UnconnectedSubScope.Remove(subScope);
            SubScope.Add(new SubScopeConnectionItem(subScope, typical.TypicalInstanceDictionary.GetInstancesOfType(subScope)));
        }

        private void initializeCollections()
        {
            _controllers = new ObservableCollection<TECController>();
            _subScope = new ObservableCollection<SubScopeConnectionItem>();
            _unconnectedSubScope = new ObservableCollection<TECSubScope>();
        }

        private void updateAllExecute()
        {
            UpdateVM = new UpdateConnectionVM(SubScope);
        }

        private void handleControllerSelected(TECController controller)
        {
            ObservableCollection<SubScopeConnectionItem> typSS = new ObservableCollection<SubScopeConnectionItem>();
            if(controller != null)
            {
                foreach (TECConnection connection in controller.ChildrenConnections)
                {
                    if (connection is TECSubScopeConnection ssConnect)
                    {
                        typSS.Add(new SubScopeConnectionItem(ssConnect.SubScope, typical.TypicalInstanceDictionary.GetInstancesOfType(ssConnect.SubScope)));
                    }
                }
                SubScope = typSS;
            }
            
        }
    }
}
