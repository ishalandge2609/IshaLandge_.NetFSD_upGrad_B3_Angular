using System;

namespace ConsoleApp1
{
    //employee class
    class Employee
    {
        public string Name { get; set; }
        public decimal BaseSalary { get; set; }

        public Employee(string name, decimal baseSalary)
        {
            Name = name;
            BaseSalary = baseSalary;
        }

        public virtual decimal CalculateSalary()
        {
            return BaseSalary;
        }
    }
    // inherit class - manager
    class Manager : Employee
    {
        public Manager(string name, decimal baseSalary) : base(name, baseSalary)
        {
        }
        public override decimal CalculateSalary()
        {
            return BaseSalary + (BaseSalary * 0.20m);
        }
    }
    //inherit class - developer
    class Developer : Employee
    {
        public Developer(string name, decimal baseSalary) : base(name, baseSalary)
        {
        }
        public override decimal CalculateSalary()
        {
            return BaseSalary + (BaseSalary * 0.10m);
        }
    }

    class Program
    {
        static void Main()
        {

            Console.Write("BaseSalary = ");
            decimal baseSalary = Convert.ToDecimal(Console.ReadLine());

            Employee manager = new Manager("Manager", baseSalary);
            Employee developer = new Developer("Developer", baseSalary);

            Console.WriteLine($"Manager Salary = {manager.CalculateSalary()}, Developer Salary = {developer.CalculateSalary()}");
        }
    }
}