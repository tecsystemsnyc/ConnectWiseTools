using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.ViewModels
{
    public class GlobalConnectionsVM : ViewModelBase, IDropTarget
    {
        Dictionary<TECScope, TECScope> dummyInstanceDictionary;

        private TECController _selectedController;
        private TECSystem _selectedSystem;
        private TECEquipment _selectedEquipment;
        private TECSubScopeConnection _selectedConnectedSubScope;
        private TECSubScope _selectedUnconnectedSubScope;

        public ObservableCollection<TECController> GlobalControllers { get; }
        public ObservableCollection<TECSubScopeConnection> ConnectedSubScope { get; }
        public ObservableCollection<TECSystem> UnconnectedSystems { get; }
        public ObservableCollection<TECEquipment> UnconnectedEquipment { get; }
        public ObservableCollection<TECSubScope> UnconnectedSubScope { get; }

        public TECController SelectedController
        {
            get
            {
                return _selectedController;
            }
            set
            {
                _selectedController = value;
                RaisePropertyChanged("SelectedController");
                handleSelectedControllerChanged();
            }
        }
        public TECSystem SelectedSystem
        {
            get
            {
                return _selectedSystem;
            }
            set
            {
                _selectedSystem = value;
                RaisePropertyChanged("SelectedSystem");
                handleSelectedSystemChanged();
            }
        }
        public TECEquipment SelectedEquipment
        {
            get
            {
                return _selectedEquipment;
            }
            set
            {
                _selectedEquipment = value;
                RaisePropertyChanged("SelectedEquipment");
                handleSelectedEquipmentChanged();
            }
        }
        public TECSubScopeConnection SelectedConnectedSubScope
        {
            get
            {
                return _selectedConnectedSubScope;
            }
            set
            {
                _selectedConnectedSubScope = value;
                RaisePropertyChanged("SelectedConnectedSubScope");
            }
        }
        public TECSubScope SelectedUnconnectedSubScope
        {
            get
            {
                return _selectedUnconnectedSubScope;
            }
            set
            {
                _selectedUnconnectedSubScope = value;
                RaisePropertyChanged("SelectedUnconnectedSubScope");
            }
        }

        public GlobalConnectionsVM(TECBid bid, ChangeWatcher watcher)
        {
            dummyInstanceDictionary = new Dictionary<TECScope, TECScope>();
            UnconnectedSystems = new ObservableCollection<TECSystem>();
            GlobalControllers = new ObservableCollection<TECController>();

            setupDummySystems(bid);

            foreach(TECController controller in bid.Controllers)
            {
                GlobalControllers.Add(controller);
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            throw new NotImplementedException();
        }

        public void Drop(IDropInfo dropInfo)
        {
            throw new NotImplementedException();
        }

        private void setupDummySystems(TECBid bid)
        {
            foreach (TECTypical typ in bid.Systems)
            {
                foreach (TECSystem sys in typ.Instances)
                {
                    List<TECEquipment> dummyEquip = new List<TECEquipment>();
                    foreach (TECEquipment equip in sys.Equipment)
                    {
                        List<TECSubScope> dummySS = new List<TECSubScope>();
                        foreach (TECSubScope ss in equip.SubScope)
                        {
                            if (!ss.IsNetwork && ss.Connection == null)
                            {
                                TECSubScope newDummySS = new TECSubScope(ss, false);
                                dummyInstanceDictionary.Add(newDummySS, ss);
                                dummySS.Add(newDummySS);
                            }
                        }
                        if (dummySS.Count > 0)
                        {
                            TECEquipment newDummyEquip = new TECEquipment(false);
                            newDummyEquip.CopyPropertiesFromScope(equip);
                            foreach(TECSubScope ss in dummySS)
                            {
                                newDummyEquip.SubScope.Add(ss);
                            }
                            dummyInstanceDictionary.Add(newDummyEquip, equip);
                            dummyEquip.Add(newDummyEquip);
                        }
                    }
                    if (dummyEquip.Count > 0)
                    {
                        TECSystem newDummySys = new TECSystem(false);
                        newDummySys.CopyPropertiesFromScope(sys);
                        foreach(TECEquipment equip in dummyEquip)
                        {
                            newDummySys.Equipment.Add(equip);
                        }
                        dummyInstanceDictionary.Add(newDummySys, sys);
                        UnconnectedSystems.Add(newDummySys);
                    }
                }
            }
        }
        
        private void handleSelectedControllerChanged()
        {
            ConnectedSubScope.Clear();

            foreach(TECSubScopeConnection ssConnect in SelectedController?.ChildrenConnections.Where(
                (connection) => connection is TECSubScopeConnection))
            {
                ConnectedSubScope.Add(ssConnect);
            }
        }
        private void handleSelectedSystemChanged()
        {
            UnconnectedEquipment.Clear();
            UnconnectedSubScope.Clear();
            SelectedEquipment = null;

            foreach(TECEquipment equip in SelectedSystem.Equipment)
            {
                UnconnectedEquipment.Add(equip);
            }
        }
        private void handleSelectedEquipmentChanged()
        {
            UnconnectedSubScope.Clear();

            foreach(TECSubScope ss in SelectedEquipment.SubScope)
            {
                UnconnectedSubScope.Add(ss);
            }
        }
    }
}
