using DebugLibrary;
using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Deployment.Application;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Input;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.ViewModelExtensions;

namespace TECUserControlLibrary.ViewModels
{
    abstract public class BuilderViewModel : ViewModelBase
    {

        #region Constants

        protected string DEFAULT_STATUS_TEXT = "Ready";

        #endregion

        #region Properties
        protected TECScopeManager workingScopeManager;
        protected FileDialogParameters workingFileParameters;

        abstract protected string defaultSaveFileName
        {
            get;
        }

        protected bool _isReady;
        public bool IsReady
        {
            get { return _isReady; }
            set
            {
                _isReady = value;
                RaisePropertyChanged("IsReady");
            }
        }

        protected bool _userCanInteract;
        public bool UserCanInteract
        {
            get { return _userCanInteract; }
            set
            {
                _userCanInteract = value;
                RaisePropertyChanged("UserCanInteract");
                if (UserCanInteract)
                {
                    Mouse.OverrideCursor = null;
                }
                else
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                }
            }
        }

        protected string _programName;
        protected string programName
        {
            get { return _programName; }
            set
            {
                _programName = value;
            }
        }
        
        public string TECLogo { get; set; }

        protected bool isNew;
      
        #region Command Properties
        public ICommand NewCommand { get; private set; }
        public ICommand LoadCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand SaveAsCommand { get; private set; }

        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }
        
        public RelayCommand<CancelEventArgs> ClosingCommand { get; private set; }
        #endregion

        #region Fields
        protected ChangeStack stack;
        public string startupFile;
        protected string saveFilePath;
        abstract protected string ScopeDirectoryPath
        {
            get;
            set;
        }
        #endregion

        #region View Models
        public MenuViewModel MenuVM { get; set; }
        public StatusBarExtension StatusBarVM { get; set; }
        #endregion

        #region File Parameters
        protected FileDialogParameters BidFileParameters
        {
            get
            {
                FileDialogParameters fileParams;
                fileParams.DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                fileParams.Filter = "Bid Database Files (*.bdb)|*.bdb" + "|All Files (*.*)|*.*";
                fileParams.DefaultExtension = "bdb";
                return fileParams;
            }
        }
        protected FileDialogParameters TemplatesFileParameters
        {
            get
            {
                FileDialogParameters fileParams;
                fileParams.DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                fileParams.Filter = "Templates Database Files (*.tdb)|*.tdb" + "|All Files (*.*)|*.*";
                fileParams.DefaultExtension = "tdb";
                return fileParams;
            }
        }
        protected FileDialogParameters DocumentFileParameters
        {
            get
            {
                FileDialogParameters fileParams;
                fileParams.DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                fileParams.Filter = "Rich Text Files (*.rtf)|*.rtf";
                fileParams.DefaultExtension = "rtf";
                return fileParams;
            }
        }
        protected FileDialogParameters CSVFileParameters
        {
            get
            {
                FileDialogParameters fileParams;
                fileParams.DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                fileParams.Filter = "Comma Separated Values Files (*.csv)|*.csv";
                fileParams.DefaultExtension = "csv";
                return fileParams;
            }
        }
        #endregion

        #endregion

        public BuilderViewModel()
        {
            isNew = true;

            setupStatusBar();
            SetBusyStatus("Initializing Program...");

            setupCommands();
            getLogo();
            setupMenu();
        }

        #region Methods

        #region Setup
        private void setupCommands()
        {
            NewCommand = new RelayCommand(NewExecute);
            LoadCommand = new RelayCommand(LoadExecute);
            SaveCommand = new RelayCommand(SaveExecute);
            SaveAsCommand = new RelayCommand(SaveAsExecute);
            UndoCommand = new RelayCommand(UndoExecute, UndoCanExecute);
            RedoCommand = new RelayCommand(RedoExecute, RedoCanExecute);
            ClosingCommand = new RelayCommand<CancelEventArgs>(e => ClosingExecute(e));
        }

        protected abstract void setupMenu();

        protected void setupStatusBar()
        {
            StatusBarVM = new StatusBarExtension();

            if (ApplicationDeployment.IsNetworkDeployed)
            { StatusBarVM.Version = "Version " + ApplicationDeployment.CurrentDeployment.CurrentVersion; }
            else
            { StatusBarVM.Version = "Undeployed Version"; }

            StatusBarVM.CurrentStatusText = DEFAULT_STATUS_TEXT;
        }
        #endregion

        #region Helper Functions
        private void checkForOpenWith(string startupFile)
        {
            if (startupFile != "")
            {
                SetBusyStatus("Loading " + startupFile);
                try
                {
                    loadFromPath(startupFile);
                }
                catch (Exception e)
                {
                    DebugHandler.LogError(e);
                }
                ResetStatus();
            }
        }

        private void getLogo()
        {
            TECLogo = Path.GetTempFileName();

            (Properties.Resources.TECLogo).Save(TECLogo, ImageFormat.Png);
        }
        protected abstract void loadFromPath(string path);

        protected void SetBusyStatus(string statusText, bool userCanInteract = true)
        {
            StatusBarVM.CurrentStatusText = statusText;
            IsReady = false;
            UserCanInteract = userCanInteract;
        }
        protected void ResetStatus()
        {
            StatusBarVM.CurrentStatusText = DEFAULT_STATUS_TEXT;
            IsReady = true;
            UserCanInteract = true;
        }

        protected void save()
        {
            if (saveFilePath != null)
            {
                SetBusyStatus("Saving to path: " + saveFilePath);
                ChangeStack stackToSave = stack.Copy();
                stack.ClearStacks();

                BackgroundWorker worker = new BackgroundWorker();

                worker.DoWork += (s, e) =>
                {
                    if (!UtilitiesMethods.IsFileLocked(saveFilePath))
                    {
                        try
                        { EstimatingLibraryDatabase.Update(saveFilePath, stackToSave); }
                        catch (Exception ex)
                        {
                            DebugHandler.LogError("Save delta failed. Saving to new file. Exception: " + ex.Message);
                            EstimatingLibraryDatabase.SaveNew(saveFilePath, workingScopeManager);
                        }
                    }
                    else
                    {
                        DebugHandler.LogError("Could not open file " + saveFilePath + " File is open elsewhere.");
                    }
                };
                worker.RunWorkerCompleted += (s, e) =>
                {
                    ResetStatus();
                };
                worker.RunWorkerAsync();
            }
            else
            {
                saveAs();
            }
        }
        private void saveAs()
        {
            //User choose path
            string path = getSavePath(workingFileParameters, defaultSaveFileName, ScopeDirectoryPath);
            if (path != null)
            {
                saveFilePath = path;
                ScopeDirectoryPath = Path.GetDirectoryName(path);

                stack.ClearStacks();
                SetBusyStatus("Saving file: " + path);

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (s, e) =>
                {
                    if (!UtilitiesMethods.IsFileLocked(path))
                    {
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                        //Create new database
                        EstimatingLibraryDatabase.SaveNew(path, workingScopeManager);
                    }
                    else
                    {
                        DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
                    }
                };
                worker.RunWorkerCompleted += (s, e) =>
                {
                    ResetStatus();
                    isNew = false;
                };

                worker.RunWorkerAsync();

            }

        }
        private bool saveAsSynchronously()
        {
            bool saveSuccessful = false;

            //User choose path
            string path = getSavePath(workingFileParameters, defaultSaveFileName, ScopeDirectoryPath);
            if (path != null)
            {
                saveFilePath = path;
                ScopeDirectoryPath = Path.GetDirectoryName(path);

                stack.ClearStacks();
                SetBusyStatus("Saving file: " + path);

                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    EstimatingLibraryDatabase.SaveNew(path, workingScopeManager);
                    saveSuccessful = true;
                }
                else
                {
                    DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
                    saveSuccessful = false;
                }


            }

            return saveSuccessful;
        }
        private bool saveSynchronously()
        {
            bool saveSuccessful = false;

            if (saveFilePath != null)
            {
                SetBusyStatus("Saving to path: " + saveFilePath);
                ChangeStack stackToSave = stack.Copy();
                stack.ClearStacks();

                if (!UtilitiesMethods.IsFileLocked(saveFilePath))
                {
                    try
                    {
                        EstimatingLibraryDatabase.Update(saveFilePath, stackToSave);
                        saveSuccessful = true;
                    }
                    catch (Exception ex)
                    {
                        DebugHandler.LogError("Save delta failed. Saving to new file. Error: " + ex.Message);
                        EstimatingLibraryDatabase.SaveNew(saveFilePath, workingScopeManager);
                    }
                }
                else
                {
                    DebugHandler.LogError("Could not open file " + saveFilePath + " File is open elsewhere.");
                }

            }
            else
            {
                if (saveAsSynchronously())
                {
                    saveSuccessful = true;
                }
                else
                {
                    saveSuccessful = false;
                }
            }

            return saveSuccessful;
        }
        protected void load()
        {
            string path = getLoadPath();
            if (path != null)
            {
                SetBusyStatus("Loading File: " + path, false);
                TECScopeManager loadingScopeManager = null;
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (s, e) =>
                {
                    saveFilePath = path;
                    ScopeDirectoryPath = Path.GetDirectoryName(path);

                    if (!UtilitiesMethods.IsFileLocked(path))
                    {
                        loadingScopeManager = EstimatingLibraryDatabase.Load(path);
                    }
                    else
                    {
                        DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
                    }
                };
                worker.RunWorkerCompleted += (s, e) =>
                {
                    ResetStatus();
                    if(loadingScopeManager == null)
                    {
                        workingScopeManager = loadingScopeManager;
                    }
                    
                    isNew = false;
                };

                worker.RunWorkerAsync();
            }
        }
        #endregion //Helper Functions

        #region Get Path Methods

        protected string getSavePath(FileDialogParameters fileParams, string defaultFileName, string initialDirectory = null)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (initialDirectory != null && !isNew)
            {
                saveFileDialog.InitialDirectory = initialDirectory;
            }
            else
            {
                saveFileDialog.InitialDirectory = fileParams.DefaultDirectory;
            }
            saveFileDialog.FileName = defaultFileName;
            saveFileDialog.Filter = fileParams.Filter;
            saveFileDialog.DefaultExt = fileParams.DefaultExtension;
            saveFileDialog.AddExtension = true;

            string savePath = null;
            if (saveFileDialog.ShowDialog() == true)
            {
                savePath = saveFileDialog.FileName;
            }
            return savePath;
        }

        protected string getLoadPath(FileDialogParameters fileParams, string initialDirectory = null)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (initialDirectory != null && !isNew)
            {
                openFileDialog.InitialDirectory = initialDirectory;
            }
            else
            {
                openFileDialog.InitialDirectory = fileParams.DefaultDirectory;
            }
            openFileDialog.Filter = fileParams.Filter;
            openFileDialog.DefaultExt = fileParams.DefaultExtension;
            openFileDialog.AddExtension = true;

            string savePath = null;
            if (openFileDialog.ShowDialog() == true)
            {
                savePath = openFileDialog.FileName;
            }
            return savePath;
        }

        #endregion

        #region Commands
        protected abstract void NewExecute();
        protected void LoadExecute()
        {
            if (!IsReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }

            if (stack.SaveStack.Count > 0)
            {
                string message = "Would you like to save your changes before loading?";
                MessageBoxResult result = MessageBox.Show(message, "Create new", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    if (saveSynchronously())
                    {
                        load();
                    }
                    else
                    {
                        MessageBox.Show("Save unsuccessful. File not loaded.");
                    }
                }
                else if (result == MessageBoxResult.No)
                {
                    load();
                }
            }
            else
            {
                load();
            }

        }
        protected void SaveExecute()
        {
            if (!IsReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }
            save();
        }
        protected void SaveAsExecute()
        {
            if (!IsReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }
            saveAs();
        }

        private void UndoExecute()
        {
            stack.Undo();
        }
        private bool UndoCanExecute()
        {
            if (stack.UndoStack.Count > 0)
                return true;
            else
                return false;
        }

        private void RedoExecute()
        {
            stack.Redo();
        }
        private bool RedoCanExecute()
        {
            if (stack.RedoStack.Count > 0)
                return true;
            else
                return false;
        }

        protected abstract void ClosingExecute(CancelEventArgs e);
        #endregion

        #endregion
    }
}