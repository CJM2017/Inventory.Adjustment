// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.UI.Infrastructure
{
    using Inventory.Adjustment.Client.QuickBooksClient;
    using Inventory.Adjustment.Data.Serializable;
    using Inventory.Adjustment.UI.Infrastructure.Interfaces;
    using log4net;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class InventoryManager : IInventoryManager
    {
        private readonly ILog _log;
        private readonly IQuickBooksClient _qbClient;
        private bool _disposedValue = false; // To detect redundant calls

        public InventoryManager(IQuickBooksClient client)
        {
            this._qbClient = client;
            this._log = LogManager.GetLogger(typeof(InventoryManager));
        }

        /// <inheritdoc/>
        public QBItemCollection<InventoryItem> Inventory { get; private set; } = new QBItemCollection<InventoryItem>();

        /// <inheritdoc/>
        public QBPriceLevelCollection<PriceLevel> PriceLevels { get; set; }

        public async Task LoadSessionData()
        {
            try
            {
                this.Inventory = await _qbClient
                    .GetInventory<InventoryItem>()
                    .ConfigureAwait(false);

                this.PriceLevels = await _qbClient
                    .GetPriceLevels<PriceLevel>()
                    .ConfigureAwait(false);

                this.CleanItems();

                var electricianMap = PriceLevels.Items.First(item => item.Name.ToLower().Equals("electrician")).PriceLevelItems.Select(
                                                             item => new { item.ItemRef.Name, item.CustomPrice }).ToDictionary(
                                                             item => item.Name, item => item.CustomPrice);

                var contractorMap = PriceLevels.Items.First(item => item.Name.ToLower().Equals("contractor")).PriceLevelItems.Select(
                                                            item => new { item.ItemRef.Name, item.CustomPrice }).ToDictionary(
                                                            item => item.Name, item => item.CustomPrice);

                foreach (var item in Inventory.Items)
                {

                    electricianMap.TryGetValue(item.Code, out double price);
                    item.ElectricianPrice = price;

                    contractorMap.TryGetValue(item.Code, out price);
                    item.ContractorPrice = price;
                }

                _log.Debug("Inventory session data has been loaded");
            }
            catch (Exception ex)
            {
                this._log.Error($"Exception occurred while {nameof(InventoryManager)} was loading the session...");
                this._log.Error(ex.ToString());

                throw new QuickBooksClientException(ex.ToString());
            }
        }

        public async Task UpdateItem(InventoryItem itemToModify)
        {
            // Update the item itself
            var returnedItem = await this._qbClient
                .UpdateInventoryItem<InventoryItem>(itemToModify)
                .ConfigureAwait(false);

            returnedItem.Item.ContractorPrice = itemToModify.ContractorPrice;
            returnedItem.Item.ElectricianPrice = itemToModify.ElectricianPrice;

            // Update the contactor price level for the item
            var contractorLevel =
                this.PriceLevels.Items.First(item => item.Name.ToLower().Equals("contractor"));

            var responseContractorLevel =
                await this._qbClient.SetPriceLevel<PriceLevel>(
                    itemToModify.ListId,
                    contractorLevel.ListId,
                    contractorLevel.EditSequence,
                    itemToModify.ContractorPrice);

            // Update the electrician price level for the item
            var electricianLevel =
                this.PriceLevels.Items.First(item => item.Name.ToLower().Equals("electrician"));

            var responseElectricianLevel =
                await this._qbClient.SetPriceLevel<PriceLevel>(
                    itemToModify.ListId,
                    electricianLevel.ListId,
                    electricianLevel.EditSequence,
                    itemToModify.ElectricianPrice);

            // Merge the returned source changes into the target session manager
            this.MergeUpdates(returnedItem.Item, responseContractorLevel.Item, responseElectricianLevel.Item);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    this._qbClient.Dispose();
                }

                _disposedValue = true;
            }
        }

        private void CleanItems()
        {
            var itemsToDelete = Inventory.Items.Where(item => item.Code == null || !item.IsActive).ToList();
            foreach (var item in itemsToDelete)
            {
                this.Inventory.Items.Remove(item);
            }
        }

        private void MergeUpdates(InventoryItem sourceItem, PriceLevel sourceContractor, PriceLevel sourceElectrician)
        {
            // Merge the inventory item
            var target = Inventory.Items.First(item => item.ListId.Equals(sourceItem.ListId));

            target.EditSequence = sourceItem.EditSequence;
            target.Cost = sourceItem.Cost;
            target.BasePrice = sourceItem.BasePrice;
            target.ContractorPrice = sourceItem.ContractorPrice;
            target.ElectricianPrice = sourceItem.ElectricianPrice;

            // Merge contractor price level
            var targetContractor = PriceLevels.Items.First(level => level.ListId.Equals(sourceContractor.ListId));
            targetContractor.EditSequence = sourceContractor.EditSequence;

            // Merge electrician price level
            var targetElectrician = PriceLevels.Items.First(level => level.ListId.Equals(sourceElectrician.ListId));
            targetElectrician.EditSequence = sourceElectrician.EditSequence;
        }
    }
}
