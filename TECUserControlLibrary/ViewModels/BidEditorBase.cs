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
using System.Windows.Threading;
using TECUserControlLibrary.ViewModelExtensions;

namespace TECUserControlLibrary.ViewModels
{
    abstract public class BidEditorBase : BuilderViewModel
    {
        
        #region Properties
        
        private bool _templatesLoaded;
        protected bool templatesLoaded
        {
            get { return _templatesLoaded; }
            set
            {
                _templatesLoaded = value;
                TemplatesLoadedSet?.Invoke();
                if (isNew)
                {
                    setupBid();
                }
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

        protected bool isEstimate;

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
        public ICommand DocumentCommand { get; private set; }
        public ICommand LoadTemplatesCommand { get; private set; }
        public ICommand CSVExportCommand { get; private set; }
        public ICommand BudgetCommand { get; private set; }
        public ICommand ExcelExportCommand { get; private set; }

        public ICommand RefreshTemplatesCommand { get; private set; }
        #endregion
        
        #region Delgates
        public Action BidSet;
        public Action TemplatesLoadedSet;
        #endregion
        
        #endregion

        public BidEditorBase() : base()
        {
            isNew = true;
            
            setupCommands();
            setupTemplates();
            setupBid();
            setupMenu();
        }

        #region Methods

        #region Setup
        private void setupCommands()
        {
            DocumentCommand = new RelayCommand(DocumentExecute);
            CSVExportCommand = new RelayCommand(CSVExportExecute);
            BudgetCommand = new RelayCommand(BudgetExecute);
            LoadTemplatesCommand = new RelayCommand(LoadTemplatesExecute);
            ExcelExportCommand = new RelayCommand(ExcelExportExecute);
            RefreshTemplatesCommand = new RelayCommand(RefreshTemplatesExecute);
        }
        private void setupTemplates()
        {
            Templates = new TECTemplates();
            
            if ((TemplatesFilePath != "") && (File.Exists(TemplatesFilePath)))
            {
                if (!UtilitiesMethods.IsFileLocked(TemplatesFilePath))
                {
                    loadTemplates(TemplatesFilePath);
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
                            loadTemplates(TemplatesFilePath);
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
            saveFilePath = null;
        }
        protected override void setupMenu()
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
            //MenuVM.ExportExcelCommand = ExcelExportCommand;

            MenuVM.RefreshTemplatesCommand = RefreshTemplatesCommand;

            //Toggle Templates Command gets handled in each MainView model for ScopeBuilder and EstimateBuilder
        }
        
        #endregion

        #region Helper Functions
        
        override protected string getSavePath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (ScopeDirectoryPath != null && !isNew)
            {
                saveFileDialog.InitialDirectory = ScopeDirectoryPath;
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

            if (ScopeDirectoryPath != null)
            {
                saveFileDialog.InitialDirectory = ScopeDirectoryPath;
            }
            else
            {
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            saveFileDialog.Filter = "Rich Text Files (*.rtf)|*.rtf";
            saveFileDialog.DefaultExt = "rtf";
            //saveFileDialog.Filter = "Word Document (*.docx)|*.docx";
            //saveFileDialog.DefaultExt = "docx";
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
            if (ScopeDirectoryPath != null)
            {
                saveFileDialog.InitialDirectory = ScopeDirectoryPath;
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
        private string getExcelSavePath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (ScopeDirectoryPath != null)
            {
                saveFileDialog.InitialDirectory = ScopeDirectoryPath;
            }
            else
            {
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
            saveFileDialog.DefaultExt = "xlsx";
            saveFileDialog.AddExtension = true;

            string path = null;

            if (saveFileDialog.ShowDialog() == true)
            {
                path = saveFileDialog.FileName;
            }

            return path;
        }
        override protected string getLoadPath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (ScopeDirectoryPath != null)
            {
                openFileDialog.InitialDirectory = ScopeDirectoryPath;
            }
            else
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            openFileDialog.Filter = "Bid Database Files (*.bdb)|*.bdb" + "|All Files (*.*)|*.*";
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
            }

            return path;
        }
        private void buildTitleString()
        {
            TitleString = Bid.Name + " - " + programName;
        }
        override protected void loadFromPath(string path)
        {
            if (path != null)
            {
                SetBusyStatus("Loading " + path);
                saveFilePath = path;
                ScopeDirectoryPath = Path.GetDirectoryName(path);

                if (!UtilitiesMethods.IsFileLocked(path))
                { Bid = EstimatingLibraryDatabase.LoadDBToBid(path, Templates); }
                else
                {
                    DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
                }
                ResetStatus();
            }
        }

        

        

        private void loadBid()
        {
            //User choose path
            string path = getLoadPath();
            if (path != null)
            {
                loadBidFromPath(path);
            }
        }
        protected void loadBidFromPath(string path)
        {
            if (path != null)
            {
                SetBusyStatus("Loading File: " + path, false);
                TECBid loadingBid = new TECBid();
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (s, e) =>
                {
                    bidDBFilePath = path;
                    ScopeDirectoryPath = Path.GetDirectoryName(path);

                    if (!UtilitiesMethods.IsFileLocked(path))
                    {
                        loadingBid = EstimatingLibraryDatabase.LoadDBToBid(path, Templates);
                    }
                    else
                    {
                        DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
                    }
                };
                worker.RunWorkerCompleted += (s, e) =>
                {
                    ResetStatus();
                    Bid = loadingBid;
                    isNew = false;
                };

                worker.RunWorkerAsync();
            }
        }
        private void loadTemplates(string TemplatesFilePath)
        {
            var loadedTemplates = new TECTemplates();
            if (TemplatesFilePath != null)
            {
                SetBusyStatus("Loading templates from file: " + TemplatesFilePath, false);

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (s, e) =>
                {
                    if (!UtilitiesMethods.IsFileLocked(TemplatesFilePath))
                    {

                        loadedTemplates = EstimatingLibraryDatabase.LoadDBToTemplates(TemplatesFilePath);
                    }
                    else
                    {
                        DebugHandler.LogError("Could not open file " + TemplatesFilePath + " File is open elsewhere.");
                    }
                    DebugHandler.LogDebugMessage("Finished loading templates");
                };
                worker.RunWorkerCompleted += (s, e) =>
                {
                    Templates = loadedTemplates;
                    UtilitiesMethods.AddCatalogsToBid(Bid, Templates);
                    templatesLoaded = true;
                    ResetStatus();
                };
                worker.RunWorkerAsync();
            }
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
        override protected void NewExecute()
        {
            if (!IsReady)
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
                        setupBid();
                    }
                    else
                    {
                        MessageBox.Show("Save unsuccessful. New scope not created.");
                    }
                }
                else if (result == MessageBoxResult.No)
                {
                    DebugHandler.LogDebugMessage("Creating new bid.");
                    setupBid();
                    isNew = true;
                }
            }
            else
            {
                DebugHandler.LogDebugMessage("Creating new bid.");
                setupBid();
                isNew = true;
            }
            
        }
        override protected void LoadExecute()
        {
            if (!IsReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }

            if (stack.SaveStack.Count > 0)
            {
                string message = "Would you like to save your changes before loading?";
                MessageBoxResult result = MessageBox.Show(message, "Create new", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    if (saveSynchronously())
                    {
                        loadBid();
                    }
                    else
                    {
                        MessageBox.Show("Save unsuccessful. File not loaded.");
                    }
                }
                else if (result == MessageBoxResult.No)
                {
                    loadBid();
                }
            }
            else
            {
                loadBid();
            }
            
        }
        
        private void DocumentExecute()
        {
            string path = getDocumentSavePath();

            if (path != null)
            {
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    ScopeDocumentBuilder.CreateScopeDocument(Bid, path, isEstimate);
                    //var thing = new ScopeWordDocumentBuilder();
                    //thing.CreateScopeWordDocument(Bid, path, isEstimate);
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
        private void ExcelExportExecute()
        {
            //User choose path
            string path = getExcelSavePath();
            if (path != null)
            {
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    EstimateSpreadsheetExporter.Export(Bid, path);
                    DebugHandler.LogDebugMessage("Exported to estimating spreadhseet.");
                }
                else
                {
                    DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
                }
            }
        }
        protected void LoadTemplatesExecute()
        {
            if (!IsReady)
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

            loadTemplates(TemplatesFilePath);
        }
        
        private void RefreshTemplatesExecute()
        {
            if (!IsReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }
            if (TemplatesFilePath != null)
            {
                SetBusyStatus("Loading templates from file: " + TemplatesFilePath, false);

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (s, e) =>
                {
                    if (!UtilitiesMethods.IsFileLocked(TemplatesFilePath))
                    {

                        Templates = EstimatingLibraryDatabase.LoadDBToTemplates(TemplatesFilePath);
                        UtilitiesMethods.AddCatalogsToBid(Bid, Templates);
                        templatesLoaded = true;
                    }
                    else
                    {
                        DebugHandler.LogError("Could not open file " + TemplatesFilePath + " File is open elsewhere.");
                    }
                    DebugHandler.LogDebugMessage("Finished refreshing templates");
                };
                worker.RunWorkerCompleted += (s, e) =>
                {
                    ResetStatus();
                };

                worker.RunWorkerAsync();
                
            }
        }

        override protected void ClosingExecute(CancelEventArgs e)
        {
            if (!IsReady)
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
        #endregion
        
        #endregion
    }
}