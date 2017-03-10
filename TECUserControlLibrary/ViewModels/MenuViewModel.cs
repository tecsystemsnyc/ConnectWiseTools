using GalaSoft.MvvmLight;
using System.Windows;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModels
{

    public class MenuViewModel : ViewModelBase
    {

        public MenuViewModel()
        {

        }

        #region Properties

        #region Visibility Properties

        private Visibility _newFileVisibility;
        private Visibility _exportMenuVisibility;
        private Visibility _viewMenuVisibility;
        private Visibility _loadTemplatesVisibility;

        public Visibility NewFileVisibility
        {
            get { return _newFileVisibility; }
            set
            {
                _newFileVisibility = value;
                RaisePropertyChanged("NewFileVisibility");
            }
        }
        public Visibility ExportMenuVisibility
        {
            get { return _exportMenuVisibility; }
            set
            {
                _exportMenuVisibility = value;
                RaisePropertyChanged("ExportMenuVisibility");
            }
        }
        public Visibility ViewMenuVisibility
        {
            get { return _viewMenuVisibility; }
            set
            {
                _viewMenuVisibility = value;
                RaisePropertyChanged("ViewMenuVisibility");
            }
        }
        public Visibility LoadTemplatesVisibility
        {
            get { return _loadTemplatesVisibility; }
            set
            {
                _loadTemplatesVisibility = value;
                RaisePropertyChanged("LoadTemplatesVisibility");
            }
        }

        #endregion

        #region Command Properties

        public ICommand NewCommand { get; private set; }
        public ICommand LoadCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand SaveAsCommand { get; private set; }
        public ICommand ExportProposalCommand { get; private set; }
        public ICommand ExportPointsListCommand { get; private set; }
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }
        public ICommand ToggleTemplatesCommand { get; private set; }
        public ICommand LoadTemplatesCommand { get; private set; }
        public ICommand RefreshTemplatesCommand { get; private set; }

        #endregion

        #region Header Properties

        private string _toggleTemplatesHeader;
        public string ToggleTemplatesHeader
        {
            get { return _toggleTemplatesHeader; }
            set
            {
                _toggleTemplatesHeader = value;
                RaisePropertyChanged("ToggleTemplatesHeader");
            }
        }

        #endregion

        #endregion
    }
}