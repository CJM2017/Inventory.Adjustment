// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.Data.Serializable
{
    using Prism.Mvvm;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;

    public class PriceLevel : BindableBase
    {
        private bool _isActive;
        private string _id;
        private string _name;
        private string _editSequence;
        private ObservableCollection<PriceLevelItem> _priceLevelItems;

        /// <summary>
        /// Creates new instance.
        /// </summary>
        public PriceLevel()
        {
            _isActive = true;
            _name = string.Empty;
            _id = string.Empty;
            _editSequence = string.Empty;
            _priceLevelItems = new ObservableCollection<PriceLevelItem>();
        }

        /// <summary>
        /// Gets or sets the list id.
        /// </summary>
        [XmlElement("ListID")]
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
        /// Gets or sets the list id.
        /// </summary>
        [XmlElement("EditSequence")]
        public string EditSequence
        {
            get => _editSequence;
            set
            {
                _editSequence = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the active flag.
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
        /// Gets or sets the price level name.
        /// </summary>
        [XmlElement("Name")]
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
        /// Gets or sets the list of price level items.
        /// </summary>
        [XmlElement("PriceLevelPerItemRet")]
        public ObservableCollection<PriceLevelItem> PriceLevelItems
        {
            get => _priceLevelItems;
            set
            {
                _priceLevelItems = value;
                RaisePropertyChanged();
            }
        }
    }
}
