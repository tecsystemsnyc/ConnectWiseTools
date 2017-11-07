using EstimatingLibrary.Interfaces;
using System;

namespace EstimatingLibrary
{
    public class TECPoint : TECLabeled, INotifyPointChanged, ITypicalable
    {
        #region Properties
        private IOType _type;
        private int _quantity;

        public event Action<int> PointChanged;

        public IOType Type
        {
            get { return _type; }
            set
            {
                if (TECIO.PointIO.Contains(value) || TECIO.NetworkIO.Contains(value))
                {
                    var old = Type;
                    _type = value;
                    // Call raisePropertyChanged whenever the property is updated
                    notifyCombinedChanged(Change.Edit, "Type", this, value, old);
                }
                else
                {
                    throw new InvalidOperationException("TECPoint cannot be non PointIO.");
                }
            }
        }
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                var old = Quantity;
                if (!IsTypical)
                {
                    PointChanged?.Invoke(old - value);
                }
                _quantity = value;
                notifyCombinedChanged(Change.Edit, "Quantity", this, value, old);

            }
        }

        public int PointNumber
        {
            get
            {
                return Quantity;
            }
        }
        
        public bool IsTypical { get; private set; }
        #endregion //Properties

        #region Constructors
        public TECPoint(Guid guid, bool isTypical) : base(guid) { IsTypical = isTypical; }
        public TECPoint(bool isTypical) : this(Guid.NewGuid(), isTypical) { }

        public TECPoint(TECPoint pointSource, bool isTypical) : this(isTypical)
        {
            _type = pointSource.Type;
            _label = pointSource.Label;
            _quantity = pointSource.Quantity;
        }
        #endregion //Constructors

        #region Methods
        public void notifyPointChanged(int numPoints)
        {
            if (!IsTypical)
            {
                PointChanged?.Invoke(numPoints);
            }
        }
        #endregion

    }
}
