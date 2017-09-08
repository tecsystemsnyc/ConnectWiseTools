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
        AI = 1,
        AO,
        DI,
        DO,
        UI,
        UO,
        BACnetMSTP,
        BACnetIP,
        LonWorks,
        ModbusTCP,
        ModbusRTU
    }
    public class TECIO : TECObject, ISaveable
    {
        #region Properties

        private IOType _type;
        public IOType Type
        {
            get { return _type; }
            set
            {
                var old = Type;
                _type = value;
                NotifyCombinedChanged(Change.Edit, "Type", this, value, old);
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
                NotifyCombinedChanged(Change.Edit, "Quantity", this, value, old);
            }
        }

        private TECIOModule _ioModule;
        public TECIOModule IOModule
        {
            get { return _ioModule; }
            set
            {
                var old = IOModule;
                _ioModule = value;
                NotifyCombinedChanged(Change.Edit, "IOModule", this, value, old);
                //NotifyCombinedChanged("ObjectPropertyChanged", temp, oldNew, typeof(TECIO), typeof(TECIOModule));
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
            _ioModule = ioSource.IOModule;
        }

        public static IOType convertStringToType(string type)
        {
            switch (type.ToUpper())
            {
                case "AI": return IOType.AI;
                case "AO": return IOType.AO;
                case "DI": return IOType.DI;
                case "DO": return IOType.DO;
                case "UI": return IOType.UI;
                case "UO": return IOType.UO;
                case "BACNETMSTP": return IOType.BACnetMSTP;
                case "BACNETIP": return IOType.BACnetIP;
                case "LONWORKS": return IOType.LonWorks;
                case "MODBUSTCP": return IOType.ModbusTCP;
                case "MODBUSRTU": return IOType.ModbusRTU;

                default: return 0;
            }
        }

        public static string convertTypeToString(IOType type)
        {
            switch (type)
            {
                case IOType.AI: return "AI";
                case IOType.AO: return "AO";
                case IOType.DI: return "DI";
                case IOType.DO: return "DO";
                case IOType.UI: return "UI";
                case IOType.UO: return "UO";
                case IOType.BACnetMSTP: return "BACnetMSTP";
                case IOType.BACnetIP: return "BACnetIP";
                case IOType.LonWorks: return "LonWorks";
                case IOType.ModbusTCP: return "ModbusTCP";
                case IOType.ModbusRTU: return "ModbusRTU";

                default: return "";
            }
        }

        private SaveableMap saveObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.Add(this.IOModule, "IOModule");
            return saveList;
        }

        private SaveableMap relatedObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.Add(this.IOModule, "IOModule");
            return saveList;
        }
    }
}
