
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

    public static Money Add(Money first, Money second) => new(first.Value + second.Value);

    public static Money Subtract(Money first, Money second) => 
                                        (first.Value < second.Value) 
                                        ? throw new InvalidOperationException("Insufficient funds.") 
                                        : new(first.Value - second.Value);

    public override string ToString() => $"${Value:N2}";

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}