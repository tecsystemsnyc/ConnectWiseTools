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
                var oldNew = Tuple.Create<Object, Object>(_location, value);
                var temp = Copy();
                _location = value;
                NotifyPropertyChanged("Location", temp, this);
                temp = Copy();
                NotifyPropertyChanged("ObjectPropertyChanged", temp, oldNew, typeof(TECScope), typeof(TECLabeled));
            }
        }
        #endregion

        public TECLocated(Guid guid) : base(guid) { }

        #region Methods
        public void SetLocationFromParent(TECLabeled location)
        {
            var oldNew = Tuple.Create<Object, Object>(_location, location);
            _location = location;
            RaisePropertyChanged("Location");
            var temp = Copy();
            NotifyPropertyChanged("ObjectPropertyChanged", temp, oldNew, typeof(TECScope), typeof(TECLabeled));
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
