using Library.eCommerce.Models;
using Spring2025_Samples.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Work done by Sofia Bernal
namespace Library.eCommerce.Services
{
    public class ProductServiceProxy
    {
        private ProductServiceProxy()
        {
            Products = new List<Product?>();
            
        }


        private int LastKey
        {
            get
            {
                if (!Products.Any())
                {
                    return 0;
                }

                return Products.Select(p => p?.Id ?? 0).Max();
            }
        }

        private static ProductServiceProxy? instance;
        private static object instanceLock = new object();

        public static ProductServiceProxy Current
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductServiceProxy();
                    }
                }

                return instance;
            }
        }

        public List<Product?> Products { get; private set; }

        public Product AddOrUpdate(Product product)
        {
            //check if product is already in inventory
            if (product.Id == 0)
            {
                product.Id = LastKey + 1;
                Products.Add(product);
            }
            else
            {
                //Find the product in the inventory
                Product? existingProduct = null;
                foreach (var p in Products)
                {
                    if (p.Id == product.Id)
                    {
                        existingProduct = p;
                        break;
                    }
                }

                if (existingProduct != null)
                {
                    existingProduct.Quantity += product.Quantity;
                    existingProduct.Price = product.Price;
                    existingProduct.Name = product.Name;

                }
                else
                {
                    Products.Add(product);

                }

            }

            Console.WriteLine($"Inventory for {product.Name} is {product.Quantity}");
            return product;
        }

        // public Item AddOrUpdate(Item item)
        // {
        //     if(item.Id == 0)
        //     {
        //         item.Id = LastKey + 1;
        //         item.Product.Id = item.Id;
        //         Products.Add(item);
        //     }
        //
        //
        //     return item;
        // }

        public Product? Delete(int id)
        {
            if (id == 0)
            {
                return null;
            }

            Product? product = Products.FirstOrDefault(p => p.Id == id);
            Products.Remove(product);

            return product;
        }
        public Item? GetById(int id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                return new Item
                {
                    Id = product.Id,
                    Product = product,
                    Quantity = 1, // Default quantity, can be adjusted if necessary
                    Name = product.Name // Assuming Name is available in Product
                };
            }
            return null;
        }


        
    }
}

