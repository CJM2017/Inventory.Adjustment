// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.Client.QuickBooksClient.Tests
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Inventory.Adjustment.Client.QuickBooksClient;

    [TestClass]
    public class QuickBooksClientTest
    {
        private QuickBooksClient _client;

        [TestInitialize]
        public void Initialize()
        {
            try
            {
                _client = QuickBooksClient.Instance;
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
                return;
            }

            Assert.IsNotNull(_client);
        }

        [TestMethod]
        public async Task TestInitialization()
        {
            var success = await _client.TestConnectionAsync();
            Assert.IsNotNull(success);
            Assert.IsTrue(success);
        }
    }
}
