﻿// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.Client.QuickBooksClient
{
    using System;
    using System.Threading.Tasks;
    using System.Collections.ObjectModel;
    using Inventory.Adjustment.Data.Serializable;
    using System.Collections.Generic;

    public interface IQuickBooksClient : IDisposable
    {
        /// <summary>
        /// Reads the list of inventory items from local quickbooks
        /// instance using QBFC parsing.
        /// </summary>
        /// <returns>Inventory item list</returns>
        Task<ObservableCollection<InventoryItem>> GetInventoryFromQBFC();

        /// <summary>
        /// Returns a list of data from local quickbooks
        /// instance using XML deserialization.
        /// </summary>
        /// <returns>Inventory item list</returns>
        Task<QBItemCollection<T>> GetInventory<T>();

        /// <summary>
        /// Gets the price level associated with an inventory item.
        /// </summary>
        /// <returns>Price level list</returns>
        Task<QBPriceLevelCollection<T>> GetPriceLevels<T>();

        /// <summary>
        /// Sets the price level associated with an inventory item.
        /// </summary>
        /// <param name="itemName">Inventory item name</param>
        /// /// <param name="priceLevel">Price levvel to modifyparam>
        /// /// <param name="newPrice">New price to set</param>
        /// <returns></returns>
        Task<QBPriceLevelResponse<T>> SetPriceLevel<T>(string itemId, string priceLevelId, string editSequence, double newPrice);

        /// <summary>
        /// Update an existing item in quickbooks.
        /// </summary>
        /// <param name="itemToUpdate">Item in the service to be updated</param>
        /// <returns>The returned item</returns>
        Task<QBItemResponse<T>> UpdateInventoryItem<T>(InventoryItem itemToMod);
        /// <summary>
        /// Create a new inventory item in quickbooks.
        /// </summary>
        /// <param name="itemToAdd">Item to be sent to the service</param>
        /// <returns>The returned item</returns>
        InventoryItem CreateInventoryItem(InventoryItem itemToAdd);

        /// <summary>
        /// Deletes an existing item from quickbooks.
        /// </summary>
        /// <param name="itemToDelete">Item in the service to be deleted</param>
        /// <returns>The returned item</returns>
        InventoryItem DeleteInventoryItem(InventoryItem itemToDelete);

        /// <summary>
        /// Establishes a connection with the quickbooks client.
        /// </summary>
        /// <returns>Task</returns>
        Task OpenConnection();

        /// <summary>
        /// Closes the connection to the quickbooks service.
        /// </summary>
        void CloseConnection();
    }
}
