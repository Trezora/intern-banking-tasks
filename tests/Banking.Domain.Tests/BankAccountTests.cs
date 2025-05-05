using Banking.Domain.Entities;
using Banking.Domain.Exceptions;
using Banking.Domain.Factories;

namespace Banking.Domain.Tests;

public class BankAccountTests
{
    private Customer _customer;

    public BankAccountTests()
    {   
        var customerFactory = new CustomerFactory();
        _customer = customerFactory.CreateCustomer(
            "John Snow",
            "abcd@gmail.com",
            new(2003, 2, 4)
        );
    }

    [Fact]
    public void Should_return_bank_account_summary()
    {   
        // Arrange
        var bankAccount = new BankAccount(
            _customer
        );

        // Act
        var summary = bankAccount.PrintAccountSummary();  

        // Assert
        Assert.NotNull(bankAccount);
        Assert.Contains(_customer.FullName, summary);
        Assert.Contains(bankAccount.GetBalance(), summary);
    }

    [Fact]
    public void Should_throw_NegativeMoneyAmountException_during_bank_account_creation_with_invalid_initial_deposit()
    {
        // Arrange & Act & Assert
        var exception = Assert.Throws<NegativeMoneyAmountException>(() =>
            new BankAccount(-100.00m, _customer));
    }

    [Fact]
    public void Should_make_valid_deposit_and_return_OperatioResult_success()
    {   
        // Arrange
        var bankAccount = new BankAccount(
            _customer
        );

        // Act
        var operationResult = bankAccount.Deposit(100.00m);

        // Assert
        Assert.True(operationResult.Result);
        Assert.Equal("Deposit succeeded.", operationResult.Message);
        Assert.Equal($"  - Balance: Money {{ Value = {100.00m} }}", bankAccount.GetBalance());
    }

    [Fact]
    public void Should_make_invalid_deposit_and_return_OperationResult_failure()
    {
        // Arrange
        var bankAccount = new BankAccount(
            _customer
        );

        // Act
        var operationResult = bankAccount.Deposit(0.00m);

        // Assert
        Assert.False(operationResult.Result);
        Assert.Equal("Withdraw failed: Deposit money amount cannot be zero.", operationResult.Message);
        Assert.Equal($"  - Balance: Money {{ Value = {0.00m} }}", bankAccount.GetBalance());
    }

    [Fact]
    public void Should_make_valid_withdraw_and_return_OperationResult_success()
    {
        // Arrange
        var bankAccount = new BankAccount(
            200.00m,
            _customer
        );

        // Act
        var operationResult = bankAccount.Withdraw(125.00m);

        // Assert
        Assert.True(operationResult.Result);
        Assert.Equal("Withdraw succeeded.", operationResult.Message);
        Assert.Equal($"  - Balance: Money {{ Value = {75.00m} }}", bankAccount.GetBalance());
    }

    [Fact]
    public void Should_make_valid_withdraw_and_return_OperationResult_failure()
    {
        // Arrange
        var bankAccount = new BankAccount(
            200.00m,
            _customer
        );

        // Act
        var operationResult = bankAccount.Withdraw(225.00m);

        // Assert
        Assert.False(operationResult.Result);
        Assert.Equal("Withdraw failed: Insuficient funds.", operationResult.Message);
        Assert.Equal($"  - Balance: Money {{ Value = {200.00m} }}", bankAccount.GetBalance());
    }

    [Theory]
    [MemberData(nameof(GetTransactionSequences))]
    public void Should_Process_Complex_Transaction_Sequences(
        decimal initialBalance, 
        (string type, decimal amount)[] transactions, 
        decimal expectedFinalBalance)
    {
        // Arrange
        var bankAccount = new BankAccount(initialBalance, _customer);
            
        // Act
        transactions.ToList().ForEach(t =>
        {   
            if (t.type == "deposit")
                bankAccount.Deposit(t.amount);
            else if (t.type == "withdraw")
                bankAccount.Withdraw(t.amount);
        });

        // Assert
        Assert.Equal($"  - Balance: Money {{ Value = {expectedFinalBalance} }}", bankAccount.GetBalance());
    }

    /*
     * Method for Multiple Depost Withdraw operations.
     */
    public static IEnumerable<object[]> GetTransactionSequences()
    {
        yield return new object[]
        {
            1000.00m, 
            new (string, decimal)[]
            {
                ("deposit", 500.00m),
                ("withdraw", 200.00m),
                ("deposit", 100.00m),
                ("withdraw", 300.00m),
            },
            1100.00m 
        };
            
        yield return new object[]
        {
            0.00m,  
            new (string, decimal)[]
            {
                ("deposit", 1000.00m),
                ("withdraw", 250.00m),
                ("deposit", 50.00m),
                ("withdraw", 150.00m),
                ("deposit", 25.00m),
            },
            675.00m  
        };
            
        yield return new object[]
        {
            500.00m,   
            new (string, decimal)[]
            {
                ("withdraw", 100.00m),
                ("withdraw", 200.00m),
                ("withdraw", 199.00m),
            },
            1.00m      
        };
            
        yield return new object[]
        {
            100.00m,  
            new (string, decimal)[]
            {
                ("deposit", 100.00m),
                ("deposit", 100.00m),
                ("withdraw", 250.00m),
                ("deposit", 50.00m),
            },
            100.00m    
        };
    }
}