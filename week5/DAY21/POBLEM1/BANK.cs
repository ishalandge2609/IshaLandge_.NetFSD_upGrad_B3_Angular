using System;

namespace ConsoleApp1
{
    internal class BankAccount
    {
        private readonly string _accountNumber;
        private decimal _balance;

        //properties
        public string AccountNumber
        {
            get { return _accountNumber; }
        }
        public decimal Balance
        {
            get { return _balance; }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Balance cannot be negative.");
                }
                _balance = value;
            }
        }

        // intializing objects
        public BankAccount(string accountNumber, decimal balance = 0)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                throw new ArgumentException("Account number cannot be empty.");
            }

            if (balance < 0)
            {
                throw new ArgumentException("Initial balance cannot be negative.");
            }

            _accountNumber = accountNumber;
            _balance = balance;
        }

        // deposit
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Deposit amount must be greater than 0.");
                return;
            }

            Balance += amount;
        }


        // withdraw
        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Withdrawal amount must be greater than 0.");
                return;
            }

            if (amount > Balance)
            {
                Console.WriteLine("Withdrawal failed: Insufficient balance.");
                return;
            }

            Balance -= amount;
        }

        static void Main()
        {
            BankAccount acc = new BankAccount("UBI24946595", 0);

            Console.WriteLine($"Current Balance = {acc.Balance}");

            Console.Write("Deposit = ");
            decimal depositAmount = Convert.ToDecimal(Console.ReadLine());

            Console.Write("Withdraw = ");
            decimal withdrawAmount = Convert.ToDecimal(Console.ReadLine());

            acc.Deposit(depositAmount);
            acc.Withdraw(withdrawAmount);

            Console.WriteLine($"Current Balance = {acc.Balance}");

        }
    }
}
