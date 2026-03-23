using System.Text;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {

            try
            {
                Console.Write("Enter your message: ");
                string message = Console.ReadLine();

                // File path
                string filePath = "log.txt";

                // Convert string to byte array
                byte[] data = Encoding.UTF8.GetBytes(message + Environment.NewLine);

                // Open file in append mode
                using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write))
                {
                    fs.Write(data, 0, data.Length);
                }

                Console.WriteLine("Message successfully written to file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        }
    }


