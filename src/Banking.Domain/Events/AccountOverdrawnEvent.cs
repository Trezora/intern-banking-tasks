using Banking.Domain.Primitives;

namespace Banking.Domain.Events;

public sealed record AccountOverdrawnEvent(Guid CustomerId) : IDomainEvent
{
}