// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.Data.Serializable
{
    using Prism.Mvvm;
    using System;

    /// <summary>
    /// Data model class for an inventory item.
    /// </summary>
    public class InventoryItem : BindableBase
    {
        private string _name;
        private string _code;
        private string _vendor;
        private string _description;

        private int _stock;
        private double _cost;

        private double _basePrice;
        private double _contractorPrice;
        private double _electricianPrice;

        private DateTime _creationTime;
        private DateTime _lastModifiedTime;

        /// <summary>
        /// Gets or sets the name for the item.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

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
        /// Gets or sets the item vendor.
        /// </summary>
        public string Vendor
        {
            get => _vendor;
            set
            {
                _vendor = value;
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
        /// Gets or sets the base price for the item.
        /// </summary>
        public double BasePrice
        {
            get => _basePrice;
            set
            {
                _basePrice = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the contractor's price for the item;
        /// </summary>
        public double ContractorPrice
        {
            get => _contractorPrice;
            set
            {
                _contractorPrice = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the electrician's price for the item.
        /// </summary>
        public double ElectricianPrice
        {
            get => _electricianPrice;
            set
            {
                _electricianPrice = value;
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
