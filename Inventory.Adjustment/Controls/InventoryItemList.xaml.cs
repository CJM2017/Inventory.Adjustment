// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using Inventory.Adjustment.UI.ViewModels;

    /// <summary>
    /// Interaction logic for InventoryItemList.xaml
    /// </summary>
    public partial class InventoryItemList : UserControl
    {
        private InventoryItemListViewModel _viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryItemList"/> class
        /// </summary>
        public InventoryItemList()
        {
            InitializeComponent();
            _viewModel = new InventoryItemListViewModel();
            this.DataContext = _viewModel;
        }
    }

    /// <summary>
    /// Data template selector for platforms in draggable list
    /// Only used for this split point dialog, as templates are bound to the xunderlying XAML
    /// </summary>
    public class InventoryDataTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Select template for list box based on data item
        /// </summary>
        /// <param name="item">data item</param>
        /// <param name="container">container holding the list</param>
        /// <returns> DataTempalte for list box</returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            return element.FindResource("NormalListItemDataTemplate") as DataTemplate;
        }
    }
}
