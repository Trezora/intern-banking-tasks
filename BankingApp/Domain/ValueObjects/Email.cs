
using System.Text.RegularExpressions;

namespace BankingApp.Domain.ValueObjects.Emails;

public sealed class Email : ValueObject
{   
    private static readonly Regex EmailPattern = new(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled);

    public string Value { get; }

    private Email(string value) 
    {
        Value = value;
    }

    public override IEnumerable<object> GetAtomicValues()
    {   
        yield return NormalizedValue;
    }

    public override string ToString() => Value;

    public string NormalizedValue => Value.ToLowerInvariant();

    public static Email Create(string value)
    {   
        if (string.IsNullOrEmpty(value)) 
            throw new ArgumentNullException("Email is required.");
        if (!EmailPattern.IsMatch(value)) 
            throw new ArgumentException("Invalid email format.");

        return new Email(value);
    }
}