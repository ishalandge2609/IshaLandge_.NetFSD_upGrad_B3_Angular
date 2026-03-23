namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
       
            {
                TaskManager manager = new TaskManager();
                int choice;

                do
                {
                    Console.WriteLine("\nTo-Do List Manager");
                    Console.WriteLine("1. Add Task");
                    Console.WriteLine("2. View Tasks");
                    Console.WriteLine("3. Remove Task");
                    Console.WriteLine("4. Exit");

                    Console.Write("Choose an option: ");
                    string input = Console.ReadLine();

                    if (!int.TryParse(input, out choice))
                    {
                        Console.WriteLine("Invalid input!");
                        continue;
                    }

                    switch (choice)
                    {
                        case 1:
                            manager.AddTask();
                            break;

                        case 2:
                            manager.ViewTasks();
                            break;

                        case 3:
                            manager.RemoveTask();
                            break;

                        case 4:
                            Console.WriteLine("Thank you for using To-Do List Manager!");
                            break;

                        default:
                            Console.WriteLine("Invalid choice!");
                            break;
                    }

                } while (choice != 4);
            }
        }
    }
}

