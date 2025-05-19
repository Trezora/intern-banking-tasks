using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Banking.Domain.Entities;
using Banking.Domain.ValueObjects;

namespace Banking.Infrastructure.Persistence.Configurations;

public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        builder.ToTable("bank_accounts");

        builder.Ignore(b => b.Id);

        // Use AccountNumber as the primary key
        builder.HasKey(b => b.AccountNumber);

        builder.Property(b => b.AccountNumber)
            .HasColumnName("account_number")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(b => b.Balance)
            .HasColumnName("balance")
            .IsRequired();

        builder.Property(b => b.CustomerId)
            .HasColumnName("customer_id")
            .HasConversion(
                id => id.Value,
                value => new CustomerId(value))
            .IsRequired();

        builder.HasOne<Customer>()
            .WithMany(c => c.Accounts)
            .HasForeignKey(b => b.CustomerId)
            .HasPrincipalKey(c => c.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
