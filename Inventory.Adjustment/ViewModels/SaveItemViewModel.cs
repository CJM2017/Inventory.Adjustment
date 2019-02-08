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
    using Inventory.Adjustment.Client.QuickBooksClient;
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

            Save();
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

        private async Task Save()
        {
            // TODO - pub/sub event driven
            // Set mouse to busy
            UpdateMouse(true);
            var error = false;

            try
            {
                // Update the item itself
                var returnedItem = await this._sessionManager.QBClient.UpdateInventoryItem<InventoryItem>(this._itemToModify);
                returnedItem.Item.ContractorPrice = this._itemToModify.ContractorPrice;
                returnedItem.Item.ElectricianPrice = this._itemToModify.ElectricianPrice;
                UpdateProgress();

                // Update the contactor price level for the item
                var contractorLevel = this._sessionManager.PriceLevels.Items.First(item => item.Name.ToLower().Equals("contractor"));
                var responseContractorLevel = await this._sessionManager.QBClient.SetPriceLevel<PriceLevel>(
                                                                                                           this._itemToModify.ListId,
                                                                                                           contractorLevel.ListId,
                                                                                                           contractorLevel.EditSequence,
                                                                                                           this._itemToModify.ContractorPrice);
                UpdateProgress();

                // Update the electrician price level for the item
                var electricianLevel = this._sessionManager.PriceLevels.Items.First(item => item.Name.ToLower().Equals("electrician"));
                var responseElectricianLevel = await this._sessionManager.QBClient.SetPriceLevel<PriceLevel>(
                                                                                                            this._itemToModify.ListId,
                                                                                                            electricianLevel.ListId,
                                                                                                            electricianLevel.EditSequence,
                                                                                                            this._itemToModify.ElectricianPrice);
                UpdateProgress();

                // Merge the returned source changes into the target session manager
                this._sessionManager.MergeUpdates(returnedItem.Item, responseContractorLevel.Item, responseElectricianLevel.Item);
                UpdateProgress();
            }
            catch (QuickBooksClientException ex)
            {
                // TODO - Log
                error = true;
            }
            finally
            {
                await TearDown(error);
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
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.AppStarting;
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
                string errorMessage = $"Report: Something went wrong while updating Item #: {itemCode}. " +
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