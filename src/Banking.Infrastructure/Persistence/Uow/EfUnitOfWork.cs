using System.ComponentModel;
using Banking.Domain.Shared;
using Banking.Infrastructure.Persistence.Context;
using MediatR;

namespace Banking.Infrastructure.Persistence.Uow;

public class EfUnitOfWork : IUnitOfWork
{   
    private readonly AppDbContext _appDbContext;
    private readonly IMediator _mediator;

    public EfUnitOfWork(AppDbContext appDbContext, IMediator mediator)
    {
        _appDbContext = appDbContext;
        _mediator = mediator;
    }
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await _appDbContext.SaveChangesAsync(cancellationToken);

        var domainEvents = _appDbContext.GetDomainEvents();

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
            // we can publish integration event 
        }

        _appDbContext.ClearDomainEvents();

        return result;
    }
}