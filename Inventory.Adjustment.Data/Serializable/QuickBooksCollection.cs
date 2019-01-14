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
    public class QuickBooksCollection<T> : BindableBase
    {
        private ObservableCollection<T> _inventoryItems;

        [XmlElement("ItemServiceRet")]
        public ObservableCollection<T> InventoryItems
        {
            get => _inventoryItems;
            set
            {
                _inventoryItems = value;
                RaisePropertyChanged();
            }
        }
    }
}