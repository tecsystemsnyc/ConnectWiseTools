using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public enum Flavor { Tag=1, Location, Note, Exclusion, Wire, Conduit }

    public abstract class TECObject : INotifyPropertyChanged
    {
        #region Properties
        protected Guid _guid;
        
        public Guid Guid
        {
            get { return _guid; }
        }

        public Flavor Flavor;
        #endregion

        #region Property Changed
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged<T>(string propertyName, T oldvalue, T newvalue)
        {
            RaiseExtendedPropertyChanged(this, new PropertyChangedExtendedEventArgs<T>(propertyName, oldvalue, newvalue));
        }
        protected void NotifyPropertyChanged<T>(string propertyName, T oldvalue, T newvalue, Type oldType, Type newType)
        {
            RaiseExtendedPropertyChanged(this, new PropertyChangedExtendedEventArgs<T>(propertyName, oldvalue, newvalue, oldType, newType));
        }
        protected void RaiseExtendedPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
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

        #region Methods
        abstract public Object Copy();
        #endregion
    }
}
