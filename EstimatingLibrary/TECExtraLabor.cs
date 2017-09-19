using EstimatingLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstimatingLibrary.Utilities;

namespace EstimatingLibrary
{
    public class TECExtraLabor : TECObject, INotifyCostChanged
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
                notifyCombinedChanged(Change.Edit, "PMExtraHours", this, value, old);
                CostChanged?.Invoke(new CostBatch());
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
                notifyCombinedChanged(Change.Edit, "ENGExtraHours", this, value, old);
                CostChanged?.Invoke(new CostBatch());
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
                notifyCombinedChanged(Change.Edit, "CommExtraHours", this, value, old);
                CostChanged?.Invoke(new CostBatch());

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
                notifyCombinedChanged(Change.Edit, "SoftExtraHours", this, value, old);
                CostChanged?.Invoke(new CostBatch());
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
                notifyCombinedChanged(Change.Edit, "GraphExtraHours", this, value, old);
                CostChanged?.Invoke(new CostBatch());
            }
        }


        #endregion Graph
        public event Action<CostBatch> CostChanged;

        public CostBatch CostBatch
        {
            get { return getCosts(); }
        }
        private CostBatch getCosts()
        {
            double totalhours = PMExtraHours +
                ENGExtraHours +
                CommExtraHours +
                SoftExtraHours +
                GraphExtraHours;
            return new CostBatch(0, totalhours, CostType.TEC);
        }

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
