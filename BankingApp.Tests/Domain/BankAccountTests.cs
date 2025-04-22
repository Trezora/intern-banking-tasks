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
            bankAccount.Deposit(-10.00m));

    }

    [Fact]
    public void CreateBankAccountWithValidBalanceAndMakeValidDeposit()
    {
        // Arrange
        var bankAccount = new BankAccount(100.00m, _customer);

        // Act
        var result = bankAccount.Deposit(100.00m);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("$200.00", bankAccount.GetBalance().ToString());
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

        // Act & Assert
        foreach (var amount in depositAmounts.Select(a => (decimal)a))
        {
            var result = account.Deposit(amount);
            Assert.True(result.IsSuccess, $"Deposit failed: {result.Message}");
        }

        Assert.Equal(expectedBalance, account.GetBalance().ToString());
    }


    [Fact]
    public void CreateValidBankAccountWithValidBalanceAndMakeInvalidWithdraw()
    {
        // Arrange
        var bankAccount = new BankAccount(500.00m, _customer);

        // Act
        var result = bankAccount.Withdraw(501.00m);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Failure: Insufficient funds.", result.Message);
    }

    [Theory]
    [InlineData(500.00, new double[] {10.00, 150.00, 30.00}, "$310.00")]
    [InlineData(500.00, new double[] {100.00, 250.00, 50.00}, "$100.00")]
    [InlineData(500.00, new double[] {125.00, 5.00, 0.00}, "$370.00")]
    public void CreateValidBankAccountWithValidBalanceAndMakeValidSeveralWithdraw(
        decimal initialDeposit,
        double[] withdrawAmounts, 
        string expectedBalance)
    {
        // Arrange
        var account = new BankAccount(initialDeposit, _customer);

        // Act & Assert
        foreach (var amount in withdrawAmounts.Select(a => (decimal)a))
        {
            var result = account.Withdraw(amount);
            Assert.True(result.IsSuccess, $"Withdraw failed: {result.Message}");
        }

        Assert.Equal(expectedBalance, account.GetBalance().ToString());
    }

    [Theory]
    [InlineData(300.00, new double[] { 0.00, 290.00, 10.00, 5.00, 10.00, 25.00 }, "$0.00")]
    public void CreateValidBankAccountWithValidBalanceAndMakeSeveralDepositAndWithdraw(
        decimal initialDeposit,
        double[] transactionAmounts,
        string expectedBalance)
    {
        // Arrange
        var account = new BankAccount(initialDeposit, _customer);

        // Act & Assert
        transactionAmounts
            .Select((amount, index) => new { Amount = (decimal)amount, Index = index })
            .ToList()
            .ForEach(entry =>
            {
                if (entry.Index % 2 == 0)
                {
                    var result = account.Deposit(entry.Amount);
                    Assert.True(result.IsSuccess, $"Deposit failed at index {entry.Index}: {result.Message}");
                }
                else
                {
                    var result = account.Withdraw(entry.Amount);
                    Assert.True(result.IsSuccess, $"Withdraw failed at index {entry.Index}: {result.Message}");
                }
            });

        // Assert final balance
        Assert.Equal(expectedBalance, account.GetBalance().ToString());
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
