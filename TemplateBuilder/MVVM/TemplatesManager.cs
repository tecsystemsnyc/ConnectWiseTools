using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.BaseVMs;

namespace TemplateBuilder.MVVM
{
    public class TemplatesManager : AppManager
    {
        private TECTemplates templates;

        public TemplatesManager() : base(new TemplatesMainVM())
        {
        }
    }
}
