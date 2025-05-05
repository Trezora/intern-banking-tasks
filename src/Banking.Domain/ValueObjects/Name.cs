using Banking.Domain.Exceptions;

namespace Banking.Domain.ValueObjects;

public sealed class Name : ValueObject
{
    public string Value { get; }

    public Name(string value)
    {   
        if (string.IsNullOrWhiteSpace(value))
            throw new EmptyCustomerNameException();

        Value = value;
    }

    public static implicit operator string(Name name)
        => name.Value;

    public static implicit operator Name(string name)
        => new(name);

    protected override IEnumerable<object> GetAtomicValues()
    {
       yield return Value;
    }

    public override string ToString()
    {
        return $"{Value}";
    }
}