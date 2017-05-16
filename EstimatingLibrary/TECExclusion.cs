using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace EstimatingLibrary
{
    public class TECExclusion : TECObject
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
        public TECExclusion(Guid guid)
        {
            _text = "";
            _guid = guid;
        }
        public TECExclusion() : this(Guid.NewGuid()) { }

        public TECExclusion(TECExclusion exclusionSource) : this()
        {
            _text = exclusionSource.Text;
        }

        public override Object Copy()
        {
            TECExclusion exclusion = new TECExclusion(this);
            exclusion._guid = this.Guid;
            return exclusion;
        }
        #endregion //Constructors
    }
}
