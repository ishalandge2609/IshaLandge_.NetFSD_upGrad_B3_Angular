using System;
namespace ConsoleApp2
{
    

    class Program
    {
        static int CountVowels(string text)
        {
            int count = 0;

            foreach (char ch in text.ToLower())
            {
                if (ch == 'a' || ch == 'e' || ch == 'i' || ch == 'o' || ch == 'u')
                {
                    count++;
                }
            }

            return count;
        }

        static void Main()
        {
            Console.Write("Enter a word: ");
            string input = Console.ReadLine();

            int result = CountVowels(input);

            Console.WriteLine("Number of vowels: " + result);
        }
    }
}
