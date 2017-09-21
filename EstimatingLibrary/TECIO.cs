using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public enum IOType
    {
        AI,
        AO,
        DI,
        DO,
        UI,
        UO,
        BACnetMSTP,
        BACnetIP,
        LonWorks,
        ModbusTCP,
        ModbusRTU,
    }

    public class TECIO : TECObject
    {
        public static List<IOType> PointIO = new List<IOType>()
        {
            IOType.AI, IOType.AO, IOType.DI, IOType.DO, IOType.BACnetMSTP, IOType.BACnetIP, IOType.LonWorks, IOType.ModbusTCP, IOType.ModbusRTU
        };
        public static List<IOType> NetworkIO = new List<IOType>()
        {
            IOType.BACnetMSTP, IOType.BACnetIP, IOType.LonWorks, IOType.ModbusTCP, IOType.ModbusRTU
        };
        public static List<IOType> UniversalIO = new List<IOType>()
        {
            IOType.UI, IOType.UO
        };

        #region Properties
        private IOType _type;
        public IOType Type
        {
            get { return _type; }
            set
            {
                var old = Type;
                _type = value;
                notifyCombinedChanged(Change.Edit, "Type", this, value, old);
            }
        }


        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                var old = Quantity;
                _quantity = value;
                notifyCombinedChanged(Change.Edit, "Quantity", this, value, old);
            }
        }

        #endregion

        public TECIO(Guid guid, IOType type) : base(guid)
        {
            _type = type;
            _quantity = 1;
        }

        public TECIO(IOType type) : this(Guid.NewGuid(), type) { }

        public TECIO(TECIO ioSource) : this(ioSource.Type)
        {
            _quantity = ioSource.Quantity;
        }
    }

    public class IOCollection
    {
        private Dictionary<IOType, TECIO> ioDictionary;

        public IOCollection()
        {
            ioDictionary = new Dictionary<IOType, TECIO>();
        }
        public IOCollection(IEnumerable<TECIO> io) : this()
        {
            AddIO(io);
        }
        public IOCollection(IOCollection collection) : this()
        {
            foreach(TECIO io in collection.ListIO())
            {
                AddIO(io);
            }
        }

        public List<TECIO> ListIO()
        {
            List<TECIO> list = new List<TECIO>();
            foreach(KeyValuePair<IOType, TECIO> pair in ioDictionary)
            {
                list.Add(pair.Value);
            }
            return list;
        }
        public void AddIO(IOType type)
        {
            if (ioDictionary.ContainsKey(type))
            {
                ioDictionary[type].Quantity++;
            }
            else
            {
                TECIO io = new TECIO(type);
                ioDictionary.Add(type, io);
            }
        }
        public void AddIO(TECIO io)
        {
            if (ioDictionary.ContainsKey(io.Type))
            {
                ioDictionary[io.Type].Quantity += io.Quantity;
            }
            else
            {
                ioDictionary.Add(io.Type, new TECIO(io));
            }
        }
        public void AddIO(IEnumerable<TECIO> ioList)
        {
            foreach(TECIO io in ioList)
            {
                AddIO(io);
            }
        }
        public void RemoveIO(IOType type)
        {
            if (ioDictionary.ContainsKey(type))
            {
                TECIO io = ioDictionary[type];
                io.Quantity--;
                if (io.Quantity < 1)
                {
                    ioDictionary.Remove(io.Type);
                }
            }
            else
            {
                throw new ObjectDisposedException("IOCollection does not contain type.");
            }
        }
        public void RemoveIO(TECIO io)
        {
            if (ioDictionary.ContainsKey(io.Type) && ioDictionary[io.Type].Quantity >= io.Quantity)
            {
                TECIO collectionIO = ioDictionary[io.Type];
                collectionIO.Quantity -= io.Quantity;
                if (collectionIO.Quantity < 1)
                {
                    ioDictionary.Remove(collectionIO.Type);
                }
            }
            else
            {
                throw new ObjectDisposedException("IOCollection does not contain enough type.");
            }
        }
        public void RemoveIO(IEnumerable<TECIO> ioList)
        {
            foreach(TECIO io in ioList)
            {
                RemoveIO(io);
            }
        }

        public static IOCollection operator +(IOCollection left, IOCollection right)
        {
            IOCollection newCollection = new IOCollection(left);
            newCollection.AddIO(right.ListIO());
            return newCollection;
        }
        public static IOCollection operator -(IOCollection left, IOCollection right)
        {
            IOCollection newCollection = new IOCollection(left);
            newCollection.RemoveIO(right.ListIO());
            return newCollection;
        }
    }
}
