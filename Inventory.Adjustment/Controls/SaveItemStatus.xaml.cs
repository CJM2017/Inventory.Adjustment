// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.UI.Controls
{
    using Inventory.Adjustment.UI.Infrastructure.Interfaces;
    using Inventory.Adjustment.UI.ViewModels;
    using MahApps.Metro.Controls.Dialogs;

    /// <summary>
    /// Interaction logic for SaveItemStatus.xaml
    /// </summary>
    public partial class SaveItemStatus : CustomDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SaveItemStatus"/> class.
        /// </summary>
        public SaveItemStatus(InventoryItemListViewModel vm)
        {
            InitializeComponent();
        }
    }
}
