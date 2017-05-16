using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public abstract class TECScopeManager : TECObject
    {
        #region Properties
        private Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        protected TECLabor _labor;
        public TECLabor Labor
        {
            get { return _labor; }
            set
            {
                var temp = Copy();
                _labor = value;
                NotifyPropertyChanged("Labor", temp, this);
            }
        }

        protected TECCatalogs _catalogs;
        public TECCatalogs Catalogs
        {
            get { return _catalogs; }
            set
            {
                var temp = Copy();
                _catalogs = value;
                NotifyPropertyChanged("Catalogs", temp, this);
            }
        }

        #endregion

        protected TECScopeManager(Guid guid)
        {
            _guid = guid;
            _labor = new TECLabor();
            _catalogs = new TECCatalogs();
        }

        protected TECScopeManager() : this(Guid.NewGuid()) { }
    }
}
