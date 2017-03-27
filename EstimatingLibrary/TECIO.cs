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
    public class TECIO : TECObject
    {
        #region Properties
        private Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        private IOType _type;
        public IOType Type
        {
            get { return _type; }
            set
            {
                var temp = this.Copy();
                _type = value;
                NotifyPropertyChanged("Type", temp, this);
            }
        }

        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                var temp = this.Copy();
                _quantity = value;
                NotifyPropertyChanged("Quantity", temp, this);
            }
        }

        private TECIOModule _ioModule;
        public TECIOModule IOModule
        {
            get { return _ioModule; }
            set
            {
                var temp = this.Copy();
                _ioModule = value;
                NotifyPropertyChanged("IOModule", temp, this);
            }
        }

        #endregion
        
        public TECIO(Guid guid)
        {
            _guid = guid;
            _quantity = 1;
        }

        public TECIO() : this(Guid.NewGuid()) { }

        public TECIO(TECIO ioSource) : this()
        {
            _quantity = ioSource.Quantity;
            _type = ioSource.Type;
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

        public override object Copy()
        {
            var outIO = new TECIO();
            outIO._type = Type;
            outIO._quantity = Quantity;
            return outIO;
        }
    }
}
