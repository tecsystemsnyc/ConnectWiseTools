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

        public void StartEditor(TECTemplates templates, TECBid bid)
        {
            if (builderType != BuilderType.EB)
            {
                throw new InvalidOperationException("EB BuilderType method called for non EB instnce.");
            }
        }

        public void StartEditor(TECTemplates templates)
        {
            if (builderType != BuilderType.TB)
            {
                throw new InvalidOperationException("TB BuilderType method called for non TB instance.");
            }
        }

        private void setupVMs(BuilderType type)
        {
            MenuVM = new MenuVM(type);
        }
    }
}
