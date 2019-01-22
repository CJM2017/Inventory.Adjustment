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

    /// <summary>
    /// View model class for the inventory item list.
    /// </summary>
    public class InventoryItemListViewModel : BindableBase
    {
        private readonly ISessionManager _sessionManager;
        private ObservableCollection<InventoryItem> _items;
        private List<InventoryItem> _selectedItems;
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

            SelectedItems = new List<InventoryItem>();
            Items = _sessionManager.Inventory.Items;
            this.CleanItems();
        }

        public DelegateCommand EditItemCommand { get; private set; }

        public DelegateCommand SearchCommand { get; private set; }

        public DelegateCommand DeleteItemCommand { get; private set; }

        public ObservableCollection<InventoryItem> Items
        {
            get => _items;
            set
            {
                _items = value;
                RaisePropertyChanged();
            }
        }

        public string SearchString
        {
            get => _searchString;
            set
            {
                _searchString = value;

                // Reset
                if (_searchString.Equals(string.Empty))
                {
                    Items = _sessionManager.Inventory.Items;
                }

                SearchCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(); 
            }
        }

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

        private void ExecuteSearch()
        {
            Items = new ObservableCollection<InventoryItem>(_sessionManager.Inventory.Items.Where(item => item.Code.ToLower().Contains(SearchString.ToLower())));
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
            var itemsToDelete = Items.Where(item => item.Code == null).ToList();

            foreach (var item in itemsToDelete)
            {
                Items.Remove(item);
            }
        }
    }
}
