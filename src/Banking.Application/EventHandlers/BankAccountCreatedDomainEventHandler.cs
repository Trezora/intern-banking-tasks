namespace Banking.Application.EventHandlers;

using System.Threading;
using System.Threading.Tasks;
using Banking.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

public class BankAccountCreatedDomainEventHandler : INotificationHandler<BankAccountCreatedDomainEvent>
{
    private readonly ILogger<BankAccountCreatedDomainEventHandler> _logger;

    public BankAccountCreatedDomainEventHandler(ILogger<BankAccountCreatedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(BankAccountCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain event handled: BankAccount for customer ID {CustomerId} with account number {AccountNumber}",
                               notification.CustomerId.Value,  notification.AccountNumber);

        // I can publish an integration event here using Rebus or another message bus
        return Task.CompletedTask;
    }
}
