// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.Data.Serializable
{
    using Prism.Mvvm;

    /// <summary>
    /// Data model class for an inventory item.
    /// </summary>
    public class InventoryItem : BindableBase
    {
        private string _code;
        private string _description;
        private double _cost;
        private double _price;
        private int _stock;

        /// <summary>
        /// Gets or sets the code / identifier for the item.
        /// </summary>
        public string Code
        {
            get => _code;
            set
            {
                _code = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the description for the item.
        /// </summary>
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the cost of the item.
        /// </summary>
        public double Cost
        {
            get => _cost;
            set
            {
                _cost = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the selling price for the item.
        /// </summary>
        public double Price
        {
            get => _price;
            set
            {
                _price = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the item count / inventory.
        /// </summary>
        public int Stock
        {
            get => _stock;
            set
            {
                _stock = value;
                RaisePropertyChanged();
            }
        }
    }
}
