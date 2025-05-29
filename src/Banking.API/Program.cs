using System.Reflection;
using Banking.Application.Abstractions.Caching;
using Banking.Application.Behaviours;
using Banking.Application.Commands;
using Banking.Application.EventHandlers;
using Banking.Application.Services;
using Banking.Domain.Repositories;
using Banking.Domain.Shared;
using Banking.Infrastructure.Caching;
using Banking.Infrastructure.Persistence.Context;
using Banking.Infrastructure.Persistence.Repositories;
using Banking.Infrastructure.Persistence.Uow;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Add services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICacheService, CacheService>();

// Add Redis
builder.Services.AddStackExchangeRedisCache(redisOptions =>
{
    string connection = builder.Configuration
        .GetConnectionString("Redis")!;

    redisOptions.Configuration = connection;
});

// Add Validators
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
builder.Services.AddValidatorsFromAssembly(typeof(CreateCustomerCommand).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(CreateBankAccountCommand).Assembly);

// Add Repositories
builder.Services.AddScoped<ICustomerRespository, CustomerRepository>();
builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();

// Add Unit of Work
builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();

// Add MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.RegisterServicesFromAssembly(typeof(CustomerCreatedDomainEventHandler).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(BankAccountCreatedDomainEventHandler).Assembly);
});

// Swagger & MVC
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Swagger & Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
