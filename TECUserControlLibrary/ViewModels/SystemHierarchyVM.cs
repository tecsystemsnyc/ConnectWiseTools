using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TECUserControlLibrary.Utilities;
using TECUserControlLibrary.ViewModels.AddVMs;

namespace TECUserControlLibrary.ViewModels
{
    public class SystemHierarchyVM : ViewModelBase, IDropTarget
    {
        private TECCatalogs catalogs;
        private IAddVM selectedVM;
        private TECSystem selectedSystem;
        private TECEquipment selectedEquipment;
        private TECSubScope selectedSubScope;
        private IEndDevice selectedDevice;
        private TECPoint selectedPoint;
        private TECController selectedController;
        private TECPanel selectedPanel;
        private SystemConnectionsVM connectionsVM;

        public IAddVM SelectedVM
        {
            get { return selectedVM; }
            set
            {
                selectedVM = value;
                RaisePropertyChanged("SelectedVM");
            }
        }
        public TECSystem SelectedSystem
        {
            get { return selectedSystem; }
            set
            {
                selectedSystem = value;
                RaisePropertyChanged("SelectedSystem");
                Selected?.Invoke(value);
                SystemSelected(value);
            }
        }

        private void SystemSelected(TECSystem value)
        {
            ConnectionsVM = new SystemConnectionsVM(value);
        }

        public TECEquipment SelectedEquipment
        {
            get { return selectedEquipment; }
            set
            {
                selectedEquipment = value;
                RaisePropertyChanged("SelectedEquipment");
                Selected?.Invoke(value);
            }
        }
        public TECSubScope SelectedSubScope
        {
            get { return selectedSubScope; }
            set
            {
                selectedSubScope = value;
                RaisePropertyChanged("SelectedSubScope");
                Selected?.Invoke(value);
            }
        }
        public IEndDevice SelectedDevice
        {
            get { return selectedDevice; }
            set
            {
                selectedDevice = value;
                RaisePropertyChanged("SelectedDevice");
                Selected?.Invoke(value as TECObject);
            }
        }
        public TECPoint SelectedPoint
        {
            get { return selectedPoint; }
            set
            {
                selectedPoint= value;
                RaisePropertyChanged("SelectedPoint");
                Selected?.Invoke(value);
            }
        }
        public TECController SelectedController
        {
            get { return selectedController; }
            set
            {
                selectedController = value;
                RaisePropertyChanged("SelectedController");
                Selected?.Invoke(value);
            }
        }
        public TECPanel SelectedPanel
        {
            get { return selectedPanel; }
            set
            {
                selectedPanel = value;
                RaisePropertyChanged("SelectedPanel");
                Selected?.Invoke(value);
            }
        }

        public RelayCommand AddSystemCommand { get; private set; }
        public RelayCommand<TECSystem> AddEquipmentCommand { get; private set; }
        public RelayCommand<TECEquipment> AddSubScopeCommand { get; private set; }
        public RelayCommand<TECSubScope> AddPointCommand { get; private set; }
        public RelayCommand<TECSystem> AddControllerCommand { get; private set; }
        public RelayCommand<TECSystem> AddPanelCommand { get; private set; }
        public RelayCommand<TECSystem> AddMiscCommand { get; private set; }
        public RelayCommand<object> BackCommand { get; private set; }

        public SystemConnectionsVM ConnectionsVM
        {
            get { return connectionsVM; }
            set
            {
                connectionsVM = value;
                RaisePropertyChanged("ConnectionsVM");
            }
        }

        public SystemHierarchyVM(TECCatalogs scopeCatalogs)
        {
            AddSystemCommand = new RelayCommand(AddSystemExecute, CanAddSystem);
            AddEquipmentCommand = new RelayCommand<TECSystem>(AddEquipmentExecute, CanAddEquipment);
            AddSubScopeCommand = new RelayCommand<TECEquipment>(AddSubScopeExecute, CanAddSubScope);
            AddPointCommand = new RelayCommand<TECSubScope>(AddPointExecute, CanAddPoint);
            AddControllerCommand = new RelayCommand<TECSystem>(AddControllerExecute, CanAddController);
            AddPanelCommand = new RelayCommand<TECSystem>(AddPanelExecute, CanAddPanel);
            AddMiscCommand = new RelayCommand<TECSystem>(AddMiscExecute, CanAddMisc);
            BackCommand = new RelayCommand<object>(BackExecute);
            catalogs = scopeCatalogs;
        }

