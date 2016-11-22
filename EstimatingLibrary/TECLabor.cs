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
        #region TEC Labor
        private double _pmCoef;
        public double PMCoef
        {
            get { return _pmCoef; }
            set
            {
                var temp = this.Copy();
                _pmCoef = value;
                NotifyPropertyChanged("PMCoef", temp, this);
            }
        }

        private double _engCoef;
        public double ENGCoef
        {
            get { return _engCoef; }
            set
            {
                var temp = this.Copy();
                _engCoef = value;
                NotifyPropertyChanged("ENGCoef", temp, this);
            }
        }

        private double _commCoef;
        public double CommCoef
        {
            get { return _commCoef; }
            set
            {
                var temp = this.Copy();
                _commCoef = value;
                NotifyPropertyChanged("CommCoef", temp, this);
            }
        }

        private double _softCoef;
        public double SoftCoef
        {
            get { return _softCoef; }
            set
            {
                var temp = this.Copy();
                _softCoef = value;
                NotifyPropertyChanged("SoftCoef", temp, this);
            }
        }

        private double _graphCoef;
        public double GraphCoef
        {
            get { return _graphCoef; }
            set
            {
                var temp = this.Copy();
                _graphCoef = value;
                NotifyPropertyChanged("GraphCoef", temp, this);
            }
        }
        #endregion //TEC Labor

        #region Subcontractor Labor
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
        #endregion //Subcontractor Labor
        #endregion

        #region Initializers
        public TECLabor()
        {
            _pmCoef = 1.0;
            _engCoef = 1.0;
            _commCoef = 1.0;
            _softCoef = 1.0;
            _graphCoef = 1.0;

            _electricalRate = 1.0;
        }

        public TECLabor(TECLabor labor)
        {
            _pmCoef = labor.PMCoef;
            _engCoef = labor.ENGCoef;
            _commCoef = labor.CommCoef;
            _softCoef = labor.SoftCoef;
            _graphCoef = labor.GraphCoef;

            _electricalRate = labor.ElectricalRate;
        }
        #endregion

        #region Methods
        public override object Copy()
        {
            TECLabor outLabor = new TECLabor(this);
            return outLabor;
        }

        #endregion
    }
}
