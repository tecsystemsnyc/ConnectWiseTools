using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModelExtensions
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class EditTabExtension : ViewModelBase
    {
        #region Properties
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
        #region CommandProperties
        public ICommand AddTagToSystemCommand { get; private set; }
        public ICommand AddTagToEquipmentCommand { get; private set; }
        public ICommand AddTagToSubScopeCommand { get; private set; }
        public ICommand AddTagToDeviceCommand { get; private set; }
        public ICommand AddTagToPointCommand { get; private set; }
        public ICommand AddTagToControllerCommand { get; private set; }
        public ICommand AddIOToControllerCommand { get; private set; }
        public ICommand DeleteSelectedSystemCommand { get; private set; }
        public ICommand DeleteSelectedEquipmentCommand { get; private set; }
        public ICommand DeleteSelectedSubScopeCommand { get; private set; }
        public ICommand DeleteSelectedDeviceCommand { get; private set; }
        public ICommand DeleteSelectedPointCommand { get; private set; }
        public ICommand DeleteSelectedControllerCommand { get; private set; }
        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the EditTabExtension class.
        /// </summary>
        public EditTabExtension(TECTemplates templates)
        {
            Templates = templates;
            setupCommands();
            TabIndex = EditIndex.Nothing;
        }

        public EditTabExtension(TECBid bid)
        {
            Bid = bid;
            setupCommands();
            TabIndex = EditIndex.Nothing;
        }

        #region Methods

        private void setupCommands()
        {
            AddTagToSystemCommand = new RelayCommand(AddTagToSystemExecute);
            AddTagToEquipmentCommand = new RelayCommand(AddTagToEquipmentExecute);
            AddTagToSubScopeCommand = new RelayCommand(AddTagToSubScopeExecute);
            AddTagToDeviceCommand = new RelayCommand(AddTagToDeviceExecute);
            AddTagToPointCommand = new RelayCommand(AddTagToPointExecute);
            AddTagToControllerCommand = new RelayCommand(AddTagToControllerExecute);
            AddIOToControllerCommand = new RelayCommand(AddIOToControllerExecute);
            DeleteSelectedSystemCommand = new RelayCommand(DeleteSelectedSystemExecute);
            DeleteSelectedEquipmentCommand = new RelayCommand(DeleteSelectedEquipmentExecute);
            DeleteSelectedSubScopeCommand = new RelayCommand(DeleteSelectedSubScopeExecute);
            DeleteSelectedDeviceCommand = new RelayCommand(DeleteSelectedDeviceExecute);
            DeleteSelectedPointCommand = new RelayCommand(DeleteSelectedPointExecute);
            DeleteSelectedControllerCommand = new RelayCommand(DeleteSelectedControllerExecute);
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
        private void AddIOToControllerExecute()
        {
            var newIO = new TECIO();
            newIO.Type = ControllerIO;
            newIO.Quantity = ControllerIOQTY;
            SelectedController.IO.Add(newIO);

            ControllerIOQTY = 1;
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
                SelectedDevice = selection as TECDevice;
            }
            else if (selection is TECPoint)
            {
                SelectedPoint = selection as TECPoint;
            }
            else if (selection is TECController)
            {
                SelectedController = selection as TECController;
            } else
            {
                TabIndex = EditIndex.Nothing;
            }
        }
        #endregion

        #endregion
    }
}