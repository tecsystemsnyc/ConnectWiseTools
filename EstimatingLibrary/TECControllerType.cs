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
        #region Properties
        private ObservableCollection<TECIO> _io;

        public ObservableCollection<TECIO> IO
        {
            get { return _io; }
            set
            {
                var old = IO;
                IO.CollectionChanged -= (sender, args) => IO_CollectionChanged(sender, args, "IO");
                _io = value;
                NotifyPropertyChanged(Change.Edit,"IO", this, value, old);
                IO.CollectionChanged += (sender, args) => IO_CollectionChanged(sender, args, "IO");
            }
        }
        #endregion

        public TECControllerType(Guid guid, TECManufacturer manufacturer) : base(guid, manufacturer) {
            _type = CostType.TEC;
            _io = new ObservableCollection<TECIO>();
            IO.CollectionChanged += (sender, args) => IO_CollectionChanged(sender, args, "IO");
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
        }

        #region Methods
        private void IO_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e, string propertyName)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (Object item in e.NewItems)
                {
                    if (item is TECIO)
                    {
                        NotifyPropertyChanged(Change.Add, propertyName, this, item);
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (Object item in e.OldItems)
                {
                    if (item is TECIO)
                    {
                        NotifyPropertyChanged(Change.Remove, propertyName, this, item);
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

        public override object Copy()
        {
            var outCost = new TECControllerType(Manufacturer);
            outCost.copyPropertiesFromHardware(this);
            outCost._guid = this.Guid;
            return outCost;
        }
        #endregion

    }
}
