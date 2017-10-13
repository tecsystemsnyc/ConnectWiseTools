using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TECUserControlLibrary.UserControls.ItemControls
{
    public class BaseItemControl : UserControl
    {

        public object ScopeParent
        {
            get { return (object)GetValue(ScopeParentProperty); }
            set { SetValue(ScopeParentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScopeParent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScopeParentProperty =
            DependencyProperty.Register("ScopeParent", typeof(object), typeof(BaseItemControl));


        public ICommand CopyCommand
        {
            get { return (ICommand)GetValue(CopyCommandProperty); }
            set { SetValue(CopyCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CopyCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CopyCommandProperty =
            DependencyProperty.Register("CopyCommand", typeof(ICommand), typeof(BaseItemControl));


        public ICommand PasteCommand
        {
            get { return (ICommand)GetValue(PasteCommandProperty); }
            set { SetValue(PasteCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PasteCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasteCommandProperty =
            DependencyProperty.Register("PasteCommand", typeof(ICommand), typeof(BaseItemControl));


        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DeleteCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeleteCommandProperty =
            DependencyProperty.Register("DeleteCommand", typeof(ICommand), typeof(BaseItemControl));

        


    }
}
