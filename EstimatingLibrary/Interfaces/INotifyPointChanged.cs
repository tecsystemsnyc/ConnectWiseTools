using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary.Interfaces
{
    public interface INotifyPointChanged
    {
        event Action<int> PointChanged;
        int PointNumber { get; }
    }
}
