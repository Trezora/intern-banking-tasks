using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Banking.Domain.Entities;
using Banking.Domain.ValueObjects;

namespace Banking.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers");

        builder.Ignore(c => c.Id);

        // Use CustomerId as the primary key
        builder.HasKey(c => c.CustomerId);

        builder.Property(c => c.CustomerId)
            .HasColumnName("customer_id")
            .HasConversion(
                id => id.Value,
                value => new CustomerId(value))
            .IsRequired();

        builder.Property(c => c.FullName)
            .HasColumnName("full_name")
            .HasConversion(
                name => name.Value,
                value => new Name(value))
            .IsRequired();

        builder.Property(c => c.EmailAddress)
            .HasColumnName("email")
            .HasConversion(
                email => email.Value,
                value => new Email(value))
            .IsRequired();

        builder.Property(c => c.DateOfBirth)
            .HasColumnName("date_of_birth")
            .IsRequired();

        builder.HasMany(c => c.Accounts)
            .WithOne()
            .HasForeignKey(b => b.CustomerId)
            .HasPrincipalKey(c => c.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
