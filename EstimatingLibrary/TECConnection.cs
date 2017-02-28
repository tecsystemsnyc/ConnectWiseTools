﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{

    public class TECConnection : TECObject
    {
        #region Properties
        private Guid _guid;
        private double _length;
        private TECController _controller;
        private ObservableCollection<TECScope> _scope;
        private ObservableCollection<IOType> _ioTypes;

        public Guid Guid
        {
            get { return _guid; }
        }
        public double Length
        {
            get { return _length; }
            set
            {
                var temp = this.Copy();
                _length = value;
                NotifyPropertyChanged("Length", temp, this);
            }
        }
        
        public TECController Controller
        {
            get { return _controller; }
            set
            {
                var temp = this.Copy();
                _controller = value;
                NotifyPropertyChanged("Controller", temp, this);
            }
        }
        public ObservableCollection<TECScope> Scope
        {
            get { return _scope; }
            set
            {
                var temp = this.Copy();
                _scope = value;
                NotifyPropertyChanged("Scope", temp, this);
            }
        }
        public ObservableCollection<TECConnectionType> ConnectionTypes
        {
            get { return getConnectionTypes(); }
            
        }
        public ObservableCollection<IOType> IOTypes
        {
            get { return _ioTypes; }
            set
            {
                var temp = this.Copy();
                _ioTypes = value;
                NotifyPropertyChanged("IOTypes", temp, this);
            }
        }

        #endregion //Properties

        #region Constructors 
        public TECConnection(Guid guid)
        {
            _guid = guid;
            _length = 0;
            _scope = new ObservableCollection<TECScope>();
            _ioTypes = new ObservableCollection<IOType>();
            _controller = new TECController();
        }
        public TECConnection() : this(Guid.NewGuid()) { }

        public TECConnection(TECConnection connectionSource) : this(connectionSource.Guid)
        {
            _length = connectionSource.Length;
            _scope = connectionSource.Scope;
            _ioTypes = connectionSource.IOTypes;
            _controller = connectionSource.Controller;
        }
        #endregion //Constructors

        #region Methods
        public override Object Copy()
        {
            TECConnection connection = new TECConnection(this);

            return connection;
        }

        private ObservableCollection<TECConnectionType> getConnectionTypes()
        {
            var outConnectionTypes = new ObservableCollection<TECConnectionType>();

            foreach(TECScope scope in Scope)
            {
                if(scope is TECSubScope)
                {
                    var sub = scope as TECSubScope;
                    foreach(TECDevice dev in sub.Devices)
                    {
                        outConnectionTypes.Add(dev.ConnectionType);
                    }
                }
            }

            return outConnectionTypes;
        }

        #endregion

    }
}
