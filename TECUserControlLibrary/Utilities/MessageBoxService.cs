using System.Windows;
using TECUserControlLibrary.Interfaces;

namespace TECUserControlLibrary.Utilities
{
    public class MessageBoxService : IUserConfirmable
    {
        public MessageBoxService() { }

        public MessageBoxResult Show(string message, string caption = null, MessageBoxButton? button = null, MessageBoxImage? icon = null)
        {
            if (icon.HasValue)
            {
                return MessageBox.Show(message, caption, button.Value, icon.Value);
            }
            else if (button.HasValue)
            {
                return MessageBox.Show(message, caption, button.Value);
            }
            else if (caption != null)
            {
                return MessageBox.Show(message, caption);
            }
            else
            {
                return MessageBox.Show(message);
            }
        }
    }
}
