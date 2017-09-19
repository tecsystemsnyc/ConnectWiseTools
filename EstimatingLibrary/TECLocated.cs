using EstimatingLibrary.Interfaces;
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
                if(_location != value)
                {
                    var old = Location;
                    _location = value;
                    notifyCombinedChanged(Change.Edit, "Location", this, Location, old);
                }
            }
        }
        #endregion

        public TECLocated(Guid guid) : base(guid) { }

        #region Methods
        public void SetLocationFromParent(TECLabeled location)
        {
            Location = location;
        }

        protected void copyPropertiesFromLocated(TECLocated scope)
        {
            copyPropertiesFromScope(scope);
            if (scope.Location != null)
            { _location = scope.Location as TECLabeled; }
        }

        protected override SaveableMap saveObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.saveObjects());
            if(this.Location != null)
            {
                saveList.Add(this.Location, "Location");
            }
            return saveList;
        }
        protected override SaveableMap relatedObjects()
        {
            SaveableMap saveList = new SaveableMap();
            SaveableMap baseMap = base.relatedObjects();
            saveList.AddRange(baseMap);
            if (this.Location != null)
            {
                saveList.Add(this.Location, "Location");
            }
            return saveList;
        }
        #endregion
    }
}
