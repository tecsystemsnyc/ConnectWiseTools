using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using EstimatingUtilitiesLibrary;
using EstimatingUtilitiesLibrary.Database;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NLog;
using System;
using System.ComponentModel;
using System.Deployment.Application;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.Utilities;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.BaseVMs
{
    abstract public class AppManager<T>  : ViewModelBase where T : TECScopeManager
    {
        private const string WIKI_LINK = "https://github.com/tecsystemsnyc/EstimatingTools/wiki";

        static private Logger logger = LogManager.GetCurrentClassLogger();

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
        public ImageSource TECLogo { get; }
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
        public RelayCommand<(CancelEventArgs, Window)> ClosingCommand { get; private set; }

        abstract protected FileDialogParameters workingFileParameters { get; }
        abstract protected string defaultDirectory { get; set; }
        abstract protected string defaultFileName { get; }
        abstract protected string templatesFilePath { get; set; }
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
            ClosingCommand = new RelayCommand<(CancelEventArgs, Window)>(((CancelEventArgs, Window) tup) 
                => closingExecute(tup.Item1, tup.Item2));
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
            MenuVM.SetWikiCommand(wikiExecute);
        }
        //New
        private void newExecute()
        {
            logger.Info("User clicked File->New");
            string message = "Would you like to save your current changes?";
            checkForChanges(message, () => {
                logger.Info("Instantiating new Working Scope.");
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
            logger.Info("User clicked File->Load");
            string message = "Would you like to save your changes before loading?";
            string loadFilePath;
            checkForChanges(message, () =>
            {
                loadFilePath = UIHelpers.GetLoadPath(workingFileParameters, defaultDirectory, workingFileDirectory);
                if(loadFilePath != null)
                {
                    logger.Info("User chose load path: {0}, loading...", loadFilePath);
                    ViewEnabled = false;
                    StatusBarVM.CurrentStatusText = "Loading...";
                    buildTitleString(loadFilePath, appName);
                    databaseManager = new DatabaseManager<T>(loadFilePath);
                    databaseManager.LoadComplete += handleLoadComplete;
                    databaseManager.AsyncLoad();
                }
                else
                {
                    logger.Info("User cancelled load.");
                }
            });
        }
        private bool loadCanExecute()
        {
            return databaseReady();
        }
        protected void handleLoadComplete(T scopeManager)
        {
            logger.Info("Load complete.");
            handleLoaded(scopeManager);
            StatusBarVM.CurrentStatusText = "Ready";
            ViewEnabled = true;
        }
        protected abstract void handleLoaded(T scopeManager);
        //Save Delta
        private void saveDeltaExecute()
        {
            logger.Info("User clicked File->Save");

            if (databaseManager != null)
            {
                logger.Info("Saving deltas...");
                StatusBarVM.CurrentStatusText = "Saving...";
                databaseManager.SaveComplete += handleSaveDeltaComplete;
                databaseManager.AsyncSave(deltaStack.CleansedStack());
                deltaStack = new DeltaStacker(watcher, isTemplates);
            }
            else
            {
                logger.Info("Working Scope is new, executing save new.");
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
                logger.Info("Save Delta successful.");
                StatusBarVM.CurrentStatusText = "Ready";
            }
            else
            {
                logger.Info("Save Delta failed, attempting to save as new to the same path.");
                databaseManager.SaveComplete += handleSaveNewComplete;
                databaseManager.AsyncNew(getWorkingScope());
            }
        }        
        //Save New
        private void saveNewExecute()
        {
            logger.Info("User clicked File->Save As");
            string saveFilePath = UIHelpers.GetSavePath(workingFileParameters, defaultFileName, defaultDirectory, workingFileDirectory);
            if(saveFilePath != null)
            {
                logger.Info("Saving new file...");
                StatusBarVM.CurrentStatusText = "Saving...";
                databaseManager = new DatabaseManager<T>(saveFilePath);
                databaseManager.SaveComplete += handleSaveNewComplete;
                databaseManager.AsyncNew(getWorkingScope());
                buildTitleString(saveFilePath, appName);
            }
            else
            {
                logger.Info("User cancelled save new.");
            }
        }
        private bool canSaveNew()
        {
            return databaseReady();
        }
        protected void handleSaveNewComplete(bool success)
        {
            databaseManager.SaveComplete -= handleSaveNewComplete;
            if (success)
            {
                logger.Info("Save New sucessful.");
                StatusBarVM.CurrentStatusText = "Ready";
                deltaStack = new DeltaStacker(watcher, isTemplates);
            }
            else
            {
                logger.Fatal("Save New failed.");
                MessageBox.Show("File failed to save. Contact technical support.");
            }
        }
        //Refresh
        protected void refreshExecute()
        {
            logger.Info("User clicked File->Refresh");
            string message = "Would you like to save your changes before refreshing?";
            ViewEnabled = false;
            checkForChanges(message, refresh);

            void refresh()
            {
                logger.Info("Refreshing Working Manager, Loading...");
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
        //Help
        private void wikiExecute()
        {
            System.Diagnostics.Process.Start(WIKI_LINK);
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
        private ImageSource getLogo()
        {
            //String path = Path.GetTempFileName();
            //(Properties.Resources.TECLogo).Save(path, ImageFormat.Png);
            //return path;

            return Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.TECLogo.GetHbitmap(),
                IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                
        }
        protected void buildTitleString(string filePath, string appName)
        {
            string title = Path.GetFileNameWithoutExtension(filePath);
            if(title == "")
            {
                title = "New Document";
            }
            TitleString = title + " - " + appName;
        }
        protected void checkForChanges(string taskMessage, Action onComplete, Action onCancel = null)
        {
            logger.Info("Checking for changes.");

            if (deltaStack.CleansedStack().Count > 0)
            {
                logger.Info("Changes exist. User prompt: {0}", taskMessage);
                MessageBoxResult result = MessageBox.Show(taskMessage, "Save", MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Exclamation);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        logger.Info("User responded 'yes', saving delta.");
                        saveDeltaExecute();
                        if (databaseManager != null)
                        {
                            databaseManager.SaveComplete += (dbSuccess) => saveComplete(dbSuccess);
                        }
                        else
                        {
                            saveComplete(false, "databaseManager is null. User (probably) cancelled save.");
                        }
                        break;
                    case MessageBoxResult.No:
                        logger.Info("User responded 'no'.");
                        executeOnComplete();
                        break;
                    default:
                        logger.Info("User cancelled");
                        onCancel?.Invoke();
                        return;
                }
            }
            else
            {
                logger.Info("No changes.");
                executeOnComplete();
            }

            void saveComplete(bool success, string nonSuccessMessage = "databaseManager.SaveComplete returned false.")
            {
                databaseManager.SaveComplete -= (dbSuccess) => saveComplete(dbSuccess);
                if (success)
                {
                    logger.Info("Save successful.");
                    executeOnComplete();
                }
                else
                {
                    logger.Info("Save unsuccessful, {0}", nonSuccessMessage);
                    return;
                }
            }

            void executeOnComplete()
            {
                logger.Info("Done checking for changes, executing onComplete.");
                onComplete();
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
        
        private void closingExecute(CancelEventArgs e, Window window)
        {
            
            logger.Info("App manager recieved command to close.");
            if (databaseManager == null || !databaseManager.IsBusy)
            {
                bool changesExist = (deltaStack?.CleansedStack().Count > 0);
                if (changesExist)
                {
                    logger.Info("Unsaved changes exist, prompting user action.");
                    MessageBoxResult result = MessageBox.Show("You have unsaved changes. Would you like to save before quitting?", "Save?", MessageBoxButton.YesNoCancel);
                    if (result == MessageBoxResult.Yes)
                    {
                        e.Cancel = true;
                        if (databaseManager == null)
                        {
                            logger.Info("Saving new file before closing.");
                            string saveFilePath = UIHelpers.GetSavePath(workingFileParameters, defaultFileName, defaultDirectory, workingFileDirectory);
                            databaseManager = new DatabaseManager<T>(saveFilePath);
                            databaseManager.SaveComplete += closeWindowOnSuccess;
                            databaseManager.AsyncNew(getWorkingScope());
                        }
                        else
                        {
                            logger.Info("Saving delta before closing.");
                            databaseManager.SaveComplete += closeWindowOnSuccess;
                            databaseManager.AsyncSave(deltaStack.CleansedStack());
                        }
                    }
                    else if (result == MessageBoxResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                }
                if (!e.Cancel)
                {
                    logger.Info("Closing finally...");
                    Properties.Settings.Default.Save();
                }
                else
                {
                    logger.Info("Close cancelled. Save may have executed.");
                }
            }
            else
            {
                logger.Info("Close cancelled, database manager is busy.");
                e.Cancel = true;
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
            }

            void closeWindowOnSuccess(bool success)
            {
                databaseManager.SaveComplete -= closeWindowOnSuccess;
                if (success)
                {
                    logger.Info("Save successful, attempting window.Close()");
                    deltaStack = new DeltaStacker(watcher, isTemplates);
                    window.Close();
                }
                else
                {
                    logger.Info("Save unsuccessful, notifying user. Not closing window.");
                    MessageBox.Show("Save failed. Check logs for more info. Attempt to 'Save As' and try again.");
                }
            }
        }

        protected void notifyFileLocked(string fileName)
        {
            string message = string.Format("Could not access file: {0}. File may be open elsewhere.", fileName);
            logger.Warn(message);
            MessageBox.Show(message, "File Locked", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
    
}
