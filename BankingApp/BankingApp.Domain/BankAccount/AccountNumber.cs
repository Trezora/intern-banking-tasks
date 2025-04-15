using System.Text.RegularExpressions;

public readonly struct AccountNumber
{    
    private readonly string _value;
    
    public AccountNumber(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentNullException(nameof(value));

        // Here we need to check valid form of Account Number 
        // It must be only [0-9] and -  like ***-****-***** (for example)
        // TODO:
        if (!Regex.IsMatch(value, @"^\d{3}-\d{4}-\d{5}$"))
            throw new ArgumentException("Invalid Account Number Format");

    
        _value = value;
    }

    public static AccountNumber GenerateAccountNumber()
    {
        // Generates string according to this format: ***-****-***** 
        var rand = new Random();
        return new AccountNumber($"{rand.Next(100, 999)}-{rand.Next(1000, 9999)}-{rand.Next(10000, 99999)}");
    }

    public override string ToString() => _value;
}