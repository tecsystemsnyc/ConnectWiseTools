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
                NotifyCombinedChanged(Change.Edit, "Escalation", this, value, old);
            }
        }
        public double Overhead
        {
            get { return _overhead; }
            set
            {
                var old = Overhead;
                _overhead = value;
                NotifyCombinedChanged(Change.Edit, "Overhead", this, value, old);
            }
        }
        public double Profit
        {
            get { return _profit; }
            set
            {
                var old = Profit;
                _profit = value;
                NotifyCombinedChanged(Change.Edit, "Profit", this, value, old);
            }
        }
        public double SubcontractorMarkup
        {
            get { return _subcontractorMarkup; }
            set
            {
                var old = SubcontractorMarkup;
                _subcontractorMarkup = value;
                NotifyCombinedChanged(Change.Edit, "SubcontractorMarkup", this, value, old);
            }
        }
        public double SubcontractorEscalation
        {
            get { return _subcontractorEscalation; }
            set
            {
                var old = SubcontractorEscalation;
                _subcontractorEscalation = value;
                NotifyCombinedChanged(Change.Edit, "SubcontractorEscalation", this, value, old);
            }
        }

        public bool IsTaxExempt
        {
            get { return _isTaxExempt; }
            set
            {
                var old = IsTaxExempt;
                _isTaxExempt = value;
                NotifyCombinedChanged(Change.Edit, "IsTaxExempt", this, value, old);
            }
        }
        public bool RequiresBond
        {
            get { return _requiresBond; }
            set
            {
                var old = RequiresBond;
                _requiresBond = value;
                NotifyCombinedChanged(Change.Edit, "RequiresBond", this, value, old);
            }
        }
        public bool RequiresWrapUp
        {
            get { return _requiresWrapUp; }
            set
            {
                var old = RequiresWrapUp;
                _requiresWrapUp = value;
                NotifyCombinedChanged(Change.Edit, "RequiresWrapUp", this, value, old);
            }
        }
        public bool HasBMS
        {
            get { return _hasBMS; }
            set
            {
                var old = HasBMS;
                _hasBMS = value;
                NotifyCombinedChanged(Change.Edit, "HasBMS", this, value, old);
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
                NotifyCombinedChanged(Change.Edit, "PMCoef", this, value, old);
            }
        }

        private double _pmExtraHours;
        public double PMExtraHours
        {
            get { return _pmExtraHours; }
            set
            {
                var old = PMExtraHours;
                _pmExtraHours = value;
                NotifyCombinedChanged(Change.Edit, "PMExtraHours", this, value, old);

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
                NotifyCombinedChanged(Change.Edit, "PMRate", this, value, old);

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
                NotifyCombinedChanged(Change.Edit, "ENGCoef", this, value, old);


            }
        }

        private double _engExtraHours;
        public double ENGExtraHours
        {
            get { return _engExtraHours; }
            set
            {

                var old = ENGExtraHours;
                _engExtraHours = value;
                NotifyCombinedChanged(Change.Edit, "ENGExtraHours", this, value, old);

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
                NotifyCombinedChanged(Change.Edit, "ENGRate", this, value, old);


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
                NotifyCombinedChanged(Change.Edit, "CommCoef", this, value, old);


            }
        }

        private double _commExtraHours;
        public double CommExtraHours
        {
            get { return _commExtraHours; }
            set
            {

                var old = CommExtraHours;
                _commExtraHours = value;
                NotifyCombinedChanged(Change.Edit, "CommExtraHours", this, value, old);


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
                NotifyCombinedChanged(Change.Edit, "CommRate", this, value, old);


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
                NotifyCombinedChanged(Change.Edit, "SoftCoef", this, value, old);


            }
        }

        private double _softExtraHours;
        public double SoftExtraHours
        {
            get { return _softExtraHours; }
            set
            {
                var old = SoftExtraHours;
                _softExtraHours = value;
                NotifyCombinedChanged(Change.Edit, "SoftExtraHours", this, value, old);

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
                NotifyCombinedChanged(Change.Edit, "SoftRate", this, value, old);

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
                NotifyCombinedChanged(Change.Edit, "GraphCoef", this, value, old);

            }
        }

        private double _graphExtraHours;
        public double GraphExtraHours
        {
            get { return _graphExtraHours; }
            set
            {
                var old = GraphExtraHours;
                _graphExtraHours = value;
                NotifyCombinedChanged(Change.Edit, "GraphExtraHours", this, value, old);

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
                NotifyCombinedChanged(Change.Edit, "GraphRate", this, value, old);

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
                NotifyCombinedChanged(Change.Edit, "ElectricalRate", this, value, old);
                RaisePropertyChanged("ElectricalEffectiveRate");
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
                NotifyCombinedChanged(Change.Edit, "ElectricalNonUnionRate", this, value, old);
                RaisePropertyChanged("ElectricalEffectiveRate");
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
                NotifyCombinedChanged(Change.Edit, "ElectricalSuperRate", this, value, old);
                RaisePropertyChanged("ElectricalSuperEffectiveRate");
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
                NotifyCombinedChanged(Change.Edit, "ElectricalSuperNonUnionRate", this, value, old);
                RaisePropertyChanged("ElectricalSuperEffectiveRate");
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
                NotifyCombinedChanged(Change.Edit, "ElectricalIsOnOvertime", this, value, old);
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
                NotifyCombinedChanged(Change.Edit, "ElectricalIsUnion", this, value, old);
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
                NotifyCombinedChanged(Change.Edit, "ElectricalSuperRatio", this, value, old);
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

            _pmCoef = 1.0;
            _pmExtraHours = 0;
            _pmRate = 0;

            _engCoef = 1.0;
            _engExtraHours = 0;
            _engRate = 0;

            _commCoef = 1.0;
            _commExtraHours = 0;
            _commRate = 0;

            _softCoef = 1.0;
            _softExtraHours = 0;
            _softRate = 0;

            _graphCoef = 1.0;
            _graphExtraHours = 0;
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
            _pmExtraHours = parametersSource.PMExtraHours;
            _pmRate = parametersSource.PMRate;

            _engCoef = parametersSource.ENGCoef;
            _engExtraHours = parametersSource.ENGExtraHours;
            _engRate = parametersSource.ENGRate;

            _commCoef = parametersSource.CommCoef;
            _commExtraHours = parametersSource.CommExtraHours;
            _commRate = parametersSource.CommRate;

            _softCoef = parametersSource.SoftCoef;
            _softExtraHours = parametersSource.SoftExtraHours;
            _softRate = parametersSource.SoftRate;

            _graphCoef = parametersSource.GraphCoef;
            _graphExtraHours = parametersSource.GraphExtraHours;
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
            RaisePropertyChanged("ElectricalEffectiveRate");
            RaisePropertyChanged("ElectricalSuperEffectiveRate");
        }
    }
}
