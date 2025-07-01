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
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext with retry policy for Azure SQL Database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: new[] { 40613, 40197, 40501, 49918, 49919, 49920, 4060, 40532, 40549, 40550, 40551, 40552, 40553 });
        
        sqlOptions.CommandTimeout(60);
    });
});

builder.Services.AddDbContext<IdentityDbContext>(options =>
{
    var identityConnectionString = builder.Configuration.GetConnectionString("IdentityConnection");
    options.UseSqlServer(identityConnectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: new[] { 40613, 40197, 40501, 49918, 49919, 49920, 4060, 40532, 40549, 40550, 40551, 40552, 40553 });
        
        sqlOptions.CommandTimeout(60);
    });
});

// Identity Services
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

// Add In-Memory Distributed Cache
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();

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

// Add Controllers and Swagger
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

builder.Services.AddHealthChecks();

var app = builder.Build();

// Enable detailed errors for debugging
app.UseDeveloperExceptionPage();

// Database setup and migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var appContext = services.GetRequiredService<AppDbContext>();
        var identityContext = services.GetRequiredService<IdentityDbContext>();
        
        await appContext.Database.MigrateAsync();
        await identityContext.Database.MigrateAsync();
        await RoleSeeder.SeedRolesAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Database setup failed: {Message}", ex.Message);
        throw;
    }
}

// Configure middleware pipeline
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();