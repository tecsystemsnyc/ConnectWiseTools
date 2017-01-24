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
using System.Windows.Controls;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModelExtensions
{
    public class ScopeCollectionExtension : ViewModelBase, IDropTarget
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
        
        public int TabIndex
        {
            get { return _tabIndex; }
            set
            {
                _tabIndex = value;
                RaisePropertyChanged("TabIndex");
            }
        }
        private int _tabIndex;

        public string TagName
        {
            get { return _tagName; }
            set
            {
                _tagName = value;
                RaisePropertyChanged("TagName");
            }
        }
        private string _tagName;

        private string _manufacturerName;
        public string ManufacturerName
        {
            get { return _manufacturerName; }
            set
            {
                _manufacturerName = value;
                RaisePropertyChanged("ManufacturerName");
            }
        }

        private double _manufacturerMultiplier;
        public double ManufacturerMultiplier
        {
            get { return _manufacturerMultiplier; }
            set
            {
                _manufacturerMultiplier = value;
                RaisePropertyChanged("ManufacturerMultiplier");
            }
        }

        private TECTag _selectedTag;
        public TECTag SelectedTag
        {
            get { return _selectedTag; }
            set
            {
                _selectedTag = value;
                RaisePropertyChanged("SelectedTag");
            }
        }
        #region Delegates
        public Action<IDropInfo> DragHandler;
        public Action<IDropInfo> DropHandler;
        #endregion

        #region Commands
        public ICommand SearchCollectionCommand { get; private set; }
        public ICommand EndSearchCommand { get; private set; }
        public ICommand AddTagCommand { get; private set; }
        public ICommand AddDeviceCommand { get; private set; }
        public ICommand AddManufacturerCommand { get; private set; }
        public ICommand AddControllerCommand { get; private set; }
        public ICommand AddTypeToControllerCommand { get; private set; }
        public ICommand AddTagToDeviceCommand { get; private set; }
        public ICommand AddTagToControllerCommand { get; private set; }

        #endregion

        #region Visibility Properties
        public Visibility SystemsVisibility
        {
            get { return _SystemsVisibility; }
            set
            {
                _SystemsVisibility = value;
                RaisePropertyChanged("SystemsVisibility");
            }
        }
        private Visibility _SystemsVisibility;

        public Visibility EquipmentVisibility
        {
            get { return _EquipmentVisibility; }
            set
            {
                _EquipmentVisibility = value;
                RaisePropertyChanged("EquipmentVisibility");
            }
        }
        private Visibility _EquipmentVisibility;

        public Visibility SubScopeVisibility
        {
            get { return _SubScopeVisibility; }
            set
            {
                _SubScopeVisibility = value;
                RaisePropertyChanged("SubScopeVisibility");
            }
        }
        private Visibility _SubScopeVisibility;

        public Visibility DevicesVisibility
        {
            get { return _DevicesVisibility; }
            set
            {
                _DevicesVisibility = value;
                RaisePropertyChanged("DevicesVisibility");
            }
        }
        private Visibility _DevicesVisibility;

        public Visibility DevicesEditVisibility
        {
            get { return _DevicesEditVisibility; }
            set
            {
                _DevicesEditVisibility = value;
                RaisePropertyChanged("DevicesEditVisibility");
            }
        }
        private Visibility _DevicesEditVisibility;

        public Visibility ManufacturerVisibility
        {
            get { return _ManufacturerVisibility; }
            set
            {
                _ManufacturerVisibility = value;
                RaisePropertyChanged("ManufacturerVisibility");
            }
        }
        private Visibility _ManufacturerVisibility;

        public Visibility TagsVisibility
        {
            get { return _tagsVisibility; }
            set
            {
                _tagsVisibility = value;
                RaisePropertyChanged("TagsVisibility");
            }
        }
        private Visibility _tagsVisibility;

        public Visibility ControllerEditVisibility
        {
            get { return _controllerEditVisibility; }
            set
            {
                _controllerEditVisibility = value;
                RaisePropertyChanged("ControllerEditVisibility");
            }
        }
        private Visibility _controllerEditVisibility;

        public Visibility ControllerVisibility
        {
            get { return _controllerVisibility; }
            set
            {
                _controllerVisibility = value;
                RaisePropertyChanged("ControllerVisibility");
            }
        }
        private Visibility _controllerVisibility;
        #endregion //Visibility Properties

        #region Device Interface Properties
        public string DeviceName
        {
            get { return _deviceName; }
            set
            {
                _deviceName = value;
                RaisePropertyChanged("DeviceName");
            }
        }
        private string _deviceName;

        public string DeviceDescription
        {
            get { return _deviceDescription; }
            set
            {
                _deviceDescription = value;
                RaisePropertyChanged("DeviceDescription");
            }
        }
        private string _deviceDescription;
        
        public double DeviceCost
        {
            get { return _deviceCost; }
            set
            {
                _deviceCost = value;
                RaisePropertyChanged("DeviceCost");
            }
        }
        private double _deviceCost;

        public ConnectionType DeviceConnectionType
        {
            get { return _deviceConnectionType; }
            set
            {
                _deviceConnectionType = value;
                RaisePropertyChanged("DeviceConnectionType");
            }
        }
        private ConnectionType _deviceConnectionType;

        public string DeviceButtonContent
        {
            get { return _deviceButtonContent; }
            set
            {
                _deviceButtonContent = value;
                RaisePropertyChanged("DeviceButtonContent");
            }
        }
        private string _deviceButtonContent;

        public TECManufacturer DeviceManufacturer
        {
            get { return _deviceManufacturer; }
            set
            {
                _deviceManufacturer = value;
                RaisePropertyChanged("DeviceManufacturer");
            }
        }
        private TECManufacturer _deviceManufacturer;

        public double DeviceMultiplier
        {
            get { return _deviceMultiplier; }
            set
            {
                _deviceMultiplier = value;
                RaisePropertyChanged("DeviceMultiplier");
            }
        }
        private double _deviceMultiplier;

        public ObservableCollection<TECTag> DeviceTags
        {
            get { return _deviceTags; }
            set
            {
                _deviceTags = value;
                RaisePropertyChanged("DeviceTags");
            }
        }
        private ObservableCollection<TECTag> _deviceTags;
        #endregion //Device Interface Properties

        #region Controller Interface Properties
        public string ControllerName
        {
            get { return _controllerName; }
            set
            {
                _controllerName = value;
                RaisePropertyChanged("ControllerName");
            }
        }
        private string _controllerName;

        public string ControllerDescription
        {
            get { return _controllerDescription; }
            set
            {
                _controllerDescription = value;
                RaisePropertyChanged("ControllerDescription");
            }
        }
        private string _controllerDescription;

        public double ControllerCost
        {
            get { return _controllerCost; }
            set
            {
                _controllerCost = value;
                RaisePropertyChanged("ControllerCost");
            }
        }
        private double _controllerCost;

        public ConnectionType ControllerType
        {
            get { return _controllerType; }
            set
            {
                _controllerType = value;
                RaisePropertyChanged("ControllerType");
            }
        }
        private ConnectionType _controllerType;

        public ObservableCollection<ConnectionType> ControllerConnectionTypes
        {
            get { return _controllerConnectionTypes; }
            set
            {
                _controllerConnectionTypes = value;
                RaisePropertyChanged("ControllerConnectionTypes");
            }
        }
        private ObservableCollection<ConnectionType> _controllerConnectionTypes;

        public ObservableCollection<TECTag> ControllerTags
        {
            get { return _controllerTags; }
            set {
                _controllerTags = value;
                RaisePropertyChanged("ControllerTags");
            }
        }
        private ObservableCollection<TECTag> _controllerTags;
        
        #endregion //Device Interface Properties

        #region Scope Collections
        private ObservableCollection<TECSystem> _systemItemsCollection;
        public ObservableCollection<TECSystem> SystemItemsCollection
        {
            get { return _systemItemsCollection; }
            set
            {
                _systemItemsCollection = value;
                RaisePropertyChanged("SystemItemsCollection");
            }

        }

        private ObservableCollection<TECEquipment> _equipmentItemsCollection;
        public ObservableCollection<TECEquipment> EquipmentItemsCollection
        {
            get { return _equipmentItemsCollection; }
            set
            {
                _equipmentItemsCollection = value;
                RaisePropertyChanged("EquipmentItemsCollection");
            }
        }

        private ObservableCollection<TECSubScope> _subScopeItemsCollection;
        public ObservableCollection<TECSubScope> SubScopeItemsCollection
        {
            get { return _subScopeItemsCollection; }
            set
            {
                _subScopeItemsCollection = value;
                RaisePropertyChanged("SubScopeItemsCollection");
            }
        }

        private ObservableCollection<TECDevice> _devicesItemsCollection;
        public ObservableCollection<TECDevice> DevicesItemsCollection
        {
            get { return _devicesItemsCollection; }
            set
            {
                _devicesItemsCollection = value;
                RaisePropertyChanged("DevicesItemsCollection");
            }
        }

        private ObservableCollection<TECController> _controllersItemsCollection;
        public ObservableCollection<TECController> ControllersItemsCollection
        {
            get { return _controllersItemsCollection; }
            set
            {
                _controllersItemsCollection = value;
                RaisePropertyChanged("ControllersItemsCollection");
            }
        }
        #endregion

        #region Search
        private string _searchString;
        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value;
                RaisePropertyChanged("SearchString");
            }
        }
        #endregion
        #endregion

        #region Intializers
        public ScopeCollectionExtension(TECTemplates templates)
        {
            Templates = templates;
            SearchCollectionCommand = new RelayCommand(SearchCollectionExecute);
            EndSearchCommand = new RelayCommand(EndSearchExecute);
            AddTagCommand = new RelayCommand(AddTagExecute);
            AddManufacturerCommand = new RelayCommand(AddManufacturerExecute);
            AddDeviceCommand = new RelayCommand(AddDeviceExecute);
            AddControllerCommand = new RelayCommand(AddControllerExecute);
            AddTypeToControllerCommand = new RelayCommand(AddTypeToControllerExecute);
            AddTagToDeviceCommand = new RelayCommand(AddTagToDeviceExecute);
            AddTagToControllerCommand = new RelayCommand(AddTagToControllerExecute);

            ControllerConnectionTypes = new ObservableCollection<ConnectionType>();

            populateItemsCollections();

            DeviceTags = new ObservableCollection<TECTag>();
            ControllerTags = new ObservableCollection<TECTag>();
        }
        #endregion

        #region Methods

        #region Commands

        private void SearchCollectionExecute()
        {
            Console.WriteLine("here");
            if (SearchString != null)
            {
                switch (TabIndex)
                {
                    case 0:
                        SystemItemsCollection = new ObservableCollection<TECSystem>();
                        foreach (TECSystem item in Templates.SystemTemplates)
                        {
                            if (item.Name.ToUpper().Contains(SearchString.ToUpper()))
                            {
                                Console.WriteLine(item.Name);
                                SystemItemsCollection.Add(item);
                            }
                            foreach (TECTag tag in item.Tags)
                            {
                                if (tag.Text.ToUpper().Contains(SearchString.ToUpper()))
                                {
                                    SystemItemsCollection.Add(item);
                                }
                            }
                        }
                        break;
                    case 1:
                        EquipmentItemsCollection = new ObservableCollection<TECEquipment>();
                        foreach (TECEquipment item in Templates.EquipmentTemplates)
                        {
                            if (item.Name.ToUpper().Contains(SearchString.ToUpper()))
                            {
                                Console.WriteLine(item.Name);
                                EquipmentItemsCollection.Add(item);
                            }
                            foreach (TECTag tag in item.Tags)
                            {
                                if (tag.Text.ToUpper().Contains(SearchString.ToUpper()))
                                {
                                    EquipmentItemsCollection.Add(item);
                                }
                            }
                        }
                        break;
                    case 2:
                        SubScopeItemsCollection = new ObservableCollection<TECSubScope>();
                        foreach (TECSubScope item in Templates.SubScopeTemplates)
                        {
                            if (item.Name.ToUpper().Contains(SearchString.ToUpper()))
                            {
                                Console.WriteLine(item.Name);
                                SubScopeItemsCollection.Add(item);
                            }
                            foreach (TECTag tag in item.Tags)
                            {
                                if (tag.Text.ToUpper().Contains(SearchString.ToUpper()))
                                {
                                    SubScopeItemsCollection.Add(item);
                                }
                            }
                        }
                        break;
                    default:
                        break;

                }
            }
        }
        private void EndSearchExecute()
        {
            populateItemsCollections();
            SearchString = "";
        }
        private void AddTagExecute()
        {
            Console.WriteLine("here");
            TECTag newTag = new TECTag();
            newTag.Text = TagName;
            Templates.Tags.Add(newTag);
        }
        private void AddManufacturerExecute()
        {
            TECManufacturer newMan = new TECManufacturer(ManufacturerName, ManufacturerMultiplier);
            Templates.ManufacturerCatalog.Add(newMan);
        }
        private void AddDeviceExecute()
        {
            var newDevice = new TECDevice(DeviceName, DeviceDescription, DeviceCost, DeviceConnectionType, DeviceManufacturer);
            newDevice.Tags = DeviceTags;
            Templates.DeviceCatalog.Add(newDevice);
            DeviceName = "";
            DeviceDescription = "";
            DeviceCost = 0;
            DeviceConnectionType = 0;
            DeviceManufacturer = null;
            DeviceTags = new ObservableCollection<TECTag>();
        }
        private void AddControllerExecute()
        {
            var newController = new TECController();
            newController.Name = ControllerName;
            newController.Description = ControllerDescription;
            newController.Cost = ControllerCost;
            newController.Types = ControllerConnectionTypes;
            newController.Tags = ControllerTags;
            Templates.ControllerTemplates.Add(newController);
            ControllerName = "";
            ControllerDescription = "";
            ControllerCost = 0;
            ControllerConnectionTypes = new ObservableCollection<ConnectionType>();
            ControllerTags = new ObservableCollection<TECTag>();
        }
        private void AddTypeToControllerExecute()
        {
            ControllerConnectionTypes.Add(ControllerType);
        }
        private void AddTagToDeviceExecute()
        {
            DeviceTags.Add(SelectedTag);
            SelectedTag = null;
        }
        private void AddTagToControllerExecute()
        {
            ControllerTags.Add(SelectedTag);
            SelectedTag = null;
        }

        #endregion

        public void populateItemsCollections()
        {
            SystemItemsCollection = Templates.SystemTemplates;
            EquipmentItemsCollection = Templates.EquipmentTemplates;
            SubScopeItemsCollection = Templates.SubScopeTemplates;
            DevicesItemsCollection = Templates.DeviceCatalog;
            ControllersItemsCollection = Templates.ControllerTemplates;
        }

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
