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
                var temp = this.Copy();
                IO.CollectionChanged -= IO_CollectionChanged;
                _io = value;
                NotifyPropertyChanged("IO", temp, this);
                IO.CollectionChanged += IO_CollectionChanged;
            }
        }
#endregion

        public TECControllerType(Guid guid, TECManufacturer manufacturer) : base(guid, manufacturer) {
            _type = CostType.TEC;
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
        private void IO_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (Object item in e.NewItems)
                {
                    if (item is TECIO)
                    {
                        NotifyPropertyChanged("Add", this, (item as TECObject).Copy());
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (Object item in e.OldItems)
                {
                    if (item is TECIO)
                    {
                        NotifyPropertyChanged("Remove", this, (item as TECObject).Copy());
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
