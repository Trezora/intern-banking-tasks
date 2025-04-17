using System.Threading.Tasks.Dataflow;
using BankingApp.Domain.Entities.BankAcounts;
using BankingApp.Domain.Entities.Customers;
using BankingApp.Domain.ValueObjects.MoneyVO;

namespace BankingApp.Domain.Tests;

public class BankAccountTests
{   
    private readonly Customer _customer;

    public BankAccountTests()
    {
        _customer = new Customer(
            fullName: "Beka Bulikseria",
            emailAddress: "abcd@gmail.com",
            dateOfBirth: new DateTime(2003, 3, 27)
        );
    }

    [Fact]
    public void CreateBankAccountWithValidBalance()
    {
        // Arrange
        var bankAccount = new BankAccount(100.00m,  _customer);

        // Act & Assert
        Assert.NotNull(_customer);
        Assert.NotNull(bankAccount);
        Assert.Equal(bankAccount.Balance, Money.Create(100.00m));
        Assert.Equal(bankAccount.Customer, _customer);
    }

    [Fact]
    public void CreateBankAccountWithInvalidBalance()
    {
        // Arrange & Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
            new BankAccount(-100.00m,  _customer));
    }

    [Fact]
    public void CreateBankAccoutWithValidBalanceAndMakeInvalidDeposit()
    {
        // Arrange
        var bankAccount = new BankAccount(100.00m, _customer);

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            bankAccount.Deposit(-150.00m));
    }

    [Fact]
    public void CreateBankAccountWithValidBalanceAndMakeValidDeposit()
    {
        // Arrange
        var bankAccount = new BankAccount(100.00m, _customer);

        // Act
        bankAccount.Deposit(100.00m);

        // Assert
        Assert.Equal("$200.00", bankAccount.Balance.ToString());
    }

    [Theory]
    [InlineData(100.00, new double[] { 50.00, 25.00, 25.00 }, "$200.00")]
    [InlineData(200.00, new double[] { 0.00 }, "$200.00")]
    [InlineData(150.00, new double[] { 100.00, 50.00 }, "$300.00")]
    public void CreateBankAccountWithValidBalanceAndMakeValidSeveralDeposit(
        decimal initialDeposit,
        double[] depositAmounts,
        string expectedBalance)
    {

        // Arrange
        var account = new BankAccount(initialDeposit, _customer);

        // Act
        depositAmounts
            .Select(amount => (decimal)amount)
            .ToList()
            .ForEach(account.Deposit);

        // Assert
        Assert.Equal(expectedBalance, account.Balance.ToString());
    }

    [Fact]
    public void CreateValidBankAccountAndTestAccountSummary()
    {   
        // Arrange
        var bankAccount = new BankAccount(100.00m, _customer);

        // Act
        var bankAccountSummary = bankAccount.PrintAccountSummary();

        var validBankAccountSummary =  "BankAcount summary:\n" +
                            $"  - Customer: \n" +
                            $"  - {_customer.GetCustomerSummary()}\n" +
                            $"  - AccountNumber: {bankAccount.AccountNumber.ToString()}\n" +
                            $"  - Balance: {bankAccount.Balance.ToString()}";

        // Assert
        Assert.NotNull(bankAccount);

        Assert.Equal(bankAccountSummary, validBankAccountSummary);
        Assert.Contains(bankAccount.AccountNumber.ToString(), bankAccountSummary);
        Assert.Contains(bankAccount.Balance.ToString(), bankAccountSummary);
        Assert.Contains(bankAccount.Customer.GetCustomerSummary(), bankAccountSummary);
    }
}
