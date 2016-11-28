using GalaSoft.MvvmLight;
using EstimatingLibrary;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System;
using Microsoft.Win32;
using EstimatingUtilitiesLibrary;
using System.Windows;
using System.IO;
using GongSolutions.Wpf.DragDrop;
using GalaSoft.MvvmLight.Messaging;
using TECUserControlLibrary;
using System.Collections;
using System.Drawing.Imaging;
using System.Deployment.Application;
using System.ComponentModel;

namespace Scope_Builder.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase, IDropTarget
    {
        private TECBid _bid;
        private TECTemplates _templates;
        private TECSystem _selectedSystem;
        private TECEquipment _selectedEquipment;

        private VisibilityModel _dataGridVisibilty;
        public VisibilityModel DataGridVisibilty
        {
            get { return _dataGridVisibilty; }
            set
            {
                _dataGridVisibilty = value;
                RaisePropertyChanged("DataGridVisibilty");
            }
        }

        private string bidDBFilePath;
        
        private bool saveSuccessful;

        private string defaultTemplatesPath;

        public ICommand NewCommand { get; private set; }
        public ICommand LoadCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand SaveAsCommand { get; private set; }
        public ICommand DocumentCommand { get; private set; }
        public ICommand LoadTemplatesCommand { get; private set; }
        public ICommand CSVExportCommand { get; private set; }
        public ICommand BudgetCommand { get; private set; }
        public ICommand SearchCollectionCommand { get; private set; }
        public ICommand EndSearchCommand { get; private set; }
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }

        public RelayCommand<CancelEventArgs> ClosingCommand { get; private set; }

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

        public int DGTabIndex
        {
            get { return _dgTabIndex; }
            set
            {
                _dgTabIndex = value;
                RaisePropertyChanged("DGTabIndex");
                
            }
        }
        private int _dgTabIndex;

        public TECBid Bid
        {
            get { return _bid; }
            set
            {
                _bid = value;
                stack = new ChangeStack(value);
                // Call OnPropertyChanged whenever the property is updated
                RaisePropertyChanged("Bid");
            }
        }

        public TECTemplates Templates
        {
            get { return _templates; }
            set
            {
                _templates = value;
                RaisePropertyChanged("Templates");
            }
        }

        public TECSystem SelectedSystem
        {
            get { return _selectedSystem; }
            set
            {
                _selectedSystem = value;
                RaisePropertyChanged("SelectedSystem");
            }
        }

        public TECEquipment SelectedEquipment
        {
            get { return _selectedEquipment; }
            set
            {
                _selectedEquipment = value;
                RaisePropertyChanged("SelectedEquipment");
            }
        }

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

        public string TECLogo { get; set; }

        public string Version { get; set; }

        private ChangeStack stack { get; set; }
        
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
            DataGridVisibilty = new VisibilityModel();
            setVisibility(0);

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
                string message = "No template database found. Use Template Builder to create and save templates and reload them in Scope Builder.";
                MessageBox.Show(message);
            }

            Bid = new TECBid();
            Bid.DeviceCatalog = Templates.DeviceCatalog;
            Bid.ManufacturerCatalog = Templates.ManufacturerCatalog;
            Bid.Tags = Templates.Tags;

            populateItemsCollections();

            NewCommand = new RelayCommand(NewExecute);
            LoadCommand = new RelayCommand(LoadExecute);
            SaveCommand = new RelayCommand(SaveExecute);
            SaveAsCommand = new RelayCommand(SaveAsExecute);
            DocumentCommand = new RelayCommand(DocumentExecute);
            CSVExportCommand = new RelayCommand(CSVExportExecute);
            BudgetCommand = new RelayCommand(BudgetExecute);
            LoadTemplatesCommand = new RelayCommand(LoadTemplatesExecute);
            SearchCollectionCommand = new RelayCommand(SearchCollectionExecute);
            EndSearchCommand = new RelayCommand(EndSearchExecute);
            UndoCommand = new RelayCommand(UndoExecute, UndoCanExecute);
            RedoCommand = new RelayCommand(RedoExecute, RedoCanExecute);

            ClosingCommand = new RelayCommand<CancelEventArgs>(e => ClosingExecute(e));

            bidDBFilePath = null;

            TECLogo = Path.GetTempFileName();

            (Properties.Resources.TECLogo).Save(TECLogo, ImageFormat.Png);

            stack = new ChangeStack(Bid);
        }

        #region Resources Paths
        const string APPDATA_FOLDER = @"TECSystems\";
        const string TEMPLATES_FILE_NAME = @"TECTemplates.tdb";
        #endregion //Resources Paths

        #region Commands
        
        private void NewExecute()
        {
            string message = "Would you like to save your changes before creating a new scope?";
            MessageBoxResult result = MessageBox.Show(message, "Create new", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);
            if (result == MessageBoxResult.Yes)
            {
                SaveExecute();

                if (saveSuccessful) {
                    bidDBFilePath = null;
                    Bid = new TECBid();
                }
                else
                {
                    MessageBox.Show("Save unsuccessful. New scope not created.");
                }
            }
            else if (result == MessageBoxResult.No)
            {
                bidDBFilePath = null;
                Bid = new TECBid();
            }
        }

        private void LoadExecute()
        {
            //User choose path
            string path = getLoadPath();
            if (path != null)
            {
                bidDBFilePath = path;
                Properties.Settings.Default.ScopeDirectoryPath = Path.GetDirectoryName(path);
                
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    Bid = EstimatingLibraryDatabase.LoadDBToBid(path, Templates);
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
            saveSuccessful = false;
            if (bidDBFilePath != null)
            {
                if (!UtilitiesMethods.IsFileLocked(bidDBFilePath))
                {
                    SaveWindow saveWindow = new SaveWindow();
                    if (File.Exists(bidDBFilePath))
                    {
                        File.Delete(bidDBFilePath);
                    }
                    try
                    {
                        EstimatingLibraryDatabase.UpdateBidToDB(bidDBFilePath, stack);
                    }
                    catch (Exception e)
                    {
                        Console.Write("Save delta failed. Saving to new file. Error: " + e.Message);
                        EstimatingLibraryDatabase.SaveBidToNewDB(bidDBFilePath, Bid);
                    }
                    
                    stack.ClearStacks();
                    saveSuccessful = true;
                    saveWindow.Close();
                }
                else
                {
                    string message = "File is open elsewhere";
                    MessageBox.Show(message);
                }
            }
            else
            {
                SaveAsExecute();
            }
        }

        private void SaveAsExecute()
        {
            //User choose path
            saveSuccessful = false;
            string path = getSavePath();
            if (path != null)
            {
                bidDBFilePath = path;
                Properties.Settings.Default.ScopeDirectoryPath = Path.GetDirectoryName(path);
                
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    SaveWindow saveWindow = new SaveWindow();
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    //Create new database
                    EstimatingLibraryDatabase.SaveBidToNewDB(path, Bid);
                    stack.ClearStacks();
                    saveSuccessful = true;
                    saveWindow.Close();
                }
                else
                {
                    string message = "File is open elsewhere";
                    MessageBox.Show(message);
                }
                Console.WriteLine("Finished saving SQL Database.");
            }
        }

        private void DocumentExecute()
        {
            string path = getDocumentSavePath();

            if (path != null)
            {
                Properties.Settings.Default.DocumentDirectoryPath = Path.GetDirectoryName(path);
                
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    //EstimatingUtilitiesLibrary.ScopeDocumentBuilder.CreateScopeDocument(Bid, path);
                    Console.WriteLine("Scope saved to document.");
                }
                else
                {
                    string message = "File is open elsewhere";
                    MessageBox.Show(message);
                }
            }
        }

        private void CSVExportExecute()
        {
            //User choose path
            string path = getCSVSavePath();
            if (path != null)
            {
                Properties.Settings.Default.PointCSVDirectoryPath = Path.GetDirectoryName(path);
                
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    CSVWriter writer = new CSVWriter(path);
                    writer.BidPointsToCSV(Bid);
                    Console.WriteLine("Finished exporting points to a csv.");
                }
                else
                {
                    string message = "File is open elsewhere";
                    MessageBox.Show(message);
                }
            }
        }

        private void SearchCollectionExecute()
        {
            if (SearchString != null)
            {
                switch (LeftTabIndex)
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

        private void BudgetExecute()
        {
            new View.BudgetWindow();
            MessengerInstance.Send<GenericMessage<ObservableCollection<TECSystem>>>(new GenericMessage<ObservableCollection<TECSystem>>(Bid.Systems));
        }

        private void LoadTemplatesExecute()
        {
            //User choose path
            string path = getLoadTemplatesPath();

            if (path != null)
            {
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    File.Copy(path, defaultTemplatesPath, true);
                    Templates = EstimatingLibraryDatabase.LoadDBToTemplates(path);
                    Bid.DeviceCatalog = Templates.DeviceCatalog;
                    Bid.ManufacturerCatalog = Templates.ManufacturerCatalog;
                    Bid.Tags = Templates.Tags;
                }
                else
                {
                    string message = "File is open elsewhere";
                    MessageBox.Show(message);
                }
                Console.WriteLine("Finished loading templates");
            }
        }

        private void UndoExecute()
        {
            stack.Undo();
        }
        private bool UndoCanExecute()
        {
            if (stack.UndoStack.Count > 0)
                return true;
            else
                return false;
        }

        private void RedoExecute()
        {
            stack.Redo();
        }
        private bool RedoCanExecute()
        {
            if (stack.RedoStack.Count > 0)
                return true;
            else
                return false;
        }

        private void ClosingExecute(CancelEventArgs e)
        {
            bool changes = (stack.SaveStack.Count > 0);
            if (changes)
            {
                MessageBoxResult result = MessageBox.Show("You have unsaved changes. Would you like to save before quitting?", "Save?", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    SaveExecute();
                    if (!saveSuccessful)
                    {
                        e.Cancel = true;
                        MessageBox.Show("Save unsuccessful, cancelling quit.");
                    }
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                    Console.WriteLine("Closing");
                }
            }
            if (!e.Cancel)
            {
                Properties.Settings.Default.Save();
            }
        }
        #endregion //Commands

        #region Helper Functions

        private string getSavePath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (Properties.Settings.Default.ScopeDirectoryPath != null)
            {
                saveFileDialog.InitialDirectory = Properties.Settings.Default.ScopeDirectoryPath;
            }
            else
            {
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            saveFileDialog.FileName = Bid.BidNumber + " " + Bid.Name;
            saveFileDialog.Filter = "Bid Database Files (*.bdb)|*.bdb";
            saveFileDialog.DefaultExt = "bdb";
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

        private string getDocumentSavePath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            if (Properties.Settings.Default.DocumentDirectoryPath != null)
            {
                saveFileDialog.InitialDirectory = Properties.Settings.Default.DocumentDirectoryPath;
            }
            else
            {
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            saveFileDialog.Filter = "Rich Text Files (*.rtf)|*.rtf";
            saveFileDialog.DefaultExt = "rtf";
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

        private string getCSVSavePath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (Properties.Settings.Default.PointCSVDirectoryPath != null)
            {
                saveFileDialog.InitialDirectory = Properties.Settings.Default.PointCSVDirectoryPath;
            }
            else
            {
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            
            saveFileDialog.Filter = "Comma Separated Values Files (*.csv)|*.csv";
            saveFileDialog.DefaultExt = "csv";
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

        private string getLoadPath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(Properties.Settings.Default.ScopeDirectoryPath != null)
            {
                openFileDialog.InitialDirectory = Properties.Settings.Default.ScopeDirectoryPath;
            }
            else
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            
            openFileDialog.Filter = "Bid Database Files (*.bdb)|*.bdb";
            openFileDialog.DefaultExt = "bdb";
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

        private string getLoadTemplatesPath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.Filter = "Template Database Files (*.tdb)|*.tdb";
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

        private void populateItemsCollections()
        {
            SystemItemsCollection = Templates.SystemTemplates;
            EquipmentItemsCollection = Templates.EquipmentTemplates;
            SubScopeItemsCollection = Templates.SubScopeTemplates;
        }

        private void setVisibility(int tIndex)
        {
            switch (tIndex)
            {
                case 0:
                    DataGridVisibilty.ExpandSubScope = Visibility.Collapsed;
                    break;
                default:
                    break;

            }
        }

        #endregion //Helper Functions

        #region Drag Drop
        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data;
            var targetCollection = dropInfo.TargetCollection;

            if(sourceItem is TECSystem)
            {
                if (sourceItem != null && targetCollection.GetType() == typeof(ObservableCollection<TECSystem>))
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                    dropInfo.Effects = DragDropEffects.Copy;
                }
            } else if (sourceItem is TECEquipment)
            {
                if (sourceItem != null && targetCollection.GetType() == typeof(ObservableCollection<TECEquipment>))
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                    dropInfo.Effects = DragDropEffects.Copy;
                }
            } else if (sourceItem is TECSubScope)
            {
                if (sourceItem != null && targetCollection.GetType() == typeof(ObservableCollection<TECSubScope>))
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
                    if (index < Bid.Systems.Count)
                    {
                        Bid.Systems.Insert(dropInfo.InsertIndex, tempSystem);
                    }
                    else
                    {
                        Bid.Systems.Add(tempSystem);
                    }
                }
                else if (sourceItem is TECEquipment)
                {
                    TECEquipment tempEquipment = new TECEquipment(sourceItem as TECEquipment);
                    int index = dropInfo.InsertIndex;
                    ObservableCollection<TECEquipment> targetEquipment = dropInfo.TargetCollection as ObservableCollection<TECEquipment>;
                    if (index < Bid.Systems.Count)
                    {
                        targetEquipment.Insert(dropInfo.InsertIndex, tempEquipment);
                    }
                    else
                    {
                        targetEquipment.Add(tempEquipment);
                    }
                }
                else if (sourceItem is TECSubScope)
                {
                    TECSubScope tempSubScope = new TECSubScope(sourceItem as TECSubScope);

                    int index = dropInfo.InsertIndex;
                    ObservableCollection<TECSubScope> targetSubScope = dropInfo.TargetCollection as ObservableCollection<TECSubScope>;

                    if (index < Bid.Systems.Count)
                    {
                        targetSubScope.Insert(dropInfo.InsertIndex, tempSubScope);
                    }
                    else
                    {
                        targetSubScope.Add(tempSubScope);
                    }
                }
            }
        }
        #endregion
        
    }
}