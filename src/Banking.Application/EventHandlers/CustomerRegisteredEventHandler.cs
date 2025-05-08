using Banking.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Banking.Application.EventHandlers;

public class CustomerRegisteredEventHandler : INotificationHandler<CustomerRegisteredEvent>
{
    private readonly ILogger<CustomerRegisteredEvent> _logger;

    public CustomerRegisteredEventHandler(ILogger<CustomerRegisteredEvent> logger)
    {
        _logger = logger;
    }

    public Task Handle(CustomerRegisteredEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Customer registered with ID: {CustomerId}", notification.CustomerId);
        return Task.CompletedTask;
    }
}
