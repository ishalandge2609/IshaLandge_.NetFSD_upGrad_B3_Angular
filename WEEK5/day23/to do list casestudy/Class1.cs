using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    internal class Class1
    
        {
            static void Main()
            {
                List<string> tasks = new List<string>();

                while (true)
                {
                    Console.WriteLine("\nTo-Do List Manager");
                    Console.WriteLine("1. Add Task");
                    Console.WriteLine("2. View Tasks");
                    Console.WriteLine("3. Remove Task");
                    Console.WriteLine("4. Exit");

                    Console.Write("Choose an option: ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            Console.Write("Enter task: ");
                            string task = Console.ReadLine();

                            if (!string.IsNullOrWhiteSpace(task))
                            {
                                tasks.Add(task);
                                Console.WriteLine("Task added!");
                            }
                            else
                            {
                                Console.WriteLine("Task cannot be empty.");
                            }
                            break;

                        case "2":
                            if (tasks.Count == 0)
                            {
                                Console.WriteLine("No tasks available.");
                            }
                            else
                            {
                                Console.WriteLine("Tasks:");
                                for (int i = 0; i < tasks.Count; i++)
                                {
                                    Console.WriteLine($"{i + 1}. {tasks[i]}");
                                }
                            }
                            break;

                        case "3":
                            if (tasks.Count == 0)
                            {
                                Console.WriteLine("No tasks to remove.");
                                break;
                            }

                            Console.Write("Enter task number to remove: ");
                            string input = Console.ReadLine();

                            try
                            {
                                int index = Convert.ToInt32(input);

                                if (index >= 1 && index <= tasks.Count)
                                {
                                    string removed = tasks[index - 1];
                                    tasks.RemoveAt(index - 1);
                                    Console.WriteLine($"Removed: {removed}");
                                }
                                else
                                {
                                    Console.WriteLine("Invalid task number.");
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Please enter a valid number.");
                            }
                            break;

                        case "4":
                            return;

                        default:
                            Console.WriteLine("Invalid option.");
                            break;
                    }
                }
            }
        }
    }
