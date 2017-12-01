using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using EstimatingUtilitiesLibrary;
using EstimatingUtilitiesLibrary.Database;
using EstimatingUtilitiesLibrary.Exports;
using NLog;
using System;
using TECUserControlLibrary.BaseVMs;
using TECUserControlLibrary.Debug;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.Utilities;

namespace EstimateBuilder.MVVM
{
    public class EstimateManager : AppManager<TECBid>
    {
        #region Fields and Properties
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private TECBid bid;
        private TECTemplates templates;
        private TECEstimator estimate;

        private DatabaseManager<TECTemplates> templatesDatabaseManager;

        /// <summary>
        /// Estimate-typed splash vm for manipulation
        /// </summary>
        private EstimateMenuVM menuVM
        {
            get { return MenuVM as EstimateMenuVM; }
        }
        /// <summary>
        /// Estimate-typed splash vm for manipulation
        /// </summary>
        private EstimateEditorVM editorVM
        {
            get { return EditorVM as EstimateEditorVM; }
        }
        /// <summary>
        /// Estimate-typed splash vm for manipulation
        /// </summary>
        private EstimateSplashVM splashVM
        {
            get { return SplashVM as EstimateSplashVM; }
        }

        override protected FileDialogParameters workingFileParameters
        {
            get
            {
                return FileDialogParameters.EstimateFileParameters;
            }
        }
        override protected string defaultDirectory
        {
            get
            {
                return Properties.Settings.Default.DefaultDirectory;
            }
            set
            {
                Properties.Settings.Default.DefaultDirectory = value;
                Properties.Settings.Default.Save();
            }
        }
        override protected string defaultFileName
        {
            get
            {
                string fileName = "";
                if (bid.Name != null && bid.Name != "")
                {
                    fileName += bid.Name;
                    if (bid.BidNumber != null && bid.BidNumber != "")
                    {
                        fileName += (" - " + bid.BidNumber);
                    }
                }
                return fileName;
            }
        }
        
        override protected string templatesFilePath
        {
            get { return Properties.Settings.Default.TemplatesFilePath; }
            set
            {
                Properties.Settings.Default.TemplatesFilePath = value;
                Properties.Settings.Default.Save();
            }
        }
        #endregion

        public EstimateManager() : base("Estimate Builder", 
            new EstimateSplashVM(Properties.Settings.Default.TemplatesFilePath, Properties.Settings.Default.DefaultDirectory), new EstimateMenuVM())
        {
            splashVM.BidPath = getStartUpFilePath();
            splashVM.EditorStarted += userStartedEditorHandler;
            TitleString = "Estimate Builder";
            setupCommands();
        }
        
        private void userStartedEditorHandler(string bidFilePath, string templatesFilePath)
        {
            this.templatesFilePath = templatesFilePath;
            buildTitleString(bidFilePath, "Estimate Builder");
            if(templatesFilePath != "")
            {
                templatesDatabaseManager = new DatabaseManager<TECTemplates>(templatesFilePath);
                templatesDatabaseManager.LoadComplete += assignData;
                ViewEnabled = false;
                templatesDatabaseManager.AsyncLoad();
            } else
            {
                assignData(new TECTemplates());
            }
            

            void assignData(TECTemplates loadedTemplates)
            {
                templates = loadedTemplates;
                if (bidFilePath != "")
                {
                    databaseManager = new DatabaseManager<TECBid>(bidFilePath);
                    databaseManager.LoadComplete += handleLoaded;
                    databaseManager.AsyncLoad();
                }
                else
                {
                    handleLoaded(getNewWorkingScope());
                }
            }
        }

        protected override void handleLoaded(TECBid loadedBid)
        {
            bid = loadedBid;
            watcher = new ChangeWatcher(bid);
            doStack = new DoStacker(watcher);
            deltaStack = new DeltaStacker(watcher);
            bid.Catalogs.Unionize(templates.Catalogs);

            estimate = new TECEstimator(bid, watcher);

            EditorVM = new EstimateEditorVM(bid, templates, watcher, estimate);
            CurrentVM = EditorVM;
            ViewEnabled = true;
        }
        private void handleLoadedTemplates(TECTemplates templates)
        {
            this.templates = templates;
            bid.Catalogs.Unionize(templates.Catalogs);
            estimate = new TECEstimator(bid, watcher);
            editorVM.Refresh(bid, this.templates, watcher, estimate);
        }

