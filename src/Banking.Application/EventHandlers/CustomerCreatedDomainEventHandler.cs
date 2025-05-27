namespace Banking.Application.EventHandlers;

using Banking.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

public class CustomerCreatedDomainEventHandler : INotificationHandler<CustomerCreatedDomainEvent>
{
    private readonly ILogger<CustomerCreatedDomainEventHandler> _logger;

    public CustomerCreatedDomainEventHandler(ILogger<CustomerCreatedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(CustomerCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain event handled: CustomerCreated with ID {CustomerId}", notification.CustomerId.Value);

        // I can publish an integration event here using Rebus or another message bus
        return Task.CompletedTask;
    }
}
