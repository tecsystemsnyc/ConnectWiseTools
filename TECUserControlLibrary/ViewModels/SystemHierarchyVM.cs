using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using TECUserControlLibrary.Utilities;
using TECUserControlLibrary.ViewModels.AddVMs;

namespace TECUserControlLibrary.ViewModels
{
    public class SystemHierarchyVM : ViewModelBase, IDropTarget
    {
        private TECCatalogs catalogs;
        private TECScopeManager scopeManager;
        private ViewModelBase selectedVM;
        private TECSystem selectedSystem;
        private TECEquipment selectedEquipment;
        private TECSubScope selectedSubScope;
        private IEndDevice selectedDevice;
        private TECPoint selectedPoint;
        private TECController selectedController;
        private TECPanel selectedPanel;
        private SystemConnectionsVM connectionsVM;
        private MiscCostsVM miscVM;
        private ControllersPanelsVM controllersPanelsVM;

        public ViewModelBase SelectedVM
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
            if(value != null)
            {
                ConnectionsVM = new SystemConnectionsVM(value, catalogs.ConduitTypes);
                MiscVM = new MiscCostsVM(value);
                ControllersPanelsVM = new ControllersPanelsVM(value, scopeManager);
            }
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
        public RelayCommand<TECScopeBranch> AddScopeBranchCommand { get; private set; }

        public RelayCommand<TECSystem> DeleteSystemCommand { get; private set; }
        public RelayCommand<TECEquipment> DeleteEquipmentCommand { get; private set; }
        public RelayCommand<TECSubScope> DeleteSubScopeCommand { get; private set; }
        public RelayCommand<IEndDevice> DeleteDeviceCommand { get; private set; }
        public RelayCommand<TECPoint> DeletePointCommand { get; private set; }
        public RelayCommand<TECController> DeleteControllerCommand { get; private set; }
        public RelayCommand<TECPanel> DeletePanelCommand { get; private set; }

        public SystemConnectionsVM ConnectionsVM
        {
            get { return connectionsVM; }
            set
            {
                connectionsVM = value;
                RaisePropertyChanged("ConnectionsVM");
                value.UpdateVM += updateVM =>
                {
                    SelectedVM = updateVM;
                };
            }
        }
        public MiscCostsVM MiscVM
        {
            get { return miscVM; }
            set
            {
                miscVM = value;
                RaisePropertyChanged("MiscVM");
                miscVM.SelectionChanged += misc =>
                {
                    Selected?.Invoke(misc as TECObject);
                };
            }
        }
        public ControllersPanelsVM ControllersPanelsVM
        {
            get { return controllersPanelsVM; }
            set
            {
                controllersPanelsVM = value;
                RaisePropertyChanged("ControllersPanelsVM");
                controllersPanelsVM.SelectionChanged += item =>
                {
                    Selected?.Invoke(item as TECObject);
                };
            }
        }

        public SystemHierarchyVM(TECScopeManager scopeManager)
        {
            AddSystemCommand = new RelayCommand(addSystemExecute, canAddSystem);
            AddEquipmentCommand = new RelayCommand<TECSystem>(addEquipmentExecute, canAddEquipment);
            AddSubScopeCommand = new RelayCommand<TECEquipment>(addSubScopeExecute, canAddSubScope);
            AddPointCommand = new RelayCommand<TECSubScope>(addPointExecute, canAddPoint);
            AddControllerCommand = new RelayCommand<TECSystem>(addControllerExecute, canAddController);
            AddPanelCommand = new RelayCommand<TECSystem>(addPanelExecute, canAddPanel);
            AddMiscCommand = new RelayCommand<TECSystem>(addMiscExecute, canAddMisc);
            BackCommand = new RelayCommand<object>(backExecute);
            AddScopeBranchCommand = new RelayCommand<TECScopeBranch>(addBranchExecute);

            DeleteSystemCommand = new RelayCommand<TECSystem>(deleteSystemExecute, canDeleteSystem);
            DeleteEquipmentCommand = new RelayCommand<TECEquipment>(deleteEquipmentExecute, canDeleteEquipment);
            DeleteSubScopeCommand = new RelayCommand<TECSubScope>(deleteSubScopeExecute, canDeleteSubScope);
            DeleteDeviceCommand = new RelayCommand<IEndDevice>(deleteDeviceExecute, canDeleteDevice);
            DeletePointCommand = new RelayCommand<TECPoint>(deletePointExecute, canDeletePoint);
            DeletePanelCommand = new RelayCommand<TECPanel>(deletePanelExecute, canDeletePanel);
            DeleteControllerCommand = new RelayCommand<TECController>(deleteControllerExecute, canDeleteController);
            catalogs = scopeManager.Catalogs;
            this.scopeManager = scopeManager;
        }
        
