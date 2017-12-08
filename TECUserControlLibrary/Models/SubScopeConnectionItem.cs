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
        
        private TECElectricalMaterial noneConduit;

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
        public string EquipmentName { get; }

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

        public SubScopeConnectionItem(TECSubScope subScope, TECElectricalMaterial noneConduit, TECEquipment parent, bool needsUpdate = false)
        {
            SubScope = subScope;
            EquipmentName = parent.Name;
            NeedsUpdate = needsUpdate;
            if (subScope.IsTypical)
            {
                subScope.Connection.PropertyChanged += Connection_PropertyChanged;
            }
            this.noneConduit = noneConduit;
            ChangeConduitCommand = new RelayCommand(UpdateConduitExecute, CanUpdateConduit);
        }

        private bool CanUpdateConduit()
        {
            bool bothAreNone = (SelectedConduitType == noneConduit && SubScope.Connection.ConduitType == null);
            bool areSame = bothAreNone || (SelectedConduitType == SubScope.Connection?.ConduitType);

            return !areSame;
        }

        private void UpdateConduitExecute()
        {
            if (SelectedConduitType == noneConduit)
            {
                SubScope.Connection.ConduitType = null;
            }
            else
            {
                SubScope.Connection.ConduitType = SelectedConduitType;
            }
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
