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
        }

        /// <summary>
        /// Gets a singleton instance of this class.
        /// </summary>
        public static QuickBooksClient Instance => _instance ?? (_instance = new QuickBooksClient());

        /// <inheritdoc/>
        public QBSessionManager Manager => _manager ?? (_manager = new QBSessionManager());

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

        public async Task<bool> TestConnectionAsync()
        {
            return await RunTestsAsync().ConfigureAwait(false);
        }

        private async Task<bool> RunTestsAsync()
        {
            bool result = true;

            Task.Run(() =>
            {
                try
                {
                    Manager.OpenConnection("", "Test Connection");
                    Manager.BeginSession("", ENOpenMode.omDontCare);
                    Manager.EndSession();
                    Manager.CloseConnection();
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
