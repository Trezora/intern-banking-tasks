using Banking.Domain.Exceptions;

namespace Banking.Domain.ValueObjects;

public record Money
{
    public decimal Value { get; }

    public Money(decimal value)
    {
        if (value < 0)
            throw new NegativeMoneyAmountException(); 
        
        Value = value;
    }

    public static implicit operator decimal(Money money) 
        => money.Value;
    
    public static implicit operator Money(decimal value) 
        => new (value);

    public Money Add(Money other)
        => new(Value + other.Value);
    
    public Money Subtract(Money other)
        =>  new(Value - other.Value);
    

}