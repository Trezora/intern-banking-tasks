using BankingApp.Domain.ValueObjects.Emails;
using BankingApp.Domain.ValueObjects.Names;

namespace BankingApp.Domain.Entities.Customers;

public sealed class Customer : Entity
{   
    public Name FullName { get; private set; }
    public Email EmailAddress { get; private set; }
    public DateTime DateOfBirth { get; private set; }

    public Customer(string fullName, string emailAddress, DateTime dateOfBirth) 
            : base(Guid.NewGuid())
    {
        FullName = Name.Create(fullName);
        EmailAddress = Email.Create(emailAddress);
        DateOfBirth = dateOfBirth;
    }

}