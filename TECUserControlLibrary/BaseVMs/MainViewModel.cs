using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.BaseVMs
{
    public enum BuilderType { EB, TB };

    public abstract class MainViewModel
    {
        private BuilderType builderType;

        #region Properties
        public EditorVM EditorVM { get; private set; }
        public SplashVM SplashVM { get; private set; }
        public MenuVM MenuVM { get; private set; }
        public StatusBarVM StatusBarVM { get; private set; }
        #endregion

        public MainViewModel(SplashVM splashVM, MenuVM menuVM, EditorVM editorVM)
        {
            SplashVM = splashVM;
            MenuVM = menuVM;
            EditorVM = editorVM;
            StatusBarVM = new StatusBarVM();
        }

        public event Action<string, string> EditorStarted;

        public void StartEditor()
        {

        }
    }
}
