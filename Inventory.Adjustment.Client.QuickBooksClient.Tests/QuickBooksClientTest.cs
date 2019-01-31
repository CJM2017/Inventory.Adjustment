// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.Client.QuickBooksClient.Tests
{
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Inventory.Adjustment.Client.QuickBooksClient;
    using Inventory.Adjustment.Data.Serializable;
    using System.Linq;

    [TestClass]
    public class QuickBooksClientTest
    {
        private IQuickBooksClient _client;

        [TestInitialize]
        public void Initialize()
        {
            _client = new QuickBooksClient(string.Empty, "Inventory Adjustment v1.0.0", "US");
            Assert.IsNotNull(_client);
        }

        [TestMethod]
        public async Task TestGetInventory()
        {
            var inventory = await _client.GetInventory<InventoryItem>();

            Assert.IsNotNull(inventory);
            Assert.IsTrue(inventory.Items.ToList().Count > 0);
        }

        [TestMethod]
        public async Task TestGetPriceLevel()
        {
            var priceLevels = await _client.GetPriceLevels<PriceLevel>();

            Assert.IsNotNull(priceLevels);
            Assert.IsTrue(priceLevels.Items.Count > 0);
        }

        [TestMethod]
        public async Task TestUpdateInventoryItem()
        {
            var inventory = await _client.GetInventory<InventoryItem>();
            var itemToMod = inventory.Items.First(item => item.Code == "71564a");
            var returnedItem = await _client.UpdateInventoryItem<InventoryItem>(itemToMod);

            Assert.IsNotNull(returnedItem);
        }

        [TestMethod]
        public async Task TestSetPriceLevel()
        {
            var inventory = await _client.GetInventory<InventoryItem>();
            var priceLevels = await _client.GetPriceLevels<PriceLevel>();

            var itemToMod = inventory.Items.First(item => item.Code == "71564a");
            var contractorLevel = priceLevels.Items.First(item => item.Name.ToLower().Equals("contractor"));
            var electricianevel = priceLevels.Items.First(item => item.Name.ToLower().Equals("electrician"));

            var response1 = await _client.SetPriceLevel<PriceLevel>(itemToMod.ListId, contractorLevel.ListId, contractorLevel.EditSequence, 100.00);
            var response2 = await _client.SetPriceLevel<PriceLevel>(itemToMod.ListId, electricianevel.ListId, electricianevel.EditSequence, 200.00);

            Assert.IsNotNull(response1);
            Assert.IsNotNull(response2);
        }
    }
}
