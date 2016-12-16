using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public enum ConnectionType
    {
        TwoC18 = 1, ThreeC18, FourC18, SixC18,
        WireTHHN14, WireTHHN12,
        Cat6, Fiber
    }

    public static class TECConnectionType
    {
        public static ConnectionType convertStringToType(string type)
        {
            switch (type.ToUpper())
            {
                case "TWOC18": return ConnectionType.TwoC18;
                case "THREEC18": return ConnectionType.ThreeC18;
                case "FOURC18": return ConnectionType.FourC18;
                case "SIXC18": return ConnectionType.SixC18;

                case "WIRETHHN14": return ConnectionType.WireTHHN14;
                case "WIRETHHN12": return ConnectionType.WireTHHN12;

                case "CAT6": return ConnectionType.Cat6;
                case "FIBER": return ConnectionType.Fiber;

                default: return 0;
            }
        }

        public static string convertTypeToString(ConnectionType type)
        {
            switch (type)
            {
                case ConnectionType.TwoC18: return "TwoC18";
                case ConnectionType.ThreeC18: return "ThreeC18";
                case ConnectionType.FourC18: return "FourC18";
                case ConnectionType.SixC18: return "SixC18";

                case ConnectionType.WireTHHN12: return "WireTHHN12";
                case ConnectionType.WireTHHN14: return "WireTHHN14";

                case ConnectionType.Cat6: return "Cat6";
                case ConnectionType.Fiber: return "Fiber";

                default: return "";
            }
        }
    }
}
