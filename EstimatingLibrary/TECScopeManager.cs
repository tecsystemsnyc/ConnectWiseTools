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
        protected TECLabor _labor;
        virtual public TECLabor Labor
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
        virtual public TECCatalogs Catalogs
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

        protected TECScopeManager(Guid guid): base(guid)
        {
            _labor = new TECLabor();
            _catalogs = new TECCatalogs();
        }

        protected TECScopeManager() : this(Guid.NewGuid()) { }
    }
}
