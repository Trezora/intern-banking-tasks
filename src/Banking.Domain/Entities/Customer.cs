using Banking.Domain.Events;
using Banking.Domain.Primitives;
using Banking.Domain.ValueObjects;
using Banking.Domain.Shared;

namespace Banking.Domain.Entities;

public sealed class Customer : AggregateRoot
{
    public CustomerId CustomerId { get; private set; }
    public Name FullName { get; private set; }
    public Email EmailAddress { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    private readonly List<BankAccount> _accounts = [];
    public IReadOnlyCollection<BankAccount> Accounts => _accounts.AsReadOnly();
    
    #pragma warning disable CS8618
    private Customer() : base() { } // This is required by EF Core

    public static Customer Create(
        CustomerId id,
        Name fullName,
        Email email,
        DateTime dateOfBirth)
    {
        var customer = new Customer
        {
            CustomerId = id,
            FullName = fullName,
            EmailAddress = email,
            DateOfBirth = dateOfBirth
        };

        customer.RaiseDomainEvent(new CustomerCreatedDomainEvent(Guid.NewGuid(), id));

        return customer;
    }


    public BankAccount OpenNewAccount(Money initialDeposit)
    {
        BankAccount newAccount = BankAccount.Create(Guid.NewGuid(),  CustomerId, initialDeposit);

        _accounts.Add(newAccount);

        RaiseDomainEvent(new BankAccountCreatedDomainEvent(
                            Guid.NewGuid(),
                            newAccount.AccountNumber,
                            CustomerId)
                        );

        return newAccount;
    }

    public Result<BankAccount> MakeDeposit(BankAccount bankAccount, Money amount)
    {
        var depositOperationResult = bankAccount.Deposit(amount);
        
        return depositOperationResult;
    }

    public Result<BankAccount> MakeWithdraw(BankAccount bankAccount, Money amount)
    {
        var withdrawOperationResult = bankAccount.Withdraw(amount);
        
        return withdrawOperationResult;
    }

    public IEnumerable<string> ListAccounts()
    {
        if (_accounts.Count == 0)
            return ["Customer has no bank accounts."];
        
        return _accounts.Select(account => account.PrintAccountSummary());
    }

    public int GetAge()
    {
        var age = DateTime.Today.Year - DateOfBirth.Year;
        
        return (DateOfBirth.Date > DateTime.Today.AddYears(-age)) ? --age : age;
    }

    public string GetCustomerSummary() =>  "Customer summary:\n" +
                                            $"  - Full name: {FullName.Value}\n" +
                                            $"  - Email: {EmailAddress.Value}\n" +
                                            $"  - DateTime: {DateOfBirth:yyyy-MM-dd}";
}