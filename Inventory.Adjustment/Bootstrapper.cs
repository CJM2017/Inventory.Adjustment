// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using Inventory.Adjustment.Utilities;
    using Inventory.Adjustment.UI.Infrastructure;

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
            Application.Current.MainWindow = new MainWindow();
            Application.Current.MainWindow.Show();
            Navigation.Navigate(new Uri("Views/InventoryPage.xaml", UriKind.RelativeOrAbsolute));

            _manager = SessionManager.Instance;
            if (_manager.Container == null)
            {
                throw new InvalidOperationException("Composition Container for application was not created.");
            }
        }
    }
}
