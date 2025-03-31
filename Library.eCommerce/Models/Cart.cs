using Library.eCommerce.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Work done by Sofia Bernal

namespace Spring2025_Samples.Models;
public class ShoppingCart
{
    public Dictionary<int, int> Items { get; private set; }  

    public ShoppingCart()
    {
        Items = new Dictionary<int, int>();
    }

    //ADD TO CART FUNCTION
    public bool AddToCart(int productId, int productQuantity)
    {
        
        var product = ProductServiceProxy.Current.Products.FirstOrDefault(p => p.Id == productId); 

        if (product == null)
        {
            Console.WriteLine("Product is not in inventory");
            return false;
        }
        if(productQuantity < 0  || productQuantity > product.Quantity)
        {
             Console.WriteLine("Not enough stock for this product");
             return false;
        }

        if (productQuantity == 0)
        {
             Console.WriteLine("Enter a valid quantity");
             return false;
        }
        if (Items.ContainsKey(productId))
        {
            Items[productId] += productQuantity;
            
        }
        else
        {
            Items[productId] = productQuantity;
        }
       
        product.Quantity -= productQuantity;
        return false;
    }
    
    public string? Display
    {
        get
        {
            return $"ID: {Items.Keys}, Quantity: {Items.Values}";
        }
    }

    //PRINT CART FUNCTION
    public void PrintCart()
    {
        
        if (!Items.Any())
            
        {
            Console.WriteLine("Your cart is currently empty");
            return;
        }
        Console.WriteLine("--------------Shopping Cart Contents------------");
        foreach (var x in Items)
        {
            var product = ProductServiceProxy.Current.Products.FirstOrDefault(p => p.Id == x.Key);
            if (product != null)
            {
                Console.WriteLine($"ID: {x.Key} \t Name: {product.Name} \t Quantity: {x.Value} \t Price for each: ${product.Price} \t ");
            }
          
        }
    }
    
    //FUNCTION TO DELETE FROM SHOPPING CART
    public bool deleteProduct(int productId)
    {
        if (!Items.ContainsKey(productId))
        {
            Console.WriteLine("Product not found in cart.");
            return false;
        }

        var product = ProductServiceProxy.Current.Products.FirstOrDefault(p => p.Id == productId);
        if (product == null)
        {
            Console.WriteLine("Product no longer exists in inventory.");
            return false;
        }

        int removedQuantity = Items[productId];
        product.Quantity += removedQuantity; // Restore stock to inventory

        Items.Remove(productId);
        Console.WriteLine($"{product.Name} removed from cart. {removedQuantity} items returned to inventory.");
        return true;
    }

    //CHECKOUT 
    public void PrintReceipt()
    {
        PrintCart();
        double totalPrice = 0.0;
        double totalTax;
        const double salesTax = 0.07;
        foreach (var x in Items)
        {
            var product = ProductServiceProxy.Current.Products.FirstOrDefault(p => p.Id == x.Key);
            if (product != null)
            {
                totalPrice += product.Price * x.Value;
                
            }
        }
        totalTax = totalPrice * salesTax;
        Console.WriteLine($"SUBTOTAL: ${totalPrice:F2}");
        Console.WriteLine($"TAX: ${totalTax:F2}");
        Console.WriteLine($"TOTAL PRICE (including tax): ${totalTax + totalPrice:F2}");
        Console.WriteLine($"THANK YOU FOR SHOPPING TODAY!");
    }
    public override string ToString()
    {
        return Display ?? string.Empty;
    }
}