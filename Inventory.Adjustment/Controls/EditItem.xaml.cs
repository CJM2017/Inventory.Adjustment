// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.UI.Controls
{
    using Inventory.Adjustment.UI.ViewModels;
    using MahApps.Metro.Controls.Dialogs;

    /// <summary>
    /// Interaction logic for EditItem.xaml
    /// </summary>
    public partial class EditItem : CustomDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditItem"/> class.
        /// </summary>
        public EditItem(InventoryItemListViewModel vm)
        {
            InitializeComponent();
            this.DataContext = new EditItemViewModelcs(this.Dispatcher, vm);
        }
    }
}
