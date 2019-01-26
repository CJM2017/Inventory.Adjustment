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
            var inventory = await _client.GetInventoryFromXML<InventoryItem>();

            Assert.IsNotNull(inventory);
            Assert.IsTrue(inventory.Items.ToList().Count > 0);
        }

        [TestMethod]
        public async Task TestGetPriceLevel()
        {
            var priceLevels = await _client.GetPriceLevelsFromXML<PriceLevel>();

            Assert.IsNotNull(priceLevels);
            Assert.IsTrue(priceLevels.Items.Count > 0);
        }
    }
}
