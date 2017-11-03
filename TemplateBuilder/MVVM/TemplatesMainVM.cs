using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateBuilder.MVVM
{
    public class TemplatesMainVM : TECUserControlLibrary.BaseVMs.MainViewModel
    {
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

        public TemplatesMainVM() : base (new TemplatesSplashVM(), new TemplatesMenuVM(), new TemplatesEditorVM())
        {

        }
    }
}
