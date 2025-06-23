using Banking.Domain.Shared;
using Banking.Infrastructure.Identity;
using Banking.Infrastructure.Persistence.Context;
using MediatR;

namespace Banking.Infrastructure.Persistence.Uow;

public class EfUnitOfWork : IUnitOfWork
{   
    private readonly AppDbContext _appDbContext;
    private readonly IdentityDbContext _identityDbContext;
    private readonly IMediator _mediator;

    public EfUnitOfWork(
        AppDbContext appDbContext,
        IdentityDbContext identityDbContext,
        IMediator mediator)
    {
        _appDbContext = appDbContext;
        _identityDbContext = identityDbContext;
        _mediator = mediator;
    }
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var appResult = await _appDbContext.SaveChangesAsync(cancellationToken);

        var identityResult = await _identityDbContext.SaveChangesAsync(cancellationToken);

        var domainEvents = _appDbContext.GetDomainEvents();

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
            // we can publish integration event 
        }

        _appDbContext.ClearDomainEvents();

        return appResult + identityResult;
    }
}