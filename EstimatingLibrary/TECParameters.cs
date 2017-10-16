using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECParameters : TECObject
    {
        #region Properties
        private double _escalation;
        private double _overhead;
        private double _profit;
        private double _subcontractorMarkup;
        private double _subcontractorEscalation;
        private double _warranty;
        private double _shipping;
        private double _tax;
        private double _subcontractorWarranty;
        private double _subcontractorShipping;
        private double _bondRate;

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
                notifyCombinedChanged(Change.Edit, "Escalation", this, value, old);
            }
        }
        public double Overhead
        {
            get { return _overhead; }
            set
            {
                var old = Overhead;
                _overhead = value;
                notifyCombinedChanged(Change.Edit, "Overhead", this, value, old);
            }
        }
        public double Profit
        {
            get { return _profit; }
            set
            {
                var old = Profit;
                _profit = value;
                notifyCombinedChanged(Change.Edit, "Profit", this, value, old);
            }
        }
        public double SubcontractorMarkup
        {
            get { return _subcontractorMarkup; }
            set
            {
                var old = SubcontractorMarkup;
                _subcontractorMarkup = value;
                notifyCombinedChanged(Change.Edit, "SubcontractorMarkup", this, value, old);
            }
        }
        public double SubcontractorEscalation
        {
            get { return _subcontractorEscalation; }
            set
            {
                var old = SubcontractorEscalation;
                _subcontractorEscalation = value;
                notifyCombinedChanged(Change.Edit, "SubcontractorEscalation", this, value, old);
            }
        }
        public double Warranty
        {
            get { return _warranty; }
            set
            {
                var old = Warranty;
                _warranty = value;
                notifyCombinedChanged(Change.Edit, "Warranty", this, value, old);
            }
        }
        public double Shipping
        {
            get { return _shipping; }
            set
            {
                var old = Shipping;
                _shipping = value;
                notifyCombinedChanged(Change.Edit, "Shipping", this, value, old);
            }
        }
        public double Tax
        {
            get { return _tax; }
            set
            {
                var old = Tax;
                _tax = value;
                notifyCombinedChanged(Change.Edit, "Tax", this, value, old);
            }
        }
        public double SubcontractorWarranty
        {
            get { return _subcontractorWarranty; }
            set
            {
                var old = SubcontractorWarranty;
                _subcontractorWarranty = value;
                notifyCombinedChanged(Change.Edit, "SubcontractorWarranty", this, value, old);
            }
        }
        public double SubcontractorShipping
        {
            get { return _subcontractorShipping; }
            set
            {
                var old = Shipping;
                _subcontractorShipping = value;
                notifyCombinedChanged(Change.Edit, "SubcontractorShipping", this, value, old);
            }
        }
        public double BondRate
        {
            get { return _bondRate; }
            set
            {
                var old = BondRate;
                _bondRate = value;
                notifyCombinedChanged(Change.Edit, "BondRate", this, value, old);
            }
        }

        public bool IsTaxExempt
        {
            get { return _isTaxExempt; }
            set
            {
                var old = IsTaxExempt;
                _isTaxExempt = value;
                notifyCombinedChanged(Change.Edit, "IsTaxExempt", this, value, old);
            }
        }
        public bool RequiresBond
        {
            get { return _requiresBond; }
            set
            {
                var old = RequiresBond;
                _requiresBond = value;
                notifyCombinedChanged(Change.Edit, "RequiresBond", this, value, old);
            }
        }
        public bool RequiresWrapUp
        {
            get { return _requiresWrapUp; }
            set
            {
                var old = RequiresWrapUp;
                _requiresWrapUp = value;
                notifyCombinedChanged(Change.Edit, "RequiresWrapUp", this, value, old);
            }
        }
        public bool HasBMS
        {
            get { return _hasBMS; }
            set
            {
                var old = HasBMS;
                _hasBMS = value;
                notifyCombinedChanged(Change.Edit, "HasBMS", this, value, old);
            }
        }
        #endregion
        #region Labor
        #region PM
        private double _pmCoef;
        public double PMCoef
        {
            get { return _pmCoef; }
            set
            {
                var old = PMCoef;
                _pmCoef = value;
                notifyCombinedChanged(Change.Edit, "PMCoef", this, value, old);
            }
        }

        private double _pmRate;
        public double PMRate
        {
            get { return _pmRate; }
            set
            {
                var old = PMRate;
                _pmRate = value;
                notifyCombinedChanged(Change.Edit, "PMRate", this, value, old);

            }
        }

        #endregion PM

        #region ENG
        private double _engCoef;
        public double ENGCoef
        {
            get { return _engCoef; }
            set
            {
                var old = ENGCoef;
                _engCoef = value;
                notifyCombinedChanged(Change.Edit, "ENGCoef", this, value, old);


            }
        }

        private double _engRate;
        public double ENGRate
        {
            get { return _engRate; }
            set
            {

                var old = ENGRate;
                _engRate = value;
                notifyCombinedChanged(Change.Edit, "ENGRate", this, value, old);


            }
        }
        #endregion ENG

        #region Comm
        private double _commCoef;
        public double CommCoef
        {
            get { return _commCoef; }
            set
            {

                var old = CommCoef;
                _commCoef = value;
                notifyCombinedChanged(Change.Edit, "CommCoef", this, value, old);


            }
        }

        private double _commRate;
        public double CommRate
        {
            get { return _commRate; }
            set
            {

                var old = CommRate;
                _commRate = value;
                notifyCombinedChanged(Change.Edit, "CommRate", this, value, old);


            }
        }
        #endregion Comm

        #region Soft
        private double _softCoef;
        public double SoftCoef
        {
            get { return _softCoef; }
            set
            {

                var old = SoftCoef;
                _softCoef = value;
                notifyCombinedChanged(Change.Edit, "SoftCoef", this, value, old);


            }
        }
        
        private double _softRate;
        public double SoftRate
        {
            get { return _softRate; }
            set
            {
                var old = SoftRate;
                _softRate = value;
                notifyCombinedChanged(Change.Edit, "SoftRate", this, value, old);

            }
        }

        #endregion Soft

        #region Graph
        private double _graphCoef;
        public double GraphCoef
        {
            get { return _graphCoef; }
            set
            {
                var old = GraphCoef;
                _graphCoef = value;
                notifyCombinedChanged(Change.Edit, "GraphCoef", this, value, old);

            }
        }
        
        private double _graphRate;
        public double GraphRate
        {
            get { return _graphRate; }
            set
            {
                var old = GraphRate;
                _graphRate = value;
                notifyCombinedChanged(Change.Edit, "GraphRate", this, value, old);

            }
        }
        #endregion Graph

        #region Electrical
        private double _electricalRate;
        public double ElectricalRate
        {
            get { return _electricalRate; }
            set
            {
                var old = ElectricalRate;
                _electricalRate = value;
                notifyCombinedChanged(Change.Edit, "ElectricalRate", this, value, old);
                raisePropertyChanged("ElectricalEffectiveRate");
            }
        }

        private double _electricalNonUnionRate;
        public double ElectricalNonUnionRate
        {
            get { return _electricalNonUnionRate; }
            set
            {
                var old = ElectricalNonUnionRate;
                _electricalNonUnionRate = value;
                notifyCombinedChanged(Change.Edit, "ElectricalNonUnionRate", this, value, old);
                raisePropertyChanged("ElectricalEffectiveRate");
            }
        }

        public double ElectricalEffectiveRate
        {
            get
            {
                double rate;
                if (ElectricalIsUnion)
                {
                    rate = ElectricalRate;
                }
                else
                {
                    rate = ElectricalNonUnionRate;
                }

                if (ElectricalIsOnOvertime)
                {
                    return (rate * 1.5);
                }
                else
                {
                    return rate;
                }
            }
        }

        private double _electricalSuperRate;
        public double ElectricalSuperRate
        {
            get { return _electricalSuperRate; }
            set
            {
                var old = ElectricalSuperRate;
                _electricalSuperRate = value;
                notifyCombinedChanged(Change.Edit, "ElectricalSuperRate", this, value, old);
                raisePropertyChanged("ElectricalSuperEffectiveRate");
            }
        }

        private double _electricalSuperNonUnionRate;
        public double ElectricalSuperNonUnionRate
        {
            get { return _electricalSuperNonUnionRate; }
            set
            {
                var old = ElectricalSuperNonUnionRate;
                _electricalSuperNonUnionRate = value;
                notifyCombinedChanged(Change.Edit, "ElectricalSuperNonUnionRate", this, value, old);
                raisePropertyChanged("ElectricalSuperEffectiveRate");
            }
        }

        public double ElectricalSuperEffectiveRate
        {
            get
            {
                double rate;
                if (ElectricalIsUnion)
                {
                    rate = ElectricalSuperRate;
                }
                else
                {
                    rate = ElectricalSuperNonUnionRate;
                }

                if (ElectricalIsOnOvertime)
                {
                    return (rate * 1.5);
                }
                else
                {
                    return rate;
                }
            }
        }

        private bool _electricalIsOnOvertime;
        public bool ElectricalIsOnOvertime
        {
            get { return _electricalIsOnOvertime; }
            set
            {
                var old = ElectricalIsOnOvertime;
                _electricalIsOnOvertime = value;
                notifyCombinedChanged(Change.Edit, "ElectricalIsOnOvertime", this, value, old);
                raiseEffectiveRateChanged();
            }
        }

        private bool _electricalIsUnion;
        public bool ElectricalIsUnion
        {
            get { return _electricalIsUnion; }
            set
            {
                var old = ElectricalIsUnion;
                _electricalIsUnion = value;
                notifyCombinedChanged(Change.Edit, "ElectricalIsUnion", this, value, old);
                raiseEffectiveRateChanged();
            }
        }

        private double _electricalSuperRatio;
        public double ElectricalSuperRatio
        {
            get { return _electricalSuperRatio; }
            set
            {
                var old = ElectricalSuperRatio;
                _electricalSuperRatio = value;
                notifyCombinedChanged(Change.Edit, "ElectricalSuperRatio", this, value, old);
            }
        }

        #endregion Electrical

        #endregion
        public TECParameters(Guid guid) : base(guid)
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
            _warranty = 0.05;
            _shipping = 0.03;
            _tax = 0.0875;

            _pmCoef = 1.0;
            _pmRate = 0;

            _engCoef = 1.0;
            _engRate = 0;

            _commCoef = 1.0;
            _commRate = 0;

            _softCoef = 1.0;
            _softRate = 0;

            _graphCoef = 1.0;
            _graphRate = 0;

            _electricalRate = 0;
            _electricalNonUnionRate = 0;
            _electricalSuperRate = 0;
            _electricalSuperNonUnionRate = 0;

            _electricalIsOnOvertime = false;
            _electricalIsUnion = true;
        }

        public TECParameters(TECParameters parametersSource) : this(parametersSource.Guid)
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

            _pmCoef = parametersSource.PMCoef;
            _pmRate = parametersSource.PMRate;

            _engCoef = parametersSource.ENGCoef;
            _engRate = parametersSource.ENGRate;

            _commCoef = parametersSource.CommCoef;
            _commRate = parametersSource.CommRate;

            _softCoef = parametersSource.SoftCoef;
            _softRate = parametersSource.SoftRate;

            _graphCoef = parametersSource.GraphCoef;
            _graphRate = parametersSource.GraphRate;

            _electricalRate = parametersSource.ElectricalRate;
            _electricalNonUnionRate = parametersSource.ElectricalNonUnionRate;
            _electricalSuperRate = parametersSource.ElectricalSuperRate;
            _electricalSuperNonUnionRate = parametersSource.ElectricalSuperNonUnionRate;
            _electricalSuperRatio = parametersSource.ElectricalSuperRatio;

            _electricalIsOnOvertime = parametersSource.ElectricalIsOnOvertime;
            _electricalIsUnion = parametersSource.ElectricalIsUnion;
        }

        public void UpdateConstants(TECParameters labor)
        {
            PMCoef = labor.PMCoef;
            PMRate = labor.PMRate;

            ENGCoef = labor.ENGCoef;
            ENGRate = labor.ENGRate;

            SoftCoef = labor.SoftCoef;
            SoftRate = labor.SoftRate;

            GraphCoef = labor.GraphCoef;
            GraphRate = labor.GraphRate;

            CommCoef = labor.CommCoef;
            CommRate = labor.CommRate;

            ElectricalRate = labor.ElectricalRate;
            ElectricalNonUnionRate = labor.ElectricalNonUnionRate;
            ElectricalSuperRate = labor.ElectricalSuperRate;
            ElectricalSuperNonUnionRate = labor.ElectricalSuperNonUnionRate;

            ElectricalSuperRatio = labor.ElectricalSuperRatio;
        }

        private void raiseEffectiveRateChanged()
        {
            raisePropertyChanged("ElectricalEffectiveRate");
            raisePropertyChanged("ElectricalSuperEffectiveRate");
        }
    }
}
