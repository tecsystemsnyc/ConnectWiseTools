using GalaSoft.MvvmLight;
using Microsoft.Win32;
using System.Windows.Input;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.ViewModels
{
    public abstract class SplashVM : ViewModelBase
    {
        
        protected string defaultDirectory;

        private string _titleText;
        private string _subtitleText;

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

        public ICommand GetTemplatesPathCommand { get; protected set; }
        public ICommand OpenExistingCommand { get; protected set; }
        public ICommand CreateNewCommand { get; protected set; }

        public SplashVM(string titleText, string subtitleText, string defaultDirectory)
        {
            this.defaultDirectory = defaultDirectory;
            _titleText = titleText;
            _subtitleText = subtitleText;
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