using Banking.Domain.Primitives;
using MediatR;

namespace Banking.Application.Behaviors;

public class DomainEventToMediatorBridge<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IMediator _mediator;

    public DomainEventToMediatorBridge(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next();

        if (request is IDomainEvent domainEvent)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        return response;
    }
}