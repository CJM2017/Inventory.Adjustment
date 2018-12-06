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
    using Interop.QBFC13;

    public class QuickBooksClient : IQuickBooksClient
    {
        private static QuickBooksClient _instance;
        private QBSessionManager _manager;
        private bool _disposedValue = false; // To detect redundant calls

        private string _appName;
        private string _appId;

        private QuickBooksClient()
        {
            _manager = new QBSessionManager();
        }

        /// <summary>
        /// Gets a singleton instance of this class.
        /// </summary>
        public static QuickBooksClient Instance => _instance ?? (_instance = new QuickBooksClient());

        /// <inheritdoc/>
        public ObservableCollection<InventoryItem> GetInventory()
        {
            // TODO
            return null;
        }

        /// <inheritdoc/>
        public InventoryItem CreateInventoryItem(InventoryItem itemToAdd)
        {
            // TODO
            return null;
        }

        /// <inheritdoc/>
        public InventoryItem UpdateInventoryItem(InventoryItem itemToUpdate)
        {
            // TODO
            return null;
        }

        /// <inheritdoc/>
        public InventoryItem DeleteInventoryItem(InventoryItem itemToDelete)
        {
            // TODO
            return null;
        }

        public void Dispose()
        {
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        /// <summary>
        /// Wrapper for test thread.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> TestConnectionAsync()
        {
            return await RunTestsAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Async test to determine if we can communicate
        /// with the quickbooks client.
        /// </summary>
        /// <returns></returns>
        private async Task<bool> RunTestsAsync()
        {
            bool result = true;

            Task.Run(() =>
            {
                try
                {
                    _manager.OpenConnection("", "Inventory Adjustment");
                    _manager.BeginSession("", ENOpenMode.omDontCare);
                    _manager.EndSession();
                    _manager.CloseConnection();
                }
                catch (Exception ex)
                {
                    result = false;
                } 
            }).GetAwaiter().GetResult();

            return result;
        }
    }
}
