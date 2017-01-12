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

            if (scope is TECSystem)
            {
                foreach (TECEquipment equip in (scope as TECSystem).Equipment)
                {
                    Children.Add(new TECProposalScope(equip));
                }
            }
            else if (scope is TECEquipment)
            {
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

    }
}
