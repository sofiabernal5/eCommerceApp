using Library.eCommerce.Models;
using Library.eCommerce.Services;
using Spring2025_Samples.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Maui.eCommerce.ViewModels
{
    public class CartViewModel : INotifyPropertyChanged
    {
        public Item? SelectedItem { get; set; }
        public string? Query { get; set; }
        public string? ErrorMessage { get; set; }
        public string? TotalPrice { get; set; }

        public CartViewModel()
        {
            RefreshCart();
        }

        // Returns a list of items in the cart as ObservableCollection
        public ObservableCollection<Item> CartItems
        {
            get
            {
                return new ObservableCollection<Item>(ShoppingCartService.Current.CartItems);
            }
        }

        // Refresh the cart and calculate the total price
        public void RefreshCart()
        {
            NotifyPropertyChanged(nameof(CartItems));
            CalculateTotal();
        }

        // Delete the selected item from the cart
        public void Delete()
        {
            if (SelectedItem != null)
            {
                ShoppingCartService.Current.RemoveItem(SelectedItem.Id);
                RefreshCart();
            }
        }

        // Clear the entire cart
        public void Clear()
        {
            ShoppingCartService.Current.ClearCart();
            RefreshCart();
        }

        // Checkout and clear the cart if items exist
        public void Checkout()
        {
            if (CartItems.Any())
            {
                ShoppingCartService.Current.Checkout();
                RefreshCart();
            }
            else
            {
                ErrorMessage = "Cart is empty. Add items before checkout.";
            }
        }

        // Add a new item to the cart
        public void Add()
        {
            var newItem = new Item
            {
                Id = ShoppingCartService.Current.CartItems.Count + 1,
                Name = $"Item {ShoppingCartService.Current.CartItems.Count + 1}",
                Price = ShoppingCartService.Current.CartItems.Sum(i => i.Price),
                Quantity = ShoppingCartService.Current.CartItems.Count + 1,
            };

            ShoppingCartService.Current.AddItem(newItem);
            RefreshCart();
        }

        // Calculate the total price of items in the cart
        private void CalculateTotal()
        {
            double total = CartItems.Sum(item => item?.Product?.Price ?? 0);
            TotalPrice = $"Total: ${total:F2}";
        }

        // Notify property changed to update UI
        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}