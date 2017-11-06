using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.BaseVMs;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.ViewModels;

namespace TemplateBuilder.MVVM
{
    public class TemplatesManager : AppManager
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
            
        }
    }
}
