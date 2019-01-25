﻿// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.Data.Serializable
{
    using Prism.Mvvm;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot("ItemQueryRs")]
    public class QuickBooksCollection<T> : BindableBase
    {
        private IEnumerable<T> _items;

        [XmlElement("ItemInventoryRet")]
        public IEnumerable<T> Items
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
