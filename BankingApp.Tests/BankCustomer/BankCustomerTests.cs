using BankingApp.Domain.Customer;
using Xunit;

namespace BankingApp.Tests.BankCustomer
{  
    public class BankCustomerTests
    {
        private readonly Customer _bankCustomer1;
         private readonly Customer _bankCustomer2;

        public BankCustomerTests()
        {
            _bankCustomer1 = new Customer(
                Guid.NewGuid(),
                "Beka Buliskeria",
                "abc@gmail.com",
                new DateTime(2003, 8, 27)
            );

            _bankCustomer2 = new Customer(
                Guid.NewGuid(),
                "Zeragia",
                "abcd@gmail.com",
                new DateTime(2003, 2, 27)
            );

        }

        [Fact]
        public void TestBankCustomerSummary()
        {
            var summary = _bankCustomer1.GetCustomerSummary();

            Assert.Contains("- Name: Beka Buliskeria", summary);
            Assert.Contains("- Email: abc@gmail.com", summary);
            Assert.Contains("- Date of Birth: 2003-08-27", summary);
            Assert.Contains("- Age: 21", summary);
        }
        
        [Fact]
        public void TestGetAgeBeforeBirthDay()
        {
            Assert.True(_bankCustomer2.GetAge() == 22);
        }

        [Fact]
        public void TestGetAgeAfterBirthday()
        {
            Assert.True(_bankCustomer1.GetAge() == 21);
        }
    }
}
