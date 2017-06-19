﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public enum CostType { None, TEC, Electrical }

    public class TECCost : TECScope
    { 
        #region Properties

        protected double _cost;
        public double Cost
        {
            get { return _cost; }
            set
            {
                var temp = this.Copy();
                _cost = value;
                NotifyPropertyChanged("Cost", temp, this);
                RaisePropertyChanged("TotalCost");
            }
        }

        virtual public double ExtendedCost
        {
            get { return Cost; }
        }

        protected double _labor;
        public double Labor
        {
            get { return _labor; }
            set
            {
                var temp = this.Copy();
                _labor = value;
                NotifyPropertyChanged("Labor", temp, this);
                RaisePropertyChanged("TotalLabor");
            }
        }

        protected CostType _type;
        public CostType Type
        {
            get { return _type; }
            set
            {
                var temp = this.Copy();
                _type = value;
                NotifyPropertyChanged("Type", temp, this);
            }
        }

        public double TotalCost
        {
            get { return Cost * Quantity; }
        }

        public double TotalLabor
        {
            get { return Labor * Quantity; }
        }

        #endregion

        #region Constructors
        public TECCost(Guid guid) : base(guid)
        {
            _cost = 0;
            _labor = 0;
            _type = 0;
            base.PropertyChanged += TECCost_PropertyChanged;
        }

        public TECCost() : this(Guid.NewGuid()) { }
        #endregion

        protected void copyPropertiesFromCost(TECCost cost)
        {
            copyPropertiesFromScope(cost);
            _cost = cost.Cost;
            _labor = cost.Labor;
            _type = cost.Type;
        }

        public override object Copy()
        {
            var outCost = new TECCost();
            outCost.copyPropertiesFromCost(this);
            outCost._guid = this.Guid;
            return outCost;
        }

        public override object DragDropCopy()
        {
            return this.Copy();
        }

        private void TECCost_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Quantity")
            {
                RaisePropertyChanged("TotalCost");
                RaisePropertyChanged("TotalLabor");
            }
        }
    }
}
