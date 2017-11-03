using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateBuilder.MVVM
{
    public class TemplatesMainVM : TECUserControlLibrary.BaseVMs.MainViewModel
    {
        public TemplatesMainVM() : base (new TemplateSplashVM(), new TemplatesMenuVM(), new TemplatesEditorVM())
        {

        }
    }
}
