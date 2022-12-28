// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment
{
    using System;
    using System.Windows;
    using Inventory.Adjustment.Client.QuickBooksClient;
    using Inventory.Adjustment.UI.Infrastructure;
    using log4net;

    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        private ILog _log;
        private Bootstrapper bootstrapper;

        /// <summary>
        /// Override for starting up the application.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this._log = LogManager.GetLogger(typeof(App));
            this._log.Debug("Application is starting up...");

            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
            this.Exit += this.OnExit;

            this.bootstrapper = new Bootstrapper();

            try
            {
                this.bootstrapper.Run();
            }
            catch (QuickBooksClientException ex)
            {
                // Log
                this._log.Error($"{nameof(QuickBooksClientException)} occurred while running the bootstrapper...");
                this._log.Error(ex.ToString());

                Application.Current.Shutdown();
            }
        }

        private void OnExit(object sender, ExitEventArgs exitEventArgs)
        {
            try
            {
                SessionManager.Instance.Dispose();
            }
            catch (Exception ex)
            {
                // LOG the error
                this._log.Error("Exception occurred while disposing the session manager...");
                this._log.Error(ex.ToString());
            }
        }
    }
}
