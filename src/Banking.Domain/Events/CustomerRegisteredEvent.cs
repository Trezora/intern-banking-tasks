using Banking.Domain.Primitives;

namespace Banking.Domain.Events;

public sealed record CustomerRegisteredEvent(Guid CustomerId) : IDomainEvent
{

}