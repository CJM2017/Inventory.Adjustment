// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.Data.Serializable
{
    using Prism.Mvvm;
    using System.Xml.Serialization;

    [XmlRoot("PriceLevelModRs")]
    public class QBPriceLevelResponse<T> : BindableBase
    {
        private T _item;

        public QBPriceLevelResponse()
        {
            _item = default(T);
        }

        [XmlElement("PriceLevelRet")]
        public T Item
        {
            get => _item;
            set
            {
                _item = value;
                RaisePropertyChanged();
            }
        }
    }
}
