// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.UI.Controls
{
    using Inventory.Adjustment.Data.Serializable;
    using Inventory.Adjustment.UI.Infrastructure.Interfaces;
    using Inventory.Adjustment.UI.ViewModels;
    using MahApps.Metro.Controls.Dialogs;
    using System.Threading.Tasks;

    /// <summary>
    /// Interaction logic for SaveItemStatus.xaml
    /// </summary>
    public partial class SaveItemStatus : CustomDialog
    {
        private readonly SaveItemViewModel _viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveItemStatus"/> class.
        /// </summary>
        public SaveItemStatus(
            ISessionManager sessionManager,
            IDialogCoordinator dialogCoordinator,
            InventoryItemListViewModel vm,
            InventoryItem itemToMod)
        {
            InitializeComponent();
            this._viewModel = new SaveItemViewModel(sessionManager, dialogCoordinator, vm, this.Dispatcher, itemToMod);
            this.DataContext = this._viewModel;
        }
    }
}
