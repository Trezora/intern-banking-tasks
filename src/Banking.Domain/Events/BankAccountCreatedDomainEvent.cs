using Banking.Domain.Primitives;
using Banking.Domain.ValueObjects;

namespace Banking.Domain.Events;

public record BankAccountCreatedDomainEvent(Guid Id, CustomerId CustomerId, Guid AccountNumber) : DomainEvent(Id);