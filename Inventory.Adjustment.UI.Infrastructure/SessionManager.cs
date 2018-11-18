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

    /// <summary>
    /// Instance to hold session data for the application
    /// </summary>
    public class SessionManager : IDisposable, ISessionManager
    {
        private static SessionManager instance;
        private readonly CompositionContainer container;
        private bool disposedValue = false; // To detect redundant calls

        private SessionManager()
        {
            AggregateCatalog catalog = new AggregateCatalog();

            // 1. this adds the executables of the current executable folder to the catalog
            var execPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            catalog.Catalogs.Add(new DirectoryCatalog(execPath, "*.dll"));
            catalog.Catalogs.Add(new DirectoryCatalog(execPath, "*.exe"));

            // 2. this creates the container that is passed to map view and beyond
            this.container = new CompositionContainer(catalog);
        }

        public static SessionManager Instance => instance ?? (instance = new SessionManager());
        public CompositionContainer Container => this.container;

        public void Dispose()
        {
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SessionManager() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }
    }
}
