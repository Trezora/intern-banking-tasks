
namespace Banking.Domain.Primitives;

public abstract class AggregateRoot : Entity
{   
    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected AggregateRoot() : base()
    {
    }
    
    protected AggregateRoot(Guid id)
        : base(id)
    {
    }

    public void RaiseDomainEvent(DomainEvent @domainEvent)
    {
        _domainEvents.Add(@domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}