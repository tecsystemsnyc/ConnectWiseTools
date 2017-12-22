using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Windows.Input;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels.AddVMs
{
    public class AddSystemVM : AddVM
    {
        private TECScopeManager parent;
        private TECSystem toAdd;
        private int quantity;
        private TECSystem underlyingTemplate;

        public TECSystem ToAdd
        {
            get { return toAdd; }
            private set
            {
                toAdd = value;
                RaisePropertyChanged("ToAdd");
            }
        }
        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                RaisePropertyChanged("Quantity");
            }
        }

        public AddSystemVM(TECScopeManager scopeManager) : base(scopeManager)
        {
            Quantity = 1;
            parent = scopeManager;
            toAdd = new TECSystem(false);
            AddCommand = new RelayCommand(addExecute, addCanExecute);
        }

        private bool addCanExecute()
        {
            return true;
        }
        private void addExecute()
        {
            for (int x = 0; x < Quantity; x++)
            {
                if (parent is TECBid bid)
                {
                    TECTypical typical = new TECTypical(toAdd, bid);
                    bid.Systems.Add(typical);
                    Added?.Invoke(typical);
                }
                else if (parent is TECTemplates templates)
                {
                    TECSystem system = null;
                    if(underlyingTemplate != null)
                    {
                        system = new TECSystem(underlyingTemplate, ToAdd.IsTypical, templates,
                        synchronizers: new Tuple<TemplateSynchronizer<TECEquipment>, TemplateSynchronizer<TECSubScope>>(templates.EquipmentSynchronizer, templates.SubScopeSynchronizer));
                        system.CopyPropertiesFromScope(ToAdd);
                    } else
                    {
                        system = ToAdd;
                    }
                    
                    templates.SystemTemplates.Add(system);
                    Added?.Invoke(system);
                }
            }
            
        }

        internal void SetTemplate(TECSystem system)
        {
            underlyingTemplate = system;
            ToAdd = new TECSystem(system, false, parent);
        }
    }
}
