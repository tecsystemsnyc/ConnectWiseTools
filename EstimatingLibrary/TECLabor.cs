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

        private Guid _guid;
        public Guid Guid { get { return _guid; } }

        private int _numPoints;
        public int NumPoints
        {
            get { return _numPoints; }
            set
            {
                _numPoints = value;
                raiseLaborChanged();
            }
        }

        #region PM
        private double _pmCoef;
        public double PMCoef
        {
            get { return _pmCoef; }
            set
            {
                var temp = this.Copy();
                _pmCoef = value;
                NotifyPropertyChanged("PMCoef", temp, this);
                raiseLaborChanged();
            }
        }

        public double PMPointHours
        {
            get
            {
                return (NumPoints * PMCoef);
            }
        }

        private double _pmExtraHours;
        public double PMExtraHours
        {
            get { return _pmExtraHours; }
            set
            {
                var temp = this.Copy();
                _pmExtraHours = value;
                NotifyPropertyChanged("PMExtaHours", temp, this);
                raiseLaborChanged();
            }
        }

        public double PMTotalHours
        {
            get
            {
                return (PMPointHours + PMExtraHours);
            }
        }

        private double _pmRate;
        public double PMRate
        {
            get { return _pmRate; }
            set
            {
                var temp = this.Copy();
                _pmRate = value;
                NotifyPropertyChanged("PMRate", temp, this);
                raiseLaborChanged();
            }
        }

        public double PMSubTotal
        {
            get
            {
                return (PMTotalHours * PMRate);
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
                var temp = this.Copy();
                _engCoef = value;
                NotifyPropertyChanged("ENGCoef", temp, this);
                raiseLaborChanged();
            }
        }

        public double ENGPointHours
        {
            get
            {
                return (NumPoints * ENGCoef);
            }
        }

        private double _engExtraHours;
        public double ENGExtraHours
        {
            get { return _engExtraHours; }
            set
            {
                var temp = this.Copy();
                _engExtraHours = value;
                NotifyPropertyChanged("ENGExtaHours", temp, this);
                raiseLaborChanged();
            }
        }

        public double ENGTotalHours
        {
            get
            {
                return (ENGPointHours + ENGExtraHours);
            }
        }

        private double _engRate;
        public double ENGRate
        {
            get { return _engRate; }
            set
            {
                var temp = this.Copy();
                _engRate = value;
                NotifyPropertyChanged("ENGRate", temp, this);
                raiseLaborChanged();
            }
        }

        public double ENGSubTotal
        {
            get
            {
                return (ENGTotalHours * ENGRate);
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
                var temp = this.Copy();
                _commCoef = value;
                NotifyPropertyChanged("CommCoef", temp, this);
                raiseLaborChanged();
            }
        }

        public double CommPointHours
        {
            get
            {
                return (NumPoints * CommCoef);
            }
        }

        private double _commExtraHours;
        public double CommExtraHours
        {
            get { return _commExtraHours; }
            set
            {
                var temp = this.Copy();
                _commExtraHours = value;
                NotifyPropertyChanged("CommExtaHours", temp, this);
                raiseLaborChanged();
            }
        }

        public double CommTotalHours
        {
            get
            {
                return (CommPointHours + CommExtraHours);
            }
        }

        private double _commRate;
        public double CommRate
        {
            get { return _commRate; }
            set
            {
                var temp = this.Copy();
                _commRate = value;
                NotifyPropertyChanged("CommRate", temp, this);
                raiseLaborChanged();
            }
        }

        public double CommSubTotal
        {
            get
            {
                return (CommTotalHours * CommRate);
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
                var temp = this.Copy();
                _softCoef = value;
                NotifyPropertyChanged("SoftCoef", temp, this);
                raiseLaborChanged();
            }
        }

        public double SoftPointHours
        {
            get
            {
                return (NumPoints * SoftCoef);
            }
        }

        private double _softExtraHours;
        public double SoftExtraHours
        {
            get { return _softExtraHours; }
            set
            {
                var temp = this.Copy();
                _softExtraHours = value;
                NotifyPropertyChanged("SoftExtaHours", temp, this);
                raiseLaborChanged();
            }
        }

        public double SoftTotalHours
        {
            get
            {
                return (SoftPointHours + SoftExtraHours);
            }
        }

        private double _softRate;
        public double SoftRate
        {
            get { return _softRate; }
            set
            {
                var temp = this.Copy();
                _softRate = value;
                NotifyPropertyChanged("SoftRate", temp, this);
                raiseLaborChanged();
            }
        }

        public double SoftSubTotal
        {
            get
            {
                return (SoftTotalHours * SoftRate);
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
                var temp = this.Copy();
                _graphCoef = value;
                NotifyPropertyChanged("GraphCoef", temp, this);
                raiseLaborChanged();
            }
        }

        public double GraphPointHours
        {
            get
            {
                return (NumPoints * GraphCoef);
            }
        }

        private double _graphExtraHours;
        public double GraphExtraHours
        {
            get { return _graphExtraHours; }
            set
            {
                var temp = this.Copy();
                _graphExtraHours = value;
                NotifyPropertyChanged("GraphExtaHours", temp, this);
                raiseLaborChanged();
            }
        }

        public double GraphTotalHours
        {
            get
            {
                return (GraphPointHours + GraphExtraHours);
            }
        }

        private double _graphRate;
        public double GraphRate
        {
            get { return _graphRate; }
            set
            {
                var temp = this.Copy();
                _graphRate = value;
                NotifyPropertyChanged("GraphRate", temp, this);
                raiseLaborChanged();
            }
        }

        public double GraphSubTotal
        {
            get
            {
                return (GraphTotalHours * GraphRate);
            }
        }
        #endregion Graph

        public double TECSubTotal
        {
            get
            {
                return (PMSubTotal + ENGSubTotal + CommSubTotal + SoftSubTotal + GraphSubTotal);
            }
        }

        #region Electrical
        private double _electricalHours;
        public double ElectricalHours
        {
            get { return _electricalHours; }
            set
            {
                _electricalHours = value;
                raiseLaborChanged();
            }
        }

        private double _electricalRate;
        public double ElectricalRate
        {
            get { return _electricalRate; }
            set
            {
                var temp = this.Copy();
                _electricalRate = value;
                NotifyPropertyChanged("ElectricalRate", temp, this);
            }
        }

        private double _electricalSuperRate;
        public double ElectricalSuperRate
        {
            get { return _electricalSuperRate; }
            set
            {
                var temp = this.Copy();
                _electricalSuperRate = value;
                NotifyPropertyChanged("ElectricalSuperRate", temp, this);
            }
        }

        public double ElectricalSubTotal
        {
            get
            {
                return (ElectricalHours * ElectricalRate);
            }
        }
        #endregion Electrical

        public double SubcontractorSubTotal
        {
            get
            {
                return (ElectricalSubTotal);
            }
        }

        public double TotalCost
        {
            get
            {
                return (TECSubTotal + SubcontractorSubTotal);
            }
        }

        #endregion

        #region Initializers
        public TECLabor() : this(Guid.NewGuid()) { }

        public TECLabor(Guid guid)
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

            _electricalHours = 0;
            _electricalRate = 0;
            _electricalSuperRate = 0;
        }

        public TECLabor(TECLabor labor)
        {
            _guid = labor._guid;

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

            _electricalHours = labor.ElectricalHours;
            _electricalRate = labor.ElectricalRate;
        }
        #endregion

        #region Methods
        private void raiseLaborChanged()
        {
            RaisePropertyChanged("NumPoints");

            RaisePropertyChanged("PMCoef");
            RaisePropertyChanged("PMPointHours");
            RaisePropertyChanged("PMExtraHours");
            RaisePropertyChanged("PMTotalHours");
            RaisePropertyChanged("PMRate");
            RaisePropertyChanged("PMSubTotal");

            RaisePropertyChanged("ENGCoef");
            RaisePropertyChanged("ENGPointHours");
            RaisePropertyChanged("ENGExtraHours");
            RaisePropertyChanged("ENGTotalHours");
            RaisePropertyChanged("ENGRate");
            RaisePropertyChanged("ENGSubTotal");

            RaisePropertyChanged("SoftCoef");
            RaisePropertyChanged("SoftPointHours");
            RaisePropertyChanged("SoftExtraHours");
            RaisePropertyChanged("SoftTotalHours");
            RaisePropertyChanged("SoftRate");
            RaisePropertyChanged("SoftSubTotal");

            RaisePropertyChanged("GraphCoef");
            RaisePropertyChanged("GraphPointHours");
            RaisePropertyChanged("GraphExtraHours");
            RaisePropertyChanged("GraphTotalHours");
            RaisePropertyChanged("GraphRate");
            RaisePropertyChanged("GraphSubTotal");

            RaisePropertyChanged("CommCoef");
            RaisePropertyChanged("CommPointHours");
            RaisePropertyChanged("CommExtraHours");
            RaisePropertyChanged("CommTotalHours");
            RaisePropertyChanged("CommRate");
            RaisePropertyChanged("CommSubTotal");

            RaisePropertyChanged("TECSubTotal");

            RaisePropertyChanged("ElectricalHours");
            RaisePropertyChanged("ElectricalRate");
            RaisePropertyChanged("ElectricalSubTotal");

            RaisePropertyChanged("SubcontractorSubTotal");

            RaisePropertyChanged("TotalCost");
        }

        public override object Copy()
        {
            TECLabor outLabor = new TECLabor(this);
            return outLabor;
        }

        #endregion
    }
}
