using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstimatingLibrary;

namespace TECUserControlLibrary.ViewModels
{
    public class TypicalHierarchyVM : SystemHierarchyVM
    {
        private TECTypical selectedTypical;

        public TECTypical SelectedTypical
        {
            get { return selectedTypical; }
            set
            {
                selectedTypical = value;
                RaisePropertyChanged("SelectedTypical");
                NotifySelected(value);
            }
        }

        public TypicalHierarchyVM(TECCatalogs scopeCatalogs) : base(scopeCatalogs)
        {
        }
    }
}
