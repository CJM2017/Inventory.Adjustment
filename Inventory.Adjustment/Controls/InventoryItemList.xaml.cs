// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;
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
            InitializeComponent();
            _viewModel = new InventoryItemListViewModel();
            this.DataContext = _viewModel;
        }
    }
}
