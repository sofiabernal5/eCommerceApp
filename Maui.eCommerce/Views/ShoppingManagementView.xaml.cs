using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce.Views;

public partial class ShoppingManagementView : ContentPage
{
    public ShoppingManagementView()
    {
        InitializeComponent();
        BindingContext = new ShoppingManagementViewModel();
    }

    private void AddToCartClicked(object sender, EventArgs e)
    {
        if (BindingContext is ShoppingManagementViewModel viewModel && viewModel.SelectedItem != null)
        {
            viewModel.AddToCart();
        }
        else
        {
            DisplayAlert("Error", "No item selected to add.", "OK");
        }
    }
    private void GoBackClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//InventoryManagement");
    }
}