using System.Text;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {

            try
            {
                Console.Write("Enter folder path: ");
                string folderPath = Console.ReadLine();

                // Check if directory exists
                if (!Directory.Exists(folderPath))
                {
                    Console.WriteLine("Invalid directory path!");
                    return;
                }

                // Get all files from directory
                string[] files = Directory.GetFiles(folderPath);

                int fileCount = 0;

                foreach (string file in files)
                {
                    FileInfo fileInfo = new FileInfo(file);

                    Console.WriteLine("----------------------------");
                    Console.WriteLine("File Name: " + fileInfo.Name);
                    Console.WriteLine("File Size: " + fileInfo.Length + " bytes");
                    Console.WriteLine("Created On: " + fileInfo.CreationTime);

                    fileCount++;
                }

                Console.WriteLine("----------------------------");
                Console.WriteLine("Total Files: " + fileCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        }
    }


