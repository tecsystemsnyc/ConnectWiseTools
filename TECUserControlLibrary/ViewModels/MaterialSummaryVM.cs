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
        #region Fields
        private double _totalTECCost;
        private double _totalTECLabor;
        private double _totalElecCost;
        private double _totalElecLabor;
        #endregion

        //Constructor
        public MaterialSummaryVM(TECBid bid, ChangeWatcher changeWatcher)
        {
            reinitializeTotals();
            initializeVMs();
            loadBid(bid);
            resubscribe(changeWatcher);
        }

        #region Properties
        public double TotalTECCost
        {
            get { return _totalTECCost; }
            private set
            {
                _totalTECCost = value;
                RaisePropertyChanged("TotalTECCost");
            }
        }
        public double TotalTECLabor
        {
            get { return _totalTECLabor; }
            private set
            {
                _totalTECCost = value;
                RaisePropertyChanged("TotalTECLabor");
            }
        }
        public double TotalElecCost
        {
            get { return _totalElecCost; }
            set
            {
                _totalElecCost = value;
                RaisePropertyChanged("TotalElecCost");
            }
        }
        public double TotalElecLabor
        {
            get { return _totalElecLabor; }
            set
            {
                _totalElecLabor = value;
                RaisePropertyChanged("TotalElecLabor");
            }
        }

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
            reinitializeTotals();
            resetVMs();
            loadBid(bid);
            resubscribe(changeWatcher);
        }

        #region Initialization Methods
        private void reinitializeTotals()
        {
            TotalTECCost = 0;
            TotalTECLabor = 0;
            TotalElecCost = 0;
            TotalElecLabor = 0;
        }

        private void loadBid(TECBid bid)
        {
            foreach (TECSystem typical in bid.Systems)
            {
                foreach (TECSystem instance in typical.SystemInstances)
                {
                    addSystem(instance);
                }
            }
            foreach(TECController controller in bid.Controllers)
            {
                addController(controller);
            }
            foreach(TECPanel panel in bid.Panels)
            {
                addPanel(panel);
            }
            foreach(TECMisc misc in bid.MiscCosts)
            {
                MiscSummaryVM.AddCost(misc);
            }
        }

        private void resubscribe(ChangeWatcher changeWatcher)
        {
            changeWatcher.InstanceChanged -= instanceChanged;
            changeWatcher.InstanceChanged += instanceChanged;
        }

        private void initializeVMs()
        {
            DeviceSummaryVM = new HardwareSummaryVM();
            ControllerSummaryVM = new HardwareSummaryVM();
            PanelSummaryVM = new HardwareSummaryVM();
            WireSummaryVM = new LengthSummaryVM();
            ConduitSummaryVM = new LengthSummaryVM();
            MiscSummaryVM = new MiscCostsSummaryVM();
        }

        private void resetVMs()
        {
            DeviceSummaryVM.Reset();
            ControllerSummaryVM.Reset();
            PanelSummaryVM.Reset();
            WireSummaryVM.Reset();
            ConduitSummaryVM.Reset();
            MiscSummaryVM.Reset();
        }
        #endregion

        #region Add/Remove Methods
        private List<CostObject> addSystem(TECSystem system)
        {
            List<CostObject> deltas = new List<CostObject>();
            foreach(TECEquipment equip in system.Equipment)
            {
                deltas.AddRange(addEquipment(equip));
            }
            foreach(TECMisc misc in system.MiscCosts)
            {
                deltas.AddRange(MiscSummaryVM.AddCost(misc));
            }
            foreach(TECCost cost in system.AssociatedCosts)
            {
                deltas.AddRange(MiscSummaryVM.AddCost(cost));
            }
            return deltas;
        }
        private List<CostObject> addEquipment(TECEquipment equip)
        {
            List<CostObject> deltas = new List<CostObject>();
            foreach (TECSubScope ss in equip.SubScope)
            {
                deltas.AddRange(addSubScope(ss));
            }
            foreach(TECCost cost in equip.AssociatedCosts)
            {
                deltas.AddRange(MiscSummaryVM.AddCost(cost));
            }
            return deltas;
        }
        private List<CostObject> addSubScope(TECSubScope ss)
        {
            List<CostObject> deltas = new List<CostObject>();
            foreach (TECDevice dev in ss.Devices)
            {
                deltas.AddRange(DeviceSummaryVM.AddHardware(dev));
            }
            foreach(TECCost cost in ss.AssociatedCosts)
            {
                deltas.AddRange(MiscSummaryVM.AddCost(cost));
            }
            return deltas;
        }
        private List<CostObject> addController(TECController controller)
        {
            List<CostObject> deltas = new List<CostObject>();
            deltas.AddRange(ControllerSummaryVM.AddHardware(controller.Type));
            foreach(TECCost cost in controller.AssociatedCosts)
            {
                deltas.AddRange(ControllerSummaryVM.AddCost(cost));
            }
            foreach(TECConnection connection in controller.ChildrenConnections)
            {
                deltas.AddRange(addConnection(connection));
            }
            return deltas;
        }
        private List<CostObject> addPanel(TECPanel panel)
        {
            List<CostObject> deltas = new List<CostObject>();
            deltas.AddRange(PanelSummaryVM.AddHardware(panel.Type));
            foreach(TECCost cost in panel.AssociatedCosts)
            {
                deltas.AddRange(PanelSummaryVM.AddCost(cost));
            }
            return deltas;
        }
        private List<CostObject> addConnection(TECConnection connection)
        {
            List<CostObject> deltas = new List<CostObject>();
            if (connection is TECNetworkConnection netConnect)
            {
                deltas.AddRange(WireSummaryVM.AddRun(netConnect.ConnectionType, netConnect.Length));
            }
            else if (connection is TECSubScopeConnection ssConnect)
            {
                foreach(TECElectricalMaterial connectionType in ssConnect.ConnectionTypes)
                {
                    deltas.AddRange(WireSummaryVM.AddRun(connectionType, ssConnect.Length));
                }
            }
            if (connection.ConduitType != null)
            {
                deltas.AddRange(ConduitSummaryVM.AddRun(connection.ConduitType, connection.ConduitLength));
            }
            return deltas;
        }
        
        private List<CostObject> removeSystem(TECSystem system)
        {
            List<CostObject> deltas = new List<CostObject>();
            foreach (TECEquipment equip in system.Equipment)
            {
                deltas.AddRange(addEquipment(equip));
            }
            foreach(TECMisc misc in system.MiscCosts)
            {
                deltas.AddRange(MiscSummaryVM.RemoveCost(misc));
            }
            foreach(TECCost cost in system.AssociatedCosts) {
                deltas.AddRange(MiscSummaryVM.RemoveCost(cost));
            }
            return deltas;
        }
        private List<CostObject> removeEquipment(TECEquipment equip)
        {
            List<CostObject> deltas = new List<CostObject>();
            foreach (TECSubScope ss in equip.SubScope)
            {
                deltas.AddRange(removeSubScope(ss));
            }
            foreach(TECCost cost in equip.AssociatedCosts)
            {
                deltas.AddRange(MiscSummaryVM.RemoveCost(cost));
            }
            return deltas;
        }
        private List<CostObject> removeSubScope(TECSubScope ss)
        {
            List<CostObject> deltas = new List<CostObject>();
            foreach (TECDevice dev in ss.Devices)
            {
                deltas.AddRange(DeviceSummaryVM.RemoveHardware(dev));
            }
            foreach(TECCost cost in ss.AssociatedCosts)
            {
                deltas.AddRange(MiscSummaryVM.RemoveCost(cost));
            }
            return deltas;
        }
        private List<CostObject> removeController(TECController controller)
        {
            List<CostObject> deltas = new List<CostObject>();
            deltas.AddRange(ControllerSummaryVM.RemoveHardware(controller.Type));
            foreach(TECCost cost in controller.AssociatedCosts)
            {
                deltas.AddRange(ControllerSummaryVM.RemoveCost(cost));
            }
            foreach(TECConnection connection in controller.ChildrenConnections)
            {
                deltas.AddRange(removeConnection(connection));
            }
            return deltas;
        }
        private List<CostObject> removePanel(TECPanel panel)
        {
            List<CostObject> deltas = new List<CostObject>();
            deltas.AddRange(PanelSummaryVM.RemoveHardware(panel.Type));
            foreach(TECCost cost in panel.AssociatedCosts)
            {
                deltas.AddRange(PanelSummaryVM.RemoveCost(cost));
            }
            return deltas;
        }
        private List<CostObject> removeConnection(TECConnection connection)
        {
            List<CostObject> deltas = new List<CostObject>();
            if (connection is TECNetworkConnection netConnect)
            {
                deltas.AddRange(WireSummaryVM.RemoveRun(netConnect.ConnectionType, netConnect.Length));
            }
            else if (connection is TECSubScopeConnection ssConnect)
            {
                foreach(TECElectricalMaterial connectionType in ssConnect.ConnectionTypes)
                {
                    deltas.AddRange(WireSummaryVM.RemoveRun(connectionType, ssConnect.Length));
                }
            }
            deltas.AddRange(ConduitSummaryVM.RemoveRun(connection.ConduitType, connection.ConduitLength));
            return deltas;
        }
        #endregion

        private void instanceChanged(PropertyChangedExtendedEventArgs args)
        {
            if (args.Change == Change.Add)
            {
                if (args.Value is TECSystem system)
                {
                    updateTotals(addSystem(system));
                }
                else if (args.Value is TECEquipment equip)
                {
                    updateTotals(addEquipment(equip));
                }
                else if (args.Value is TECSubScope ss)
                {
                    updateTotals(addSubScope(ss));
                }
                else if (args.Value is TECController controller)
                {
                    updateTotals(addController(controller));
                }
                else if (args.Value is TECPanel panel)
                {
                    updateTotals(addPanel(panel));
                }
                else if (args.Value is TECConnection connection)
                {
                    updateTotals(addConnection(connection));
                }
                else if (args.Value is TECDevice dev && args.Sender is TECSubScope sub)
                {
                    updateTotals(DeviceSummaryVM.AddHardware(dev));
                    if (sub.Connection != null)
                    {
                        foreach(TECElectricalMaterial connectionType in dev.ConnectionTypes)
                        {
                            updateTotals(WireSummaryVM.AddRun(connectionType, sub.Connection.Length));
                        }
                    }
                }
            }
            else if (args.Change == Change.Remove)
            {
                if (args.Value is TECSystem system)
                {
                    updateTotals(removeSystem(system));
                }
                else if (args.Value is TECEquipment equip)
                {
                    updateTotals(removeEquipment(equip));
                }
                else if (args.Value is TECSubScope ss)
                {
                    updateTotals(removeSubScope(ss));
                }
                else if (args.Value is TECController controller)
                {
                    updateTotals(removeController(controller));
                }
                else if (args.Value is TECPanel panel)
                {
                    updateTotals(removePanel(panel));
                }
                else if (args.Value is TECConnection connection)
                {
                    updateTotals(removeConnection(connection));
                }
                else if (args.Value is TECDevice dev && args.Sender is TECSubScope sub)
                {
                    updateTotals(DeviceSummaryVM.RemoveHardware(dev));
                    if (sub.Connection != null)
                    {
                        foreach(TECElectricalMaterial connectionType in dev.ConnectionTypes)
                        {
                            updateTotals(WireSummaryVM.AddRun(connectionType, sub.Connection.Length));
                        }
                    }
                }
            }
            else if (args.Change == Change.Edit)
            {
                if (args.Sender is TECConnection connection)
                {
                    if (args.PropertyName == "Length")
                    {
                        double deltaLength = (double)args.Value - (double)args.OldValue;
                        if (connection is TECNetworkConnection netConnect)
                        {
                            updateTotals(WireSummaryVM.AddLength(netConnect.ConnectionType, deltaLength));
                        }
                        else if (connection is TECSubScopeConnection ssConnect)
                        {
                            foreach(TECElectricalMaterial connectionType in ssConnect.ConnectionTypes)
                            {
                                updateTotals(WireSummaryVM.AddLength(connectionType, deltaLength));
                            }
                        }
                    }
                    else if (args.PropertyName == "ConduitLength")
                    {
                        if (connection.ConduitType != null)
                        {
                            double deltaLength = (double)args.Value - (double)args.OldValue;
                            updateTotals(ConduitSummaryVM.AddLength(connection.ConduitType, deltaLength));
                        }
                    }
                    else if (args.PropertyName == "ConnectionType")
                    {
                        updateTotals(WireSummaryVM.RemoveRun(args.OldValue as TECElectricalMaterial, connection.Length));
                        updateTotals(WireSummaryVM.AddRun(args.Value as TECElectricalMaterial, connection.Length));
                    }
                    else if (args.PropertyName == "ConduitType")
                    {
                        if (args.OldValue != null)
                        {
                            updateTotals(ConduitSummaryVM.RemoveRun(args.OldValue as TECElectricalMaterial, connection.ConduitLength));
                        }
                        if (args.Value != null)
                        {
                            updateTotals(ConduitSummaryVM.AddRun(args.Value as TECElectricalMaterial, connection.ConduitLength));
                        }
                    }
                }
                else if (args.Sender is TECMisc misc)
                {
                    if (args.PropertyName == "Quantity")
                    {
                        int deltaQuantity = (int)args.Value - (int)args.OldValue;
                        updateTotals(MiscSummaryVM.ChangeQuantity(misc, deltaQuantity));
                    }
                    else if (args.PropertyName == "Cost")
                    {
                        updateTotals(MiscSummaryVM.UpdateCost(misc));
                    }
                    else if (args.PropertyName == "Labor")
                    {
                        updateTotals(MiscSummaryVM.UpdateCost(misc));
                    }
                }
            }
            else
            {
                throw new NotImplementedException("Change type not recognized.");
            }
        }

        private void updateTotals(List<CostObject> deltas)
        {
            foreach (CostObject delta in deltas)
            {
                if (delta.Type == CostType.TEC)
                {
                    TotalTECCost += delta.Cost;
                    TotalTECLabor += delta.Labor;
                }
                else if (delta.Type == CostType.Electrical)
                {
                    TotalElecCost += delta.Cost;
                    TotalTECLabor += delta.Labor;
                }
                else
                {
                    throw new NullReferenceException("CostType in CostObjet not valid.");
                }
            }
        }
        #endregion
    }
}
