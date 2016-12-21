using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECVisualConnection : TECObject
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
        
        public TECVisualConnection()
        {

        }

        public TECVisualConnection(TECVisualScope vs1, TECVisualScope vs2, TECConnection connection)
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
                Connection.Length = getLength(Scope1, Scope2, 1);
            }
        }

        double getLength(TECVisualScope scope1, TECVisualScope scope2, double scale)
        {
            var length = Math.Pow((Math.Pow((scope1.X - scope2.X), 2) + Math.Pow((scope1.Y - scope2.Y), 2)), 0.5) * scale;

            return length;
        }

        public override object Copy()
        {
            throw new NotImplementedException();
        }
    }
}
