using EstimatingLibrary.Utilities;
using EstimatingUtilitiesLibrary;
using EstimatingUtilitiesLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.BaseVMs
{
    abstract public class AppManager
    {
        protected DatabaseManager databaseManager;
        protected DoStacker doStack;
        protected DeltaStacker deltaStack;
        protected ChangeWatcher watcher;
        private string _titleString;

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
        #endregion
        
        public AppManager(SplashVM splashVM, MenuVM menuVM, EditorVM editorVM)
        {
            SplashVM = splashVM;
            MenuVM = menuVM;
            EditorVM = editorVM;
            StatusBarVM = new StatusBarVM();
        }
    }
}
