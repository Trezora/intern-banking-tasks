using System.Text.RegularExpressions;

namespace BankingApp.Domain.ValueObjects.Names;

public sealed class Name : ValueObject
{   
    private static readonly Regex FullNamePattern = new(@"^[A-Za-z]+\s[A-Za-z]+$", RegexOptions.Compiled);
    private const int DefaultLength = 15;
    public string Value { get; }

    private Name (string value)
    {
        Value = value;
    }

    public static Name Create(string value)
    {
        if (string.IsNullOrEmpty(value)) 
            throw new ArgumentNullException("Full name is required.");
        if (value.Length > DefaultLength) 
            throw new ArgumentOutOfRangeException($"Full name must not exceed {DefaultLength}.");
        if (!FullNamePattern.IsMatch(value)) 
            throw new ArgumentException("Full name must be in the format 'First Last'.");

        return new Name(value);
    }

    public override string ToString() => Value;

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
