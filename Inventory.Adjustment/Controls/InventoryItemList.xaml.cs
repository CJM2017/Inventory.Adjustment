// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.UI.Controls
{
    using System;
    using System.Collections;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Inventory.Adjustment.UI.Infrastructure;
    using Inventory.Adjustment.UI.ViewModels;
    using MahApps.Metro.Controls.Dialogs;

    /// <summary>
    /// Interaction logic for InventoryItemList.xaml
    /// </summary>
    public partial class InventoryItemList : UserControl
    {
        // window that we register for resize events on
        private Window window;
        private InventoryItemListViewModel _viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryItemList"/> class
        /// </summary>
        public InventoryItemList()
        {
            InitializeComponent();

            _viewModel = new InventoryItemListViewModel(
                SessionManager.Instance.InventoryManager,
                DialogCoordinator.Instance,
                this.Dispatcher);

            this.DataContext = _viewModel;

            // call this as a post-load event so that all the fields are set
            this.Loaded += SetPopupStickyLogic;
            this.Unloaded += OnUnload;
        }

        /// <summary>
        /// Provides a way to "dock" the Popup control to the Window
        /// so that the popup "sticks" to the window while the window is dragged around.
        /// See: https://stackoverflow.com/questions/1600218/how-can-i-move-a-wpf-popup-when-its-anchor-element-moves
        /// </summary>
        /// <param name="s">ignored</param>
        /// <param name="e">also ignored</param>
        private void SetPopupStickyLogic(object s, RoutedEventArgs e)
        {
            window = Window.GetWindow(RootGrid);
            if (window != null)
            {
                window.LocationChanged += OnLocationOrSizeChanged;

                // Also handle the window being resized (so the popup's position stays
                //  relative to its target element if the target element moves upon
                //  window resize)
                window.SizeChanged += OnLocationOrSizeChanged;
            }
        }

        private void OnUnload(object s, RoutedEventArgs e)
        {
            if (window != null)
            {
                window.LocationChanged -= OnLocationOrSizeChanged;
                window.SizeChanged -= OnLocationOrSizeChanged;
            }
        }

        private void OnLocationOrSizeChanged(object sender, EventArgs e)
        {
            var offset = LoadingPopup.HorizontalOffset;

            // "bump" the offset to cause the popup to reposition itself on its own
            LoadingPopup.HorizontalOffset = offset + 1;
            LoadingPopup.HorizontalOffset = offset;
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
                if (sender is DataGrid grid && grid.SelectedItems
                    != null && grid.SelectedItems.Count > 0)
                {
                    while (grid.SelectedItems.Count > 0)
                    {
                        try
                        {
                            DataGridRow dgr = 
                                grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItems[0]) as DataGridRow;
                            (dgr as DataGridRow).IsSelected = false;
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}
