using System;
using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace TECUserControlLibrary.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ProposalVM : ViewModelBase
    {
        private TECBid _bid;
        public TECBid Bid
        {
            get { return _bid; }
            set
            {
                _bid = value;
                RaisePropertyChanged("Bid");
            }
        }

        public RelayCommand<TECScopeBranch> AddScopeBranchCommand { get; private set; }

        public ProposalVM(TECBid bid)
        {
            Bid = bid;
            AddScopeBranchCommand = new RelayCommand<TECScopeBranch>(addScopBranchExecute);
        }
        
        public void Refresh(TECBid bid)
        {
            Bid = bid;
        }

        private void addScopBranchExecute(TECScopeBranch obj)
        {
            if(obj == null)
            {
                Bid.ScopeTree.Add(new TECScopeBranch(false));
            } else
            {
                obj.Branches.Add(new TECScopeBranch(obj.IsTypical));
            }
        }
    }
}