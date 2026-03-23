using System;

namespace ConsoleApp1
{
    class Product
    {
        private decimal _price;

        public string Name { get; set; }

        public decimal Price
        {
            get { return _price; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Price cannot be negative.");

                _price = value;
            }
        }

        public virtual decimal CalculateDiscount()
        {
            return Price;
        }
    }

    class Electronics : Product
    {
        public override decimal CalculateDiscount()
        {
            return Price - (Price * 0.05m);
        }
    }

    class Clothing : Product
    {
        public override decimal CalculateDiscount()
        {
            return Price - (Price * 0.15m);
        }
    }

    class Program
    {
        static void Main()
        {
            Console.Write("Electronics Price = ");
            decimal price = Convert.ToDecimal(Console.ReadLine());

            Product item = new Electronics();
            item.Name = "Laptop";
            item.Price = price;

            Console.WriteLine($"Final Price after 5% discount = {item.CalculateDiscount()}");
        }
    }
}