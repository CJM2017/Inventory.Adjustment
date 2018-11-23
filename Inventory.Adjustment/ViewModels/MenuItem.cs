// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.ViewModels
{
    using System;
    using Prism.Mvvm;
    using Prism.Commands;

    /// <summary>
    /// Representation of hamburger menu items
    /// </summary>
    public class MenuItem : BindableBase
    {
        private object _icon;
        private string _text;
        private string _tag;
        private bool _isEnabled = true;
        private bool _isSelected = false;
        private DelegateCommand _command;
        private Uri _navDestination;

        /// <summary>
        /// Gets or sets the menu item's icon
        /// </summary>
        public object Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the menu item's text description
        /// </summary>
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the tag for this menu item
        /// </summary>
        public string Tag
        {
            get => _tag;
            set
            {
                _tag = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this menu item is enabled
        /// </summary>
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this menu item is selected
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the command that will fire when this menu item is clicked
        /// </summary>
        public DelegateCommand Command
        {
            get => _command;
            set
            {
                _command = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Uri that will be navigated to when this menu item is clicked
        /// </summary>
        public Uri NavDestination
        {
            get => _navDestination;
            set
            {
                _navDestination = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this menu item navigates (if navigation destination is set)
        /// or runs a command.
        /// </summary>
        public bool IsNavigation => _navDestination != null;
    }
}
