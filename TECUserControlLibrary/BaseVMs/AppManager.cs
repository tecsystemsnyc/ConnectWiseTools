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
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.BaseVMs
{
    abstract public class AppManager : ViewModelBase
    {
        protected DatabaseManager databaseManager;
        protected DoStacker doStack;
        protected DeltaStacker deltaStack;
        protected ChangeWatcher watcher;
        private string _titleString;
        private ViewModelBase _currentVM;

        #region Properties
        /// <summary>
        /// Generic for presentation purposes.
        /// </summary>
        public EditorVM EditorVM { get; }
        /// <summary>
        /// Generic for presentation purposes.
        /// </summary>
        public SplashVM SplashVM { get; }
        /// <summary>
        /// Generic for presentation purposes.
        /// </summary>
        public MenuVM MenuVM { get; }
        public StatusBarVM StatusBarVM { get; }

        public ViewModelBase CurrentVM
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
        #endregion

        public AppManager(SplashVM splashVM, MenuVM menuVM, EditorVM editorVM)
        {
            SplashVM = splashVM;
            MenuVM = menuVM;
            EditorVM = editorVM;
            StatusBarVM = new StatusBarVM();
            
        }

        private void setupCommands()
        {
            //MenuVM.SetUndoCommand
        }
    }
}
