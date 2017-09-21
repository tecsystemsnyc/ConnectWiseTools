using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECIOModule : TECHardware
    {
        private const CostType COST_TYPE = CostType.TEC;

        private ObservableCollection<TECIO> _io;
        public ObservableCollection<TECIO> IO
        {
            get { return _io; }
            set
            {
                var old = IO;
                IO.CollectionChanged -= (sender, args) => collectionChanged(sender, args, "IO");
                _io = value;
                IO.CollectionChanged += (sender, args) => collectionChanged(sender, args, "IO");
                notifyCombinedChanged(Change.Edit, "IO", this, value, old);
            }
        }
        
        public TECIOModule(Guid guid, TECManufacturer manufacturer) : base(guid, manufacturer, COST_TYPE)
        {
            _io = new ObservableCollection<TECIO>();
        }
        public TECIOModule(TECManufacturer manufacturer) : this(Guid.NewGuid(), manufacturer) { }
        public TECIOModule(TECIOModule ioModuleSource) : this(ioModuleSource.Manufacturer)
        {
            copyPropertiesFromHardware(this);
            _io = ioModuleSource.IO;
        }

        private void collectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e, string propertyName)
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
        }

    }
}
