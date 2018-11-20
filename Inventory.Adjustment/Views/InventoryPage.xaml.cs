// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.UI.Views
{
    using System.Windows.Controls;
    using Inventory.Adjustment.UI.ViewModels;

    /// <summary>
    /// Interaction logic for InventoryPage.xaml
    /// </summary>
    public partial class InventoryPage : Page
    {
        private InventoryViewModel viewModel;

        public InventoryPage()
        {
            this.InitializeComponent();
            this.viewModel = new InventoryViewModel();
        }
    }
}
