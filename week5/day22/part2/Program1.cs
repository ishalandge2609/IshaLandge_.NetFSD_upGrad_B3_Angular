using System;

namespace ConsoleApp1
{
    class Node
    {
        public int Id;
        public string Name;
        public Node Next;

        public Node(int id, string name)
        {
            Id = id;
            Name = name;
            Next = null;
        }
    }

    class EmployeeLinkedList
    {
        private Node head;

        public void InsertAtBeginning(int id, string name)
        {
            Node newNode = new Node(id, name);
            newNode.Next = head;
            head = newNode;
        }

        public void InsertAtEnd(int id, string name)
        {
            Node newNode = new Node(id, name);
            if (head == null)
            {
                head = newNode;
                return;
            }
            Node temp = head;
            while (temp.Next != null) temp = temp.Next;
            temp.Next = newNode;
        }

        public void Delete(int id)
        {
            if (head == null) return;
            if (head.Id == id)
            {
                head = head.Next;
                return;
            }
            Node temp = head;
            while (temp.Next != null && temp.Next.Id != id) temp = temp.Next;
            if (temp.Next != null) temp.Next = temp.Next.Next;
        }

        public void Display()
        {
            Console.WriteLine("Employee List:");
            Node temp = head;
            while (temp != null)
            {
                Console.WriteLine($"{temp.Id} - {temp.Name}");
                temp = temp.Next;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            EmployeeLinkedList list = new EmployeeLinkedList();

            //  Insertion at End
            list.InsertAtEnd(101, "John");
            list.InsertAtEnd(103, "Mike");

            // Insertion at Beginning
            list.InsertAtBeginning(102, "Sara");

            // Deletion
            list.Delete(102);

            list.Display();
            Console.ReadLine();
        }
    }
}




