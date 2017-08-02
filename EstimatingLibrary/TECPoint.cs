using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{

    public enum PointTypes { AI = 1, AO, BI, BO, Serial };

    public class TECPoint : TECLabeled, INotifyPointChanged
    {
        #region Properties
        private PointTypes _type;
        private int _quantity;

        public event Action<List<TECPoint>> PointChanged;

        public PointTypes Type
        {
            get { return _type; }
            set
            {
                var old = Type;
                _type = value;
                // Call RaisePropertyChanged whenever the property is updated
                NotifyPropertyChanged(Change.Edit, "Type", this, value, old);
            }
        }
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                var old = Quantity;
                _quantity = value;
                NotifyPropertyChanged(Change.Edit, "Quantity", this, value, old);

            }
        }
        public string TypeString
        {
            get { return convertTypeToString(Type); }
            set
            {
                if (convertStringToType(value) != 0)
                {
                    Type = convertStringToType(value);
                }
                else
                {
                    string message = "TypeString set failed in TECPoint. Unrecognized TypeString.";
                    throw new InvalidCastException(message);
                }
            }
        }

        public int PointNumber
        {
            get
            {
                return Quantity;
            }
        }
        #endregion //Properties

        #region Constructors
        public TECPoint(Guid guid) : base(guid) { }
        public TECPoint() : this(Guid.NewGuid()) { }

        public TECPoint(TECPoint pointSource) : this()
        {
            _type = pointSource.Type;
            _label = pointSource.Label;
            _quantity = pointSource.Quantity;
        }
        #endregion //Constructors

        #region Methods
        #region Conversion Methods
        public static PointTypes convertStringToType(string type)
        {
            switch (type.ToUpper())
            {
                case "AI": return PointTypes.AI;
                case "AO": return PointTypes.AO;
                case "BI": return PointTypes.BI;
                case "BO": return PointTypes.BO;
                case "SERIAL": return PointTypes.Serial;
                default: return 0;
            }
        }
        public static string convertTypeToString(PointTypes type)
        {
            switch (type)
            {
                case PointTypes.AI: return "AI";
                case PointTypes.AO: return "AO";
                case PointTypes.BI: return "BI";
                case PointTypes.BO: return "BO";
                case PointTypes.Serial: return "SERIAL";
                default: return "";
            }
        }

        public void NotifyPointChanged(List<TECPoint> points)
        {
            throw new NotImplementedException();
        }
        #endregion //Conversion Methods
        #endregion

    }
}
