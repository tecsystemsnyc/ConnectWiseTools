using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConnectWiseInformationInterface.ViewModel
{
    public class SettingsVM : ViewModelBase
    {
        private string _appId;
        private string _site;
        private string _companyName;
        private string _publicKey;
        private string _privateKey;

        public string AppID
        {
            get { return _appId; }
            set
            {
                if (AppID != value)
                {
                    _appId = value;
                    RaisePropertyChanged("AppID");
                }
            }
        }
        public string Site
        {
            get { return _site; }
            set
            {
                if (Site != value)
                {
                    _site = value;
                    RaisePropertyChanged("Site");
                }
            }
        }
        public string CompanyName
        {
            get { return _companyName; }
            set
            {
                if (CompanyName != value)
                {
                    _companyName = value;
                    RaisePropertyChanged("CompanyName");
                }
            }
        }
        public string PublicKey
        {
            get { return _publicKey; }
            set
            {
                if (PublicKey != value)
                {
                    _publicKey = value;
                    RaisePropertyChanged("PublicKey");
                }
            }
        }
        public string PrivateKey
        {
            get { return _privateKey; }
            set
            {
                if (PrivateKey != value)
                {
                    _privateKey = value;
                    RaisePropertyChanged("PrivateKey");
                }
            }
        }

        public ICommand SaveSettingsCommand { get; private set; }

        public SettingsVM()
        {
            AppID = Properties.Settings.Default.AppID;
            Site = Properties.Settings.Default.Site;
            CompanyName = Properties.Settings.Default.CompanyName;
            PublicKey = Properties.Settings.Default.PublicKey;
            PrivateKey = Properties.Settings.Default.PrivateKey;

            SaveSettingsCommand = new RelayCommand(saveSettingsExecute, saveSettingsCanExecute);
        }

        public bool CanLoad()
        {
            bool canLoad =
                (AppID != "") &&
                (Site != "") &&
                (CompanyName != "") &&
                (PublicKey != "") &&
                (PrivateKey != "");
            return canLoad;
        }

        private void saveSettingsExecute()
        {
            Properties.Settings.Default.AppID = AppID;
            Properties.Settings.Default.Site = Site;
            Properties.Settings.Default.CompanyName = CompanyName;
            Properties.Settings.Default.PublicKey = PublicKey;
            Properties.Settings.Default.PrivateKey = PrivateKey;

            Properties.Settings.Default.Save();
        }

        private bool saveSettingsCanExecute()
        {
            bool settingsDifferent =
                (Properties.Settings.Default.AppID != AppID) ||
                (Properties.Settings.Default.Site != Site) ||
                (Properties.Settings.Default.CompanyName != CompanyName) ||
                (Properties.Settings.Default.PublicKey != PublicKey) ||
                (Properties.Settings.Default.PrivateKey != PrivateKey);
            return settingsDifferent;
        }
    }
}
