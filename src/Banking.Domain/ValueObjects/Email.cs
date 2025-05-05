using Banking.Domain.Exceptions;

namespace Banking.Domain.ValueObjects;

public record Email
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
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }    

    public static implicit operator string(Email email)
        => email.Value;
        
    public static implicit operator Email(string email)
        => new(email);
}