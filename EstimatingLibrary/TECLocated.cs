using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public abstract class TECLocated : TECScope
    {
        #region Properties
        protected TECLabeled _location;

        public TECLabeled Location
        {
            get { return _location; }
            set
            {
                var old = Location;
                _location = value;
                NotifyCombinedChanged(Change.Edit, "Location", this, value, old);
                
                //NotifyCombinedChanged(Change.Edit, "ObjectPropertyChanged", temp, oldNew, typeof(TECScope), typeof(TECLabeled));
            }
        }
        #endregion

        public TECLocated(Guid guid) : base(guid) { }

        #region Methods
        public void SetLocationFromParent(TECLabeled location)
        {
            var old = Location;
            _location = location;
            RaisePropertyChanged("Location");
            NotifyCombinedChanged(Change.Edit, "Location", this, location, old);

        }

        protected void copyPropertiesFromLocated(TECLocated scope)
        {
            copyPropertiesFromScope(scope);
            if (scope.Location != null)
            { _location = scope.Location as TECLabeled; }
        }
        #endregion
    }
}
