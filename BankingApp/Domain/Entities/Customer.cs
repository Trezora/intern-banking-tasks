using BankingApp.Domain.Entities.BankAcounts;
using BankingApp.Domain.ValueObjects.Emails;
using BankingApp.Domain.ValueObjects.Names;

namespace BankingApp.Domain.Entities.Customers;

public sealed class Customer : Entity
{   
    public Name FullName { get; private set; }
    public Email EmailAddress { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    private readonly List<BankAccount> _bankAccounts;
    public IReadOnlyCollection<BankAccount> BankAccounts => _bankAccounts.AsReadOnly();

    internal Customer(string fullName, string emailAddress, DateTime dateOfBirth) 
            : base(Guid.NewGuid())
    {
        FullName = Name.Create(fullName);
        EmailAddress = Email.Create(emailAddress);
        DateOfBirth = dateOfBirth;
        _bankAccounts = new List<BankAccount>();
    }

    public void OpenNewAccount(decimal initialDeposit) => _bankAccounts.Add(new BankAccount(initialDeposit, this));
    
    public string ListAccounts()
    {
        if (!_bankAccounts.Any()) return "No accounts found for this customer.";

        var summaries = _bankAccounts
            .Select(account => account.PrintAccountSummary())
            .ToList();

        return string.Join("\n\n", summaries);

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