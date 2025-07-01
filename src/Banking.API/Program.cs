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

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add detailed console logging
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
    builder.Logging.AddDebug();
    builder.Logging.SetMinimumLevel(LogLevel.Information);

    // Add DbContext with retry policy for Azure SQL Database
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        Console.WriteLine($"Main DB Connection String: {connectionString?.Substring(0, Math.Min(50, connectionString?.Length ?? 0))}...");
        
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
        Console.WriteLine($"Identity DB Connection String: {identityConnectionString?.Substring(0, Math.Min(50, identityConnectionString?.Length ?? 0))}...");
        
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

    // Add In-Memory Distributed Cache as fallback (free alternative to Redis)
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

    builder.Services.AddHealthChecks();

    var app = builder.Build();
    Console.WriteLine("Application built successfully");

    // Enable detailed errors for debugging
    app.UseDeveloperExceptionPage();

    // Get logger for detailed logging
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Application starting up...");

    // Database setup and migration (MOVED BEFORE MIDDLEWARE SETUP)
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        logger.LogInformation("Starting database setup...");
        
        try
        {
            // Test and migrate main database
            logger.LogInformation("Testing main database connection...");
            var appContext = services.GetRequiredService<AppDbContext>();
            var canConnectMain = await appContext.Database.CanConnectAsync();
            logger.LogInformation($"Main database can connect: {canConnectMain}");
            
            if (canConnectMain)
            {
                logger.LogInformation("Migrating main database...");
                await appContext.Database.MigrateAsync();
                logger.LogInformation("Main database migration completed");
            }

            // Test and migrate identity database
            logger.LogInformation("Testing identity database connection...");
            var identityContext = services.GetRequiredService<IdentityDbContext>();
            var canConnectIdentity = await identityContext.Database.CanConnectAsync();
            logger.LogInformation($"Identity database can connect: {canConnectIdentity}");
            
            if (canConnectIdentity)
            {
                logger.LogInformation("Migrating identity database...");
                await identityContext.Database.MigrateAsync();
                logger.LogInformation("Identity database migration completed");
            }

            // Seed roles
            logger.LogInformation("Seeding roles...");
            await RoleSeeder.SeedRolesAsync(services);
            logger.LogInformation("Role seeding completed");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Database setup failed: {Message}", ex.Message);
            Console.WriteLine($"=== DATABASE SETUP FAILED ===");
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine($"Type: {ex.GetType().Name}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            throw;
        }
    }

    // Swagger & Middleware setup
    logger.LogInformation("Setting up middleware...");
    
    // Enable Swagger in all environments for debugging
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Banking API v1");
        c.RoutePrefix = "swagger";
    });

    // Add a simple root endpoint
    app.MapGet("/", () => Results.Redirect("/swagger"));
    
    // Add debug endpoint
    app.MapGet("/debug", () => 
    {
        return Results.Ok(new { 
            Status = "App is running",
            Environment = app.Environment.EnvironmentName,
            Time = DateTime.UtcNow 
        });
    });

    // Health check endpoint
    app.UseHealthChecks("/health");

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    logger.LogInformation("Application configured successfully, starting...");
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"=== APPLICATION STARTUP FAILED ===");
    Console.WriteLine($"Error: {ex.Message}");
    Console.WriteLine($"Type: {ex.GetType().Name}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
    }
    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
    Console.WriteLine($"=================================");
    throw;
}