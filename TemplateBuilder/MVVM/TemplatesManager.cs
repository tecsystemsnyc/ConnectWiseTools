using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using EstimatingUtilitiesLibrary;
using EstimatingUtilitiesLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.BaseVMs;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.Utilities;
using TECUserControlLibrary.ViewModels;

namespace TemplateBuilder.MVVM
{
    public class TemplatesManager : AppManager<TECTemplates>
    {
        private TECTemplates templates;

        private TemplatesMenuVM menuVM
        {
            get { return MenuVM as TemplatesMenuVM; }
        }
        private TemplatesEditorVM editorVM
        {
            get { return EditorVM as TemplatesEditorVM; }
        }
        private TemplatesSplashVM splashVM
        {
            get { return SplashVM as TemplatesSplashVM; }
        }

        override protected FileDialogParameters workingFileParameters
        {
            get
            {
                return FileDialogParameters.TemplatesFileParameters;
            }
        }
        override protected string defaultDirectory
        {
            get
            {
                return Properties.Settings.Default.DefaultDirectory;
            }
            set
            {
                Properties.Settings.Default.DefaultDirectory = value;
                Properties.Settings.Default.Save();
            }
        }
        override protected string defaultFileName
        {
            get
            {
                throw new NotImplementedException("Need to construct file name for templates file.");
            }
        }

        public TemplatesManager() : base(new TemplatesSplashVM(Properties.Settings.Default.TemplatesFilePath, Properties.Settings.Default.DefaultDirectory), new TemplatesMenuVM())
        {
            splashVM.EditorStarted += userStartedEditorHandler;
            TitleString = "Template Builder";
            setupCommands();
        }

        private void userStartedEditorHandler(string path)
        {
            buildTitleString(path, "TemplateBuilder");
            databaseManager = new DatabaseManager<TECTemplates>(path);
            databaseManager.LoadComplete += handleLoadedTemplates;
            ViewEnabled = false;
            databaseManager.AsyncLoad();
        }

        private void handleLoadedTemplates(TECTemplates loaded)
        {
            templates = loaded;
            watcher = new ChangeWatcher(templates);
            doStack = new DoStacker(watcher);
            deltaStack = new DeltaStacker(watcher);

            EditorVM = new TemplatesEditorVM(templates);
            CurrentVM = EditorVM;
            ViewEnabled = true;
        }

        #region Menu Commands Methods
        private void setupCommands()
        {
            menuVM.SetNewCommand(newExecute, newCanExecute);
            menuVM.SetRefreshTemplatesCommand(refreshTemplatesExecute, canRefreshTemplates);
        }
        //New
        private void newExecute()
        {
            string message = "Would you like to save your changed before creating new templates?";
            checkForChanges(message, () => {
                handleLoadedTemplates(new TECTemplates());
            });
        }
        private bool newCanExecute()
        {
            return true;
        }
        //Load
        protected override void handleLoadComplete(TECTemplates templates)
        {
            handleLoadedTemplates(templates);
            StatusBarVM.CurrentStatusText = "Ready";
            ViewEnabled = true;
        }
        //Save Delta
        protected override void handleSaveDeltaComplete(bool success)
        {
            databaseManager.SaveComplete -= handleSaveDeltaComplete;
            if (success)
            {
                StatusBarVM.CurrentStatusText = "Ready";
            }
            else
            {
                databaseManager.SaveComplete += handleSaveNewComplete;
                databaseManager.AsyncNew(templates);
            }
        }
        //Refresh Templates
        private void refreshTemplatesExecute()
        {
            throw new NotImplementedException();
        }
        private bool canRefreshTemplates()
        {
            return true;
        }
        #endregion

        protected override TECScopeManager getWorkingScope()
        {
            return templates;
        }
    }
}
