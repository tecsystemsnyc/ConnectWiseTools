using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TECUserControlLibrary.Utilities;
using TECUserControlLibrary.ViewModels;

namespace TemplateBuilder.MVVM
{
    public class TemplatesSplashVM : SplashVM
    {
        const string SPLASH_TITLE = "Welcome to Template Builder";
        const string SPLASH_SUBTITLE = "Please select object templates or create new templates";

        private string _templatesPath;
        
        public string TemplatesPath
        {
            get
            {
                return _templatesPath;
            }
            set
            {
                _templatesPath = value;
                RaisePropertyChanged("TemplatesPath");
            }
        }
        
        public event Action<string> Started;

        public TemplatesSplashVM(string templatesPath, string defaultDirectory) :
            base(SPLASH_TITLE, SPLASH_SUBTITLE, defaultDirectory)
        {
            _templatesPath = templatesPath;

            GetTemplatesPathCommand = new RelayCommand(getTemplatesPathExecute);
            OpenExistingCommand = new RelayCommand(openExistingExecute, openExistingCanExecute);
            CreateNewCommand = new RelayCommand(createNewExecute, createNewCanExecute);
        }

        private void getTemplatesPathExecute()
        {
            TemplatesPath = getPath(UIHelpers.TemplatesFileParameters, defaultDirectory);
        }

        private void openExistingExecute()
        {
            Started?.Invoke(TemplatesPath);
        }
        private bool openExistingCanExecute()
        {
            return (TemplatesPath != "");
        }

        private void createNewExecute()
        {
            Started?.Invoke("");
        }
        private bool createNewCanExecute()
        {
            return true;
        }
    }
}
