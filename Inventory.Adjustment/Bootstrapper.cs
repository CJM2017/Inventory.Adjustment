// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment
{
    using System;
    using System.Windows;
    using Inventory.Adjustment.Utilities;
    using Inventory.Adjustment.UI.Infrastructure;
    using Inventory.Adjustment.Client.QuickBooksClient;
    using Inventory.Adjustment.UI.Infrastructure.Interfaces;
    using System.Threading.Tasks;

    /// <summary>
    /// Creates and initializes global services and
    /// creates the main window.
    /// </summary>
    public class Bootstrapper
    {
        private ISessionManager _manager;

        /// <summary>
        /// Run the application.
        /// </summary>
        public void Run()
        {
            log4net.Config.XmlConfigurator.Configure();
            
            try
            {
                _manager = SessionManager.Instance;

                Task.Run(async () =>
                {
                    await _manager.LoadSessionData();
                }).GetAwaiter().GetResult();
                
                if (_manager.Container == null)
                {
                    throw new InvalidOperationException("Composition Container for application was not created.");
                }
            }
            catch (QuickBooksClientException ex)
            {
                throw ex;
            }

            Application.Current.MainWindow = new MainWindow();
            Application.Current.MainWindow.Show();
            Navigation.Navigate(new Uri("Views/InventoryPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
