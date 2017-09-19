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
    public class TECScopeBranch : TECLabeled, ISaveable, ITypicalable
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

        public bool IsTypical { get; private set; }
        #endregion //Properites

        #region Constructors
        public TECScopeBranch(Guid guid, bool isTypical) : base(guid)
        {
            IsTypical = isTypical;
            _branches = new ObservableCollection<TECScopeBranch>();
            Branches.CollectionChanged += Branches_CollectionChanged;
        }

        public TECScopeBranch(bool isTypical) : this(Guid.NewGuid(), isTypical) { }

        //Copy Constructor
        public TECScopeBranch(TECScopeBranch scopeBranchSource, bool isTypical) : this(isTypical)
        {
            foreach (TECScopeBranch branch in scopeBranchSource.Branches)
            {
                Branches.Add(new TECScopeBranch(branch, isTypical));
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
