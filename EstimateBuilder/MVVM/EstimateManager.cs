﻿using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.BaseVMs;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.Utilities;
using EstimatingUtilitiesLibrary;
using EstimatingUtilitiesLibrary.Database;
using System.IO;
using System.Windows;
using System.ComponentModel;
using TECUserControlLibrary.Debug;
using EstimatingUtilitiesLibrary.Exports;
using NLog;

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
                throw new NotImplementedException("Need to construct file name from bid.");
            }
        }
        
        private string templatesFilePath
        {
            get { return Properties.Settings.Default.TemplatesFilePath; }
            set
            {
                Properties.Settings.Default.TemplatesFilePath = value;
                Properties.Settings.Default.Save();
            }
        }
        #endregion

        public EstimateManager() : base(new EstimateSplashVM(Properties.Settings.Default.TemplatesFilePath, Properties.Settings.Default.DefaultDirectory), new EstimateMenuVM())
        {
            splashVM.EditorStarted += userStartedEditorHandler;
            TitleString = "Estimate Builder";
            setupCommands();
        }
        
        private void userStartedEditorHandler(string bidFilePath, string templatesFilePath)
        {
            buildTitleString(bidFilePath, "Estimate Builder");
            templatesDatabaseManager = new DatabaseManager<TECTemplates>(templatesFilePath);
            templatesDatabaseManager.LoadComplete += scopeManager =>
            {
                templates = scopeManager as TECTemplates;
                if(bidFilePath != "")
                {
                    databaseManager = new DatabaseManager<TECBid>(bidFilePath);
                    databaseManager.LoadComplete += handleLoaded;
                    databaseManager.AsyncLoad();
                } 
                else
                {
                    handleLoaded(new TECBid());
                }
                
            };
            ViewEnabled = false;
            templatesDatabaseManager.AsyncLoad();
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
            throw new NotImplementedException();
        }

        #region Menu Commands Methods
        private void setupCommands()
        {
            menuVM.SetLoadTemplatesCommand(loadTemplatesExecute, canLoadTemplates);
            menuVM.SetRefreshBidCommand(refreshExecute, canRefresh);
            menuVM.SetRefreshTemplatesCommand(refreshTemplatesExecute, canRefreshTemplates);
            menuVM.SetExportProposalCommand(exportProposalExecute, canExportProposal);
            menuVM.SetExportPointsListCommand(exportPointsListExecute, canExportPointsList);
            menuVM.SetExportEngineeringCommand(exportEngineeringExecute, canExportEngineering);
            menuVM.SetDebugWindowCommand(debugWindowExecute, canDebugWindow);
        }
        //Load Templates
        private void loadTemplatesExecute()
        {
            ViewEnabled = false;
            string loadFilePath = UIHelpers.GetLoadPath(FileDialogParameters.TemplatesFileParameters, defaultDirectory);
            StatusBarVM.CurrentStatusText = "Loading Templates...";
            templatesDatabaseManager = new DatabaseManager<TECTemplates>(loadFilePath);
            templatesDatabaseManager.LoadComplete += handleTemplatesLoadComplete;
            templatesDatabaseManager.AsyncLoad();
        }
        private bool canLoadTemplates()
        {
            return true;
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
            ViewEnabled = false;
            checkForChanges(message, refreshTemplates);

            void refreshTemplates()
            {
                StatusBarVM.CurrentStatusText = "Loading...";
                templatesDatabaseManager.LoadComplete += handleTemplatesLoadComplete;
                templatesDatabaseManager.AsyncLoad();
            }
        }
        private bool canRefreshTemplates()
        {
            return templatesDatabaseManager != null;
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
                    logger.Warn("Could not open file " + path + " File is open elsewhere.");
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
                    Console.WriteLine("Points saved to csv.");
                }
                else
                {
                    Console.WriteLine("Could not open file " + path + " File is open elsewhere.");
                }
            }
        }
        private bool canExportPointsList()
        {
            return true;
        }
        //Export Engineering
        private void exportEngineeringExecute()
        {
            string path = UIHelpers.GetSavePath(FileDialogParameters.CSVFileParameters,
                                        defaultFileName, defaultDirectory, workingFileDirectory);
            if (path != null)
            {
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    TurnoverExporter.GenerateEngineeringExport(path, bid, estimate);
                    Console.WriteLine("Exported to engineering turnover document.");
                }
                else
                {
                    Console.WriteLine("Could not open file " + path + " File is open elsewhere.");
                }
            }
        }
        private bool canExportEngineering()
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

        protected override TECBid getWorkingScope()
        {
            return bid;
        }
        protected override TECBid getNewWorkingScope()
        {
            return new TECBid();
        }

    }
}
