using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectWiseInformationInterface.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        private string _cwAppID;

        public string CWAppID
        {
            get { return _cwAppID; }
            set
            {
                _cwAppID = value;
                RaisePropertyChanged("CWAppID");
            }
        }

        public MainViewModel(){}
            
    }
}
