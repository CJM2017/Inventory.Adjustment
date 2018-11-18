// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.ViewModels
{
    using Prism.Commands;
    using Prism.Mvvm;
    using System;

    /// <summary>
    /// Representation of hamburger menu items
    /// </summary>
    public class MenuItem : BindableBase
    {
        private object icon;
        private string text;
        private string tag;
        private bool isEnabled = true;
        private bool isSelected = false;
        private DelegateCommand command;
        private Uri navDestination;

        /// <summary>
        /// Gets or sets the menu item's icon
        /// </summary>
        public object Icon
        {
            get => this.icon;
            set
            {
                this.icon = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the menu item's text description
        /// </summary>
        public string Text
        {
            get => this.text;
            set
            {
                this.text = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the tag for this menu item
        /// </summary>
        public string Tag
        {
            get => this.tag;
            set
            {
                this.tag = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this menu item is enabled
        /// </summary>
        public bool IsEnabled
        {
            get => this.isEnabled;
            set
            {
                this.isEnabled = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this menu item is selected
        /// </summary>
        public bool IsSelected
        {
            get => this.isSelected;
            set
            {
                this.isSelected = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the command that will fire when this menu item is clicked
        /// </summary>
        public DelegateCommand Command
        {
            get => this.command;
            set
            {
                this.command = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Uri that will be navigated to when this menu item is clicked
        /// </summary>
        public Uri NavDestination
        {
            get => this.navDestination;
            set
            {
                this.navDestination = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this menu item navigates (if navigation destination is set)
        /// or runs a command.
        /// </summary>
        public bool IsNavigation => this.navDestination != null;
    }
}
