﻿// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.ViewModels
{
    using System;
    using System.Reflection;
    using System.Linq;
    using Prism.Mvvm;
    using System.Collections.ObjectModel;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;
    using Inventory.Adjustment.UI.Infrastructure;
    using Inventory.Adjustment.UI.Infrastructure.Events;

    /// <summary>
    /// View model for application shell
    /// </summary>
    public class ShellViewModel : BindableBase
    {
        private readonly ObservableCollection<MenuItem> appMenu;
        private readonly ObservableCollection<MenuItem> optionsMenu;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel"/> class
        /// </summary>
        public ShellViewModel()
        {
            AppName = Assembly.GetExecutingAssembly().GetName().Name.ToString().Replace(".", " ");
            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            WindowTitle = $"{AppName} v{Version}";

            appMenu = new ObservableCollection<MenuItem>();
            optionsMenu = new ObservableCollection<MenuItem>();

            BuildAppMenu();

            ApplicationEventAggregator.Instance.EventAggregator.GetEvent<PageNavigationEvent>().Subscribe(ForcePageNaviationHandler);
        }

        /// <summary>
        /// Gets or sets the main window title
        /// </summary>
        public string WindowTitle { get; set; }

        /// <summary>
        /// Gets or sets the applications name
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// Gets or sets the application version number
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets the list of menu items to navigate to
        /// </summary>
        public ObservableCollection<MenuItem> AppMenu => this.appMenu;

        /// <summary>
        /// Gets the list of optional menu items to navigate to
        /// </summary>
        public ObservableCollection<MenuItem> OptionsMenu => this.optionsMenu;

        /// <summary>
        /// Update menu bar items if selected show indicator in UI is properly shown
        /// </summary>
        /// <param name="menuItem">the selected menu item</param>
        public void UpdateActiveMenuItem(MenuItem menuItem)
        {
            foreach (var item in AppMenu.Union(OptionsMenu))
            {
                item.IsSelected = item == menuItem;
            }
        }

        private void ForcePageNaviationHandler(Uri source)
        {
            foreach (var item in AppMenu.Union(OptionsMenu))
            {
                item.IsSelected = item.NavDestination == source;
                item.Text = item.Tag;
            }
        }

        private void BuildAppMenu()
        {
            // Inventory menu item image
            var inventoryImage = new Image
            {
                Height = 24,
                Width = 24,
                Source = new BitmapImage(new Uri("pack://application:,,,/Inventory.Adjustment.UI;Component/Resources/inventory.png"))
            };

            // Add inventory menu item
            this.appMenu.Add(new MenuItem()
            {
                IsSelected = false,
                Icon = inventoryImage,
                Text = string.Empty,
                Tag = "Inventory",
                IsEnabled = true,
                NavDestination = new Uri("Views/InventoryPage.xaml", UriKind.RelativeOrAbsolute)
            });
        }
    }
}
