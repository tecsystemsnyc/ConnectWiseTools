using System;

namespace EstimatingLibrary
{
    public abstract class TECScopeManager : TECObject
    {
        #region Properties
        protected TECCatalogs _catalogs;
        virtual public TECCatalogs Catalogs
        {
            get { return _catalogs; }
            set
            {
                var old = Catalogs;
                _catalogs = value;
                notifyCombinedChanged(Change.Edit, "Catalogs", this, value, old);
            }
        }

        #endregion

        protected TECScopeManager(Guid guid): base(guid)
        {
            _catalogs = new TECCatalogs();
        }

        protected TECScopeManager() : this(Guid.NewGuid()) { }
    }
}
