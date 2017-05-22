using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows.Forms;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private string _defaultDirectory;
        public string DefaultDirectory
        {
            get { return _defaultDirectory; }
            set
            {
                _defaultDirectory = value;
                RaisePropertyChanged("DefaultDirectory");
            }
        }

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

        public ICommand ResetDefaultDirectoryCommand { get; private set; }
        public ICommand ReloadTemplatesCommand { get; private set; }

        public SettingsViewModel(string defaultDirectory, bool templatesHidden, string templatesLoadPath, ICommand reloadTemplatesCommand)
        {
            ResetDefaultDirectoryCommand = new RelayCommand(ResetDefaultDirectoryExecute);

            DefaultDirectory = defaultDirectory;
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

        private void ResetDefaultDirectoryExecute()
        {
            string newDefaultDirectory = getDirectory();
            if (newDefaultDirectory != null)
            {
                DefaultDirectory = newDefaultDirectory;
            }
        }

        private string getDirectory()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Select the folder that you want to use as the default.";
            dialog.RootFolder = Environment.SpecialFolder.Desktop;
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                return dialog.SelectedPath;
            }
            else
            {
                return "";
            }
        }
    }
}