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

namespace EstimateBuilder.MVVM
{
    public class EstimateManager : AppManager<TECBid>
    {
        #region Fields and Properties
        private TECBid bid;
        private TECTemplates templates;

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
            buildTitleString(bidFilePath);
            DatabaseManager templatesManager = new DatabaseManager(templatesFilePath);
            templatesManager.LoadComplete += scopeManager =>
            {
                templates = scopeManager as TECTemplates;
                if(bidFilePath != "")
                {
                    databaseManager = new DatabaseManager(bidFilePath);
                    databaseManager.LoadComplete += handleLoadedBid;
                    databaseManager.AsyncLoad();
                } 
                else
                {
                    handleLoadedBid(new TECBid());
                }
                
            };
            ViewEnabled = false;
            templatesManager.AsyncLoad();
        }

        private void handleLoadedBid(TECScopeManager loadedBid)
        {
            bid = loadedBid as TECBid;
            watcher = new ChangeWatcher(bid);
            doStack = new DoStacker(watcher);
            deltaStack = new DeltaStacker(watcher);
            bid.Catalogs.Unionize(templates.Catalogs);

            TECEstimator estimate = new TECEstimator(bid, watcher);

            EditorVM = new EstimateEditorVM(bid, templates, watcher, estimate);
            CurrentVM = EditorVM;
            ViewEnabled = true;
        }

        #region Menu Commands Methods
        private void setupCommands()
        {
            menuVM.SetNewCommand(newExecute, newCanExecute);
            menuVM.SetLoadCommand(loadExecute, loadCanExecute);
            menuVM.SetSaveDeltaCommand(saveDeltaExecute, canSaveDelta);
            menuVM.SetSaveNewCommand(saveNewExecute, canSaveNew);
            menuVM.SetLoadTemplatesCommand(loadTemplatesExecute, canLoadTemplates);
            menuVM.SetRefreshBidCommand(refreshBidExecute, canRefreshBid);
            menuVM.SetRefreshTemplatesCommand(refreshTemplatesExecute, canRefreshTemplates);
            menuVM.SetExportProposalCommand(exportProposalExecute, canExportProposal);
            menuVM.SetExportPointsListCommand(exportPointsListExecute, canExportPointsList);
            menuVM.SetExportEngineeringCommand(exportEngineeringExecute, canExportEngineering);
            menuVM.SetDebugWindowCommand(debugWindowExecute, canDebugWindow);
        }
        //New
        private void newExecute()
        {
            string message = "Would you like to save your changed before creating a new bid?";
            checkForChanges(message, () => {
                handleLoadedBid(new TECBid());
            });
        }
        private bool newCanExecute()
        {
            return true;
        }
        //Load
        private void loadExecute()
        {
            string message = "Would you like to save your changed before loading?";
            ViewEnabled = false;
            string loadFilePath;
            checkForChanges(message, () =>
            {
                loadFilePath = UIHelpers.GetLoadPath(FileDialogParameters.EstimateFileParameters, defaultDirectory, workingFileDirectory);
                StatusBarVM.CurrentStatusText = "Loading...";
                databaseManager = new DatabaseManager(loadFilePath);
                databaseManager.LoadComplete += handleLoadComplete;
                databaseManager.AsyncLoad();
            });
        }
        private bool loadCanExecute()
        {
            return true;
        }
        private void handleLoadComplete(TECScopeManager bid)
        {
            handleLoadedBid(bid);
            StatusBarVM.CurrentStatusText = "Ready";
            ViewEnabled = true;
        }
        //Save Delta
        private void saveDeltaExecute()
        {
            if (databaseManager != null)
            {
                StatusBarVM.CurrentStatusText = "Saving...";
                databaseManager.SaveComplete += handleSaveDeltaComplete;
                databaseManager.AsyncSave(deltaStack.CleansedStack());
                deltaStack = new DeltaStacker(watcher);
            }
            else
            {
                saveNewExecute();
            }
        }
        private bool canSaveDelta()
        {
            return deltaStack.CleansedStack().Count > 0;
        }
        private void handleSaveDeltaComplete(bool success)
        {
            databaseManager.SaveComplete -= handleSaveDeltaComplete;
            if (success)
            {
                StatusBarVM.CurrentStatusText = "Ready";
            }
            else
            {
                databaseManager.SaveComplete += handleSaveNewComplete;
                databaseManager.AsyncNew(bid);
            }
        }
        //Save New
        private void saveNewExecute()
        {
            string saveFilePath = UIHelpers.GetSavePath(FileDialogParameters.EstimateFileParameters, defaultFileName, defaultDirectory, workingFileDirectory);
            StatusBarVM.CurrentStatusText = "Saving...";
            databaseManager = new DatabaseManager(saveFilePath);
            databaseManager.SaveComplete += handleSaveNewComplete;
            databaseManager.AsyncNew(bid);
        }
        private bool canSaveNew()
        {
            return true;
        }
        private void handleSaveNewComplete(bool success)
        {
            if (success)
            {
                StatusBarVM.CurrentStatusText = "Ready";
            }
            else
            {
                MessageBox.Show("File failed to save. Contact technical support.");
            }
        }
        //Load Templates
        private void loadTemplatesExecute()
        {
            throw new NotImplementedException();
        }
        private bool canLoadTemplates()
        {
            return true;
        }
        //Refresh Bid
        private void refreshBidExecute()
        {
            throw new NotImplementedException();
        }
        private bool canRefreshBid()
        {
            return true;
        }
        //Refresh Templates
        private void refreshTemplatesExecute()
        {
            throw new NotImplementedException();
        }
        private bool canRefreshTemplates()
        {
            return true;
        }
        //Export Proposal
        private void exportProposalExecute()
        {
            throw new NotImplementedException();
        }
        private bool canExportProposal()
        {
            return true;
        }
        //Export Points List
        private void exportPointsListExecute()
        {
            throw new NotImplementedException();
        }
        private bool canExportPointsList()
        {
            return true;
        }
        //Export Engineering
        private void exportEngineeringExecute()
        {
            throw new NotImplementedException();
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

        private void buildTitleString(string filePath)
        {
            string title = Path.GetFileNameWithoutExtension(filePath);
            TitleString = title + " - Estimate Builder";
        }
        private void checkForChanges(string taskMessage, Action onComplete)
        {
            if(deltaStack.CleansedStack().Count > 0)
            {
                MessageBoxResult result = MessageBox.Show(taskMessage, "Save", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        databaseManager.SaveComplete += saveComplete;
                        saveDeltaExecute();
                        break;
                    case MessageBoxResult.No:
                        onComplete();
                        break;
                    default:
                        return;
                }
            }
            else
                onComplete();
            
            void saveComplete(bool success)
            {
                databaseManager.SaveComplete -= saveComplete;
                if (success)
                    onComplete();
                else
                    return;
            }
        }
    }
}
