using System.Data.SqlTypes;
using Domain.Exceptions;
using Domain.Services;
using Domain.ValueObjects;
using Shared.OperationResults;

namespace Domain.Entities;

public sealed class BankAccount
{   
    private readonly Guid _accountNumber;
    private Money _balance;
    public Customer Customer { get; private set; }

    private readonly ITransactionLogger? _transactionLogger;
    private readonly IAccountNotifier? _accountNotifier;

    public BankAccount(
        Money balance,
        Customer customer,
        ITransactionLogger? transactionLogger = null,
        IAccountNotifier? accountNotifier = null)
    {   
        _accountNumber = Guid.NewGuid();
        _balance = balance;
        Customer = customer;
        _transactionLogger = transactionLogger;
        _accountNotifier = accountNotifier;
    }

    public BankAccount(
        Customer customer,
        ITransactionLogger? transactionLogger = null,
        IAccountNotifier? accountNotifier = null)
    {
        _accountNumber = Guid.NewGuid();
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
            _transactionLogger?.LogFailedTransaction(_accountNumber, errorMessage);

            return OperationResult.Failed("Withdraw failed: Deposit money amount cannot be zero.");
        }
             
        _balance = _balance.Add(new Money(amount));
        _transactionLogger?.LogDeposit(_accountNumber, amount, _balance);
        _accountNotifier?.NotifySuccessfulDeposit(_accountNumber, Customer.FullName, Customer.EmailAddress, amount);

        return OperationResult.Succeeded("Deposit succeeded.");
    }

    public OperationResult Withdraw(decimal amount)
    {
        if (amount == 0)
        {   
            var errorMessage = "Withdraw failed: Withdraw money amount cannot be zero.";
            _transactionLogger?.LogFailedTransaction(_accountNumber, errorMessage);

            return OperationResult.Failed("Withdraw failed: Withdraw Money amount cannot be zero.");
        }

        if (amount > _balance.Value)
        {   
            var errorMessage = "Withdraw failed: Insufficient funds.";
            _transactionLogger?.LogFailedTransaction(_accountNumber, errorMessage);

            return OperationResult.Failed("Withdraw failed: Insuficient funds.");
        }
            
        _balance = _balance.Subtract(new Money(amount));
        _transactionLogger?.LogWithdrawal(_accountNumber, amount, _balance);
        _accountNotifier?.NotifySuccessfulWithdrawal(_accountNumber, Customer.FullName, Customer.EmailAddress, amount);

        return OperationResult.Succeeded("Withdraw succeeded.");
    }

    public string GetBalance() => $"  - Balance: {_balance:C}";

    public string PrintAccountSummary() =>  "BankAcount summary:\n" +
                                            $"  - Customer: {Customer.FullName}\n" +
                                            $"  - AccountNumber: {_accountNumber}\n" +
                                            $"  - Balance: {_balance:C}";
}