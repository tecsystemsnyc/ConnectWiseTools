using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECVisualScope : TECObject
    {
        private TECScope _scope;
        private double _x;
        private double _y;
        

        public TECScope Scope
        {
            get { return _scope; }
            set
            {
                _scope = value;
                RaisePropertyChanged("Scope");
            }
        }
        public double X
        {
            get { return _x; }
            set
            {
                var temp = this.Copy();
                _x = value;
                NotifyPropertyChanged("X", temp, this);
            }
        }
        public double Y
        {
            get { return _y; }
            set
            {
                var temp = this.Copy();
                _y = value;
                NotifyPropertyChanged("Y", temp, this);
            }
        }

        public Guid Guid;

        public TECVisualScope(TECScope scope, double x, double y)
        {
            Guid = Guid.NewGuid();
            Scope = scope;
            _x = x;
            _y = y;
        }
        public TECVisualScope(Guid guid, double x, double y)
        {
            Guid = guid;
            _x = x;
            _y = y;
        }
        public TECVisualScope(TECVisualScope vScope)
        {
            _scope = vScope.Scope;
            _x = vScope.X;
            _y = vScope.Y;
            Guid = vScope.Guid;
        }

        public override object Copy()
        {
            TECVisualScope outScope = new TECVisualScope(this);
            return outScope;
        }
    }
}
