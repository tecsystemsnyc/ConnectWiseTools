using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECBidParameters : TECObject
    {
        #region Properties
        private double _escalation;
        private double _overhead;
        private double _profit;
        private double _subcontractorMarkup;
        private double _subcontractorEscalation;

        private bool _isTaxExempt;
        private bool _requiresBond;
        private bool _requiresWrapUp;
        private bool _hasBMS;
       
        public double Escalation
        {
            get { return _escalation; }
            set
            {
                var old = Escalation;
                _escalation = value;
                NotifyPropertyChanged(Change.Edit, "Escalation", this, value, old);
            }
        }
        public double Overhead
        {
            get { return _overhead; }
            set
            {
                var old = Overhead;
                _overhead = value;
                NotifyPropertyChanged(Change.Edit, "Overhead", this, value, old);
            }
        }
        public double Profit
        {
            get { return _profit; }
            set
            {
                var old = Profit;
                _profit = value;
                NotifyPropertyChanged(Change.Edit, "Profit", this, value, old);
            }
        }
        public double SubcontractorMarkup
        {
            get { return _subcontractorMarkup; }
            set
            {
                var old = SubcontractorMarkup;
                _subcontractorMarkup = value;
                NotifyPropertyChanged(Change.Edit, "SubcontractorMarkup", this, value, old);
            }
        }
        public double SubcontractorEscalation
        {
            get { return _subcontractorEscalation; }
            set
            {
                var old = SubcontractorEscalation;
                _subcontractorEscalation = value;
                NotifyPropertyChanged(Change.Edit, "SubcontractorEscalation", this, value, old);
            }
        }

        public bool IsTaxExempt
        {
            get { return _isTaxExempt; }
            set
            {
                var old = IsTaxExempt;
                _isTaxExempt = value;
                NotifyPropertyChanged(Change.Edit, "IsTaxExempt", this, value, old);
            }
        }
        public bool RequiresBond
        {
            get { return _requiresBond; }
            set
            {
                var old = RequiresBond;
                _requiresBond = value;
                NotifyPropertyChanged(Change.Edit, "RequiresBond", this, value, old);
            }
        }
        public bool RequiresWrapUp
        {
            get { return _requiresWrapUp; }
            set
            {
                var old = RequiresWrapUp;
                _requiresWrapUp = value;
                NotifyPropertyChanged(Change.Edit, "RequiresWrapUp", this, value, old);
            }
        }
        public bool HasBMS
        {
            get { return _hasBMS; }
            set
            {
                var old = HasBMS;
                _hasBMS = value;
                NotifyPropertyChanged(Change.Edit, "HasBMS", this, value, old);
            }
        }
        #endregion

        public TECBidParameters(Guid guid) : base(guid)
        {
            _isTaxExempt = false;
            _requiresBond = false;
            _requiresWrapUp = false;
            _hasBMS = true;

            _escalation = 0;
            _overhead = 0;
            _profit = 0;
            _subcontractorMarkup = 0;
            _subcontractorEscalation = 0;
        }

        public TECBidParameters() : this(Guid.NewGuid()) { }
        public TECBidParameters(TECBidParameters parametersSource) : this()
        {
            _isTaxExempt = parametersSource.IsTaxExempt;
            _requiresBond = parametersSource.RequiresBond;
            _requiresWrapUp = parametersSource.RequiresWrapUp;
            _hasBMS = parametersSource.HasBMS;

            _escalation = parametersSource.Escalation;
            _overhead = parametersSource.Overhead;
            _profit = parametersSource.Profit;
            _subcontractorMarkup = parametersSource.SubcontractorMarkup;
            _subcontractorEscalation = parametersSource.SubcontractorEscalation;
        }

        public override object Copy()
        {
            var outParameters = new TECBidParameters();
            outParameters._guid = _guid;
            outParameters._escalation = _escalation;
            outParameters._overhead = _overhead;
            outParameters._profit = _profit;
            outParameters._subcontractorMarkup = _subcontractorMarkup;
            outParameters._subcontractorEscalation = _subcontractorEscalation;
            outParameters._isTaxExempt = _isTaxExempt;
            outParameters._requiresBond = _requiresBond;
            outParameters._requiresWrapUp = _requiresWrapUp;
            outParameters._hasBMS = _hasBMS;

            return outParameters;
        }
    }
}
