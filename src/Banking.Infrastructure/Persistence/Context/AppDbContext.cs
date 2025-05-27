using Microsoft.EntityFrameworkCore;
using Banking.Domain.Entities;
using Banking.Infrastructure.Persistence.Configurations;
using Banking.Domain.Primitives;

namespace Banking.Infrastructure.Persistence.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<BankAccount> BankAccounts => Set<BankAccount>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        modelBuilder.ApplyConfiguration(new BankAccountConfiguration());
    }

    public List<DomainEvent> GetDomainEvents()
    {
        return ChangeTracker
            .Entries<AggregateRoot>()
            .Where(e => e.Entity.DomainEvents != null && e.Entity.DomainEvents.Any())
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();
    }       

    public void ClearDomainEvents()
    {
        foreach (var entry in ChangeTracker.Entries<Customer>())
        {
            entry.Entity.ClearDomainEvents();
        }
    }

}
