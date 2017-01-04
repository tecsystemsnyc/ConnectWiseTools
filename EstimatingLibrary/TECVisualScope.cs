using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<TECSubScope> SubScope
        {
            get {
                return getSubScope();
            }
        }

        public Guid Guid;

        public TECVisualScope() : this(new TECSystem(), 0, 0)
        {

        }

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

        private ObservableCollection<TECSubScope> getSubScope()
        {
            var outScope = new ObservableCollection<TECSubScope>();

            if(this.Scope is TECSystem)
            {
                var sys = this.Scope as TECSystem;
                foreach(TECEquipment equipment in sys.Equipment)
                {
                    foreach(TECSubScope sub in equipment.SubScope)
                    {
                        outScope.Add(sub);
                    }
                }
            } else if(this.Scope is TECEquipment)
            {
                var equipment = this.Scope as TECEquipment;
                foreach (TECSubScope sub in equipment.SubScope)
                {
                    outScope.Add(sub);
                }
            } else if(this.Scope is TECSubScope)
            {
                SubScope.Add(this.Scope as TECSubScope);
            }

            return outScope;
        }
    }
}
