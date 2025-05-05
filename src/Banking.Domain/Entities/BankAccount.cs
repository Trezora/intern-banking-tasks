using Banking.Domain.Primitives;
using Banking.Domain.Services;
using Banking.Domain.ValueObjects;
using Banking.Shared.OperationResults;

namespace Banking.Domain.Entities;

public sealed class BankAccount : Entity
{   
    public Guid AccountNumber { get; private set; }
    private Money _balance;
    public Customer Customer { get; private set; }

    private readonly ITransactionLogger? _transactionLogger;
    private readonly IAccountNotifier? _accountNotifier;

    public BankAccount(
        Guid accountNumber,
        Money balance,
        Customer customer,
        ITransactionLogger? transactionLogger = null,
        IAccountNotifier? accountNotifier = null) : base(accountNumber)
    {   
        AccountNumber = accountNumber;
        _balance = balance;
        Customer = customer;
        _transactionLogger = transactionLogger;
        _accountNotifier = accountNotifier;
    }

    public BankAccount(
        Guid accountNumber,
        Customer customer,
        ITransactionLogger? transactionLogger = null,
        IAccountNotifier? accountNotifier = null) : base(accountNumber)
    {
        AccountNumber = accountNumber;
        _balance = new Money(0.00m);
        Customer = customer;
        _transactionLogger = transactionLogger;
        _accountNotifier = accountNotifier;
    }

    public OperationResult Deposit(decimal amount)
    {   
        if (amount == 0)
        {
            var errorMessage = "Deposit failed: Deposit money amount cannot be zero.";
            _transactionLogger?.LogFailedTransaction(AccountNumber, errorMessage);

            return OperationResult.Failed("Withdraw failed: Deposit money amount cannot be zero.");
        }
             
        _balance = _balance.Add(new Money(amount));
        _transactionLogger?.LogDeposit(AccountNumber, amount, _balance);
        _accountNotifier?.NotifySuccessfulDeposit(AccountNumber, Customer.FullName, Customer.EmailAddress, amount);

        return OperationResult.Succeeded("Deposit succeeded.");
    }

    public OperationResult Withdraw(decimal amount)
    {
        if (amount == 0)
        {   
            var errorMessage = "Withdraw failed: Withdraw money amount cannot be zero.";
           _transactionLogger?.LogFailedTransaction(AccountNumber, errorMessage);

            return OperationResult.Failed("Withdraw failed: Withdraw Money amount cannot be zero.");
        }

        if (amount > _balance.Value)
        {   
            var errorMessage = "Withdraw failed: Insufficient funds.";
            _transactionLogger?.LogFailedTransaction(AccountNumber, errorMessage);

            return OperationResult.Failed("Withdraw failed: Insuficient funds.");
        }
            
        _balance = _balance.Subtract(new Money(amount));
        _transactionLogger?.LogWithdrawal(AccountNumber, amount, _balance);
        _accountNotifier?.NotifySuccessfulWithdrawal(AccountNumber, Customer.FullName, Customer.EmailAddress, amount);

        return OperationResult.Succeeded("Withdraw succeeded.");
    }

    public string GetBalance() => $"  - Balance: {_balance:C}";

    public string PrintAccountSummary() =>  "BankAcount summary:\n" +
                                            $"  - Customer: {Customer.FullName}\n" +
                                            $"  - AccountNumber: {AccountNumber}\n" +
                                            $"  - Balance: {_balance:C}";
}