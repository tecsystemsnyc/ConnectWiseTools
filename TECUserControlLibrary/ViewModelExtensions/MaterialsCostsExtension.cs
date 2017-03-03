using EstimatingLibrary;
using GalaSoft.MvvmLight;

namespace TECUserControlLibrary.ViewModelExtensions
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MaterialsCostsExtension : ViewModelBase
    {
        private TECTemplates _templates;
        public TECTemplates Templates
        {
            get { return _templates; }
            set
            {
                _templates = value;
                RaisePropertyChanged("Templates");
            }
        }

        public MaterialsCostsExtension(TECTemplates templates)
        {
            Templates = templates;
        }
    }
}