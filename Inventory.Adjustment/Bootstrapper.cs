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

    /// <summary>
    /// Creates and initializes global services and
    /// creates the main window.
    /// </summary>
    class Bootstrapper
    {
        private SessionManager _manager;

        /// <summary>
        /// Run the application.
        /// </summary>
        public void Run()
        {
            log4net.Config.XmlConfigurator.Configure();
            
            try
            {
                _manager = SessionManager.Instance;

                if (_manager.Container == null)
                {
                    throw new InvalidOperationException("Composition Container for application was not created.");
                }
            }
            catch (QuickBooksClientException ex)
            {
                string errorLabel = "QuickBooks Client Error";
                string errorMessage = "Error occurred: Unable to connect to " +
                                      "the QuickBooks Desktop Application - " +
                                      "Please verify that QuickBooks is also running";

                MessageBox.Show(errorMessage, errorLabel, MessageBoxButton.OK, MessageBoxImage.Error);
                throw ex;
            }

            Application.Current.MainWindow = new MainWindow();
            Application.Current.MainWindow.Show();
            Navigation.Navigate(new Uri("Views/InventoryPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
