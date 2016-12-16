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
        private Guid _infoGuid;
        private ObservableCollection<TECSystem> _systemTemplates;
        private ObservableCollection<TECEquipment> _equipmentTemplates;
        private ObservableCollection<TECSubScope> _subScopeTemplates;
        private ObservableCollection<TECDevice> _deviceCatalog;
        private ObservableCollection<TECManufacturer> _manufacturerCatalog;
        private ObservableCollection<TECTag> _tags;

        public Guid InfoGuid
        {
            get { return _infoGuid; }
        }
        public ObservableCollection<TECSystem> SystemTemplates
        {
            get { return _systemTemplates; }
            set
            {
                var temp = this.Copy();
                SystemTemplates.CollectionChanged -= CollectionChanged;
                _systemTemplates = value;
                SystemTemplates.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("SystemTemplates", temp, this);
            }
        }
        public ObservableCollection<TECEquipment> EquipmentTemplates
        {
            get { return _equipmentTemplates; }
            set
            {
                var temp = this.Copy();
                EquipmentTemplates.CollectionChanged -= CollectionChanged;
                _equipmentTemplates = value;
                EquipmentTemplates.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("EquipmentTemplates", temp, this);
            }
        }
        public ObservableCollection<TECSubScope> SubScopeTemplates
        {
            get { return _subScopeTemplates; }
            set
            {
                var temp = this.Copy();
                SubScopeTemplates.CollectionChanged -= CollectionChanged;
                _subScopeTemplates = value;
                SubScopeTemplates.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("SubScopeTemplates", temp, this);
            }
        }
        public ObservableCollection<TECDevice> DeviceCatalog
        {
            get { return _deviceCatalog; }
            set
            {
                var temp = this.Copy();
                DeviceCatalog.CollectionChanged -= CollectionChanged;
                _deviceCatalog = value;
                DeviceCatalog.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("DeviceCatalog", temp, this);
            }
        }
        public ObservableCollection<TECManufacturer> ManufacturerCatalog
        {
            get { return _manufacturerCatalog; }
            set
            {
                var temp = this.Copy();
                ManufacturerCatalog.CollectionChanged -= CollectionChanged;
                _manufacturerCatalog = value;
                ManufacturerCatalog.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("ManufacturerCatalog", temp, this);
            }
        }
        public ObservableCollection<TECTag> Tags
        {
            get { return _tags; }
            set
            {
                var temp = this.Copy();
                Tags.CollectionChanged -= CollectionChanged;
                _tags = value;
                Tags.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("Tags", temp, this);
            }
        }
       
        public override object Copy()
        {
            return new TECTemplates(this);
        }

        #endregion //Properties

        #region Constructors

        public TECTemplates() : this(Guid.NewGuid()) {}

        public TECTemplates(Guid guid)
        {
            _infoGuid = guid;

            _systemTemplates = new ObservableCollection<TECSystem>();
            _equipmentTemplates = new ObservableCollection<TECEquipment>();
            _subScopeTemplates = new ObservableCollection<TECSubScope>();
            _deviceCatalog = new ObservableCollection<TECDevice>();
            _tags = new ObservableCollection<TECTag>();
            _manufacturerCatalog = new ObservableCollection<TECManufacturer>();

            SystemTemplates.CollectionChanged += CollectionChanged;
            EquipmentTemplates.CollectionChanged += CollectionChanged;
            SubScopeTemplates.CollectionChanged += CollectionChanged;
            DeviceCatalog.CollectionChanged += CollectionChanged;
            Tags.CollectionChanged += CollectionChanged;
            ManufacturerCatalog.CollectionChanged += CollectionChanged;
        }

        public TECTemplates(TECTemplates templatesSource) : this()
        {
            foreach(TECSystem system in templatesSource.SystemTemplates)
            {
                SystemTemplates.Add(system);
            }
            foreach (TECEquipment equip in templatesSource.EquipmentTemplates)
            {
                EquipmentTemplates.Add(equip);
            }
            foreach (TECSubScope subScope in templatesSource.SubScopeTemplates)
            {
                SubScopeTemplates.Add(subScope);
            }
            foreach (TECDevice device in templatesSource.DeviceCatalog)
            {
                DeviceCatalog.Add(device);
            }
            foreach (TECTag tag in templatesSource.Tags)
            {
                Tags.Add(tag);
            }
            foreach (TECManufacturer man in templatesSource.ManufacturerCatalog)
            {
                ManufacturerCatalog.Add(man);
            }
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
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                NotifyPropertyChanged("Edit", this, sender);
            }
        }
        #endregion
    }
}
