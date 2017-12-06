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
using TECUserControlLibrary.Utilities;
using TECUserControlLibrary.ViewModels.AddVMs;

namespace TECUserControlLibrary.ViewModels
{
    public class EquipmentHierarchyVM: ViewModelBase, IDropTarget
    {
        private TECCatalogs catalogs;
        private TECScopeManager scopeManager;
        private ViewModelBase selectedVM;
        private TECEquipment selectedEquipment;
        private TECSubScope selectedSubScope;
        private IEndDevice selectedDevice;
        private TECPoint selectedPoint;

        public ViewModelBase SelectedVM
        {
            get { return selectedVM; }
            set
            {
                selectedVM = value;
                RaisePropertyChanged("SelectedVM");
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
                selectedPoint = value;
                RaisePropertyChanged("SelectedPoint");
                Selected?.Invoke(value);
            }
        }

        public RelayCommand AddEquipmentCommand { get; private set; }
        public RelayCommand<TECEquipment> AddSubScopeCommand { get; private set; }
        public RelayCommand<TECSubScope> AddPointCommand { get; private set; }
        public RelayCommand<object> BackCommand { get; private set; }

        public RelayCommand<TECEquipment> DeleteEquipmentCommand { get; private set; }
        public RelayCommand<TECSubScope> DeleteSubScopeCommand { get; private set; }
        public RelayCommand<IEndDevice> DeleteDeviceCommand { get; private set; }
        public RelayCommand<TECPoint> DeletePointCommand { get; private set; }
        

        public EquipmentHierarchyVM(TECScopeManager scopeManager)
        {
            AddEquipmentCommand = new RelayCommand(addEquipmentExecute, canAddEquipment);
            AddSubScopeCommand = new RelayCommand<TECEquipment>(addSubScopeExecute, canAddSubScope);
            AddPointCommand = new RelayCommand<TECSubScope>(addPointExecute, canAddPoint);
            BackCommand = new RelayCommand<object>(backExecute);

            DeleteEquipmentCommand = new RelayCommand<TECEquipment>(deleteEquipmentExecute, canDeleteEquipment);
            DeleteSubScopeCommand = new RelayCommand<TECSubScope>(deleteSubScopeExecute, canDeleteSubScope);
            DeleteDeviceCommand = new RelayCommand<IEndDevice>(deleteDeviceExecute, canDeleteDevice);
            DeletePointCommand = new RelayCommand<TECPoint>(deletePointExecute, canDeletePoint);
            catalogs = scopeManager.Catalogs;
            this.scopeManager = scopeManager;
        }

        public event Action<TECObject> Selected;

        private void backExecute(object obj)
        {
            if (obj is TECSubScope)
            {
                SelectedSubScope = null;
            }
        }

        public void Refresh(TECScopeManager scopeManager)
        {
            catalogs = scopeManager.Catalogs;
        }

        private void addEquipmentExecute()
        {
            SelectedVM = new AddEquipmentVM(toAdd =>
            {
                (scopeManager as TECTemplates).EquipmentTemplates.Add(toAdd);
            }, scopeManager);
        }
        private bool canAddEquipment()
        {
            return true;
        }

        private void addSubScopeExecute(TECEquipment equipment)
        {
            SelectedVM = new AddSubScopeVM(equipment, scopeManager);
        }
        private bool canAddSubScope(TECEquipment equipment)
        {
            return true;
        }

        private void addPointExecute(TECSubScope subScope)
        {
            SelectedVM = new AddPointVM(subScope, scopeManager);
        }
        private bool canAddPoint(TECSubScope subScope)
        {
            return true;
        }
        

        private void deleteEquipmentExecute(TECEquipment obj)
        {
            (scopeManager as TECTemplates).EquipmentTemplates.Remove(obj);
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
        

        public void DragOver(IDropInfo dropInfo)
        {
            UIHelpers.StandardDragOver(dropInfo);

        }
        public void Drop(IDropInfo dropInfo)
        {
            if (dropInfo.Data is TECEquipment equipment)
            {
                SelectedVM = new AddEquipmentVM(toAdd => {
                    (scopeManager as TECTemplates).EquipmentTemplates.Add(toAdd);
                }, scopeManager);
                ((AddEquipmentVM)SelectedVM).ToAdd = equipment;
            }
            else if (dropInfo.Data is TECSubScope subScope)
            {
                SelectedVM = new AddSubScopeVM(SelectedEquipment, scopeManager);
                ((AddSubScopeVM)SelectedVM).ToAdd = subScope;
            }
            else if (dropInfo.Data is TECPoint point)
            {
                SelectedVM = new AddPointVM(SelectedSubScope, scopeManager);
                ((AddPointVM)SelectedVM).ToAdd = point;
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
