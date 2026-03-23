
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp2
{
    internal class TaskManager
    {
        private List<string> tasks = new List<string>();

        // Add Task
        public void AddTask()
        {
            Console.Write("Enter task: ");
            string task = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(task))
            {
                Console.WriteLine("Task cannot be empty.");
            }
            else
            {
                tasks.Add(task);
                Console.WriteLine("Task added!");
            }
        }

        // View Tasks
        public void ViewTasks()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks available.");
                return;
            }

            Console.WriteLine("\nTasks:");
            for (int i = 0; i < tasks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {tasks[i]}");
            }
        }

        // Remove Task
        public void RemoveTask()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks to remove.");
                return;
            }

            Console.Write("Enter task number: ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int index))
            {
                if (index >= 1 && index <= tasks.Count)
                {
                    Console.WriteLine("Removed: " + tasks[index - 1]);
                    tasks.RemoveAt(index - 1);
                }
                else
                {
                    Console.WriteLine("Invalid task number.");
                }
            }
            else
            {
                Console.WriteLine("Please enter a valid number.");
            }


        }
    }
}
