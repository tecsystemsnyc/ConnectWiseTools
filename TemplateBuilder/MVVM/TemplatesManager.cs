using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using EstimatingUtilitiesLibrary;
using EstimatingUtilitiesLibrary.Database;
using EstimatingUtilitiesLibrary.Exports;
using NLog;
using System;
using System.Deployment.Application;
using TECUserControlLibrary.BaseVMs;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.Utilities;

namespace TemplateBuilder.MVVM
{
    public class TemplatesManager : AppManager<TECTemplates>
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

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
                return string.Format("Templates v{0}", Version);
            }
        }
        protected override string templatesFilePath
        {
            get
            {
                return Properties.Settings.Default.TemplatesFilePath;
            }
            set
            {
                Properties.Settings.Default.TemplatesFilePath = value;
                Properties.Settings.Default.Save();
            }
        }

        public TemplatesManager() : base("Template Builder",
            new TemplatesSplashVM(Properties.Settings.Default.TemplatesFilePath, Properties.Settings.Default.DefaultDirectory), new TemplatesMenuVM())
        {
            string startUpFilePath = getStartUpFilePath();
            if (startUpFilePath != null && startUpFilePath != "")
            {
                splashVM.TemplatesPath = startUpFilePath;
            }
            splashVM.EditorStarted += userStartedEditorHandler;
            TitleString = "Template Builder";
            setupCommands();
        }

        private void userStartedEditorHandler(string path)
        {
            buildTitleString(path, "TemplateBuilder");
            if(path != "")
            {
                templatesFilePath = path;
                databaseManager = new DatabaseManager<TECTemplates>(path);
                databaseManager.LoadComplete += handleLoaded;
                ViewEnabled = false;
                databaseManager.AsyncLoad();
            }
            else
            {
                handleLoaded(new TECTemplates());
            }
        }

        protected override void handleLoaded(TECTemplates loaded)
        {
            templates = loaded;
            watcher = new ChangeWatcher(templates);
            doStack = new DoStacker(watcher);
            deltaStack = new DeltaStacker(watcher, templates);

            EditorVM = new TemplatesEditorVM(templates);
            CurrentVM = EditorVM;
            ViewEnabled = true;
        }

        #region Menu Commands Methods
        private void setupCommands()
        {
            menuVM.SetRefreshTemplatesCommand(refreshExecute, canRefresh);
            menuVM.SetExportTemplatesCommand(exportTemplatesExecute);
        }

        //Export Templates
        private void exportTemplatesExecute()
        {
            string path = UIHelpers.GetSavePath(FileDialogParameters.ExcelFileParameters,
                defaultFileName, defaultDirectory, workingFileDirectory);
            if (path != null)
            {
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    Templates.Export(path, templates);
                    logger.Info("Exported templates spreadsheet.");
                }
                else
                {
                    notifyFileLocked(path);
                }
            }
        }
        #endregion
        
        private string getStartUpFilePath()
        {
            string startUpFilePath = Properties.Settings.Default.StartUpFilePath;
            Properties.Settings.Default.StartUpFilePath = null;
            Properties.Settings.Default.Save();
            return startUpFilePath;
        }
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
