// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.UI.Infrastructure
{
    using Prism.Events;

    /// <summary>
    /// Singleton class fir accessing application event aggregator
    /// </summary>
    public class ApplicationEventAggregator
    {
        private static ApplicationEventAggregator _instance;
        private IEventAggregator _eventAggregator;

        private ApplicationEventAggregator()
        {
        }

        public static ApplicationEventAggregator Instance => _instance ?? (_instance = new ApplicationEventAggregator());
        public IEventAggregator EventAggregator => _eventAggregator ?? (_eventAggregator = new EventAggregator());
    }
}
