using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {

        private bool _templatesHidden;
        public bool TemplatesHidden
        {
            get { return _templatesHidden; }
            set
            {
                if (value != _templatesHidden)
                {
                    _templatesHidden = value;
                    RaisePropertyChanged("TemplatesHidden");
                }
            }
        }

        private string _templatesLoadPath;
        public string TemplatesLoadPath
        {
            get
            {
                return _templatesLoadPath;
            }
            set
            {
                _templatesLoadPath = value;
                RaisePropertyChanged("TemplatesLoadPath");
            }
        }

        public ICommand ReloadTemplatesCommand { get; private set; }

        public SettingsViewModel(bool templatesHidden, string templatesLoadPath, ICommand reloadTemplatesCommand)
        {
            TemplatesHidden = templatesHidden;
            TemplatesLoadPath = templatesLoadPath;
            ReloadTemplatesCommand = reloadTemplatesCommand;
        }

        public void Refresh(bool templatesHidden, string templatesLoadPath, ICommand reloadTemplatesCommand)
        {
            TemplatesHidden = templatesHidden;
            TemplatesLoadPath = templatesLoadPath;
            ReloadTemplatesCommand = reloadTemplatesCommand;
        }
    }
}