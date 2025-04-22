
using System.Text.RegularExpressions;

namespace BankingApp.Domain.ValueObjects.AccountNumbers;

public sealed class AccountNumber : ValueObject
{   
    // private static readonly Regex AccountNumberFormat = new(@"^\d{3}-\d{4}-\d{5}$", RegexOptions.Compiled);
    
    public string Value { get; }

    private AccountNumber(string value)
    {
        Value = value;
    }

    public static AccountNumber Create()
    {
        return GenerateAccountNumber();
    }

    private static AccountNumber GenerateAccountNumber()
    {
        var rand = new Random();
        string formatted = $"{rand.Next(100, 999)}-{rand.Next(1000, 9999)}-{rand.Next(10000, 99999)}";
        return new AccountNumber(formatted);
    }

    public override string ToString() => Value;

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}