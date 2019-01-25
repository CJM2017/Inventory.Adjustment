// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.Data.Serializable
{
    using Prism.Mvvm;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
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

        private double _quantity;
        private double _cost;
        private double _basePrice;


        private Vendor _vendor;
        private ObservableCollection<DataExtension> _customDataItems;
        private DateTime _creationTime;
        private DateTime _lastModifiedTime;

        public InventoryItem()
        {
            _name = string.Empty;
            _code = string.Empty;
            _description = string.Empty;

            _quantity = 0.0;
            _cost = 0.0;
            _basePrice = 0.0;

            _vendor = new Vendor();
            _creationTime = new DateTime();
            _lastModifiedTime = new DateTime();
            _customDataItems = new ObservableCollection<DataExtension>();
        }

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
        /// Gets or sets the time the item was created.
        /// </summary>
        [XmlElement("TimeCreated")]
        public DateTime CreationTime
        {
            get => _creationTime;
            set
            {
                _creationTime = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the time the item was last modified.
        /// </summary>
        [XmlElement("TimeModified")]
        public DateTime LastModified
        {
            get => _lastModifiedTime;
            set
            {
                _lastModifiedTime = value;
                RaisePropertyChanged();
            }
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
        /// Gets or sets the code / identifier for the item.
        /// </summary>
        [XmlElement("FullName")]
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
        [XmlElement("PurchaseCost")]
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
        [XmlIgnore]
        public double ContractorPrice
        {
            get
            {
                double price = 0.0;

                if (CustomDataItems != null && CustomDataItems.Count > 0)
                {
                    string returnedValue = CustomDataItems.FirstOrDefault(item => item.Name.Equals("Price 2")).StringValue;
                    double.TryParse(returnedValue, out price);
                }

                return price;
            }
        }

        /// <summary>
        /// Gets or sets the electrician's price for the item.
        /// </summary>
        [XmlIgnore]
        public double ElectricianPrice
        {
            get
            {
                double price = 0.0;

                if (CustomDataItems != null && CustomDataItems.Count > 0)
                {
                    string returnedValue = CustomDataItems?.FirstOrDefault(item => item.Name.Equals("Price 3")).StringValue;
                    double.TryParse(returnedValue, out price);
                }

                return price;
            }

        }
        /// <summary>
        /// Gets or sets the item count / inventory.
        /// </summary>
        [XmlElement("QuantityOnHand")]
        public double Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                RaisePropertyChanged();
            }
        }

        [XmlElement("DataExtRet")]
        public ObservableCollection<DataExtension> CustomDataItems
        {
            get => _customDataItems;
            set
            {
                _customDataItems = value;
                RaisePropertyChanged();
            }
        }

        private void ItializeCustomItems()
        {
            _customDataItems = new ObservableCollection<DataExtension>();

            for (int i = 2; i <= 3; i++)
            {
                var customItem = new DataExtension
                {
                    Name = $"Price {i}",
                    StringValue = "0.0"
                };

                _customDataItems.Add(customItem);
            }
        }
    }
}
