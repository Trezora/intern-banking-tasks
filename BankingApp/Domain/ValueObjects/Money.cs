
using BankingApp.Domain.Shared.OperationResults;

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
    
    public static OperationResult<Money> Add(Money first, Money second) =>
        (second.Value < 0)
        ? OperationResult<Money>.Failure(null, "Failure: Cannot add negative money.")
        : OperationResult<Money>.Success(new Money(first.Value + second.Value), "Success");



    public static OperationResult<Money> Subtract(Money first, Money second) =>
        (second.Value < 0)
        ? OperationResult<Money>.Failure(null, "Failure: Cannot subtract negative money.")
        : (first.Value < second.Value)
        ? OperationResult<Money>.Failure(null, "Failure: Insufficient funds.")
        : OperationResult<Money>.Success(new Money(first.Value - second.Value), "Success");


    public override string ToString() => $"${Value:N2}";

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}