// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.UI.Views
{
    using Inventory.Adjustment.UI.ViewModels;
    using Inventory.Adjustment.UI.Infrastructure.Bases;

    /// <summary>
    /// Interaction logic for InventoryPage.xaml
    /// </summary>
    public partial class InventoryPage : BasePage
    {
        private InventoryViewModel viewModel;

        public InventoryPage()
        {
            this.InitializeComponent();
            this.viewModel = new InventoryViewModel();
        }

        /// <inheritdoc />
        public override void DoLoad()
        {
            // TODO
        }

        /// <inheritdoc />
        public override void DoUnLoad()
        {
            // TODO
        }

        /// <inheritdoc />
        public override bool CanNavigate()
        {
            return true;
        }
    }
}
