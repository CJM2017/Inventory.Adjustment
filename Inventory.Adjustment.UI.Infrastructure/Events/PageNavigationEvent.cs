// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.UI.Infrastructure.Events
{
    using System;
    using Prism.Events;
    
    /// <summary>
    /// Event for sending notifications when a new route is created
    /// </summary>
    public class PageNavigationEvent : PubSubEvent<Uri>
    {
    }
}
