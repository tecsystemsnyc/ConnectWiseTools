using System.Windows;
using System.Windows.Controls;

namespace TECUserControlLibrary.Views
{
    public class BaseView<T> : UserControl
    {
        
        public T ViewModel
        {
            get { return (T)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(T), typeof(BaseView<T>));


    }
}
