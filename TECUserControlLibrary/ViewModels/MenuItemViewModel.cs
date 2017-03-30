using GalaSoft.MvvmLight;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MenuItemViewModel : ViewModelBase
    {
        private string _displayName;
        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = value;
                RaisePropertyChanged("DisplayName");
            }
        }

        public ICommand Command { get; set; }

        /// <summary>
        /// Initializes a new instance of the MenuItemViewModel class.
        /// </summary>
        public MenuItemViewModel()
        {

        }
    }
}