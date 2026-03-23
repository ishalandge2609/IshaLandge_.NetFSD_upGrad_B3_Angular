using System.Text;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Get all drives
                DriveInfo[] drives = DriveInfo.GetDrives();

                Console.WriteLine("---- Drive Information ----\n");

                foreach (DriveInfo drive in drives)
                {
                    // Check if drive is ready
                    if (drive.IsReady)
                    {
                        Console.WriteLine("----------------------------");
                        Console.WriteLine("Drive Name: " + drive.Name);
                        Console.WriteLine("Drive Type: " + drive.DriveType);

                        // Convert bytes to GB
                        double totalSize = drive.TotalSize / (1024.0 * 1024 * 1024);
                        double freeSpace = drive.AvailableFreeSpace / (1024.0 * 1024 * 1024);

                        Console.WriteLine("Total Size: " + totalSize.ToString("F2") + " GB");
                        Console.WriteLine("Free Space: " + freeSpace.ToString("F2") + " GB");

                        // Calculate free space percentage
                        double freePercent = (drive.AvailableFreeSpace / (double)drive.TotalSize) * 100;

                        Console.WriteLine("Free Space %: " + freePercent.ToString("F2") + "%");

                        // Warning condition
                        if (freePercent < 15)
                        {
                            Console.WriteLine("⚠ Warning: Low disk space!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("----------------------------");
                        Console.WriteLine("Drive Name: " + drive.Name);
                        Console.WriteLine("Drive not ready.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        }
    }

    


