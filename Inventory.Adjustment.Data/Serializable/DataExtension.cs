// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.Data.Serializable
{
    using Prism.Mvvm;
    using System.Xml.Serialization;

    public class DataExtension : BindableBase
    {
        private string _name;
        private string _value;

        public DataExtension()
        {
            _name = string.Empty;
            _value = string.Empty;
        }

        /// <summary>
        /// Gets or sets the data extension's name.
        /// </summary>
        [XmlElement("DataExtName")]
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
        /// Gets or sets the data extension's value.
        /// </summary>
        [XmlElement("DataExtValue")]
        public string StringValue
        {
            get => _value;
            set
            {
                _value = value;
                RaisePropertyChanged();
            }
        }
    }
}
