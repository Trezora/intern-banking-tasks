using Banking.Domain.Primitives;
using Banking.Domain.ValueObjects;
using Banking.Domain.Shared;

namespace Banking.Domain.Entities;

public sealed class BankAccount : Entity
{   
    public Guid AccountNumber { get; private set; }
    private Money _balance;
    
    // public property for EF Core to map
    public decimal Balance 
    { 
        get => _balance.Value;
        private set => _balance = new Money(value);
    }
    
    public CustomerId CustomerId { get; private set; }
    
    #pragma warning disable CS8618
    private BankAccount() : base() { } // This is required by EF Core

    public static BankAccount Create(
        Guid accountNumber,
        CustomerId customerId,
        Money balance)
    {
        var bankAccount = new BankAccount
        {
            AccountNumber = accountNumber,
            CustomerId = customerId,
            Balance = balance
        };

        return bankAccount;
    }

    internal Result<BankAccount> Deposit(Money amount)
    {
        if (amount.Value == 0)
        {
            return Result<BankAccount>.FailureWith("Deposit failed: Deposit money amount cannot be zero.");
        }

        _balance = _balance.Add(amount);

        return Result<BankAccount>.Success(this);
    }

    internal Result<BankAccount> Withdraw(Money amount)
    {
        if (amount.Value == 0)
        {   
            return Result<BankAccount>.FailureWith("Withdraw failed: Withdraw Money amount cannot be zero.");
        }

        if (amount.Value > _balance.Value)
        {
            return Result<BankAccount>.FailureWith("Withdraw failed: Insuficient funds.");
        }
            
        _balance = _balance.Subtract(amount);

        return Result<BankAccount>.Success(this);
    }

    public Money GetBalance() => _balance;

    public string PrintAccountSummary() =>  "BankAcount summary:\n" +
                                            $"  - Customer: {CustomerId}\n" +
                                            $"  - AccountNumber: {AccountNumber}\n" +
                                            $"  - Balance: {_balance:C}";

}