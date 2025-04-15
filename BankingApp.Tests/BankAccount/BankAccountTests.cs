using BankingApp.Domain.Accounts;
using BankingApp.Domain.Customers;
using Xunit;

namespace BankingApp.Tests.BankAccounts
{   
    public class BankAccountTests
    {   
        private readonly Customer _customer;
        private readonly BankAccount _account;
        
        public BankAccountTests()
        {   
            _customer = new Customer(
                "Beka Buliskeria",
                "abc@gmail.com",    
                new DateTime(2003, 8, 27)
            );
            Money initialDeposit = new Money(150.0m);

            _account = new BankAccount(_customer, initialDeposit);
        }

        [Fact]
        public void TestInvalidBankAccountCreation()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var invalidDeposit = new Money(-100.0m);
                var invalidBankAccount = new BankAccount(_customer, invalidDeposit);
            });
        }

        [Fact]
        public void TestBankAccountSummary()
        {
    
            var summary = _account.PrintAccountSummary();
            
            Assert.Contains(_account.AccountId.ToString(), summary);
            Assert.Contains(_account.Customer.GetCustomerSummary(), summary);
            Assert.Contains(_account.Balance.ToString(), summary);
            Assert.Contains(_account.AccountNumber.ToString(), summary);

        }
    }
}