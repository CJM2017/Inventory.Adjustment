// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using MahApps.Metro.Controls;
    using Inventory.Adjustment.ViewModels;
    using Inventory.Adjustment.Utilities;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        // a private unpopulated control used to prevent MahApp's from caching
        // previously navigated pages
        private readonly ContentControl fakeControl = new ContentControl();
        private readonly ShellViewModel viewModel;

        private ViewModels.MenuItem lastMenuItem;
        private ViewModels.MenuItem currentMenuItem;

        public MainWindow()
        {
            this.InitializeComponent();

            this.viewModel = new ShellViewModel();
            this.DataContext = viewModel;
            this.SizeChanged += WindowResizedHandler;

            Application.Current.MainWindow.FontSize = 15;
        }

        private void CenterWindowOnScreen()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;

            double windowWidth = this.Width;
            double windowHeight = this.Height;

            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        private void WindowResizedHandler(object sender, SizeChangedEventArgs e)
        {
            // TODO
        }

        private void HandleClick(object sender, ItemClickEventArgs e)
        {
            lastMenuItem = currentMenuItem;
            currentMenuItem = e.ClickedItem as ViewModels.MenuItem;

            if (currentMenuItem != null && currentMenuItem.IsNavigation)
            {
                Navigation.Navigate(currentMenuItem.NavDestination);
            }
        }

        private void HamburgerMenuControlOnItemClick(object sender, ItemClickEventArgs e)
        {
            HandleClick(sender, e);
        }

        private void HamburgerMenuControlOnOptionItemClick(object sender, ItemClickEventArgs e)
        {
            HandleClick(sender, e);
        }
    }
}
