using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.ViewModelExtensions
{
    public class StatusBarExtension : ViewModelBase
    {

        public string CurrentStatusText
        {
            get { return _currentStatusText; }
            private set
            {
                _currentStatusText = value;
                RaisePropertyChanged("CurrentStatusText");
            }
        }
        private string _currentStatusText;
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
        
        public string TECLogo { get; set; }
        public string Version { get; set; }
    }
}
