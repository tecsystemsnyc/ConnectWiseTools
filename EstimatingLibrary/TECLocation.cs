using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECLocation : TECObject
    {
        #region Properties

        private string _name;
        private Guid _guid;

        public string Name
        {
            get { return _name; }
            set
            {
                var temp = this.Copy();
                _name = value;
                // Call RaisePropertyChanged whenever the property is updated
                NotifyPropertyChanged("Name", temp, this);
            }
        }

        public Guid Guid
        {
            get { return _guid; }
        }

        #endregion //Properties

        #region Constructors
        public TECLocation(Guid guid)
        {
            _name = "";
            _guid = guid;
        }
        public TECLocation() : this(Guid.NewGuid()) { }

        public TECLocation(TECLocation sourceLocation) : this(sourceLocation.Guid)
        {
            _name = sourceLocation.Name;
        }
        
        #endregion //Constructors

        public override object Copy()
        {
            TECLocation Location = new TECLocation(this);
            Location._guid = Guid;
            return Location;
        }
    }
}
