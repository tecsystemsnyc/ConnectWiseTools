using DebugLibrary;
using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using EstimatingUtilitiesLibrary;
using EstimatingUtilitiesLibrary.DatabaseHelpers;
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

namespace TECUserControlLibrary.ViewModels
{
    abstract public class BidEditorBase : BuilderViewModel
    {
        #region Properties

        private bool _templatesLoaded;
        virtual protected bool templatesLoaded
        {
            get { return _templatesLoaded; }
            set
            {
                _templatesLoaded = value;
            }
        }
        protected bool isEstimate;

        private TECEstimator _estimate;

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
                    Bid.PropertyChanged -= Bid_PropertyChanged;
                }
                base.workingScopeManager = value;
                RaisePropertyChanged("Bid");
                Bid.PropertyChanged += Bid_PropertyChanged;
                buildTitleString();
                updateBidWithTemplates();
                refresh();
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
        public ICommand RefreshBidCommand { get; private set; }
        #endregion

        #endregion

        public BidEditorBase() : base()
        {
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
            RefreshBidCommand = new RelayCommand(RefreshBidExecute, RefreshBidCanExecute);
        }
        private void setupData()
        {
            if (isNew)
            {
                Bid = new TECBid();
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
                    string path = getLoadPath(TemplatesFileParameters);
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
        protected override void setupMenu()
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

            //Toggle Templates Command gets handled in each MainView model for ScopeBuilder and EstimateBuilder
        }

        private void updateBidWithTemplates()
        {
            if (Templates != null && Bid != null)
            {
                UtilitiesMethods.UnionizeCatalogs(Bid.Catalogs, Templates.Catalogs);
                ModelLinkingHelper.LinkBidToCatalogs(Bid);
                if (isNew)
                {
                    Bid.Labor.UpdateConstants(Templates.Labor);
                    loadedStackLength = deltaStack.CleansedStack().Count;
                }
            }
        }
        #endregion
        #region Refresh
        protected abstract void refresh();
        #endregion
        #region Helper Functions
        protected override void buildTitleString()
        {
            string bidName = "";
            if (Bid != null)
            {
                bidName = Bid.Name;
            }
            TitleString = bidName + " - " + programName;
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
                        loadedTemplates = DatabaseLoader.Load(TemplatesFilePath) as TECTemplates;
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
        private void DocumentExecute()
        {
            string path = getSavePath(WordDocumentFileParameters, defaultSaveFileName, ScopeDirectoryPath);

            if (path != null)
            {
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    //ScopeDocumentBuilder.CreateScopeDocument(Bid, path, isEstimate);
                    var builder = new ScopeWordDocumentBuilder();
                    builder.CreateScopeWordDocument(Bid, Estimate, path, isEstimate);
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
        private void RefreshBidExecute()
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
                    Bid = loadFromPath(saveFilePath) as TECBid;
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
                Bid = loadFromPath(saveFilePath) as TECBid;
                ResetStatus();
            }
        }
        private bool RefreshBidCanExecute()
        {
            return (!isNew);
        }
        #endregion
        #endregion
    }
}