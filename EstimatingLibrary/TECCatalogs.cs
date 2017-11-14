﻿using EstimatingLibrary.Interfaces;
using System;
using System.Collections.ObjectModel;

namespace EstimatingLibrary
{
    public class TECCatalogs : TECObject, IRelatable
    {
        private ObservableCollection<TECElectricalMaterial> _connectionTypes;
        private ObservableCollection<TECElectricalMaterial> _conduitTypes;
        private ObservableCollection<TECCost> _associatedCosts;
        private ObservableCollection<TECPanelType> _panelTypes;
        private ObservableCollection<TECControllerType> _controllerTypes;
        private ObservableCollection<TECIOModule> _ioModules;
        private ObservableCollection<TECDevice> _devices;
        private ObservableCollection<TECValve> _valves;
        private ObservableCollection<TECManufacturer> _manufacturers;
        private ObservableCollection<TECLabeled> _tags;

        public ObservableCollection<TECIOModule> IOModules
        {
            get { return _ioModules; }
            set
            {
                var old = IOModules;
                IOModules.CollectionChanged -= (sender, e) => CollectionChanged(sender, e, "IOModules");
                _ioModules = value;
                IOModules.CollectionChanged += (sender, e) => CollectionChanged(sender, e, "IOModules");
                notifyCombinedChanged(Change.Edit, "IOModules", this, value, old);
            }
        }
        public ObservableCollection<TECDevice> Devices
        {
            get { return _devices; }
            set
            {
                var old = Devices;
                Devices.CollectionChanged -= (sender, e) => CollectionChanged(sender, e, "Devices");
                _devices = value;
                Devices.CollectionChanged += (sender, e) => CollectionChanged(sender, e, "Devices");
                notifyCombinedChanged(Change.Edit, "Devices", this, value, old);
            }
        }
        public ObservableCollection<TECValve> Valves
        {
            get { return _valves; }
            set
            {
                var old = Valves;
                Valves.CollectionChanged -= (sender, e) => CollectionChanged(sender, e, "Valves");
                _valves = value;
                Valves.CollectionChanged += (sender, e) => CollectionChanged(sender, e, "Valves");
                notifyCombinedChanged(Change.Edit, "Valves", this, value, old);
            }
        }
        public ObservableCollection<TECManufacturer> Manufacturers
        {
            get { return _manufacturers; }
            set
            {
                var old = Manufacturers;
                Manufacturers.CollectionChanged -= (sender, e) => CollectionChanged(sender, e, "Manufacturers");
                _manufacturers = value;
                Manufacturers.CollectionChanged += (sender, e) => CollectionChanged(sender, e, "Manufacturers");
                notifyCombinedChanged(Change.Edit, "Manufacturers", this, value, old);
            }
        }
        public ObservableCollection<TECPanelType> PanelTypes
        {
            get { return _panelTypes; }
            set
            {
                var old = PanelTypes;
                PanelTypes.CollectionChanged -= (sender, e) => CollectionChanged(sender, e, "PanelTypes");
                _panelTypes = value;
                PanelTypes.CollectionChanged += (sender, e) => CollectionChanged(sender, e, "PanelTypes");
                notifyCombinedChanged(Change.Edit, "PanelTypes", this, value, old);
            }
        }
        public ObservableCollection<TECControllerType> ControllerTypes
        {
            
            get { return _controllerTypes; }
            set
            {
                var old = ControllerTypes;
                PanelTypes.CollectionChanged -= (sender, e) => CollectionChanged(sender, e, "ControllerTypes");
                _controllerTypes = value;
                ControllerTypes.CollectionChanged += (sender, e) => CollectionChanged(sender, e, "ControllerTypes");
                notifyCombinedChanged(Change.Edit, "ControllerTypes", this, value, old);
            }
        }
        public ObservableCollection<TECElectricalMaterial> ConnectionTypes
        {
            get { return _connectionTypes; }
            set
            {
                var old = ConnectionTypes;
                ConnectionTypes.CollectionChanged -= (sender, e) => CollectionChanged(sender, e, "ConnectionTypes");
                _connectionTypes = value;
                ConnectionTypes.CollectionChanged += (sender, e) => CollectionChanged(sender, e, "ConnectionTypes");
                notifyCombinedChanged(Change.Edit, "ConnectionTypes", this, value, old);
            }
        }
        public ObservableCollection<TECElectricalMaterial> ConduitTypes
        {
            get { return _conduitTypes; }
            set
            {
                var old = ConduitTypes;
                ConduitTypes.CollectionChanged -= (sender, e) => CollectionChanged(sender, e, "ConduitTypes");
                _conduitTypes = value;
                ConduitTypes.CollectionChanged += (sender, e) => CollectionChanged(sender, e, "ConduitTypes");
                notifyCombinedChanged(Change.Edit, "ConduitTypes", this, value, old);
            }
        }
        public ObservableCollection<TECCost> AssociatedCosts
        {
            get { return _associatedCosts; }
            set
            {
                var old = AssociatedCosts;
                AssociatedCosts.CollectionChanged -= (sender, e) => CollectionChanged(sender, e, "AssociatedCosts");
                AssociatedCosts.CollectionChanged -= ScopeChildren_CollectionChanged;
                _associatedCosts = value;
                AssociatedCosts.CollectionChanged += (sender, e) => CollectionChanged(sender, e, "AssociatedCosts");
                AssociatedCosts.CollectionChanged += ScopeChildren_CollectionChanged;
                notifyCombinedChanged(Change.Edit, "AssociatedCosts", this, value, old);
            }
        }
        public ObservableCollection<TECLabeled> Tags
        {
            get { return _tags; }
            set
            {
                var old = Tags;
                Tags.CollectionChanged -= (sender, e) => CollectionChanged(sender, e, "Tags");
                Tags.CollectionChanged -= ScopeChildren_CollectionChanged;
                _tags = value;
                Tags.CollectionChanged += (sender, e) => CollectionChanged(sender, e, "Tags");
                Tags.CollectionChanged += ScopeChildren_CollectionChanged;
                notifyCombinedChanged(Change.Edit, "Tags", this, value, old);
            }
        }

