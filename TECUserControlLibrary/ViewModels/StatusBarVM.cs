using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.ViewModels
{
    public class StatusBarVM : ViewModelBase
    {
        private const string BUG_REPORT_URL = "https://goo.gl/forms/FvbdNI8gMHmmKcvn2";

        public string BugReportURL
        {
            get { return BUG_REPORT_URL; }
        }

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

        private string _version;
        public string Version
        {
            get { return _version; }
            set
            {
                _version = value;
                RaisePropertyChanged("Version");
            }
        }
    }
}
