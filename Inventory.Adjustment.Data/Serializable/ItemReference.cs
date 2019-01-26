// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.Data.Serializable
{
    using Prism.Mvvm;
    using System.Xml.Serialization;

    public class ItemReference : BindableBase
    {
        private string _id;
        private string _name;

        public ItemReference()
        {
            _id = string.Empty;
            _name = string.Empty;
        }

        /// <summary>
        /// Gets ors sets the item UID.
        /// </summary>
        [XmlElement("ListId")]
        public string ListId
        {
            get => _id;
            set
            {
                _id = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the items name.
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
    }
}
