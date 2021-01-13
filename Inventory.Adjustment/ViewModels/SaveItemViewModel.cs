// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.UI.ViewModels
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Inventory.Adjustment.Data.Serializable;
    using Inventory.Adjustment.UI.Infrastructure.Interfaces;
    using MahApps.Metro.Controls.Dialogs;
    using Prism.Mvvm;

    public class SaveItemViewModel : BindableBase
    {
        private const int _increment = 20;
        private const int _max = 100;
        private int _progress;

        private readonly ISessionManager _sessionManager;
        private readonly IDialogCoordinator _dialogCoordinator;

        private readonly Dispatcher _dispatcher;
        private readonly InventoryItemListViewModel _inventoryItemListViewModel;
        private readonly InventoryItem _itemToModify;

        public SaveItemViewModel(
            ISessionManager sessionManger,
            IDialogCoordinator dialogCoordinator,
            InventoryItemListViewModel vm,
            Dispatcher dispatcher,
            InventoryItem itemToMod)
        {
            this._progress = 20;
            this._sessionManager = sessionManger;
            this._dialogCoordinator = dialogCoordinator;
            this._inventoryItemListViewModel = vm;
            this._dispatcher = dispatcher;
            this._itemToModify = itemToMod;
        }

        /// <summary>
        /// Gets or sets the status towards completed progress.
        /// </summary>
        public int Progress
        {
            get => this._progress;
            set
            {
                this._progress = value;
                RaisePropertyChanged();
            }
        }

        private void UpdateProgress()
        {
            int newValue = Progress + _increment;
            Progress = newValue < _max ? newValue : _max; 
        }

        private async Task TearDown(bool error)
        {
            ClearQueue();
            await CloseDialog();
            UpdateMouse(false);

            if (error)
            {
                ShowErrorMessage(this._itemToModify.Code);
            }
        }

        private void ClearQueue()
        {
            while (this._inventoryItemListViewModel.ItemsToModify.Any())
            {
                this._inventoryItemListViewModel.ItemsToModify.Remove(this._inventoryItemListViewModel.ItemsToModify.First());
            }
        }

        private void UpdateMouse(bool busy)
        {
            this._dispatcher.Invoke(() =>
            {
                if (busy)
                {
                    Mouse.OverrideCursor = Cursors.AppStarting;
                }
                else
                {
                    Mouse.OverrideCursor = null;
                }
            });
        }

        private void ShowErrorMessage(string itemCode)
        {
            this._dispatcher.Invoke(() =>
            {
                string errorLabel = "QuickBooks Client Error";
                string errorMessage = $"Report: Something went wrong while updating Item-# {itemCode}. " +
                                       "Please check your connection to QuickBooks and try again!";

                MessageBox.Show(errorMessage, errorLabel, MessageBoxButton.OK, MessageBoxImage.Error);
            });
        }

        private async Task CloseDialog()
        {
            var dialogOnScreen = await this._dialogCoordinator.GetCurrentDialogAsync<BaseMetroDialog>(this);

            if (dialogOnScreen != null)
            {
                await this._dialogCoordinator.HideMetroDialogAsync(this, dialogOnScreen);
            }
        }
    }
}