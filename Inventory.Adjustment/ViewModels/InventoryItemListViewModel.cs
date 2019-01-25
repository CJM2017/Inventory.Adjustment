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

    /// <summary>
    /// View model class for the inventory item list.
    /// </summary>
    public class InventoryItemListViewModel : BindableBase
    {
        private readonly ISessionManager _sessionManager;

        private ObservableCollection<InventoryItem> _items;
        private List<InventoryItem> _selectedItems;

        private string _selectedField;
        private string _searchString;

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryItemListViewModel"/> class
        /// </summary>
        public InventoryItemListViewModel(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;

            EditItemCommand = new DelegateCommand(ExecuteEdit, () => SelectedItems.Count > 0);
            SearchCommand = new DelegateCommand(ExecuteSearch, () => SearchString != null && SearchString != string.Empty);
            DeleteItemCommand = new DelegateCommand(ExecuteDelete, () => SelectedItems.Count > 0);

            GridHeaders = new List<string>() { "Code", "Description", "Vendor", "Quantity", "Cost",
                                                "Price", "Contractor Price", "Electrician Price" };

            DropDownOptions = GetDropDownOptions().OrderBy(item => item).ToList();
            SelectedField = DropDownOptions.First();

            SelectedItems = new List<InventoryItem>();
            Items =  new ObservableCollection<InventoryItem>(_sessionManager.Inventory.Items);
            this.CleanItems();  
        }

        public DelegateCommand EditItemCommand { get; private set; }

        public DelegateCommand SearchCommand { get; private set; }

        public DelegateCommand DeleteItemCommand { get; private set; }

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

                // Reset
                if (string.IsNullOrEmpty(value))
                {
                    Items = new ObservableCollection<InventoryItem>(_sessionManager.Inventory.Items);
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
                DeleteItemCommand.RaiseCanExecuteChanged();
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

        private void ExecuteEdit()
        {
            // TODO
        }

        private void ExecuteDelete()
        {
            // TODO
        }

        private void CleanItems()
        {
            var itemsToDelete = Items.Where(item => item.Code == null || !item.IsActive).ToList();
            foreach (var item in itemsToDelete)
            {
                Items.Remove(item);
            }
        }
    }
}
