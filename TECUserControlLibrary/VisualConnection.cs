using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary
{
    public class VisualConnection : ViewModelBase
    {
        private TECConnection _connection;
        public TECConnection Connection
        {
            get { return _connection; }
            set
            {
                _connection = value;
                RaisePropertyChanged("Connection");
            }
        }

        private TECVisualScope _scope1;
        public TECVisualScope Scope1
        {
            get { return _scope1; }
            set
            {
                _scope1 = value;
                RaisePropertyChanged("Scope1");
            }
        }
        private TECVisualScope _scope2;
        public TECVisualScope Scope2
        {
            get { return _scope2; }
            set
            {
                _scope2 = value;
                RaisePropertyChanged("Scope2");
            }
        }
        
        public VisualConnection()
        {

        }

        public VisualConnection(TECVisualScope vs1, TECVisualScope vs2, TECConnection connection)
        {
            Scope1 = vs1;
            Scope2 = vs2;
            Connection = connection;

            Scope1.PropertyChanged += scopeChanged;
        }

        private void scopeChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "X" || e.PropertyName == "Y")
            {
                Connection.Length = UtilitiesMethods.getLength(Scope1, Scope2, 1);
            }
        }
    }
}
