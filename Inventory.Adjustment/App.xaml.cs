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
    
    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        private Bootstrapper bootstrapper;

        /// <summary>
        /// Override for starting up the application.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
            this.Exit += this.OnExit;

            this.bootstrapper = new Bootstrapper();

            try
            {
                this.bootstrapper.Run();
            }
            catch (QuickBooksClientException)
            {
                // Log
                Application.Current.Shutdown();
            }
        }

        private void OnExit(object sender, ExitEventArgs exitEventArgs)
        {
            try
            {
                SessionManager.Instance.Dispose();
            }
            catch (Exception)
            {
                // LOG the error 
            }
        }
    }
}
