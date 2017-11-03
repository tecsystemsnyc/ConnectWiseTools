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
