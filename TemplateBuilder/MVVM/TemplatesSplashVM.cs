using GalaSoft.MvvmLight.CommandWpf;
using System;
using TECUserControlLibrary.Models;
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
        
        public event Action<string> EditorStarted;

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
            TemplatesPath = getPath(FileDialogParameters.TemplatesFileParameters, defaultDirectory);
        }

        private void openExistingExecute()
        {
            EditorStarted?.Invoke(TemplatesPath);
        }
        private bool openExistingCanExecute()
        {
            return (TemplatesPath != "");
        }

        private void createNewExecute()
        {
            EditorStarted?.Invoke("");
        }
        private bool createNewCanExecute()
        {
            return true;
        }
    }
}
