using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using TemplateBuilder.Model;
using TECUserControlLibrary;
using System.Collections;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Reflection;
using System.Deployment.Application;
using System.ComponentModel;

namespace TemplateBuilder.ViewModel
{

    public class MainViewModel : ViewModelBase, IDropTarget
    {
        //Initializer
        public MainViewModel()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                Version = "Version " + ApplicationDeployment.CurrentDeployment.CurrentVersion;
            }
            else
            {
                Version = "Undeployed Version";
            }

            Templates = new TECTemplates();
            ViewData = new MainViewModelData();
            DataGridVisibilty = new VisibilityModel();

            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string resourcesFolder = Path.Combine(appData, APPDATA_FOLDER);

            if (!Directory.Exists(resourcesFolder))
            {
                Directory.CreateDirectory(resourcesFolder);
            }

            defaultTemplatesPath = Path.Combine(resourcesFolder, TEMPLATES_FILE_NAME);

            if (File.Exists(defaultTemplatesPath))
            {
                if (!UtilitiesMethods.IsFileLocked(defaultTemplatesPath))
                {
                    Templates = EstimatingLibraryDatabase.LoadDBToTemplates(defaultTemplatesPath);
                }
                else
                {
                    string message = "TECTemplates file is open elsewhere. Could not load default templates. Please close TECTemplates.tdb and restart program.";
                    MessageBox.Show(message);
                }
            }
            else
            {
                string message = "No template database found. Reload templates to add one.";
                MessageBox.Show(message);
            }

            populateItemsCollections();

            LoadCommand = new RelayCommand(LoadExecute);
            SaveCommand = new RelayCommand(SaveExecute);
            SaveToCommand = new RelayCommand(SaveToExecute);
            AddDeviceCommand = new RelayCommand(AddDeviceExecute);
            AddTagCommand = new RelayCommand(AddTagExecute);
            AddManufacturerCommand = new RelayCommand(AddManufacturerExecute);
            AddTagToSystemCommand = new RelayCommand(AddTagToSystemExecute);
            AddTagToEquipmentCommand = new RelayCommand(AddTagToEquipmentExecute);
            AddTagToSubScopeCommand = new RelayCommand(AddTagToSubScopeExecute);
            AddTagToDeviceCommand = new RelayCommand(AddTagToDeviceExecute);
            AddTagToPointCommand = new RelayCommand(AddTagToPointExecute);
            AddPointCommand = new RelayCommand(AddPointExecute);
            SearchCollectionCommand = new RelayCommand(SearchCollectionExecute);
            EndSearchCommand = new RelayCommand(EndSearchExecute);
            ClosingCommand = new RelayCommand<CancelEventArgs>(e => ClosingExecute(e));

            DeleteSelectedSystemCommand = new RelayCommand(DeleteSelectedSystemExecute);
            DeleteSelectedEquipmentCommand = new RelayCommand(DeleteSelectedEquipmentExecute);
            DeleteSelectedSubScopeCommand = new RelayCommand(DeleteSelectedSubScopeExecute);
            DeleteSelectedDeviceCommand = new RelayCommand(DeleteSelectedDeviceExecute);
            DeleteSelectedPointCommand = new RelayCommand(DeleteSelectedPointExecute);

            UndoCommand = new RelayCommand(UndoExecute, UndoCanExecute);
            RedoCommand = new RelayCommand(RedoExecute, RedoCanExecute);

            setVisibility(0);
            DeviceButtonContent = "Add New";
            DeviceManufacturer = new TECManufacturer();

            TECLogo = Path.GetTempFileName();

            (Properties.Resources.TECLogo).Save(TECLogo, ImageFormat.Png);

