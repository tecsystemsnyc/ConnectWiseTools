using System;
using System.Windows.Markup;

namespace TECUserControlLibrary.Utilities
{
    public abstract class BaseConverter : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
