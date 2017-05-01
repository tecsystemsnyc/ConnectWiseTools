using DebugLibrary;
using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
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
    abstract public class BuilderViewModel : ViewModelBase, IDropTarget
    {

        #region Constants
        protected string DEFAULT_STATUS_TEXT = "Ready";
        #endregion

        #region Properties
        private TECScopeManager _workingScopeManager;
        protected virtual TECScopeManager workingScopeManager
        {
            get
            {
                return _workingScopeManager;
            }
            set
            {
                _workingScopeManager = value;
                stack = new ChangeStack(value);
            }
        }
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

        public string Version { get; set; }

        public string TitleString
        {
            get { return _titleString; }
            set
            {
                _titleString = value;
                RaisePropertyChanged("TitleString");
            }
        }
        private string _titleString;

        abstract protected string ScopeDirectoryPath
        {
            get;
            set;
        }

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
        
        #endregion

        #region View Models
        public MenuViewModel MenuVM { get; set; }
        public StatusBarExtension StatusBarVM { get; set; }
        public SettingsViewModel SettingsVM { get; set; }
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

        #region SettingsProperties
        abstract protected bool TemplatesHidden
        {
            get;
            set;
        }
        abstract protected string TemplatesFilePath
        {
            get;
            set;
        }
        #endregion

        #endregion

        public BuilderViewModel()
        {
            isNew = true;

            setupVMs();

            setupCommands();
            getLogo();
        }

        #region Methods
        private void checkForOpenWith()
        {
            startupFile = getStartupFile();
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
            clearStartupFile();
        }
        private void getLogo()
        {
            TECLogo = Path.GetTempFileName();

            (Properties.Resources.TECLogo).Save(TECLogo, ImageFormat.Png);
        }
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
        protected abstract string getStartupFile();
        protected abstract void clearStartupFile();

        #region Setup
        private void setupVMs()
        {
            setupMenu();
            setupStatusBar();
            setupSettings();
        }

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
        private void setupStatusBar()
        {
            StatusBarVM = new StatusBarExtension();

            if (ApplicationDeployment.IsNetworkDeployed)
            { StatusBarVM.Version = "Version " + ApplicationDeployment.CurrentDeployment.CurrentVersion; }
            else
            { StatusBarVM.Version = "Undeployed Version"; }

            StatusBarVM.CurrentStatusText = DEFAULT_STATUS_TEXT;
        }
        private void setupSettings()
        {
            SettingsVM = new SettingsViewModel(TemplatesHidden, TemplatesFilePath, MenuVM.LoadTemplatesCommand);
            SettingsVM.PropertyChanged += SettingsVM_PropertyChanged;
        }

        
        #endregion
        #region Save/Load
        protected void saveNewToPath(string path)
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
        }
        protected void saveToPath(string path, ChangeStack saveStack)
        {
            if (!UtilitiesMethods.IsFileLocked(path))
            {
                try
                { EstimatingLibraryDatabase.Update(path, saveStack); }
                catch (Exception ex)
                {
                    DebugHandler.LogError("Save delta failed. Saving to new file. Exception: " + ex.Message);
                    EstimatingLibraryDatabase.SaveNew(path, workingScopeManager);
                }
            }
            else
            {
                DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
            }
        }
        private void save()
        {
            if (saveFilePath != null)
            {
                SetBusyStatus("Saving to path: " + saveFilePath);
                ChangeStack stackToSave = stack.Copy();
                stack.ClearStacks();

                BackgroundWorker worker = new BackgroundWorker();

                worker.DoWork += (s, e) =>
                {
                    saveToPath(saveFilePath, stackToSave);
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
                    saveNewToPath(path);
                };
                worker.RunWorkerCompleted += (s, e) =>
                {
                    ResetStatus();
                    isNew = false;
                };

                worker.RunWorkerAsync();
            }

        }
        protected bool saveAsSynchronously()
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
        protected bool saveSynchronously()
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
        
        protected TECScopeManager loadFromPath(string path)
        {
            saveFilePath = path;
            ScopeDirectoryPath = Path.GetDirectoryName(path);
            TECScopeManager outScope = null;
            if (!UtilitiesMethods.IsFileLocked(path))
            { outScope = EstimatingLibraryDatabase.Load(path); }
            else
            {
                DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
            }
            return outScope;
        }
        private void load()
        {
            string path = getLoadPath(workingFileParameters, ScopeDirectoryPath);
            if (path != null)
            {
                SetBusyStatus("Loading File: " + path, false);
                TECScopeManager loadingScopeManager = null;
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (s, e) =>
                {
                    saveFilePath = path;
                    ScopeDirectoryPath = Path.GetDirectoryName(path);

                    loadingScopeManager = loadFromPath(path);
                };
                worker.RunWorkerCompleted += (s, e) =>
                {
                    ResetStatus();
                    if (loadingScopeManager != null)
                    {
                        workingScopeManager = loadingScopeManager;
                    }

                    isNew = false;
                };

                worker.RunWorkerAsync();
            }
        }
        #endregion 
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
        private void ClosingExecute(CancelEventArgs e)
        {
            bool changes = (stack.SaveStack.Count > 0);
            if (changes)
            {
                MessageBoxResult result = MessageBox.Show("You have unsaved changes. Would you like to save before quitting?", "Save?", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    if (!saveSynchronously())
                    {
                        MessageBox.Show("Save unsuccessful.");
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
        #endregion
        #region Drag-Drop
        public void DragOver(IDropInfo dropInfo)
        {
            UIHelpers.FileDragOver(dropInfo);
        }
        public void Drop(IDropInfo dropInfo)
        {
            string path = UIHelpers.FileDrop(dropInfo);
            if (path != null)
            {
                loadFromPath(path);
            }
        }
        #endregion

        #region Event Handlers

        private void SettingsVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TemplatesHidden")
            {
                TemplatesHidden = SettingsVM.TemplatesHidden;
            }
        }

        #endregion

        #endregion
    }
}