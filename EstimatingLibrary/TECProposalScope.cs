using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECProposalScope : TECObject
    {
        #region Properties
        public TECScope Scope { get; set; }
        public ObservableCollection<TECProposalScope> Children { get; set; }
        public ObservableCollection<TECScopeBranch> Notes { get; set; }
        public bool IsProposed
        {
            get { return _isProposed; }
            set
            {
                var temp = Copy();
                _isProposed = value;
                NotifyPropertyChanged("IsProposed", temp, this);
            }
        }
        private bool _isProposed;

        public Guid Guid { get { return Scope.Guid; } }
        #endregion

        #region Constructors
        public TECProposalScope(TECScope scope)
        {
            Scope = scope;
            _isProposed = false;
            Notes = new ObservableCollection<TECScopeBranch>();
            Children = new ObservableCollection<TECProposalScope>();

            Children.CollectionChanged += CollectionChanged;
            Notes.CollectionChanged += CollectionChanged;

            if (scope is TECSystem)
            {
                (scope as TECSystem).Equipment.CollectionChanged += CollectionChanged;
                foreach (TECEquipment equip in (scope as TECSystem).Equipment)
                {
                    Children.Add(new TECProposalScope(equip));
                }
            }
            else if (scope is TECEquipment)
            {
                (scope as TECEquipment).SubScope.CollectionChanged += CollectionChanged;
                foreach (TECSubScope ss in (scope as TECEquipment).SubScope)
                {
                    Children.Add(new TECProposalScope(ss));
                }
            }
            else if (scope is TECSubScope)
            {
                //Do Nothing
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        public TECProposalScope(TECScope scope, bool isProposed, ObservableCollection<TECScopeBranch> notes)
        {
            Scope = scope;
            _isProposed = isProposed;
            Notes = notes;
            Children = new ObservableCollection<TECProposalScope>();

            Notes.CollectionChanged += CollectionChanged;
            Children.CollectionChanged += CollectionChanged;
        }
        public TECProposalScope(TECProposalScope scopeSource) : this(scopeSource.Scope, scopeSource.IsProposed, scopeSource.Notes)
        {
            Children.CollectionChanged -= CollectionChanged;
            Children = scopeSource.Children;
            Children.CollectionChanged += CollectionChanged;
        }
        #endregion

        #region Event handlers
        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    if (item is TECScopeBranch)
                    {
                        NotifyPropertyChanged("Add", this, item);
                    }
                    else if (item is TECScope)
                    {
                        addProposalScope(item as TECScope);
                    }
                    else if (item is TECProposalScope)
                    {
                        NotifyPropertyChanged("MetaAdd", this, item);
                    }

                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    if (item is TECScopeBranch)
                    {
                        NotifyPropertyChanged("Remove", this, item);
                    }
                    else if (item is TECScope)
                    {
                        removeProposalScope(item as TECScope);
                    }
                    else if (item is TECProposalScope)
                    {
                        NotifyPropertyChanged("MetaRemove", this, item);
                    }
                }
            }
        }
        #endregion

        #region Methods
        public override object Copy()
        {
            TECProposalScope pScopeToReturn = new TECProposalScope(this.Scope);
            pScopeToReturn._isProposed = IsProposed;
            foreach (TECProposalScope child in Children)
            {
                pScopeToReturn.Children.Add(child.Copy() as TECProposalScope);
            }

            foreach (TECScopeBranch branch in Notes)
            {
                pScopeToReturn.Notes.Add(branch.Copy() as TECScopeBranch);
            }

            return pScopeToReturn;
        }

        private void addProposalScope(TECScope scope)
        {
            this.Children.Add(new TECProposalScope(scope));
        }
        private void removeProposalScope(TECScope scope)
        {
            List<TECProposalScope> scopeToRemove = new List<TECProposalScope>();
            foreach (TECProposalScope pScope in this.Children)
            {
                if (pScope.Scope == scope)
                {
                    scopeToRemove.Add(pScope);
                }
            }
            foreach (TECProposalScope pScope in scopeToRemove)
            {
                this.Children.Remove(pScope);
            }

        }
        #endregion
    }
}
