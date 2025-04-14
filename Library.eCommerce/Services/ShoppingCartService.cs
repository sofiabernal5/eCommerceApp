using Library.eCommerce.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Library.eCommerce.Services
{
    public class ShoppingCartService
    {
        private static ShoppingCartService? _instance;
        private ObservableCollection<Item?> _items = new ObservableCollection<Item?>();
        private ProductServiceProxy _productService;
        
        public static ShoppingCartService Current => _instance ??= new ShoppingCartService();

        public ObservableCollection<Item?> CartItems => _items;
        
        // Add event to notify when cart changes
        public event EventHandler? CartChanged;

        private ShoppingCartService()
        {
            _productService = ProductServiceProxy.Current;
        }

        // Add to cart (transfers item from inventory to cart)
        // In ShoppingCartService.cs - AddToCart method
        public bool AddToCart(Item? inventoryItem)
        {
            if (inventoryItem == null || inventoryItem.Quantity <= 0)
                return false;

            // Check if we already have this item in cart
            var existingCartItem = _items.FirstOrDefault(i => 
                i?.Product?.Id == inventoryItem.Product?.Id);
    
            // Create new cart item or update existing
            if (existingCartItem != null)
            {
                // Increment quantity in cart
                existingCartItem.Quantity += 1;
            }
            else
            {
                // Add new item to cart with quantity 1
                var newCartItem = new Item
                {
                    Product = inventoryItem.Product,
                    Quantity = 1
                };
                _items.Add(newCartItem);
            }
    
            // Decrease quantity in inventory
            inventoryItem.Quantity -= 1;
    
            // Notify subscribers
            OnCartChanged();
            return true;
        }

        // Remove from cart (transfers item from cart back to inventory)
        public bool RemoveFromCart(Item? cartItem)
        {
            if (cartItem == null || cartItem.Quantity <= 0)
                return false;

            // Find corresponding inventory item
            var inventoryItem = _productService.Products.FirstOrDefault(i => 
                i?.Product?.Id == cartItem.Product?.Id);
            
            if (inventoryItem != null)
            {
                // Increase inventory quantity
                inventoryItem.Quantity += cartItem.Quantity;
                
                // Remove from cart
                _items.Remove(cartItem);
                
                // Notify subscribers
                OnCartChanged();
                return true;
            }
            
            return false;
        }
        
        // Update quantity in cart
        public bool UpdateCartItemQuantity(Item? cartItem, int newQuantity)
        {
            if (cartItem == null)
                return false;
                
            // Find corresponding inventory item
            var inventoryItem = _productService.Products.FirstOrDefault(i => 
                i?.Product?.Id == cartItem.Product?.Id);
            
            if (inventoryItem == null)
                return false;
                
            int currentQuantity = cartItem.Quantity;
            int quantityDifference = newQuantity - currentQuantity;
            
            // Check if we have enough inventory
            if (quantityDifference > 0 && inventoryItem.Quantity < quantityDifference)
                return false;
                
            // Update quantities
            cartItem.Quantity = newQuantity;
            inventoryItem.Quantity -= quantityDifference;
            
            // If quantity is 0, remove from cart
            if (cartItem.Quantity <= 0)
                _items.Remove(cartItem);
                
            // Notify subscribers
            OnCartChanged();
            return true;
        }

        // Clear the cart (return all items to inventory)
        public void ClearCart()
        {
            foreach (var cartItem in _items.ToList())
            {
                if (cartItem != null)
                {
                    // Find corresponding inventory item
                    var inventoryItem = _productService.Products.FirstOrDefault(i => 
                        i?.Product?.Id == cartItem.Product?.Id);
                    
                    if (inventoryItem != null)
                    {
                        // Return quantity to inventory
                        inventoryItem.Quantity += cartItem.Quantity;
                    }
                }
            }
            
            _items.Clear();
            OnCartChanged();
        }
        
        // Generate receipt and clear cart
        public string Checkout()
        {
            if (_items.Count == 0)
                return "Your cart is empty.";

            double subtotal = 0;
            string receipt = "ITEMIZED RECEIPT\n==================\n\n";

            foreach (var item in _items)
            {
                if (item != null)
                {
                    double itemTotal = (item.Product?.Price ?? 0) * item.Quantity;
                    receipt += $"{item.Product?.Name} × {item.Quantity} @ ${item.Product?.Price:F2} = ${itemTotal:F2}\n";
                    subtotal += itemTotal;
                }
            }

            double tax = subtotal * 0.07;
            double total = subtotal + tax;

            receipt += "\n==================\n";
            receipt += $"Subtotal: ${subtotal:F2}\n";
            receipt += $"Tax (7%): ${tax:F2}\n";
            receipt += $"TOTAL: ${total:F2}\n";
            receipt += "==================\n";
            receipt += "Thank you for your purchase!";

            // Items are already removed from inventory, so just clear the cart
            _items.Clear();
            OnCartChanged();

            return receipt;
        }
        
        // Notify that cart has changed
        private void OnCartChanged()
        {
            CartChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}