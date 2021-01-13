// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.UI.Infrastructure.Interfaces
{
    using Inventory.Adjustment.Data.Serializable;
    using System;
    using System.Threading.Tasks;

    public interface IInventoryManager : IDisposable
    {
        /// <summary>
        /// Gets the inventory.
        /// </summary>
        QBItemCollection<InventoryItem> Inventory { get; }

        /// <summary>
        /// Gets the price levels.
        /// </summary>
        QBPriceLevelCollection<PriceLevel> PriceLevels { get; set; }

        /// <summary>
        /// Loads the current session inventory.
        /// </summary>
        /// <returns></returns>
        Task LoadSessionData();

        /// <summary>
        /// Updates an inventory item.
        /// </summary>
        /// <param name="itemToModify">The inventory item to modify.</param>
        /// <returns></returns>
        Task UpdateItem(InventoryItem itemToModify);
    }
}
