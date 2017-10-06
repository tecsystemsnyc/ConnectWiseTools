using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TECUserControlLibrary.ViewModels.AddVMs;

namespace TECUserControlLibrary.ViewModels
{
    public class SystemHierarchyVM : ViewModelBase
    {
        private TECCatalogs catalogs;
        private IAddVM selectedVM;

        public IAddVM SelectedVM
        {
            get { return selectedVM; }
            set
            {
                selectedVM = value;
                RaisePropertyChanged("SelectedVM");
            }
        }

        public RelayCommand AddSystemCommand { get; private set; }
        public RelayCommand<TECSystem> AddEquipmentCommand { get; private set; }
        public RelayCommand<TECEquipment> AddSubScopeCommand { get; private set; }
        public RelayCommand<TECSubScope> AddPointCommand { get; private set; }
        public RelayCommand<TECSystem> AddControllerCommand { get; private set; }
        public RelayCommand<TECSystem> AddPanelCommand { get; private set; }
        public RelayCommand<TECSystem> AddMiscCommand { get; private set; }

        public SystemHierarchyVM(TECCatalogs scopeCatalogs)
        {
            AddSystemCommand = new RelayCommand(AddSystemExecute, CanAddSystem);
            AddEquipmentCommand = new RelayCommand<TECSystem>(AddEquipmentExecute, CanAddEquipment);
            AddSubScopeCommand = new RelayCommand<TECEquipment>(AddSubScopeExecute, CanAddSubScope);
            AddPointCommand = new RelayCommand<TECSubScope>(AddPointExecute, CanAddPoint);
            AddControllerCommand = new RelayCommand<TECSystem>(AddControllerExecute, CanAddController);
            AddPanelCommand = new RelayCommand<TECSystem>(AddPanelExecute, CanAddPanel);
            AddMiscCommand = new RelayCommand<TECSystem>(AddMiscExecute, CanAddMisc);
            catalogs = scopeCatalogs;
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
            return true;
        }

        private void AddPanelExecute(TECSystem system)
        {
            SelectedVM = new AddPanelVM(system, catalogs.PanelTypes);
        }
        private bool CanAddPanel(TECSystem system)
        {
            return true;
        }

        private void AddMiscExecute(TECSystem system)
        {
            SelectedVM = new AddMiscVM(system);
        }
        private bool CanAddMisc(TECSystem system)
        {
            return true;
        }
    }
}
