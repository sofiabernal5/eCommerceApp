using Maui.eCommerce.ViewModels;
using Microsoft.Maui.Controls;
using System;

namespace Maui.eCommerce.Views
{
    public partial class ShoppingManagementView : ContentPage
    {
        private ShoppingManagementViewModel ViewModel => (ShoppingManagementViewModel)BindingContext;

        public ShoppingManagementView()
        {
            InitializeComponent();
            BindingContext = new ShoppingManagementViewModel();
        }

        private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
        {
            ViewModel.RefreshInventory();
            ViewModel.RefreshShoppingCart();
        }

        private async void CreateInventoryItemClicked(object sender, EventArgs e)
        {
            // Show dialog to create new item
            string name = await DisplayPromptAsync("Create Item", "Enter product name:", initialValue: "New Product");
            if (string.IsNullOrEmpty(name)) return;
            
            string priceStr = await DisplayPromptAsync("Create Item", "Enter product price:", initialValue: "9.99");
            if (!double.TryParse(priceStr, out double price)) return;
            
            string quantityStr = await DisplayPromptAsync("Create Item", "Enter quantity:", initialValue: "10");
            if (!int.TryParse(quantityStr, out int quantity)) return;
            
            ViewModel.CreateInventoryItem(name, price, quantity);
        }

        private async void UpdateInventoryItemClicked(object sender, EventArgs e)
        {
            if (ViewModel.SelectedInventoryItem == null) return;
            
            // Show dialog to update item
            string name = await DisplayPromptAsync("Update Item", "Enter product name:", 
                initialValue: ViewModel.SelectedInventoryItem.Product.Name);
            if (string.IsNullOrEmpty(name)) return;
            
            string priceStr = await DisplayPromptAsync("Update Item", "Enter product price:", 
                initialValue: ViewModel.SelectedInventoryItem.Product.Price.ToString());
            if (!double.TryParse(priceStr, out double price)) return;
            
            string quantityStr = await DisplayPromptAsync("Update Item", "Enter quantity:", 
                initialValue: ViewModel.SelectedInventoryItem.Quantity.ToString());
            if (!int.TryParse(quantityStr, out int quantity)) return;
            
            ViewModel.UpdateInventoryItem(name, price, quantity);
        }

        private async void DeleteInventoryItemClicked(object sender, EventArgs e)
        {
            if (ViewModel.SelectedInventoryItem == null) return;
            
            bool confirm = await DisplayAlert("Confirm Delete", 
                $"Are you sure you want to delete {ViewModel.SelectedInventoryItem.Product.Name}?", 
                "Yes", "No");
                
            if (confirm)
            {
                ViewModel.DeleteInventoryItem();
            }
        }

        private void AddToCartClicked(object sender, EventArgs e)
        {
            ViewModel.AddToCart();
        }
        
        private void QuickAddToCartClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Library.eCommerce.Models.Item item)
            {
                // Get the parent StackLayout
                var stackLayout = button.Parent as StackLayout;
                if (stackLayout != null)
                {
                    // Find the quantity entry in the StackLayout's children
                    var entry = stackLayout.Children.FirstOrDefault(c => c is Entry) as Entry;
                    if (entry != null && int.TryParse(entry.Text, out int quantity) && quantity > 0)
                    {
                        // Call the ViewModel method to add the specified quantity
                        ViewModel.QuickAddToCart(item, quantity);
                        
                        // Reset the entry to "1" after adding
                        entry.Text = "1";
                    }
                }
            }
        }
        
        private async void UpdateCartItemClicked(object sender, EventArgs e)
        {
            if (ViewModel.SelectedCartItem == null) return;
            
            // Show dialog to update quantity
            string quantityStr = await DisplayPromptAsync("Update Quantity", "Enter new quantity:", 
                initialValue: ViewModel.SelectedCartItem.Quantity.ToString());
                
            if (int.TryParse(quantityStr, out int quantity))
            {
                ViewModel.UpdateCartItemQuantity(quantity);
            }
        }
        
        private void RemoveFromCartClicked(object sender, EventArgs e)
        {
            ViewModel.RemoveFromCart();
        }

        private async void CheckoutClicked(object sender, EventArgs e)
        {
            string receipt = ViewModel.Checkout();
            await DisplayAlert("Checkout Complete", receipt, "OK");
        }
        
        private async void CancelClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
    }
}