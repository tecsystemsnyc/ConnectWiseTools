using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.BaseVMs
{
    public abstract class MainViewModel
    {
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

        public MainViewModel(SplashVM splashVM, MenuVM menuVM, EditorVM editorVM)
        {
            SplashVM = splashVM;
            MenuVM = menuVM;
            EditorVM = editorVM;
            StatusBarVM = new StatusBarVM();
        }
    }
}
