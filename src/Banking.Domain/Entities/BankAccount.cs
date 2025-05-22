using Banking.Domain.Primitives;
using Banking.Domain.Services;
using Banking.Domain.ValueObjects;
using Banking.Shared.OperationResults;

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

    private readonly ITransactionLogger? _transactionLogger;
    private readonly IAccountNotifier? _accountNotifier;
    
    #pragma warning disable CS8618
    private BankAccount() : base() { } // This is required by EF Core

    public BankAccount(
        Guid accountNumber,
        Money balance,
        CustomerId customerId,
        ITransactionLogger? transactionLogger = null,
        IAccountNotifier? accountNotifier = null) : base(accountNumber)
    {
        AccountNumber = accountNumber;
        _balance = balance;
        CustomerId = customerId;
        _transactionLogger = transactionLogger;
        _accountNotifier = accountNotifier;
    }

    public BankAccount(
        Guid accountNumber,
        CustomerId customerId,
        ITransactionLogger? transactionLogger = null,
        IAccountNotifier? accountNotifier = null) : base(accountNumber)
    {
        AccountNumber = accountNumber;
        _balance = new Money(0.00m);
        CustomerId = customerId;
        _transactionLogger = transactionLogger;
        _accountNotifier = accountNotifier;
    }

    internal Result<BankAccount> Deposit(Money amount)
    {   
        if (amount.Value == 0)
        {
            var errorMessage = "Deposit failed: Deposit money amount cannot be zero.";
            _transactionLogger?.LogFailedTransaction(AccountNumber, errorMessage);

            return Result<BankAccount>.FailureWith("Deposit failed: Deposit money amount cannot be zero.");
        }
             
        _balance = _balance.Add(amount);
        _transactionLogger?.LogDeposit(AccountNumber, amount, _balance);
        _accountNotifier?.NotifySuccessfulDeposit(AccountNumber, CustomerId, amount);

        return Result<BankAccount>.Success(this);
    }

    internal Result<BankAccount> Withdraw(Money amount)
    {
        if (amount.Value == 0)
        {   
            var errorMessage = "Withdraw failed: Withdraw money amount cannot be zero.";
           _transactionLogger?.LogFailedTransaction(AccountNumber, errorMessage);

            return Result<BankAccount>.FailureWith("Withdraw failed: Withdraw Money amount cannot be zero.");
        }

        if (amount.Value > _balance.Value)
        {   
            var errorMessage = "Withdraw failed: Insufficient funds.";
            _transactionLogger?.LogFailedTransaction(AccountNumber, errorMessage);

            return Result<BankAccount>.FailureWith("Withdraw failed: Insuficient funds.");
        }
            
        _balance = _balance.Subtract(amount);
        _transactionLogger?.LogWithdrawal(AccountNumber, amount, _balance);
        _accountNotifier?.NotifySuccessfulWithdrawal(AccountNumber, CustomerId, amount);

        return Result<BankAccount>.Success(this);
    }

    public Money GetBalance() => _balance;

    public string PrintAccountSummary() =>  "BankAcount summary:\n" +
                                            $"  - Customer: {CustomerId}\n" +
                                            $"  - AccountNumber: {AccountNumber}\n" +
                                            $"  - Balance: {_balance:C}";

}