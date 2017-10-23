using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TECUserControlLibrary.Interfaces;

namespace TECUserControlLibrary.Utilities
{
    public class MessageBoxService : IUserConfirmable
    {
        public MessageBoxService() { }

        public bool? Show(string message)
        {
            MessageBoxResult result = MessageBox.Show(message, "Confirm", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Yes)
            {
                return true;
            }
            else if (result == MessageBoxResult.No)
            {
                return false;
            }
            else
            {
                return null;
            }
        }
    }
}
