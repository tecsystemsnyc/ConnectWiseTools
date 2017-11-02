using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.BaseVMs
{
    public enum BuilderType { EB, TB };

    public class MainViewModel
    {
        private BuilderType builderType;

        #region Properties
        public EditorVM EditorVM { get; private set; }
        public SplashVM SplashVM { get; private set; }
        public MenuVM MenuVM { get; private set; }
        public StatusBarVM StatusBarVM { get; private set; }
        #endregion

        public MainViewModel(string titleText, string subtitleText, string initialTemplates, string defaultDirectory, BuilderType type)
        {
            builderType = type;
            SplashVM = new SplashVM(titleText, subtitleText, initialTemplates, defaultDirectory, builderType);
            SplashVM.Started += EditorStarted;
        }

        public event Action<string, string> EditorStarted;

        public void StartEditor()
        {

        }
    }
}
