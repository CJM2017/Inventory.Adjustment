// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;

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

            this.bootstrapper = new Bootstrapper();
            this.bootstrapper.Run();

            this.Exit += this.OnExit;
            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        private void OnExit(object sender, ExitEventArgs exitEventArgs)
        {
            // TODO
        }
    }
}
