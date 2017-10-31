using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.Utilities;
using TECUserControlLibrary.ViewModels.Interfaces;

namespace TECUserControlLibrary.ViewModels
{
    public class MaterialSummaryVM : ViewModelBase
    {
        #region Fields
        private double _totalTECCost;
        private double _totalTECLabor;
        private double _totalElecCost;
        private double _totalElecLabor;

        private IComponentSummaryVM _currentVM;
        private MaterialSummaryIndex _selectedIndex;
        private string _currentType;
        #endregion

        //Constructor
        public MaterialSummaryVM(TECBid bid, ChangeWatcher changeWatcher)
        {
            reinitializeTotals();
            initializeVMs();
            loadBid(bid);
            resubscribe(changeWatcher);
            SelectedIndex = MaterialSummaryIndex.Devices;
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
                _totalTECLabor = value;
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

        public IComponentSummaryVM CurrentVM
        {
            get
            {
                return _currentVM;
            }
            private set
            {
                _currentVM = value;
                RaisePropertyChanged("CurrentVM");
            }
        }
        public MaterialSummaryIndex SelectedIndex
        {
            set
            {
                switch (value)
                {
                    case MaterialSummaryIndex.Devices:
                        CurrentVM = DeviceSummaryVM;
                        CurrentType = "Device";
                        break;
                    case MaterialSummaryIndex.Controllers:
                        CurrentVM = ControllerSummaryVM;
                        CurrentType = "Controller";
                        break;
                    case MaterialSummaryIndex.Panels:
                        CurrentVM = PanelSummaryVM;
                        CurrentType = "Panel";
                        break;
                    case MaterialSummaryIndex.Wire:
                        CurrentVM = WireSummaryVM;
                        CurrentType = "Wire";
                        break;
                    case MaterialSummaryIndex.Conduit:
                        CurrentVM = ConduitSummaryVM;
                        CurrentType = "Conduit";
                        break;
                    case MaterialSummaryIndex.Misc:
                        CurrentVM = MiscSummaryVM;
                        CurrentType = "Misc";
                        break;
                    default:
                        throw new InvalidOperationException("Material Summary Index not found.");
                }
                _selectedIndex = value;
                RaisePropertyChanged("SelectedIndex");
            }
            get
            {
                return _selectedIndex;
            }
        }
        public string CurrentType
        {
            get { return _currentType; }
            set
            {
                _currentType = value;
                RaisePropertyChanged("CurrentType");
            }
        }
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
            foreach (TECTypical typical in bid.Systems)
            {
                foreach (TECSystem instance in typical.Instances)
                {
                    updateTotals(addSystem(instance));
                }
            }
            foreach(TECController controller in bid.Controllers)
            {
                updateTotals(addController(controller));
            }
            foreach(TECPanel panel in bid.Panels)
            {
                updateTotals(addPanel(panel));
            }
            foreach(TECMisc misc in bid.MiscCosts)
            {
                updateTotals(MiscSummaryVM.AddCost(misc));
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
        private CostBatch addSystem(TECSystem system)
        {
            CostBatch deltas = new CostBatch();
            foreach(TECEquipment equip in system.Equipment)
            {
                deltas += (addEquipment(equip));
            }
            foreach(TECController controller in system.Controllers)
            {
                deltas += addController(controller);
            }
            foreach(TECPanel panel in system.Panels)
            {
                deltas += addPanel(panel);
            }
            foreach(TECMisc misc in system.MiscCosts)
            {
                deltas += (MiscSummaryVM.AddCost(misc));
            }
            foreach(TECCost cost in system.AssociatedCosts)
            {
                deltas += (MiscSummaryVM.AddCost(cost));
            }
            return deltas;
        }
        private CostBatch addEquipment(TECEquipment equip)
        {
            CostBatch deltas = new CostBatch();
            foreach (TECSubScope ss in equip.SubScope)
            {
                deltas += (addSubScope(ss));
            }
            foreach(TECCost cost in equip.AssociatedCosts)
            {
                deltas += (MiscSummaryVM.AddCost(cost));
            }
            return deltas;
        }
        private CostBatch addSubScope(TECSubScope ss)
        {
            CostBatch deltas = new CostBatch();
            foreach (TECDevice dev in ss.Devices)
            {
                deltas += (DeviceSummaryVM.AddHardware(dev));
            }
            foreach(TECCost cost in ss.AssociatedCosts)
            {
                deltas += (MiscSummaryVM.AddCost(cost));
            }
            return deltas;
        }
        private CostBatch addController(TECController controller)
        {
            CostBatch deltas = new CostBatch();
            deltas += (ControllerSummaryVM.AddHardware(controller.Type));
            foreach(TECCost cost in controller.AssociatedCosts)
            {
                deltas += (ControllerSummaryVM.AddCost(cost));
            }
            foreach(TECCost cost in controller.Type.AssociatedCosts)
            {
                deltas += (ControllerSummaryVM.AddCost(cost));
            }
            foreach(TECConnection connection in controller.ChildrenConnections)
            {
                deltas += (addConnection(connection));
            }
            foreach(TECIOModule module in controller.IOModules)
            {
                addIOModule(module);
            }
            return deltas;
        }
        private CostBatch addIOModule(TECIOModule module)
        {
            //Costs associated with IO Modules will fall under controller associated costs.
            CostBatch deltas = new CostBatch();
            deltas += (ControllerSummaryVM.AddCost(module));
            foreach(TECCost cost in module.AssociatedCosts)
            {
                deltas += (ControllerSummaryVM.AddCost(cost));
            }
            return deltas;
        }
        private CostBatch addPanel(TECPanel panel)
        {
            CostBatch deltas = new CostBatch();
            deltas += (PanelSummaryVM.AddHardware(panel.Type));
            foreach(TECCost cost in panel.AssociatedCosts)
            {
                deltas += (PanelSummaryVM.AddCost(cost));
            }
            return deltas;
        }
        private CostBatch addConnection(TECConnection connection)
        {
            if (!connection.IsTypical)
            {
                CostBatch deltas = new CostBatch();
                foreach (TECElectricalMaterial connectionType in connection.GetConnectionTypes())
                {
                    deltas += (WireSummaryVM.AddRun(connectionType, connection.Length));
                }
                if (connection.ConduitType != null)
                {
                    deltas += (ConduitSummaryVM.AddRun(connection.ConduitType, connection.ConduitLength));
                }
                return deltas;
            }
            else
            {
                return new CostBatch();
            }
        }
        
        private CostBatch removeSystem(TECSystem system)
        {
            CostBatch deltas = new CostBatch();
            foreach (TECEquipment equip in system.Equipment)
            {
                deltas += removeEquipment(equip);
            }
            foreach (TECController controller in system.Controllers)
            {
                deltas += removeController(controller);
            }
            foreach (TECPanel panel in system.Panels)
            {
                deltas += removePanel(panel);
            }
            foreach(TECMisc misc in system.MiscCosts)
            {
                deltas += (MiscSummaryVM.RemoveCost(misc));
            }
            foreach(TECCost cost in system.AssociatedCosts) {
                deltas += (MiscSummaryVM.RemoveCost(cost));
            }
            return deltas;
        }
        private CostBatch removeEquipment(TECEquipment equip)
        {
            CostBatch deltas = new CostBatch();
            foreach (TECSubScope ss in equip.SubScope)
            {
                deltas += (removeSubScope(ss));
            }
            foreach(TECCost cost in equip.AssociatedCosts)
            {
                deltas += (MiscSummaryVM.RemoveCost(cost));
            }
            return deltas;
        }
        private CostBatch removeSubScope(TECSubScope ss)
        {
            CostBatch deltas = new CostBatch();
            foreach (TECDevice dev in ss.Devices)
            {
                deltas += (DeviceSummaryVM.RemoveHardware(dev));
            }
            foreach(TECCost cost in ss.AssociatedCosts)
            {
                deltas += (MiscSummaryVM.RemoveCost(cost));
            }
            return deltas;
        }
        private CostBatch removeController(TECController controller)
        {
            CostBatch deltas = new CostBatch();
            deltas += (ControllerSummaryVM.RemoveHardware(controller.Type));
            foreach(TECCost cost in controller.AssociatedCosts)
            {
                deltas += (ControllerSummaryVM.RemoveCost(cost));
            }
            foreach(TECCost cost in controller.Type.AssociatedCosts)
            {
                deltas += (ControllerSummaryVM.RemoveCost(cost));
            }
            foreach(TECConnection connection in controller.ChildrenConnections)
            {
                deltas += (removeConnection(connection));
            }
            foreach(TECIOModule module in controller.IOModules)
            {
                removeIOModule(module);
            }
            return deltas;
        }
        private CostBatch removeIOModule(TECIOModule module)
        {
            //Costs associated with IO Modules will fall under controller associated costs.
            CostBatch deltas = new CostBatch();
            deltas += (ControllerSummaryVM.RemoveCost(module));
            foreach(TECCost cost in module.AssociatedCosts)
            {
                deltas += (ControllerSummaryVM.RemoveCost(cost));
            }
            return deltas;
        }
        private CostBatch removePanel(TECPanel panel)
        {
            CostBatch deltas = new CostBatch();
            deltas += (PanelSummaryVM.RemoveHardware(panel.Type));
            foreach(TECCost cost in panel.AssociatedCosts)
            {
                deltas += (PanelSummaryVM.RemoveCost(cost));
            }
            return deltas;
        }
        private CostBatch removeConnection(TECConnection connection)
        {
            CostBatch deltas = new CostBatch();
            foreach (TECElectricalMaterial connectionType in connection.GetConnectionTypes())
            {
                deltas += (WireSummaryVM.RemoveRun(connectionType, connection.Length));
            }
            if (connection.ConduitType != null)
            {
                deltas += (ConduitSummaryVM.RemoveRun(connection.ConduitType, connection.ConduitLength));
            }
            return deltas;
        }
        #endregion

        private void instanceChanged(TECChangedEventArgs args)
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
                else if (args.Value is TECIOModule module)
                {
                    updateTotals(addIOModule(module));
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
                else if (args.Value is TECMisc misc)
                {
                    updateTotals(MiscSummaryVM.AddCost(misc));
                }
                else if (args.Value is TECCost cost)
                {
                    if (args.Sender is TECHardware || args.Sender is TECElectricalMaterial)
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        updateTotals(MiscSummaryVM.AddCost(cost));
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
                else if (args.Value is TECIOModule module)
                {
                    updateTotals(removeIOModule(module));
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
                else if (args.Value is TECMisc misc)
                {
                    updateTotals(MiscSummaryVM.RemoveCost(misc));
                }
                else if (args.Value is TECCost cost)
                {
                    if (args.Sender is TECHardware || args.Sender is TECElectricalMaterial)
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        updateTotals(MiscSummaryVM.RemoveCost(cost));
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
                        foreach (TECElectricalMaterial connectionType in connection.GetConnectionTypes())
                        {
                            updateTotals(WireSummaryVM.AddLength(connectionType, deltaLength));
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

        private void updateTotals(CostBatch deltas)
        {
            TotalTECCost += deltas.GetCost(CostType.TEC);
            TotalTECLabor += deltas.GetLabor(CostType.TEC);
            TotalElecCost += deltas.GetCost(CostType.Electrical);
            TotalElecLabor += deltas.GetLabor(CostType.Electrical);
        }
        #endregion
    }
}
