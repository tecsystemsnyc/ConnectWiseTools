using DebugLibrary;
using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using EstimatingUtilitiesLibrary;
using EstimatingUtilitiesLibrary.Database;
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
        #region Fields 
        public SplashVM SplashVM;
        
        protected bool isNew;
        protected bool _isReady;
        protected int loadedStackLength;
        protected bool _userCanInteract;
        protected DatabaseManager workingDB;
        protected FileDialogParameters workingFileParameters;
        protected ChangeWatcher watcher;

        private string _titleString;
        private TECScopeManager _workingScopeManager;
        private ViewModelBase _currentVM;
        private DoStacker doStack;
        private DeltaStacker deltaStack;
        #endregion
        #region Constructors
        public BuilderViewModel(string splashTitle, string splashSubtitle, bool isEstimate)
        {
            TitleString = "";
            setupStatusBar();
            getStartupFile();
            isNew = workingScopeManager == null;
            if (isNew)
            {
                SplashVM = new SplashVM(splashTitle, splashSubtitle, TemplatesFilePath, defaultDirectory, isEstimate);
                SplashVM.Started += startUp;
                CurrentVM = SplashVM;
            } else
            {
                startFromFile();
            }
            getLogo();
        }
        #endregion
        #region Properties
        public ViewModelBase CurrentVM
        {
            get { return _currentVM; }
            set
            {
                _currentVM = value;
                RaisePropertyChanged("CurrentVM");
            }
        }

        public abstract Visibility TemplatesVisibility { get; set; }
        public bool IsReady
        {
            get { return _isReady; }
            set
            {
                _isReady = value;
                RaisePropertyChanged("IsReady");
            }
        }
        public string TECLogo { get; set; }
        public string TitleString
        {
            get { return _titleString; }
            set
            {
                _titleString = value;
                RaisePropertyChanged("TitleString");
            }
        }
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
        public string Version { get; set; }
        public ICommand LoadCommand { get; private set; }
        public ICommand NewCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand SaveAsCommand { get; private set; }
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }
        public RelayCommand<CancelEventArgs> ClosingCommand { get; private set; }
        public MenuVM MenuVM { get; set; }
        public StatusBarVM StatusBarVM { get; set; }
        public SettingsVM SettingsVM { get; set; }
        
        protected abstract string defaultSaveFileName { get; }
        protected abstract string ScopeDirectoryPath { get; set; }
        protected abstract bool TemplatesHidden { get; set; }
        protected abstract string TemplatesFilePath { get; set; }
        protected abstract string startupFilePath { get; set; }
        protected abstract string defaultDirectory { get; set; }
        protected virtual string saveFilePath{ get; set; }
        protected virtual TECScopeManager workingScopeManager
        {
            get{ return _workingScopeManager; }
            set
            {
                _workingScopeManager = value;
                watcher = new ChangeWatcher(value);
                deltaStack = new DeltaStacker(watcher);
                doStack = new DoStacker(watcher);
            }
        }
        
        #endregion
        #region Methods
        public void DragOver(IDropInfo dropInfo)
        {
            UIHelpers.FileDragOver(dropInfo);
        }
        public void Drop(IDropInfo dropInfo)
        {
            string path = UIHelpers.FileDrop(dropInfo);
            throw new NotImplementedException();

        }

        protected abstract TECScopeManager NewScopeManager();
        protected virtual void setupMenu(MenuType menuType)
        {
            MenuVM = new MenuVM(menuType);

            MenuVM.NewCommand = NewCommand;
            MenuVM.LoadCommand = LoadCommand;
            MenuVM.SaveCommand = SaveCommand;
            MenuVM.SaveAsCommand = SaveAsCommand;
            MenuVM.UndoCommand = UndoCommand;
            MenuVM.RedoCommand = RedoCommand;
            MenuVM.LoadTemplatesCommand = LoadCommand;
        }
        protected virtual void setupExtensions(MenuType menuType)
        {
            setupMenu(menuType);
            setupSettings();
        }
        protected virtual void setupCommands()
        {
            NewCommand = new RelayCommand(NewExecute);
            LoadCommand = new RelayCommand(loadExecute);
            SaveCommand = new RelayCommand(saveExecute);
            SaveAsCommand = new RelayCommand(saveAsExecute);
            UndoCommand = new RelayCommand(undoExecute, undoCanExecute);
            RedoCommand = new RelayCommand(redoExecute, redoCanExecute);
            ClosingCommand = new RelayCommand<CancelEventArgs>(e => closingExecute(e));
        }
        protected abstract void startUp(string main, string templates);
        protected abstract void startFromFile();

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
        protected bool saveNew(bool async)
        {
            //User choose path
            string path = UIHelpers.GetSavePath(workingFileParameters, defaultSaveFileName, defaultDirectory, ScopeDirectoryPath, isNew);
            if (path != null)
            {
                saveFilePath = path;
                ScopeDirectoryPath = Path.GetDirectoryName(path);
                workingDB = new DatabaseManager(path);
                SetBusyStatus("Saving file: " + path, true);

                if (async)
                {
                    workingDB.AsyncNew(workingScopeManager);
                    return false;
                }
                else
                {
                    bool success = workingDB.New(workingScopeManager);
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
                    workingDB.AsyncSave(deltaStack.CleansedStack());
                    return false;
                }
                else
                {
                    bool success = workingDB.Save(deltaStack.CleansedStack());
                    ResetStatus();
                    return success;
                }
            }
            else
            {
                return saveNew(async);
            }
        }
        protected bool load(bool async, string path)
        {
            saveFilePath = path;
            ScopeDirectoryPath = Path.GetDirectoryName(path);
            SetBusyStatus("Loading File: " + path, false);
            workingDB = new DatabaseManager(path);

            if (async)
            {
                workingDB.LoadComplete += (scope) =>
                {
                    if (scope != null)
                    {
                        workingScopeManager = scope;
                    }
                    if (isNew)
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
                workingDB.AsyncLoad();
                return false;
            }
            else
            {
                workingScopeManager = workingDB.Load();
                isNew = false;
                ResetStatus();
                return (workingScopeManager != null);
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

        private void getLogo()
        {
            TECLogo = Path.GetTempFileName();

            (Properties.Resources.TECLogo).Save(TECLogo, ImageFormat.Png);
        }
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
        private void undoExecute()
        {
            doStack.Undo();
        }
        private bool undoCanExecute()
        {
            if (doStack.UndoCount() > 0)
                return true;
            else
                return false;
        }
        private void redoExecute()
        {
            doStack.Redo();
        }
        private bool redoCanExecute()
        {
            if (doStack.RedoCount() > 0)
                return true;
            else
                return false;
        }
        private void loadExecute()
        {
            if (!IsReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }
            string path = UIHelpers.GetLoadPath(workingFileParameters, defaultDirectory, ScopeDirectoryPath);
            if (deltaStack.CleansedStack().Count > 0 && deltaStack.CleansedStack().Count != loadedStackLength)
            {
                string message = "Would you like to save your changes before loading?";
                MessageBoxResult result = MessageBox.Show(message, "Save", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    if (saveDelta(false))
                    {
                        load(true, path);
                    }
                    else
                    {
                        MessageBox.Show("Save unsuccessful. File not loaded.");
                    }
                }
                else if (result == MessageBoxResult.No)
                {
                    load(true, path);
                }
                else
                {
                    return;
                }
            }
            else
            {
                load(true, path);
            }
        }
        private void saveExecute()
        {
            if (!IsReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }
            saveDelta(true);
        }
        private void saveAsExecute()
        {
            if (!IsReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }
            saveNew(true);
        }
        private void NewExecute()
        {
            if (!IsReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }
            if (deltaStack.CleansedStack().Count > 0 && deltaStack.CleansedStack().Count != loadedStackLength)
            {
                string message = "Would you like to save your changes before creating a new scope?";
                MessageBoxResult result = MessageBox.Show(message, "Create new", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    SetBusyStatus("Saving...", false);
                    if (saveDelta(false))
                    {
                        DebugHandler.LogDebugMessage("Creating new.");
                        isNew = true;
                        workingScopeManager = NewScopeManager();
                        saveFilePath = null;
                    }
                    else
                    {
                        DebugHandler.LogError("Save unsuccessful. New scope not created.");
                    }
                    ResetStatus();
                }
                else if (result == MessageBoxResult.No)
                {
                    DebugHandler.LogDebugMessage("Creating new.");
                    isNew = true;
                    workingScopeManager = NewScopeManager();
                    saveFilePath = null;
                }
                else
                {
                    return;
                }
            }
            else
            {
                DebugHandler.LogDebugMessage("Creating new.");
                isNew = true;
                workingScopeManager = NewScopeManager();
                saveFilePath = null;
            }
        }
        private void closingExecute(CancelEventArgs e)
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
        private void getStartupFile()
        {
            if (startupFilePath != "")
            {
                SetBusyStatus("Loading " + startupFilePath, false);
                try
                {
                    if (File.Exists(startupFilePath))
                    {
                        load(false, startupFilePath);
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
        #endregion

    }
}