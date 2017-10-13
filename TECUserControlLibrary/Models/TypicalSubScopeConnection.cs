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
    public class TypicalSubScopeConnection : ViewModelBase
    {
        private ObservableCollection<TECSubScopeConnection> _instances;
        private bool _needsUpdate;

        public TECSubScopeConnection Connection { get; private set; }
        public ReadOnlyObservableCollection<TECSubScopeConnection> Instances { get { return new ReadOnlyObservableCollection<TECSubScopeConnection>(_instances); } }
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
        public event Action<IEnumerable<TECSubScopeConnection>> Update;

        public TypicalSubScopeConnection(TECSubScopeConnection connection, IEnumerable<TECSubScopeConnection> instances)
        {
            Connection = connection;
            _instances = new ObservableCollection<TECSubScopeConnection>(instances);
            NeedsUpdate = true;

            connection.PropertyChanged += Connection_PropertyChanged;
        }

        public void UpdateInstance(TECSubScopeConnection instance)
        {
            
        }
        public void AddInstance(TECSubScopeConnection instance)
        {
            _instances.Add(instance);
        }
        public void RemoveInstance(TECSubScopeConnection instance)
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
