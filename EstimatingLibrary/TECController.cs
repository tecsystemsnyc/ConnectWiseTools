using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECController : TECScope
    {
        private double _cost;
        private ObservableCollection<TECConnection> _connections;
        private ObservableCollection<TECIO> _io;
        private TECManufacturer _manufacturer;

        public double Cost
        {
            get { return _cost; }
            set
            {
                var temp = this.Copy();
                _cost = value;
                NotifyPropertyChanged("Cost", temp, this);
            }
        }
        public ObservableCollection<TECConnection> Connections
        {
            get { return _connections; }
            set
            {
                var temp = this.Copy();
                _connections = value;
                NotifyPropertyChanged("Connections", temp, this);
            }
        }
        public ObservableCollection<TECIO> IO
        {
            get { return _io; }
            set
            {
                var temp = this.Copy();
                _io = value;
                NotifyPropertyChanged("IO", temp, this);
            }
        }
        public TECManufacturer Manufacturer
        {
            get { return _manufacturer; }
            set
            {
                var temp = this.Copy();
                _manufacturer = value;
                NotifyPropertyChanged("Manufacturer", temp, this);
                NotifyPropertyChanged("ChildChanged", (object)this, (object)value);
            }
        }

        public List<IOType> AvailableIO
        {
            get { return getAvailableIO(); }
        }
        public List<IOType> NetworkIO
        {
            get { return getNetworkIO(); }
        }

        public TECController(string name, string desciption, Guid guid, double cost) : base(name, desciption, guid)
        {
            _cost = cost;
            _io = new ObservableCollection<TECIO>();
            _connections = new ObservableCollection<TECConnection>();
            _manufacturer = new TECManufacturer();
            IO.CollectionChanged += CollectionChanged;
        }

        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach(Object item in e.NewItems)
                {
                    (item as TECIO).PropertyChanged += ObjectPropertyChanged;
                    NotifyPropertyChanged("Add", this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (Object item in e.OldItems)
                {
                    (item as TECIO).PropertyChanged -= ObjectPropertyChanged;
                    NotifyPropertyChanged("Remove", this, item);
                }
            }
        }

        private void ObjectPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;
            if (e.PropertyName == "Quantity")
            {
                NotifyPropertyChanged("ChildChanged", (object)this, (object)args.NewValue);
            }
        }

        public TECController() : this("", "", Guid.NewGuid(), 0)
        {
        }

        #region Methods
        public override Object Copy()
        {
            TECController outController = new TECController(this.Name,
                this.Description,
                this.Guid,
                this.Cost);

            foreach (TECIO io in this.IO)
            {
                outController.IO.Add(io);
            }

            foreach (TECConnection conn in this.Connections)
            {
                outController.Connections.Add(conn);
            }
            
            return outController;
        }

        public override Object DragDropCopy()
        {
            var outController = new TECController();
            outController.Name = Name;
            outController.Description = Description;
            outController.Cost = Cost;
            outController.IO = IO;
            outController.Tags = Tags;

            return outController;
        }

        private List<IOType> getAvailableIO()
        {
            var availableIO = new List<IOType>();
            foreach (TECIO type in this.IO)
            {
                for(var x = 0; x < type.Quantity; x++)
                {
                    availableIO.Add(type.Type);
                }
            }

            foreach (TECConnection connected in this.Connections)
            {
                foreach(TECScope scope in connected.Scope)
                {
                    if(scope is TECSubScope)
                    {
                        foreach(TECDevice device in ((TECSubScope)scope).Devices)
                        {
                            availableIO.Remove(device.IOType);
                        }
                    }
                }
            }
            return availableIO;
        }

        private List<IOType> getNetworkIO()
        {
            var outIO = new List<IOType>();
            foreach (TECIO io in this.IO)
            {
                var type = io.Type;
                if(type != IOType.AI && type != IOType.AO && type != IOType.DI && type != IOType.DO)
                {
                    for (var x = 0; x < io.Quantity; x++)
                    {
                        outIO.Add(type);
                    }
                }
            }

            return outIO;
        }

        public int NumberOfIOType(IOType ioType)
        {
            int outNum = 0;

            foreach(TECIO type in IO)
            {
                if(type.Type == ioType)
                {
                    outNum = type.Quantity;
                }
            }

            return outNum;
        }

        public List<IOType> getUniqueIO()
        {
            var outList = new List<IOType>();

            foreach(TECIO io in this.IO)
            {
                if (!outList.Contains(io.Type))
                {
                    outList.Add(io.Type);
                }
            }
            return outList;
        }


        #endregion
    }
}
