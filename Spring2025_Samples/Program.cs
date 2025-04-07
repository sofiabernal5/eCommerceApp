//// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using Library.eCommerce.Services;
using Spring2025_Samples.Models;
using System;
using System.Xml.Serialization;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to Amazon!");

            // Console.WriteLine("C. Create new inventory item");
            // Console.WriteLine("R. Read all inventory items");
            // Console.WriteLine("U. Update an inventory item");
            // Console.WriteLine("D. Delete an inventory item");
            // Console.WriteLine("Q. Quit");
            //
            // List<Product?> list = ProductServiceProxy.Current.Products;
            //
            // char choice;
            // do
            // {
            //     string? input = Console.ReadLine();
            //     choice = input[0];
            //     switch (choice)
            //     {
            //         case 'C':
            //         case 'c':
            //             ProductServiceProxy.Current.AddOrUpdate(new Product
            //             {
            //                 Name = Console.ReadLine()
            //             });
            //             break;
            //         case 'R':
            //         case 'r':
            //
            //             list.ForEach(Console.WriteLine);
            //             break;
            //         case 'U':
            //         case 'u':
            //             //select one of the products
            //             Console.WriteLine("Which product would you like to update?");
            //             int selection = int.Parse(Console.ReadLine() ?? "-1");
            //             var selectedProd = list.FirstOrDefault(p => p.Id == selection);
            //
            //             if(selectedProd != null)
            //             {
            //                 selectedProd.Name = Console.ReadLine() ?? "ERROR";
            //                 ProductServiceProxy.Current.AddOrUpdate(selectedProd);
            //             }
            //             break;
            //         case 'D':
            //         case 'd':
            //             //select one of the products
            //             //throw it away
            //             Console.WriteLine("Which product would you like to update?");
            //             selection = int.Parse(Console.ReadLine() ?? "-1");
            //             ProductServiceProxy.Current.Delete(selection);
            //             break;
            //         case 'Q':
            //         case 'q':
            //             break;
            //         default:
            //             Console.WriteLine("Error: Unknown Command");
            //             break;
            //     }
            // } while (choice != 'Q' && choice != 'q');
            //
            // Console.ReadLine();
        }
    }


}