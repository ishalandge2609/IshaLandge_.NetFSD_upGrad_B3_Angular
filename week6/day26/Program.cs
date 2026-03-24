using ConsoleApp3;
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        ProductDAL dal = new ProductDAL();

        while (true)
        {
            Console.WriteLine("\n===== PRODUCT MANAGEMENT =====");
            Console.WriteLine("1. Insert");
            Console.WriteLine("2. View All");
            Console.WriteLine("3. View By Id");
            Console.WriteLine("4. Update");
            Console.WriteLine("5. Delete");
            Console.WriteLine("6. Exit");

            Console.Write("Enter choice: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input! Enter number.");
                continue;
            }

            switch (choice)
            {
                // INSERT
                case 1:
                    Products p = new Products();

                    Console.Write("Name: ");
                    p.ProductName = Console.ReadLine();

                    Console.Write("Category: ");
                    p.Category = Console.ReadLine();

                    Console.Write("Price: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal price))
                    {
                        Console.WriteLine("Invalid price!");
                        break;
                    }
                    p.Price = price;

                    dal.InsertProduct(p);
                    Console.WriteLine("Product Inserted!");
                    break;

                // VIEW ALL
                case 2:
                    List<Products> list = dal.GetAllProducts();

                    if (list.Count == 0)
                    {
                        Console.WriteLine("No products found.");
                    }
                    else
                    {
                        foreach (var item in list)
                        {
                            Console.WriteLine($"{item.ProductId} {item.ProductName} {item.Category} {item.Price}");
                        }
                    }
                    break;

                // VIEW BY ID
                case 3:
                    Console.Write("Enter Product Id: ");
                    if (!int.TryParse(Console.ReadLine(), out int pid))
                    {
                        Console.WriteLine("Invalid ID!");
                        break;
                    }

                    Products product = dal.GetProductById(pid);

                    if (product != null)
                    {
                        Console.WriteLine($"{product.ProductId} {product.ProductName} {product.Category} {product.Price}");
                    }
                    else
                    {
                        Console.WriteLine("Product not found!");
                    }
                    break;

                // UPDATE
                case 4:
                    Products up = new Products();

                    Console.Write("ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int upId))
                    {
                        Console.WriteLine("Invalid ID!");
                        break;
                    }
                    up.ProductId = upId;

                    Console.Write("Name: ");
                    up.ProductName = Console.ReadLine();

                    Console.Write("Category: ");
                    up.Category = Console.ReadLine();

                    Console.Write("Price: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal upPrice))
                    {
                        Console.WriteLine("Invalid price!");
                        break;
                    }
                    up.Price = upPrice;

                    dal.UpdateProduct(up);
                    Console.WriteLine("Product Updated!");
                    break;

                // DELETE
                case 5:
                    Console.Write("Enter ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int delId))
                    {
                        Console.WriteLine("Invalid ID!");
                        break;
                    }

                    dal.DeleteProduct(delId);
                    Console.WriteLine("Product Deleted!");
                    break;

                // EXIT
                case 6:
                    Console.WriteLine("Exiting...");
                    return;

                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }
        }
    }
}