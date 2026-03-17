namespace ConsoleApp5
{
    using System;

    class Student
    {
        // Method to calculate average
        public double CalculateAverage(int m1, int m2, int m3)
        {
            double avg = (m1 + m2 + m3) / 3.0;
            return avg;
        }

        // Method to determine grade
        public string GetGrade(double average)
        {
            if (average >= 90)
                return "A+";
            else if (average >= 80)
                return "A";
            else if (average >= 70)
                return "B";
            else if (average >= 60)
                return "C";
            else
                return "Fail";
        }
    }

    class Program
    {
        static void Main()
        {
            int m1, m2, m3;

            Console.Write("Enter Marks 1: ");
            m1 = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter Marks 2: ");
            m2 = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter Marks 3: ");
            m3 = Convert.ToInt32(Console.ReadLine());

            // Create object
            Student s = new Student();

            // Call method to calculate average
            double average = s.CalculateAverage(m1, m2, m3);

            // Call method to get grade
            string grade = s.GetGrade(average);

            Console.WriteLine("Average = " + average);
            Console.WriteLine("Grade = " + grade);

            Console.ReadLine();
        }
    }
}
