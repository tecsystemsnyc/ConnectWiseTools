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
        protected bool isEstimate;

        protected override string defaultSaveFileName
        {
            get
            {
                return (Bid.BidNumber + " " + Bid.Name);
            }
        }
        protected override TECScopeManager workingScopeManager
        {
            get
            { return base.workingScopeManager; }
            set
            {
                base.workingScopeManager = value;
                RaisePropertyChanged("Bid");
                Bid.PropertyChanged += Bid_PropertyChanged;
                BidSet?.Invoke();
                buildTitleString();
            }
        }
        public TECBid Bid
        {
            get { return workingScopeManager as TECBid; }
            set
            {
                workingScopeManager = value;
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
        #region Settings Properties
        override protected string TemplatesFilePath
        {
            get { return Properties.Settings.Default.TemplatesFilePath; }
            set
            {
                if (Properties.Settings.Default.TemplatesFilePath != value)
                {
                    Properties.Settings.Default.TemplatesFilePath = value;
                    Properties.Settings.Default.Save();
                    TemplatesFilePathChanged();
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

            workingFileParameters = BidFileParameters;
            
            setupData();
        }

        #region Methods
        #region Setup
        override protected void setupCommands()
        {
            base.setupCommands();
            DocumentCommand = new RelayCommand(DocumentExecute);
            CSVExportCommand = new RelayCommand(CSVExportExecute);
            BudgetCommand = new RelayCommand(BudgetExecute);
            LoadTemplatesCommand = new RelayCommand(LoadTemplatesExecute);
            ExcelExportCommand = new RelayCommand(ExcelExportExecute);
            RefreshTemplatesCommand = new RelayCommand(RefreshTemplatesExecute);
        }
        private void setupData()
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
                    TemplatesFilePath = getLoadPath(TemplatesFileParameters);

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
            var newCatalogs = UtilitiesMethods.UnionizeCatalogs(Bid.Catalogs, Templates.Catalogs);
            Bid.Catalogs = newCatalogs;
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
        private void buildTitleString()
        {
            TitleString = Bid.Name + " - " + programName;
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
                        loadedTemplates = EstimatingLibraryDatabase.Load(TemplatesFilePath) as TECTemplates;
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
                    if (isNew)
                    {
                        setupBid();
                    }
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
        #endregion
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
                    SetBusyStatus("Saving...", false);
                    if (saveDelta(false))
                    {
                        setupBid();
                    }
                    else
                    {
                        MessageBox.Show("Save unsuccessful. New scope not created.");
                    }
                    ResetStatus();
                }
                else if (result == MessageBoxResult.No)
                {
                    DebugHandler.LogDebugMessage("Creating new bid.");
                    setupBid();
                    isNew = true;
                }
                else
                {
                    return;
                }
            }
            else
            {
                DebugHandler.LogDebugMessage("Creating new bid.");
                setupBid();
                isNew = true;
            }
            
        }
        private void DocumentExecute()
        {
            string path = getSavePath(DocumentFileParameters, defaultSaveFileName, ScopeDirectoryPath);

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
            string path = getSavePath(CSVFileParameters, defaultSaveFileName, ScopeDirectoryPath);
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
            string path = getSavePath(CSVFileParameters, defaultSaveFileName, ScopeDirectoryPath);
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
            string path = getLoadPath(TemplatesFileParameters);
            if (path != null)
            {
                TemplatesFilePath = path;
                loadTemplates(TemplatesFilePath);
            }
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
                loadTemplates(TemplatesFilePath);
            }
        }
        #endregion
        #endregion
    }
}