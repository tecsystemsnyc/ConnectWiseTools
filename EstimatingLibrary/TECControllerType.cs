using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECControllerType : TECHardware
    {
        private const CostType COST_TYPE = CostType.TEC;

        #region Properties
        private ObservableCollection<TECIO> _io;
        private ObservableCollection<TECIOModule> _ioModules;


        public ObservableCollection<TECIO> IO
        {
            get { return _io; }
            set
            {
                var old = IO;
                IO.CollectionChanged -= (sender, args) => collectionChanged(sender, args, "IO");
                _io = value;
                notifyCombinedChanged(Change.Edit,"IO", this, value, old);
                IO.CollectionChanged += (sender, args) => collectionChanged(sender, args, "IO");
            }
        }
        public ObservableCollection<TECIOModule> IOModules
        {
            get { return _ioModules; }
            set
            {
                var old = IOModules;
                IOModules.CollectionChanged -= (sender, args) => collectionChanged(sender, args, "IOModules");
                _ioModules = value;
                IOModules.CollectionChanged += (sender, args) => collectionChanged(sender, args, "IOModules");
                notifyCombinedChanged(Change.Edit, "IOModules", this, value, old);
            }
        }
        #endregion

        public TECControllerType(Guid guid, TECManufacturer manufacturer) : base(guid, manufacturer, COST_TYPE)
        {
            _io = new ObservableCollection<TECIO>();
            _ioModules = new ObservableCollection<TECIOModule>();
            IO.CollectionChanged += (sender, args) => collectionChanged(sender, args, "IO");
            IOModules.CollectionChanged += (sender, args) => collectionChanged(sender, args, "IOModules");

        }
        public TECControllerType(TECManufacturer manufacturer) : this(Guid.NewGuid(), manufacturer) { }
        public TECControllerType(TECControllerType typeSource) : this(typeSource.Manufacturer)
        {
            copyPropertiesFromHardware(typeSource);
            foreach (TECIO io in typeSource.IO)
            {
                TECIO ioToAdd = new TECIO(io);
                _io.Add(new TECIO(io));
            }
            foreach (TECIOModule module in typeSource.IOModules)
            {
                _ioModules.Add(module);
            }
        }

        #region Methods
        private void collectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e, string propertyName)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (Object item in e.NewItems)
                {
                    notifyCombinedChanged(Change.Add, propertyName, this, item);
                    if(item is INotifyCostChanged cost)
                    {
                        notifyCostChanged(cost.CostBatch);
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (Object item in e.OldItems)
                {
                    notifyCombinedChanged(Change.Remove, propertyName, this, item);
                    if (item is INotifyCostChanged cost)
                    {
                        notifyCostChanged(cost.CostBatch * -1);
                    }
                }
            }
        }
        
        public List<IOType> getUniqueIO()
        {
            var outList = new List<IOType>();

            foreach (TECIO io in this.IO)
            {
                if (!outList.Contains(io.Type))
                {
                    outList.Add(io.Type);
                }
            }
            return outList;
        }
        public int NumberOfIOType(IOType ioType)
        {
            int outNum = 0;

            foreach (TECIO type in IO)
            {
                if (type.Type == ioType)
                {
                    outNum = type.Quantity;
                }
            }

            return outNum;
        }

        protected override SaveableMap saveObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.saveObjects());
            saveList.AddRange(this.IO, "IO");
            saveList.AddRange(this.IOModules, "IOModules");
            return saveList;
        }

        protected override SaveableMap relatedObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.relatedObjects());
            saveList.AddRange(this.IOModules, "IOModules");
            return saveList;
        }
        #endregion

    }
}
