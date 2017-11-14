using EstimatingLibrary.Utilities;
using System;

namespace EstimatingLibrary.Interfaces
{
    public interface INotifyCostChanged
    {
        event Action<CostBatch> CostChanged;
        CostBatch CostBatch { get; }
    }
}
