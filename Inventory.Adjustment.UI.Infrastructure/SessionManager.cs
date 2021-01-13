// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.UI.Infrastructure
{
    using System;
    using System.Reflection;
    using System.ComponentModel.Composition.Hosting;
    using Inventory.Adjustment.UI.Infrastructure.Interfaces;
    using Inventory.Adjustment.Client.QuickBooksClient;
    using log4net;

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
            this._log = LogManager.GetLogger(typeof(SessionManager));
            AggregateCatalog catalog = new AggregateCatalog();

            // 1. this adds the executables of the current executable folder to the catalog
            var execPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            catalog.Catalogs.Add(new DirectoryCatalog(execPath, "*.dll"));
            catalog.Catalogs.Add(new DirectoryCatalog(execPath, "*.exe"));

            // 2. this creates the container that is passed to map view and beyond
            this._container = new CompositionContainer(catalog);

            // Get information on the application 
            this.AppName = Assembly.GetExecutingAssembly().GetName().Name.ToString().Replace(".", " ").Replace("UI", "");
            this.AppVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var client = new QuickBooksClient(string.Empty, $"{this.AppName} v{this.AppVersion}", "US");

            this.InventoryManager = new InventoryManager(client);
        }

        /// <summary>
        /// Gets a singleton instance of this class.
        /// </summary>
        public static ISessionManager Instance => _instance ?? (_instance = new SessionManager());

        /// <inheritdoc/>
        public InventoryManager InventoryManager { get; }

        /// <inheritdoc/>
        public string AppName { get; private set; }

        /// <inheritdoc/>
        public string AppVersion { get; private set; }

        /// <inheritdoc/>
        public CompositionContainer Container => _container;

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
                    this.InventoryManager.Dispose();
                }

                _disposedValue = true;
            }
        }
    }
}
