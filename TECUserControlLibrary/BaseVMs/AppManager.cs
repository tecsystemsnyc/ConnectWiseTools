using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using EstimatingUtilitiesLibrary;
using EstimatingUtilitiesLibrary.Database;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.Utilities;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.BaseVMs
{
    abstract public class AppManager : ViewModelBase
    {
        protected DatabaseManager databaseManager;
        protected ChangeWatcher watcher;
        protected DoStacker doStack;
        protected DeltaStacker deltaStack;
        protected string workingFileDirectory;
        private string _titleString;
        private object _currentVM;

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
        public string Version { get; set; }
        public RelayCommand<CancelEventArgs> ClosingCommand { get; private set; }

        abstract protected FileDialogParameters workingFileParameters { get; }
        abstract protected string defaultDirectory { get; set; }
        abstract protected string defaultFileName { get; }
        #endregion

        public AppManager(SplashVM splashVM, MenuVM menuVM)
        {
            SplashVM = splashVM;
            MenuVM = menuVM;
            StatusBarVM = new StatusBarVM();
            setupCommands();
            CurrentVM = SplashVM;
        }

        private void setupCommands()
        {
            MenuVM.SetUndoCommand(undoExecute, canUndo);
            MenuVM.SetRedoCommand(redoExecute, canRedo);
        }
        
        private void undoExecute()
        {
            doStack.Undo();
        }
        private bool canUndo()
        {
            return doStack.UndoCount() > 0;
        }
        private void redoExecute()
        {
            doStack.Redo();
        }
        private bool canRedo()
        {
            return doStack.RedoCount() > 0;
        }
    }
}
