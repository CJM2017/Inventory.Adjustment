﻿// Project      : Inventory Adjusment
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
        private bool _isActive;

        private string _name;
        private string _code;
        private string _description;

        private double _stock;
        private double _cost;

        private double _basePrice;
        private double _contractorPrice;
        private double _electricianPrice;

        private Vendor _vendor;
        private DateTime _creationTime;
        private DateTime _lastModifiedTime;

        /// <summary>
        /// Gets any property of the class
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <returns>Object</returns>
        [XmlIgnore]
        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
        }

        /// <summary>
        /// Gets or sets the name for the item.
        /// </summary>
        [XmlElement("IsActive")]
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the name for the item.
        /// </summary>
        [XmlElement("FullName")]
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
        [XmlElement("ManufacturerPartNumber")]
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
        /// Gets or sets the vendor.
        /// </summary>
        [XmlElement("PrefVendorRef")]
        public Vendor Vendor
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
        [XmlElement("PurchaseDesc")]
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
        [XmlElement("Cost")]
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
        [XmlElement("SalesPrice")]
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
        [XmlElement("ContractorPrice")]
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
        [XmlElement("ElectricianPrice")]
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
        [XmlElement("QuantityOnHand")]
        public double Quantity
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
