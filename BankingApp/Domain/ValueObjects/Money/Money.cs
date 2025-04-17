
namespace BankingApp.Domain.ValueObjects.MoneyVO;

public sealed class Money : ValueObject
{   
    public decimal Value { get; }

    private Money(decimal value)
    {
        Value = value;
    }

    public static Money Create(decimal value)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);

        return new Money(value);
    }

    public static Money Add(Money initial, Money final) => new(initial.Value + final.Value);

    public static Money Subtract(Money initial, Money final) => new(initial.Value + final.Value);

    public override string ToString() => $"${Value:N2}";

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}