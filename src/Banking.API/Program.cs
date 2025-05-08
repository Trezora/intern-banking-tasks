using System.Reflection;
using Banking.Application.EventHandlers;
using Banking.Application.Services;
using Banking.Domain.Primitives;
using Banking.Domain.Repositories;
using Banking.Infrastructure.Persistence.Repositories;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddSingleton<ICustomerRespository, InMemoryCustomerRepository>();
builder.Services.AddSingleton<IBankAccountRepository, InMemoryBankAccountRepository>();

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    
    // Register specific handlers
    cfg.RegisterServicesFromAssembly(typeof(CustomerRegisteredEventHandler).Assembly);
    
    // Map domain events to MediatR notifications
    cfg.AddOpenBehavior(typeof(DomainEventToMediatorBridge<,>));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

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
