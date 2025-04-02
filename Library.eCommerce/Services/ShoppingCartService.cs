using Library.eCommerce.Models;

namespace Library.eCommerce.Services
{
    public class ShoppingCartService
    {
        private ProductServiceProxy _prodSvc;
        private List<Item> items;
        public List<Item> CartItems
        {
            get
            {
                return items;
            }
        }
        public static ShoppingCartService Current {  
            get
            {
                if(instance == null)
                {
                    instance = new ShoppingCartService();
                }

                return instance;
            } 
        }
        private static ShoppingCartService? instance;
        private ShoppingCartService() { 
            items = new List<Item>();
        }
        
        public void ClearCart()
        {
            items.Clear(); // Clears all items in the cart
        }

        public void Checkout()
        {
            if (items.Any())
            {
                // For now, just print a message or perform any necessary action.
                Console.WriteLine("Checkout successful. Cart is now empty.");
        
                // Clear the cart after checkout.
                items.Clear();
            }
            else
            {
                Console.WriteLine("Cart is empty. Nothing to checkout.");
            }
        }
        public void AddItem(Item item) 
        {
            var existingItem = items.FirstOrDefault(i => i.Id == item.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;  // Update quantity if item already exists
            }
            else
            {
                items.Add(item);  // Add new item if it doesn't exist
            }
        }
        
        public void RemoveItem(int id)
        {
            var itemToRemove = items.FirstOrDefault(i => i.Id == id);
            if (itemToRemove != null)
            {
                items.Remove(itemToRemove);
                Console.WriteLine($"Item with ID {id} removed from cart.");
            }
            else
            {
                Console.WriteLine($"Item with ID {id} not found in cart.");
            }
        }


    }
}