using Domain.Entities;
using Domain.Exceptions;

namespace Domain.Factories;

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