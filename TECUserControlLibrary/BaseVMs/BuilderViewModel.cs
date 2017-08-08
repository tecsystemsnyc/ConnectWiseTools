using DebugLibrary;
using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using EstimatingUtilitiesLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Deployment.Application;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Input;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.Utilities;

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
            }
        }
        protected FileDialogParameters workingFileParameters;
        protected int loadedStackLength;

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

        abstract public Visibility TemplatesVisibility
        {
            get;
            set;
        }

        //Used to update relevant properties in children view models
        virtual protected string saveFilePath
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
        protected DoStacker doStack;
        protected DeltaStacker deltaStack;
        protected ChangeWatcher watcher;
        #endregion

        #region View Models
        public MenuVM MenuVM { get; set; }
        public StatusBarVM StatusBarVM { get; set; }
        public SettingsVM SettingsVM { get; set; }
        #endregion

        #region File Parameters
        protected FileDialogParameters BidFileParameters
        {
            get
            {
                FileDialogParameters fileParams;
                fileParams.Filter = "Bid Database Files (*.bdb)|*.bdb" + "|All Files (*.*)|*.*";
                fileParams.DefaultExtension = "bdb";
                return fileParams;
            }
        }
        protected FileDialogParameters EstimateFileParameters
        {
            get
            {
                FileDialogParameters fileParams;
                fileParams.Filter = "Estimate Database Files (*.edb)|*.edb" + "|All Files (*.*)|*.*";
                fileParams.DefaultExtension = "edb";
                return fileParams;
            }
        }
        protected FileDialogParameters TemplatesFileParameters
        {
            get
            {
                FileDialogParameters fileParams;
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
                fileParams.Filter = "Rich Text Files (*.rtf)|*.rtf";
                fileParams.DefaultExtension = "rtf";
                return fileParams;
            }
        }
        protected FileDialogParameters WordDocumentFileParameters
        {
            get
            {
                FileDialogParameters fileParams;
                fileParams.Filter = "Word Documents (*.docx)|*.docx";
                fileParams.DefaultExtension = "docx";
                return fileParams;
            }
        }
        protected FileDialogParameters CSVFileParameters
        {
            get
            {
                FileDialogParameters fileParams;
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
        abstract protected string startupFilePath
        {
            get;
            set;
        }
        abstract protected string defaultDirectory { get; set; }
        #endregion

        #endregion

        public BuilderViewModel()
        {
            setupCommands();
            setupExtensions();
            getStartupFile();

            if (workingScopeManager == null)
            {
                isNew = true;
            }
            getLogo();
        }

        #region Methods
        protected void getStartupFile()
        {
            if (startupFilePath != "")
            {
                SetBusyStatus("Loading " + startupFilePath, false);
                try
                {
                    if (File.Exists(startupFilePath))
                    {
                        workingScopeManager = loadFromPath(startupFilePath);
                    }
                    else
                    {
                        DebugHandler.LogError("Startup file doesn't exist. Path: " + startupFilePath);
                    }
                }
                catch (Exception e)
                {
                    DebugHandler.LogError(e);
                }
                ResetStatus();
            }
            startupFilePath = "";
        }
        private void getLogo()
        {
            TECLogo = Path.GetTempFileName();

            (Properties.Resources.TECLogo).Save(TECLogo, ImageFormat.Png);
        }
        protected void SetBusyStatus(string statusText, bool userCanInteract)
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
        abstract protected void buildTitleString();

        #region Setup
        virtual protected void setupExtensions()
        {
            setupMenu();
            setupStatusBar();
            setupSettings();
        }

        virtual protected void setupCommands()
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
            StatusBarVM = new StatusBarVM();

            if (ApplicationDeployment.IsNetworkDeployed)
            { StatusBarVM.Version = "Version " + ApplicationDeployment.CurrentDeployment.CurrentVersion; }
            else
            { StatusBarVM.Version = "Undeployed Version"; }

            StatusBarVM.CurrentStatusText = DEFAULT_STATUS_TEXT;
        }
        private void setupSettings()
        {
            SettingsVM = new SettingsVM(defaultDirectory, TemplatesHidden, TemplatesFilePath, MenuVM.LoadTemplatesCommand);
            SettingsVM.PropertyChanged += SettingsVM_PropertyChanged;
        }

        #endregion

        #region Save/Load
        protected bool saveNew(bool async)
        {
            //User choose path
            string path = getSavePath(workingFileParameters, defaultSaveFileName, ScopeDirectoryPath);
            if (path != null)
            {
                saveFilePath = path;
                ScopeDirectoryPath = Path.GetDirectoryName(path);

                SetBusyStatus("Saving file: " + path, true);

                if (async)
                {
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += (s, e) =>
                    {
                        saveNewToPath(path);
                    };
                    worker.RunWorkerCompleted += (s, e) =>
                    {
                        isNew = false;
                        ResetStatus();
                    };

                    worker.RunWorkerAsync();
                    return false;
                }
                else
                {
                    bool success = saveNewToPath(path);
                    ResetStatus();
                    return success;
                }
            }
            else
            {
                return false;
            }
        }
        protected bool saveDelta(bool async)
        {
            if (saveFilePath != null && File.Exists(saveFilePath))
            {
                SetBusyStatus("Saving to path: " + saveFilePath, true);

                if (async)
                {
                    BackgroundWorker worker = new BackgroundWorker();

                    worker.DoWork += (s, e) =>
                    {
                        saveDeltaToPath(saveFilePath, deltaStack.CleansedStack());
                    };

                    worker.RunWorkerCompleted += (s, e) =>
                    {
                        isNew = false;
                        ResetStatus();
                    };

                    worker.RunWorkerAsync();
                    return false;
                }
                else
                {
                    bool success = saveDeltaToPath(saveFilePath, deltaStack.CleansedStack());
                    ResetStatus();
                    return success;
                }
            }
            else
            {
                return saveNew(async);
            }
        }
        protected bool load(bool async, string path = null)
        {
            if(path == null)
            {
                path = getLoadPath(workingFileParameters, ScopeDirectoryPath);
            }
            if (path != null)
            {
                saveFilePath = path;
                ScopeDirectoryPath = Path.GetDirectoryName(path);
                SetBusyStatus("Loading File: " + path, false);

                if (async)
                {
                    TECScopeManager loadingScopeManager = null;
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += (s, e) =>
                    {
                        loadingScopeManager = loadFromPath(path);
                    };
                    worker.RunWorkerCompleted += (s, e) =>
                    {
                        if (loadingScopeManager != null)
                        {
                            workingScopeManager = loadingScopeManager;
                        }
                        if(isNew)
                        {
                            loadedStackLength = deltaStack.CleansedStack().Count;
                        }
                        else
                        {
                            loadedStackLength = 0;
                        }
                        isNew = false;
                        ResetStatus();
                    };

                    worker.RunWorkerAsync();
                    return false;
                }
                else
                {
                    workingScopeManager = loadFromPath(path);
                    isNew = false;
                    ResetStatus();
                    return (workingScopeManager != null);
                }
            }
            else
            {
                return false;
            }
        }

        private bool saveNewToPath(string path)
        {
            if (!UtilitiesMethods.IsFileLocked(path))
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                //Create new database
                DatabaseHelper.SaveNew(path, workingScopeManager);
                return true;
            }
            else
            {
                DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
                return false;
            }
        }
        private bool saveDeltaToPath(string path, List<UpdateItem> updates)
        {
            if (!UtilitiesMethods.IsFileLocked(path))
            {
                try
                {
                    DatabaseUpdater.Update(path, updates);
                    return true;
                }
                catch (Exception ex) when (DebugBooleans.CatchSaveDelta)
                {
                    DebugHandler.LogError("Save delta failed. Saving to new file. Exception: " + ex.Message);
                    return saveNewToPath(path);
                }
            }
            else
            {
                DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
                return false;
            }
        }
        protected TECScopeManager loadFromPath(string path)
        {
            saveFilePath = path;
            ScopeDirectoryPath = Path.GetDirectoryName(path);
            TECScopeManager outScope = null;
            if (!UtilitiesMethods.IsFileLocked(path))
            { outScope = DatabaseHelper.Load(path); }
            else
            {
                DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
            }
            return outScope;
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
                saveFileDialog.InitialDirectory = defaultDirectory;
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
            if (initialDirectory != null)
            {
                openFileDialog.InitialDirectory = initialDirectory;
            }
            else
            {
                openFileDialog.InitialDirectory = defaultDirectory;
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

            if (deltaStack.CleansedStack().Count > 0 && deltaStack.CleansedStack().Count != loadedStackLength)
            {
                string message = "Would you like to save your changes before loading?";
                MessageBoxResult result = MessageBox.Show(message, "Create new", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    if (saveDelta(false))
                    {
                        load(true);
                    }
                    else
                    {
                        MessageBox.Show("Save unsuccessful. File not loaded.");
                    }
                }
                else if (result == MessageBoxResult.No)
                {
                    load(true);
                }
                else
                {
                    return;
                }
            }
            else
            {
                load(true);
            }
        }
        protected void SaveExecute()
        {
            if (!IsReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }
            saveDelta(true);
        }
        protected void SaveAsExecute()
        {
            if (!IsReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }
            saveNew(true);
        }
        private void UndoExecute()
        {
            doStack.Undo();
        }
        private bool UndoCanExecute()
        {
            if (doStack.UndoCount() > 0)
                return true;
            else
                return false;
        }
        private void RedoExecute()
        {
            doStack.Redo();
        }
        private bool RedoCanExecute()
        {
            if (doStack.RedoCount() > 0)
                return true;
            else
                return false;
        }
        private void ClosingExecute(CancelEventArgs e)
        {
            if (IsReady)
            {
                bool changesExist = (deltaStack.CleansedStack().Count > 0 && deltaStack.CleansedStack().Count != loadedStackLength);
                if (changesExist)
                {
                    MessageBoxResult result = MessageBox.Show("You have unsaved changes. Would you like to save before quitting?", "Save?", MessageBoxButton.YesNoCancel);
                    if (result == MessageBoxResult.Yes)
                    {
                        if (!saveDelta(false))
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
            else if (e.PropertyName == "DefaultDirectory")
            {
                defaultDirectory = SettingsVM.DefaultDirectory;
            }
        }

        protected void TemplatesHiddenChanged()
        {
            SettingsVM.TemplatesHidden = TemplatesHidden;
            if (TemplatesHidden)
            {
                TemplatesVisibility = Visibility.Hidden;
                MenuVM.TemplatesHidden = true;
            }
            else
            {
                TemplatesVisibility = Visibility.Visible;
                MenuVM.TemplatesHidden = false;
            }
        }

        protected void TemplatesFilePathChanged()
        {
            SettingsVM.TemplatesLoadPath = TemplatesFilePath;
        }

        #endregion

        #endregion
    }
}