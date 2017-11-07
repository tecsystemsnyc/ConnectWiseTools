using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using EstimatingUtilitiesLibrary;
using EstimatingUtilitiesLibrary.Database;
using System;
using TECUserControlLibrary.BaseVMs;
using TECUserControlLibrary.Models;

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
            databaseManager.LoadComplete += handleLoaded;
            ViewEnabled = false;
            databaseManager.AsyncLoad();
        }

        protected override void handleLoaded(TECTemplates loaded)
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
            menuVM.SetRefreshTemplatesCommand(refreshExecute, canRefresh);
        }
        #endregion

        protected override TECTemplates getWorkingScope()
        {
            return templates;
        }
        protected override TECTemplates getNewWorkingScope()
        {
            return new TECTemplates();
        }
    }
}
