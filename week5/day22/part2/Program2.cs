using System;

namespace ConsoleApp1
{
    class UndoStack
    {
        private string[] stack;
        private int top;
        private int capacity;

        public UndoStack(int size)
        {
            capacity = size;
            stack = new string[capacity];
            top = -1;
        }

        public void Push(string action)
        {
            if (top == capacity - 1)
            {
                Console.WriteLine("Stack is full.");
                return;
            }
            stack[++top] = action;
            Display();
        }

        public void Pop()
        {
            if (top == -1)
            {
                Console.WriteLine("Stack is empty.");
                return;
            }
            top--;
            Display();
        }

        public void Display()
        {
            if (top == -1)
            {
                Console.WriteLine("Current State After Operations: [Empty]");
                return;
            }
            Console.Write("Current State After Operations: ");
            for (int i = 0; i <= top; i++)
            {
                Console.Write(stack[i] + (i == top ? "" : ", "));
            }
            Console.WriteLine();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            UndoStack undoSystem = new UndoStack(10);
            undoSystem.Push("Type A");
            undoSystem.Push("Type B");
            undoSystem.Push("Type C");
            undoSystem.Pop();
            undoSystem.Pop();
            Console.ReadLine();
        }
    }
}



