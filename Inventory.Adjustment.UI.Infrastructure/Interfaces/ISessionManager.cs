// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.UI.Infrastructure.Interfaces
{
    using System.ComponentModel.Composition.Hosting;

    /// <summary>
    /// Interface for session data for the application
    /// </summary>
    public interface ISessionManager
    {
        CompositionContainer Container { get; }
    }
}
