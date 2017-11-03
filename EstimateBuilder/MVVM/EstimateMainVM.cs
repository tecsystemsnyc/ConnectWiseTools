using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.ViewModels;
using TECUserControlLibrary.ViewModels.SummaryVMs;

namespace EstimateBuilder.MVVM
{
    public class EstimateMainVM : TECUserControlLibrary.BaseVMs.MainViewModel
    {
        /// <summary>
        /// Estimate-typed splash vm for manipulation
        /// </summary>
        private EstimateMenuVM menuVM
        {
            get { return MenuVM as EstimateMenuVM; }
        }
        /// <summary>
        /// Estimate-typed splash vm for manipulation
        /// </summary>
        private EstimateEditorVM editorVM
        {
            get { return EditorVM as EstimateEditorVM; }
        }
        /// <summary>
        /// Estimate-typed splash vm for manipulation
        /// </summary>
        private EstimateSplashVM splashVM
        {

            get { return SplashVM as EstimateSplashVM; }
        }
    
        #region VMs
        public ScopeEditorVM ScopeEditorVM { get; set; }
        public LaborVM LaborVM { get; set; }
        public ReviewVM ReviewVM { get; set; }
        public ProposalVM ProposalVM { get; set; }
        public ElectricalVM ElectricalVM { get; set; }
        public NetworkVM NetworkVM { get; set; }
        public ItemizedSummaryVM ItemizedSummaryVM { get; private set; }
        public MaterialSummaryVM MaterialSummaryVM { get; private set; }
        #endregion

        public EstimateMainVM() : base(new EstimateSplashVM(), new EstimateMenuVM(), new EstimateEditorVM())
        {

        }
    }
}