        public event Action<TECObject> Selected;

        private void BackExecute(object obj)
        {
            if(obj is TECEquipment)
            {
                SelectedEquipment = null;
            } else if (obj is TECSubScope)
            {
                SelectedSubScope = null;
            }
        }

        public void Refresh(TECCatalogs scopeCatalogs)
        {
            catalogs = scopeCatalogs;
        }

        private void AddSystemExecute()
        {
            throw new NotImplementedException();
        }
        private bool CanAddSystem()
        {
            throw new NotImplementedException();
        }

        private void AddEquipmentExecute(TECSystem system)
        {
            SelectedVM = new AddEquipmentVM(system);
        }
        private bool CanAddEquipment(TECSystem system)
        {
            return true;
        }

        private void AddSubScopeExecute(TECEquipment equipment)
        {
            SelectedVM = new AddSubScopeVM(equipment);
        }
        private bool CanAddSubScope(TECEquipment equipment)
        {
            return true;
        }

        private void AddPointExecute(TECSubScope subScope)
        {
            SelectedVM = new AddPointVM(subScope);
        }
        private bool CanAddPoint(TECSubScope subScope)
        {
            return true;
        }

        private void AddControllerExecute(TECSystem system)
        {
            SelectedVM = new AddControllerVM(system, catalogs.ControllerTypes);
        }
        private bool CanAddController(TECSystem system)
        {
            return catalogs.ControllerTypes.Count > 0;
        }

        private void AddPanelExecute(TECSystem system)
        {
            SelectedVM = new AddPanelVM(system, catalogs.PanelTypes);
        }
        private bool CanAddPanel(TECSystem system)
        {
            return catalogs.PanelTypes.Count > 0;
        }

        private void AddMiscExecute(TECSystem system)
        {
            SelectedVM = new AddMiscVM(system);
        }
        private bool CanAddMisc(TECSystem system)
        {
            return true;
        }

        public void DragOver(IDropInfo dropInfo)
        {
            UIHelpers.StandardDragOver(dropInfo);
        }
        public void Drop(IDropInfo dropInfo)
        {
            if(dropInfo.Data is TECEquipment equipment)
            {
                SelectedVM = new AddEquipmentVM(SelectedSystem);
                ((AddEquipmentVM)SelectedVM).ToAdd = new TECEquipment(equipment, SelectedSystem.IsTypical);
            } else if (dropInfo.Data is TECSubScope subScope)
            {
                SelectedVM = new AddSubScopeVM(SelectedEquipment);
                ((AddSubScopeVM)SelectedVM).ToAdd = new TECSubScope(subScope, SelectedSystem.IsTypical);
            }
            else if (dropInfo.Data is TECPoint point)
            {
                SelectedVM = new AddPointVM(SelectedSubScope);
                ((AddPointVM)SelectedVM).ToAdd = new TECPoint(point, SelectedSystem.IsTypical);
            }
            else if (dropInfo.Data is TECController controller)
            {
                SelectedVM = new AddControllerVM(SelectedSystem, catalogs.ControllerTypes);
                ((AddControllerVM)SelectedVM).ToAdd = new TECController(controller, SelectedSystem.IsTypical);
            }
            else if (dropInfo.Data is TECPanel panel)
            {
                SelectedVM = new AddPanelVM(SelectedSystem, catalogs.PanelTypes);
                ((AddPanelVM)SelectedVM).ToAdd = new TECPanel(panel, SelectedSystem.IsTypical);
            }
            else if (dropInfo.Data is TECMisc misc)
            {
                SelectedVM = new AddMiscVM(SelectedSystem);
                ((AddMiscVM)SelectedVM).ToAdd = new TECMisc(misc, SelectedSystem.IsTypical);
            }
        }

        protected void NotifySelected(TECObject item)
        {
            Selected?.Invoke(item);
        }
    }
}
