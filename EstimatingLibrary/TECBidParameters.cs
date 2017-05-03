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

        private Guid _guid;

        public Guid Guid
        {
            get { return _guid; }
        }

        public double Escalation
        {
            get { return _escalation; }
            set
            {
                var temp = this.Copy();
                _escalation = value;
                NotifyPropertyChanged("Escalation", temp, this);
            }
        }
        public double Overhead
        {
            get { return _overhead; }
            set
            {
                var temp = this.Copy();
                _overhead = value;
                NotifyPropertyChanged("Overhead", temp, this);
            }
        }
        public double Profit
        {
            get { return _profit; }
            set
            {
                var temp = this.Copy();
                _profit = value;
                NotifyPropertyChanged("Profit", temp, this);
            }
        }
        public double SubcontractorMarkup
        {
            get { return _subcontractorMarkup; }
            set
            {
                var temp = this.Copy();
                _subcontractorMarkup = value;
                NotifyPropertyChanged("SubcontractorMarkup", temp, this);
            }
        }
        public double SubcontractorEscalation
        {
            get { return _subcontractorEscalation; }
            set
            {
                var temp = this.Copy();
                _subcontractorEscalation = value;
                NotifyPropertyChanged("SubcontractorEscalation", temp, this);
            }
        }

        public bool IsTaxExempt
        {
            get { return _isTaxExempt; }
            set
            {
                var temp = this.Copy();
                _isTaxExempt = value;
                NotifyPropertyChanged("IsTaxExempt", temp, this);
            }
        }
        public bool RequiresBond
        {
            get { return _requiresBond; }
            set
            {
                var temp = this.Copy();
                _requiresBond = value;
                NotifyPropertyChanged("RequiresBond", temp, this);
            }
        }
        public bool RequiresWrapUp
        {
            get { return _requiresWrapUp; }
            set
            {
                var temp = this.Copy();
                _requiresWrapUp = value;
                NotifyPropertyChanged("RequiresWrapUp", temp, this);
            }
        }
        public bool HasBMS
        {
            get { return _hasBMS; }
            set
            {
                var temp = this.Copy();
                _hasBMS = value;
                NotifyPropertyChanged("HasBMS", temp, this);
            }
        }
        #endregion

        public TECBidParameters(Guid guid)
        {
            _guid = guid;
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
