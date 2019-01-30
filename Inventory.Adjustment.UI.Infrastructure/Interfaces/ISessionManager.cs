// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.UI.Infrastructure.Interfaces
{
    using System;
    using System.ComponentModel.Composition.Hosting;
    using Inventory.Adjustment.Data.Serializable;
    using Inventory.Adjustment.Client.QuickBooksClient;
    using System.Threading.Tasks;


    /// <summary>
    /// Interface for session data for the application
    /// </summary>
    public interface ISessionManager : IDisposable
    {
        /// <summary>
        /// Gets the application name.
        /// </summary>
        string AppName { get; }

        /// <summary>
        /// Gets the application version number.
        /// </summary>
        string AppVersion { get; }

        /// <summary>
        /// Gets the container.
        /// </summary>
        CompositionContainer Container { get; }

        /// <summary>
        /// Gets the the quickbooks client for the session.
        /// </summary>
        IQuickBooksClient QBClient { get; }

        /// <summary>
        /// Gets or sets the collection of inventory items.
        /// </summary>
        QBItemCollection<InventoryItem> Inventory { get; set; }

        /// <summary>
        /// Gets or sets the inventory price levels.
        /// </summary>
        QBPriceLevelCollection<PriceLevel> PriceLevels { get; set; }

        /// <summary>
        /// Load the inbentory items from quickbooks into this session.
        /// </summary>
        /// <returns>Task</returns>
        Task LoadSessionData();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inventoryItem"></param>
        /// <param name="priceLevelItem"></param>
        bool MergeUpdates(InventoryItem inventoryItem, PriceLevelItem priceLevelItem);
    }
}
