using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public enum IOType
    {
        AI = 0,
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
    class TECIO
    {
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
                case "BACnetMSTP": return IOType.BACnetMSTP;
                case "BACnetIP": return IOType.BACnetIP;
                case "LonWorks": return IOType.LonWorks;
                case "ModbusTCP": return IOType.ModbusTCP;
                case "ModbusRTU": return IOType.ModbusRTU;

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
    }
}
