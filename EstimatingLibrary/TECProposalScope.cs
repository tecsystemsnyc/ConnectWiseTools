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

        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    if (item is TECScopeBranch)
                    {
                        //Do Nothing
                    }
                    else if (item is TECScope)
                    {
                        addProposalScope(item as TECScope);
                    }
                   
                    NotifyPropertyChanged("MetaAdd", this, item);
                    
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    if (item is TECScopeBranch)
                    {
                        //Do Nothing
                    }
                    else if (item is TECScope)
                    {
                        removeProposalScope(item as TECScope);
                    }
                    
                    NotifyPropertyChanged("MetaRemove", this, item);
                    
                }
            }
        }

        public override object Copy()
        {
            TECProposalScope pScopeToReturn = new TECProposalScope(this.Scope);
            pScopeToReturn._isProposed = IsProposed;
            foreach(TECProposalScope child in Children)
            {
                pScopeToReturn.Children.Add(child.Copy() as TECProposalScope);
            }

            foreach(TECScopeBranch branch in Notes)
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
    }
}
