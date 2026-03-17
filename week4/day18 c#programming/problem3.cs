namespace problemstatement3
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            string name;
            double salary, bonus, finalSalary;
            int experience;
            double bonusRate;

            Console.Write("Enter Name: ");
            name = Console.ReadLine();

            Console.Write("Enter Salary: ");
            salary = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter Experience (years): ");
            experience = Convert.ToInt32(Console.ReadLine());

            // Determine bonus percentage
            if (experience < 2)
            {
                bonusRate = 0.05;
            }
            else if (experience <= 5)
            {
                bonusRate = 0.10;
            }
            else
            {
                bonusRate = 0.15;
            }

            // Calculate bonus using ternary operator
            bonus = salary > 0 ? salary * bonusRate : 0;

            finalSalary = salary + bonus;

            Console.WriteLine("\nEmployee: " + name);
            Console.WriteLine($"Bonus: {bonus:C}");
            Console.WriteLine($"Final Salary: {finalSalary:C}");

            Console.ReadLine();
        }
    }
}
