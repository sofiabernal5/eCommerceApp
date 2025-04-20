using Library.eCommerce.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Library.eCommerce.Services
{
    public class ShoppingCartService
    {
        private static ShoppingCartService? _instance;
        private Dictionary<string, ObservableCollection<Item?>> _carts = new Dictionary<string, ObservableCollection<Item?>>();
        private string _currentCart = "Default";
        private ProductServiceProxy _productService;
        
        public static ShoppingCartService Current => _instance ??= new ShoppingCartService();

        public ObservableCollection<Item?> CartItems => _carts.ContainsKey(_currentCart) ? 
            _carts[_currentCart] : (_carts[_currentCart] = new ObservableCollection<Item?>());
        
        public List<string> CartNames => _carts.Keys.ToList();
        
        public string CurrentCart 
        { 
            get => _currentCart; 
            set 
            {
                if (_currentCart != value && !string.IsNullOrEmpty(value))
                {
                    _currentCart = value;
                    
                    // Ensure the cart exists
                    if (!_carts.ContainsKey(_currentCart))
                    {
                        _carts[_currentCart] = new ObservableCollection<Item?>();
                    }
                    
                    // Notify of change
                    CartChanged?.Invoke(this, EventArgs.Empty);
                }
            } 
        }
        
        // Add event to notify when cart changes
        public event EventHandler? CartChanged;

        private ShoppingCartService()
        {
            _productService = ProductServiceProxy.Current;
            _carts["Default"] = new ObservableCollection<Item?>();
        }
        
        // Create a new cart
        // public bool CreateCart(string cartName)
        // {
        //     if (string.IsNullOrEmpty(cartName) || _carts.ContainsKey(cartName))
        //         return false;
        //         
        //     _carts[cartName] = new ObservableCollection<Item?>();
        //     return true;
        // }
        
        // Delete a cart (except Default)
        // public bool DeleteCart(string cartName)
        // {
        //     if (cartName == "Default" || !_carts.ContainsKey(cartName))
        //         return false;
        //         
        //     // Return all items to inventory
        //     var cartItems = _carts[cartName].ToList();
        //     foreach (var item in cartItems)
        //     {
        //         if (item != null)
        //         {
        //             ReturnToInventory(item);
        //         }
        //     }
        //     
        //     _carts.Remove(cartName);
        //     
        //     // If current cart was deleted, switch to Default
        //     if (_currentCart == cartName)
        //     {
        //         _currentCart = "Default";
        //         CartChanged?.Invoke(this, EventArgs.Empty);
        //     }
        //     
        //     return true;
        // }
        
        // Helper method to return item to inventory
        // private void ReturnToInventory(Item cartItem)
        // {
        //     var inventoryItem = _productService.Products.FirstOrDefault(i => 
        //         i?.Product?.Id == cartItem.Product?.Id);
        //         
        //     if (inventoryItem != null)
        //     {
        //         inventoryItem.Quantity += cartItem.Quantity;
        //     }
        // }

        // Add to cart (transfers item from inventory to cart)
        // public bool AddToCart(Item? inventoryItem)
        // {
        //     if (inventoryItem == null || inventoryItem.Quantity <= 0)
        //         return false;
        //
        //     // Check if we already have this item in cart
        //     var existingCartItem = CartItems.FirstOrDefault(i => 
        //         i?.Product?.Id == inventoryItem.Product?.Id);
        //
        //     // Create new cart item or update existing
        //     if (existingCartItem != null)
        //     {
        //         // Increment quantity in cart
        //         existingCartItem.Quantity += 1;
        //     }
        //     else
        //     {
        //         // Add new item to cart with quantity 1
        //         var newCartItem = new Item
        //         {
        //             Product = inventoryItem.Product,
        //             Quantity = 1
        //         };
        //         CartItems.Add(newCartItem);
        //     }
        //
        //     // Decrease quantity in inventory
        //     inventoryItem.Quantity -= 1;
        //
        //     // Notify subscribers
        //     OnCartChanged();
        //     return true;
        // }
        
        // Add to cart with specified quantity
        // public bool AddToCart(Item? inventoryItem, int quantity)
        // {
        //     if (inventoryItem == null || inventoryItem.Quantity < quantity || quantity <= 0)
        //         return false;
        //
        //     // Check if we already have this item in cart
        //     var existingCartItem = CartItems.FirstOrDefault(i => 
        //         i?.Product?.Id == inventoryItem.Product?.Id);
        //
        //     // Create new cart item or update existing
        //     if (existingCartItem != null)
        //     {
        //         // Increment quantity in cart
        //         existingCartItem.Quantity += quantity;
        //     }
        //     else
        //     {
        //         // Add new item to cart with specified quantity
        //         var newCartItem = new Item
        //         {
        //             Product = inventoryItem.Product,
        //             Quantity = quantity
        //         };
        //         CartItems.Add(newCartItem);
        //     }
        //
        //     // Decrease quantity in inventory
        //     inventoryItem.Quantity -= quantity;
        //
        //     // Notify subscribers
        //     OnCartChanged();
        //     return true;
        // }

        // Remove from cart (transfers item from cart back to inventory)
        // public bool RemoveFromCart(Item? cartItem)
        // {
        //     if (cartItem == null || cartItem.Quantity <= 0)
        //         return false;
        //
        //     // Find corresponding inventory item
        //     var inventoryItem = _productService.Products.FirstOrDefault(i => 
        //         i?.Product?.Id == cartItem.Product?.Id);
        //     
        //     if (inventoryItem != null)
        //     {
        //         // Increase inventory quantity
        //         inventoryItem.Quantity += cartItem.Quantity;
        //         
        //         // Remove from cart
        //         CartItems.Remove(cartItem);
        //         
        //         // Notify subscribers
        //         OnCartChanged();
        //         return true;
        //     }
        //     
        //     return false;
        // }
        
        // Update quantity in cart
        // public bool UpdateCartItemQuantity(Item? cartItem, int newQuantity)
        // {
        //     if (cartItem == null)
        //         return false;
        //         
        //     // Find corresponding inventory item
        //     var inventoryItem = _productService.Products.FirstOrDefault(i => 
        //         i?.Product?.Id == cartItem.Product?.Id);
        //     
        //     if (inventoryItem == null)
        //         return false;
        //         
        //     int currentQuantity = cartItem.Quantity;
        //     int quantityDifference = newQuantity - currentQuantity;
        //     
        //     // Check if we have enough inventory
        //     if (quantityDifference > 0 && inventoryItem.Quantity < quantityDifference)
        //         return false;
        //         
        //     // Update quantities
        //     cartItem.Quantity = newQuantity;
        //     inventoryItem.Quantity -= quantityDifference;
        //     
        //     // If quantity is 0, remove from cart
        //     if (cartItem.Quantity <= 0)
        //         CartItems.Remove(cartItem);
        //         
        //     // Notify subscribers
        //     OnCartChanged();
        //     return true;
        // }
        
        
        
        
        // Notify that cart has changed
        // private void OnCartChanged()
        // {
        //     CartChanged?.Invoke(this, EventArgs.Empty);
        // }
    }
}