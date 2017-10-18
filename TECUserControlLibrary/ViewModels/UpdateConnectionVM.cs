using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.ViewModels
{
    public class UpdateConnectionVM : ViewModelBase
    {
        #region Fields
        private Dictionary<SubScopeUpdatedWrapper, SubScopeConnectionItem> typicalDictionary;

        private readonly List<SubScopeUpdatedWrapper> _subScope;
        private SubScopeUpdatedWrapper _selectedInstance;
        private SubScopeConnectionItem _selectedTypical;
        #endregion

        #region Properties
        public List<SubScopeUpdatedWrapper> SubScope
        {
            get { return _subScope; }
        }
        public SubScopeUpdatedWrapper SelectedInstance
        {
            get { return _selectedInstance; }
            set
            {
                _selectedInstance = value;
                RaisePropertyChanged("SelectedInstance");
                handleNewSelectedInstance(value);
            }
        }
        public SubScopeConnectionItem SelectedTypical
        {
            get { return _selectedTypical; }
            set
            {
                _selectedTypical = value;
                RaisePropertyChanged("SelectedTypical");
            }
        }

        public ICommand UpdateCommand { get; private set; }
        #endregion

        public UpdateConnectionVM(IEnumerable<SubScopeConnectionItem> subScope)
        {
            initializeCollections();
            List<SubScopeUpdatedWrapper> instances = new List<SubScopeUpdatedWrapper>();
            foreach(SubScopeConnectionItem typ in subScope)
            {
                foreach(TECSubScope instance in typ.Instances)
                {
                    SubScopeUpdatedWrapper wrapped = new SubScopeUpdatedWrapper(instance);
                    instances.Add(wrapped);
                    typicalDictionary.Add(wrapped, typ);
                }
            }
            _subScope = instances;

            UpdateCommand = new RelayCommand(updateExecute, canUpdate);
        }

        private void initializeCollections()
        {
            typicalDictionary = new Dictionary<SubScopeUpdatedWrapper, SubScopeConnectionItem>();
        }

        private void handleNewSelectedInstance(SubScopeUpdatedWrapper subScope)
        {
            SelectedTypical = typicalDictionary[subScope];
        }

        private void updateExecute()
        {
            SelectedTypical.UpdateInstance(SelectedInstance.SubScope);
            SelectedInstance.Updated = true;
        }
        private bool canUpdate()
        {
            if (SelectedInstance == null)
            {
                return false;
            }
            else
            {
                return !SelectedInstance.Updated;
            }
        }
    }

    public class SubScopeUpdatedWrapper : INotifyPropertyChanged
    {
        private readonly TECSubScope _subScope;
        private bool _updated;

        public TECSubScope SubScope { get { return _subScope; } }
        public bool Updated
        {
            get { return _updated; }
            set
            {
                _updated = value;
                raisePropertyChanged("Updated");
            }
        }

        public SubScopeUpdatedWrapper(TECSubScope subScope)
        {
            _subScope = subScope;
            _updated = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void raisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
