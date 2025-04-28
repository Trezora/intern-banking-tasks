using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Customer
{
    public Guid Id { get; private set; }
    public Name FullName { get; private set; }
    public Email EmailAddress { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    private readonly List<BankAccount> _accounts = [];
    public IReadOnlyCollection<BankAccount> Accounts => _accounts.AsReadOnly();

    internal Customer(
        Guid id,
        Name fullName,
        Email email,
        DateTime dateOfBirth)
    {
        Id = id;
        FullName = fullName;   
        EmailAddress = email;
        DateOfBirth = dateOfBirth;
    }

    public BankAccount OpenNewAccount(decimal initialDeposit = 0)
    {
        BankAccount newAccount = initialDeposit > 0
            ? new BankAccount(new Money(initialDeposit), this)
            : new BankAccount(this);

            _accounts.Add(newAccount);
        
        return newAccount;
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