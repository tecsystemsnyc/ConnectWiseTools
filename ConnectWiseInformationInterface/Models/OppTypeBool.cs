using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectWiseInformationInterface.Models
{
    public class OppTypeBool : INotifyPropertyChanged
    {
        private bool _include;
        public bool Include
        {
            get
            {
                return _include;
            }
            set
            {
                if (Include != value)
                {
                    _include = value;
                    notifyPropertyChanged("Include");
                }
            }
        }

        public string Name { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public OppTypeBool(string name)
        {
            this.Name = name;
            _include = false;
        }

        private void notifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
