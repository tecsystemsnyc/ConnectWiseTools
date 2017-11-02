using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows.Input;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.Utilities;
using Microsoft.Win32;
using System.Windows;
using TECUserControlLibrary.BaseVMs;

namespace TECUserControlLibrary.ViewModels
{
    public class SplashVM : ViewModelBase
    {
        private string _bidPath;
        private string _templatesPath;
        private string _defaultDirectory;
        private Visibility _bidVisibility;
        private string _titleText;
        private string _subtitleText;
        private BuilderType _type;
        private string _loadingText;

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
        public Visibility BidVisibility
        {
            get
            {
                return _bidVisibility;
            }
            set
            {
                _bidVisibility = value;
                RaisePropertyChanged("BidVisibility");
            }
        }
        public string TitleText
        {
            get { return _titleText; }
            set
            {
                _titleText = value;
                RaisePropertyChanged("TitleText");
            }
        }
        public string SubtitleText
        {
            get { return _subtitleText; }
            set
            {
                _subtitleText = value;
                RaisePropertyChanged("SubtitleText");
            }
        }
        public string LoadingText
        {
            get { return _loadingText; }
            set
            {
                _loadingText = value;
                RaisePropertyChanged("LoadingText");
            }
        }
        
        public ICommand GetBidPathCommand { get; private set; }
        public ICommand GetTemplatesPathCommand { get; private set; }
        public ICommand OpenExistingCommand { get; private set; }
        public ICommand CreateNewCommand { get; private set; }

        public event Action<string, string> Started;

        public SplashVM(string titleText, string subtitleText, string initialTemplates, string defaultDirectory, BuilderType type)
        {
            GetBidPathCommand = new RelayCommand(getBidPathExecute);
            GetTemplatesPathCommand = new RelayCommand(getTemplatesPathExecute);
            OpenExistingCommand = new RelayCommand(openExistingExecute, openExistingCanExecute);
            CreateNewCommand = new RelayCommand(createNewExecute, createNewCanExecute);

            _bidPath = "";
            _defaultDirectory = defaultDirectory;
            _templatesPath = initialTemplates;
            _titleText = titleText;
            _subtitleText = subtitleText;
            _type = type;
            if (_type == BuilderType.EB)
            {
                _bidVisibility = Visibility.Visible;
            }
            else if (_type == BuilderType.TB)
            {
                _bidVisibility = Visibility.Collapsed;
            }
            else
            {
                throw new NotImplementedException();
            }
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
            LoadingText = "Loading...";
            Started?.Invoke(BidPath, TemplatesPath);
        }
        private bool openExistingCanExecute()
        {
            return ((BidPath != "" && TemplatesPath != "") ||
                ((_type == BuilderType.TB) && TemplatesPath != ""));
        }

        private void createNewExecute()
        {
            LoadingText = "Creating...";
            Started?.Invoke("", TemplatesPath);
        }
        private bool createNewCanExecute()
        {
            return (TemplatesPath != "" || (_type == BuilderType.TB));
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