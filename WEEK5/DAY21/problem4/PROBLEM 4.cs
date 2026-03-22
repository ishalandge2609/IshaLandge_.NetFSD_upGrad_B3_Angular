using System;

namespace ConsoleApp1
{
    // parent class
    class Vehicle
    {
        public string Brand { get; set; }
        public decimal RentalRatePerDay { get; set; }

        public virtual decimal CalculateRental(int days)
        {
            return RentalRatePerDay * days;
        }
    }

    // child class Car
    class Car : Vehicle
    {
        public override decimal CalculateRental(int days)
        {
            return (RentalRatePerDay * days) + 500;
        }
    }

    // child class Bike
    class Bike : Vehicle
    {
        public override decimal CalculateRental(int days)
        {
            decimal total = RentalRatePerDay * days;
            return total - (total * 0.05m);
        }
    }

    class Program
    {
        static void Main()
        {
            Console.Write("Car RentalRatePerDay = ");
            decimal rate;

            if (!decimal.TryParse(Console.ReadLine(), out rate))
            {
                Console.WriteLine("Invalid rental rate.");
                return;
            }

            Console.Write("Days = ");
            int days;

            if (!int.TryParse(Console.ReadLine(), out days))
            {
                Console.WriteLine("Invalid number of days.");
                return;
            }

            Vehicle car = new Car();
            car.Brand = "Toyota";
            car.RentalRatePerDay = rate;

            Console.WriteLine($"Total Rental = {car.CalculateRental(days)}");
        }
    }
}