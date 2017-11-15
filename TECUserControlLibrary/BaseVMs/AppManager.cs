using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using EstimatingUtilitiesLibrary;
using EstimatingUtilitiesLibrary.Database;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.ComponentModel;
using System.Deployment.Application;
using System.Drawing.Imaging;
using System.IO;
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
        /// <summary>
        /// Exclusively for use instantiating the delta stacker
        /// </summary>
        private bool isTemplates = false;
        protected string appName { get; }

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

        public AppManager(string name, SplashVM splashVM, MenuVM menuVM)
        {
            if(typeof(T) == typeof(TECTemplates)) { isTemplates = true; }
            appName = name;
            ViewEnabled = true;
            Version = getVersionNumber();
            TECLogo = getLogo();

            SplashVM = splashVM;
            SplashVM.Version = Version;
            MenuVM = menuVM;
            StatusBarVM = new StatusBarVM();
            StatusBarVM.SetVersionNumber(Version);
            StatusBarVM.CurrentStatusText = "Ready";
            setupCommands();
            CurrentVM = SplashVM;
            ClosingCommand = new RelayCommand<CancelEventArgs>(closingExecute);
        }
        
        #region Menu Commands Methods
        private void setupCommands()
        {
            MenuVM.SetUndoCommand(undoExecute, canUndo);
            MenuVM.SetRedoCommand(redoExecute, canRedo);
            MenuVM.SetNewCommand(newExecute, newCanExecute);
            MenuVM.SetLoadCommand(loadExecute, loadCanExecute);
            MenuVM.SetSaveDeltaCommand(saveDeltaExecute, canSaveDelta);
            MenuVM.SetSaveNewCommand(saveNewExecute, canSaveNew);
        }
        //New
        private void newExecute()
        {
            string message = "Would you like to save your current changes?";
            checkForChanges(message, () => {
                databaseManager = null;
                handleLoaded(getNewWorkingScope());
            });
        }
        private bool newCanExecute()
        {
            return databaseReady();
        }
        //Load
        private void loadExecute()
        {
            string message = "Would you like to save your changes before loading?";
            string loadFilePath;
            checkForChanges(message, () =>
            {
                loadFilePath = UIHelpers.GetLoadPath(workingFileParameters, defaultDirectory, workingFileDirectory);
                if(loadFilePath != null)
                {
                    ViewEnabled = false;
                    StatusBarVM.CurrentStatusText = "Loading...";
                    buildTitleString(loadFilePath, appName);
                    databaseManager = new DatabaseManager<T>(loadFilePath);
                    databaseManager.LoadComplete += handleLoadComplete;
                    databaseManager.AsyncLoad();
                }
            });
        }
        private bool loadCanExecute()
        {
            return databaseReady();
        }
        protected void handleLoadComplete(T scopeManager)
        {
            handleLoaded(scopeManager);
            StatusBarVM.CurrentStatusText = "Ready";
            ViewEnabled = true;
        }
        protected abstract void handleLoaded(T scopeManager);
        //Save Delta
        private void saveDeltaExecute()
        {
            if (databaseManager != null)
            {
                StatusBarVM.CurrentStatusText = "Saving...";
                databaseManager.SaveComplete += handleSaveDeltaComplete;
                databaseManager.AsyncSave(deltaStack.CleansedStack());
                deltaStack = new DeltaStacker(watcher, isTemplates);
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
                databaseManager.AsyncNew(getWorkingScope());
            }
        }        
        //Save New
        private void saveNewExecute()
        {
            string saveFilePath = UIHelpers.GetSavePath(workingFileParameters, defaultFileName, defaultDirectory, workingFileDirectory);
            if(saveFilePath != null)
            {
                StatusBarVM.CurrentStatusText = "Saving...";
                databaseManager = new DatabaseManager<T>(saveFilePath);
                databaseManager.SaveComplete += handleSaveNewComplete;
                databaseManager.AsyncNew(getWorkingScope());
            }
        }
        private bool canSaveNew()
        {
            return databaseReady();
        }
        protected void handleSaveNewComplete(bool success)
        {
            if (success)
            {
                StatusBarVM.CurrentStatusText = "Ready";
                deltaStack = new DeltaStacker(watcher, isTemplates);
            }
            else
            {
                MessageBox.Show("File failed to save. Contact technical support.");
            }
        }
        //Refresh
        protected void refreshExecute()
        {
            string message = "Would you like to save your changes before refreshing?";
            ViewEnabled = false;
            checkForChanges(message, refresh);

            void refresh()
            {
                StatusBarVM.CurrentStatusText = "Loading...";
                databaseManager.LoadComplete += handleLoadComplete;
                databaseManager.AsyncLoad();
            }
        }
        protected bool canRefresh()
        {
            return databaseManager != null && databaseReady();
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

        protected string getVersionNumber()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                return ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            else
            {
                return "Undeployed";
            }
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
        protected bool databaseReady()
        {
            if(databaseManager == null || !databaseManager.IsBusy)
            {
                return true;
            }
            return false;
        }

        protected abstract T getWorkingScope();
        protected abstract T getNewWorkingScope();

        private bool saveNewBeforeClosing()
        {
            string saveFilePath = UIHelpers.GetSavePath(workingFileParameters, defaultFileName, defaultDirectory, workingFileDirectory);
            try
            {
                databaseManager = new DatabaseManager<T>(saveFilePath);
                return databaseManager.New(getWorkingScope());
            }
            catch
            {
                return false;
            }
        }
        private void closingExecute(CancelEventArgs e)
        {
            if (databaseManager == null || !databaseManager.IsBusy)
            {
                bool changesExist = (deltaStack?.CleansedStack().Count > 0);
                if (changesExist)
                {
                    MessageBoxResult result = MessageBox.Show("You have unsaved changes. Would you like to save before quitting?", "Save?", MessageBoxButton.YesNoCancel);
                    if (result == MessageBoxResult.Yes)
                    {
                        if(databaseManager == null)
                        {
                            if (!saveNewBeforeClosing())
                            {
                                MessageBox.Show("Save unsuccessful.");
                                e.Cancel = true;
                                return;
                            }
                        }

                        else if (!databaseManager.Save(deltaStack.CleansedStack()))
                        {
                            MessageBox.Show("Save unsuccessful.");
                            e.Cancel = true;
                            return;
                        }
                    }
                    else if (result == MessageBoxResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                }
                if (!e.Cancel)
                {
                    Properties.Settings.Default.Save();
                }
            }
            else
            {
                e.Cancel = true;
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
            }
        }
    }
}
