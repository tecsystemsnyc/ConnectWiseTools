using GalaSoft.MvvmLight.CommandWpf;
using NLog;
using System;
using TECUserControlLibrary.ViewModels;

namespace EstimateBuilder.MVVM
{
    public class EstimateMenuVM : MenuVM
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
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
        public void SetExportSummaryCommand(Action execute, Func<bool> canExecute = null)
        {
            RelayCommand command = new RelayCommand(execute, forceNullToTrue(canExecute));
            setCommand("Summary", command);
        }
        public void SetExportBudgetCommand(Action execute, Func<bool> canExecute = null)
        {
            RelayCommand command = new RelayCommand(execute, forceNullToTrue(canExecute));
            setCommand("Budget", command);
        }
        public void SetExportBOMCommand(Action execute, Func<bool> canExecute = null)
        {
            RelayCommand command = new RelayCommand(execute, forceNullToTrue(canExecute));
            setCommand("BOM", command);
        }
        public void SetLoadTemplatesCommand(Action execute, Func<bool> canExecute = null)
        {
            RelayCommand command = new RelayCommand(execute, forceNullToTrue(canExecute));
            setCommand("Load Templates", command);
        }
        public void SetDebugWindowCommand(Action execute, Func<bool> canExecute = null)
        {
            RelayCommand command = new RelayCommand(execute, forceNullToTrue(canExecute));
            try
            {
                setCommand("Debug Window", command);
            }
            catch (Exception)
            {
                logger.Debug("No debug window to set command to.");
            }
        }

        private void setupMenu()
        {
            //File menu items
            addMenuItem("Refresh Bid", "Can't refresh", parentItemName: "File");
            addMenuItem("Export", parentItemName: "File");

            //Export menu items
            addMenuItem("Proposal", parentItemName: "Export");
            addMenuItem("Points List", parentItemName: "Export");
            addMenuItem("Summary", parentItemName: "Export");
            addMenuItem("Budget", parentItemName: "Export");
            addMenuItem("BOM", parentItemName: "Export");

            //Templates menu items
            addMenuItem("Load Templates", busyText, parentItemName: "Templates");

#if DEBUG
            addMenuItem("Debug");
            addMenuItem("Debug Window", parentItemName: "Debug");
#endif
        }
    }
}
