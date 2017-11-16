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
            if(BidPath == null)
            {
                BidPath = "";
            }
        }
        private void getTemplatesPathExecute()
        {
            TemplatesPath = getPath(FileDialogParameters.TemplatesFileParameters, defaultDirectory);
            if(TemplatesPath == null)
            {
                TemplatesPath = "";
            }
        }

        private void openExistingExecute()
        {
            if (!File.Exists(BidPath))
            {
                MessageBox.Show("Bid file no longer exists at that path.");
            }else if (!File.Exists(TemplatesPath))
            {
                MessageBox.Show("Templates file no longer exist at that path.");
            } else
            {
                EditorStarted?.Invoke(BidPath, TemplatesPath);
            }
        }
        private bool openExistingCanExecute()
        {
            return (BidPath != "" && TemplatesPath != ""
                && BidPath != null && TemplatesPath != null);
        }
    
        private void createNewExecute()
        {
            if (!File.Exists(TemplatesPath))
            {
                MessageBox.Show("Templates file no longer exist at that path.");
            } else
            {
                EditorStarted?.Invoke("", TemplatesPath);
            }
        }
        private bool createNewCanExecute()
        {
            bool outBool = false;
            if(TemplatesPath == "" || TemplatesPath == null)
            {
                HintText = "Select Templates file (.tdb) to get started.";
            }
            else
            {
                HintText = "";
                outBool = true;
            }
            return outBool;
        }
    }
}
