using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModelExtensions
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class EditTabExtension : ViewModelBase, IDropTarget
    {
        #region Properties
        private bool isBid;

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
        
        public TECSystem SelectedSystem
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
        private TECSystem _selectedSystem;

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

        public TECTag SelectedTag
        {
            get { return _selectedTag; }
            set
            {
                _selectedTag = value;
                RaisePropertyChanged("SelectedTag");
            }
        }
        private TECTag _selectedTag;

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

        private TECAssociatedCost _selectedAssociatedCost;
        public TECAssociatedCost SelectedAssociatedCost
        {
            get { return _selectedAssociatedCost; }
            set
            {
                _selectedAssociatedCost = value;
                RaisePropertyChanged("SelectedAssociatedCost");
            }
        }

        private ObservableCollection<TECTag> _tagSelections;
        public ObservableCollection<TECTag> TagSelections
        {
            get { return _tagSelections; }
            set {
                _tagSelections = value;
                RaisePropertyChanged("TagSelections");
            }
        }

        private ObservableCollection<TECAssociatedCost> _associatedCostSelections;
        public ObservableCollection<TECAssociatedCost> AssociatedCostSelections
        {
            get { return _associatedCostSelections; }
            set
            {
                _associatedCostSelections = value;
                RaisePropertyChanged("AssociatedCostSelections");
            }
        }

        private ObservableCollection<TECConduitType> _conduitTypeSelections;
        public ObservableCollection<TECConduitType> ConduitTypeSelections
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

        private ObservableCollection<TECConnectionType> _connectionTypeSelections;
        public ObservableCollection<TECConnectionType> ConnectionTypeSelections
        {
            get { return _connectionTypeSelections; }
            set
            {
                _connectionTypeSelections = value;
                RaisePropertyChanged("ConnectionTypeSelections");
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
        public ICommand AddTagToPointCommand { get; private set; }
        public ICommand AddTagToControllerCommand { get; private set; }
        public ICommand AddTagToPanelCommand { get; private set; }
        public ICommand AddIOToControllerCommand { get; private set; }
        public ICommand DeleteSelectedSystemCommand { get; private set; }
        public ICommand DeleteSelectedEquipmentCommand { get; private set; }
        public ICommand DeleteSelectedSubScopeCommand { get; private set; }
        public ICommand DeleteSelectedDeviceCommand { get; private set; }
        public ICommand DeleteSelectedPointCommand { get; private set; }
        public ICommand DeleteSelectedControllerCommand { get; private set; }
        public ICommand DeleteSelectedPanelCommand { get; private set; }
        public ICommand AddAssociatedCostToSystemCommand { get; private set; }
        public ICommand AddAssociatedCostToEquipmentCommand { get; private set; }
        public ICommand AddAssociatedCostToSubScopeCommand { get; private set; }
        public ICommand AddAssociatedCostToDeviceCommand { get; private set; }
        public ICommand AddAssociatedCostToControllerCommand { get; private set; }
        public ICommand AddAssociatedCostToPanelCommand { get; private set; }

        #endregion

        public Action<IDropInfo> DragHandler;
        public Action<IDropInfo> DropHandler;
        #endregion

        /// <summary>
        /// Initializes a new instance of the EditTabExtension class.
        /// </summary>
        public EditTabExtension(TECTemplates templates)
        {
            Templates = templates;
            setCatalogs(Templates);
            setupCommands();
            isBid = false;
            TabIndex = EditIndex.Nothing;
        }
        public EditTabExtension(TECBid bid)
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
            } else if (bidOrTemplates is TECTemplates)
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
                foreach (TECIO io in SelectedController.IO)
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
            AddTagToSystemCommand = new RelayCommand(AddTagToSystemExecute);
            AddTagToEquipmentCommand = new RelayCommand(AddTagToEquipmentExecute);
            AddTagToSubScopeCommand = new RelayCommand(AddTagToSubScopeExecute);
            AddTagToDeviceCommand = new RelayCommand(AddTagToDeviceExecute);
            AddTagToPointCommand = new RelayCommand(AddTagToPointExecute);
            AddTagToControllerCommand = new RelayCommand(AddTagToControllerExecute);
            AddTagToPanelCommand = new RelayCommand(AddTagToPanelExecute);
            AddIOToControllerCommand = new RelayCommand(AddIOToControllerExecute, addIOCanExecute);
            DeleteSelectedSystemCommand = new RelayCommand(DeleteSelectedSystemExecute);
            DeleteSelectedEquipmentCommand = new RelayCommand(DeleteSelectedEquipmentExecute);
            DeleteSelectedSubScopeCommand = new RelayCommand(DeleteSelectedSubScopeExecute);
            DeleteSelectedDeviceCommand = new RelayCommand(DeleteSelectedDeviceExecute);
            DeleteSelectedPointCommand = new RelayCommand(DeleteSelectedPointExecute);
            DeleteSelectedControllerCommand = new RelayCommand(DeleteSelectedControllerExecute);
            DeleteSelectedPanelCommand = new RelayCommand(DeleteSelectedPanelExecute);
            AddAssociatedCostToSystemCommand = new RelayCommand(AddAssociatedCostToSystemExecute);
            AddAssociatedCostToEquipmentCommand = new RelayCommand(AddAssociatedCostToEquipmentExecute);
            AddAssociatedCostToSubScopeCommand = new RelayCommand(AddAssociatedCostToSubScopeExecute);
            AddAssociatedCostToDeviceCommand = new RelayCommand(AddAssociatedCostToDeviceExecute);
            AddAssociatedCostToControllerCommand = new RelayCommand(AddAssociatedCostToControllerExecute);
            AddAssociatedCostToPanelCommand = new RelayCommand(AddAssociatedCostToPanelExecute);
        }
        private void setCatalogs(object type)
        {
            if(type is TECBid)
            {
                TagSelections = Bid.Tags;
                ConduitTypeSelections = Bid.ConduitTypes;
                AssociatedCostSelections = Bid.AssociatedCostsCatalog;
                ManufacturerSelections = Bid.ManufacturerCatalog;
                ConnectionTypeSelections = Bid.ConnectionTypes;
                IOModuleSelections = Bid.IOModuleCatalog;
                PanelTypeSelections = Bid.PanelTypeCatalog;
            }
            else if (type is TECTemplates)
            {
                TagSelections = Templates.Tags;
                AssociatedCostSelections = Templates.AssociatedCostsCatalog;
                ConduitTypeSelections = Templates.ConduitTypeCatalog;
                ManufacturerSelections = Templates.ManufacturerCatalog;
                ConnectionTypeSelections = Templates.ConnectionTypeCatalog;
                IOModuleSelections = Templates.IOModuleCatalog;
                PanelTypeSelections = Templates.PanelTypeCatalog;
            }

        }
        
        #region Commands
        private void AddTagToSystemExecute()
        {
            if (SelectedTag != null && SelectedSystem != null)
            {
                SelectedSystem.Tags.Add(SelectedTag);
            }
        }
        private void AddTagToEquipmentExecute()
        {
            if (SelectedTag != null && SelectedEquipment != null)
            {
                SelectedEquipment.Tags.Add(SelectedTag);
            }

        }
        private void AddTagToSubScopeExecute()
        {
            if (SelectedTag != null && SelectedSubScope != null)
            {
                SelectedSubScope.Tags.Add(SelectedTag);
            }
        }
        private void AddTagToDeviceExecute()
        {
            if (SelectedTag != null && SelectedDevice != null)
            {
                SelectedDevice.Tags.Add(SelectedTag);
            }
        }
        private void AddTagToPointExecute()
        {
            if (SelectedTag != null && SelectedPoint != null)
            {
                SelectedPoint.Tags.Add(SelectedTag);
            }
        }
        private void AddTagToControllerExecute()
        {
            if (SelectedTag != null && SelectedController != null)
            {
                SelectedController.Tags.Add(SelectedTag);
            }

        }
        private void AddTagToPanelExecute()
        {
            if (SelectedTag != null && SelectedPanel != null)
            {
                SelectedPanel.Tags.Add(SelectedTag);
            }

        }

        private void AddIOToControllerExecute()
        {
            var newIO = new TECIO();
            newIO.Type = ControllerIO;
            newIO.Quantity = ControllerIOQTY;
            SelectedController.IO.Add(newIO);

            ControllerIOQTY = 1;
            ControllerIO = 0;
        }
        private void DeleteSelectedSystemExecute()
        {
            if(Templates != null)
            {
                Templates.SystemTemplates.Remove(SelectedSystem);
            } else if(Bid != null)
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
                Templates.DeviceCatalog.Remove(SelectedDevice);
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

        private void AddAssociatedCostToSystemExecute()
        {
            if (SelectedAssociatedCost != null && SelectedSystem != null)
            {
                SelectedSystem.AssociatedCosts.Add(SelectedAssociatedCost);
            }
        }
        private void AddAssociatedCostToEquipmentExecute()
        {
            if (SelectedAssociatedCost != null && SelectedEquipment != null)
            {
                SelectedEquipment.AssociatedCosts.Add(SelectedAssociatedCost);
            }

        }
        private void AddAssociatedCostToSubScopeExecute()
        {
            if (SelectedAssociatedCost != null && SelectedSubScope != null)
            {
                SelectedSubScope.AssociatedCosts.Add(SelectedAssociatedCost);
            }
        }
        private void AddAssociatedCostToDeviceExecute()
        {
            if (SelectedAssociatedCost != null && SelectedDevice != null)
            {
                SelectedDevice.AssociatedCosts.Add(SelectedAssociatedCost);
            }
        }
        private void AddAssociatedCostToControllerExecute()
        {
            if (SelectedAssociatedCost != null && SelectedController != null)
            {
                SelectedController.AssociatedCosts.Add(SelectedAssociatedCost);
            }
        }
        private void AddAssociatedCostToPanelExecute()
        {
            if (SelectedAssociatedCost != null && SelectedPanel != null)
            {
                SelectedPanel.AssociatedCosts.Add(SelectedAssociatedCost);
            }
        }
        #endregion

        #region Events
        public void updateSelection(object selection)
        {
            if (selection is TECSystem)
            {
                SelectedSystem = selection as TECSystem;
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
            } else if (selection is TECPanel)
            {
                SelectedPanel = selection as TECPanel;
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