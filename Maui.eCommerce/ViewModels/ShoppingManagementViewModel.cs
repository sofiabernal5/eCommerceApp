using Library.eCommerce.Models;
using Library.eCommerce.Services;
using System;
using System.Collections.Generic;
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
        private int _inventorySortOption;
        private int _cartSortOption;

        // Wishlist support
        private Dictionary<string, ObservableCollection<Item?>> _wishlists;
        private string _currentWishlist;

        // Configuration
        private double _taxRate = 0.07; // Default 7%

        public event PropertyChangedEventHandler? PropertyChanged;

        public ShoppingManagementViewModel()
        {
            _invSvc = ProductServiceProxy.Current;
            _cartSvc = ShoppingCartService.Current;
            _inventory = new ObservableCollection<Item?>(_invSvc.Products);
            _checkoutMessage = string.Empty;

            // Initialize wishlists with a default cart
            _wishlists = new Dictionary<string, ObservableCollection<Item?>>();
            _currentWishlist = "Default Cart";
            _wishlists[_currentWishlist] = new ObservableCollection<Item?>();

            // Add the "My Wishlist" cart as a second default option
            if (!_wishlists.ContainsKey("My Wishlist"))
            {
                _wishlists["My Wishlist"] = new ObservableCollection<Item?>();
            }

            // Subscribe to cart changes
            _cartSvc.CartChanged += (s, e) =>
            {
                NotifyPropertyChanged(nameof(ShoppingCart));
                NotifyPropertyChanged(nameof(HasCartItems));
            };

            // Load tax rate from settings
            LoadSettings();
        }

        // Load settings from preferences
        private void LoadSettings()
        {
            if (Preferences.ContainsKey("TaxRate"))
            {
                _taxRate = Preferences.Get("TaxRate", 0.07);
            }
        }

        // Save settings to preferences
        public void SaveSettings()
        {
            Preferences.Set("TaxRate", _taxRate);
        }

        public ObservableCollection<Item?> Inventory
        {
            get
            {
                var sortedItems = _inventory;

                // Apply sorting
                if (_inventorySortOption == 0) // Sort by name
                {
                    sortedItems = new ObservableCollection<Item?>(_inventory.OrderBy(i => i?.Product?.Name));
                }
                else if (_inventorySortOption == 1) // Sort by price
                {
                    sortedItems = new ObservableCollection<Item?>(_inventory.OrderBy(i => i?.Product?.Price));
                }

                return sortedItems;
            }
        }

        public ObservableCollection<Item?> ShoppingCart
        {
            get
            {
                var cartItems = _wishlists[_currentWishlist];
                
                // Apply sorting
                if (_cartSortOption == 0) // Sort by name
                {
                    return new ObservableCollection<Item?>(cartItems.OrderBy(i => i?.Product?.Name));
                }
                else if (_cartSortOption == 1) // Sort by price
                {
                    return new ObservableCollection<Item?>(cartItems.OrderBy(i => i?.Product?.Price));
                }
                
                return cartItems;
            }
        }

        public List<string> WishlistNames => _wishlists.Keys.ToList();

        public string CurrentWishlist
        {
            get { return _currentWishlist; }
            set
            {
                if (_currentWishlist != value && !string.IsNullOrEmpty(value))
                {
                    _currentWishlist = value;
                    NotifyPropertyChanged(nameof(CurrentWishlist));
                    NotifyPropertyChanged(nameof(ShoppingCart));
                    NotifyPropertyChanged(nameof(HasCartItems));
                    NotifyPropertyChanged(nameof(CanDeleteWishlist));
                }
            }
        }

        public bool CanDeleteWishlist => _wishlists.Count > 1 && _currentWishlist != "Default Cart";

        public int InventorySortOption
        {
            get { return _inventorySortOption; }
            set
            {
                if (_inventorySortOption != value)
                {
                    _inventorySortOption = value;
                    NotifyPropertyChanged(nameof(InventorySortOption));
                    NotifyPropertyChanged(nameof(Inventory));
                }
            }
        }

        public int CartSortOption
        {
            get { return _cartSortOption; }
            set
            {
                if (_cartSortOption != value)
                {
                    _cartSortOption = value;
                    NotifyPropertyChanged(nameof(CartSortOption));
                    NotifyPropertyChanged(nameof(ShoppingCart));
                }
            }
        }

        public double TaxRate
        {
            get { return _taxRate; }
            set
            {
                if (_taxRate != value && value >= 0)
                {
                    _taxRate = value;
                    NotifyPropertyChanged(nameof(TaxRate));
                    SaveSettings();
                }
            }
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

        public bool HasCartItems => _wishlists[_currentWishlist].Count > 0;

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

        // Add to cart (transfers item from inventory to cart)
        public void AddToCart()
        {
            if (CanAddToCart)
            {
                AddItemToCurrentCart(SelectedInventoryItem, 1);
                NotifyPropertyChanged(nameof(Inventory));
                NotifyPropertyChanged(nameof(CanAddToCart));
            }
        }
        
        // Quick add to cart with specified quantity (called from the inline entry/button control)
        public void QuickAddToCart(Item? item, int quantity)
        {
            if (item != null && item.Quantity >= quantity && quantity > 0)
            {
                AddItemToCurrentCart(item, quantity);
                NotifyPropertyChanged(nameof(Inventory));
            }
        }

        // Helper method to add item to current cart
        private void AddItemToCurrentCart(Item? inventoryItem, int quantity)
        {
            if (inventoryItem == null || inventoryItem.Quantity < quantity)
                return;

            // Check if we already have this item in cart
            var existingCartItem = _wishlists[_currentWishlist].FirstOrDefault(i =>
                i?.Product?.Id == inventoryItem.Product?.Id);

            // Create new cart item or update existing
            if (existingCartItem != null)
            {
                // Increment quantity in cart
                existingCartItem.Quantity += quantity;
            }
            else
            {
                // Add new item to cart with the specified quantity
                var newCartItem = new Item
                {
                    Product = inventoryItem.Product,
                    Quantity = quantity
                };
                _wishlists[_currentWishlist].Add(newCartItem);
            }

            // Decrease quantity in inventory
            inventoryItem.Quantity -= quantity;

            // Notify subscribers
            NotifyPropertyChanged(nameof(ShoppingCart));
            NotifyPropertyChanged(nameof(HasCartItems));
        }

        // Remove selected cart item and return to inventory
        public void RemoveFromCart()
        {
            if (HasSelectedCartItem)
            {
                // Find corresponding inventory item
                var inventoryItem = _invSvc.Products.FirstOrDefault(i =>
                    i?.Product?.Id == SelectedCartItem.Product?.Id);

                if (inventoryItem != null)
                {
                    // Increase inventory quantity
                    inventoryItem.Quantity += SelectedCartItem.Quantity;

                    // Remove from cart
                    _wishlists[_currentWishlist].Remove(SelectedCartItem);
                    SelectedCartItem = null;

                    // Notify subscribers
                    NotifyPropertyChanged(nameof(ShoppingCart));
                    NotifyPropertyChanged(nameof(HasCartItems));
                    NotifyPropertyChanged(nameof(Inventory));
                }
            }
        }

        // Update cart item quantity
        public void UpdateCartItemQuantity(int newQuantity)
        {
            if (HasSelectedCartItem)
            {
                // Find corresponding inventory item
                var inventoryItem = _invSvc.Products.FirstOrDefault(i =>
                    i?.Product?.Id == SelectedCartItem.Product?.Id);

                if (inventoryItem == null)
                    return;

                int currentQuantity = SelectedCartItem.Quantity;
                int quantityDifference = newQuantity - currentQuantity;

                // Check if we have enough inventory
                if (quantityDifference > 0 && inventoryItem.Quantity < quantityDifference)
                    return;

                // Update quantities
                SelectedCartItem.Quantity = newQuantity;
                inventoryItem.Quantity -= quantityDifference;

                // If quantity is 0, remove from cart
                if (SelectedCartItem.Quantity <= 0)
                {
                    _wishlists[_currentWishlist].Remove(SelectedCartItem);
                    SelectedCartItem = null;
                }

                // Notify subscribers
                NotifyPropertyChanged(nameof(ShoppingCart));
                NotifyPropertyChanged(nameof(HasCartItems));
                NotifyPropertyChanged(nameof(Inventory));
            }
        }

        // Process checkout for the current cart
        public string Checkout()
        {
            if (HasCartItems)
            {
                double subtotal = 0;
                string receipt = $"CART: {_currentWishlist}\n";
                receipt += "ITEMIZED RECEIPT\n==================\n\n";

                foreach (var item in _wishlists[_currentWishlist])
                {
                    if (item != null)
                    {
                        double itemTotal = (item.Product?.Price ?? 0) * item.Quantity;
                        receipt +=
                            $"{item.Product?.Name} × {item.Quantity} @ ${item.Product?.Price:F2} = ${itemTotal:F2}\n";
                        subtotal += itemTotal;
                    }
                }

                double tax = subtotal * _taxRate;
                double total = subtotal + tax;

                receipt += "\n==================\n";
                receipt += $"Subtotal: ${subtotal:F2}\n";
                receipt += $"Tax ({_taxRate:P0}): ${tax:F2}\n";
                receipt += $"TOTAL: ${total:F2}\n";
                receipt += "==================\n";
                receipt += "Thank you for your purchase!";

                // Clear the cart
                ClearCurrentCart();

                return receipt;
            }

            return "Your cart is empty.";
        }

        // Clear the current cart, return all items to inventory
        private void ClearCurrentCart()
        {
            foreach (var cartItem in _wishlists[_currentWishlist].ToList())
            {
                if (cartItem != null)
                {
                    // Find corresponding inventory item
                    var inventoryItem = _invSvc.Products.FirstOrDefault(i =>
                        i?.Product?.Id == cartItem.Product?.Id);

                    if (inventoryItem != null)
                    {
                        // Return quantity to inventory
                        inventoryItem.Quantity += cartItem.Quantity;
                    }
                }
            }

            _wishlists[_currentWishlist].Clear();
            NotifyPropertyChanged(nameof(ShoppingCart));
            NotifyPropertyChanged(nameof(HasCartItems));
            NotifyPropertyChanged(nameof(Inventory));
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

        // Create a new wishlist cart
        public void CreateWishlist(string name)
        {
            if (!string.IsNullOrEmpty(name) && !_wishlists.ContainsKey(name))
            {
                _wishlists[name] = new ObservableCollection<Item?>();
                CurrentWishlist = name;
                NotifyPropertyChanged(nameof(WishlistNames));
            }
        }

        // Delete current wishlist
        public void DeleteCurrentWishlist()
        {
            if (_wishlists.Count > 1 && _currentWishlist != "Default Cart")
            {
                // Return all items in this cart to inventory
                ClearCurrentCart();

                // Remove the wishlist
                _wishlists.Remove(_currentWishlist);

                // Switch to the default cart
                CurrentWishlist = "Default Cart";

                NotifyPropertyChanged(nameof(WishlistNames));
                NotifyPropertyChanged(nameof(CanDeleteWishlist));
            }
        }

        // Switch to a different wishlist
        public void SwitchWishlist()
        {
            NotifyPropertyChanged(nameof(ShoppingCart));
            NotifyPropertyChanged(nameof(HasCartItems));
        }

        public void RefreshInventory()
        {
            _inventory = new ObservableCollection<Item?>(_invSvc.Products);
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