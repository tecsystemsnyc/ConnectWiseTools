using EstimatingLibrary;
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

namespace EstimateBuilder.MVVM
{
    public class EstimateManager : AppManager
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

        private string bidFilePath;
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
                    databaseManager.LoadComplete += databaseManager_bidLoaded;
                    databaseManager.AsyncLoad();
                } 
                else
                {
                    databaseManager_bidLoaded(new TECBid());
                }
                
            };
            ViewEnabled = false;
            templatesManager.AsyncLoad();
        }

        private void databaseManager_bidLoaded(TECScopeManager loadedBid)
        {
            bid = loadedBid as TECBid;
            watcher = new ChangeWatcher(bid);
            doStack = new DoStacker(watcher);
            deltaStack = new DeltaStacker(watcher);

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
                databaseManager_bidLoaded(new TECBid());
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
            checkForChanges(message, () =>
            {
                
            });
        }
        private bool loadCanExecute()
        {
            return true;
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
            if (success)
            {
                StatusBarVM.CurrentStatusText = "Ready";
            }
            else
            {
                databaseManager.SaveComplete += handleSaveNewComplete;
                databaseManager.AsyncNew(bid);
            }
            databaseManager.SaveComplete -= handleSaveDeltaComplete;
        }
        //Save New
        private void saveNewExecute()
        {
            throw new NotImplementedException();
        }
        private bool canSaveNew()
        {
            throw new NotImplementedException();
        }
        private void handleSaveNewComplete(bool success)
        {
            throw new NotImplementedException();
        }
        //Load Templates
        private void loadTemplatesExecute()
        {
            throw new NotImplementedException();
        }
        private bool canLoadTemplates()
        {
            throw new NotImplementedException();
        }
        //Refresh Bid
        private void refreshBidExecute()
        {
            throw new NotImplementedException();
        }
        private bool canRefreshBid()
        {
            throw new NotImplementedException();
        }
        //Refresh Templates
        private void refreshTemplatesExecute()
        {
            throw new NotImplementedException();
        }
        private bool canRefreshTemplates()
        {
            throw new NotImplementedException();
        }
        //Export Proposal
        private void exportProposalExecute()
        {
            throw new NotImplementedException();
        }
        private bool canExportProposal()
        {
            throw new NotImplementedException();
        }
        //Export Points List
        private void exportPointsListExecute()
        {
            throw new NotImplementedException();
        }
        private bool canExportPointsList()
        {
            throw new NotImplementedException();
        }
        //Export Engineering
        private void exportEngineeringExecute()
        {
            throw new NotImplementedException();
        }
        private bool canExportEngineering()
        {
            throw new NotImplementedException();
        }
        //Debug Window
        private void debugWindowExecute()
        {
            throw new NotImplementedException();
        }
        private bool canDebugWindow()
        {
            throw new NotImplementedException();
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
                if (success)
                    onComplete();
                else
                    return;
                databaseManager.SaveComplete -= saveComplete;
            }
        }
    }
}
