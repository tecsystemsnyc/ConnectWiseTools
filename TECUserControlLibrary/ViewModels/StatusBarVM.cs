using GalaSoft.MvvmLight;

namespace TECUserControlLibrary.ViewModels
{
    public class StatusBarVM : ViewModelBase
    {
        private string _currentStatusText;
        public string CurrentStatusText
        {
            get { return _currentStatusText; }
            set
            {
                _currentStatusText = value;
                RaisePropertyChanged("CurrentStatusText");
            }
        }

        private string _contextText;
        public string ContextText
        {
            get { return _contextText; }
            set
            {
                _contextText = value;
                RaisePropertyChanged("ContextText");
            }
        }

        private string _versionString;
        public string VersionString
        {
            get { return _versionString; }
            private set
            {
                _versionString = value;
                RaisePropertyChanged("VersionString");
            }
        }

        public void SetVersionNumber(string versionNumber)
        {
            VersionString = string.Format("Version: {0}", versionNumber);
        }
    }
}
