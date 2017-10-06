using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModels.AddVMs
{
    public class AddSystemVM : ViewModelBase, IAddVM
    {
        public Action<object> Added => throw new NotImplementedException();

        public ICommand AddCommand => throw new NotImplementedException();
    }
}
