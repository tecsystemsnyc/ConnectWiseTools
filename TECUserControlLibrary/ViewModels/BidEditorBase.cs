using DebugLibrary;
using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Deployment.Application;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Input;
using TECUserControlLibrary.ViewModelExtensions;

namespace TECUserControlLibrary.ViewModels
{
    public class BidEditorBase : ViewModelBase
    {

        #region Constants

        string DEFAULT_STATUS_TEXT = "Ready";

        #endregion

        #region Properties
        protected bool isReady
        {
            get;
            private set;
        }

        private bool _templatesLoaded;
        protected bool templatesLoaded
        {
            get { return _templatesLoaded; }
            set
            {
                _templatesLoaded = value;
                TemplatesLoadedSet?.Invoke();
            }
        }
        private string _programName;
        protected string programName
        {
            get { return _programName; }
            set
            {
                _programName = value;
                buildTitleString();
            }
        }

        private TECBid _bid;
        public TECBid Bid
        {
            get { return _bid; }
            set
            {
                _bid = value;
                stack = new ChangeStack(value);
                // Call OnPropertyChanged whenever the property is updated
                RaisePropertyChanged("Bid");
                buildTitleString();
                Bid.PropertyChanged += Bid_PropertyChanged;
                BidSet?.Invoke();
            }
        }
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

        public string TitleString
        {
            get { return _titleString; }
            set
            {
                _titleString = value;
                RaisePropertyChanged("TitleString");
            }
        }
        private string _titleString;

        public string TECLogo { get; set; }

        #region Settings Properties
        public string TemplatesFilePath
        {
            get { return Properties.Settings.Default.TemplatesFilePath; }
            set
            {
                if (Properties.Settings.Default.TemplatesFilePath != value)
                {
                    Properties.Settings.Default.TemplatesFilePath = value;
                    RaisePropertyChanged("TemplatesFilePath");
                    Properties.Settings.Default.Save();
                }
            }
        }
        #endregion

        #region Command Properties
        public ICommand NewCommand { get; private set; }
        public ICommand LoadCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand SaveAsCommand { get; private set; }
        public ICommand DocumentCommand { get; private set; }
        public ICommand LoadTemplatesCommand { get; private set; }
        public ICommand CSVExportCommand { get; private set; }
        public ICommand BudgetCommand { get; private set; }

        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }

        public ICommand RefreshTemplatesCommand { get; private set; }

        public RelayCommand<CancelEventArgs> ClosingCommand { get; private set; }
        #endregion

        #region Fields
        private ChangeStack stack;
        private string bidDBFilePath;
        public string startupFile;
        public string scopeDirectoryPath;
        #endregion

        #region Delgates
        public Action BidSet;
        public Action TemplatesLoadedSet;
        #endregion

        #region View Models
        public MenuViewModel MenuVM { get; set; }
        public StatusBarExtension StatusBarVM { get; set; }
        #endregion

        #endregion
        public BidEditorBase()
        {
            setupStatusBar();
            SetBusyStatus("Initializing Program...");

            setupCommands();
            setupTemplates();
            getLogo();
            setupBid();
            setupStack();
            setupMenu();

            ResetStatus();
        }

        #region Methods

        #region Setup
        private void setupCommands()
        {
            NewCommand = new RelayCommand(NewExecute);
            LoadCommand = new RelayCommand(LoadExecute);
            SaveCommand = new RelayCommand(SaveExecute);
            SaveAsCommand = new RelayCommand(SaveAsExecute);
            DocumentCommand = new RelayCommand(DocumentExecute);
            CSVExportCommand = new RelayCommand(CSVExportExecute);
            BudgetCommand = new RelayCommand(BudgetExecute);
            LoadTemplatesCommand = new RelayCommand(LoadTemplatesExecute);
            UndoCommand = new RelayCommand(UndoExecute, UndoCanExecute);
            RedoCommand = new RelayCommand(RedoExecute, RedoCanExecute);

            RefreshTemplatesCommand = new RelayCommand(RefreshTemplatesExecute);

            ClosingCommand = new RelayCommand<CancelEventArgs>(e => ClosingExecute(e));
        }
        private void setupTemplates()
        {
            Templates = new TECTemplates();
            
            if ((TemplatesFilePath != "") && (File.Exists(TemplatesFilePath)))
            {
                if (!UtilitiesMethods.IsFileLocked(TemplatesFilePath))
                {
                    Templates = EstimatingLibraryDatabase.LoadDBToTemplates(TemplatesFilePath);
                    templatesLoaded = true;
                }
                else
                {
                    DebugHandler.LogError("TECTemplates file is open elsewhere. Could not load templates. Please close the templates file and load again.");
                    templatesLoaded = false;
                }
            }
            else
            {
                string message = "No templates file loaded. Would you like to load templates?";
                MessageBoxResult result = MessageBox.Show(message, "Load Templates?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    //User choose path
                    TemplatesFilePath = getLoadTemplatesPath();

                    if (TemplatesFilePath != null)
                    {
                        if (!UtilitiesMethods.IsFileLocked(TemplatesFilePath))
                        {
                            Templates = EstimatingLibraryDatabase.LoadDBToTemplates(TemplatesFilePath);
                            templatesLoaded = true;
                            DebugHandler.LogDebugMessage("Finished loading templates.");
                        }
                        else
                        {
                            DebugHandler.LogError("TECTemplates file is open elsewhere. Could not load templates. Please close the templates file and load again.");
                            templatesLoaded = false;
                        }
                    }
                }
                else
                {
                    templatesLoaded = false;
                }
            }
        }
        private void setupBid()
        {
            Bid = new TECBid();
            Bid.Labor.UpdateConstants(Templates.Labor);
            UtilitiesMethods.AddCatalogsToBid(Bid, Templates);
            bidDBFilePath = null;
        }
        private void setupStack()
        {
            stack = new ChangeStack(Bid);
        }

