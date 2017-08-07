using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.ViewModels
{
    public class MaterialSummaryVM : ViewModelBase
    {
        //Constructor
        public MaterialSummaryVM(TECBid bid, ChangeWatcher changeWatcher)
        {
            initializeVMs();
            loadBid(bid);
            resubscribe(changeWatcher);
        }

        #region Properties
        public HardwareSummaryVM DeviceSummaryVM { get; private set; }
        public HardwareSummaryVM ControllerSummaryVM { get; private set; }
        public HardwareSummaryVM PanelSummaryVM { get; private set; }
        public LengthSummaryVM WireSummaryVM { get; private set; }
        public LengthSummaryVM ConduitSummaryVM { get; private set; }
        public MiscCostsSummaryVM MiscSummaryVM { get; private set; }
        #endregion

        #region Methods
        public void Refresh(TECBid bid, ChangeWatcher changeWatcher)
        {
            resetVMs();
            loadBid(bid);
            resubscribe(changeWatcher);
        }

        #region Initialization Methods
        private void loadBid(TECBid bid)
        {

        }

        private void resubscribe(ChangeWatcher changeWatcher)
        {
            changeWatcher.InstanceChanged -= instanceChanged;
            changeWatcher.InstanceChanged += instanceChanged;
        }

        private void initializeVMs()
        {
            DeviceSummaryVM = new HardwareSummaryVM(typeof(TECDevice));
            ControllerSummaryVM = new HardwareSummaryVM(typeof(TECController));
            PanelSummaryVM = new HardwareSummaryVM(typeof(TECPanel));

        }

        private void resetVMs()
        {

        }
        #endregion

        private void instanceChanged(PropertyChangedExtendedEventArgs obj)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
