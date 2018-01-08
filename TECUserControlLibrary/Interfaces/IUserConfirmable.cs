using System.Windows;

namespace TECUserControlLibrary.Interfaces
{
    public interface IUserConfirmable
    {
        MessageBoxResult Show(string message, string caption = null, MessageBoxButton? button = null, MessageBoxImage? icon = null);
    }
}
