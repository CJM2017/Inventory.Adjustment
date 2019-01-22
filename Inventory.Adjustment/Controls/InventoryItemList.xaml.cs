// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.UI.Controls
{
    using System;
    using System.Collections;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Inventory.Adjustment.UI.Infrastructure;
    using Inventory.Adjustment.UI.ViewModels;

    /// <summary>
    /// Interaction logic for InventoryItemList.xaml
    /// </summary>
    public partial class InventoryItemList : UserControl
    {
        private InventoryItemListViewModel _viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryItemList"/> class
        /// </summary>
        public InventoryItemList()
        {
            _viewModel = new InventoryItemListViewModel(SessionManager.Instance);
            this.DataContext = _viewModel;

            InitializeComponent();
        }

        private void TextChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            TextBox searchTextBox = sender as TextBox;
            if (searchTextBox != null)
            {
                this._viewModel.SearchString = searchTextBox.Text;
            }
        }

        private void Handle_Selection(object sender, SelectionChangedEventArgs args)
        {
            if (args.OriginalSource != null)
            {
                Type type = args.OriginalSource.GetType();
                if (type == typeof(DataGrid))
                {
                    IList items = InventoryDataGrid.SelectedItems;
                    this._viewModel.UpdateSelection(items);
                }
            }
        }

        private void DataGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGrid grid = sender as DataGrid;
                if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count > 0)
                {
                    while (grid.SelectedItems.Count > 0)
                    {
                        DataGridRow dgr = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItems[0]) as DataGridRow;
                        (dgr as DataGridRow).IsSelected = false;
                    }
                }
            }
        }
    }
}
