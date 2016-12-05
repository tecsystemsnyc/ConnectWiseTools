using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECTemplates : TECObject
    {
        #region Properties
        private ObservableCollection<TECSystem> _systemTemplates;
        private ObservableCollection<TECEquipment> _equipmentTemplates;
        private ObservableCollection<TECSubScope> _subScopeTemplates;
        private ObservableCollection<TECDevice> _deviceCatalog;
        private ObservableCollection<TECManufacturer> _manufacturerCatalog;
        private ObservableCollection<TECTag> _tags;

        public ObservableCollection<TECSystem> SystemTemplates
        {
            get { return _systemTemplates; }
            set
            {
                _systemTemplates = value;
                SystemTemplates.CollectionChanged += CollectionChanged;
                RaisePropertyChanged("SystemTemplates");
            }
        }
        public ObservableCollection<TECEquipment> EquipmentTemplates
        {
            get { return _equipmentTemplates; }
            set
            {
                _equipmentTemplates = value;
                EquipmentTemplates.CollectionChanged += CollectionChanged;
                RaisePropertyChanged("EquipmentTemplates");
            }
        }
        public ObservableCollection<TECSubScope> SubScopeTemplates
        {
            get { return _subScopeTemplates; }
            set
            {
                _subScopeTemplates = value;
                SubScopeTemplates.CollectionChanged += CollectionChanged;
                RaisePropertyChanged("SubScopeTemplates");
            }
        }
        public ObservableCollection<TECDevice> DeviceCatalog
        {
            get { return _deviceCatalog; }
            set
            {
                _deviceCatalog = value;
                DeviceCatalog.CollectionChanged += CollectionChanged;
                RaisePropertyChanged("DeviceCatalog");
            }
        }
        public ObservableCollection<TECManufacturer> ManufacturerCatalog
        {
            get { return _manufacturerCatalog; }
            set
            {
                _manufacturerCatalog = value;
                ManufacturerCatalog.CollectionChanged += CollectionChanged;
                RaisePropertyChanged("ManufacturerCatalog");
            }
        }
        public ObservableCollection<TECTag> Tags
        {
            get { return _tags; }
            set
            {
                _tags = value;
                Tags.CollectionChanged += CollectionChanged;
                RaisePropertyChanged("Tags");
            }
        }
       
        public override object Copy()
        {
            throw new NotImplementedException();
        }

        #endregion //Properties

        #region Constructors

        public TECTemplates()
        {
            SystemTemplates = new ObservableCollection<TECSystem>();
            EquipmentTemplates = new ObservableCollection<TECEquipment>();
            SubScopeTemplates = new ObservableCollection<TECSubScope>();
            DeviceCatalog = new ObservableCollection<TECDevice>();
            Tags = new ObservableCollection<TECTag>();
            ManufacturerCatalog = new ObservableCollection<TECManufacturer>();
            
        }

        #endregion //Constructors

        #region Collection Changed
        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine("Collection changed");
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    NotifyPropertyChanged("Add", this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyPropertyChanged("Remove", this, item);
                }
            }
        }
        #endregion
    }
}
