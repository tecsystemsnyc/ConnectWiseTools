using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECLabor : TECObject
    {
        #region Properties
        #region PM
        private double _pmCoef;
        public double PMCoef
        {
            get { return _pmCoef; }
            set
            {
                var old = PMCoef;
                _pmCoef = value;
                NotifyPropertyChanged(Change.Edit, "PMCoef", this, value, old);
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
                NotifyPropertyChanged(Change.Edit, "PMExtraHours", this, value, old);

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
                NotifyPropertyChanged(Change.Edit, "PMRate", this, value, old);

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
                NotifyPropertyChanged(Change.Edit, "ENGCoef", this, value, old);


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
                NotifyPropertyChanged(Change.Edit, "ENGExtraHours", this, value, old);

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
                NotifyPropertyChanged(Change.Edit, "ENGRate", this, value, old);


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
                NotifyPropertyChanged(Change.Edit, "CommCoef", this, value, old);


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
                NotifyPropertyChanged(Change.Edit, "CommExtraHours", this, value, old);


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
                NotifyPropertyChanged(Change.Edit, "CommRate", this, value, old);


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
                NotifyPropertyChanged(Change.Edit, "SoftCoef", this, value, old);


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
                NotifyPropertyChanged(Change.Edit, "SoftExtraHours", this, value, old);

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
                NotifyPropertyChanged(Change.Edit, "SoftRate", this, value, old);

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
                NotifyPropertyChanged(Change.Edit, "GraphCoef", this, value, old);

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
                NotifyPropertyChanged(Change.Edit, "GraphExtraHours", this, value, old);

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
                NotifyPropertyChanged(Change.Edit, "GraphRate", this, value, old);

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
                NotifyPropertyChanged(Change.Edit, "ElectricalRate", this, value, old);
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
                NotifyPropertyChanged(Change.Edit, "ElectricalNonUnionRate", this, value, old);
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
                NotifyPropertyChanged(Change.Edit, "ElectricalSuperRate", this, value, old);
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
                NotifyPropertyChanged(Change.Edit, "ElectricalSuperNonUnionRate", this, value, old);
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
                NotifyPropertyChanged(Change.Edit, "ElectricalIsOnOvertime", this, value, old);
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
                NotifyPropertyChanged(Change.Edit, "ElectricalIsUnion", this, value, old);
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
                NotifyPropertyChanged(Change.Edit, "ElectricalSuperRatio", this, value, old);
            }
        }

        #endregion Electrical

        #endregion

        #region Initializers
        public TECLabor() : this(Guid.NewGuid()) { }

        public TECLabor(Guid guid) : base(guid)
        {
            _guid = guid;

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

        public TECLabor(TECLabor labor) : this()
        {
            _pmCoef = labor.PMCoef;
            _pmExtraHours = labor.PMExtraHours;
            _pmRate = labor.PMRate;

            _engCoef = labor.ENGCoef;
            _engExtraHours = labor.ENGExtraHours;
            _engRate = labor.ENGRate;

            _commCoef = labor.CommCoef;
            _commExtraHours = labor.CommExtraHours;
            _commRate = labor.CommRate;

            _softCoef = labor.SoftCoef;
            _softExtraHours = labor.SoftExtraHours;
            _softRate = labor.SoftRate;

            _graphCoef = labor.GraphCoef;
            _graphExtraHours = labor.GraphExtraHours;
            _graphRate = labor.GraphRate;

            _electricalRate = labor.ElectricalRate;
            _electricalNonUnionRate = labor.ElectricalNonUnionRate;
            _electricalSuperRate = labor.ElectricalSuperRate;
            _electricalSuperNonUnionRate = labor.ElectricalSuperNonUnionRate;
            _electricalSuperRatio = labor.ElectricalSuperRatio;

            _electricalIsOnOvertime = labor.ElectricalIsOnOvertime;
            _electricalIsUnion = labor.ElectricalIsUnion;
        }
        #endregion

        #region Methods
        public void UpdateConstants(TECLabor labor)
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

        #endregion
    }
}
