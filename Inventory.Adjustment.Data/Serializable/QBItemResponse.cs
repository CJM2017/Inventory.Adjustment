// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.Data.Serializable
{
    using Prism.Mvvm;
    using System.Xml.Serialization;

    [XmlRoot("ItemInventoryModRs")]
    public class QBItemResponse<T> : BindableBase
    {
        private T _item;

        public QBItemResponse()
        {
            _item = default(T);
        }

        [XmlElement("ItemInventoryRet")]
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
