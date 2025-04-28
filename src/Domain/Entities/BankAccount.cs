using System.Data.SqlTypes;
using Domain.Exceptions;
using Domain.ValueObjects;
using Shared.OperationResults;

namespace Domain.Entities;

public sealed class BankAccount
{   
    private readonly Guid _accountNumber;
    private Money _balance;
    public Customer Customer { get; private set; }

    public BankAccount(
        Money balance,
        Customer customer)
    {   
        _accountNumber = Guid.NewGuid();
        _balance = balance;
        Customer = customer;
    }

    public BankAccount(
        Customer customer)
    {
        _accountNumber = Guid.NewGuid();
        _balance = new Money(0.00m);
        Customer = customer;
    }

    public OperationResult Deposit(decimal amount)
    {   
        if (amount == 0)
            return OperationResult.Failed("Withdraw failed: Depost money amount cannot be zero.");
             
        _balance = _balance.Add(new Money(amount));

        return OperationResult.Succeeded("Deposit succeeded.");
    }

    public OperationResult Withdraw(decimal amount)
    {
        if (amount == 0)
            return OperationResult.Failed("Withdraw failed: Withdraw Money amount cannot be zero.");
        if (amount > _balance.Value)
            return OperationResult.Failed("Withdraw failed: Insuficient funds.");
            
        _balance = _balance.Subtract(new Money(amount));

        return OperationResult.Succeeded("Withdraw succeeded.");
    }

    public string GetBalance() => $"  - Balance: {_balance:C}";

    public string PrintAccountSummary() =>  "BankAcount summary:\n" +
                                            $"  - Customer: {Customer.FullName}\n" +
                                            $"  - AccountNumber: {_accountNumber}\n" +
                                            $"  - Balance: {_balance:C}";
}