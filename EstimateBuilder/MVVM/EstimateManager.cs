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

namespace EstimateBuilder.MVVM
{
    public class EstimateManager : AppManager
    {
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

        public EstimateManager() : base(new EstimateSplashVM(Properties.Settings.Default.TemplatesFilePath, Properties.Settings.Default.DefaultDirectory), new EstimateMenuVM())
        {
            splashVM.EditorStarted += userStartedEditorHandler;
        }

        private void userStartedEditorHandler(string bidFilePath, string templatesFilePath)
        {
            DatabaseManager templatesManager = new DatabaseManager(templatesFilePath);
            templates = templatesManager.Load() as TECTemplates;
            databaseManager = new DatabaseManager(bidFilePath);
            databaseManager.LoadComplete += databaseManager_bidLoaded;
            databaseManager.AsyncLoad();
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
        }

        private void setupCommands()
        {

        }
        private void saveDeltaExecute()
        {
            if (databaseManager != null)
            {
                databaseManager.Save(deltaStack.CleansedStack());
            }
            else
            {
                string savePath = UIHelpers.GetSavePath(workingFileParameters, defaultFileName, defaultDirectory, workingFileDirectory);
                throw new NotImplementedException("Need to handle save path return.");
            }

            throw new NotImplementedException("Need a method for clearing the delta stack.");
        }
        private bool canSaveDelta()
        {
            return deltaStack.CleansedStack().Count > 0;
        }
    }
}
