using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public enum Change { Add, Remove, Edit }

    public abstract class TECObject : INotifyPropertyChanged, INotifyTECChanged
    {
        #region Properties
        protected Guid _guid;
        
        public Guid Guid
        {
            get { return _guid; }
        }

        #endregion

        #region Property Changed
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<TECChangedEventArgs> TECChanged;

        protected void NotifyTECChanged(Change change, string propertyName, TECObject sender,
            object value, object oldValue = null)
        {
            TECChangedEventArgs args = new TECChangedEventArgs(change, propertyName, sender, value, oldValue);
            TECChanged?.Invoke(args);
        }

        protected void NotifyCombinedChanged(Change change, string propertyName, TECObject sender,
            object value, object oldValue = null)
        {
            TECChangedEventArgs args = new TECChangedEventArgs(change, propertyName, sender, value, oldValue);
            PropertyChanged?.Invoke(sender, args);
            TECChanged?.Invoke(args);
        }

        protected void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion //Property Changed

        public TECObject(Guid guid)
        {
            _guid = guid;
        }
    }
}
