﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECScopeBranch : TECScope
    {//TECScopeBranch exists as an alternate object to TECSystem. It's purpose is to serve as a non-specific scope object with unlimited branches in both depth and breadth.
        #region Properties
        public ObservableCollection<TECScopeBranch> Branches { get; set; }

        #endregion //Properites

        #region Constructors
        public TECScopeBranch(Guid guid) : base(guid)
        {
            Branches = new ObservableCollection<TECScopeBranch>();
            Branches.CollectionChanged += Branches_CollectionChanged;
        }
        
        public TECScopeBranch() : this(Guid.NewGuid()) { }

        //Copy Constructor
        public TECScopeBranch(TECScopeBranch scopeBranchSource) : this()
        {
            foreach (TECScopeBranch branch in scopeBranchSource.Branches)
            {
                Branches.Add(new TECScopeBranch(branch));
            }
            _name = scopeBranchSource.Name;
            _description = scopeBranchSource.Description;
            _tags = scopeBranchSource.Tags;
        }
        #endregion //Constructors

        public override Object Copy()
        {
            TECScopeBranch outScope = new TECScopeBranch(this);
            outScope._guid = Guid;
            return outScope;
        }

        public override object DragDropCopy()
        {
            TECScopeBranch outScope = new TECScopeBranch(this);
            return outScope;
        }

        private void Branches_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    NotifyPropertyChanged("Add", this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyPropertyChanged("Remove", this, item);
                }
            }
        }

    }
}
