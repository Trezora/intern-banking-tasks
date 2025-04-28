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

    public void OpenNewAccount(decimal initialDeposit = 0)
    {
        BankAccount newAccount = initialDeposit > 0
            ? new BankAccount(new Money(initialDeposit), this)
            : new BankAccount(this);

            _accounts.Add(newAccount);
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
                                            $"  - Full name: {FullName.ToString()}\n" +
                                            $"  - Email: {EmailAddress.ToString()}\n" +
                                            $"  - DateTime: {DateOfBirth.ToString("yyyy-MM-dd")}";
}