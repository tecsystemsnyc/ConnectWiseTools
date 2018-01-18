﻿using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Windows.Input;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels.AddVMs
{
    public class AddEquipmentVM : AddVM
    {
        private TECSystem parent;
        private TECEquipment toAdd;
        private int quantity;
        private Action<TECEquipment> add;
        private bool isTypical = false;
        private TECEquipment underlyingTemplate;
        private bool _displayReferenceProperty = false;

        public TECEquipment ToAdd
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
        public bool DisplayReferenceProperty {
            get { return _displayReferenceProperty; }
            private set
            {
                _displayReferenceProperty = value;
                RaisePropertyChanged("DisplayReferenceProperty");
            }
        }

        public AddEquipmentVM(TECSystem parentSystem, TECScopeManager scopeManager) : base(scopeManager)
        {
            parent = parentSystem;
            isTypical = parent.IsTypical;
            toAdd = new TECEquipment(parentSystem.IsTypical);
            add = equip =>
            {
                parentSystem.Equipment.Add(equip);
            };
            AddCommand = new RelayCommand(addExecute, addCanExecute);
            Quantity = 1;
        }
        public AddEquipmentVM(Action<TECEquipment> addMethod, TECScopeManager scopeManager) : base(scopeManager)
        {
            toAdd = new TECEquipment(false);
            add = addMethod;
            AddCommand = new RelayCommand(addExecute, addCanExecute);
            Quantity = 1;
            PropertiesVM.DisplayReferenceProperty = false;
        }

        private bool addCanExecute()
        {
            return true;
        }
        private void addExecute()
        {
            for (int x = 0; x < Quantity; x++)
            {
                TECEquipment equipment = null;
                if (AsReference && underlyingTemplate != null)
                {
                    equipment = templates.EquipmentSynchronizer.NewItem(underlyingTemplate);
                }
                else if (underlyingTemplate != null)
                {
                    if (templates != null)
                    {
                        equipment = new TECEquipment(underlyingTemplate, isTypical, ssSynchronizer: templates.SubScopeSynchronizer);
                    }
                    else
                    {
                        equipment = new TECEquipment(underlyingTemplate, isTypical);
                    }
                }
                else
                {
                    equipment = new TECEquipment(ToAdd, isTypical);
                }
                equipment.CopyPropertiesFromScope(ToAdd);
                add(equipment);
                Added?.Invoke(equipment);
            }
        }

        public void SetTemplate(TECEquipment template)
        {
            underlyingTemplate = template;
            ToAdd = new TECEquipment(template, isTypical);
            DisplayReferenceProperty = true;
        }
        
    }
}
