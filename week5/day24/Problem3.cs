using System.Text;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {

            try
            {
                // 1. Take input
                Console.Write("Enter Employee Name: ");
                string name = Console.ReadLine();

                Console.Write("Enter Sales Amount: ");
                double sales = Convert.ToDouble(Console.ReadLine());

                Console.Write("Enter Rating (1-5): ");
                int rating = Convert.ToInt32(Console.ReadLine());

                // Validate rating
                if (rating < 1 || rating > 5)
                {
                    Console.WriteLine("Invalid rating! Must be between 1 and 5.");
                    return;
                }

                // 2. Call method returning tuple
                var result = GetPerformanceData(sales, rating);

                // 3. Pattern Matching
                string performance = result switch
                {
                    ( >= 100000, >= 4) => "High Performer",
                    ( >= 50000, >= 3) => "Average Performer",
                    _ => "Needs Improvement"
                };

                // 4. Output
                Console.WriteLine("\n----- Employee Report -----");
                Console.WriteLine("Employee Name: " + name);
                Console.WriteLine("Sales Amount: " + result.sales);
                Console.WriteLine("Rating: " + result.rating);
                Console.WriteLine("Performance: " + performance);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Method returning tuple
        static (double sales, int rating) GetPerformanceData(double sales, int rating)
        {
            return (sales, rating);
        }
    }
        }
    


