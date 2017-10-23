using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.Interfaces
{
    public interface IUserConfirmable
    {
        bool? Show(string message);
    }
}
