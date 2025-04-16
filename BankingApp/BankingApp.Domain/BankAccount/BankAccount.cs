
using BankingApp.Domain.Customers;

namespace BankingApp.Domain.Accounts
{
    public class BankAccount
    {   
        public Guid AccountId {get; private set; }
        public Customer Customer { get; private set; }
        public Money Balance { get; private set;}
        public AccountNumber AccountNumber { get; private set; }

        public BankAccount(Customer customer, Money initialDeposit)
        {   
            AccountId = Guid.NewGuid();
            Customer = customer;
            Balance = initialDeposit;
            AccountNumber = AccountNumber.GenerateAccountNumber();
        }

        public void Deposit(decimal amount)
        {   
            // here we do not nee varification as we do it in Money 
            Balance = Balance.Add(new Money(amount));
        }
        public string PrintAccountSummary()
        {
            return $"Account Summary:\n" +
                   $"- ID: {AccountId}\n" +
                   $"- Customer:\n {Customer.GetCustomerSummary()}\n" +
                   $"- Account Number: {AccountNumber.ToString()}\n" +
                   $"- Balance: {Balance.ToString()}\n";

        }
    }
}
