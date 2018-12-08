// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.Client.QuickBooksClient
{
    using System;

    class QuickBooksClientException : Exception
    {
        public QuickBooksClientException()
        {
        }

        public QuickBooksClientException(string message)
            : base(message)
        {
        }
    }
}
