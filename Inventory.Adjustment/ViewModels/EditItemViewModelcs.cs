// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.UI.ViewModels
{
    using System;
    using System.Windows.Threading;
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
        private readonly InventoryItem _selectedItem;

        private string _Code;
        private bool _buttonsEnabled;

        private double _cost;
        private double _salesPrice;
        private double _electricianPrice;
        private double _contractorPrice;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditItemViewModel"/> class
        /// </summary>
        public EditItemViewModelcs(
            Dispatcher dispatcher, 
            InventoryItemListViewModel vm, 
            InventoryItem selectedItem)
        {
            this._dispatcher = dispatcher;
            this._inventoryItemListViewModel = vm;
            this._selectedItem = selectedItem;

            Initialize();
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

        public DelegateCommand AutoCalcCommand => new DelegateCommand(this.AutoCalc);

        public DelegateCommand SaveCommand => new DelegateCommand(this.Save);

        public DelegateCommand CancelCommand => new DelegateCommand(this.Cancel);

        private void Initialize()
        {
            ButtonsEnabled = true;

            ItemCode = this._selectedItem.Code;
            ItemCost = this._selectedItem.Cost;
            ItemSalesPrice = this._selectedItem.BasePrice;

            ContractorPrice = this._selectedItem.ContractorPrice;
            ElectricianPrice = this._selectedItem.ElectricianPrice;
        }

        private void AutoCalc()
        {
            ContractorPrice = CalculateContractor(ItemCost);
            ElectricianPrice = CalculateElectrician(ItemCost);
        }

        private double CalculateContractor(double cost)
        {
            return ((cost * 3) * 0.6) + 20;
        }

        private double CalculateElectrician(double cost)
        {
            return ((cost * 3) / 2) + 20;
        }

        private void Save()
        {
            // Close first so the next dialog loads smoothly
            CloseDialog();

            // Set the item to modify fields
            this._inventoryItemListViewModel.ItemsToModify.Add(new InventoryItem()
            {
                ListId = this._selectedItem.ListId,
                EditSequence = this._selectedItem.EditSequence,
                Cost = ItemCost,
                BasePrice = ItemSalesPrice,
                ContractorPrice = this.ContractorPrice,
                ElectricianPrice = this.ElectricianPrice
            });
        }

        private void Cancel()
        {
            CloseDialog();
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
