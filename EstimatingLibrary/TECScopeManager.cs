using EstimatingLibrary.Utilities;
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
                var old = Labor;
                _labor = value;
                NotifyCombinedChanged(Change.Edit, "Labor", this, value, old);
            }
        }

        protected TECCatalogs _catalogs;
        virtual public TECCatalogs Catalogs
        {
            get { return _catalogs; }
            set
            {
                var old = Catalogs;
                _catalogs = value;
                NotifyCombinedChanged(Change.Edit, "Catalogs", this, value, old);
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
