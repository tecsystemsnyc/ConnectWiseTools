using ConnectWiseDotNetSDK.ConnectWise.Client.Sales.Model;
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

        public OpportunityType Type { get; } 
        public string Name
        {
            get { return Type.Description; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public OppTypeBool(OpportunityType oppType)
        {
            this.Type = oppType;
            _include = true;
        }

        private void notifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
