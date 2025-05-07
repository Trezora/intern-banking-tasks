using Banking.Domain.Events;
using Banking.Domain.Primitives;
using Banking.Domain.ValueObjects;
using Banking.Shared.OperationResults;

namespace Banking.Domain.Entities;

public sealed class Customer : AggregateRoot
{
    public CustomerId CustomerId { get; private set; }
    public Name FullName { get; private set; }
    public Email EmailAddress { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    private readonly List<BankAccount> _accounts = [];
    public IReadOnlyCollection<BankAccount> Accounts => _accounts.AsReadOnly();

    internal Customer(
        CustomerId id, 
        Name fullName,
        Email email,
        DateTime dateOfBirth) : base(id)
    {
        CustomerId = id;
        FullName = fullName;   
        EmailAddress = email;
        DateOfBirth = dateOfBirth;

        RaiseDomainEvent(new CustomerRegisteredEvent(CustomerId)); 
    }

    public BankAccount OpenNewAccount(Money initialDeposit)
    {
        BankAccount newAccount = initialDeposit.Value > 0
            ? new BankAccount(Guid.NewGuid(), initialDeposit, CustomerId)
            : new BankAccount(Guid.NewGuid(), CustomerId);

        _accounts.Add(newAccount);
        
        return newAccount;
    }

    public OperationResult MakeDeposit(BankAccount bankAccount, Money amount)
    {
        var depositOperationResult = bankAccount.Deposit(amount);
        
        return depositOperationResult;
    }

    public OperationResult MakeWithdraw(BankAccount bankAccount, Money amount)
    {
        var withdrawOperationResult = bankAccount.Withdraw(amount);

        if (!withdrawOperationResult.Result) 
            RaiseDomainEvent(new AccountOverdrawnEvent(CustomerId)); 
        
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