            Stack = new ChangeStack(Templates);
        }

        #region Resources Paths
        const string APPDATA_FOLDER = @"TECSystems\";
        const string TEMPLATES_FILE_NAME = @"TECTemplates.tdb";
        #endregion //Resources Paths

        #region Properties
        public string TECLogo { get; set; }

        public string Version { get; set; }

        #region Interface Properties
        public MainViewModelData ViewData
        {
            get { return _viewData; }
            set
            {
                _viewData = value;
                RaisePropertyChanged("ViewData");
            }
        }
        private MainViewModelData _viewData;

        public VisibilityModel DataGridVisibilty
        {
            get { return _dataGridVisibility; }
            set
            {
                _dataGridVisibility = value;
                RaisePropertyChanged("DataGridVisibilty");
            }
        }
        private VisibilityModel _dataGridVisibility;

        #region Selected Object Properties
        public TECTemplates Templates
        {
            get { return _templates; }
            set
            {
                _templates = value;
                Stack = new ChangeStack(value);
                RaisePropertyChanged("Templates");
            }
        }
        private TECTemplates _templates;

        public ChangeStack Stack { get; set; }

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
                RightTabIndex = EditIndex.System;
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
                RightTabIndex = EditIndex.Equipment;
            }
        }
        private TECEquipment _selectedEquipment;

        public TECSubScope SelectedSubScope
        {
            get
            {
                return _selectedSubscope;
            }
            set
            {
                _selectedSubscope = value;
                RaisePropertyChanged("SelectedSubscope");
                RightTabIndex = EditIndex.SubScope;
            }
        }
        private TECSubScope _selectedSubscope;

        public TECDevice SelectedDevice
        {
            get
            {
                return _selectedDevice;
            }
            set
            {
                _selectedDevice = value;
                DeviceButtonContent = "Edit";
                if (value != null)
                {
                    DeviceName = SelectedDevice.Name;
                    DeviceDescription = SelectedDevice.Description;
                    DeviceCost = SelectedDevice.Cost;
                }
                RaisePropertyChanged("SelectedDevice");
                RightTabIndex = EditIndex.Device;
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
                RightTabIndex = EditIndex.Point;
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
        #endregion //Selected Object Properties

        #region Tab Indexes
        public int DGTabIndex
        {
            get
            {
                return _DGTabIndex;
            }
            set
            {
                _DGTabIndex = value;
                setVisibility(_DGTabIndex);
                RaisePropertyChanged("DGTabIndex");
            }
        }
        private int _DGTabIndex;

        public int LeftTabIndex
        {
            get { return _leftTabIndex; }
            set
            {
                _leftTabIndex = value;
                RaisePropertyChanged("LeftTabIndex");
            }
        }
        private int _leftTabIndex;

        public EditIndex RightTabIndex
        {
            get { return _rightTabIndex; }
            set
            {
                _rightTabIndex = value;
                RaisePropertyChanged("RightTabIndex");
            }
        }
        private EditIndex _rightTabIndex;
        #endregion //Tab Indexes

        #region Visibility Properties
        public Visibility LeftSystemsVisibility
        {
            get { return _leftSystemsVisibility; }
            set
            {
                _leftSystemsVisibility = value;
                RaisePropertyChanged("LeftSystemsVisibility");
            }
        }
        private Visibility _leftSystemsVisibility;

        public Visibility LeftEquipmentVisibility
        {
            get { return _leftEquipmentVisibility; }
            set
            {
                _leftEquipmentVisibility = value;
                RaisePropertyChanged("LeftEquipmentVisibility");
            }
        }
        private Visibility _leftEquipmentVisibility;

        public Visibility LeftSubScopeVisibility
        {
            get { return _leftSubScopeVisibility; }
            set
            {
                _leftSubScopeVisibility = value;
                RaisePropertyChanged("LeftSubScopeVisibility");
            }
        }
        private Visibility _leftSubScopeVisibility;

        public Visibility LeftDevicesVisibility
        {
            get { return _leftDevicesVisibility; }
            set
            {
                _leftDevicesVisibility = value;
                RaisePropertyChanged("LeftDevicesVisibility");
            }
        }
        private Visibility _leftDevicesVisibility;

        public Visibility LeftDevicesEditVisibility
        {
            get { return _leftDevicesEditVisibility; }
            set
            {
                _leftDevicesEditVisibility = value;
                RaisePropertyChanged("LeftDevicesEditVisibility");
            }
        }
        private Visibility _leftDevicesEditVisibility;

        public Visibility LeftManufacturerVisibility
        {
            get { return _leftManufacturerVisibility; }
            set
            {
                _leftManufacturerVisibility = value;
                RaisePropertyChanged("LeftManufacturerVisibility");
            }
        }
        private Visibility _leftManufacturerVisibility;
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

        public string DeviceWire
        {
            get { return _deviceWire; }
            set
            {
                _deviceWire = value;
                RaisePropertyChanged("DeviceWire");
            }
        }
        private string _deviceWire;

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
        #endregion //Device Interface Properties

        #region Point Interface Properties
        public string PointName
        {
            get { return _pointName; }
            set
            {
                _pointName = value;
                RaisePropertyChanged("PointName");
            }
        }
        private string _pointName;

        public string PointDescription
        {
            get { return _pointDescription; }
            set
            {
                _pointDescription = value;
                RaisePropertyChanged("PointDescription");
            }
        }
        private string _pointDescription;

        public PointTypes PointType
        {
            get { return _pointType; }
            set
            {
                _pointType = value;
                RaisePropertyChanged("PointType");
            }
        }
        private PointTypes _pointType;

        public int PointQuantity
        {
            get { return _pointQuantity; }
            set
            {
                _pointQuantity = value;
                RaisePropertyChanged("PointQuantity");
            }
        }
        private int _pointQuantity;
        #endregion //Point Interface Properties

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
        #endregion //Interface Properties

        #region Commands Properties
        public ICommand LoadCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand SaveToCommand { get; private set; }
        public ICommand AddDeviceCommand { get; private set; }
        public ICommand AddTagCommand { get; private set; }
        public ICommand AddTagToSystemCommand { get; private set; }
        public ICommand AddTagToEquipmentCommand { get; private set; }
        public ICommand AddTagToSubScopeCommand { get; private set; }
        public ICommand AddTagToDeviceCommand { get; private set; }
        public ICommand AddTagToPointCommand { get; private set; }
        public ICommand AddPointCommand { get; private set; }
        public ICommand SearchCollectionCommand { get; private set; }
        public ICommand EndSearchCommand { get; private set; }
        public ICommand AddManufacturerCommand { get; private set; }
        public ICommand DeleteSelectedSystemCommand { get; private set; }
        public ICommand DeleteSelectedEquipmentCommand { get; private set; }
        public ICommand DeleteSelectedSubScopeCommand { get; private set; }
        public ICommand DeleteSelectedDeviceCommand { get; private set; }
        public ICommand DeleteSelectedPointCommand { get; private set; }
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }

        public RelayCommand<CancelEventArgs> ClosingCommand { get; private set; }
        #endregion //Commands Properties

        string defaultTemplatesPath;
        #endregion //Properties

        #region Methods
        #region Commands Methods
        private void LoadExecute()
        {
            //User choose path
            string path = getLoadPath();

            if (path != null)
            {
                Properties.Settings.Default.TemplateDirectoryPath = path;

                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    File.Copy(path, defaultTemplatesPath, true);
                    Templates = EstimatingLibraryDatabase.LoadDBToTemplates(defaultTemplatesPath);
                    populateItemsCollections();
                }
                else
                {
                    string message = "File is open elsewhere";
                    MessageBox.Show(message);
                }
                Console.WriteLine("Finished loading SQL Database.");
            }
        }
        private void SaveExecute()
        {

            if (!UtilitiesMethods.IsFileLocked(defaultTemplatesPath))
            {
                SaveWindow saveWindow = new SaveWindow();
                
                EstimatingLibraryDatabase.UpdateTemplatesToDB(defaultTemplatesPath, Stack);

                Stack.ClearStacks();

                Console.WriteLine("Finished saving SQL Database.");

                saveWindow.Close();
            }
            else
            {
                string message = "File is open elsewhere";
                MessageBox.Show(message);
            }
        }
        private void SaveToExecute()
        {
            //User choose path
            string path = getSavePath();
            if (path != null)
            {
                Properties.Settings.Default.TemplateDirectoryPath = path;

                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    SaveWindow saveWindow = new SaveWindow();

                    EstimatingLibraryDatabase.SaveTemplatesToNewDB(path, Templates);

                    Stack.ClearStacks();

                    Console.WriteLine("Finished saving SQL Database.");

                    saveWindow.Close();
                }
                else
                {
                    string message = "File is open elsewhere";
                    MessageBox.Show(message);
                }
            }
        }
        private void AddDeviceExecute()
        {
            Templates.DeviceCatalog.Add(new TECDevice(DeviceName, DeviceDescription, DeviceCost, DeviceWire, DeviceManufacturer));
            DeviceName = "";
            DeviceDescription = "";
            DeviceCost = 0;
            DeviceWire = "";
        }
        private void AddTagExecute()
        {
            TECTag newTag = new TECTag(TagName);
            Templates.Tags.Add(newTag);
        }
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
        private void AddPointExecute()
        {
            TECPoint newPoint = new TECPoint();
            newPoint.Name = PointName;
            newPoint.Description = PointDescription;
            newPoint.Type = PointType;
            if (PointType != 0)
            {
                SelectedSubScope.Points.Add(newPoint);
            }
        }
        private void SearchCollectionExecute()
        {
            if (ViewData.SearchString != null)
            {
                switch (LeftTabIndex)
                {
                    case 0:
                        ViewData.SystemItemsCollection = new ObservableCollection<TECSystem>();
                        foreach (TECSystem item in Templates.SystemTemplates)
                        {
                            if (item.Name.ToUpper().Contains(ViewData.SearchString.ToUpper()))
                            {
                                Console.WriteLine(item.Name);
                                ViewData.SystemItemsCollection.Add(item);
                            }
                            foreach (TECTag tag in item.Tags)
                            {
                                if (tag.Text.ToUpper().Contains(ViewData.SearchString.ToUpper()))
                                {
                                    ViewData.SystemItemsCollection.Add(item);
                                }
                            }
                        }
                        break;
                    case 1:
                        ViewData.EquipmentItemsCollection = new ObservableCollection<TECEquipment>();
                        foreach (TECEquipment item in Templates.EquipmentTemplates)
                        {
                            if (item.Name.ToUpper().Contains(ViewData.SearchString.ToUpper()))
                            {
                                Console.WriteLine(item.Name);
                                ViewData.EquipmentItemsCollection.Add(item);
                            }
                            foreach (TECTag tag in item.Tags)
                            {
                                if (tag.Text.ToUpper().Contains(ViewData.SearchString.ToUpper()))
                                {
                                    ViewData.EquipmentItemsCollection.Add(item);
                                }
                            }
                        }
                        break;
                    case 2:
                        ViewData.SubScopeItemsCollection = new ObservableCollection<TECSubScope>();
                        foreach (TECSubScope item in Templates.SubScopeTemplates)
                        {
                            if (item.Name.ToUpper().Contains(ViewData.SearchString.ToUpper()))
                            {
                                Console.WriteLine(item.Name);
                                ViewData.SubScopeItemsCollection.Add(item);
                            }
                            foreach (TECTag tag in item.Tags)
                            {
                                if (tag.Text.ToUpper().Contains(ViewData.SearchString.ToUpper()))
                                {
                                    ViewData.SubScopeItemsCollection.Add(item);
                                }
                            }
                        }
                        break;
                    case 3:
                        ViewData.DevicesItemsCollection = new ObservableCollection<TECDevice>();
                        foreach (TECDevice item in Templates.DeviceCatalog)
                        {
                            if (item.Name.ToUpper().Contains(ViewData.SearchString.ToUpper()))
                            {
                                Console.WriteLine(item.Name);
                                ViewData.DevicesItemsCollection.Add(item);
                            }
                            foreach (TECTag tag in item.Tags)
                            {
                                if (tag.Text.ToUpper().Contains(ViewData.SearchString.ToUpper()))
                                {
                                    ViewData.DevicesItemsCollection.Add(item);
                                }
                            }
                        }
                        break;
                    default:
                        break;

                }
            }
        }
        private void AddManufacturerExecute()
        {
            TECManufacturer newMan = new TECManufacturer(ViewData.ManufacturerName, "", ViewData.ManufacturerMultiplier);
            Templates.ManufacturerCatalog.Add(newMan);
        }

        private void ClosingExecute(CancelEventArgs e)
        {
            bool changes = (Stack.SaveStack.Count > 0);
            if (changes)
            {
                MessageBoxResult result = MessageBox.Show("You have unsaved changes. Would you like to save before quitting?", "Save?", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    SaveExecute();
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                    Console.WriteLine("Closing");
                }
            }
            
        }

        private void DeleteSelectedSystemExecute()
        {
            Templates.SystemTemplates.Remove(SelectedSystem);
            SelectedSystem = null;
            resetEditTab();
        }
        private void DeleteSelectedEquipmentExecute()
        {
            if (SelectedSystem != null)
            {
                SelectedSystem.Equipment.Remove(SelectedEquipment);
            }
            else
            {
                Templates.EquipmentTemplates.Remove(SelectedEquipment);
            }
            SelectedEquipment = null;
            resetEditTab();
        }
        private void DeleteSelectedSubScopeExecute()
        {
            if (SelectedEquipment != null)
            {
                SelectedEquipment.SubScope.Remove(SelectedSubScope);
            }
            else
            {
                Templates.SubScopeTemplates.Remove(SelectedSubScope);
            }
            SelectedSubScope = null;
            resetEditTab();
        }
        private void DeleteSelectedDeviceExecute()
        {
            if (SelectedSubScope != null)
            {
                SelectedSubScope.Devices.Remove(SelectedDevice);
            }
            else
            {
                Templates.DeviceCatalog.Remove(SelectedDevice);
            }
            SelectedDevice = null;
            resetEditTab();
        }
        private void DeleteSelectedPointExecute()
        {
            if (SelectedSubScope != null)
            {
                SelectedSubScope.Points.Remove(SelectedPoint);
            }
            SelectedPoint = null;
            resetEditTab();
        }

        private void EndSearchExecute()
        {
            populateItemsCollections();
            ViewData.SearchString = "";
        }

        private void UndoExecute()
        {
            Stack.Undo();
        }
        private bool UndoCanExecute()
        {
            if (Stack.UndoStack.Count > 0)
                return true;
            else
                return false;
        }

        private void RedoExecute()
        {
            Stack.Redo();
        }
        private bool RedoCanExecute()
        {
            if (Stack.RedoStack.Count > 0)
                return true;
            else
                return false;
        }
        #endregion //Commands Methods

        #region Get Path Methods
        private string getLoadPath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (Properties.Settings.Default.TemplateDirectoryPath != null)
            {
                openFileDialog.InitialDirectory = Properties.Settings.Default.TemplateDirectoryPath;
            }
            else
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            openFileDialog.Filter = "TEC Template Database Files (*.tdb)|*.tdb";
            openFileDialog.DefaultExt = "tdb";
            openFileDialog.AddExtension = true;

            string path = null;

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    path = openFileDialog.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Cannot load this file. Original error: " + ex.Message);
                }
            }

            return path;
        }
        private string getSavePath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (Properties.Settings.Default.TemplateDirectoryPath != null)
            {
                saveFileDialog.InitialDirectory = Properties.Settings.Default.TemplateDirectoryPath;
            }
            else
            {
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            saveFileDialog.Filter = "TEC Template Database Files (*.tdb)|*.tdb";
            saveFileDialog.DefaultExt = "tdb";
            saveFileDialog.AddExtension = true;

            string path = null;

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    path = saveFileDialog.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Cannot save in this location. Original error: " + ex.Message);
                }
            }

            return path;
        }
        #endregion //Get Path Methods

        #region Drag Drop Methods
        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data;
            var targetCollection = dropInfo.TargetCollection;

            if (sourceItem is TECSystem)
            {
                if (sourceItem != null && targetCollection.GetType() == typeof(ObservableCollection<TECSystem>))
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                    dropInfo.Effects = DragDropEffects.Copy;
                }
            }
            else if (sourceItem is TECEquipment)
            {
                if (sourceItem != null && targetCollection.GetType() == typeof(ObservableCollection<TECEquipment>))
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                    dropInfo.Effects = DragDropEffects.Copy;
                }
            }
            else if (sourceItem is TECSubScope)
            {
                if (sourceItem != null && targetCollection.GetType() == typeof(ObservableCollection<TECSubScope>))
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                    dropInfo.Effects = DragDropEffects.Copy;
                }
            }
            else if (sourceItem is TECDevice)
            {
                if (sourceItem != null && targetCollection.GetType() == typeof(ObservableCollection<TECDevice>))
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                    dropInfo.Effects = DragDropEffects.Copy;
                }
            }

        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data;
            if (dropInfo.VisualTarget == dropInfo.DragInfo.VisualSource)
            {
                if (dropInfo.InsertIndex > dropInfo.DragInfo.SourceIndex)
                {
                    if (dropInfo.InsertIndex > ((IList)dropInfo.TargetCollection).Count)
                    {
                        ((IList)dropInfo.TargetCollection).Add(sourceItem);
                    }
                    else
                    {
                        ((IList)dropInfo.TargetCollection).Insert(dropInfo.InsertIndex, sourceItem);
                    }
                    ((IList)dropInfo.DragInfo.SourceCollection).Remove(sourceItem);
                }
                else
                {
                    ((IList)dropInfo.DragInfo.SourceCollection).Remove(sourceItem);
                    ((IList)dropInfo.TargetCollection).Insert(dropInfo.InsertIndex, sourceItem);
                }

            }
            else
            {
                if (sourceItem is TECSystem)
                {
                    TECSystem tempSystem = new TECSystem(sourceItem as TECSystem);
                    int index = dropInfo.InsertIndex;

                    if (index < Templates.SystemTemplates.Count)
                    {
                        Templates.SystemTemplates.Insert(dropInfo.InsertIndex, tempSystem);
                    }
                    else { Templates.SystemTemplates.Add(tempSystem); }

                }
                else if (sourceItem is TECEquipment)
                {
                    TECEquipment tempEquipment = new TECEquipment(sourceItem as TECEquipment);
                    int index = dropInfo.InsertIndex;
                    ObservableCollection<TECEquipment> targetEquipment = dropInfo.TargetCollection as ObservableCollection<TECEquipment>;
                    if (DGTabIndex == 1)
                    {
                        if (index < Templates.EquipmentTemplates.Count)
                        {
                            Templates.EquipmentTemplates.Insert(dropInfo.InsertIndex, tempEquipment);
                        }
                        else
                        {
                            Templates.EquipmentTemplates.Add(tempEquipment);
                        }
                    }
                    else
                    {
                        if (index < targetEquipment.Count)
                        {

                            targetEquipment.Insert(dropInfo.InsertIndex, tempEquipment);
                        }
                        else
                        {
                            targetEquipment.Add(tempEquipment);
                        }
                    }

                }
                else if (sourceItem is TECSubScope)
                {
                    TECSubScope tempSubscope = new TECSubScope(sourceItem as TECSubScope);
                    int index = dropInfo.InsertIndex;
                    ObservableCollection<TECSubScope> targetSubScope = dropInfo.TargetCollection as ObservableCollection<TECSubScope>;

                    if (DGTabIndex == 2)
                    {
                        if (index < Templates.SubScopeTemplates.Count)
                        {
                            Templates.SubScopeTemplates.Insert(dropInfo.InsertIndex, tempSubscope);
                        }
                        else
                        {
                            Templates.SubScopeTemplates.Add(tempSubscope);
                        }
                    }
                    else
                    {
                        if (index < targetSubScope.Count)
                        {
                            targetSubScope.Insert(dropInfo.InsertIndex, tempSubscope);
                        }
                        else
                        {
                            targetSubScope.Add(tempSubscope);
                        }
                    }
                }

                else if (sourceItem is TECDevice)
                {
                    TECDevice tempDevice = new TECDevice(sourceItem as TECDevice);
                    int index = dropInfo.InsertIndex;
                    ObservableCollection<TECDevice> targetDevices = dropInfo.TargetCollection as ObservableCollection<TECDevice>;

                    if (DGTabIndex == 3)
                    {
                        if (index < Templates.DeviceCatalog.Count)
                        {
                            Templates.DeviceCatalog.Insert(dropInfo.InsertIndex, tempDevice);
                        }
                        else
                        {
                            Templates.DeviceCatalog.Add(tempDevice);
                        }
                    }
                    else
                    {
                        if (index < targetDevices.Count)
                        {
                            targetDevices.Insert(dropInfo.InsertIndex, tempDevice);
                        }
                        else
                        {
                            targetDevices.Add(tempDevice);
                        }
                    }

                }

            }
        }
        #endregion

        #region Helper Methods
        private void setVisibility(int gridIndex)
        {
            nullifySelected();

            switch (gridIndex)
            {
                case 0:
                    LeftTabIndex = 0;

                    LeftSystemsVisibility = Visibility.Visible;
                    LeftEquipmentVisibility = Visibility.Visible;
                    LeftSubScopeVisibility = Visibility.Visible;
                    LeftDevicesVisibility = Visibility.Visible;
                    LeftDevicesEditVisibility = Visibility.Collapsed;
                    LeftManufacturerVisibility = Visibility.Collapsed;

                    DataGridVisibilty.SystemQuantity = Visibility.Collapsed;
                    DataGridVisibilty.EquipmentQuantity = Visibility.Visible;
                    DataGridVisibilty.SystemTotalPrice = Visibility.Collapsed;
                    DataGridVisibilty.SubScopeQuantity = Visibility.Visible;
                    break;
                case 1:
                    LeftTabIndex = 1;

                    LeftSystemsVisibility = Visibility.Collapsed;
                    LeftEquipmentVisibility = Visibility.Visible;
                    LeftSubScopeVisibility = Visibility.Visible;
                    LeftDevicesVisibility = Visibility.Visible;
                    LeftDevicesEditVisibility = Visibility.Collapsed;
                    LeftManufacturerVisibility = Visibility.Collapsed;

                    DataGridVisibilty.EquipmentQuantity = Visibility.Collapsed;
                    DataGridVisibilty.SubScopeQuantity = Visibility.Visible;
                    break;
                case 2:
                    LeftTabIndex = 2;

                    LeftSystemsVisibility = Visibility.Collapsed;
                    LeftEquipmentVisibility = Visibility.Collapsed;
                    LeftSubScopeVisibility = Visibility.Visible;
                    LeftDevicesVisibility = Visibility.Visible;
                    LeftDevicesEditVisibility = Visibility.Collapsed;
                    LeftManufacturerVisibility = Visibility.Collapsed;

                    DataGridVisibilty.SubScopeQuantity = Visibility.Collapsed;
                    break;
                case 3:
                    LeftTabIndex = 4;

                    LeftSystemsVisibility = Visibility.Collapsed;
                    LeftEquipmentVisibility = Visibility.Collapsed;
                    LeftSubScopeVisibility = Visibility.Collapsed;
                    LeftDevicesVisibility = Visibility.Collapsed;
                    LeftDevicesEditVisibility = Visibility.Visible;
                    LeftManufacturerVisibility = Visibility.Visible;
                    break;
                default:
                    LeftTabIndex = 0;

                    LeftSystemsVisibility = Visibility.Visible;
                    LeftEquipmentVisibility = Visibility.Visible;
                    LeftSubScopeVisibility = Visibility.Visible;
                    LeftDevicesVisibility = Visibility.Visible;
                    LeftDevicesEditVisibility = Visibility.Collapsed;
                    LeftManufacturerVisibility = Visibility.Collapsed;
                    break;

            }
        }

        private void nullifySelected()
        {
            SelectedDevice = null;
            SelectedPoint = null;
            SelectedSubScope = null;
            SelectedEquipment = null;
            SelectedSystem = null;
            resetEditTab();
        }

        private void populateItemsCollections()
        {
            ViewData.SystemItemsCollection = Templates.SystemTemplates;
            ViewData.EquipmentItemsCollection = Templates.EquipmentTemplates;
            ViewData.SubScopeItemsCollection = Templates.SubScopeTemplates;
            ViewData.DevicesItemsCollection = Templates.DeviceCatalog;
        }

        private void resetEditTab()
        {
            RightTabIndex = (EditIndex)DGTabIndex;
        }
        #endregion //Helper Methods
        #endregion //Methods
    }
}