using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{

    public abstract class TECScope : TECObject
    {
        #region Properties

        protected string _name;
        protected string _description;
        protected Guid _guid;
        protected int _quantity;
        protected TECLocation _location;

        protected ObservableCollection<TECTag> _tags;

        public string Name {
            get { return _name; }
            set
            {
                var temp = this.Copy();
                _name = value;
                // Call RaisePropertyChanged whenever the property is updated
                NotifyPropertyChanged("Name", temp, this);
                
            }
        }
        public string Description {
            get { return _description; }
            set
            {
                var temp = this.Copy();
                _description = value;
                // Call RaisePropertyChanged whenever the property is updated
                NotifyPropertyChanged("Description", temp, this);
            
            }
        }
        public Guid Guid
        {
            get { return _guid; }
        }

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                var temp = this.Copy();
                _quantity = value;
                NotifyPropertyChanged("Quantity", temp, this);
                
            }
        }
        public ObservableCollection<TECTag> Tags
        {
            get { return _tags; }
            set
            {
                    var temp = this.Copy();
                    _tags = value;
                    NotifyPropertyChanged("Tags", temp, this);
                
            }
        }

        public TECLocation Location
        {
            get { return _location; }
            set
            {
                var oldNew = Tuple.Create<Object, Object>(_location, value);
                var temp = Copy();
                _location = value;
                NotifyPropertyChanged("LocationChanged", temp, oldNew);
                NotifyPropertyChanged("Location", temp, this);
            }
        }
        
        #endregion //Properties

        #region Constructors
        public TECScope(string name, string description, Guid guid)
        {
            _name = name;
            _description = description;
            _guid = guid;

            _quantity = 1;
            _tags = new ObservableCollection<TECTag>();
        }


        #endregion //Constructors

    }
}
