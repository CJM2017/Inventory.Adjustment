// Project      : Inventory Adjusment
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
        Task<ObservableCollection<InventoryItem>> GetInventory();

        /// <summary>
        /// Returns a list of data from local quickbooks
        /// instance using XML deserialization.
        /// </summary>
        /// <returns></returns>
        Task<QuickBooksCollection<T>> GetDataFromXML<T>();

        /// <summary>
        /// Create a new inventory item in quickbooks.
        /// </summary>
        /// <param name="itemToAdd">Item to be sent to the service</param>
        /// <returns>The returned item</returns>
        InventoryItem CreateInventoryItem(InventoryItem itemToAdd);

        /// <summary>
        /// Update an existing item in quickbooks.
        /// </summary>
        /// <param name="itemToUpdate">Item in the service to be updated</param>
        /// <returns>The returned item</returns>
        InventoryItem UpdateInventoryItem(InventoryItem itemToUpdate);

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
    }
}
