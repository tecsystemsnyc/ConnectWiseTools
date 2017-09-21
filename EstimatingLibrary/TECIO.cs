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

    public class TECIO : TECObject, ISaveable
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

        public SaveableMap SaveObjects
        {
            get { return saveObjects(); }
        }
        public SaveableMap RelatedObjects
        {
            get { return relatedObjects(); }
        }

        #endregion

        public TECIO(Guid guid) : base(guid)
        {
            _quantity = 1;
        }

        public TECIO() : this(Guid.NewGuid()) { }

        public TECIO(TECIO ioSource) : this()
        {
            _quantity = ioSource.Quantity;
            _type = ioSource.Type;
        }

        private SaveableMap saveObjects()
        {
            SaveableMap saveList = new SaveableMap();
            return saveList;
        }

        private SaveableMap relatedObjects()
        {
            SaveableMap saveList = new SaveableMap();
            return saveList;
        }
    }
}
