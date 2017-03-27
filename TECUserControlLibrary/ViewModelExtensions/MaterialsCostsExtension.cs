using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModelExtensions
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MaterialsCostsExtension : ViewModelBase, IDropTarget
    {
        #region Properties
        private TECTemplates _templates;
        public TECTemplates Templates
        {
            get { return _templates; }
            set
            {
                _templates = value;
                RaisePropertyChanged("Templates");
            }
        }

        private string _connectionTypeName;
        public string ConnectionTypeName
        {
            get { return _connectionTypeName; }
            set
            {
                _connectionTypeName = value;
                RaisePropertyChanged("ConnectionTypeName");
            }
        }
        private double _connectionTypeCost;
        public double ConnectionTypeCost
        {
            get { return _connectionTypeCost; }
            set
            {
                _connectionTypeCost = value;
                RaisePropertyChanged("ConnectionTypeCost");
            }
        }
        private double _connectionTypeLabor;
        public double ConnectionTypeLabor
        {
            get { return _connectionTypeLabor; }
            set
            {
                _connectionTypeLabor = value;
                RaisePropertyChanged("ConnectionTypeLabor");
            }
        }

        private string _conduitTypeName;
        public string ConduitTypeName
        {
            get { return _conduitTypeName; }
            set
            {
                _conduitTypeName = value;
                RaisePropertyChanged("ConduitTypeName");
            }
        }
        private double _conduitTypeCost;
        public double ConduitTypeCost
        {
            get { return _conduitTypeCost; }
            set
            {
                _conduitTypeCost = value;
                RaisePropertyChanged("ConduitTypeCost");
            }
        }
        private double _conduitTypeLabor;
        public double ConduitTypeLabor
        {
            get { return _conduitTypeLabor; }
            set
            {
                _conduitTypeLabor = value;
                RaisePropertyChanged("ConduitTypeLabor");
            }
        }

        private string _associatedCostName;
        public string AssociatedCostName
        {
            get { return _associatedCostName; }
            set
            {
                _associatedCostName = value;
                RaisePropertyChanged("AssociatedCostName");
            }
        }
        private double _associatedCostCost;
        public double AssociatedCostCost
        {
            get { return _associatedCostCost; }
            set
            {
                _associatedCostCost = value;
                RaisePropertyChanged("AssociatedCostCost");
            }
        }
        private double _associatedCostLabor;
        public double AssociatedCostLabor
        {
            get { return _associatedCostLabor; }
            set
            {
                _associatedCostLabor = value;
                RaisePropertyChanged("AssociatedCostLabor");
            }
        }

        private string _panelTypeName;
        public string PanelTypeName
        {
            get { return _panelTypeName; }
            set
            {
                _panelTypeName = value;
                RaisePropertyChanged("PanelTypeName");
            }
        }
        private double _panelTypeCost;
        public double PanelTypeCost
        {
            get { return _panelTypeCost; }
            set
            {
                _panelTypeCost = value;
                RaisePropertyChanged("PanelTypeCost");
            }
        }

        #region Command Properties
        public ICommand AddConnectionTypeCommand { get; private set; }
        public ICommand AddConduitTypeCommand { get; private set; }
        public ICommand AddAssociatedCostCommand { get; private set; }
        public ICommand AddPanelTypeCommand { get; private set; }
        #endregion

        #region Delegates
        public Action<IDropInfo> DragHandler;
        public Action<IDropInfo> DropHandler;
        #endregion

        #endregion

        public MaterialsCostsExtension(TECTemplates templates)
        {
            Templates = templates;
            setupCommands();
        }
        
        #region Methods
        private void setupCommands()
        {
            AddConnectionTypeCommand = new RelayCommand(addConnectionTypeExecute);
            AddConduitTypeCommand = new RelayCommand(addConduitTypeExecute);
            AddAssociatedCostCommand = new RelayCommand(addAsociatedCostExecute, canAddAssociatedCost);
            AddPanelTypeCommand = new RelayCommand(addPanelTypeExecute, canAddPanelTypeExecute);
        }

        private void addConnectionTypeExecute()
        {
            var connectionType = new TECConnectionType();
            connectionType.Name = ConnectionTypeName;
            connectionType.Cost = ConnectionTypeCost;
            connectionType.Labor = ConnectionTypeLabor;
            Templates.ConnectionTypeCatalog.Add(connectionType);
            ConnectionTypeName = "";
            ConnectionTypeCost = 0;
            ConnectionTypeLabor = 0;
        }
        private void addConduitTypeExecute()
        {
            var conduitType = new TECConduitType();
            conduitType.Name = ConduitTypeName;
            conduitType.Cost = ConduitTypeCost;
            conduitType.Labor = ConduitTypeLabor;
            Templates.ConduitTypeCatalog.Add(conduitType);
            ConduitTypeName = "";
            ConduitTypeCost = 0;
            ConduitTypeLabor = 0;
        }
        private void addAsociatedCostExecute()
        {
            var associatedCost = new TECAssociatedCost();
            associatedCost.Name = AssociatedCostName;
            associatedCost.Cost = AssociatedCostCost;
            associatedCost.Labor = AssociatedCostLabor;
            Templates.AssociatedCostsCatalog.Add(associatedCost);
            AssociatedCostName = "";
            AssociatedCostCost = 0;
            AssociatedCostLabor = 0;
        }
        private bool canAddAssociatedCost()
        {
            if(AssociatedCostName == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private void addPanelTypeExecute()
        {
            var panelType = new TECPanelType();
            panelType.Name = PanelTypeName;
            panelType.Cost = PanelTypeCost;

            Templates.PanelTypeCatalog.Add(panelType);
            PanelTypeName = "";
            PanelTypeCost = 0;
        }
        private bool canAddPanelTypeExecute()
        {
            if(PanelTypeName != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            DragHandler(dropInfo);
        }

        public void Drop(IDropInfo dropInfo)
        {
            DropHandler(dropInfo);
        }
        #endregion
    }

}