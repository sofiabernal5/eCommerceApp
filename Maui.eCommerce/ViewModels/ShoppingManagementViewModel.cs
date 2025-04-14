using Library.eCommerce.Models;
using Library.eCommerce.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Spring2025_Samples.Models;

namespace Maui.eCommerce.ViewModels
{
    public class ShoppingManagementViewModel : INotifyPropertyChanged
    {
        private ProductServiceProxy _invSvc;
        private ShoppingCartService _cartSvc;
        private ObservableCollection<Item?> _inventory;
        private Item? _selectedInventoryItem;
        private Item? _selectedCartItem;
        private string _checkoutMessage;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ShoppingManagementViewModel()
        {
            _invSvc = ProductServiceProxy.Current;
            _cartSvc = ShoppingCartService.Current;
            _inventory = new ObservableCollection<Item?>(_invSvc.Products);
            _checkoutMessage = string.Empty;
            
            // Subscribe to cart changes
            _cartSvc.CartChanged += (s, e) => 
            {
                NotifyPropertyChanged(nameof(ShoppingCart));
                NotifyPropertyChanged(nameof(HasCartItems));
            };
        }

        public ObservableCollection<Item?> Inventory
        {
            get { return _inventory; }
        }

        public ObservableCollection<Item?> ShoppingCart
        {
            get { return _cartSvc.CartItems; }
        }

        public Item? SelectedInventoryItem
        {
            get { return _selectedInventoryItem; }
            set
            {
                _selectedInventoryItem = value;
                NotifyPropertyChanged(nameof(SelectedInventoryItem));
                NotifyPropertyChanged(nameof(HasSelectedInventoryItem));
                NotifyPropertyChanged(nameof(CanAddToCart));
            }
        }

        public Item? SelectedCartItem
        {
            get { return _selectedCartItem; }
            set
            {
                _selectedCartItem = value;
                NotifyPropertyChanged(nameof(SelectedCartItem));
                NotifyPropertyChanged(nameof(HasSelectedCartItem));
            }
        }
        
        public bool HasSelectedInventoryItem => SelectedInventoryItem != null;
        
        public bool HasSelectedCartItem => SelectedCartItem != null;
        
        public bool HasCartItems => ShoppingCart.Count > 0;
        
        public bool CanAddToCart => SelectedInventoryItem != null && SelectedInventoryItem.Quantity > 0;
        
        public string CheckoutMessage
        {
            get { return _checkoutMessage; }
            set
            {
                _checkoutMessage = value;
                NotifyPropertyChanged(nameof(CheckoutMessage));
            }
        }

        // Add selected inventory item to cart
        public void AddToCart()
        {
            if (CanAddToCart)
            {
                _cartSvc.AddToCart(SelectedInventoryItem);
                NotifyPropertyChanged(nameof(CanAddToCart));
            }
        }

        // Remove selected cart item and return to inventory
        public void RemoveFromCart()
        {
            if (HasSelectedCartItem)
            {
                _cartSvc.RemoveFromCart(SelectedCartItem);
                SelectedCartItem = null;
            }
        }
        
        // Process checkout
        public string Checkout()
        {
            if (HasCartItems)
            {
                string receipt = _cartSvc.Checkout();
                RefreshInventory();
                return receipt;
            }
            return "Your cart is empty.";
        }
        
        // Create new inventory item
        public void CreateInventoryItem(string name, double price, int quantity)
        {
            // Generate a new unique ID
            int newId = _inventory.Max(i => i?.Product?.Id ?? 0) + 1;
            
            var newProduct = new Product
            {
                Id = newId,
                Name = name,
                Price = price
            };
            
            var newItem = new Item
            {
                Product = newProduct,
                Quantity = quantity
            };
            
            _invSvc.Products.Add(newItem);
            RefreshInventory();
        }
        
        // Update existing inventory item
        public void UpdateInventoryItem(string name, double price, int quantity)
        {
            if (HasSelectedInventoryItem)
            {
                SelectedInventoryItem.Product.Name = name;
                SelectedInventoryItem.Product.Price = price;
                SelectedInventoryItem.Quantity = quantity;
                
                RefreshInventory();
            }
        }
        
        // Delete inventory item
        public void DeleteInventoryItem()
        {
            if (HasSelectedInventoryItem)
            {
                _invSvc.Products.Remove(SelectedInventoryItem);
                SelectedInventoryItem = null;
                RefreshInventory();
            }
        }
        
        // Update cart item quantity
        public void UpdateCartItemQuantity(int newQuantity)
        {
            if (HasSelectedCartItem)
            {
                _cartSvc.UpdateCartItemQuantity(SelectedCartItem, newQuantity);
            }
        }

        public void RefreshInventory()
        {
            NotifyPropertyChanged(nameof(Inventory));
        }

        public void RefreshShoppingCart()
        {
            NotifyPropertyChanged(nameof(ShoppingCart));
            NotifyPropertyChanged(nameof(HasCartItems));
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}