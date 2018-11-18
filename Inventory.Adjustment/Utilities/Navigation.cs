// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.Utilities
{
    using System;
    using System.Windows.Controls;

    /// <summary>
    /// Utility class to assist with page navigation
    /// </summary>
    public static class Navigation
    {
        private static Frame frame;

        public static Frame Frame
        {
            get => frame;
            set => frame = value;
        }

        /// <summary>
        /// Navigate to the passed page uri
        /// </summary>
        /// <param name="sourcePageUri">URI of page to navigate too</param>
        /// <returns>status of navigation</returns>
        public static bool Navigate(Uri sourcePageUri)
        {
            if (frame.CurrentSource != sourcePageUri)
            {
                //ApplicationEventAggregator.Instance.EventAggregator.GetEvent<PageNavigationEvent>().Publish(sourcePageUri);

                return frame.NavigationService.Navigate(sourcePageUri);
            }

            return true;
        }
    }
}