        private void setupMenu()
        {
            MenuVM = new MenuViewModel(MenuType.SB);

            MenuVM.NewCommand = NewCommand;
            MenuVM.LoadCommand = LoadCommand;
            MenuVM.SaveCommand = SaveCommand;
            MenuVM.SaveAsCommand = SaveAsCommand;
            MenuVM.ExportProposalCommand = DocumentCommand;
            MenuVM.LoadTemplatesCommand = LoadTemplatesCommand;
            MenuVM.ExportPointsListCommand = CSVExportCommand;
            MenuVM.UndoCommand = UndoCommand;
            MenuVM.RedoCommand = RedoCommand;

            MenuVM.RefreshTemplatesCommand = RefreshTemplatesCommand;

            //Toggle Templates Command gets handled in each MainView model for ScopeBuilder and EstimateBuilder
        }
        private void setupStatusBar()
        {
            StatusBarVM = new StatusBarExtension();

            if (ApplicationDeployment.IsNetworkDeployed)
            { StatusBarVM.Version = "Version " + ApplicationDeployment.CurrentDeployment.CurrentVersion; }
            else
            { StatusBarVM.Version = "Undeployed Version"; }

            StatusBarVM.CurrentStatusText = DEFAULT_STATUS_TEXT;
        }
        #endregion

        #region Helper Functions

        public void checkForOpenWith(string startupFile)
        {
            if (startupFile != "")
            {
                SetBusyStatus("Loading " + startupFile);
                LoadFromPath(startupFile);
                ResetStatus();
            }
        }

        private void getLogo()
        {
            TECLogo = Path.GetTempFileName();

            (Properties.Resources.TECLogo).Save(TECLogo, ImageFormat.Png);
        }
        private string getSavePath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (scopeDirectoryPath != null)
            {
                saveFileDialog.InitialDirectory = scopeDirectoryPath;
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
                path = saveFileDialog.FileName;
            }

