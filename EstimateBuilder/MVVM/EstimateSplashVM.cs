using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.IO;
using System.Windows;
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
        public ICommand ClearBidPathCommand { get; private set; }
        
        public event Action<string, string> EditorStarted;

        public EstimateSplashVM(string templatesPath, string defaultDirectory) :
            base(SPLASH_TITLE, SPLASH_SUBTITLE, defaultDirectory)
        {
            _bidPath = "";
            _templatesPath = templatesPath;
            
            GetBidPathCommand = new RelayCommand(getBidPathExecute);
            ClearBidPathCommand = new RelayCommand(clearBidPathExecute);
            GetTemplatesPathCommand = new RelayCommand(getTemplatesPathExecute);
            ClearTemplatesPathCommand = new RelayCommand(clearTemplatesPathExecute);
            OpenExistingCommand = new RelayCommand(openExistingExecute, openExistingCanExecute);
            CreateNewCommand = new RelayCommand(createNewExecute, createNewCanExecute);
        }
        
        private void getBidPathExecute()
        {
            string path = getPath(FileDialogParameters.EstimateFileParameters, defaultDirectory);
            if(path != null)
            {
                BidPath = path;
            }
        }
        private void clearBidPathExecute()
        {
            BidPath = "";
        }
        private void getTemplatesPathExecute()
        {
            string path = getPath(FileDialogParameters.TemplatesFileParameters, defaultDirectory);
            if(path != null)
            {
                TemplatesPath = path;
            }
        }
        private void clearTemplatesPathExecute()
        {
            TemplatesPath = "";
        }

        private void openExistingExecute()
        {
            LoadingText = "Loading...";
            if (!File.Exists(BidPath))
            {
                MessageBox.Show("Bid file no longer exists at that path.");
            }
            else if (TemplatesPath == "" || TemplatesPath == null)
            {
                MessageBoxResult result = MessageBox.Show("No templates have been selected. Would you like to continue?", "Continue?", MessageBoxButton.YesNo,
                    MessageBoxImage.Exclamation);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        EditorStarted?.Invoke(BidPath, "");
                        break;
                    default:
                        LoadingText = "";
                        break;
                }
            }
            else if (!File.Exists(TemplatesPath))
            {
                MessageBox.Show("Templates file no longer exist at that path.");
                LoadingText = "";
            }
            else
            {
                EditorStarted?.Invoke(BidPath, TemplatesPath);
            }
        }
        private bool openExistingCanExecute()
        {
            return (BidPath != "" && BidPath != null);
        }
    
        private void createNewExecute()
        {
            LoadingText = "Loading...";
            if (TemplatesPath == "" || TemplatesPath == null)
            {
                MessageBoxResult result = MessageBox.Show("No templates have been selected. Would you like to continue?", "Continue?", MessageBoxButton.YesNo,
                    MessageBoxImage.Exclamation);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        EditorStarted?.Invoke("", "");
                        break;
                    default:
                        LoadingText = "";
                        break;
                }
            }
            else if (!File.Exists(TemplatesPath))
            {
                MessageBox.Show("Templates file no longer exist at that path.");
                LoadingText = "";
            }
            else
            {
                EditorStarted?.Invoke("", TemplatesPath);
            }
        }
        private bool createNewCanExecute()
        {
            return true;
        }
    }
}
