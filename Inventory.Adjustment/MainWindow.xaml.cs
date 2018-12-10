// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
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
        private readonly ContentControl _fakeControl = new ContentControl();
        private readonly ShellViewModel _viewModel;

        private ViewModels.MenuItem lastMenuItem;
        private ViewModels.MenuItem currentMenuItem;

        public MainWindow()
        {
            _viewModel = new ShellViewModel();
            DataContext = _viewModel;
            this.SizeChanged += WindowResizedHandler;

            Navigation.Frame = new System.Windows.Controls.Frame();
            Navigation.Frame.NavigationStopped += FrameOnNavigationStopped;
            Navigation.Frame.Navigated += FrameOnNavigated;

            Application.Current.MainWindow.FontSize = 15;
    
            InitializeComponent();
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

        private void FrameOnNavigated(object sender, NavigationEventArgs e)
        {
            // need to set twice so mahapps doesn't cache the previous control
            // by setting the context first to our fake control mahapp's "TransitioningContentPresenter"
            // keeps a reference to our fake control instead of the previous page
            HamburgerMenuControl.Content = _fakeControl;
            HamburgerMenuControl.Content = e.Content;

            if (currentMenuItem != null)
            {
                var vm = (ShellViewModel)this.DataContext;
                vm.UpdateActiveMenuItem(currentMenuItem);
            }
        }

        private void FrameOnNavigationStopped(object sender, NavigationEventArgs e)
        {
            var vm = (ShellViewModel)this.DataContext;
            vm.UpdateActiveMenuItem(lastMenuItem);
            currentMenuItem = lastMenuItem;
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