        #region Menu Commands Methods
        private void setupCommands()
        {
            menuVM.SetLoadTemplatesCommand(loadTemplatesExecute, canLoadTemplates);
            menuVM.SetRefreshBidCommand(refreshExecute, canRefresh);
            menuVM.SetRefreshTemplatesCommand(refreshTemplatesExecute, canRefreshTemplates);
            menuVM.SetExportProposalCommand(exportProposalExecute, canExportProposal);
            menuVM.SetExportPointsListCommand(exportPointsListExecute, canExportPointsList);
            menuVM.SetExportSummaryCommand(exportSummaryExecute, canExportSummary);
            menuVM.SetExportBudgetCommand(exportBudgetExecute, canExportBudget);
            menuVM.SetExportBOMCommand(exportBOMExecute, canExportBOM);
            menuVM.SetDebugWindowCommand(debugWindowExecute, canDebugWindow);
        }
        //Load Templates
        private void loadTemplatesExecute()
        {
            string loadFilePath = UIHelpers.GetLoadPath(FileDialogParameters.TemplatesFileParameters, defaultDirectory);
            if(loadFilePath != null)
            {
                ViewEnabled = false;
                StatusBarVM.CurrentStatusText = "Loading Templates...";
                templatesDatabaseManager = new DatabaseManager<TECTemplates>(loadFilePath);
                templatesDatabaseManager.LoadComplete += handleTemplatesLoadComplete;
                templatesDatabaseManager.AsyncLoad();
            }
        }
        private bool canLoadTemplates()
        {
            return databaseReady();
        }
        private void handleTemplatesLoadComplete(TECTemplates templates)
        {
            handleLoadedTemplates(templates);
            StatusBarVM.CurrentStatusText = "Ready";
            ViewEnabled = true;
        }
        //Refresh Templates
        private void refreshTemplatesExecute()
        {
            string message = "Would you like to save your changes before refreshing?";
            checkForChanges(message, refreshTemplates, () => { ViewEnabled = true; });

            void refreshTemplates()
            {
                ViewEnabled = false;
                StatusBarVM.CurrentStatusText = "Loading...";
                templatesDatabaseManager.LoadComplete += handleTemplatesLoadComplete;
                templatesDatabaseManager.AsyncLoad();
            }
        }
        private bool canRefreshTemplates()
        {
            return templatesDatabaseManager != null && databaseReady();
        }
        //Export Proposal
        private void exportProposalExecute()
        {
            string path = UIHelpers.GetSavePath(FileDialogParameters.WordDocumentFileParameters, 
                defaultFileName, defaultDirectory, workingFileDirectory);

            if (path != null)
            {
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    ScopeWordDocumentBuilder.CreateScopeWordDocument(bid, estimate, path);
                    logger.Info("Scope saved to document.");
                }
                else
                {
                    logger.Warn("Could not open file {0}. File is open elsewhere.", path);
                }
            }
        }
        private bool canExportProposal()
        {
            return true;
        }
        //Export Points List
        private void exportPointsListExecute()
        {
            string path = UIHelpers.GetSavePath(FileDialogParameters.CSVFileParameters,
                            defaultFileName, defaultDirectory, workingFileDirectory);
            if (path != null)
            {
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    CSVWriter writer = new CSVWriter(path);
                    writer.BidPointsToCSV(bid);
                    logger.Info("Points saved to csv.");
                }
                else
                {
                    logger.Warn("Could not open file {0}. File is open elsewhere.", path);
                }
            }
        }
        private bool canExportPointsList()
        {
            return true;
        }
        //Export Summary
        private void exportSummaryExecute()
        {
            string path = UIHelpers.GetSavePath(FileDialogParameters.WordDocumentFileParameters,
                                        defaultFileName, defaultDirectory, workingFileDirectory);
            if (path != null)
            {
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    Turnover.GenerateSummaryExport(path, bid, estimate);
                    logger.Info("Exported to summary turnover document.");
                }
                else
                {
                    logger.Warn("Could not open file {0}. File is open elsewhere.", path);
                }
            }
        }
        private bool canExportSummary()
        {
            return true; 
        }
        //Export Budget
        private void exportBudgetExecute()
        {
            string path = UIHelpers.GetSavePath(FileDialogParameters.ExcelFileParameters,
                                        defaultFileName, defaultDirectory, workingFileDirectory);
            if (path != null)
            {
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    Budget.GenerateReport(path, bid);
                    logger.Info("Exported to budget document.");
                }
                else
                {
                    logger.Warn("Could not open file {0}. File is open elsewhere.", path);
                }
            }
        }
        private bool canExportBudget()
        {
            return true;
        }
        //Export Budget
        private void exportBOMExecute()
        {
            string path = UIHelpers.GetSavePath(FileDialogParameters.ExcelFileParameters,
                                        defaultFileName, defaultDirectory, workingFileDirectory);
            if (path != null)
            {
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    Turnover.GenerateBOM(path, bid);
                    logger.Info("Exported to BOM document.");
                }
                else
                {
                    logger.Warn("Could not open file {0}. File is open elsewhere.", path);
                }
            }
        }
        private bool canExportBOM()
        {
            return true;
        }
        //Debug Window
        private void debugWindowExecute()
        {
            var dbWindow = new EBDebugWindow(bid);
            dbWindow.Show();
        }
        private bool canDebugWindow()
        {
            return true;
        }
        #endregion
        
        private string getStartUpFilePath()
        {
            string startUpFilePath = Properties.Settings.Default.StartUpFilePath;
            Properties.Settings.Default.StartUpFilePath = null;
            Properties.Settings.Default.Save();
            return startUpFilePath;
        }
        protected override TECBid getWorkingScope()
        {
            return bid;
        }
        protected override TECBid getNewWorkingScope()
        {
            TECBid outBid = new TECBid();
            if(templates!= null && templates.Parameters.Count > 0)
            {
                outBid.Parameters = templates.Parameters[0];
            }
            return outBid;
        }

    }
}
