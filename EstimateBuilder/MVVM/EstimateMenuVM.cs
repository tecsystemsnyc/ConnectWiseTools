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

        public ICommand RefreshBidCommand { set { setCommand("Refresh Bid", value); } }
        public ICommand ExportProposalCommand { set { setCommand("Proposal", value); } }
        public ICommand ExportPointsListCommand { set { setCommand("Points List", value); } }
        public ICommand ExportEngineeringCommand { set { setCommand("Engineering", value); } }
        public ICommand LoadTemplatesCommand { set { setCommand("Load Templates", value); } }
        public ICommand DebugWindowCommand { set { setCommand("Debug Window", value); } }

        public EstimateMenuVM() : base()
        {
            setupMenu();
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
