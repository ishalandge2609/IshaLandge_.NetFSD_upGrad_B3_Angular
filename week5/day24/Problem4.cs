using System.Text;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
             try
                {
                    Console.Write("Enter root directory path: ");
                    string rootPath = Console.ReadLine();

                    // Check if directory exists
                    if (!Directory.Exists(rootPath))
                    {
                        Console.WriteLine("Invalid directory path!");
                        return;
                    }

                    // Create DirectoryInfo object
                    DirectoryInfo dirInfo = new DirectoryInfo(rootPath);

                    // Get all subdirectories
                    DirectoryInfo[] subDirs = dirInfo.GetDirectories();

                    Console.WriteLine("\n---- Folder Analysis ----");

                    foreach (DirectoryInfo subDir in subDirs)
                    {
                        // Count files in each directory
                        FileInfo[] files = subDir.GetFiles();

                        Console.WriteLine("----------------------------");
                        Console.WriteLine("Folder Name: " + subDir.Name);
                        Console.WriteLine("Number of Files: " + files.Length);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }

    


