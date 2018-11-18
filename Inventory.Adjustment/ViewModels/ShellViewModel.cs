// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.ViewModels
{
    using System;
    using System.Reflection;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Prism.Mvvm;

    public class ShellViewModel : BindableBase
    {
        public ShellViewModel()
        {
            AppName = Assembly.GetExecutingAssembly().GetName().Name.ToString().Replace(".", " ");
            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            WindowTitle = $"{AppName} v{Version}";
        }

        public string WindowTitle { get; set; }

        public string AppName { get; set; }

        public string Version { get; set; }
    }
}
