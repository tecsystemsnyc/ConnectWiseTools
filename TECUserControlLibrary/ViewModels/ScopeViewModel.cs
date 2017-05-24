using EstimatingLibrary;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows;

namespace EstimateBuilder.ViewModel
{

    public class ScopeViewModel : ViewModelBase
    {

        /// <summary>
        /// The X coordinate of the location of the rectangle (in content coordinates).
        /// </summary>
        private double x = 0;

        /// <summary>
        /// The Y coordinate of the location of the rectangle (in content coordinates).
        /// </summary>
        private double y = 0;

        /// <summary>
        /// The system with which this view is associated.
        /// </summary>
        private TECSystem system = new TECSystem();

        public ScopeViewModel()
        {
        }

        public ScopeViewModel(double x, double y, TECSystem system)
        {
            this.x = x;
            this.y = y;
            this.system = system;
        }

        /// <summary>
        /// The X coordinate of the location of the rectangle (in content coordinates).
        /// </summary>
        public double X
        {
            get
            {
                return x;
            }
            set
            {
                if (x == value)
                {
                    return;
                }

                x = value;

                RaisePropertyChanged("X");
            }
        }

        /// <summary>
        /// The Y coordinate of the location of the rectangle (in content coordinates).
        /// </summary>
        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                if (y == value)
                {
                    return;
                }

                y = value;

                RaisePropertyChanged("Y");
            }
        }

        /// <summary>
        /// The system with which this view is associated.
        /// </summary>
        public TECSystem System
        {
            get
            {
                return system;
            }
            set
            {
                if (system == value)
                {
                    return;
                }

                system = value;

                RaisePropertyChanged("Y");
            }
        }

    }
}
