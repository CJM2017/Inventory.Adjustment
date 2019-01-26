// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.Data.Serializable
{
    using Prism.Mvvm;
    using System.Xml.Serialization;

    public class PriceLevelItem : BindableBase
    {
        private double _price;
        private ItemReference _ref;

        public PriceLevelItem()
        {
            _ref = new ItemReference();
            _price = 0.0;
        }

        /// <summary>
        /// Gets or sets the item reference.
        /// </summary>
        [XmlElement("ItemRef")]
        public ItemReference ItemRef
        {
            get => _ref;
            set
            {
                _ref = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the items custom price.
        /// </summary>
        [XmlElement("CustomPrice")]
        public double CustomPrice
        {
            get => _price;
            set
            {
                _price = value;
                RaisePropertyChanged();
            }
        }
    }
}
