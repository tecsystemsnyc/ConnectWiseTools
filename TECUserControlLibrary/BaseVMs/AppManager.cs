using EstimatingLibrary.Utilities;
using EstimatingUtilitiesLibrary;
using EstimatingUtilitiesLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.BaseVMs
{
    abstract public class AppManager
    {
        protected DatabaseManager databaseManager;
        protected DoStacker doStack;
        protected DeltaStacker deltaStack;
        protected ChangeWatcher watcher;
        private string _titleString;

        public MainViewModel MainVM { get; protected set; }
        
        public AppManager(string splashTitle, string splashSubtitle, BuilderType type)
        {
            MainVM = MainViewModel(splashTitle, splashSubtitle);

        }
    }
}
