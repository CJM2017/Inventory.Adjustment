﻿// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.Data.Serializable
{
    using Prism.Mvvm;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;

    [XmlRoot("ItemQueryRs")]
    public class QBItemCollection<T> : BindableBase
    {
        private ObservableCollection<T> _items;

        public QBItemCollection()
        {
            _items = new ObservableCollection<T>();
        }

        [XmlElement("ItemInventoryRet")]
        public ObservableCollection<T> Items
        {
            get => _items;
            set
            {
                _items = value;
                RaisePropertyChanged();
            }
        }
    }
}