        public event Action<TECObject> Selected;

        private void backExecute(object obj)
        {
            if(obj is TECEquipment)
            {
                SelectedEquipment = null;
            } else if (obj is TECSubScope)
            {
                SelectedSubScope = null;
            }
        }

        public void Refresh(TECScopeManager scopeManager)
        {
            catalogs = scopeManager.Catalogs;
        }

        private void addSystemExecute()
        {
            SelectedVM = new AddSystemVM(scopeManager);
        }
        private bool canAddSystem()
        {
            return true;
        }

        private void addEquipmentExecute(TECSystem system)
        {
            SelectedVM = new AddEquipmentVM(system);
        }
        private bool canAddEquipment(TECSystem system)
        {
            return true;
        }

        private void addSubScopeExecute(TECEquipment equipment)
        {
            SelectedVM = new AddSubScopeVM(equipment);
        }
        private bool canAddSubScope(TECEquipment equipment)
        {
            return true;
        }

        private void addPointExecute(TECSubScope subScope)
        {
            SelectedVM = new AddPointVM(subScope);
        }
        private bool canAddPoint(TECSubScope subScope)
        {
            return true;
        }

        private void addControllerExecute(TECSystem system)
        {
            SelectedVM = new AddControllerVM(system, catalogs.ControllerTypes);
        }
        private bool canAddController(TECSystem system)
        {
            return catalogs.ControllerTypes.Count > 0;
        }

        private void addPanelExecute(TECSystem system)
        {
            SelectedVM = new AddPanelVM(system, catalogs.PanelTypes);
        }
        private bool canAddPanel(TECSystem system)
        {
            return catalogs.PanelTypes.Count > 0;
        }

        private void addMiscExecute(TECSystem system)
        {
            SelectedVM = new AddMiscVM(system);
        }
        private bool canAddMisc(TECSystem system)
        {
            return true;
        }
        
        private void addBranchExecute(TECScopeBranch obj)
        {
            if(obj == null)
            {
                SelectedSystem.ScopeBranches.Add(new TECScopeBranch(SelectedSystem.IsTypical));
            } else
            {
                obj.Branches.Add(new TECScopeBranch(obj.IsTypical));
            }
        }
        
        private void deleteSystemExecute(TECSystem obj)
        {
            if(scopeManager is TECBid bid)
            {
                bid.Systems.Remove(obj as TECTypical);
            } else if(scopeManager is TECTemplates templates)
            {
                templates.SystemTemplates.Remove(obj);
            }
        }

        private bool canDeleteSystem(TECSystem arg)
        {
            return scopeManager != null;
        }

        private void deleteEquipmentExecute(TECEquipment obj)
        {
            SelectedSystem.Equipment.Remove(obj);
        }

        private bool canDeleteEquipment(TECEquipment arg)
        {
            return true;
        }

        private void deleteSubScopeExecute(TECSubScope obj)
        {
            SelectedEquipment.SubScope.Remove(obj);
        }

        private bool canDeleteSubScope(TECSubScope arg)
        {
            return true;
        }

        private void deleteDeviceExecute(IEndDevice obj)
        {
            SelectedSubScope.Devices.Remove(obj);
        }

        private bool canDeleteDevice(IEndDevice arg)
        {
            return true;
        }

        private void deletePointExecute(TECPoint obj)
        {
            SelectedSubScope.Points.Remove(obj);
        }

        private bool canDeletePoint(TECPoint arg)
        {
            return true;
        }

        private void deletePanelExecute(TECPanel obj)
        {
            SelectedSystem.Panels.Remove(obj);
        }

        private bool canDeletePanel(TECPanel arg)
        {
            return true;
        }

        private void deleteControllerExecute(TECController obj)
        {
            SelectedSystem.RemoveController(obj);
        }

        private bool canDeleteController(TECController arg)
        {
            return true;
        }

        public void DragOver(IDropInfo dropInfo)
        {
            if(dropInfo.Data is TECSystem)
            {
                UIHelpers.SystemToTypicalDragOver(dropInfo);
            }
            else
            {
                UIHelpers.StandardDragOver(dropInfo);
            }
            
        }
        public void Drop(IDropInfo dropInfo)
        {
            if (dropInfo.Data is TECEquipment equipment)
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
            else if (dropInfo.Data is TECSystem system)
            {
                SelectedVM = new AddSystemVM(scopeManager as TECBid);
                ((AddSystemVM)SelectedVM).ToAdd = new TECSystem(system, false);
            }
            else if (dropInfo.Data is IEndDevice)
            {
                UIHelpers.StandardDrop(dropInfo, scopeManager);
            }
        }

        protected void NotifySelected(TECObject item)
        {
            Selected?.Invoke(item);
        }
    }
}
