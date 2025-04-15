using BankingApp.Domain.Accounts;
using BankingApp.Domain.Customers;

namespace BankingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Create a New Bank Account ===");

            // Get user info
            Console.Write("Enter your full name: ");
            var nameInput = Console.ReadLine();
            string name = nameInput ?? throw new ArgumentNullException("Name cannot be null");

            Console.Write("Enter your email: ");
            var emailInput = Console.ReadLine();
            string email = emailInput ?? throw new ArgumentNullException("Email cannot be null");

            Console.Write("Enter your birthday (yyyy-mm-dd): ");
            var birthdayInput = Console.ReadLine();
            DateTime birthdate;
            while (!DateTime.TryParse(birthdayInput, out birthdate))
            {
                Console.Write("Invalid date. Please enter again (yyyy-mm-dd): ");
                birthdayInput = Console.ReadLine();
            }

            // Get initial deposit
            Console.Write("Enter initial deposit: ");
            string depositInput = Console.ReadLine() ?? throw new ArgumentNullException("Initial deposit cannot be null");
            decimal deposit;
            while (!decimal.TryParse(depositInput, out deposit) || deposit <= 0)
            {
                Console.Write("Amount must be a positive number. Try again: ");
                depositInput = Console.ReadLine() ?? throw new ArgumentNullException("Initial deposit cannot be null");
            }

            // Create account
            var customer = new Customer(name, email, birthdate);
            var initialDeposit = new Money(deposit);
            var account = new BankAccount(customer, initialDeposit);

            // Show summary
            Console.WriteLine();
            Console.WriteLine(account.PrintAccountSummary());
        }
    }
}
