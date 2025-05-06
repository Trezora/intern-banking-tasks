using Banking.Domain.Entities;
using Banking.Domain.Exceptions;
using Banking.Domain.ValueObjects;

namespace Banking.Domain.Factories;

public class CustomerFactory
{
    private readonly HashSet<Email> _existingEmails = [];
    public Customer CreateCustomer(Name fullName, Email email, DateTime dateTime)
    {
        if (_existingEmails.Contains(email))
            throw new EmailAlreadyExistException(email);
        
        _existingEmails.Add(email);
        Customer newCustomer = new(new CustomerId(Guid.NewGuid()), fullName, email, dateTime);
        
        return newCustomer;
    }
}