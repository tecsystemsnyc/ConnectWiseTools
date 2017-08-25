using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECExtraLabor : TECObject
    {
        #region Labor
        #region PM
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

        #endregion PM

        #region ENG
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
        #endregion ENG

        #region Comm
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
        #endregion Comm

        #region Soft
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

        #endregion Soft

        #region Graph
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
        #endregion Graph
        

        #endregion
        public TECExtraLabor(Guid guid) : base(guid)
        {
            _pmExtraHours = 0;
            _engExtraHours = 0;
            _commExtraHours = 0;
            _softExtraHours = 0;
            _graphExtraHours = 0;
        }
        
    }
}
