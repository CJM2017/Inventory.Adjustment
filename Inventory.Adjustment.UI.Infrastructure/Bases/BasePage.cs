// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.UI.Infrastructure.Bases
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using MahApps.Metro.Controls.Dialogs;

    /// <summary>
    /// A base implementation of a WPF page that provides automated
    /// load/unload lifecycle methods
    /// </summary>
    public abstract class BasePage : Page
    {
        // private field to cache navigation service (NavigationService property is null on unloaded)
        private NavigationService cachedService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePage"/> class.
        /// </summary>
        protected BasePage()
        {
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        /// <summary>
        /// Called to determine if the page is ready to be navigated away from.
        /// (e.g: Could return false if a page's has invalid input)
        /// </summary>
        /// <returns>Boolean value indicating whether this page can be navigated away from</returns>
        public abstract bool CanNavigate();

        /// <summary>
        /// Called when this element is loaded. Use this method to register for outside
        /// events and construct disposable resources.
        /// </summary>
        public abstract void DoLoad();

        /// <summary>
        /// Called when this element is unloaded. Use this method to unsubscribe from events
        /// subscribed to and dispose of any resources created in DoLoad.
        /// </summary>
        public abstract void DoUnLoad();

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // if DataContext is null this does nothing (so it's safe)
            DialogParticipation.SetRegister(this, DataContext);
            NavigationService.Navigating += OnNavigating;
            cachedService = NavigationService;
            DoLoad();
        }

        private void OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            if (!CanNavigate())
            {
                NavigationService.StopLoading();
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            DialogParticipation.SetRegister(this, null);
            cachedService.Navigating -= OnNavigating;
            DoUnLoad();
        }
    }
}
