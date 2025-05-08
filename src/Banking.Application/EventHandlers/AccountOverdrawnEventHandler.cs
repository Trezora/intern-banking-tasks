using Banking.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Banking.Application.EventHandlers;

public class AccountOverdrawnEventHandler : INotificationHandler<AccountOverdrawnEvent>
{   
    private readonly ILogger<AccountOverdrawnEvent> _logger;

    public AccountOverdrawnEventHandler(ILogger<AccountOverdrawnEvent> logger)
    {
        _logger = logger;
    }

    public Task Handle(AccountOverdrawnEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogWarning("Account overdrawn attempt for customer ID: {CustomerId}", notification.CustomerId);
        return Task.CompletedTask;
    }
}