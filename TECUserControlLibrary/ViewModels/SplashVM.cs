using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows.Input;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.Utilities;
using Microsoft.Win32;

namespace TECUserControlLibrary.ViewModels
{
    public class SplashVM : ViewModelBase
    {
        private string _bidPath;
        private string _templatesPath;
        private string _defaultDirectory;

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
        public ICommand GetTemplatesPathCommand { get; private set; }
        public ICommand OpenExistingCommand { get; private set; }
        public ICommand CreateNewCommand { get; private set; }

        public event Action<string, string> Started;

        public SplashVM(string initialTemplates, string defaultDirectory)
        {
            GetBidPathCommand = new RelayCommand(getBidPathExecute);
            GetTemplatesPathCommand = new RelayCommand(getTemplatesPathExecute);
            OpenExistingCommand = new RelayCommand(openExistingExecute, openExistingCanExecute);
            CreateNewCommand = new RelayCommand(createNewExecute, createNewCanExecute);

            _bidPath = "";
            _defaultDirectory = defaultDirectory;
            _templatesPath = initialTemplates;
        }

        private void getBidPathExecute()
        {
            BidPath = getPath(UIHelpers.BidFileParameters, _defaultDirectory);
        }

        private void getTemplatesPathExecute()
        {
            TemplatesPath = getPath(UIHelpers.TemplatesFileParameters, _defaultDirectory);
        }

        private void openExistingExecute()
        {
            Started?.Invoke(BidPath, TemplatesPath);
        }
        private bool openExistingCanExecute()
        {
            if(BidPath != "" && TemplatesPath != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void createNewExecute()
        {
            Started?.Invoke("", TemplatesPath);
        }
        private bool createNewCanExecute()
        {
            if (TemplatesPath != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected string getPath(FileDialogParameters fileParams, string initialDirectory = null)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = fileParams.Filter;
            openFileDialog.DefaultExt = fileParams.DefaultExtension;
            openFileDialog.AddExtension = true;

            string savePath = null;
            if (openFileDialog.ShowDialog() == true)
            {
                savePath = openFileDialog.FileName;
            }
            return savePath;
        }
    }
}