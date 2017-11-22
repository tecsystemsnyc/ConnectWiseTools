using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using TECUserControlLibrary.Interfaces;

namespace TECUserControlLibrary.Models
{
    public class SubScopeConnectionItem : ViewModelBase, ISubScopeConnectionItem
    {
        private bool _needsUpdate;
        private TECElectricalMaterial _selectedConduitType;

        public TECSubScope SubScope { get; private set; }
        public bool NeedsUpdate
        {
            get { return _needsUpdate; }
            set
            {
                _needsUpdate = value;
                RaisePropertyChanged("NeedsUpdate");
            }
        }

        public TECElectricalMaterial SelectedConduitType
        {
            get { return _selectedConduitType; }
            set
            {
                _selectedConduitType = value;
                RaisePropertyChanged("SelectedConduitType");
            }
        }
        public ICommand ChangeConduitCommand { get; private set; }

        public event Action<ISubScopeConnectionItem> PropagationPropertyChanged;

        public SubScopeConnectionItem(TECSubScope subScope, bool needsUpdate = false)
        {
            SubScope = subScope;
            NeedsUpdate = needsUpdate;
            if (subScope.IsTypical)
            {
                subScope.Connection.PropertyChanged += Connection_PropertyChanged;
            }
            ChangeConduitCommand = new RelayCommand(UpdateConduitExecute, CanUpdateConduit);
        }

        private bool CanUpdateConduit()
        {
            if(SelectedConduitType == SubScope.Connection.ConduitType)
            {
                return false;
            }
            return true;
        }

        private void UpdateConduitExecute()
        {
            SubScope.Connection.ConduitType = SelectedConduitType;
        }

        private void Connection_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            List<string> properties = new List<string>() { "Length", "ConduitLength", "ConduitType" };
            if (properties.Contains(e.PropertyName))
            {
                PropagationPropertyChanged?.Invoke(this);
            }
        }
    }
}
