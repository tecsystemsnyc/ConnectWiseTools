using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECVisualConnection : TECObject
    {
        #region Properties
        private ObservableCollection<TECConnection> _connections;
        public ObservableCollection<TECConnection> Connections
        {
            get { return _connections; }
            set
            {
                _connections = value;
                RaisePropertyChanged("Connections");
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
        #endregion

        #region Constructors
        public TECVisualConnection() : base(Guid.NewGuid())
        {

        }
        public TECVisualConnection(TECVisualScope vs1, TECVisualScope vs2) : this()
        {
            Scope1 = vs1;
            Scope2 = vs2;
            Scope1.PropertyChanged += scopeChanged;
        }
        #endregion

        #region Methods
        private void scopeChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "X" || e.PropertyName == "Y")
            {
                foreach (TECConnection connection in Connections)
                {
                    connection.Length = getLength(Scope1, Scope2, 1);
                }

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
        #endregion

    }
}
