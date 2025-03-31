//// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
//Work done by Sofia Bernal
using Library.eCommerce.Services;
using Spring2025_Samples.Models;
using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to Amazon!");

            Console.WriteLine("C. Create a new inventory item");
            Console.WriteLine("R. Read all inventory items");
            Console.WriteLine("U. Update an inventory item");
            Console.WriteLine("D. Delete an inventory item");
            Console.WriteLine("A. Add an item from inventory to cart");
            Console.WriteLine("V. View items in your cart");
            Console.WriteLine("X. Remove an item from cart");
            Console.WriteLine("Q. Checkout and Quit");

            List<Product?> list = ProductServiceProxy.Current.Products;
            ShoppingCart cart = new ShoppingCart();
            char choice;
            
            do
            {
                string? input = Console.ReadLine();
                choice = input[0];
                double price;
                int invQuantity;
                switch (choice)
                {
                    case 'C':
                    case 'c':
                        
                        //asks for product name 
                        Console.Write("Enter product name: ");
                        string name = Console.ReadLine();

                        //asks for product price
                        Console.Write("Enter product price: ");
                        while (!double.TryParse(Console.ReadLine(), out price))
                        {
                            Console.Write("Invalid input. Please enter a valid price: ");
                        }
                        
                        //asks for product quantity
                        Console.Write("Enter product quantity: ");
                        while (!int.TryParse(Console.ReadLine(), out invQuantity))
                        {
                            Console.Write("Invalid input. Please enter a valid quantity: ");
                        }
                        //calls function to add into inventory
                        ProductServiceProxy.Current.AddOrUpdate(new Product
                        {
                            Name = name,
                            Price = price,
                            Quantity = invQuantity
                        });
                        
                        break;
                    case 'R':
                    case 'r':
                        list.ForEach(Console.WriteLine);
                        break;
                    case 'U':
                    case 'u':
                        //select one of the products
                        Console.WriteLine("Which product would you like to update?");
                        int selection = int.Parse(Console.ReadLine() ?? "-1");
                        var selectedProd = list.FirstOrDefault(p => p.Id == selection);

                        if(selectedProd != null)
                        {
                            selectedProd.Name = Console.ReadLine() ?? "ERROR";
                            ProductServiceProxy.Current.AddOrUpdate(selectedProd);
                        }
                        break;
                    case 'D':
                    case 'd':
                        //select one of the products
                        //throw it away
                        Console.WriteLine("Which product would you like to delete?");
                        selection = int.Parse(Console.ReadLine() ?? "-1");
                        ProductServiceProxy.Current.Delete(selection);
                        break;
                    case 'A':
                    case 'a':
                        Console.WriteLine("Which Product would you like to Add to cart (ID number)");
                        int productToAdd = int.Parse(Console.ReadLine() ?? "-1");
                        var selectedProduct = list.FirstOrDefault(p => p.Id == productToAdd);

                        if (selectedProduct == null)
                        {
                            Console.WriteLine("Product is not found in inventory");
                        }
                        
                        if(selectedProduct != null)
                        {
                            Console.WriteLine("Enter the Quantity");
                            int quantity = int.Parse(Console.ReadLine());
                            Console.WriteLine($"Index of product: {productToAdd}, Quantity: {quantity}");
                            cart.AddToCart(productToAdd, quantity);
                        }
                        break;
                    case 'V':
                    case 'v':
                        Console.WriteLine("Shopping cart: ");
                        cart.PrintCart();
                        break;
                    case 'X':
                    case 'x':
                        Console.WriteLine("Which product would you like to remove from shopping cart?");
                        selection = int.Parse(Console.ReadLine() ?? "-1");
                        cart.deleteProduct(selection);
                        break;
                    case 'Q':
                    case 'q':
                        break;
                    default:
                        Console.WriteLine("Error: Unknown Command");
                        break;
                }
            } while (choice != 'Q' && choice != 'q');

            cart.PrintReceipt();
            Console.ReadLine();
        }
    }


}
