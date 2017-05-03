using EstimatingLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECTag : TECObject, GuidObject
    {
        #region Properties
        private string _text;
        private Guid _guid;

        public string Text
        {
            get { return _text; }
            set
            {
                var temp = this.Copy();
                _text = value;
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
        public TECTag(Guid guid)
        {
            _text = "";
            _guid = guid;
        }
        public TECTag() : this(Guid.NewGuid()) { }

        public TECTag(TECTag sourceTag) : this(sourceTag.Guid)
        {
            _text = sourceTag.Text;
        }

        public override object Copy()
        {
            TECTag Tag = new TECTag(this);
            Tag._guid = Guid;
            return Tag;
        }
        #endregion //Constructors


    }
}
