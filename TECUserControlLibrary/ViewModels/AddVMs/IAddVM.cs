using System;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModels.AddVMs
{
    public interface IAddVM
    {
        Action<object> Added { get; }
        ICommand AddCommand { get; }
    }
}
