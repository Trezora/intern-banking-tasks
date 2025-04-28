using System.Runtime.Remoting;
using System.Threading.Tasks.Dataflow;

namespace BankingApp.Domain.Entities;

public abstract class Entity : IEquatable<Entity>
{   
    public Guid Id { get; private init; } // you can only set value once and it can be done only in this class
    
    protected Entity(Guid id)
    {
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        if (obj is not Entity entity) return false;

        return entity.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode() * 57; // multiply by some prime number
    }

    public bool Equals(Entity? other)
    {
        if (other is null) return false;
        if (other.GetType() != GetType()) return false;    

        return other.Id == Id;
    }

    public static bool operator ==(Entity? first, Entity? second)
    {
        return first is not null && second is not null && first.Equals(second);
    }

    public static bool operator !=(Entity? first, Entity? second)
    {
        return !(first == second);
    }
}