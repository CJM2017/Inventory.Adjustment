// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.UI.ViewModels
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;
    using Inventory.Adjustment.Client.QuickBooksClient;
    using Inventory.Adjustment.Data.Serializable;
    using Inventory.Adjustment.UI.Infrastructure.Interfaces;
    using MahApps.Metro.Controls.Dialogs;
    using Prism.Commands;
    using Prism.Mvvm;

    public class EditItemViewModelcs : BindableBase
    {
        private readonly Dispatcher _dispatcher;
        private readonly InventoryItemListViewModel _inventoryItemListViewModel;
        private readonly ISessionManager _sessionManger;
        private readonly InventoryItem _itemToEdit;

        private string _Code;
        private bool _autoCalc;
        private bool _buttonsEnabled;

        private double _cost;
        private double _salesPrice;
        private double _electricianPrice;
        private double _contractorPrice;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditItemViewModelcs"/> class
        /// </summary>
        public EditItemViewModelcs(
            Dispatcher dispatcher, 
            InventoryItemListViewModel vm, 
            ISessionManager sessionManager, 
            InventoryItem itemToEdit)
        {
            this._dispatcher = dispatcher;
            this._inventoryItemListViewModel = vm;
            this._sessionManger = sessionManager;
            this._itemToEdit = itemToEdit;

            Initialize();
        }

        /// <summary>
        /// Gets or sets the item's code.
        /// </summary>
        public bool AutoCalc
        {
            get => this._autoCalc;
            set
            {
                this._autoCalc = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the item's code.
        /// </summary>
        public string ItemCode
        {
            get => this._Code;
            set
            {
                this._Code = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the item's cost.
        /// </summary>
        public double ItemCost
        {
            get => this._cost;
            set
            {
                this._cost = Math.Round(value, 2);
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the item's sales price.
        /// </summary>
        public double ItemSalesPrice
        {
            get => this._salesPrice;
            set
            {
                this._salesPrice = Math.Round(value, 2);
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the item's sales price.
        /// </summary>
        public double ElectricianPrice
        {
            get => this._electricianPrice;
            set
            {
                this._electricianPrice = Math.Round(value, 2);
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the item's sales price.
        /// </summary>
        public double ContractorPrice
        {
            get => this._contractorPrice;
            set
            {
                this._contractorPrice = Math.Round(value, 2);
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the state of the dialog buttons
        /// </summary>
        public bool ButtonsEnabled
        {
            get => this._buttonsEnabled;
            set
            {
                this._buttonsEnabled = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand SaveCommand => new DelegateCommand(this.Save);

        public DelegateCommand CancelCommand => new DelegateCommand(this.Cancel);

        private void Initialize()
        {
            AutoCalc = true;
            ButtonsEnabled = true;

            ItemCode = this._itemToEdit.Code;
            ItemCost = this._itemToEdit.Cost;
            ItemSalesPrice = this._itemToEdit.BasePrice;

            ContractorPrice = AutoCalc ? CalculateContractor(ItemCost) : this._itemToEdit.ContractorPrice;
            ElectricianPrice = AutoCalc ? CalculateElectrician(ItemCost) : this._itemToEdit.ElectricianPrice;
        }

        private double CalculateContractor(double cost)
        {
            return ((cost * 3) * 0.6) + 20;
        }

        private double CalculateElectrician(double cost)
        {
            return ((cost * 3) / 2) + 20;
        }

        private async void Save()
        {
            var returnedItem = default(InventoryItem);
            var priceLevelItem = default(PriceLevelItem);

            // Prevent the user from clicking again
            DisableButtons();

            try
            {
                // Update the item in inventory
                await this._sessionManger.QBClient.UpdateInventoryItem(this._itemToEdit.ListId, this._itemToEdit.EditSequence, ItemCost, ItemSalesPrice);

                // Update the contactor price level for the item
                var contractorLevel = this._sessionManger.PriceLevels.Items.First(item => item.Name.ToLower().Equals("contractor"));
                await this._sessionManger.QBClient.SetPriceLevelWithXML(this._itemToEdit.ListId, contractorLevel.ListId, contractorLevel.EditSequence, ContractorPrice);

                // Update the electrician price level for the item
                var electricianLevel = this._sessionManger.PriceLevels.Items.First(item => item.Name.ToLower().Equals("electrician"));
                await this._sessionManger.QBClient.SetPriceLevelWithXML(this._itemToEdit.ListId, electricianLevel.ListId, electricianLevel.EditSequence, ElectricianPrice);

                // Merge the returned changes into the session manager
                this._sessionManger.MergeUpdates(returnedItem, priceLevelItem);
            }
            catch (QuickBooksClientException ex)
            {
                CloseDialog();

                this._dispatcher.Invoke(() => ShowErrorMessage());
            }
            finally
            {
                CloseDialog();
            }
        }

        private void Cancel()
        {
            CloseDialog();
        }
        
        private void DisableButtons()
        {
            ButtonsEnabled = false;
        }

        private void ShowErrorMessage()
        {
            string errorLabel = "QuickBooks Client Error";
            string errorMessage = $"Report: Something went wrong while updating Item # {this._itemToEdit.Code}. " +
                                   "Please check your connection to QuickBooks and try again";

            MessageBox.Show(errorMessage, errorLabel, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void CloseDialog()
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
