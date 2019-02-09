// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.UI.Utilities
{
    using System;
    using System.Windows.Data;

    /// <summary>
    /// Initializes a new instance of the <see cref="BoolToOppositeBoolConverter"/> class
    /// Adopted from https://www.codeproject.com/Articles/24330/WPF-Bind-to-Opposite-Boolean-Value-Using-a-Convert
    /// </summary>
    public class BoolToOppositeBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
