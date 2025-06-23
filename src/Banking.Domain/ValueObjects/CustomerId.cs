
using Banking.Domain.Exceptions;

namespace Banking.Domain.ValueObjects;

public sealed class CustomerId : ValueObject
{   
    public Guid Value { get; }

    public CustomerId(Guid value)
    {   
        if (value == Guid.Empty)
            throw new EmptyCustomerIdException();
            
        Value = value;
    }

    // Private constructor for EF Core
    private CustomerId()
    {
        // EF sets this via reflection
        Value = Guid.Empty;
    }

    public static implicit operator Guid(CustomerId customerId) => customerId.Value;

    public static implicit operator CustomerId(Guid value) => new(value);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}