using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TECUserControlLibrary.ViewModels;

namespace EstimateBuilder.MVVM
{
    public class EstimateMenuVM : MenuVM
    {
        private const bool DEBUG = true;

        public EstimateMenuVM() : base()
        {
            setupMenu();
        }

        public void SetRefreshBidCommand (Action execute, Func<bool> canExecute = null)
        {
            RelayCommand command = new RelayCommand(execute, forceNullToTrue(canExecute));
            setCommand("Refresh Bid", command);
        }
        public void SetExportProposalCommand(Action execute, Func<bool> canExecute = null)
        {
            RelayCommand command = new RelayCommand(execute, forceNullToTrue(canExecute));
            setCommand("Proposal", command);
        }
        public void SetExportPointsListCommand(Action execute, Func<bool> canExecute = null)
        {
            RelayCommand command = new RelayCommand(execute, forceNullToTrue(canExecute));
            setCommand("Points List", command);
        }
        public void SetExportEngineeringCommand(Action execute, Func<bool> canExecute = null)
        {
            RelayCommand command = new RelayCommand(execute, forceNullToTrue(canExecute));
            setCommand("Engineering", command);
        }
        public void SetLoadTemplatesCommand(Action execute, Func<bool> canExecute = null)
        {
            RelayCommand command = new RelayCommand(execute, forceNullToTrue(canExecute));
            setCommand("Load Templates", command);
        }
        public void SetDebugWindowCommand(Action execute, Func<bool> canExecute = null)
        {
            RelayCommand command = new RelayCommand(execute, forceNullToTrue(canExecute));
            setCommand("Debug Window", command);
        }

        private void setupMenu()
        {
            //File menu items
            addMenuItem("Refresh Bid", "File");
            addMenuItem("Export", "File");

            //Export menu items
            addMenuItem("Proposal", "Export");
            addMenuItem("Points List", "Export");
            addMenuItem("Engineering", "Export");

            //Templates menu items
            addMenuItem("Load Templates", "Templates");

            if (DEBUG)
            {
                addMenuItem("Debug");

                addMenuItem("Debug Window", "Debug");
            }
        }
    }
}
