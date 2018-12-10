// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for WorkflowToolbar.xaml
    /// </summary>
    public partial class WorkflowToolbar : UserControl
    {
        /// <summary>
        /// Depedency property for HeaderName
        /// </summary>
        public static readonly DependencyProperty HeaderNameProperty =
            DependencyProperty.Register(
                "HeaderName",
                typeof(string),
                typeof(WorkflowToolbar),
                new FrameworkPropertyMetadata(AddTabControl));

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowToolbar"/> class.
        /// </summary>
        public WorkflowToolbar()
        {
            this.DataContext = this;
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets value of the headername for the toolbar (e.g: Inventory, Purchases, etc)
        /// </summary>
        public string HeaderName
        {
            get => (string)this.GetValue(HeaderNameProperty);
            set => this.SetValue(HeaderNameProperty, value);
        }

        /// <summary>
        /// Create the header label for tab control
        /// TODO: for now only allows a single tab control label at once but eventually need to enable more
        /// </summary>
        /// <param name="o"> the UI control dependency object</param>
        /// <param name="e"> dependency property changed event arguments</param>
        private static void AddTabControl(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var toolbar = o as WorkflowToolbar;
            if (toolbar == null)
            {
                return;
            }

            var tab = new TabItem()
            {
                Name = toolbar.HeaderName,
                Header = toolbar.HeaderName
            };

            toolbar.TabControl.Items.Add(tab);
        }
    }
}
