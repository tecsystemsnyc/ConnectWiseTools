using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECProposalScope
    {
        public TECScope Scope;
        public ObservableCollection<TECProposalScope> Children;
        public ObservableCollection<string> Notes;
        public bool IsProposed;

        public TECProposalScope(TECScope scope)
        {
            Scope = scope;
            IsProposed = false;
            Notes = new ObservableCollection<string>();
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
    }
}
