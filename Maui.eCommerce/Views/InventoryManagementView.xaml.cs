using Library.eCommerce.Services;
using Maui.eCommerce.ViewModels;
using System;

namespace Maui.eCommerce.Views
{
    public partial class InventoryManagementView : ContentPage
    {
        private InventoryManagementViewModel ViewModel => (InventoryManagementViewModel)BindingContext;

        public InventoryManagementView()
        {
            InitializeComponent();
            BindingContext = new InventoryManagementViewModel();
        }

        private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
        {
            ViewModel.RefreshProductList();
        }

        private void SearchClicked(object sender, EventArgs e)
        {
            ViewModel.RefreshProductList();
        }

        private async void AddClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("//ProductDetails");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Navigation Error", $"Could not navigate to product details: {ex.Message}", "OK");
            }
        }

        private async void ItemEditClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is int productId)
            {
                try
                {
                    await Shell.Current.GoToAsync($"//ProductDetails?productId={productId}");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Navigation Error", $"Could not navigate to product details: {ex.Message}", "OK");
                }
            }
        }

        private async void EditClicked(object sender, EventArgs e)
        {
            if (ViewModel.SelectedProduct != null)
            {
                var productId = ViewModel.SelectedProduct.Id;
                try
                {
                    await Shell.Current.GoToAsync($"//ProductDetails?productId={productId}");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Navigation Error", $"Could not navigate to product details: {ex.Message}", "OK");
                }
            }
            else
            {
                await DisplayAlert("Selection Required", "Please select a product to edit", "OK");
            }
        }

        private async void ItemDeleteClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is int productId)
            {
                var product = ProductServiceProxy.Current.Products.FirstOrDefault(p => p?.Id == productId);
                if (product != null)
                {
                    bool confirm = await DisplayAlert("Confirm Delete", 
                        $"Are you sure you want to delete {product.Product?.Name}?", 
                        "Yes", "No");
                    
                    if (confirm)
                    {
                        ProductServiceProxy.Current.Delete(productId);
                        ViewModel.RefreshProductList();
                    }
                }
            }
        }

        private async void DeleteClicked(object sender, EventArgs e)
        {
            if (ViewModel.SelectedProduct != null)
            {
                bool confirm = await DisplayAlert("Confirm Delete", 
                    $"Are you sure you want to delete {ViewModel.SelectedProduct.Product?.Name}?", 
                    "Yes", "No");
                
                if (confirm)
                {
                    ViewModel.Delete();
                }
            }
            else
            {
                await DisplayAlert("Selection Required", "Please select a product to delete", "OK");
            }
        }

        
        private async void CancelClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
        
        private async void GoToShoppingClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//ShoppingManagement");
        }
        private void SortOptionChanged(object sender, EventArgs e)
        {
            ViewModel.RefreshProductList();
        }
        
    }
}