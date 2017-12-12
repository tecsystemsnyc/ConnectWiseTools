using System;
using System.Collections.Generic;

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
            IOType.AI, IOType.AO, IOType.DI, IOType.DO
        };
        public static List<IOType> NetworkIO = new List<IOType>()
        {
            IOType.BACnetMSTP, IOType.BACnetIP, IOType.LonWorks, IOType.ModbusTCP, IOType.ModbusRTU
        };
        public static List<IOType> UniversalIO = new List<IOType>()
        {
            IOType.UI, IOType.UO
        };

        public static IOType GetUniversalType(IOType type)
        {
            if (PointIO.Contains(type))
            {
                if (type == IOType.AI || type == IOType.DI)
                {
                    return IOType.UI;
                }
                else if (type == IOType.AO || type == IOType.DO)
                {
                    return IOType.UO;
                }
                else
                {
                    throw new NotImplementedException("PointIO type not recognized.");
                }
            }
            else
            {
                throw new InvalidOperationException("Universal type must come from point type.");
            }
        }

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
        public bool Contains(IOType type)
        {
            bool containsExact = ioDictionary.ContainsKey(type);
            if (containsExact)
            {
                return true;
            }
            else
            {
                if (TECIO.PointIO.Contains(type))
                {
                    return ioDictionary.ContainsKey(TECIO.GetUniversalType(type));
                }
                else
                {
                    return false;
                }
            }
        }
        public bool Contains(TECIO io)
        {
            if (TECIO.PointIO.Contains(io.Type))
            {
                IOType universalType = TECIO.GetUniversalType(io.Type);
                int quantity = ioDictionary.ContainsKey(io.Type) ? ioDictionary[io.Type].Quantity : 0;
                quantity += ioDictionary.ContainsKey(universalType) ? ioDictionary[universalType].Quantity : 0;
                return quantity >= io.Quantity;
            }
            else
            {
                if (ioDictionary.ContainsKey(io.Type))
                {
                    TECIO collectionIO = ioDictionary[io.Type];
                    return (collectionIO.Quantity >= io.Quantity);
                }
                else
                {
                    return false;
                }
            }
        }
        public bool Contains(IEnumerable<TECIO> io)
        {
            //Normalize collections
            IOCollection thisCollection = new IOCollection(this);
            IOCollection ioCollection = new IOCollection(io);

            foreach (TECIO ioToCheck in ioCollection.ListIO())
            {
                if (thisCollection.Contains(ioToCheck))
                {
                    thisCollection.RemoveIO(ioToCheck);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        public bool Contains(IOCollection io)
        {
            return (this.Contains(io.ListIO()));
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
                if (TECIO.PointIO.Contains(type))
                {
                    IOType universalType = TECIO.GetUniversalType(type);
                    if (ioDictionary.ContainsKey(universalType))
                    {
                        TECIO universalIO = ioDictionary[universalType];
                        universalIO.Quantity--;
                        if (universalIO.Quantity < 1)
                        {
                            ioDictionary.Remove(universalIO.Type);
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("IOCollection does not contain IOType.");
                    }
                }
                else
                {
                    throw new InvalidOperationException("IOCollection does not contain IOType.");
                }
            }
        }
        public void RemoveIO(TECIO io)
        {
            if (this.Contains(io))
            {
                TECIO sameIO = ioDictionary.ContainsKey(io.Type) ? ioDictionary[io.Type] : null;
                if (sameIO?.Quantity >= io.Quantity)
                {
                    sameIO.Quantity -= io.Quantity;
                }
                else
                {
                    int quantityRemaining = io.Quantity;
                    if (sameIO != null)
                    {
                        quantityRemaining -= sameIO.Quantity;
                        sameIO.Quantity = 0;
                    }

                    IOType universalType = TECIO.GetUniversalType(io.Type);
                    TECIO universalIO = ioDictionary.ContainsKey(universalType) ? ioDictionary[universalType] : null;
                    if (universalIO?.Quantity >= quantityRemaining)
                    {
                        universalIO.Quantity -= quantityRemaining;
                        if (universalIO.Quantity < 1)
                        {
                            ioDictionary.Remove(universalIO.Type);
                        }
                    }
                    else
                    {
                        throw new DataMisalignedException("ContainsIO passed but RemoveIO failed.");
                    }
                }
                
                if (sameIO?.Quantity < 1)
                {
                    ioDictionary.Remove(sameIO.Type);
                }
            }
            else
            {
                throw new InvalidOperationException("IOCollection does not contain enough IO.");
            }
        }
        public void RemoveIO(IEnumerable<TECIO> ioList)
        {
            if (this.Contains(ioList))
            {
                foreach (TECIO io in ioList)
                {
                    RemoveIO(io);
                }
            }
            else
            {
                throw new InvalidOperationException("IOCollection does not contain enough IO.");
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

        public static bool IOTypesMatch(IOCollection collection1, IOCollection collection2)
        {
            foreach(KeyValuePair<IOType, TECIO> pair in collection1.ioDictionary)
            {
                if (!collection2.Contains(pair.Key)) return false;
            }
            foreach(KeyValuePair<IOType, TECIO> pair in collection2.ioDictionary)
            {
                if (!collection1.Contains(pair.Key)) return false;
            }
            return true;
        }
    }
}
