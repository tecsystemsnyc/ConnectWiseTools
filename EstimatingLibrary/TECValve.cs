﻿using EstimatingLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECValve: TECHardware, ITECConnectable
    {

        #region Fields
        private TECDevice _actuator;
        private double _cv;
        private double _size;
        private string _style;

        #endregion
        #region Properties
        public TECDevice Actuator
        {
            get { return _actuator; }
            set
            {
                var old = Actuator;
                _actuator = value;
                NotifyCombinedChanged(Change.Edit, "Actuator", this, _actuator, old);
            }
        }
        public double Cv
        {
            get { return _cv; }
            set
            {
                var old = _cv;
                _cv = value;
                NotifyCombinedChanged(Change.Edit, "Cv", this, _cv, old);
            }
        }
        public double Size
        {
            get { return _size; }
            set
            {
                var old = _size;
                _size = value;
                NotifyCombinedChanged(Change.Edit, "Size", this, _size, old);
            }
        }
        public string Style
        {
            get { return _style; }
            set
            {
                var old = _style;
                _style = value;
                NotifyCombinedChanged(Change.Edit, "Style", this, _size, old);
            }
        }


        public ObservableCollection<TECElectricalMaterial> ConnectionTypes
        {
            get
            {
                return Actuator.ConnectionTypes;
            }
        }
        #endregion

        public TECValve(TECManufacturer manufacturer, TECDevice actuator) : base (Guid.NewGuid(), manufacturer)
        {
            _actuator = actuator;
        }
        

    }
}