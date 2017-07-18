using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using EstimatingLibrary.Interfaces;

namespace EstimatingLibrary
{
    public class TECLabeled : TECObject
    {
        #region Properties

        protected string _label;

        public string Label
        {
            get { return _label; }
            set
            {
                var temp = this.Copy();
                _label = value;
                // Call RaisePropertyChanged whenever the property is updated
                NotifyPropertyChanged("Label", temp, this);
            }
        }

        #endregion //Properties

        #region Constructors
        public TECLabeled(Guid guid) : base(guid)
        {
            _label = "";
        }
        public TECLabeled() : this(Guid.NewGuid()) { }

        public TECLabeled(TECLabeled source) : this()
        {
            _label = source.Label;
        }

        public override object Copy()
        {
            TECLabeled labeled = new TECLabeled(this);
            labeled._guid = Guid;
            return labeled;
        }
        #endregion //Constructors


    }
}
