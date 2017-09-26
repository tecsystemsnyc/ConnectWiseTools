using EstimatingLibrary;
using System.IO;
using System;
using EstimatingUtilitiesLibrary;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using System.ComponentModel;
using TECUserControlLibrary.ViewModels;
using DebugLibrary;
using TECUserControlLibrary.Utilities;
using EstimatingLibrary.Utilities;
using EstimatingUtilitiesLibrary.Database;
using GalaSoft.MvvmLight;

namespace EstimateBuilder.MVVM
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : BuilderViewModel
    {
        #region Fields
        public SplashVM SplashVM;

        private ViewModelBase _currentVM;
        private DatabaseManager bidDB;
        private DatabaseManager templatesDB;
        private bool _templatesLoaded;
        private TECEstimator _estimate;
        private TECTemplates _templates;

        #endregion
        #region Constructors
        public MainViewModel() : base()
        {
            if (workingScopeManager == null)
            {
                SplashVM = new SplashVM(TemplatesFilePath, defaultDirectory);
                SplashVM.Started += startUp;
                CurrentVM = SplashVM;
            }
        }

        #endregion
        #region Properties
        public ViewModelBase CurrentVM
        {
            get { return _currentVM; }
            set
            {
                _currentVM = value;
                RaisePropertyChanged("CurrentVM");
            }
        }
        public ScopeEditorVM ScopeEditorVM { get; set; }
        public LaborVM LaborVM { get; set; }
        public ReviewVM ReviewVM { get; set; }
        public ProposalVM ProposalVM { get; set; }
        public ElectricalVM ElectricalVM { get; set; }
        public ICommand ToggleTemplatesCommand { get; private set; }
        public ICommand DocumentCommand { get; private set; }
        public ICommand LoadTemplatesCommand { get; private set; }
        public ICommand CSVExportCommand { get; private set; }
        public ICommand BudgetCommand { get; private set; }
        public ICommand ExcelExportCommand { get; private set; }
        public ICommand RefreshTemplatesCommand { get; private set; }
        public ICommand RefreshBidCommand { get; private set; }
        public TECBid Bid
        {
            get { return workingScopeManager as TECBid; }
            set
            {
                workingScopeManager = value;
            }
        }
        public TECTemplates Templates
        {
            get { return _templates; }
            set
            {
                _templates = value;
                RaisePropertyChanged("Templates");
                updateBidWithTemplates();
                refresh();
            }
        }
        public TECEstimator Estimate
        {
            get { return _estimate; }
            set
            {
                _estimate = value;
                RaisePropertyChanged("Estimate");
            }
        }
        public override Visibility TemplatesVisibility
        {
            get
            {
                return ScopeEditorVM.TemplatesVisibility;
            }

            set
            {
                ScopeEditorVM.TemplatesVisibility = value;
                RaisePropertyChanged("TemplatesVisibility");
            }
        }

        protected override bool TemplatesHidden
        {
            get
            {
                return Properties.Settings.Default.TemplatesHidden;
            }
            set
            {
                if (Properties.Settings.Default.TemplatesHidden != value)
                {
                    Properties.Settings.Default.TemplatesHidden = value;
                    RaisePropertyChanged("TemplatesHidden");
                    TemplatesHiddenChanged();
                    Properties.Settings.Default.Save();
                }
            }
        }
        protected override string ScopeDirectoryPath
        {
            get { return Properties.Settings.Default.ScopeDirectoryPath; }
            set
            {
                Properties.Settings.Default.ScopeDirectoryPath = value;
                Properties.Settings.Default.Save();
            }
        }
        protected override string TemplatesFilePath
        {
            get { return Properties.Settings.Default.TemplatesFilePath; }
            set
            {
                if (Properties.Settings.Default.TemplatesFilePath != value)
                {
                    Properties.Settings.Default.TemplatesFilePath = value;
                    Properties.Settings.Default.Save();
                    SettingsVM.TemplatesLoadPath = TemplatesFilePath;

                    TemplatesFilePathChanged();
                }
            }
        }
        protected override string startupFilePath
        {
            get
            {
                return Properties.Settings.Default.StartupFile;
            }

            set
            {
                Properties.Settings.Default.StartupFile = value;
                Properties.Settings.Default.Save();
            }
        }
        protected override string defaultDirectory
        {
            get
            {
                return (Properties.Settings.Default.DefaultDirectory);
            }

            set
            {
                Properties.Settings.Default.DefaultDirectory = value;
                Properties.Settings.Default.Save();
            }
        }
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
                if (Bid != null)
                {
                    Bid.PropertyChanged -= bid_PropertyChanged;
                }
                base.workingScopeManager = value;
                RaisePropertyChanged("Bid");
                Bid.PropertyChanged += bid_PropertyChanged;
                buildTitleString();
                updateBidWithTemplates();
                refresh();
            }
        }
        
        private bool templatesLoaded
        {
            get { return _templatesLoaded; }
            set
            {
                _templatesLoaded = value;
                if (LaborVM != null)
                {
                    LaborVM.TemplatesLoaded = templatesLoaded;
                }
            }
        }
        #endregion
        #region Methods
        protected override void NewExecute()
        {
            if (!IsReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }
            if (deltaStack.CleansedStack().Count > 0 && deltaStack.CleansedStack().Count != loadedStackLength)
            {
                string message = "Would you like to save your changes before creating a new scope?";
                MessageBoxResult result = MessageBox.Show(message, "Create new", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    SetBusyStatus("Saving...", false);
                    if (saveDelta(false))
                    {
                        DebugHandler.LogDebugMessage("Creating new bid.");
                        isNew = true;
                        Bid = new TECBid();
                        saveFilePath = null;
                    }
                    else
                    {
                        DebugHandler.LogError("Save unsuccessful. New scope not created.");
                    }
                    ResetStatus();
                }
                else if (result == MessageBoxResult.No)
                {
                    DebugHandler.LogDebugMessage("Creating new bid.");
                    isNew = true;
                    Bid = new TECBid();
                    saveFilePath = null;
                }
                else
                {
                    return;
                }
            }
            else
            {
                DebugHandler.LogDebugMessage("Creating new bid.");
                isNew = true;
                Bid = new TECBid();
                saveFilePath = null;
            }

        }
        protected override void setupExtensions(MenuType menuType)
        {
            base.setupExtensions(menuType);
            setupScopeEditorVM(new TECBid(), new TECTemplates());
            setupLaborVM(new TECBid(), new TECTemplates());
            setupReviewVM(new TECBid());
            setupProposalVM(new TECBid());
            setupElectricalVM(new TECBid());
        }
        protected override void setupCommands()
        {
            base.setupCommands();
            DocumentCommand = new RelayCommand(documentExecute);
            CSVExportCommand = new RelayCommand(cSVExportExecute);
            BudgetCommand = new RelayCommand(budgetExecute);
            LoadTemplatesCommand = new RelayCommand(LoadTemplatesExecute);
            ExcelExportCommand = new RelayCommand(excelExportExecute);
            RefreshTemplatesCommand = new RelayCommand(refreshTemplatesExecute);
            RefreshBidCommand = new RelayCommand(refreshBidExecute, refreshBidCanExecute);
            ToggleTemplatesCommand = new RelayCommand(toggleTemplatesExecute);
        }
        protected override void setupMenu(MenuType menuType)
        {
            MenuVM = new MenuVM(MenuType.SB);

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
            MenuVM.RefreshBidCommand = RefreshBidCommand;

            MenuVM.TemplatesHidden = TemplatesHidden;
            MenuVM.ToggleTemplatesCommand = ToggleTemplatesCommand;
        }
        protected void LoadTemplatesExecute()
        {
            if (!IsReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }
            //User choose path
            string path = UIHelpers.GetLoadPath(UIHelpers.TemplatesFileParameters, defaultDirectory);
            if (path != null)
            {
                TemplatesFilePath = path;
                loadTemplates(TemplatesFilePath);
            }
        }
        protected void buildTitleString()
        {
            string bidName = "";
            if (Bid != null)
            {
                bidName = Bid.Name;
            }
            TitleString = bidName + " - Estimate Builder";
        }
        
        private void startUp(string bidPath, string templatesPath)
        {
            templatesDB = new DatabaseManager(templatesPath);
            if (bidPath == "")
            {
                isNew = true;
            }
            else
            {
                isNew = false;
            }

            buildTitleString();
            setupCommands();
            setupExtensions(MenuType.EB);
            setupData();

            workingFileParameters = UIHelpers.EstimateFileParameters;
            CurrentVM = this;
        }
        private void setupScopeEditorVM(TECBid bid, TECTemplates templates)
        {
            ScopeEditorVM = new ScopeEditorVM(bid, templates);
            ScopeEditorVM.PropertyChanged += scopeEditorVM_PropertyChanged;
            if (TemplatesHidden)
            { ScopeEditorVM.TemplatesVisibility = Visibility.Hidden; }
            else
            { ScopeEditorVM.TemplatesVisibility = Visibility.Visible; }
        }
        private void setupLaborVM(TECBid bid, TECTemplates templates)
        {
            LaborVM = new LaborVM();
            LaborVM.Bid = bid;
            LaborVM.Templates = templates;
            LaborVM.LoadTemplates += LoadTemplatesExecute;
            LaborVM.TemplatesLoaded = templatesLoaded;
        }
        private void setupReviewVM(TECBid bid)
        {
            ReviewVM = new ReviewVM();
            ReviewVM.Bid = bid;
        }
        private void setupProposalVM(TECBid bid)
        {
            ProposalVM = new ProposalVM(bid);
        }
        private void setupElectricalVM(TECBid bid)
        {
            ElectricalVM = new ElectricalVM(bid);
        }
        private void toggleTemplatesExecute()
        {
            if (TemplatesHidden)
            {
                TemplatesHidden = false;
            }
            else
            {
                TemplatesHidden = true;
            }
        }
        private void documentExecute()
        {
            string path = UIHelpers.GetSavePath(UIHelpers.WordDocumentFileParameters, defaultSaveFileName,
                defaultDirectory, ScopeDirectoryPath, isNew);

            if (path != null)
            {
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    //ScopeDocumentBuilder.CreateScopeDocument(Bid, path, isEstimate);
                    var builder = new ScopeWordDocumentBuilder();
                    builder.CreateScopeWordDocument(Bid, Estimate, path);
                    DebugHandler.LogDebugMessage("Scope saved to document.");
                }
                else
                {
                    DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
                }
            }
        }
        private void cSVExportExecute()
        {
            //User choose path
            string path = UIHelpers.GetSavePath(UIHelpers.CSVFileParameters, defaultSaveFileName, defaultDirectory, ScopeDirectoryPath, isNew);
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
        private void budgetExecute()
        {
            //new View.BudgetWindow();
            //MessengerInstance.Send<GenericMessage<ObservableCollection<TECSystem>>>(new GenericMessage<ObservableCollection<TECSystem>>(Bid.Systems));
        }
        private void excelExportExecute()
        {
            //User choose path
            string path = UIHelpers.GetSavePath(UIHelpers.CSVFileParameters, defaultSaveFileName, defaultDirectory, ScopeDirectoryPath, isNew);
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
        private void refreshTemplatesExecute()
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
        private void refreshBidExecute()
        {
            if (!IsReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }
            if (deltaStack.CleansedStack().Count > 0 && deltaStack.CleansedStack().Count != loadedStackLength)
            {
                string message = "Would you like to save your changes before reloading?";
                MessageBoxResult result = MessageBox.Show(message, "Create new", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    SetBusyStatus("Saving...", false);
                    if (saveDelta(false))
                    {
                        SetBusyStatus("Loading...", false);
                        DebugHandler.LogDebugMessage("Reloading bid.");
                        refresh();
                    }
                    else
                    {
                        DebugHandler.LogError("Save unsuccessful. Reload bid did not occur..");
                    }
                    ResetStatus();
                }
                else if (result == MessageBoxResult.No)
                {
                    SetBusyStatus("Loading...", false);
                    DebugHandler.LogDebugMessage("Reloading bid.");
                    //Bid = load(saveFilePath) as TECBid;
                    ResetStatus();
                }
                else
                {
                    return;
                }
            }
            else
            {
                SetBusyStatus("Loading...", false);
                DebugHandler.LogDebugMessage("Reloading bid.");
                //Bid = load(saveFilePath) as TECBid;
                ResetStatus();
            }
        }
        private bool refreshBidCanExecute()
        {
            return (!isNew);
        }
        private void refresh()
        {
            if (Bid != null && Templates != null)
            {
                ScopeEditorVM.Refresh(Bid, Templates);
                LaborVM.Refresh(Bid, Templates);
                //ReviewVM.Refresh(Bid);
                ProposalVM.Refresh(Bid);
                ElectricalVM.Refresh(Bid);
            }
        }
        private void updateBidWithTemplates()
        {
            if (Templates != null && Bid != null)
            {
                UtilitiesMethods.UnionizeCatalogs(Bid.Catalogs, Templates.Catalogs);
                ModelLinkingHelper.LinkBidToCatalogs(Bid);
                if (isNew && Templates.Parameters.Count > 0)
                {
                    Bid.Parameters.UpdateConstants(Templates.Parameters[0]);
                    loadedStackLength = deltaStack.CleansedStack().Count;
                }
            }
        }
        private void loadTemplates(string TemplatesFilePath)
        {
            var loadedTemplates = new TECTemplates();
            if (TemplatesFilePath != null)
            {
                SetBusyStatus("Loading templates from file: " + TemplatesFilePath, false);
                DatabaseManager manager = new DatabaseManager(TemplatesFilePath);
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (s, e) =>
                {
                    if (!UtilitiesMethods.IsFileLocked(TemplatesFilePath))
                    {
                        loadedTemplates = manager.Load() as TECTemplates;
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
                    ResetStatus();
                };
                worker.RunWorkerAsync();
            }
        }
        private void setupData()
        {
            if (isNew)
            {
                Bid = new TECBid();
                watcher = new ChangeWatcher(Bid);
                doStack = new DoStacker(watcher);
                deltaStack = new DeltaStacker(watcher);
            }

            Templates = new TECTemplates();

            if (TemplatesFilePath == "" || !File.Exists(TemplatesFilePath))
            {
                TemplatesFilePath = null;
                string message = "No templates file loaded. Would you like to load templates?";
                MessageBoxResult result = MessageBox.Show(message, "Load Templates?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    //User choose path
                    string path = UIHelpers.GetLoadPath(UIHelpers.TemplatesFileParameters, defaultDirectory);
                    if (path != null)
                    {
                        TemplatesFilePath = path;
                    }
                    else
                    {
                        ResetStatus();
                    }
                }
                else
                {
                    templatesLoaded = false;
                    TemplatesFilePath = null;
                    ResetStatus();
                    return;
                }
            }

            if (!UtilitiesMethods.IsFileLocked(TemplatesFilePath))
            {
                loadTemplates(TemplatesFilePath);
                DebugHandler.LogDebugMessage("Finished loading templates.");
                templatesLoaded = true;
            }
            else
            {
                DebugHandler.LogError("TECTemplates file is open elsewhere. Could not load templates. Please close the templates file and load again.");
                templatesLoaded = false;
                ResetStatus();
            }
        }
        private void settingsVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TemplatesHidden")
            {
                TemplatesHidden = SettingsVM.TemplatesHidden;
            }
            else if (e.PropertyName == "TemplatesLoadPath")
            {
                TemplatesFilePath = SettingsVM.TemplatesLoadPath;
            }
        }
        private void scopeEditorVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TemplatesVisibility")
            {
                if (ScopeEditorVM.TemplatesVisibility == Visibility.Visible)
                {
                    TemplatesHidden = false;
                }
                else if (ScopeEditorVM.TemplatesVisibility == Visibility.Hidden)
                {
                    TemplatesHidden = true;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
        private void bid_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                buildTitleString();
            }
        }
        #endregion
    }
}