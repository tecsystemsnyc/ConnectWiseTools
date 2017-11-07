using System.Collections.Generic;
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
            if (e.ClickCount == 3 && cell.Column is DataGridTextColumn)
            {
                var textBox = FindVisualChild<TextBox>(cell);
                if (textBox != null)
                {
                    textBox.SelectAll();
                }
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
            Button button = FindVisualParent<Button>(ogSource as UIElement);
            DataGrid childGrid = FindVisualParent<DataGrid>(childRow);
            if (parentRow == null ||
                (childRow != null && childRow.IsSelected == true && childGrid.SelectedItems.Count == 1) ||
                (childRow == null && button == null))
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
            if (e.ClickCount == 3)
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

        private void CollapseAll_Clicked(object sender, RoutedEventArgs e)
        {
            Grid grid = FindVisualParent<Grid>(sender as UIElement);
            if (grid != null)
            {
                DataGrid dataGrid = FindVisualChildInGrid<DataGrid>(grid);
                if (dataGrid != null)
                {
                    List<DataGridRow> rows = FindVisualChildren<DataGridRow>(dataGrid);
                    foreach (DataGridRow row in rows)
                    {
                        row.DetailsVisibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void DataGrid_LostFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (!(sender as DataGrid).IsKeyboardFocusWithin)
            {
                (sender as DataGrid).CommitEdit(DataGridEditingUnit.Row, true);
            }
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
        static T FindVisualChildInGrid<T>(UIElement element) where T : UIElement
        {
            UIElement child = null;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                child = VisualTreeHelper.GetChild(element, i) as UIElement;
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

            }
            return null;
        }
        static List<T> FindVisualChildren<T>(UIElement element) where T : UIElement
        {
            List<T> children = new List<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                UIElement child = VisualTreeHelper.GetChild(element, i) as UIElement;

                T correctlyTyped = child as T;
                if (correctlyTyped != null)
                {
                    children.Add(child as T);
                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        children.Add(childOfChild);
                    }
                }
                else if (child != null)
                {
                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        children.Add(childOfChild);
                    }
                }
            }
            return children;
        }

        private void ShowDetails_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = FindVisualParent<DataGridRow>(e.OriginalSource as UIElement);
            if (row != null)
            {
                DataGrid grid = FindVisualParent<DataGrid>(row);
                if (grid.SelectedItems != null && grid.SelectedItems.Count > 1)
                {
                    grid.SelectedItem = row;
                }

            }
        }

        private void TabControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsUnderTabHeader(e.OriginalSource as DependencyObject))
                CommitTables(sender as TabControl);
        }

        private bool IsUnderTabHeader(DependencyObject control)
        {
            if (control is TabItem)
                return true;
            DependencyObject parent = VisualTreeHelper.GetParent(control);
            if (parent == null)
                return false;
            return IsUnderTabHeader(parent);
        }

        private void CommitTables(DependencyObject control)
        {
            if (control is DataGrid)
            {
                DataGrid grid = control as DataGrid;
                grid.CommitEdit(DataGridEditingUnit.Row, true);
                return;
            }
            int childrenCount = VisualTreeHelper.GetChildrenCount(control);
            for (int childIndex = 0; childIndex < childrenCount; childIndex++)
                CommitTables(VisualTreeHelper.GetChild(control, childIndex));
        }

    }
}
