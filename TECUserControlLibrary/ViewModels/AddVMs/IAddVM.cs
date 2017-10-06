using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModels.AddVMs
{
    public interface IAddVM
    {
        Action<object> Added { get; }
        ICommand AddCommand { get; }
    }
}
