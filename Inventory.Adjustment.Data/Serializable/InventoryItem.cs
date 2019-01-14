// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.Data.Serializable
{
    using Prism.Mvvm;
    using System;
    using System.Xml.Serialization;

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
        [XmlAttribute("FullName")]
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
        [XmlAttribute("ManufacturerPartNumber")]
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
        [XmlAttribute("Description")]
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
        [XmlAttribute("Cost")]
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
        [XmlAttribute("SalesPrice")]
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
        [XmlAttribute("ContractorPrice")]
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
        [XmlAttribute("ElectricianPrice")]
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
        [XmlAttribute("QuantityOnHand")]
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
