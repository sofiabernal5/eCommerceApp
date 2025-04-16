using Maui.eCommerce.Views;
using Microsoft.Maui.Controls;

namespace Maui.eCommerce
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            // Register routes for navigation
            Routing.RegisterRoute(nameof(ProductDetails), typeof(ProductDetails));
            Routing.RegisterRoute(nameof(InventoryManagementView), typeof(InventoryManagementView));
            Routing.RegisterRoute(nameof(ShoppingManagementView), typeof(ShoppingManagementView));
            Routing.RegisterRoute(nameof(ConfigurationView), typeof(ConfigurationView));
        }
    }
}