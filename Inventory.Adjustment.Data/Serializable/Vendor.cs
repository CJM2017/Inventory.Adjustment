// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.Data.Serializable
{
    using System.Xml.Serialization;

    public class Vendor
    {
        private string _name;

        public Vendor()
        {
            _name = string.Empty;
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
            }
        }
    }
}
