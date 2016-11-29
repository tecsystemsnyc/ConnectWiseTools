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
                NotifyPropertyChanged("Text", temp, this);
            }
        }

        public Guid Guid
        {
            get { return _guid; }
        }

        #endregion //Properties

        #region Constructors
        public TECLocation(string name, Guid guid)
        {
            _name = name;
            _guid = guid;
        }
        public TECLocation(string name) : this(name, Guid.NewGuid()) { }
        public TECLocation() : this("") { }

        public TECLocation(TECLocation sourceLocation) : this(sourceLocation.Name, sourceLocation.Guid) { }

        public override object Copy()
        {
            TECLocation Location = new TECLocation(this);
            Location._guid = Guid;
            return Location;
        }
        #endregion //Constructors
    }
}
