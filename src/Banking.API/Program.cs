using System.Reflection;
using Banking.API.OptionsSetup;
using Banking.Application.Abstractions;
using Banking.Application.Abstractions.Caching;
using Banking.Application.Behaviours;
using Banking.Application.Commands;
using Banking.Application.EventHandlers;
using Banking.Application.Services;
using Banking.Domain.Repositories;
using Banking.Domain.Shared;
using Banking.Infrastructure.Authentication;
using Banking.Infrastructure.Caching;
using Banking.Infrastructure.Identity;
using Banking.Infrastructure.Persistence.Context;
using Banking.Infrastructure.Persistence.Repositories;
using Banking.Infrastructure.Persistence.Uow;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddDbContext<IdentityDbContext>(options =>
{
    var identityConnectionString = builder.Configuration.GetConnectionString("IdentityConnection");
    options.UseMySql(identityConnectionString, ServerVersion.AutoDetect(identityConnectionString));
});

// Identity Services (fix here)
builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<IdentityDbContext>();

// Add Services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
// Add Redis
builder.Services.AddStackExchangeRedisCache(redisOptions =>
{
    string connection = builder.Configuration
        .GetConnectionString("Redis")!;

    redisOptions.Configuration = connection;
});

// Add MassTransit
builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter(); //article-created-event

    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), h =>
        {
            h.Username(builder.Configuration["MessageBroker:Username"]!);
            h.Password(builder.Configuration["MessageBroker:Password"]!);
        });

        configurator.ConfigureEndpoints(context);
    });
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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Banking API",
        Version = "v1",
        Description = "Banking backend API",
    });
});


var app = builder.Build();

// Seed Roles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await RoleSeeder.SeedRolesAsync(services);
}


// Swagger & Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
