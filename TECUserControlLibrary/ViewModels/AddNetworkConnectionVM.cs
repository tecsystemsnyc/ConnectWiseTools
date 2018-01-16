using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModels
{
    public class AddNetworkConnectionVM : ViewModelBase
    {
        #region Fields and Properties
        private TECConnectionType _selectedAllConnectionType;
        private TECConnectionType _selectedChosenConnectionType;
        private IOType _selectedIOType;

        public TECConnectionType SelectedAllConnectionType
        {
            get { return _selectedAllConnectionType; }
            set
            {
                if (SelectedAllConnectionType != value)
                {
                    _selectedAllConnectionType = value;
                    RaisePropertyChanged("SelectedAllConnectionType");
                }
            }
        }
        public TECConnectionType SelectedChosenConnectionType
        {
            get { return _selectedChosenConnectionType; }
            set
            {
                if (SelectedChosenConnectionType != value)
                {
                    _selectedChosenConnectionType = value;
                    RaisePropertyChanged("SelectedChosenConnectionType");
                }
            }
        }
        public IOType SelectedIOType
        {
            get { return _selectedIOType; }
            set
            {
                if (SelectedIOType != value)
                {
                    _selectedIOType = value;
                    RaisePropertyChanged("SelectedIOType");
                }
            }
        }

        public INetworkParentable Parentable { get; }

        public ObservableCollection<TECConnectionType> AllConnectionTypes { get; }
        public ObservableCollection<TECConnectionType> ChosenConnectionTypes { get; }
        public ObservableCollection<IOType> IOTypes { get; }

        public ICommand AddConnectionTypeCommand { get; }
        public ICommand RemoveConnectionTypeCommand { get; }
        public ICommand AddConnectionCommand { get; }
        #endregion

        public AddNetworkConnectionVM(INetworkParentable parentable, IEnumerable<TECConnectionType> allConnectionTypes)
        {
            Parentable = parentable;
            AllConnectionTypes = new ObservableCollection<TECConnectionType>(allConnectionTypes);
            ChosenConnectionTypes = new ObservableCollection<TECConnectionType>();
            IOTypes = new ObservableCollection<IOType>();
            foreach(TECIO io in parentable.AvailableNetworkIO.ListIO())
            {
                IOTypes.Add(io.Type);
            }
        }

        #region Methods
        private void addConnectionTypeExecute()
        {
            ChosenConnectionTypes.Add(SelectedAllConnectionType);
        }
        private bool addConnectionTypeCanExecute()
        {
            return SelectedAllConnectionType != null;
        }

        private void removeConnectionTypeExecute()
        {
            ChosenConnectionTypes.Remove(SelectedChosenConnectionType);
        }
        private bool removeConnectionTypeCanExecute()
        {
            return SelectedChosenConnectionType != null;
        }

        private void addConnectionExecute()
        {
            Parentable.AddNetworkConnection(Parentable.IsTypical, ChosenConnectionTypes, SelectedIOType);
        }
        private bool addConnectionCanExecute()
        {
            return Parentable.CanAddNetworkConnection(SelectedIOType);
        }
        #endregion
    }
}
