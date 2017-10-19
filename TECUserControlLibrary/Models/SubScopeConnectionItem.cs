using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TECUserControlLibrary.Models
{
    public class SubScopeConnectionItem : ViewModelBase
    {
        private bool _needsUpdate;

        public TECSubScope SubScope { get; private set; }
        public bool NeedsUpdate
        {
            get { return _needsUpdate; }
            private set
            {
                _needsUpdate = value;
                RaisePropertyChanged("NeedsUpdate");
                NeedsUpdateChanged?.Invoke();
            }
        }
        public event Action NeedsUpdateChanged;

        public SubScopeConnectionItem(TECSubScope subScope, bool needsUpdate = false)
        {
            SubScope = subScope;
            NeedsUpdate = needsUpdate;
            if (subScope.IsTypical)
            {
                subScope.Connection.PropertyChanged += Connection_PropertyChanged;
            }
        }

        private void Connection_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            List<string> properties = new List<string>() { "Length", "ConduitLength", "ConduitType" };
            if (properties.Contains(e.PropertyName))
            {
                NeedsUpdate = true;
            }
        }
    }
}
