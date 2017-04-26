using DebugLibrary;
using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.ComponentModel;
using System.Deployment.Application;
using System.Drawing.Imaging;
using System.IO;
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
        protected abstract string getSavePath();
        protected abstract string getLoadPath();
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

        #endregion //Helper Functions

        #region Commands
        protected abstract void NewExecute();
        protected abstract void LoadExecute();
        protected abstract void SaveExecute();
        protected abstract void SaveAsExecute();

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