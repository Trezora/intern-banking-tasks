using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;

namespace Banking.Domain.ValueObjects;

public abstract class ValueObject : IEquatable<ValueObject>
{   
    protected abstract IEnumerable<Object> GetAtomicValues();
    public bool Equals(ValueObject? other)
    {
        if (other == null || other.GetType() != GetType()) return false;

        var thisValues = GetAtomicValues().GetEnumerator();
        var otherValues = other.GetAtomicValues().GetEnumerator();

        while (thisValues.MoveNext() && otherValues.MoveNext())
        {
            if (thisValues.Current != null &&
                !thisValues.Current.Equals(otherValues.Current)) return false;
        }

        return !thisValues.MoveNext() && !otherValues.MoveNext();
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as ValueObject);
    }

    public override int GetHashCode()
    {
        return GetAtomicValues()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }

    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }


    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !(left == right);
    }

}