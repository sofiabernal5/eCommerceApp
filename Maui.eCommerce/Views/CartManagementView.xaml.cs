using Library.eCommerce.Services;
using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce.Views;

public partial class CartManagementView : ContentPage
{
    public CartManagementView()
    {
        InitializeComponent();
        BindingContext = new CartViewModel();
    }

    private void AddClicked(object sender, EventArgs e)
    {
        (BindingContext as CartViewModel)?.Add();
    }

    private void DeleteClicked(object sender, EventArgs e)
    {
        (BindingContext as CartViewModel)?.Delete();
    }

    private void CheckoutClicked(object sender, EventArgs e)
    {
        (BindingContext as CartViewModel)?.Checkout();
    }

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }

    private void EditClicked(object sender, EventArgs e)
    {
        var itemId = (sender as Button)?.CommandParameter;
        if (itemId != null)
        {
            Shell.Current.GoToAsync($"//Product?itemId={itemId}");
        }
    }

    private void SearchClicked(object sender, EventArgs e)
    {
        (BindingContext as CartViewModel)?.RefreshCart();
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (BindingContext as CartViewModel)?.RefreshCart();
    }
}