// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.UI.Infrastructure
{
    using System;
    using System.Reflection;
    using System.Collections.ObjectModel;
    using System.ComponentModel.Composition.Hosting;
    using Inventory.Adjustment.Data.Serializable;
    using Inventory.Adjustment.UI.Infrastructure.Interfaces;
    using Inventory.Adjustment.Client.QuickBooksClient;
    using log4net;
    using System.Threading.Tasks;

    /// <summary>
    /// Singleton class for holding session data for the application.
    /// </summary>
    public class SessionManager : ISessionManager
    {
        private readonly ILog _log;
        private static SessionManager _instance;

        private readonly CompositionContainer _container;
        private bool _disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionManager"/> class.
        /// </summary>
        private SessionManager()
        {
            _log = LogManager.GetLogger(typeof(SessionManager));
            AggregateCatalog catalog = new AggregateCatalog();

            // 1. this adds the executables of the current executable folder to the catalog
            var execPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            catalog.Catalogs.Add(new DirectoryCatalog(execPath, "*.dll"));
            catalog.Catalogs.Add(new DirectoryCatalog(execPath, "*.exe"));

            // 2. this creates the container that is passed to map view and beyond
            _container = new CompositionContainer(catalog);

            // Get information on the application 
            AppName = Assembly.GetExecutingAssembly().GetName().Name.ToString().Replace(".", " ").Replace("UI", "");
            AppVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            QBClient = new QuickBooksClient(string.Empty, $"{AppName} v{AppVersion}", "US");

            // Build the mock session
            Items = new ObservableCollection<InventoryItem>();
            BuildMockSession().GetAwaiter();
        }

        /// <summary>
        /// Gets a singleton instance of this class.
        /// </summary>
        public static SessionManager Instance => _instance ?? (_instance = new SessionManager());

        /// <inheritdoc/>
        public string AppName { get; private set; }

        /// <inheritdoc/>
        public string AppVersion { get; private set; }

        /// <inheritdoc/>
        public CompositionContainer Container => _container;

        /// <inheritdoc/>
        public QuickBooksClient QBClient { get; private set; }

        /// <inheritdoc/>
        public ObservableCollection<InventoryItem> Items { get; set; }

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

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SessionManager() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        private async Task BuildMockSession()
        {
            for (int i = 0; i < 20; i++)
            {
                Items.Add(new InventoryItem
                {
                    Code = $"Item-{i}",
                    Cost = i,
                    BasePrice = 2 * i
                });
            }
            await QBClient.OpenConnection();
            await QBClient.GetInventory();
            _log.Debug("Mock session has been created");
        }
    }
}
