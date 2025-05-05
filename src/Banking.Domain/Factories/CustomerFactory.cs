using Banking.Domain.Entities;
using Banking.Domain.Exceptions;

namespace Banking.Domain.Factories;

public class CustomerFactory
{
    private readonly HashSet<string> _existingEmails = new(StringComparer.OrdinalIgnoreCase);
    public Customer CreateCustomer(string fullName, string email, DateTime dateTime)
    {
        if (_existingEmails.Contains(email))
            throw new EmailAlreadyExistException(email);
        
        _existingEmails.Add(email);
        return new Customer(Guid.NewGuid(), fullName, email, dateTime);
    }
}