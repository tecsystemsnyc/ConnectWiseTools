using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MaterialVM : ViewModelBase, IDropTarget
    {
        #region Properties
        private TECTemplates _templates;
        public TECTemplates Templates
        {
            get { return _templates; }
            set
            {
                _templates = value;
                RaisePropertyChanged("Templates");
            }
        }

        private string _connectionTypeName;
        public string ConnectionTypeName
        {
            get { return _connectionTypeName; }
            set
            {
                _connectionTypeName = value;
                RaisePropertyChanged("ConnectionTypeName");
            }
        }
        private double _connectionTypeCost;
        public double ConnectionTypeCost
        {
            get { return _connectionTypeCost; }
            set
            {
                _connectionTypeCost = value;
                RaisePropertyChanged("ConnectionTypeCost");
            }
        }
        private double _connectionTypeLabor;
        public double ConnectionTypeLabor
        {
            get { return _connectionTypeLabor; }
            set
            {
                _connectionTypeLabor = value;
                RaisePropertyChanged("ConnectionTypeLabor");
            }
        }

        private string _conduitTypeName;
        public string ConduitTypeName
        {
            get { return _conduitTypeName; }
            set
            {
                _conduitTypeName = value;
                RaisePropertyChanged("ConduitTypeName");
            }
        }
        private double _conduitTypeCost;
        public double ConduitTypeCost
        {
            get { return _conduitTypeCost; }
            set
            {
                _conduitTypeCost = value;
                RaisePropertyChanged("ConduitTypeCost");
            }
        }
        private double _conduitTypeLabor;
        public double ConduitTypeLabor
        {
            get { return _conduitTypeLabor; }
            set
            {
                _conduitTypeLabor = value;
                RaisePropertyChanged("ConduitTypeLabor");
            }
        }

        private string _associatedCostName;
        public string AssociatedCostName
        {
            get { return _associatedCostName; }
            set
            {
                _associatedCostName = value;
                RaisePropertyChanged("AssociatedCostName");
            }
        }
        private CostType _associatedCostType;
        public CostType AssociatedCostType
        {
            get { return _associatedCostType; }
            set
            {
                _associatedCostType = value;
                RaisePropertyChanged("AssociatedCostType");
            }
        }
        private double _associatedCostCost;
        public double AssociatedCostCost
        {
            get { return _associatedCostCost; }
            set
            {
                _associatedCostCost = value;
                RaisePropertyChanged("AssociatedCostCost");
            }
        }
        private double _associatedCostLabor;
        public double AssociatedCostLabor
        {
            get { return _associatedCostLabor; }
            set
            {
                _associatedCostLabor = value;
                RaisePropertyChanged("AssociatedCostLabor");
            }
        }

        private string _panelTypeName;
        public string PanelTypeName
        {
            get { return _panelTypeName; }
            set
            {
                _panelTypeName = value;
                RaisePropertyChanged("PanelTypeName");
            }
        }
        private double _panelTypeCost;
        public double PanelTypeCost
        {
            get { return _panelTypeCost; }
            set
            {
                _panelTypeCost = value;
                RaisePropertyChanged("PanelTypeCost");
            }
        }
        private double _panelTypeLabor;
        public double PanelTypeLabor
        {
            get { return _panelTypeLabor; }
            set
            {
                _panelTypeLabor = value;
                RaisePropertyChanged("PanelTypeLabor");
            }
        }
        private TECManufacturer _panelTypeManufacturer;
        public TECManufacturer PanelTypeManufacturer
        {
            get { return _panelTypeManufacturer; }
            set
            {
                _panelTypeManufacturer = value;
                RaisePropertyChanged("PanelTypeManufacturer");
            }
        }

        private string _ioModuleName;
        public string IOModuleName
        {
            get { return _ioModuleName; }
            set
            {
                _ioModuleName = value;
                RaisePropertyChanged("IOModuleName");
            }
        }
        private string _ioModuleDescription;
        public string IOModuleDescription
        {
            get { return _ioModuleDescription; }
            set
            {
                _ioModuleDescription = value;
                RaisePropertyChanged("IOModuleDescription");
            }
        }
        private double _ioModuleCCost;
        public double IOModuleCost
        {
            get { return _ioModuleCCost; }
            set
            {
                _ioModuleCCost = value;
                RaisePropertyChanged("IOModuleCost");
            }
        }
        private TECManufacturer _ioModuleManufacturer;
        public TECManufacturer IOModuleManufacturer
        {
            get { return _ioModuleManufacturer; }
            set
            {
                _ioModuleManufacturer = value;
                RaisePropertyChanged("IOModuleManufacturer");
            }
        }

        private TECPanelType _selectedPanelType;
        public TECPanelType SelectedPanelType
        {
            get { return _selectedPanelType; }
            set
            {
                _selectedPanelType = value;
                RaisePropertyChanged("SelectedPanelType");
                SelectionChanged?.Invoke(value);
            }
        }

        private string _deviceName;
        public string DeviceName
        {
            get { return _deviceName; }
            set {
                _deviceName = value;
                RaisePropertyChanged("DeviceName");
            }
        }
        private double _deviceListPrice;
        public double DeviceListPrice
        {
            get { return _deviceListPrice; }
            set
            {
                _deviceListPrice = value;
                RaisePropertyChanged("DeviceListPrice");
            }
        }
        private double _deviceLabor;
        public double DeviceLabor
        {
            get { return _deviceLabor; }
            set
            {
                _deviceLabor = value;
                RaisePropertyChanged("DeviceLabor");
            }
        }

        #region Command Properties
        public ICommand AddConnectionTypeCommand { get; private set; }
        public ICommand AddConduitTypeCommand { get; private set; }
        public ICommand AddAssociatedCostCommand { get; private set; }
        public ICommand AddPanelTypeCommand { get; private set; }
        public ICommand AddIOModuleCommand { get; private set; }
        public ICommand AddDeviceCommand { get; private set; }
        #endregion

        #region Delegates
        public Action<IDropInfo> DragHandler;
        public Action<IDropInfo> DropHandler;

        public Action<Object> SelectionChanged;
        #endregion

        #region ViewModels
        public MiscCostsVM MiscVM { get; private set; }
        #endregion

        #endregion

        public MaterialVM(TECTemplates templates)
        {
            Templates = templates;
            setupCommands();
            setupInterfaceDefaults();
            setupVMs();
        }
        
        #region Methods
        public void Refresh(TECTemplates templates)
        {
            Templates = templates;
            MiscVM.Refresh(templates);
            setupInterfaceDefaults();
        }

        private void setupCommands()
        {
            AddConnectionTypeCommand = new RelayCommand(addConnectionTypeExecute);
            AddConduitTypeCommand = new RelayCommand(addConduitTypeExecute);
            AddAssociatedCostCommand = new RelayCommand(addAsociatedCostExecute, canAddAssociatedCost);
            AddPanelTypeCommand = new RelayCommand(addPanelTypeExecute, canAddPanelTypeExecute);
            AddIOModuleCommand = new RelayCommand(addIOModuleExecute, canAddIOModuleExecute);
        }

        private void addConnectionTypeExecute()
        {
            var connectionType = new TECElectricalMaterial();
            connectionType.Name = ConnectionTypeName;
            connectionType.Cost = ConnectionTypeCost;
            connectionType.Labor = ConnectionTypeLabor;
            Templates.Catalogs.ConnectionTypes.Add(connectionType);
            ConnectionTypeName = "";
            ConnectionTypeCost = 0;
            ConnectionTypeLabor = 0;
        }
        private void addConduitTypeExecute()
        {
            var conduitType = new TECElectricalMaterial();
            conduitType.Name = ConduitTypeName;
            conduitType.Cost = ConduitTypeCost;
            conduitType.Labor = ConduitTypeLabor;
            Templates.Catalogs.ConduitTypes.Add(conduitType);
            ConduitTypeName = "";
            ConduitTypeCost = 0;
            ConduitTypeLabor = 0;
        }
        private void addAsociatedCostExecute()
        {
            var associatedCost = new TECCost(AssociatedCostType);
            associatedCost.Name = AssociatedCostName;
            associatedCost.Cost = AssociatedCostCost;
            associatedCost.Labor = AssociatedCostLabor;
            Templates.Catalogs.AssociatedCosts.Add(associatedCost);
            AssociatedCostName = "";
            AssociatedCostCost = 0;
            AssociatedCostLabor = 0;
        }
        private bool canAddAssociatedCost()
        {
            if (AssociatedCostName == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private void addPanelTypeExecute()
        {
            var panelType = new TECPanelType(PanelTypeManufacturer);
            panelType.Name = PanelTypeName;
            panelType.Price = PanelTypeCost;
            panelType.Labor = PanelTypeLabor;

            Templates.Catalogs.PanelTypes.Add(panelType);
            PanelTypeName = "";
            PanelTypeCost = 0;
            PanelTypeLabor = 0;
            PanelTypeManufacturer = null;
        }
        private bool canAddPanelTypeExecute()
        {
            if (PanelTypeName != "" && PanelTypeManufacturer != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void addIOModuleExecute()
        {
            var ioModule = new TECIOModule(IOModuleManufacturer);
            ioModule.Name = IOModuleName;
            ioModule.Price = IOModuleCost;
            ioModule.Description = IOModuleDescription;

            Templates.Catalogs.IOModules.Add(ioModule);
            IOModuleName = "";
            IOModuleDescription = "";
            IOModuleCost = 0;
            IOModuleManufacturer = null;
        }
        private bool canAddIOModuleExecute()
        {
            if (IOModuleName != "" && IOModuleManufacturer != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            DragHandler(dropInfo);
        }

        public void Drop(IDropInfo dropInfo)
        {
            DropHandler(dropInfo);
        }

        private void setupInterfaceDefaults()
        {
            ConnectionTypeName = "";
            ConnectionTypeCost = 0;
            ConnectionTypeLabor = 0;

            ConduitTypeName = "";
            ConduitTypeCost = 0;
            ConduitTypeLabor = 0;

            AssociatedCostName = "";
            AssociatedCostCost = 0;
            AssociatedCostLabor = 0;

            PanelTypeName = "";
            PanelTypeCost = 0;

            IOModuleName = "";
            IOModuleDescription = "";
            IOModuleCost = 0;
        }

        private void setupVMs()
        {
            MiscVM = new MiscCostsVM(Templates);
        }
        #endregion
    }

}