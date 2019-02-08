﻿// Project      : Inventory Adjusment
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
    using System.Windows.Threading;
    using System.Collections.Specialized;
    using System.Threading.Tasks;

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
            var saveDialog = new SaveItemStatus(this._sessionManager, this._dialogCoordinator, this, itemToModify);
            await this._dialogCoordinator.ShowMetroDialogAsync(this, saveDialog);
        }
    }
}
