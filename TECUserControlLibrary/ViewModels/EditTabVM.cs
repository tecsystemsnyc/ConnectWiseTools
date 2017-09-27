using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class EditTabVM : ViewModelBase, IDropTarget
    {
        #region Properties
        private bool isBid;
        private TECObject _selected;
        public TECObject Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                RaisePropertyChanged("Selected");
            }
        }

        public EditIndex TabIndex
        {
            get { return _TabIndex; }
            set
            {
                _TabIndex = value;
                RaisePropertyChanged("TabIndex");
            }
        }
        private EditIndex _TabIndex;

        public TECTemplates Templates
        {
            get { return _templates; }
            set
            {
                _templates = value;
                setCatalogs(value);
                RaisePropertyChanged("Templates");
            }
        }
        private TECTemplates _templates;

        public TECBid Bid
        {
            get { return _bid; }
            set
            {
                _bid = value;
                setCatalogs(value);
                RaisePropertyChanged("Bid");
            }
        }
        private TECBid _bid;

        public TECTypical SelectedSystem
        {
            get
            {
                return _selectedSystem;
            }
            set
            {
                _selectedSystem = value;
                RaisePropertyChanged("SelectedSystem");
                TabIndex = EditIndex.System;
            }
        }
        private TECTypical _selectedSystem;

        public TECEquipment SelectedEquipment
        {
            get
            {
                return _selectedEquipment;
            }
            set
            {
                _selectedEquipment = value;
                RaisePropertyChanged("SelectedEquipment");
                TabIndex = EditIndex.Equipment;
            }
        }
        private TECEquipment _selectedEquipment;

        public TECSubScope SelectedSubScope
        {
            get
            {
                return _selectedSubScope;
            }
            set
            {
                _selectedSubScope = value;
                RaisePropertyChanged("SelectedSubscope");
                TabIndex = EditIndex.SubScope;
            }
        }
        private TECSubScope _selectedSubScope;

        public TECDevice SelectedDevice
        {
            get
            {
                return _selectedDevice;
            }
            set
            {
                _selectedDevice = value;
                RaisePropertyChanged("SelectedDevice");
                TabIndex = EditIndex.Device;
            }
        }
        private TECDevice _selectedDevice;

        public TECPoint SelectedPoint
        {
            get { return _selectedPoint; }
            set
            {
                _selectedPoint = value;
                RaisePropertyChanged("SelectedPoint");
                TabIndex = EditIndex.Point;
            }
        }
        private TECPoint _selectedPoint;

        public TECLabeled SelectedTag
        {
            get { return _selectedTag; }
            set
            {
                _selectedTag = value;
                RaisePropertyChanged("SelectedTag");
            }
        }
        private TECLabeled _selectedTag;

        private TECController _selectedController;
        public TECController SelectedController
        {
            get { return _selectedController; }
            set
            {
                _selectedController = value;
                RaisePropertyChanged("SelectedController");
                TabIndex = EditIndex.Controller;
            }
        }

        private TECPanel _selectedPanel;
        public TECPanel SelectedPanel
        {
            get { return _selectedPanel; }
            set
            {
                _selectedPanel = value;
                RaisePropertyChanged("SelectedPanel");
                TabIndex = EditIndex.Panel;
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
                TabIndex = EditIndex.PanelType;
            }
        }

        private IOType _controllerIO;
        public IOType ControllerIO
        {
            get { return _controllerIO; }
            set
            {
                _controllerIO = value;
                RaisePropertyChanged("ControllerIO");
            }
        }

        private int _controllerIOQTY;
        public int ControllerIOQTY
        {
            get { return _controllerIOQTY; }
            set
            {
                _controllerIOQTY = value;
                RaisePropertyChanged("ControllerIOQTY");
            }
        }

        private TECCost _selectedAssociatedCost;
        public TECCost SelectedAssociatedCost
        {
            get { return _selectedAssociatedCost; }
            set
            {
                _selectedAssociatedCost = value;
                RaisePropertyChanged("SelectedAssociatedCost");
            }
        }

        private ObservableCollection<TECLabeled> _tagSelections;
        public ObservableCollection<TECLabeled> TagSelections
        {
            get { return _tagSelections; }
            set
            {
                _tagSelections = value;
                RaisePropertyChanged("TagSelections");
            }
        }

        private ObservableCollection<TECCost> _associatedCostSelections;
        public ObservableCollection<TECCost> AssociatedCostSelections
        {
            get { return _associatedCostSelections; }
            set
            {
                _associatedCostSelections = value;
                RaisePropertyChanged("AssociatedCostSelections");
            }
        }

        private ObservableCollection<TECElectricalMaterial> _conduitTypeSelections;
        public ObservableCollection<TECElectricalMaterial> ConduitTypeSelections
        {
            get { return _conduitTypeSelections; }
            set
            {
                _conduitTypeSelections = value;
                RaisePropertyChanged("ConduitTypeSelections");
            }
        }

        private ObservableCollection<TECManufacturer> _manufacturerSelections;
        public ObservableCollection<TECManufacturer> ManufacturerSelections
        {
            get { return _manufacturerSelections; }
            set
            {
                _manufacturerSelections = value;
                RaisePropertyChanged("ManufacturerSelections");
            }
        }

        private ObservableCollection<TECElectricalMaterial> _connectionTypeSelections;
        public ObservableCollection<TECElectricalMaterial> ConnectionTypeSelections
        {
            get { return _connectionTypeSelections; }
            set
            {
                _connectionTypeSelections = value;
                RaisePropertyChanged("ConnectionTypeSelections");
            }
        }

        private TECElectricalMaterial _selectedConnectionType;
        public TECElectricalMaterial SelectedConnectionType
        {
            get { return _selectedConnectionType; }
            set
            {
                _selectedConnectionType = value;
                RaisePropertyChanged("SelectedConnectionType");
            }
        }

        private ObservableCollection<TECIOModule> _ioModuleSelections;
        public ObservableCollection<TECIOModule> IOModuleSelections
        {
            get { return _ioModuleSelections; }
            set
            {
                _ioModuleSelections = value;
                RaisePropertyChanged("IOModuleSelections");
            }
        }

        private ObservableCollection<TECPanelType> _panelTypeSelections;
        public ObservableCollection<TECPanelType> PanelTypeSelections
        {
            get { return _panelTypeSelections; }
            set
            {
                _panelTypeSelections = value;
                RaisePropertyChanged("PanelTypeSelections");
            }
        }

        #region CommandProperties
        public ICommand AddTagToSystemCommand { get; private set; }
        public ICommand AddTagToEquipmentCommand { get; private set; }
        public ICommand AddTagToSubScopeCommand { get; private set; }
        public ICommand AddTagToDeviceCommand { get; private set; }
        public ICommand AddTagToControllerCommand { get; private set; }
        public ICommand AddTagToPanelCommand { get; private set; }
        public ICommand AddTagToPanelTypeCommand { get; private set; }
        //public ICommand AddIOToControllerCommand { get; private set; }
        public ICommand DeleteSelectedSystemCommand { get; private set; }
        public ICommand DeleteSelectedEquipmentCommand { get; private set; }
        public ICommand DeleteSelectedSubScopeCommand { get; private set; }
        public ICommand DeleteSelectedDeviceCommand { get; private set; }
        public ICommand DeleteSelectedPointCommand { get; private set; }
        public ICommand DeleteSelectedControllerCommand { get; private set; }
        public ICommand DeleteSelectedPanelCommand { get; private set; }
        public ICommand DeleteSelectedPanelTypeCommand { get; private set; }
        public ICommand AddAssociatedCostToSystemCommand { get; private set; }
        public ICommand AddAssociatedCostToEquipmentCommand { get; private set; }
        public ICommand AddAssociatedCostToSubScopeCommand { get; private set; }
        public ICommand AddAssociatedCostToDeviceCommand { get; private set; }
        public ICommand AddAssociatedCostToControllerCommand { get; private set; }
        public ICommand AddAssociatedCostToPanelCommand { get; private set; }
        public ICommand AddAssociatedCostToPanelTypeCommand { get; private set; }
        public ICommand AddConnectionTypeToDeviceCommand { get; private set; }
        
        #endregion

        public Action<IDropInfo> DragHandler;
        public Action<IDropInfo> DropHandler;
        #endregion

        /// <summary>
        /// Initializes a new instance of the EditTabExtension class.
        /// </summary>
        public EditTabVM(TECTemplates templates)
        {
            Templates = templates;
            setCatalogs(Templates);
            setupCommands();
            isBid = false;
            TabIndex = EditIndex.Nothing;
        }
        public EditTabVM(TECBid bid)
        {
            Bid = bid;
            setCatalogs(Bid);
            setupCommands();
            isBid = true;
            TabIndex = EditIndex.Nothing;
        }

        #region Methods
        public void Refresh(TECBid bid)
        {
            refresh(bid);
        }
        public void Refresh(TECTemplates templates)
        {
            refresh(templates);
        }
        private void refresh(object bidOrTemplates)
        {
            TabIndex = EditIndex.Nothing;
            if (bidOrTemplates is TECBid)
            {
                var bid = bidOrTemplates as TECBid;
                Bid = bid;
                isBid = true;
            }
            else if (bidOrTemplates is TECTemplates)
            {
                var templates = bidOrTemplates as TECTemplates;
                Templates = templates;
                isBid = false;
            }
            setCatalogs(bidOrTemplates);
        }
        private bool addIOCanExecute()
        {
            bool hasIO = false;
            if (SelectedController != null && ControllerIO != 0)
            {
                foreach (TECIO io in SelectedController.Type.IO)
                {
                    if (io.Type == ControllerIO)
                    {
                        hasIO = true;
                        break;
                    }
                }
                return !hasIO;
            }
            else
            {
                return false;
            }
        }

        private void setupCommands()
        {
            AddTagToSystemCommand = new RelayCommand(AddTagToSystemExecute, CanAddTagToSystem);
            AddTagToEquipmentCommand = new RelayCommand(AddTagToEquipmentExecute, CanAddTagToEquipment);
            AddTagToSubScopeCommand = new RelayCommand(AddTagToSubScopeExecute, CanAddTagToSubScope);
            AddTagToDeviceCommand = new RelayCommand(AddTagToDeviceExecute, CanAddTagToDevice);
            AddTagToControllerCommand = new RelayCommand(AddTagToControllerExecute, CanAddTagToController);
            AddTagToPanelCommand = new RelayCommand(AddTagToPanelExecute, CanAddTagToPanel);
            AddTagToPanelTypeCommand = new RelayCommand(AddTagToPanelTypeExecute, CanAddTagToPanelType);

            //AddIOToControllerCommand = new RelayCommand(AddIOToControllerExecute, addIOCanExecute);

            DeleteSelectedSystemCommand = new RelayCommand(DeleteSelectedSystemExecute);
            DeleteSelectedEquipmentCommand = new RelayCommand(DeleteSelectedEquipmentExecute);
            DeleteSelectedSubScopeCommand = new RelayCommand(DeleteSelectedSubScopeExecute);
            DeleteSelectedDeviceCommand = new RelayCommand(DeleteSelectedDeviceExecute);
            DeleteSelectedPointCommand = new RelayCommand(DeleteSelectedPointExecute);
            DeleteSelectedControllerCommand = new RelayCommand(DeleteSelectedControllerExecute);
            DeleteSelectedPanelCommand = new RelayCommand(DeleteSelectedPanelExecute);
            DeleteSelectedPanelTypeCommand = new RelayCommand(DeleteSelectedPanelTypeExecute);

            AddAssociatedCostToSystemCommand = new RelayCommand(AddAssociatedCostToSystemExecute, CanAddAssociatedCostToSystem);
            AddAssociatedCostToEquipmentCommand = new RelayCommand(AddAssociatedCostToEquipmentExecute, CanAddAssociatedCostToEquipment);
            AddAssociatedCostToSubScopeCommand = new RelayCommand(AddAssociatedCostToSubScopeExecute, CanAddAssociatedCostToSubScope);
            AddAssociatedCostToDeviceCommand = new RelayCommand(AddAssociatedCostToDeviceExecute, CanAddAssociatedCostToDevice);
            AddAssociatedCostToControllerCommand = new RelayCommand(AddAssociatedCostToControllerExecute, CanAddAssociatedCostToController);
            AddAssociatedCostToPanelCommand = new RelayCommand(AddAssociatedCostToPanelExecute, CanAddAssociatedCostToPanel);
            AddAssociatedCostToPanelTypeCommand = new RelayCommand(AddAssociatedCostToPanelTypeExecute, CanAddAssociatedCostToPanelType);

            AddConnectionTypeToDeviceCommand = new RelayCommand(AddConnectionTypeToDeviceExecute, CanAddConnectionTypeToDevice);
        }
        
        private bool CanAddConnectionTypeToDevice()
        {
            if(SelectedConnectionType != null)
            {
                return true;
            } else
            {
                return false;
            }
        }

        private void AddConnectionTypeToDeviceExecute()
        {
            SelectedDevice.ConnectionTypes.Add(SelectedConnectionType);
        }

        private void setCatalogs(object type)
        {
            if (type is TECBid)
            {
                TagSelections = Bid.Catalogs.Tags;
                ConduitTypeSelections = Bid.Catalogs.ConduitTypes;
                AssociatedCostSelections = Bid.Catalogs.AssociatedCosts;
                ManufacturerSelections = Bid.Catalogs.Manufacturers;
                ConnectionTypeSelections = Bid.Catalogs.ConnectionTypes;
                IOModuleSelections = Bid.Catalogs.IOModules;
                PanelTypeSelections = Bid.Catalogs.PanelTypes;
            }
            else if (type is TECTemplates)
            {
                TagSelections = Templates.Catalogs.Tags;
                AssociatedCostSelections = Templates.Catalogs.AssociatedCosts;
                ConduitTypeSelections = Templates.Catalogs.ConduitTypes;
                ManufacturerSelections = Templates.Catalogs.Manufacturers;
                ConnectionTypeSelections = Templates.Catalogs.ConnectionTypes;
                IOModuleSelections = Templates.Catalogs.IOModules;
                PanelTypeSelections = Templates.Catalogs.PanelTypes;
            }

        }

        #region Commands
        
        private void AddTagToSystemExecute()
        {
            SelectedSystem.Tags.Add(SelectedTag);
        }
        private bool CanAddTagToSystem()
        {
            if (SelectedTag != null
                && SelectedSystem != null
                && !SelectedSystem.Tags.Contains(SelectedTag))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void AddTagToEquipmentExecute()
        {
            SelectedEquipment.Tags.Add(SelectedTag);
        }
        private bool CanAddTagToEquipment()
        {
            if (SelectedTag != null
                && SelectedEquipment != null
                && !SelectedEquipment.Tags.Contains(SelectedTag))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void AddTagToSubScopeExecute()
        {
            SelectedSubScope.Tags.Add(SelectedTag);
        }
        private bool CanAddTagToSubScope()
        {
            if (SelectedTag != null
                && SelectedSubScope != null
                && !SelectedSubScope.Tags.Contains(SelectedTag))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void AddTagToDeviceExecute()
        {
            SelectedDevice.Tags.Add(SelectedTag);
        }
        private bool CanAddTagToDevice()
        {
            if (SelectedTag != null
                && SelectedDevice != null
                && !SelectedDevice.Tags.Contains(SelectedTag))
            {
                return true;
            }
            else
            {
                return false;
            }
        } 

        private void AddTagToControllerExecute()
        {
            SelectedController.Tags.Add(SelectedTag);
        }
        private bool CanAddTagToController()
        {
            if (SelectedTag != null
                && SelectedController != null
                && !SelectedController.Tags.Contains(SelectedTag))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void AddTagToPanelExecute()
        {
            SelectedPanel.Tags.Add(SelectedTag);
        }
        private bool CanAddTagToPanel()
        {
            if (SelectedTag != null
                && SelectedPanel != null
                && !SelectedPanel.Tags.Contains(SelectedTag))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void AddTagToPanelTypeExecute()
        {
            SelectedPanelType.Tags.Add(SelectedTag);
        }
        private bool CanAddTagToPanelType()
        {
            if (SelectedTag != null
                && SelectedPanelType != null
                && !SelectedPanelType.Tags.Contains(SelectedTag))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //private void AddIOToControllerExecute()
        //{
        //    var newIO = new TECIO();
        //    newIO.Type = ControllerIO;
        //    newIO.Quantity = ControllerIOQTY;
        //    SelectedController.IO.Add(newIO);

        //    ControllerIOQTY = 1;
        //    ControllerIO = 0;
        //}
        private void DeleteSelectedSystemExecute()
        {
            if (Templates != null)
            {
                Templates.SystemTemplates.Remove(SelectedSystem);
            }
            else if (Bid != null)
            {
                Bid.Systems.Remove(SelectedSystem);
            }

            SelectedSystem = null;
        }
        private void DeleteSelectedEquipmentExecute()
        {
            if (SelectedSystem != null)
            {
                SelectedSystem.Equipment.Remove(SelectedEquipment);
            }
            else if (Templates != null)
            {
                Templates.EquipmentTemplates.Remove(SelectedEquipment);
            }
            SelectedEquipment = null;
        }
        private void DeleteSelectedSubScopeExecute()
        {
            if (SelectedEquipment != null)
            {
                SelectedEquipment.SubScope.Remove(SelectedSubScope);
            }
            else if (Templates != null)
            {
                Templates.SubScopeTemplates.Remove(SelectedSubScope);
            }
            SelectedSubScope = null;
        }
        private void DeleteSelectedDeviceExecute()
        {
            if (SelectedSubScope != null)
            {
                SelectedSubScope.Devices.Remove(SelectedDevice);
            }
            else if (Templates != null)
            {
                Templates.Catalogs.Devices.Remove(SelectedDevice);
            }
            SelectedDevice = null;
        }
        private void DeleteSelectedPointExecute()
        {
            if (SelectedSubScope != null)
            {
                SelectedSubScope.Points.Remove(SelectedPoint);
            }
            SelectedPoint = null;
        }
        private void DeleteSelectedControllerExecute()
        {
            if (SelectedController != null && Templates != null)
            {
                Templates.ControllerTemplates.Remove(SelectedController);
            }
            else if (SelectedController != null && Bid != null)
            {
                Bid.Controllers.Remove(SelectedController);
            }
            SelectedController = null;
        }
        private void DeleteSelectedPanelExecute()
        {
            if (SelectedPanel != null && Templates != null)
            {
                Templates.PanelTemplates.Remove(SelectedPanel);
            }
            else if (SelectedPanel != null && Bid != null)
            {
                Bid.Panels.Remove(SelectedPanel);
            }
            SelectedPanel = null;
        }
        private void DeleteSelectedPanelTypeExecute()
        {
            if (SelectedPanelType != null && Templates != null)
            {
                Templates.Catalogs.PanelTypes.Remove(SelectedPanelType);
            }
            else if (SelectedPanelType != null && Bid != null)
            {
                Bid.Catalogs.PanelTypes.Remove(SelectedPanelType);
            }
            SelectedPanelType = null;
        }

        private void AddAssociatedCostToSystemExecute()
        {

            SelectedSystem.AssociatedCosts.Add(SelectedAssociatedCost);

        }
        private bool CanAddAssociatedCostToSystem()
        {
            if (SelectedAssociatedCost != null && SelectedSystem != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void AddAssociatedCostToEquipmentExecute()
        {
            SelectedEquipment.AssociatedCosts.Add(SelectedAssociatedCost);
        }
        private bool CanAddAssociatedCostToEquipment()
        {
            if (SelectedAssociatedCost != null && SelectedEquipment != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void AddAssociatedCostToSubScopeExecute()
        {
            SelectedSubScope.AssociatedCosts.Add(SelectedAssociatedCost);
        }
        private bool CanAddAssociatedCostToSubScope()
        {
            if (SelectedAssociatedCost != null && SelectedSubScope != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void AddAssociatedCostToDeviceExecute()
        {
            SelectedDevice.AssociatedCosts.Add(SelectedAssociatedCost);
        }
        private bool CanAddAssociatedCostToDevice()
        {
            if (SelectedAssociatedCost != null && SelectedDevice != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void AddAssociatedCostToControllerExecute()
        {
            SelectedController.AssociatedCosts.Add(SelectedAssociatedCost);
        }
        private bool CanAddAssociatedCostToController()
        {
            if (SelectedAssociatedCost != null && SelectedController != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void AddAssociatedCostToPanelExecute()
        {
            SelectedPanel.AssociatedCosts.Add(SelectedAssociatedCost);
        }
        private bool CanAddAssociatedCostToPanel()
        {
            if (SelectedAssociatedCost != null && SelectedPanel != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void AddAssociatedCostToPanelTypeExecute()
        {
            SelectedPanelType.AssociatedCosts.Add(SelectedAssociatedCost);
        }
        private bool CanAddAssociatedCostToPanelType()
        {
            if (SelectedAssociatedCost != null && SelectedPanelType != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Events
        public void updateSelection(object selection)
        {
            Selected = selection as TECObject;
            if (selection is TECTypical typical)
            {
                SelectedSystem = typical;
            }
            else if (selection is TECEquipment)
            {
                SelectedEquipment = selection as TECEquipment;
            }
            else if (selection is TECSubScope)
            {
                SelectedSubScope = selection as TECSubScope;
            }
            else if (selection is TECDevice)
            {
                if (isBid)
                { TabIndex = EditIndex.Nothing; }
                else
                { SelectedDevice = selection as TECDevice; }

            }
            else if (selection is TECPoint)
            {
                SelectedPoint = selection as TECPoint;
            }
            else if (selection is TECController)
            {
                SelectedController = selection as TECController;
            }
            else if (selection is TECPanel)
            {
                SelectedPanel = selection as TECPanel;
            }
            else if (selection is TECPanelType)
            {
                SelectedPanelType = selection as TECPanelType;
            }
            else
            {
                TabIndex = EditIndex.Nothing;
            }
        }
        #endregion

        public void DragOver(IDropInfo dropInfo)
        {
            DragHandler(dropInfo);
        }
        public void Drop(IDropInfo dropInfo)
        {
            DropHandler(dropInfo);
        }

        #endregion
    }
}