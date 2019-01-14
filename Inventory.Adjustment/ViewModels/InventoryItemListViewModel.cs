// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.UI.ViewModels
{
    using Prism.Mvvm;
    using Prism.Commands;
    using System.Collections.ObjectModel;
    using Inventory.Adjustment.UI.Infrastructure;
    using Inventory.Adjustment.Data.Serializable;

    /// <summary>
    /// View model class for the inventory item list.
    /// </summary>
    public class InventoryItemListViewModel : BindableBase
    {
        private ObservableCollection<InventoryItem> _items;
        private InventoryItem _selectedItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryItemListViewModel"/> class
        /// </summary>
        public InventoryItemListViewModel()
        {
            Items = SessionManager.Instance.Items.InventoryItems;
            DeleteItemCommand = new DelegateCommand(ExecuteDelete, () => SelectedItem != null);
        }

        public DelegateCommand AddItemCommand => new DelegateCommand(ExecuteAdd);

        public DelegateCommand DeleteItemCommand { get; }

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
        /// Gets or sets the selected inventory item in the data grid.
        /// </summary>
        public InventoryItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                RaisePropertyChanged();
                DeleteItemCommand.RaiseCanExecuteChanged();
            }
        }

        private void ExecuteAdd()
        {
            // TODO
        }

        private void ExecuteDelete()
        {
            // TODO
        }
    }
}
