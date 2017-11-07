using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows.Input;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.ViewModels;

namespace EstimateBuilder.MVVM
{
    public class EstimateSplashVM : SplashVM
    {
        const string SPLASH_TITLE = "Welcome to Estimate Builder";
        const string SPLASH_SUBTITLE = "Please select object templates and select and existing file or create a new bid";
        
        private string _bidPath;
        private string _templatesPath;

        public string BidPath
        {
            get { return _bidPath; }
            set
            {
                _bidPath = value;
                RaisePropertyChanged("BidPath");
            }
        }
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

        public ICommand GetBidPathCommand { get; private set; }
        
        public event Action<string, string> EditorStarted;

        public EstimateSplashVM(string templatesPath, string defaultDirectory) :
            base(SPLASH_TITLE, SPLASH_SUBTITLE, defaultDirectory)
        {
            _bidPath = "";
            _templatesPath = templatesPath;
            
            GetBidPathCommand = new RelayCommand(getBidPathExecute);
            GetTemplatesPathCommand = new RelayCommand(getTemplatesPathExecute);
            OpenExistingCommand = new RelayCommand(openExistingExecute, openExistingCanExecute);
            CreateNewCommand = new RelayCommand(createNewExecute, createNewCanExecute);
        }
        
        private void getBidPathExecute()
        {
            BidPath = getPath(FileDialogParameters.EstimateFileParameters, defaultDirectory);
        }
        private void getTemplatesPathExecute()
        {
            TemplatesPath = getPath(FileDialogParameters.TemplatesFileParameters, defaultDirectory);
        }

        private void openExistingExecute()
        {
            EditorStarted?.Invoke(BidPath, TemplatesPath);
        }
        private bool openExistingCanExecute()
        {
            return (BidPath != "" && TemplatesPath != "");
        }
    
        private void createNewExecute()
        {
            EditorStarted?.Invoke("", TemplatesPath);
        }
        private bool createNewCanExecute()
        {
            return TemplatesPath != "";
        }
    }
}
