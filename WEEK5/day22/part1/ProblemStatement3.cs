using System;

namespace ConsoleApp1
{
    public class InsufficientBalanceException : Exception
    {
        public InsufficientBalanceException(string message) : base(message) { }
    }

    class BankAccount
    {
        private double balance;

        public BankAccount(double initialBalance)
        {
            balance = initialBalance;
        }

        public void Withdraw(double amount)
        {
            if (amount > balance)
            {
                throw new InsufficientBalanceException("Withdrawal amount exceeds available balance.");
            }
            balance -= amount;
            Console.WriteLine("Remaining Balance: " + balance);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Balance = ");
            double initialBalance = double.Parse(Console.ReadLine());

            BankAccount account = new BankAccount(initialBalance);

            Console.Write("Withdraw = ");
            double withdrawAmount = double.Parse(Console.ReadLine());

            try
            {
                account.Withdraw(withdrawAmount);
            }
            catch (InsufficientBalanceException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("Transaction process ended.");
            }

            Console.ReadLine();
        }
    }
}






