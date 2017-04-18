using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace TECUserControlLibrary
{
    public partial class TECResourceDictionary
    {
        private void DataGridCell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGridCell cell = sender as DataGridCell;
            if(e.ClickCount == 3 && cell.Column is DataGridTextColumn)
            {
                var textBox = FindVisualChild<TextBox>(cell);
                textBox.SelectAll();
            }
            else if (cell != null && !cell.IsEditing && !cell.IsReadOnly)
            {
                if (!cell.IsFocused)
                {
                    cell.Focus();
                }
                DataGrid dataGrid = FindVisualParent<DataGrid>(cell);
                DataGridRow dataGridRow = FindVisualParent<DataGridRow>(cell);
                if (dataGrid != null)
                {
                    if (dataGrid.SelectionUnit != DataGridSelectionUnit.FullRow)
                    {
                        if (!cell.IsSelected)
                            cell.IsSelected = true;
                    }
                    else
                    {
                        DataGridRow row = FindVisualParent<DataGridRow>(cell);
                        if (row != null && !row.IsSelected)
                        {
                            row.IsSelected = true;
                        }
                    }
                }
            }
        }

        private void SelectRowDetails(object sender, MouseButtonEventArgs e)
        {
            var ogSource = e.OriginalSource;
            var source = e.Source;
            DataGridRow parentRow = FindVisualParent<DataGridRow>(sender as DataGridDetailsPresenter);
            DataGridRow childRow = FindVisualParent<DataGridRow>(ogSource as UIElement);

            if (parentRow == null || (childRow!= null && childRow.IsSelected == true) || childRow == null)
                {
                    return;
                }
                parentRow.Focusable = true;
                parentRow.Focus();

                var focusDirection = FocusNavigationDirection.Next;
                var request = new TraversalRequest(focusDirection);
                var elementWithFocus = Keyboard.FocusedElement as UIElement;
                if (elementWithFocus != null)
                {
                    elementWithFocus.MoveFocus(request);
                }
        }

        private void TextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount == 3)
            {
                (sender as TextBox).SelectAll();
            }
        }
        
        private void SelectRow(object sender, MouseButtonEventArgs e)
        {
            var ogSource = e.OriginalSource;
            var source = e.Source;
            DataGridRow row = sender as DataGridRow;
            DataGrid grid = FindVisualParent<DataGrid>(row);
            grid.SelectedItem = grid.SelectedItem;
            
        }
        
        private void DataGrid_Documents_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            //e.Handled = true;
        }

        static T FindVisualParent<T>(UIElement element) where T : UIElement
        {
            UIElement parent = element;
            while (parent != null)
            {
                T correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }

                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }
            return null;
        }

        static T FindVisualChild<T>(UIElement element) where T : UIElement
        {
            UIElement child = null;
            if (VisualTreeHelper.GetChildrenCount(element) > 0)
            {
                child = VisualTreeHelper.GetChild(element, 0) as UIElement;
            }
            while (child != null)
            {
                T correctlyTyped = child as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }

                if (VisualTreeHelper.GetChildrenCount(child) > 0)
                {
                    child = VisualTreeHelper.GetChild(child, 0) as UIElement;
                }
                else
                {
                    child = null;
                }
            }
            return null;
        }
    }
}
