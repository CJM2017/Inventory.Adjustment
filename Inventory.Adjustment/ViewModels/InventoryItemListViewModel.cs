// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.UI.ViewModels
{
    using System;
    using Prism.Mvvm;
    using Prism.Commands;
    using System.Collections.ObjectModel;
    using Inventory.Adjustment.UI.Infrastructure;
    using Inventory.Adjustment.Data.Serializable;

    /// <summary>
    /// View model class for the inventory item list.
    /// </summary>
    class InventoryItemListViewModel : BindableBase
    {
        private ObservableCollection<InventoryItem> _items;

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryItemListViewModel"/> class
        /// </summary>
        public InventoryItemListViewModel()
        {
            Items = SessionManager.Instance.Items;
        }

        public ObservableCollection<InventoryItem> Items
        {
            get => _items;
            set
            {
                _items = value;
                RaisePropertyChanged();
            }
        }
    }
}
