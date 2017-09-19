using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECScopeBranch : TECLabeled, ISaveable
    {//TECScopeBranch exists as an alternate object to TECSystem. It's purpose is to serve as a non-specific scope object with unlimited branches in both depth and breadth.
        #region Properties
        private ObservableCollection<TECScopeBranch> _branches;
        public ObservableCollection<TECScopeBranch> Branches
        {
            get { return _branches; }
            set
            {
                var old = Branches;
                _branches = value;
                notifyCombinedChanged(Change.Edit, "Branches", this, value, old);
                Branches.CollectionChanged += Branches_CollectionChanged;
            }
        }

        public SaveableMap SaveObjects
        {
            get { return saveObjects(); }
        }
        public SaveableMap RelatedObjects
        {
            get { return new SaveableMap(); }
        }

        #endregion //Properites

        #region Constructors
        public TECScopeBranch(Guid guid) : base(guid)
        {
            _branches = new ObservableCollection<TECScopeBranch>();
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
            _label = scopeBranchSource.Label;
        }
        #endregion //Constructors
        
        private void Branches_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    notifyCombinedChanged(Change.Add, "Branches", this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    notifyCombinedChanged(Change.Remove, "Branches", this, item);
                }
            }
        }

        private SaveableMap saveObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(this.Branches, "Branches");
            return saveList;
        }

    }
}
