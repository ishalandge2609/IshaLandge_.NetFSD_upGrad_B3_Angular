using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp8
{
    class Program
    {

        static void GenerateSalesReport()
        {
            Console.WriteLine("Sales Report started...");
            Thread.Sleep(3000);
            Console.WriteLine("Sales Report completed.");
        }


        static void GenerateInventoryReport()
        {
            Console.WriteLine("Inventory Report started...");
            Thread.Sleep(2000);
            Console.WriteLine("Inventory Report completed.");
        }


        static void GenerateCustomerReport()
        {
            Console.WriteLine("Customer Report started...");
            Thread.Sleep(2500);
            Console.WriteLine("Customer Report completed.");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Report generation started...\n");


            Task task1 = Task.Run(() => GenerateSalesReport());
            Task task2 = Task.Run(() => GenerateInventoryReport());
            Task task3 = Task.Run(() => GenerateCustomerReport());


            Task.WaitAll(task1, task2, task3);

            Console.WriteLine("\nAll reports generated successfully.");


            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}