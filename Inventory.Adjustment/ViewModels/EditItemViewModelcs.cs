// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.UI.ViewModels
{
    using System.Windows.Threading;
    using MahApps.Metro.Controls.Dialogs;
    using Prism.Commands;
    using Prism.Mvvm;

    public class EditItemViewModelcs : BindableBase
    {
        private readonly Dispatcher _dispatcher;
        private readonly InventoryItemListViewModel _inventoryItemListViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditItemViewModelcs"/> class
        /// </summary>
        public EditItemViewModelcs(Dispatcher dispatcher, InventoryItemListViewModel vm)
        {
            this._dispatcher = dispatcher;
            this._inventoryItemListViewModel = vm;
        }

        public DelegateCommand CancelCommand => new DelegateCommand(this.Cancel);

        private void Cancel()
        {
            this._dispatcher.Invoke(async () =>
            {
                BaseMetroDialog dialogOnScreen = await DialogCoordinator.Instance.GetCurrentDialogAsync<BaseMetroDialog>(this._inventoryItemListViewModel);

                if (dialogOnScreen != null)
                {
                    await DialogCoordinator.Instance.HideMetroDialogAsync(this._inventoryItemListViewModel, dialogOnScreen);
                }
            });
        }   
    }
}
