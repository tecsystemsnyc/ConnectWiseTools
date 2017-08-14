using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary.Interfaces
{
    public interface INotifyTECChanged
    {
        event Action<TECChangedEventArgs> TECChanged;
    }
}
