using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace EstimatingLibrary
{
    public class TECNote : TECObject
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
        public TECNote(string text, Guid guid)
        {
            _text = text;
            _guid = guid;
        }
        public TECNote(string text) : this(text, Guid.NewGuid()) { }
        public TECNote() : this("") { }

        public TECNote(TECNote sourceNote) : this(sourceNote.Text) { }

        public override object Copy()
        {
            TECNote note = new TECNote(this);
            note._guid = Guid;
            return note;
        }
        #endregion //Constructors


    }
}
