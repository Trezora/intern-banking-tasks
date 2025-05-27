using Banking.Domain.Primitives;
using Banking.Domain.ValueObjects;

namespace Banking.Domain.Events;

public record CustomerCreatedDomainEvent(Guid Id, CustomerId CustomerId) : DomainEvent(Id);