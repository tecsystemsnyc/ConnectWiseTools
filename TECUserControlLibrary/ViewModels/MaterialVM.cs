using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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
    public class MaterialVM : ViewModelBase, IDropTarget
    {
        #region Properties
        public ReferenceDropper ReferenceDropHandler { get; }

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

        private TECObject _selected;
        public TECObject Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                RaisePropertyChanged("Selected");
                SelectionChanged?.Invoke(value);
            }
        }

        #region Connection Types
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
        private double _connectionTypePlenumCost;
        public double ConnectionTypePlenumCost
        {
            get { return _connectionTypePlenumCost; }
            set
            {
                _connectionTypePlenumCost = value;
                RaisePropertyChanged("ConnectionTypePlenumCost");
            }
        }
        private double _connectionTypePlenumLabor;
        public double ConnectionTypePlenumLabor
        {
            get { return _connectionTypePlenumLabor; }
            set
            {
                _connectionTypePlenumLabor = value;
                RaisePropertyChanged("ConnectionTypePlenumLabor");
            }
        }
        #endregion
        #region Conduit Types
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
        #endregion
        #region Associated Costs
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
        #endregion
        #region Panel Types
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
        private string _panelTypeDescription;
        public string PanelTypeDescription
        {
            get { return _panelTypeDescription; }
            set
            {
                _panelTypeDescription = value;
                RaisePropertyChanged("PanelTypeDescription");
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
        #endregion
        #region IO Modules
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
        private ObservableCollection<TECIO> _moduleIO;
        public ObservableCollection<TECIO> ModuleIO
        {
            get { return _moduleIO; }
            set
            {
                _moduleIO = value;
                RaisePropertyChanged("ModuleIO");
            }
        }
        public ICommand AddIOToModuleCommand { get; private set; }

        #endregion
        #region Devices
        private string _deviceName;
        public string DeviceName
        {
            get { return _deviceName; }
            set
            {
                _deviceName = value;
                RaisePropertyChanged("DeviceName");
            }
        }
        private string _deviceDescription;
        public string DeviceDescription
        {
            get { return _deviceDescription; }
            set
            {
                _deviceDescription = value;
                RaisePropertyChanged("DeviceDescripiton");
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
        private TECManufacturer _deviceManufacturer;
        public TECManufacturer DeviceManufacturer
        {
            get { return _deviceManufacturer; }
            set
            {
                _deviceManufacturer = value;
                RaisePropertyChanged("DeviceManufacturer");
            }
        }
        private ObservableCollection<TECConnectionType> _deviceConnectionTypes;
        public ObservableCollection<TECConnectionType> DeviceConnectionTypes
        {
            get { return _deviceConnectionTypes; }
            set
            {
                _deviceConnectionTypes = value;
                RaisePropertyChanged("DeviceConnectionTypes");
            }
        }
        #endregion
        #region Valve
        private string _valveName;
        public string ValveName
        {
            get { return _valveName; }
            set
            {
                _valveName = value;
                RaisePropertyChanged("ValveName");
            }
        }
        private string _valveDescription;
        public string ValveDescription
        {
            get { return _valveDescription; }
            set
            {
                _valveDescription = value;
                RaisePropertyChanged("ValveDescripiton");
            }
        }
        private double _valveListPrice;
        public double ValveListPrice
        {
            get { return _valveListPrice; }
            set
            {
                _valveListPrice = value;
                RaisePropertyChanged("ValveListPrice");
            }
        }
        private double _valveLabor;
        public double ValveLabor
        {
            get { return _valveLabor; }
            set
            {
                _valveLabor = value;
                RaisePropertyChanged("ValveLabor");
            }
        }
        private TECManufacturer _valveManufacturer;
        public TECManufacturer ValveManufacturer
        {
            get { return _valveManufacturer; }
            set
            {
                _valveManufacturer = value;
                RaisePropertyChanged("ValveManufacturer");
            }
        }
        private TECDevice _valveActuator;
        public TECDevice ValveActuator
        {
            get { return _valveActuator; }
            set
            {
                _valveActuator = value;
                RaisePropertyChanged("ValveActuator");
            }
        }
        #endregion
        #region Controller Types
        private string _controllerTypeName;
        public string ControllerTypeName
        {
            get { return _controllerTypeName; }
            set
            {
                _controllerTypeName = value;
                RaisePropertyChanged("ControllerTypeName");
            }
        }
        private string _controllerTypeDescription;
        public string ControllerTypeDescription
        {
            get { return _controllerTypeDescription; }
            set
            {
                _controllerTypeDescription = value;
                RaisePropertyChanged("ControllerTypeDescription");
            }
        }
        private double _controllerTypeCost;
        public double ControllerTypeCost
        {
            get { return _controllerTypeCost; }
            set
            {
                _controllerTypeCost = value;
                RaisePropertyChanged("ControllerTypeCost");
            }
        }
        private double _controllerTypeLabor;
        public double ControllerTypeLabor
        {
            get { return _controllerTypeLabor; }
            set
            {
                _controllerTypeLabor = value;
                RaisePropertyChanged("ControllerTypeLabor");
            }
        }
        private TECManufacturer _controllerTypeManufacturer;
        public TECManufacturer ControllerTypeManufacturer
        {
            get { return _controllerTypeManufacturer; }
            set
            {
                _controllerTypeManufacturer = value;
                RaisePropertyChanged("ControllerTypeManufacturer");
            }
        }
        private IOType _selectedIO;
        public IOType SelectedIO
        {
            get { return _selectedIO; }
            set
            {
                _selectedIO = value;
                RaisePropertyChanged("SelectedIO");
            }
        }
        private int _selectedIOQty;
        public int SelectedIOQty
        {
            get { return _selectedIOQty; }
            set
            {
                _selectedIOQty = value;
                RaisePropertyChanged("SelectedIOQty");
            }
        }
        private ObservableCollection<TECIO> _controllerTypeIO;
        public ObservableCollection<TECIO> ControllerTypeIO
        {
            get { return _controllerTypeIO; }
            set
            {
                _controllerTypeIO = value;
                RaisePropertyChanged("ControllerTypeIO");
            }
        }
        private QuantityCollection<TECIOModule> _controllerTypeModules;
        public QuantityCollection<TECIOModule> ControllerTypeModules
        {
            get { return _controllerTypeModules; }
            set
            {
                _controllerTypeModules = value;
                RaisePropertyChanged("ControllerTypeModules");
            }
        } 
        public ICommand AddIOCommand { get; private set; }
        #endregion
        #region Manufacturer
        private TECManufacturer _manufacturerToAdd;
        public TECManufacturer ManufacturerToAdd
        {
            get { return _manufacturerToAdd; }
            set
            {
                _manufacturerToAdd = value;
                RaisePropertyChanged("ManufacturerToAdd");
            }
        }
        #endregion
        #region Tag
        private TECLabeled _tagToAdd;
        public TECLabeled TagToAdd
        {
            get { return _tagToAdd; }
            set
            {
                _tagToAdd = value;
                RaisePropertyChanged("TagToAdd");
            }
        }
        #endregion
       
        #region Command Properties
        public ICommand AddConnectionTypeCommand { get; private set; }
        public ICommand AddConduitTypeCommand { get; private set; }
        public ICommand AddAssociatedCostCommand { get; private set; }
        public ICommand AddPanelTypeCommand { get; private set; }
        public ICommand AddIOModuleCommand { get; private set; }
        public ICommand AddDeviceCommand { get; private set; }
        public ICommand AddControllerTypeCommand { get; private set; }
        public ICommand AddValveCommand { get; private set; }
        public ICommand AddManufacturerCommand { get; private set; }
        public ICommand AddTagCommand { get; private set; }
        #endregion

        #region Delegates
        public Action<Object> SelectionChanged;
        #endregion

        #region ViewModels
        public MiscCostsVM MiscVM { get; private set; }
        #endregion

        #endregion

        public MaterialVM(TECTemplates templates)
        {
            ReferenceDropHandler = new ReferenceDropper(templates);
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
            AddControllerTypeCommand = new RelayCommand(addControllerTypeExecute, canAddControllerType);
            AddValveCommand = new RelayCommand(addValveExecute, canAddValve);
            AddDeviceCommand = new RelayCommand(addDeviceExecute, canAddDevice);
            AddManufacturerCommand = new RelayCommand(addManufacturerExecute, canAddManufacturer);
            AddTagCommand = new RelayCommand(addTagExecute, canAddTag);
            AddIOCommand = new RelayCommand(addIOToControllerTypeExecute, canAddIOToControllerType);
            AddIOToModuleCommand = new RelayCommand(addIOToModuleExecute, canAddIOToModule);
        }

        private void addIOToControllerTypeExecute()
        {
            bool wasAdded = false;
            for (int x = 0; x < SelectedIOQty; x++)
            {
                foreach (TECIO io in ControllerTypeIO)
                {
                    if (io.Type == SelectedIO)
                    {
                        io.Quantity++;
                        wasAdded = true;
                        break;
                    }
                }
                if (!wasAdded)
                {
                    ControllerTypeIO.Add(new TECIO(SelectedIO));
                }
            }
        }
        private bool canAddIOToControllerType()
        {
            return true;
        }

        private void addIOToModuleExecute()
        {
            bool wasAdded = false;
            for(int x = 0; x < SelectedIOQty; x++)
            {
                foreach (TECIO io in ModuleIO)
                {
                    if (io.Type == SelectedIO)
                    {
                        io.Quantity++;
                        wasAdded = true;
                        break;
                    }
                }
                if (!wasAdded)
                {
                    ModuleIO.Add(new TECIO(SelectedIO));
                }
            }
        }
        private bool canAddIOToModule()
        {
            return true;
        }

        private void addDeviceExecute()
        {
            TECDevice toAdd = new TECDevice(DeviceConnectionTypes, DeviceManufacturer);
            toAdd.Name = DeviceName;
            toAdd.Description = DeviceDescription;
            toAdd.Price = DeviceListPrice;
            toAdd.Labor = DeviceLabor;
            Templates.Catalogs.Devices.Add(toAdd);

            DeviceName = "";
            DeviceDescription = "";
            DeviceListPrice = 0;
            DeviceLabor = 0;
            DeviceConnectionTypes = new ObservableCollection<TECConnectionType>();
            DeviceManufacturer = null;
        }
        private bool canAddDevice()
        {
            if(DeviceManufacturer != null)
            {
                return true;
            } else
            {
                return false;
            }
        }
        private void addConnectionTypeExecute()
        {
            var connectionType = new TECConnectionType();
            connectionType.Name = ConnectionTypeName;
            connectionType.Cost = ConnectionTypeCost;
            connectionType.Labor = ConnectionTypeLabor;
            connectionType.PlenumCost = ConnectionTypePlenumCost;
            connectionType.PlenumLabor = ConnectionTypePlenumLabor;
            Templates.Catalogs.ConnectionTypes.Add(connectionType);
            ConnectionTypeName = "";
            ConnectionTypeCost = 0;
            ConnectionTypeLabor = 0;
            ConnectionTypePlenumCost = 0;
            ConnectionTypePlenumLabor = 0;
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
            panelType.Description = PanelTypeDescription;
            panelType.Price = PanelTypeCost;
            panelType.Labor = PanelTypeLabor;

            Templates.Catalogs.PanelTypes.Add(panelType);
            PanelTypeName = "";
            PanelTypeDescription = "";
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
            ioModule.IO = ModuleIO;

            Templates.Catalogs.IOModules.Add(ioModule);
            ModuleIO = new ObservableCollection<TECIO>();
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
        private void addControllerTypeExecute()
        {
            TECControllerType toAdd = new TECControllerType(ControllerTypeManufacturer);
            toAdd.Name = ControllerTypeName;
            toAdd.Description = ControllerTypeDescription;
            toAdd.Price = ControllerTypeCost;
            toAdd.Labor = ControllerTypeLabor;
            ObservableCollection<TECIOModule> ioModules = new ObservableCollection<TECIOModule>();
            foreach(QuantityItem<TECIOModule> quantItem in ControllerTypeModules)
            {
                for(int i = 0; i < quantItem.Quantity; i++)
                {
                    ioModules.Add(quantItem.Item);
                }
            }
            toAdd.IOModules = ioModules;
            toAdd.IO = ControllerTypeIO;

            Templates.Catalogs.ControllerTypes.Add(toAdd);
            ControllerTypeIO = new ObservableCollection<TECIO>();
            ControllerTypeModules = new QuantityCollection<TECIOModule>();
            ControllerTypeName = "";
            ControllerTypeDescription = "";
            ControllerTypeCost = 0;
            ControllerTypeLabor = 0;
            ControllerTypeManufacturer = null;
        }
        private bool canAddControllerType()
        {
            if(ControllerTypeManufacturer != null)
            {
                return true;
            } else
            {
                return false;
            }
        }
        private void addValveExecute()
        {
            TECValve toAdd = new TECValve(ValveManufacturer, ValveActuator);
            toAdd.Name = ValveName;
            toAdd.Description = ValveDescription;
            toAdd.Price = ValveListPrice;
            toAdd.Labor = ValveLabor;
            Templates.Catalogs.Valves.Add(toAdd);

            ValveName = "";
            ValveDescription = "";
            ValveListPrice = 0;
            ValveLabor = 0;
            ValveActuator = null;
            ValveManufacturer = null;
        }
        private bool canAddValve()
        {
            if(ValveActuator != null && ValveManufacturer != null)
            {
                return true;
            } else
            {
                return false;
            }
        }
        private void addManufacturerExecute()
        {
            Templates.Catalogs.Manufacturers.Add(ManufacturerToAdd);
            ManufacturerToAdd = new TECManufacturer();
        }

        private bool canAddManufacturer()
        {
            if(ManufacturerToAdd.Label != "")
            {
                return true;
            } else
            {
                return false;
            }
        }

        private void addTagExecute()
        {
            Templates.Catalogs.Tags.Add(TagToAdd);
            TagToAdd = new TECLabeled();
        }

        private bool canAddTag()
        {
            if(TagToAdd.Label != "")
            {
                return true;
            } else
            {
                return false;
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            UIHelpers.StandardDragOver(dropInfo);
        }

        public void Drop(IDropInfo dropInfo)
        {
            //throw new NotImplementedException("T drop<T> is alway object for some reason.");
            object drop<T>(T item)
            {
                Console.WriteLine(typeof(T));
                bool isCatalog = item.GetType().GetInterfaces().Where(i => i.IsGenericType)
                    .Any(i => i.GetGenericTypeDefinition() == typeof(ICatalog<>));
                if (isCatalog)
                {
                    return ((dynamic)item).CatalogCopy();
                }
                else if (item is IDragDropable dropable)
                {
                    return dropable.DragDropCopy(Templates);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            UIHelpers.StandardDrop(dropInfo, Templates, drop);
        }

        private void setupInterfaceDefaults()
        {
            ConnectionTypeName = "";
            ConnectionTypeCost = 0;
            ConnectionTypeLabor = 0;
            ConnectionTypePlenumCost = 0.0;
            ConnectionTypePlenumLabor = 0.0;

            ConduitTypeName = "";
            ConduitTypeCost = 0;
            ConduitTypeLabor = 0;

            AssociatedCostName = "";
            AssociatedCostCost = 0;
            AssociatedCostLabor = 0;

            PanelTypeName = "";
            PanelTypeDescription = "";
            PanelTypeCost = 0;
            PanelTypeLabor = 0;
            
            ControllerTypeName = "";
            ControllerTypeDescription = "";
            ControllerTypeCost = 0;
            ControllerTypeLabor = 0;
            ControllerTypeIO = new ObservableCollection<TECIO>();
            ControllerTypeModules = new QuantityCollection<TECIOModule>();

            IOModuleName = "";
            IOModuleDescription = "";
            IOModuleCost = 0;
            ModuleIO = new ObservableCollection<TECIO>();
            
            DeviceName = "";
            DeviceDescription = "";
            DeviceListPrice = 0;
            DeviceConnectionTypes = new ObservableCollection<TECConnectionType>();

            ValveName = "";
            ValveDescription = "";
            ValveListPrice = 0;

            ManufacturerToAdd = new TECManufacturer();

            TagToAdd = new TECLabeled();

            
        }

        private void setupVMs()
        {
            MiscVM = new MiscCostsVM(Templates);
        }
        #endregion

        public class ReferenceDropper : IDropTarget
        {
            private TECTemplates templates;

            public ReferenceDropper(TECTemplates templates)
            {
                this.templates = templates;
            }

            public void DragOver(IDropInfo dropInfo)
            {
                UIHelpers.StandardDragOver(dropInfo);
            }

            public void Drop(IDropInfo dropInfo)
            {
                if (dropInfo.Data is TECIOModule module &&
                    dropInfo.TargetCollection is QuantityCollection<TECIOModule> collection)
                {
                    collection.Add(module);
                }
                else
                {
                    UIHelpers.StandardDrop(dropInfo, templates);
                }
            }
        }
    }

}