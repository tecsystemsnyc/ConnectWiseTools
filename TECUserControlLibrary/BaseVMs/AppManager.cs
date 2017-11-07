using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using EstimatingUtilitiesLibrary;
using EstimatingUtilitiesLibrary.Database;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Deployment.Application;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.Utilities;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.BaseVMs
{
    abstract public class AppManager<T>  : ViewModelBase where T : TECScopeManager
    {
        protected DatabaseManager<T> databaseManager;
        protected ChangeWatcher watcher;
        protected DoStacker doStack;
        protected DeltaStacker deltaStack;
        protected string workingFileDirectory;
        private string _titleString;
        private object _currentVM;
        private bool _viewEnabled;

        #region Properties
        /// <summary>
        /// Generic for presentation purposes.
        /// </summary>
        public EditorVM EditorVM { get; protected set; }
        /// <summary>
        /// Generic for presentation purposes.
        /// </summary>
        public SplashVM SplashVM { get; }
        /// <summary>
        /// Generic for presentation purposes.
        /// </summary>
        public MenuVM MenuVM { get; }
        public StatusBarVM StatusBarVM { get; }

        public object CurrentVM
        {
            get { return _currentVM; }
            set
            {
                _currentVM = value;
                RaisePropertyChanged("CurrentVM");
            }
        }

        public bool ViewEnabled
        {
            get { return _viewEnabled; }
            set
            {
                _viewEnabled = value;
                RaisePropertyChanged("ViewEnabled");
            }
        }
        public string TECLogo { get; }
        public string TitleString
        {
            get { return _titleString; }
            set
            {
                _titleString = value;
                RaisePropertyChanged("TitleString");
            }
        }
        public string Version { get; }
        public RelayCommand<CancelEventArgs> ClosingCommand { get; private set; }

        abstract protected FileDialogParameters workingFileParameters { get; }
        abstract protected string defaultDirectory { get; set; }
        abstract protected string defaultFileName { get; }
        #endregion

        public AppManager(SplashVM splashVM, MenuVM menuVM)
        {
            ViewEnabled = true;
            Version = getVersion();
            TECLogo = getLogo();

            SplashVM = splashVM;
            MenuVM = menuVM;
            StatusBarVM = new StatusBarVM();
            setupCommands();
            CurrentVM = SplashVM;
        }
        
        #region Menu Commands Methods
        private void setupCommands()
        {
            MenuVM.SetUndoCommand(undoExecute, canUndo);
            MenuVM.SetRedoCommand(redoExecute, canRedo);
            MenuVM.SetLoadCommand(loadExecute, loadCanExecute);
            MenuVM.SetSaveDeltaCommand(saveDeltaExecute, canSaveDelta);
            MenuVM.SetSaveNewCommand(saveNewExecute, canSaveNew);
        }
        //Load
        private void loadExecute()
        {
            string message = "Would you like to save your changes before loading?";
            ViewEnabled = false;
            string loadFilePath;
            checkForChanges(message, () =>
            {
                loadFilePath = UIHelpers.GetLoadPath(workingFileParameters, defaultDirectory, workingFileDirectory);
                StatusBarVM.CurrentStatusText = "Loading...";
                databaseManager = new DatabaseManager<T>(loadFilePath);
                databaseManager.LoadComplete += handleLoadComplete;
                databaseManager.AsyncLoad();
            });
        }
        private bool loadCanExecute()
        {
            return true;
        }
        protected abstract void handleLoadComplete(T scopeManager);
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
        protected abstract void handleSaveDeltaComplete(bool success);
        //Save New
        private void saveNewExecute()
        {
            string saveFilePath = UIHelpers.GetSavePath(workingFileParameters, defaultFileName, defaultDirectory, workingFileDirectory);
            StatusBarVM.CurrentStatusText = "Saving...";
            databaseManager = new DatabaseManager<T>(saveFilePath);
            databaseManager.SaveComplete += handleSaveNewComplete;
            databaseManager.AsyncNew(getWorkingScope());
        }
        private bool canSaveNew()
        {
            return true;
        }
        protected void handleSaveNewComplete(bool success)
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
        //Undo
        private void undoExecute()
        {
            doStack.Undo();
        }
        private bool canUndo()
        {
            return doStack.UndoCount() > 0;
        }
        //Redo
        private void redoExecute()
        {
            doStack.Redo();
        }
        private bool canRedo()
        {
            return doStack.RedoCount() > 0;
        }
        #endregion

        private string getVersion()
        {
            String version = "";
            if (ApplicationDeployment.IsNetworkDeployed)
            { version = "Version " + ApplicationDeployment.CurrentDeployment.CurrentVersion; }
            else
            { version = "Undeployed Version"; }
            return version;
        }
        private string getLogo()
        {
            String path = Path.GetTempFileName();

            (Properties.Resources.TECLogo).Save(path, ImageFormat.Png);
            return path;
        }
        protected void buildTitleString(string filePath, string appName)
        {
            string title = Path.GetFileNameWithoutExtension(filePath);
            TitleString = title + " - " + appName;
        }
        protected void checkForChanges(string taskMessage, Action onComplete)
        {
            if (deltaStack.CleansedStack().Count > 0)
            {
                MessageBoxResult result = MessageBox.Show(taskMessage, "Save", MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Exclamation);
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
        protected abstract TECScopeManager getWorkingScope();
    }
}
