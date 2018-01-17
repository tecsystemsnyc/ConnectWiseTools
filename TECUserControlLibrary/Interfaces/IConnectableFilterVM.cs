using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TECUserControlLibrary.Interfaces
{
    public interface IConnectableFilterVM
    {
        bool FilterByIO { get; set; }
        bool IncludeConnected { get; set; }
        ReadOnlyCollection<IOType> NetworkIOTypes { get; }
        string SearchQuery { get; set; }
        IOType SelectedIOType { get; set; }
    }
}
