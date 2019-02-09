// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.UI.ViewModels
{
    using Prism.Mvvm;
    using Prism.Commands;
    using System.Linq;
    using System.Collections.ObjectModel;
    using Inventory.Adjustment.Data.Serializable;
    using Inventory.Adjustment.UI.Infrastructure.Interfaces;
    using System.Collections.Generic;
    using System.Collections;
    using System;
    using System.Reflection;
    using Inventory.Adjustment.UI.Controls;
    using MahApps.Metro.Controls.Dialogs;
    using System.Collections.Specialized;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using System.Windows;
    using Inventory.Adjustment.Client.QuickBooksClient;
    using System.Windows.Threading;

    /// <summary>
    /// View model class for the inventory item list.
    /// </summary>
    public class InventoryItemListViewModel : BindableBase
    {
        private readonly ISessionManager _sessionManager;
        private readonly IDialogCoordinator _dialogCoordinator;
        private readonly Dispatcher _dispatcher;

        private ObservableCollection<InventoryItem> _items;
        private List<InventoryItem> _selectedItems;

        private string _selectedField;
        private string _searchString;
        private bool _isSaving;

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryItemListViewModel"/> class
        /// </summary>
        public InventoryItemListViewModel(
            ISessionManager sessionManager, 
            IDialogCoordinator coordinator,
            Dispatcher dispatcher)
        {
            this._sessionManager = sessionManager;
            this._dialogCoordinator = coordinator;
            this._dispatcher = dispatcher;

            CreateItemCommand = new DelegateCommand(this.ExecuteCreate);
            EditItemCommand = new DelegateCommand(this.ExecuteEdit, () => SelectedItems.Any());
            DeleteItemCommand = new DelegateCommand(this.ExecuteDelete, () => Items != null && Items.Count > 0);
            SearchCommand = new DelegateCommand(this.ExecuteSearch, () => SearchString != null && SearchString != string.Empty);

            GridHeaders = new List<string>() { "Code", "Description", "Vendor", "Quantity", "Cost",
                                                "Price", "Contractor Price", "Electrician Price" };

            DropDownOptions = GetDropDownOptions().OrderBy(item => item).ToList();
            SelectedField = DropDownOptions.First();

            SelectedItems = new List<InventoryItem>();
            Items =  new ObservableCollection<InventoryItem>(_sessionManager.Inventory.Items);

            ItemsToModify = new ObservableCollection<InventoryItem>();
            ItemsToModify.CollectionChanged += HandleSave;
            IsSaving = false;
        }

        public DelegateCommand CreateItemCommand { get; private set; }

        public DelegateCommand EditItemCommand { get; private set; }

        public DelegateCommand SearchCommand { get; private set; }

        public DelegateCommand DeleteItemCommand { get; private set; }

        /// <summary>
        /// Item which contains user's modifications which need
        /// to be sent to the QuickBooks service for updates.
        /// </summary>
        public ObservableCollection<InventoryItem> ItemsToModify { get; private set; }

        /// <summary>
        /// Gets or sets whether the inventory item modification was made successfully.
        /// </summary>
        public bool EditsSuccessful { get; set; }

        /// <summary>
        /// Gets the list of gird headers.
        /// </summary>
        public List<string> GridHeaders { get; private set; }

        /// <summary>
        /// Gets or sets the drop down search options.
        /// </summary>
        public List<string> DropDownOptions { get; private set; }

        /// <summary>
        /// Gets or sets whether we're in the saving item state.
        /// </summary>
        public bool IsSaving
        {
            get => this._isSaving;
            set
            {
                this._isSaving = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the selected search field.
        /// </summary>
        public string SelectedField
        {
            get => _selectedField;
            set
            {
                _selectedField = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the collection of inventory items.
        /// </summary>
        public ObservableCollection<InventoryItem> Items
        {
            get => _items;
            set
            {
                _items = value;
                DeleteItemCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }
        
        /// <summary>
        /// Gets or sets the search string.
        /// </summary>
        public string SearchString
        {
            get => _searchString;
            set
            {
                _searchString = value;

                if (string.IsNullOrEmpty(value))
                {
                    Reset();
                }

                SearchCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(); 
            }
        }

        /// <summary>
        /// Gets or sets the selected items.
        /// </summary>
        public List<InventoryItem> SelectedItems
        {
            get => _selectedItems;
            set
            {
                _selectedItems = value;

                EditItemCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Updates the collection of selected items.
        /// </summary>
        /// <param name="items">List from data grid</param>
        public void UpdateSelection(IList items)
        {
            SelectedItems.Clear();
            var tempList = new List<InventoryItem>();

            foreach (object item in items)
            {
                tempList.Add(item as InventoryItem);
            }

            SelectedItems = tempList;
        }

        private IEnumerable<string> GetDropDownOptions()
        {
            Type itemType = typeof(InventoryItem);
            IEnumerable<PropertyInfo> itemProperties = itemType.GetProperties().ToList();

            var options = itemProperties.Where(prop => prop.PropertyType.Name.Equals("String")).Select(prop => prop.Name).ToList();
            options.Remove("ListId");
            options.Remove("EditSequence");
            options.Add("Vendor");

            return options;
        }

        private void ExecuteSearch()
        {
            if (SelectedField.ToLower().Equals("vendor"))
            {
                Items = new ObservableCollection<InventoryItem>(_sessionManager.Inventory.Items.Where(
                                                                item => item.Vendor.Name.ToLower().Contains(
                                                                 SearchString.ToLower())));
            }
            else
            {
                Items = new ObservableCollection<InventoryItem>(_sessionManager.Inventory.Items.Where(
                                                            item => item[SelectedField].ToString().ToLower().Contains(
                                                            SearchString.ToLower())));
            }
        }

        private void Reset()
        {
            SelectedItems = new List<InventoryItem>();
            Items = new ObservableCollection<InventoryItem>(_sessionManager.Inventory.Items);
        }

        private async void ExecuteEdit()
        {
            var editDialog = new EditItem(this._dialogCoordinator, this, SelectedItems.First());
            await this._dialogCoordinator.ShowMetroDialogAsync(this, editDialog);
        }

        private void ExecuteCreate()
        {
            // TODO
        }

        private void ExecuteDelete()
        {
            // TODO
        }

        private async void HandleSave(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    await SaveItem(item as InventoryItem);
                }
            }
        }

        private async Task SaveItem(InventoryItem itemToModify)
        {
            // Sets the saving progress wheel visible
            // and disables grid hittest
            IsSaving = true;

            // Set mouse to busy
            UpdateMouse(true);

            await Task.Run(async () =>
            {
                try
                {
                    // Update the item itself
                    var returnedItem = await this._sessionManager.QBClient.UpdateInventoryItem<InventoryItem>(itemToModify);
                    returnedItem.Item.ContractorPrice = itemToModify.ContractorPrice;
                    returnedItem.Item.ElectricianPrice = itemToModify.ElectricianPrice;

                    // Update the contactor price level for the item
                    var contractorLevel = this._sessionManager.PriceLevels.Items.First(item => item.Name.ToLower().Equals("contractor"));
                    var responseContractorLevel = await this._sessionManager.QBClient.SetPriceLevel<PriceLevel>(
                                                                                                               itemToModify.ListId,
                                                                                                               contractorLevel.ListId,
                                                                                                               contractorLevel.EditSequence,
                                                                                                               itemToModify.ContractorPrice);

                    // Update the electrician price level for the item
                    var electricianLevel = this._sessionManager.PriceLevels.Items.First(item => item.Name.ToLower().Equals("electrician"));
                    var responseElectricianLevel = await this._sessionManager.QBClient.SetPriceLevel<PriceLevel>(
                                                                                                                itemToModify.ListId,
                                                                                                                electricianLevel.ListId,
                                                                                                                electricianLevel.EditSequence,
                                                                                                                itemToModify.ElectricianPrice);

                    // Merge the returned source changes into the target session manager
                    this._sessionManager.MergeUpdates(returnedItem.Item, responseContractorLevel.Item, responseElectricianLevel.Item);
                }
                catch (QuickBooksClientException ex)
                {
                    // TODO - Log
                    ShowErrorMessage(itemToModify.Code);
                }
                finally
                {
                    TearDown();
                }
            }).ConfigureAwait(false);
        }

        private void TearDown()
        {
            ClearQueue();
            UpdateMouse(false);
            IsSaving = false;
        }

        private void ClearQueue()
        {
            while (this.ItemsToModify.Any())
            {
                this.ItemsToModify.Remove(this.ItemsToModify.First());
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
            IsSaving = false;

            this._dispatcher.Invoke(() =>
            {
                string errorLabel = "QuickBooks Client Error";
                string errorMessage = $"Report: Something went wrong while updating Item# {itemCode}. " +
                                       "Please make sure QuickBooks is still open and try again :)";

                MessageBox.Show(errorMessage, errorLabel, MessageBoxButton.OK, MessageBoxImage.Error);
            });
        }
    }
}