        public SaveableMap PropertyObjects
        {
            get { return propertyObjects(); }
        }
        public SaveableMap LinkedObjects
        {
            get { return linkedObjects(); }
        }

        public Action<TECObject> ScopeChildRemoved;

        public TECCatalogs() : base(Guid.NewGuid())
        {
            instantiateCOllections();
        }

        private void instantiateCOllections()
        {
            _conduitTypes = new ObservableCollection<TECElectricalMaterial>();
            _connectionTypes = new ObservableCollection<TECElectricalMaterial>();
            _associatedCosts = new ObservableCollection<TECCost>();
            _panelTypes = new ObservableCollection<TECPanelType>();
            _controllerTypes = new ObservableCollection<TECControllerType>();
            _ioModules = new ObservableCollection<TECIOModule>();
            _devices = new ObservableCollection<TECDevice>();
            _valves = new ObservableCollection<TECValve>();
            _manufacturers = new ObservableCollection<TECManufacturer>();
            _tags = new ObservableCollection<TECLabeled>();

            registerInitialCollectionChanges();
        }
        private void registerInitialCollectionChanges()
        {
            ConduitTypes.CollectionChanged += (sender, e) => CollectionChanged(sender, e, "ConduitTypes");
            ConnectionTypes.CollectionChanged += (sender, e) => CollectionChanged(sender, e, "ConnectionTypes");
            AssociatedCosts.CollectionChanged += (sender, e) => CollectionChanged(sender, e, "AssociatedCosts");
            PanelTypes.CollectionChanged += (sender, e) => CollectionChanged(sender, e, "PanelTypes");
            ControllerTypes.CollectionChanged += (sender, e) => CollectionChanged(sender, e, "ControllerTypes");
            IOModules.CollectionChanged += (sender, e) => CollectionChanged(sender, e, "IOModules");
            Devices.CollectionChanged += (sender, e) => CollectionChanged(sender, e, "Devices");
            Valves.CollectionChanged += (sender, e) => CollectionChanged(sender, e, "Valves");
            Manufacturers.CollectionChanged += (sender, e) => CollectionChanged(sender, e, "Manufacturers");
            Tags.CollectionChanged += (sender, e) => CollectionChanged(sender, e, "Tags");

            AssociatedCosts.CollectionChanged += ScopeChildren_CollectionChanged;
            Tags.CollectionChanged += ScopeChildren_CollectionChanged;
        }

        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e, string propertyName)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    notifyCombinedChanged(Change.Add, propertyName, this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    notifyCombinedChanged(Change.Remove, propertyName, this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                //Change order
            }
        }

        private void ScopeChildren_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (TECObject item in e.OldItems)
                {
                    ScopeChildRemoved?.Invoke(item);
                }
            }
        }
        private SaveableMap propertyObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(this.IOModules, "IOModules");
            saveList.AddRange(this.Devices, "Devices");
            saveList.AddRange(this.Valves, "Valves");
            saveList.AddRange(this.Manufacturers, "Manufacturers");
            saveList.AddRange(this.PanelTypes, "PanelTypes");
            saveList.AddRange(this.ControllerTypes, "ControllerTypes");
            saveList.AddRange(this.ConnectionTypes, "ConnectionTypes");
            saveList.AddRange(this.ConduitTypes, "ConduitTypes");
            saveList.AddRange(this.AssociatedCosts, "AssociatedCosts");
            saveList.AddRange(this.Tags, "Tags");
            return saveList;
        }
        private SaveableMap linkedObjects()
        {
            SaveableMap relatedList = new SaveableMap();
            return relatedList;
        }

        public void Unionize(TECCatalogs catalogToAdd)
        {
            unionizeScope(this.Devices, catalogToAdd.Devices);
            unionizeScope(this.Manufacturers, catalogToAdd.Manufacturers);
            unionizeScope(this.ConnectionTypes, catalogToAdd.ConnectionTypes);
            unionizeScope(this.ConduitTypes, catalogToAdd.ConduitTypes);
            unionizeScope(this.PanelTypes, catalogToAdd.PanelTypes);
            unionizeScope(this.IOModules, catalogToAdd.IOModules);
            unionizeScope(this.Tags, catalogToAdd.Tags);
            unionizeScope(this.AssociatedCosts, catalogToAdd.AssociatedCosts);

        }
        private static void unionizeScope<T>(ObservableCollection<T> bidItems, ObservableCollection<T> templateItems)
        {
            ObservableCollection<T> itemsToRemove = new ObservableCollection<T>();

            foreach (T templateItem in templateItems)
            {
                foreach (T item in bidItems)
                {
                    if ((item as TECObject).Guid == (templateItem as TECObject).Guid)
                    {
                        itemsToRemove.Add(item);
                    }
                }
            }
            foreach (T item in itemsToRemove)
            {
                bidItems.Remove(item);
            }
            foreach (T item in templateItems)
            {
                bidItems.Add(item);
            }
        }
    }
}
