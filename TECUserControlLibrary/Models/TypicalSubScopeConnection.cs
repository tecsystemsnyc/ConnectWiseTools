using EstimatingLibrary;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TECUserControlLibrary.Models
{
    public class TypicalSubScope : ViewModelBase
    {
        private ObservableCollection<TECSubScope> _instances;
        private bool _needsUpdate;

        public TECSubScope SubScope { get; private set; }
        public ReadOnlyObservableCollection<TECSubScope> Instances { get { return new ReadOnlyObservableCollection<TECSubScope>(_instances); } }
        public bool NeedsUpdate
        {
            get { return _needsUpdate; }
            private set
            {
                _needsUpdate = value;
                RaisePropertyChanged("NeedsUpdate");
            }
        }
        public ICommand UpdateCommand { get; private set; }
        public event Action<IEnumerable<TECSubScope>> Update;

        public TypicalSubScope(TECSubScope subScope, IEnumerable<TECSubScope> instances)
        {
            SubScope = subScope;
            _instances = new ObservableCollection<TECSubScope>(instances);
            NeedsUpdate = true;

            subScope.Connection.PropertyChanged += Connection_PropertyChanged;
        }

        public bool UpdateInstance(TECSubScope instance)
        {
            bool sameController = SubScope.ParentConnection.ParentController == instance.ParentConnection.ParentController;

            if (sameController)
            {
                updateProperties(SubScope, instance);
                return true;
            }
            else
            {
                bool canAdd = SubScope.ParentConnection.ParentController.CanConnectSubScope(instance);
                if (canAdd)
                {
                    SubScope.ParentConnection.ParentController.AddSubScope(instance);
                    updateProperties(SubScope, instance);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            void updateProperties(TECSubScope typical, TECSubScope toUpdate)
            {
                toUpdate.Connection.Length = typical.Connection.Length;
                toUpdate.Connection.ConduitLength = typical.Connection.ConduitLength;
                toUpdate.Connection.ConduitType = typical.Connection.ConduitType;
            }
        }
        public void AddInstance(TECSubScope instance)
        {
            _instances.Add(instance);
        }
        public void RemoveInstance(TECSubScope instance)
        {
            if (_instances.Contains(instance))
            {
                _instances.Remove(instance);
            }
            else
            {
                throw new InvalidOperationException("Instance doesnt exist in TypicalSubScopeConnection instances.");
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
        private void UpdateExecute()
        {
            NeedsUpdate = false;
            Update?.Invoke(Instances);
        }
    }
}
