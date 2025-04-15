public readonly struct Money
{    
    public decimal Amount { get; }
    
    public Money(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException("Ammount can not be negative");

        Amount = amount;
    }

    public Money Add(Money other) => new Money(Amount + other.Amount);
    public Money Subtract(Money other) =>
                    (Amount >= other.Amount) 
                    ? new Money(Amount - other.Amount) 
                    : throw new InvalidOperationException("Insufficient funds");
                    
    public override string ToString() => $"${Amount:F2}";
}