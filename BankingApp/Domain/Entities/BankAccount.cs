using BankingApp.Domain.Entities.Customers;
using BankingApp.Domain.Shared.OperationResults;
using BankingApp.Domain.ValueObjects.AccountNumbers;
using BankingApp.Domain.ValueObjects.MoneyVO;

namespace BankingApp.Domain.Entities.BankAcounts;

public sealed class BankAccount : Entity
{
    public AccountNumber AccountNumber { get; private set; }
    public Money Balance { get; private set; }
    public Customer Customer { get; private set; }

    public BankAccount(decimal balance, Customer customer)
        : base(Guid.NewGuid())
    {
        Balance = Money.Create(balance);
        AccountNumber = AccountNumber.Create();
        Customer = customer;
    }

    public OperationResult<Money> Deposit(decimal amount)
    {
        var result = Money.Add(Balance, Money.Create(amount));

        if (!result.IsSuccess) return result;

        Balance = result.Data!;
        
        return result;
    }

    public OperationResult<Money> Withdraw(decimal amount)
    {
        var result = Money.Subtract(Balance, Money.Create(amount));

        if (!result.IsSuccess) return result;

        Balance = result.Data!;

        return result;
    }

    public Money GetBalance() => Balance;

    public string PrintAccountSummary() =>  "BankAcount summary:\n" +
                                            $"  - Customer: \n" +
                                            $"  - {Customer.GetCustomerSummary()}\n" +
                                            $"  - AccountNumber: {AccountNumber.ToString()}\n" +
                                            $"  - Balance: {Balance.ToString()}";
}