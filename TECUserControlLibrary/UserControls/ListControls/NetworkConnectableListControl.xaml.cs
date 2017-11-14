using EstimatingLibrary.Interfaces;

namespace TECUserControlLibrary.UserControls.ListControls
{
    /// <summary>
    /// Interaction logic for ControllerListControl.xaml
    /// </summary>
    public partial class NetworkConnectableListControl : BaseListControl<INetworkConnectable>
    {
        public NetworkConnectableListControl()
        {
            InitializeComponent();
        }
        
    }
}
