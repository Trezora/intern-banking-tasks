using System.Text.RegularExpressions;
using Banking.Domain.Exceptions;
using Banking.Domain.Primitives;

namespace Banking.Domain.ValueObjects;

public sealed class Email : ValueObject
{
    public string Value { get; }

    public Email(string value)
    {   
        if (string.IsNullOrWhiteSpace(value))
            throw new EmptyCustomerEmailException();
        if (!IsValidEmail(value))
            throw new InvalidEmailFormatException();
            
        Value = value;
    }

    private bool IsValidEmail(string email)
    {
        var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        return regex.IsMatch(email);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value.ToLowerInvariant();
    }

    public static implicit operator string(Email email)
        => email.Value;
        
    public static implicit operator Email(string email)
        => new(email);
}