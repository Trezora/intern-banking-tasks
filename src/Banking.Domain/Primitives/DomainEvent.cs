using System.ComponentModel.DataAnnotations.Schema;
using MediatR;

namespace Banking.Domain.Primitives;

[NotMapped]
public record DomainEvent(Guid Id) : INotification;