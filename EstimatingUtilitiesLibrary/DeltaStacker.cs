using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using EstimatingLibrary;

namespace EstimatingUtilitiesLibrary
{
    public class DeltaStacker
    {

        public DeltaStacker(ChangeWatcher changeWatcher)
        {
            changeWatcher.Changed += handleChange;
        }

        private void handleChange(object sender, PropertyChangedEventArgs e)
        {
            var args = e as PropertyChangedExtendedEventArgs<object>;
            if(args != null)
            {

            }
        }
    }
}