            return path;
        }
        private string getDocumentSavePath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            if (scopeDirectoryPath != null)
            {
                saveFileDialog.InitialDirectory = scopeDirectoryPath;
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
                path = saveFileDialog.FileName;
            }

            return path;
        }
        private string getCSVSavePath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (scopeDirectoryPath != null)
            {
                saveFileDialog.InitialDirectory = scopeDirectoryPath;
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
                path = saveFileDialog.FileName;
            }

            return path;
        }
        private string getLoadPath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (scopeDirectoryPath != null)
            {
                openFileDialog.InitialDirectory = scopeDirectoryPath;
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
                path = openFileDialog.FileName;
            }

            return path;
        }
        private string getLoadTemplatesPath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Please choose a template database.";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.Filter = "Template Database Files (*.tdb)|*.tdb";
            openFileDialog.DefaultExt = "tdb";
            openFileDialog.AddExtension = true;

            string path = null;

            if (openFileDialog.ShowDialog() == true)
            {
                path = openFileDialog.FileName;
                TemplatesFilePath = path;
                Properties.Settings.Default.Save();
            }

            return path;
        }
        private void buildTitleString()
        {
            TitleString = Bid.Name + " - " + programName;
        }
        private void LoadFromPath(string path)
        {
            if (path != null)
            {
                SetBusyStatus("Loading " + path);
                bidDBFilePath = path;
                scopeDirectoryPath = Path.GetDirectoryName(path);

                if (!UtilitiesMethods.IsFileLocked(path))
                { Bid = EstimatingLibraryDatabase.LoadDBToBid(path, Templates); }
                else
                {
                    DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
                }
                ResetStatus();
            }
        }
        protected void SetBusyStatus(string statusText)
        {
            StatusBarVM.CurrentStatusText = statusText;
            isReady = false;
        }
        protected void ResetStatus()
        {
            StatusBarVM.CurrentStatusText = DEFAULT_STATUS_TEXT;
            isReady = true;
        }

        protected bool IsReady()
        {
            return isReady;
        }

        #region Event Handlers
        private void Bid_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                buildTitleString();
            }
        }
        #endregion

        #endregion //Helper Functions

        #region Commands
        private void NewExecute()
        {
            if (!isReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }
            if (stack.SaveStack.Count > 0)
            {
                string message = "Would you like to save your changes before creating a new scope?";
                MessageBoxResult result = MessageBox.Show(message, "Create new", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {

                    if (saveSynchronously())
                    {
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
                    DebugHandler.LogDebugMessage("Creating new bid.");
                    bidDBFilePath = null;
                    Bid = new TECBid();
                }
            }
            else
            {
                DebugHandler.LogDebugMessage("Creating new bid.");
                bidDBFilePath = null;
                Bid = new TECBid();
            }
            
        }
        private void LoadExecute()
        {
            if (!isReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }

            //User choose path
            string path = getLoadPath();
            if (path != null)
            {
                SetBusyStatus("Loading File: " + path);
                bidDBFilePath = path;
                scopeDirectoryPath = Path.GetDirectoryName(path);

                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    Bid = EstimatingLibraryDatabase.LoadDBToBid(path, Templates);
                }
                else
                {
                    DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
                }
                ResetStatus();
            }
        }
        private void SaveExecute()
        {
            if (!isReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }

            saveBid();
            
        }
        private void SaveAsExecute()
        {
            if (!isReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }
            saveBidAs();
        }
        private void DocumentExecute()
        {
            string path = getDocumentSavePath();

            if (path != null)
            {
                scopeDirectoryPath = Path.GetDirectoryName(path);

                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    ScopeDocumentBuilder.CreateScopeDocument(Bid, path);
                    DebugHandler.LogDebugMessage("Scope saved to document.");
                }
                else
                {
                    DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
                }
            }
        }
        private void CSVExportExecute()
        {
            //User choose path
            string path = getCSVSavePath();
            if (path != null)
            {
                scopeDirectoryPath = Path.GetDirectoryName(path);

                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    CSVWriter writer = new CSVWriter(path);
                    writer.BidPointsToCSV(Bid);
                    DebugHandler.LogDebugMessage("Points saved to csv.");
                }
                else
                {
                    DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
                }
            }
        }
        private void BudgetExecute()
        {
            //new View.BudgetWindow();
            //MessengerInstance.Send<GenericMessage<ObservableCollection<TECSystem>>>(new GenericMessage<ObservableCollection<TECSystem>>(Bid.Systems));
        }
        protected void LoadTemplatesExecute()
        {
            if (!isReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }
            //User choose path
            string path = getLoadTemplatesPath();
            if (path != "")
            {
                TemplatesFilePath = path;
            }

            if (TemplatesFilePath != null)
            {
                SetBusyStatus("Loading templates from file: " + TemplatesFilePath);
                if (!UtilitiesMethods.IsFileLocked(TemplatesFilePath))
                {
                    
                    Templates = EstimatingLibraryDatabase.LoadDBToTemplates(TemplatesFilePath);
                    Bid.DeviceCatalog = Templates.DeviceCatalog;
                    Bid.ManufacturerCatalog = Templates.ManufacturerCatalog;
                    Bid.Tags = Templates.Tags;
                    Bid.ConnectionTypes = Templates.ConnectionTypeCatalog;
                    Bid.ConduitTypes = Templates.ConduitTypeCatalog;
                    Bid.AssociatedCostsCatalog = Templates.AssociatedCostsCatalog;
                    templatesLoaded = true;
                }
                else
                {
                    DebugHandler.LogError("Could not open file " + TemplatesFilePath + " File is open elsewhere.");
                }
                DebugHandler.LogDebugMessage("Finished loading templates");
            }
            ResetStatus();
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

        private void RefreshTemplatesExecute()
        {
            if (!isReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }
            if (TemplatesFilePath != null)
            {
                SetBusyStatus("Loading templates from file: " + TemplatesFilePath);
                if (!UtilitiesMethods.IsFileLocked(TemplatesFilePath))
                {

                    Templates = EstimatingLibraryDatabase.LoadDBToTemplates(TemplatesFilePath);
                    Bid.DeviceCatalog = Templates.DeviceCatalog;
                    Bid.ManufacturerCatalog = Templates.ManufacturerCatalog;
                    Bid.Tags = Templates.Tags;
                    Bid.ConnectionTypes = Templates.ConnectionTypeCatalog;
                    Bid.ConduitTypes = Templates.ConduitTypeCatalog;
                    Bid.AssociatedCostsCatalog = Templates.AssociatedCostsCatalog;
                    templatesLoaded = true;
                }
                else
                {
                    DebugHandler.LogError("Could not open file " + TemplatesFilePath + " File is open elsewhere.");
                }
                DebugHandler.LogDebugMessage("Finished loading templates");
            }
            ResetStatus();
        }

        private void ClosingExecute(CancelEventArgs e)
        {
            if (!isReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                e.Cancel = true;
                return;
            }
            bool changes = (stack.SaveStack.Count > 0);
            if (changes)
            {
                MessageBoxResult result = MessageBox.Show("You have unsaved changes. Would you like to save before quitting?", "Save?", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    if (!saveSynchronously())
                    {
                        e.Cancel = true;
                        MessageBox.Show("Save unsuccessful, cancelling quit.");
                    }
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void saveBid()
        {
            if (bidDBFilePath != null)
            {
                SetBusyStatus("Saving to path: " + bidDBFilePath);
                ChangeStack stackToSave = stack.Copy();
                stack.ClearStacks();

                BackgroundWorker worker = new BackgroundWorker();

                worker.DoWork += (s, e) =>
                {
                    if (!UtilitiesMethods.IsFileLocked(bidDBFilePath))
                    {
                        try
                        {
                            EstimatingLibraryDatabase.UpdateBidToDB(bidDBFilePath, stackToSave);
                        }
                        catch (Exception ex)
                        {
                            DebugHandler.LogError("Save delta failed. Saving to new file. Exception: " + ex.Message);
                            EstimatingLibraryDatabase.SaveBidToNewDB(bidDBFilePath, Bid);
                        }
                    }
                    else
                    {
                        DebugHandler.LogError("Could not open file " + bidDBFilePath + " File is open elsewhere.");
                    }
                };
                worker.RunWorkerCompleted += (s, e) =>
                {
                    ResetStatus();
                };
                worker.RunWorkerAsync();
            }
            else
            {
                saveBidAs();
            }
        }

        private void saveBidAs()
        {
            //User choose path
            string path = getSavePath();
            if (path != null)
            {
                bidDBFilePath = path;
                scopeDirectoryPath = Path.GetDirectoryName(path);

                stack.ClearStacks();
                SetBusyStatus("Saving file: " + path);

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (s, e) =>
                {
                    if (!UtilitiesMethods.IsFileLocked(path))
                    {
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                        //Create new database
                        EstimatingLibraryDatabase.SaveBidToNewDB(path, Bid);
                    }
                    else
                    {
                        DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
                    }
                };
                worker.RunWorkerCompleted += (s, e) =>
                {
                    ResetStatus();
                };

                worker.RunWorkerAsync();

            }

        }

        private bool saveAsSynchronously()
        {
            bool saveSuccessful = false;

            //User choose path
            string path = getSavePath();
            if (path != null)
            {
                bidDBFilePath = path;
                scopeDirectoryPath = Path.GetDirectoryName(path);

                stack.ClearStacks();
                SetBusyStatus("Saving file: " + path);
                
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    //Create new database
                    EstimatingLibraryDatabase.SaveBidToNewDB(path, Bid);
                    saveSuccessful = true;
                }
                else
                {
                    DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
                    saveSuccessful = false;
                }
               

            }

            return saveSuccessful;
        }

        private bool saveSynchronously()
        {
            bool saveSuccessful = false;

            if (bidDBFilePath != null)
            {
                SetBusyStatus("Saving to path: " + bidDBFilePath);
                ChangeStack stackToSave = stack.Copy();
                stack.ClearStacks();
                
                if (!UtilitiesMethods.IsFileLocked(bidDBFilePath))
                {
                    try
                    {
                        EstimatingLibraryDatabase.UpdateBidToDB(bidDBFilePath, stackToSave);
                    saveSuccessful = true;
                    }
                    catch (Exception ex)
                    {
                        DebugHandler.LogError("Save delta failed. Saving to new file. Error: " + ex.Message);
                        EstimatingLibraryDatabase.SaveBidToNewDB(bidDBFilePath, Bid);
                    }
                }
                else
                {
                    DebugHandler.LogError("Could not open file " + bidDBFilePath + " File is open elsewhere.");
                }
               
            }
            else
            {
                if (saveAsSynchronously())
                {
                    saveSuccessful = true;
                }else
                {
                    saveSuccessful = false;
                }
            }

            return saveSuccessful;
        }
        #endregion

        #endregion
    }
}