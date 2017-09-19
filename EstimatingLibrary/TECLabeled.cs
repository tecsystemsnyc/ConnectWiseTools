using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;

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
                var old = Label;
                _label = value;
                // Call raisePropertyChanged whenever the property is updated
                notifyCombinedChanged(Change.Edit, "Label", this, value, old);
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
        #endregion //Constructors
    }
}
