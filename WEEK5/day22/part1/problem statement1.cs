
using System;

namespace ConsoleApp1
{
    public record Student(int RollNumber, string Name, string Course, double Marks);

     internal class Class2
    {
        static void Main(string[] args)
        {
            Console.Write("Enter number of students: ");
            int n;
            while (!int.TryParse(Console.ReadLine(), out n) || n <= 0)
            {
                Console.Write("Invalid input. Enter number of students: ");
            }

            Student[] students = new Student[n];

            for (int i = 0; i < students.Length; i++)
            {
                Console.WriteLine($"\nEntering details for student {i + 1}");

                Console.Write("Enter Roll Number: ");
                int roll;
                while (!int.TryParse(Console.ReadLine(), out roll))
                {
                    Console.Write("Invalid input. Enter Roll Number: ");
                }

                Console.Write("Enter Name: ");
                string name = Console.ReadLine();

                Console.Write("Enter Course: ");
                string course = Console.ReadLine();

                Console.Write("Enter Marks: ");
                double marks;
                while (!double.TryParse(Console.ReadLine(), out marks))
                {
                    Console.Write("Invalid input. Enter Marks: ");
                }

                students[i] = new Student(roll, name, course, marks);
            }

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n1. Display All Records");
                Console.WriteLine("2. Search by Roll Number");
                Console.WriteLine("3. Exit");
                Console.Write("Enter Choice: ");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.WriteLine("\nStudent Records:");
                    foreach (var s in students)
                    {
                        Console.WriteLine($"Roll No: {s.RollNumber} | Name: {s.Name} | Course: {s.Course} | Marks: {s.Marks}");
                    }
                }
                else if (choice == "2")
                {
                    Console.Write("Search Roll Number: ");
                    int searchRoll;
                    if (int.TryParse(Console.ReadLine(), out searchRoll))
                    {
                        bool found = false;
                        for (int i = 0; i < students.Length; i++)
                        {
                            if (students[i].RollNumber == searchRoll)
                            {
                                Console.WriteLine("\nSearch Result:");
                                Console.WriteLine("Student Found:");
                                Console.WriteLine($"Roll No: {students[i].RollNumber} | Name: {students[i].Name} | Course: {students[i].Course} | Marks: {students[i].Marks}");
                                found = true;
                                break;
                            }
                        }
                        if (!found) Console.WriteLine("\nSearch Result: Student record not found.");
                    }
                }
                else if (choice == "3")
                {
                    exit = true;
                }
            }
            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }
    }
}