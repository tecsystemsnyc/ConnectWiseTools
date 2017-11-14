using System;

namespace EstimatingLibrary.Interfaces
{
    public interface INotifyPointChanged
    {
        event Action<int> PointChanged;
        int PointNumber { get; }
    }
}
