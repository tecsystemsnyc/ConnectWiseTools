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
            get
            {
                if (SubScope.Connection?.ConduitType == null)
                {
                    return noneConduit;
                }
                else
                {
                    return SubScope.Connection?.ConduitType;
                }
            }
            set
            {
                if (value == noneConduit)
                {
                    SubScope.Connection.ConduitType = null;
                }
                else if (value != null)
                {
                    SubScope.Connection.ConduitType = value;
                }
                RaisePropertyChanged("SelectedConduitType");
            }
        }

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
        }

        private void Connection_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            List<string> properties = new List<string>() { "Length", "ConduitLength", "ConduitType", "IsPlenum" };
            if (properties.Contains(e.PropertyName))
            {
                PropagationPropertyChanged?.Invoke(this);
            }
        }
    }
}
